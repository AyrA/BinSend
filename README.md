BinSend
=======

Send and decode binary attachments via bitmessage.

This application allows you to send and receive binary attachments and decode them.

## Features
* Encodings: Base64, Hex, Raw, ASCII85
* Split large files into parts
* Fool proof even for DML addresses (assuming invalid part 1 is deleted)
* Can find part chain even if multiple files with same names exist
* Verifies each part.
* Templates for sending different content
* Address book integration
* Encrypted password storage
* Custom part size can be set to match the CPU

## Planned features
* Cancel button to abort a running transfer
* Resending of single parts
* Resume transfers at a later startup
* Better main window, the current one is a big fucking mess.
* Task list for pending tasks
* Add an image of a dragon somewhere, dragons are awesome
* Template for images
* Variable for file extension (to automate `image/{X}` mime type declarations)

## How to use
This chapter gives a brief introduction to the application

### Initial setup and first run
Put BinSend.exe, CookComputing.XmlRpcV2.dll and Newtonsoft.Json.dll
into the same directory and just launch the exe file.
During startup it will ask you for your API settings
from the official PyBitmessage client.
You can find instriuctions here: [API Reference](https://bitmessage.org/wiki/API)

Once you have set up the API, you are ready to go.

### Sending a file
To send a file, fill in the fields from top to bottom:

* From address. This is collected via the API once during startup. If you generate a new address, restart BinSend to use it. never use a DML address or others can spoof the first part.
* To address. This will propose addresses based on the address book. The text after the last space is only used for the address. The rest is assumed to be a label, so you are free to write `XYZ BM-Addr3s5...` into the field and it will detect the address once you leave it.
* Subject: All messages share the same subject. Do not only put the file name in it.
* Content: the big text field serves as content. You can either write one directly, or use the "Template" button to create and manage templates.
* Encoding: select an encoding you like. Base64 is almost always the best choice, except if your content is pre-encoded. Click the "?" Button for more informations about the various encoding types.
* Size: Specify the Size in KiloBytes per part. The step size is 100 KB, but you can also insert any valid value manually. Setting this to 0 will send everything in 1 part, this is useful for Images or HTML5 audio/video
* Click on "Send File" to choose a file and start sending it.

### Assembling a file
To assemble a file, click on the "Get Files" button in the main window.
The "part collector" window shows up (depending on your inbox in the client this may take a few seconds) and it displays all files it is able to decode. Click on the file you want and check the part table to the right. If all parts are green, assemble the file, otherwise double click on an entry to view the reason for it being marked as bad.

You can delete a file from the inbox once you do not need it any longer. You also can delete single parts.

**Note:** You can only assemble files, that are sent in a BinSend format. See below for minimum requirements.

#### Troubleshooting
If someone tries to sneak in an invalid first part, you can always delete it from the bitmessage client.
If you have an invalid part, double click it for more details. The issue can probably be resolved with resending. To prevent others from sending fake part-1 messages, always send from an address only you own.

### Templates
Click on the template button in the main window to bring up the template manager.

With the template manager you can modify templates as you wish. if you start up the application the first time, it will create a few default templates.

Double click on a template to insert it into the main window and close the manager.

### BinSend format
Binsend compatible messages are simple. The message can have any content, until a line with the text `#BEGIN#` is found, at which the parser kicks in. Do not put any other content at the end of the block.
He then expects the folowing Lines and values **in each part**:

```
SameOrigin=Yes
Name={0}
Part={1};{2}
Format={3}
Length={4}
HashList={5}
Content={6}
```
Description:

* **SameOrigin**: Can be **Yes** or **No**. Setting this to Yes, forces all parts to come from the same address. Should only be turned off, if you send parts from multiple machines with different addresses.
* **Name**: The file name.
* **Part**: Value must be in the format **X;Y**, where X is the current part and Y is the total number of parts.
* **Format**: This is the format, in which the content is encoded.
* **Length**: Size in bytes of the **decoded** part.
* **HashList**: List of SHA1 hashes. The number of hashes must match the number of parts and they must follow in the same order. Use a comma to seperate them. The hash is taken from the **decoded** content.
* **Content**: The actual content. Can be split over multiple lines.
* Comments: You can put comments in it. Put a `;` at the beginning of the line to mark it as comment. Comments do not work in the middle of a multi-line content field.

### Custom Encodings
If you come up with a custom encoding, please try it first with binary content, before asking me to include it in BinSend. It is very simple to mess up an UTF-8 message, this is also the reason, why BinEnc and yEnc are included in the code, but not enabled.

### Large files
Bitmessage is not intended to carry large files. Before sening a file, check if a link to the file is not enough instead of attaching it.
