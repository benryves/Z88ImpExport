# Z88 Import/Export

## Introduction

This is a simple Windows program to transfer files to and from a Z88 computer over a serial connection.

It is reasonably unsophisticated but is designed to work with the Z88's built-in Imp-Export popdown so can be used with a Z88 that does not have any more sophisticated file transfer software installed on it.

## Serial cable

You will need an appropriately-wired serial cable to connect the Z88 computer and your modern PC together. In spite of the similarity of the Z88's DE-9 connector to a modern PC's DE-9 connector they are not compatible so do not connect them together with a simple straight-through cable.

You can assemble your own serial cable by connecting the following pins together, using a male connector for the Z88 end (to plug into its female socket) and a female connector for the PC end (to plug into its male socket).

| Z88<br/>DE-9M| | PC<br />DE-9F|
|-----|-|----|
|2 TxD|→|2 RxD|
|3 RxD|←|3 TxD|
|4 RTS|→|8 CTS|
|5 CTS|←|7 RTS|
|7 GND|—|5 GND|
|8 DCD|←|4 DTR|
|9 DTR|→|1, 6 DCD, DSR|

I recommend buying a serial cable with a male connector on one end, cutting the other end off, then soldering on your own female connector in place of the one you cut off. This is because most of the DE-9 connector shells you can buy to assemble your own cables with have plastic hooks to hold in the DE-9 connector and these will prevent the cable from being inserted snugly into the Z88's recessed port. You should also remove the thumbscrews from the male Z88 connector end of the cable - this can normally be done by unscrewing them whilst pulling them back out of the shell.

## Setting up the PC software

The first time you run the software it will be stuck in an inactive state and will prompt you to choose the serial port. You can do this by clicking Options→Serial Port and selecting your serial port from the list of names that appears.

Please note that the software will try to open the serial port immediately and will keep it open as long as it is running so make sure to close any other programs that may be using the serial port first.

The selected serial port will be saved in your user settings.

## Setting up the Z88

The Z88 needs to be configured with the default communication settings. You can verify these in the Panel popdown (`□S`):

* Transmit baud rate: 9600
* Receive baud rate: 9600
* Parity: None
* Xon/Xoff: Yes

You can also change the default device from this page (for example, if you want files to be sent to a RAM card in slot 1 by default then change the "default device" field to `:RAM.1`).

To transfer files to and from the Z88 you need to be running the Imp-Export popdown (`□X`). How you proceed from there depends on whether you want to transfer files to or from the device.

## Sending files to the Z88

To send a file to the Z88 you first need to make sure it is ready to receive files. This is most easily handled by using the batch receive option inside Imp-Export. Type `B` then press `Enter`.

On your PC, click File→Send Files then select the files you wish to send in the dialog box that appears. These files will be sent one by one and you can monitor the progress both on the PC and the Z88.

If you prefer to save the files on the Z88 with a different name to the ones on the PC then you can use the receive file option. Type Type R then press Enter. When prompted, enter a file name, press Enter and then send a single file from the PC. If you press Enter without entering a filename then it will use the name of the file on the PC automatically.

The files will usually be stored in the default device on the Z88. You can change the default device on the Z88 via the Panel popdown (`□S`). You can also explicitly specify a device from the PC using the Options→Send to Device dropdown menu.

## Receiving files from the Z88

You do not need to do anything on the PC to start receiving files from the Z88, it is constantly listening for batch transfers.

To send a file from the Z88, use the send file feature in Imp-Export. Type `S` then press `Enter`. When prompted, enter a filename or a wildcard. For example, to send all files in the current device/directory enter `*` as the filename.

The Z88 will start sending the file(s) and you can monitor the progress on the PC. When a complete file has been received it appears in the list of received files along with its size. You can then export these files by selecting at least one of them and clicking File→Save Selected Files As. If one file is selected then a single file save dialog will be shown allowing you to choose the directory and name of the file. If more than one file is selected then a directory browser is shown instead and the files will be exported into that directory.

## Why is it so slow?

Files are transferred pretty slowly. If you're the sort of person to stare at progress bars during slow file transfers you might have noticed that the amount of data transferred in bytes is considerably higher than the size of the file which surely can't be helping matters.

The overall baud rate is kept to 9600 as I found that the higher speeds of 19200 and 38400 were extremely unreliable.

The protocol used to transfer binary data is extremely inefficient. Data bytes in the range &20–&7E are transmitted directly but anything outside that range needs to be escaped and sent in the form `ESC` `'B'` `x` `x` where xx are two ASCII characters representing the byte in hexadecimal. For example, &A9 is sent as `ESC` `'B'` `'A'` `'9'`.

This method of escaping binary data inflates the data size enormously. If you had a 256 byte file that contained a single instance of every possible byte value then 95 of those (&20–&7E) would be transmitted a single byte but the other 161 of them (&00–&1F and (&7F–&FF)) would consume four bytes each, resulting in a total transfer size of 95\*1+161\*4=739 and making the data nearly three times its original size. 
