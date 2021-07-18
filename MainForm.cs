using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace Z88ImportExport {
	public partial class MainForm : Form {

		private ImportExportProtocolHandler protocol;

		public MainForm() {
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			OpenSerialPort();
			ClearStatusBar();
		}

		private string SendToDevice = "";
		private string SendToDirectory = "";

		private void OpenSerialPort() {

			if (this.ConfirmPortIsNotBusy()) {
				if (!string.IsNullOrEmpty(Properties.Settings.Default.SerialPort)) {
					if (this.serialPort.IsOpen) this.serialPort.Close();
					this.serialPort.PortName = Properties.Settings.Default.SerialPort;
				}

				this.protocol = new ImportExportProtocolHandler(this.serialPort);
				this.protocol.FileReceived += Protocol_FileReceived;
				this.protocol.FileReceiving += Protocol_FileReceiving;
				this.protocol.FileSent += Protocol_FileSent;
				this.protocol.FileSending += Protocol_FileSending;
				this.protocol.IsBusyChanged += Protocol_IsBusyChanged;

				foreach (var port in SerialPort.GetPortNames()) {
					if (port == this.serialPort.PortName) {
						try {
							this.serialPort.Open();
						} catch (Exception ex) {
							MessageBox.Show(this, string.Format("Could not open serial port {0}: {1}", this.serialPort.PortName, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
		}

		private void Protocol_IsBusyChanged(object sender, EventArgs e) {
			Invoke(new MethodInvoker(() => {
				fileSendMenu.Enabled = protocol.IsReady;
				fileCancelMenu.Enabled = protocol.IsBusy;
				if (!protocol.IsBusy) {
					ClearStatusBar();
				}
			}));
		}

		private void ClearStatusBar() {
			mainStatusLabel.Text = this.protocol.IsReady ? "Ready" : "Inactive";
			statusProgressBar.Visible = false;
		}

		private void Protocol_FileSending(object sender, Z88FileEventArgs e) {
			Invoke(new MethodInvoker(() => {
				mainStatusLabel.Text = string.Format(@"Sending ""{0}"" ({1:N0} bytes)", e.File.Name, e.DataTransferred);
				if (e.DataTransferLength > 0) {
					statusProgressBar.Style = ProgressBarStyle.Continuous;
					if (statusProgressBar.Maximum != e.DataTransferLength) {
						statusProgressBar.Value = 0;
						statusProgressBar.Maximum = e.DataTransferLength;
					}
				} else {
					statusProgressBar.Style = ProgressBarStyle.Marquee;
				}
				statusProgressBar.Value = e.DataTransferred;
				statusProgressBar.Visible = true;
			}));
		}

		private void Protocol_FileSent(object sender, Z88FileEventArgs e) {
			Invoke(new MethodInvoker(() => {
				ClearStatusBar();
			}));
		}

		private void Protocol_FileReceiving(object sender, Z88FileEventArgs e) {
			Invoke(new MethodInvoker(() => {
				mainStatusLabel.Text = string.Format(@"Receiving ""{0}"" ({1:N0} bytes)", e.File.Name, e.DataTransferred);
				statusProgressBar.Style = ProgressBarStyle.Marquee;
				statusProgressBar.Visible = true;
			}));
		}

		private void Protocol_FileReceived(object sender, Z88FileEventArgs e) {
			var lvi = new ListViewItem {
				Text = e.File.Name,
				Tag = e.File,
				Selected = true,
			};
			lvi.SubItems.Add(string.Format(@"{0:N0} bytes", e.File.Data.Length));
			Invoke(new MethodInvoker(() => {
				receivedFilesList.Items.Add(lvi);
				ClearStatusBar();
			}));
		}

		private void fileSaveMenu_Click(object sender, EventArgs e) {
			SaveSelectedFiles();
		}

		private void SaveSelectedFiles() {
			var selectedItems = this.receivedFilesList.SelectedItems;
			if (selectedItems.Count == 1) {
				this.saveFileDialog.FileName = Path.GetFileName(((Z88File)selectedItems[0].Tag).Name);
				if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK) {
					var file = (Z88File)selectedItems[0].Tag;
					try {
						File.WriteAllBytes(this.saveFileDialog.FileName, file.Data);
					} catch (Exception ex) {
						MessageBox.Show(this, string.Format(@"Could not save file ""{0}"": {1}", this.saveFileDialog.FileName, ex.Message), file.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			} else if (selectedItems.Count > 0) {
				if (this.folderBrowserDialog.ShowDialog(this) == DialogResult.OK) {
					foreach (ListViewItem item in selectedItems) {

						var file = (Z88File)item.Tag;
						var targetFilename = Path.GetFileName(file.Name);
						foreach (var c in Path.GetInvalidFileNameChars()) {
							targetFilename = targetFilename.Replace(c, '_');
						}
						while (targetFilename.IndexOf("__") >= 0) targetFilename = targetFilename.Replace("__", "_");
						targetFilename = Path.Combine(this.folderBrowserDialog.SelectedPath, targetFilename);

						if (!File.Exists(targetFilename) || MessageBox.Show(this, string.Format(@"The file ""{0}"" already exists." + Environment.NewLine + "Would you like to overwrite it?", targetFilename), file.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
							try {
								File.WriteAllBytes(targetFilename, file.Data);
							} catch (Exception ex) {
								MessageBox.Show(string.Format(@"Could not save file ""{0}"": {1}", targetFilename, ex.Message));
							}
						}
					}
				}
			}
		}

		private void fileMenu_DropDownOpening(object sender, EventArgs e) {
			this.fileSendMenu.Enabled = this.protocol.IsReady;
			this.fileClearSelectedFilesMenu.Enabled = this.receivedFilesList.SelectedItems.Count > 0;
			this.fileSaveMenu.Enabled = this.receivedFilesList.SelectedItems.Count > 0;
		}

		private void fileSendMenu_Click(object sender, EventArgs e) {
			if (this.ConfirmPortIsReady()) {
				if (this.openFileDialog.ShowDialog(this) == DialogResult.OK && this.openFileDialog.FileNames.Length > 0) {
					SendFiles(this.openFileDialog.FileNames);
				}
			}
		}

		private void SendFiles(string[] filenames) {
			if (this.ConfirmPortIsReady()) {
				for (int i = 0; i < filenames.Length; ++i) {
					var filename = filenames[i];
					try {
						var file = Z88File.FromFile(filename);
						var directory = SendToDirectory;
						if (!string.IsNullOrEmpty(directory)) file.Name = directory + "/" + file.Name;
						var device = SendToDevice;
						if (!string.IsNullOrEmpty(device)) file.Name = device + "/" + file.Name;
						this.protocol.SendFile(file, filenames.Length > 1 && i < filenames.Length - 1);
					} catch (Exception ex) {
						switch (MessageBox.Show(this, string.Format(@"There was an error sending the file ""{0}"": {1}", filename, ex.Message), Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)) {
							case DialogResult.Abort:
								return;
							case DialogResult.Retry:
								--i;
								break;
							case DialogResult.Ignore:
								break;
						}
					}
				}
			}
		}

		private bool ConfirmPortIsNotBusy() {
			if (this.protocol != null && this.protocol.IsBusy) {
				MessageBox.Show(this, "The device is currently busy." + Environment.NewLine + "Please wait until the current transfer is complete before continuing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return false;
			} else {
				return true;
			}
		}

		private bool ConfirmPortIsReady() {
			if (!this.ConfirmPortIsNotBusy()) {
				return false;
			}
			if (this.protocol == null || !this.protocol.IsReady) {
				MessageBox.Show(this, "The device is not ready." + Environment.NewLine + "Please select a serial port from the options menu to continue.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return false;
			} else {
				return true;
			}
		}

		private void exitMenu_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void selectAllMenu_Click(object sender, EventArgs e) {
			foreach (ListViewItem lvi in this.receivedFilesList.Items) {
				lvi.Selected = true;
			}
		}

		private void selectNoneMenu_Click(object sender, EventArgs e) {
			foreach (ListViewItem lvi in this.receivedFilesList.Items) {
				lvi.Selected = false;
			}
		}

		private void invertSelectionMenu_Click(object sender, EventArgs e) {
			foreach (ListViewItem lvi in this.receivedFilesList.Items) {
				lvi.Selected ^= true;
			}
		}

		private void clearSelectedFilesMenu_Click(object sender, EventArgs e) {
			foreach (ListViewItem lvi in this.receivedFilesList.SelectedItems) {
				lvi.Remove();
			}
		}

		private void editMenu_DropDownOpening(object sender, EventArgs e) {
			bool hasReceivedFiles = this.receivedFilesList.SelectedItems.Count > 0;
			this.editSelectAllMenu.Enabled = hasReceivedFiles;
			this.editSelectNoneMenu.Enabled = hasReceivedFiles;
			this.editInvertSelectionMenu.Enabled = hasReceivedFiles;
		}

		private void optionsMenu_DropDownOpening(object sender, EventArgs e) {

			var serialPort = Properties.Settings.Default.SerialPort;

			optionsSerialPortMenu.DropDownItems.Clear();

			foreach (var port in SerialPort.GetPortNames()) {
				var item = new ToolStripMenuItem {
					Text = port,
					Tag = port,
					Checked = serialPort == port,
					Enabled = !this.protocol.IsBusy,
				};
				item.Click += optionsSerialPortMenuDropDownItem_Click;
				optionsSerialPortMenu.DropDownItems.Add(item);
			}


			var sendToDevice = SendToDevice;

			optionSendToDeviceMenu.DropDownItems.Clear();

			var directories = new List<string>();
			directories.Add("");
			for (int i = 0; i <= 3; i++) {
				directories.Add(string.Format(":RAM.{0}", i));
			}

			foreach (var device in directories) {
				var item = new ToolStripMenuItem {
					Text = string.IsNullOrEmpty(device) ? "(Default)" : device,
					Tag = device,
					Checked = sendToDevice == device,
					Enabled = this.protocol.IsReady,
				};
				item.Click += optionsSendToDeviceDropDownItem_Click;
				optionSendToDeviceMenu.DropDownItems.Add(item);
			}

			this.optionSendToDirectoryText.Text = SendToDirectory;
		}

		private void optionsSerialPortMenuDropDownItem_Click(object sender, EventArgs e) {
			if (this.ConfirmPortIsNotBusy()) {
				if (sender is ToolStripMenuItem item) {
					var port = item.Tag as string;
					Properties.Settings.Default.SerialPort = port;
					this.OpenSerialPort();
					this.ClearStatusBar();
					Properties.Settings.Default.Save();
				}
			}
		}

		private void optionsSendToDeviceDropDownItem_Click(object sender, EventArgs e) {
			if (this.ConfirmPortIsReady()) {
				if (sender is ToolStripMenuItem item) {
					var newDirectory = item.Tag as string;
					SendToDevice = newDirectory;
				}
			}
		}

		private void MainForm_Shown(object sender, EventArgs e) {
			this.ConfirmPortIsReady();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
			if (this.protocol.IsReady) {
				this.protocol.Cancel();
				this.serialPort.Close();
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.protocol.IsBusy && MessageBox.Show(this, "A file transfer is in progress." + Environment.NewLine + "Are you sure you want to exit?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No) {
				e.Cancel = true;
			}
		}

		private void CancelTransfer() {
			if (this.protocol.IsBusy && MessageBox.Show(this, "A file transfer is in progress." + Environment.NewLine + "Are you sure you want to cancel?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
				this.protocol.Cancel();
			}
		}

		private void fileCancelMenu_Click(object sender, EventArgs e) {
			CancelTransfer();
		}

		protected override bool IsInputKey(Keys keyData) {
			if (keyData == Keys.Escape) return true;
			return base.IsInputKey(keyData);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Escape:
					e.Handled = true;
					CancelTransfer();
					break;
			}
		}


		private void MainForm_DragDrop(object sender, DragEventArgs e) {
			string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (files != null && files.Length > 0) {
				SendFiles(files);
			}
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			} else {
				e.Effect = DragDropEffects.None;
			}
		}

		private void optionSendToDirectoryText_TextChanged(object sender, EventArgs e) {
			this.SendToDirectory = optionSendToDirectoryText.Text;
		}
	}
}
