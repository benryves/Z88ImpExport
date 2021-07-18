namespace Z88ImportExport {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (this.protocol != null) {
				if (disposing) {
					this.protocol.FileReceived -= Protocol_FileReceived;
					this.protocol.FileReceiving -= Protocol_FileReceiving;
					this.protocol.FileSent -= Protocol_FileSent;
					this.protocol.FileSending -= Protocol_FileSending;
					this.protocol.IsBusyChanged -= Protocol_IsBusyChanged;
				}
				this.protocol.Dispose();
				this.protocol = null;
			}
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.serialPort = new System.IO.Ports.SerialPort(this.components);
			this.receivedFilesList = new System.Windows.Forms.ListView();
			this.filesNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.filesSizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fileSendMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fileCancelMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fileMenuSep0 = new System.Windows.Forms.ToolStripSeparator();
			this.fileSaveMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fileClearSelectedFilesMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.fileMenuSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.fileExitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editSelectAllMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editSelectNoneMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.editInvertSelectionMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsSerialPortMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.optionSendToDeviceMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.optionSendToDirectoryMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.optionSendToDirectoryText = new System.Windows.Forms.ToolStripTextBox();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.mainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolContainer = new System.Windows.Forms.ToolStripContainer();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.toolContainer.BottomToolStripPanel.SuspendLayout();
			this.toolContainer.ContentPanel.SuspendLayout();
			this.toolContainer.TopToolStripPanel.SuspendLayout();
			this.toolContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// serialPort
			// 
			this.serialPort.DtrEnable = true;
			this.serialPort.Handshake = System.IO.Ports.Handshake.RequestToSendXOnXOff;
			this.serialPort.PortName = "-";
			// 
			// receivedFilesList
			// 
			this.receivedFilesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.receivedFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.filesNameColumn,
            this.filesSizeColumn});
			this.receivedFilesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.receivedFilesList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.receivedFilesList.FullRowSelect = true;
			this.receivedFilesList.HideSelection = false;
			this.receivedFilesList.Location = new System.Drawing.Point(0, 0);
			this.receivedFilesList.Name = "receivedFilesList";
			this.receivedFilesList.Size = new System.Drawing.Size(701, 353);
			this.receivedFilesList.TabIndex = 0;
			this.receivedFilesList.UseCompatibleStateImageBehavior = false;
			this.receivedFilesList.View = System.Windows.Forms.View.Details;
			// 
			// filesNameColumn
			// 
			this.filesNameColumn.Text = "Name";
			this.filesNameColumn.Width = 543;
			// 
			// filesSizeColumn
			// 
			this.filesSizeColumn.Text = "Size";
			this.filesSizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.filesSizeColumn.Width = 127;
			// 
			// menuStrip
			// 
			this.menuStrip.AllowDrop = true;
			this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.optionsMenu});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(701, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileSendMenu,
            this.fileCancelMenu,
            this.fileMenuSep0,
            this.fileSaveMenu,
            this.fileClearSelectedFilesMenu,
            this.fileMenuSep1,
            this.fileExitMenu});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "&File";
			this.fileMenu.DropDownOpening += new System.EventHandler(this.fileMenu_DropDownOpening);
			// 
			// fileSendMenu
			// 
			this.fileSendMenu.Name = "fileSendMenu";
			this.fileSendMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.fileSendMenu.Size = new System.Drawing.Size(236, 22);
			this.fileSendMenu.Text = "&Send Files...";
			this.fileSendMenu.Click += new System.EventHandler(this.fileSendMenu_Click);
			// 
			// fileCancelMenu
			// 
			this.fileCancelMenu.Enabled = false;
			this.fileCancelMenu.Name = "fileCancelMenu";
			this.fileCancelMenu.Size = new System.Drawing.Size(236, 22);
			this.fileCancelMenu.Text = "Cancel &Transfer";
			this.fileCancelMenu.Click += new System.EventHandler(this.fileCancelMenu_Click);
			// 
			// fileMenuSep0
			// 
			this.fileMenuSep0.Name = "fileMenuSep0";
			this.fileMenuSep0.Size = new System.Drawing.Size(233, 6);
			// 
			// fileSaveMenu
			// 
			this.fileSaveMenu.Name = "fileSaveMenu";
			this.fileSaveMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.fileSaveMenu.Size = new System.Drawing.Size(236, 22);
			this.fileSaveMenu.Text = "Save Selected Files &As...";
			this.fileSaveMenu.Click += new System.EventHandler(this.fileSaveMenu_Click);
			// 
			// fileClearSelectedFilesMenu
			// 
			this.fileClearSelectedFilesMenu.Name = "fileClearSelectedFilesMenu";
			this.fileClearSelectedFilesMenu.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.fileClearSelectedFilesMenu.Size = new System.Drawing.Size(236, 22);
			this.fileClearSelectedFilesMenu.Text = "&Clear Selected Files";
			this.fileClearSelectedFilesMenu.Click += new System.EventHandler(this.clearSelectedFilesMenu_Click);
			// 
			// fileMenuSep1
			// 
			this.fileMenuSep1.Name = "fileMenuSep1";
			this.fileMenuSep1.Size = new System.Drawing.Size(233, 6);
			// 
			// fileExitMenu
			// 
			this.fileExitMenu.Name = "fileExitMenu";
			this.fileExitMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.fileExitMenu.Size = new System.Drawing.Size(236, 22);
			this.fileExitMenu.Text = "E&xit";
			this.fileExitMenu.Click += new System.EventHandler(this.exitMenu_Click);
			// 
			// editMenu
			// 
			this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editSelectAllMenu,
            this.editSelectNoneMenu,
            this.editInvertSelectionMenu});
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new System.Drawing.Size(39, 20);
			this.editMenu.Text = "&Edit";
			this.editMenu.DropDownOpening += new System.EventHandler(this.editMenu_DropDownOpening);
			// 
			// editSelectAllMenu
			// 
			this.editSelectAllMenu.Name = "editSelectAllMenu";
			this.editSelectAllMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.editSelectAllMenu.Size = new System.Drawing.Size(224, 22);
			this.editSelectAllMenu.Text = "Select &All";
			this.editSelectAllMenu.Click += new System.EventHandler(this.selectAllMenu_Click);
			// 
			// editSelectNoneMenu
			// 
			this.editSelectNoneMenu.Name = "editSelectNoneMenu";
			this.editSelectNoneMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.editSelectNoneMenu.Size = new System.Drawing.Size(224, 22);
			this.editSelectNoneMenu.Text = "Select &None";
			this.editSelectNoneMenu.Click += new System.EventHandler(this.selectNoneMenu_Click);
			// 
			// editInvertSelectionMenu
			// 
			this.editInvertSelectionMenu.Name = "editInvertSelectionMenu";
			this.editInvertSelectionMenu.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
			this.editInvertSelectionMenu.Size = new System.Drawing.Size(224, 22);
			this.editInvertSelectionMenu.Text = "&Invert Selection";
			this.editInvertSelectionMenu.Click += new System.EventHandler(this.invertSelectionMenu_Click);
			// 
			// optionsMenu
			// 
			this.optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsSerialPortMenu,
            this.optionSendToDeviceMenu,
            this.optionSendToDirectoryMenu});
			this.optionsMenu.Name = "optionsMenu";
			this.optionsMenu.Size = new System.Drawing.Size(61, 20);
			this.optionsMenu.Text = "&Options";
			this.optionsMenu.DropDownOpening += new System.EventHandler(this.optionsMenu_DropDownOpening);
			// 
			// optionsSerialPortMenu
			// 
			this.optionsSerialPortMenu.Name = "optionsSerialPortMenu";
			this.optionsSerialPortMenu.Size = new System.Drawing.Size(180, 22);
			this.optionsSerialPortMenu.Text = "Serial &Port";
			// 
			// optionSendToDeviceMenu
			// 
			this.optionSendToDeviceMenu.Name = "optionSendToDeviceMenu";
			this.optionSendToDeviceMenu.Size = new System.Drawing.Size(180, 22);
			this.optionSendToDeviceMenu.Text = "Send to De&vice";
			// 
			// optionSendToDirectoryMenu
			// 
			this.optionSendToDirectoryMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionSendToDirectoryText});
			this.optionSendToDirectoryMenu.Name = "optionSendToDirectoryMenu";
			this.optionSendToDirectoryMenu.Size = new System.Drawing.Size(180, 22);
			this.optionSendToDirectoryMenu.Text = "Send to &Directory";
			// 
			// optionSendToDirectoryText
			// 
			this.optionSendToDirectoryText.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.optionSendToDirectoryText.Name = "optionSendToDirectoryText";
			this.optionSendToDirectoryText.Size = new System.Drawing.Size(200, 23);
			this.optionSendToDirectoryText.TextChanged += new System.EventHandler(this.optionSendToDirectoryText_TextChanged);
			// 
			// statusStrip
			// 
			this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainStatusLabel,
            this.statusProgressBar});
			this.statusStrip.Location = new System.Drawing.Point(0, 0);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(701, 22);
			this.statusStrip.TabIndex = 2;
			// 
			// mainStatusLabel
			// 
			this.mainStatusLabel.Name = "mainStatusLabel";
			this.mainStatusLabel.Size = new System.Drawing.Size(686, 17);
			this.mainStatusLabel.Spring = true;
			this.mainStatusLabel.Text = "Ready";
			this.mainStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusProgressBar
			// 
			this.statusProgressBar.MarqueeAnimationSpeed = 50;
			this.statusProgressBar.Name = "statusProgressBar";
			this.statusProgressBar.Size = new System.Drawing.Size(150, 16);
			this.statusProgressBar.Visible = false;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "All files (*.*)|*.*";
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.Description = "Please select the folder where you would like to save the files.";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "All files (*.*)|*.*";
			this.openFileDialog.Multiselect = true;
			this.openFileDialog.ReadOnlyChecked = true;
			// 
			// toolContainer
			// 
			// 
			// toolContainer.BottomToolStripPanel
			// 
			this.toolContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
			// 
			// toolContainer.ContentPanel
			// 
			this.toolContainer.ContentPanel.Controls.Add(this.receivedFilesList);
			this.toolContainer.ContentPanel.Size = new System.Drawing.Size(701, 353);
			this.toolContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolContainer.Location = new System.Drawing.Point(0, 0);
			this.toolContainer.Name = "toolContainer";
			this.toolContainer.Size = new System.Drawing.Size(701, 399);
			this.toolContainer.TabIndex = 3;
			this.toolContainer.Text = "toolStripContainer1";
			// 
			// toolContainer.TopToolStripPanel
			// 
			this.toolContainer.TopToolStripPanel.Controls.Add(this.menuStrip);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(701, 399);
			this.Controls.Add(this.toolContainer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "Z88 Import/Export";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.toolContainer.BottomToolStripPanel.ResumeLayout(false);
			this.toolContainer.BottomToolStripPanel.PerformLayout();
			this.toolContainer.ContentPanel.ResumeLayout(false);
			this.toolContainer.TopToolStripPanel.ResumeLayout(false);
			this.toolContainer.TopToolStripPanel.PerformLayout();
			this.toolContainer.ResumeLayout(false);
			this.toolContainer.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.IO.Ports.SerialPort serialPort;
		private System.Windows.Forms.ListView receivedFilesList;
		private System.Windows.Forms.ColumnHeader filesNameColumn;
		private System.Windows.Forms.ColumnHeader filesSizeColumn;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileMenu;
		private System.Windows.Forms.ToolStripMenuItem fileSaveMenu;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.ToolStripMenuItem fileSendMenu;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripSeparator fileMenuSep1;
		private System.Windows.Forms.ToolStripMenuItem fileExitMenu;
		private System.Windows.Forms.ToolStripStatusLabel mainStatusLabel;
		private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
		private System.Windows.Forms.ToolStripMenuItem editMenu;
		private System.Windows.Forms.ToolStripMenuItem editSelectAllMenu;
		private System.Windows.Forms.ToolStripMenuItem editSelectNoneMenu;
		private System.Windows.Forms.ToolStripMenuItem editInvertSelectionMenu;
		private System.Windows.Forms.ToolStripSeparator fileMenuSep0;
		private System.Windows.Forms.ToolStripMenuItem fileClearSelectedFilesMenu;
		private System.Windows.Forms.ToolStripMenuItem optionsMenu;
		private System.Windows.Forms.ToolStripMenuItem optionsSerialPortMenu;
		private System.Windows.Forms.ToolStripMenuItem optionSendToDeviceMenu;
		private System.Windows.Forms.ToolStripMenuItem fileCancelMenu;
		private System.Windows.Forms.ToolStripContainer toolContainer;
		private System.Windows.Forms.ToolStripMenuItem optionSendToDirectoryMenu;
		private System.Windows.Forms.ToolStripTextBox optionSendToDirectoryText;
	}
}

