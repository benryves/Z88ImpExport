using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Z88ImportExport {
	struct Z88File {

		public string Name { get; set; }
		public byte[] Data { get; set; }

		public Z88File(string name, byte[] data) {
			this.Name = name;
			this.Data = data;
		}

		public Z88File(IEnumerable<byte> name, IEnumerable<byte> data) {
			this.Name = Encoding.ASCII.GetString(new List<byte>(name).ToArray());
			this.Data = new List<byte>(data).ToArray();
		}

		public static Z88File FromFile(string filename) {
			return new Z88File(Path.GetFileName(filename), File.ReadAllBytes(filename));
		}
	}

	class Z88FileEventArgs : EventArgs {

		public Z88File File { get; private set; }

		public int DataTransferred { get; private set; }
		public int DataTransferLength { get; private set; }

		public Z88FileEventArgs(Z88File file, int dataTransferred, int dataTransferLength) {
			this.File = file;
			this.DataTransferred = dataTransferred;
			this.DataTransferLength = dataTransferLength;
		}
		public Z88FileEventArgs(Z88File file) : this(file, file.Data.Length, file.Data.Length) {
		}

	}
}
