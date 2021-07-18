using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace Z88ImportExport {

	class DataToTransfer {

		public byte[] Data { get; private set; }
		public int Offset { get; set; }
		public object Tag { get; set; }

		public DataToTransfer(byte[] data) {
			this.Data = data;
		}
	}

	class ImportExportProtocolHandler : IDisposable {

		private SerialPort serialPort;

		public bool IsBusy {
			get {
				if (this.CurrentReceiveMode != ReceiveMode.Idle || this.dataToTransfer.Count > 0) {
					return true;
				} else {
					return false;
				}
			}
		}

		public bool IsReady {
			get {
				return !this.IsBusy && this.serialPort.IsOpen;
			}
		}

		public ImportExportProtocolHandler(SerialPort serialPort) {
			this.serialPort = serialPort;
			this.serialPort.DataReceived += SerialPort_DataReceived;
		}

		// Handle escape sequences.
		bool Escaped = false;
		int BinaryCharactersPending = 0;
		byte BinaryCharacter = 0;

		// Handle receiving file names and file data.
		public enum ReceiveMode {
			Idle,
			FileName,
			FileData,
		};
		ReceiveMode CurrentReceiveMode;

		int receivedDataBytes = 0;
		List<byte> FileName = new List<byte>();
		List<byte> FileData = new List<byte>();

		private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
			if (sender is SerialPort sp) {
				while (sp.BytesToRead > 0) {
					int i = sp.ReadByte();
					if (i >= 0 && i <= 255) {

						++receivedDataBytes;


						if ((CurrentReceiveMode == ReceiveMode.FileData) && (((receivedDataBytes - FileName.Count - 4) % 32) == 0)) {
							this.OnFileReceiving(new Z88FileEventArgs(new Z88File(FileName, FileData), receivedDataBytes, receivedDataBytes));
						}

						byte b = (byte)i;
						if (b == 0x1B) {
							Escaped = true;
						} else if (Escaped) {
							Escaped = false;
							switch ((char)b) {
								case 'N':
									receivedDataBytes = 1;
									CurrentReceiveMode = ReceiveMode.FileName;
									FileName.Clear();
									this.OnIsBusyChanged(new EventArgs());
									break;
								case 'F':
									CurrentReceiveMode = ReceiveMode.FileData;
									FileData.Clear();
									this.OnIsBusyChanged(new EventArgs());
									break;
								case 'E': // End of file.
								case 'Z': // End of list of files.
									int totalReceivedDataBytes = receivedDataBytes;
									receivedDataBytes = 0;
									CurrentReceiveMode = ReceiveMode.Idle;
									this.OnIsBusyChanged(new EventArgs());
									if (FileName.Count > 0 && FileData.Count > 0) {
										this.OnFileReceived(new Z88FileEventArgs(new Z88File(FileName, FileData), totalReceivedDataBytes, totalReceivedDataBytes));
									}
									break;
								case 'B':
									BinaryCharactersPending = 2;
									break;
							}
						} else if (BinaryCharactersPending > 0) {
							BinaryCharacter <<= 4;
							BinaryCharacter |= (byte)(int.Parse(((char)b).ToString(), System.Globalization.NumberStyles.HexNumber));
							if (--BinaryCharactersPending == 0) {
								DataByteReceived(BinaryCharacter);
							}
						} else {
							DataByteReceived(b);
						}
					}
				}
			}
		}

		public void Cancel() {
			if (this.serialPort != null) {
				if (this.serialPort.IsOpen) {
					this.serialPort.DiscardOutBuffer();
					this.serialPort.DiscardInBuffer();
				}
			}
			this.CurrentReceiveMode = ReceiveMode.Idle;
			this.FileData.Clear();
			this.FileName.Clear();
			this.OnIsBusyChanged(new EventArgs());
		}

		private void DataByteReceived(byte b) {
			switch (CurrentReceiveMode) {
				case ReceiveMode.FileName:
					FileName.Add(b);
					break;
				case ReceiveMode.FileData:
					FileData.Add(b);
					break;
			}
		}

		/// <summary>
		/// An event that is raised when a file is completely received.
		/// </summary>
		public event EventHandler<Z88FileEventArgs> FileReceived;

		protected virtual void OnFileReceived(Z88FileEventArgs e) {
			FileReceived?.Invoke(this, e);
		}

		/// <summary>
		/// An event that is raised when a file is partially received.
		/// </summary>
		public event EventHandler<Z88FileEventArgs> FileReceiving;

		protected virtual void OnFileReceiving(Z88FileEventArgs e) {
			FileReceiving?.Invoke(this, e);
		}

		/// <summary>
		/// An event that is raised when a file is completely sent.
		/// </summary>
		public event EventHandler<Z88FileEventArgs> FileSent;

		protected virtual void OnFileSent(Z88FileEventArgs e) {
			FileSent?.Invoke(this, e);
		}

		/// <summary>
		/// An event that is raised when a file is partially sent.
		/// </summary>
		public event EventHandler<Z88FileEventArgs> FileSending;

		protected virtual void OnFileSending(Z88FileEventArgs e) {
			FileSending?.Invoke(this, e);
		}

		public void SendFile(Z88File file, bool moreFilesToTollow = false) {

			var data = new List<byte>(file.Name.Length + file.Data.Length);

			// Name
			data.Add(0x1B);
			data.AddRange(Encoding.ASCII.GetBytes("N" + file.Name));

			// Data

			data.Add(0x1B);
			data.AddRange(Encoding.ASCII.GetBytes("F"));

			foreach (var b in file.Data) {
				if (b >= 0x20 && b <= 0x7E) {
					data.Add(b);
				} else {
					data.Add(0x1B);
					data.AddRange(Encoding.ASCII.GetBytes(string.Format("B{0:X2}", b)));
				}
			}

			data.Add(0x1B);
			data.AddRange(Encoding.ASCII.GetBytes(moreFilesToTollow ? "E" : "Z"));

			this.dataToTransfer.Enqueue(new DataToTransfer(data.ToArray()) { Tag = file });
			ProcessDataToTransferQueue();

			this.OnFileSending(new Z88FileEventArgs(file, 0, 0));
		}

		public Queue<DataToTransfer> dataToTransfer = new Queue<DataToTransfer>();

		bool writePending = false;

		private void ProcessDataToTransferQueue() {
			if (dataToTransfer.Count > 0) {
				if (writePending) {
					return;
				} else {
					writePending = true;
				}
				var itemToWrite = dataToTransfer.Peek();
				var offset = itemToWrite.Offset;
				var dataCount = Math.Min(64, itemToWrite.Data.Length - offset);
				itemToWrite.Offset += dataCount;
				this.serialPort.BaseStream.BeginWrite(itemToWrite.Data, offset, dataCount, DataSent, itemToWrite);
				this.OnIsBusyChanged(new EventArgs());
			}
		}

		private void DataSent(IAsyncResult ar) {

			try {
				this.serialPort.BaseStream.EndWrite(ar);
			} catch (IOException) {
				dataToTransfer.Clear();
				this.OnIsBusyChanged(new EventArgs());
			}

			writePending = false;

			if (dataToTransfer.Count > 0) {

				var itemToWrite = dataToTransfer.Peek();
				var file = (Z88File)itemToWrite.Tag;

				if (itemToWrite.Offset >= itemToWrite.Data.Length - 1) {
					dataToTransfer.Dequeue();
					// File sent successfully!
					this.OnFileSent(new Z88FileEventArgs(file));
				} else {
					// More to go.
					this.OnFileSending(new Z88FileEventArgs(file, itemToWrite.Offset, itemToWrite.Data.Length));
				}

				if (dataToTransfer.Count > 0) {
					ProcessDataToTransferQueue();
				} else {
					this.OnIsBusyChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// An event that is raised when the busy state changed.
		/// </summary>
		public event EventHandler IsBusyChanged;

		protected virtual void OnIsBusyChanged(EventArgs e) {
			IsBusyChanged?.Invoke(this, e);
		}

		#region IDisposable Support

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {

			this.Cancel();

			if (!disposedValue) {

				if (this.serialPort != null) {
					this.serialPort.Dispose();
					this.serialPort = null;
				}

				disposedValue = true;
			}
		}


		~ImportExportProtocolHandler() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
		}
		#endregion

	}
}
