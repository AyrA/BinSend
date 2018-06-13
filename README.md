BinSend
=======

Send and decode binary attachments via bitmessage.

This application allows you to send and receive binary attachments and decode them.

## Features

* Encodings: Base64, Hex, Raw, Ascii85
* Split large files into parts
* Fool proof even for DML addresses (assuming invalid part 1 is deleted)
* Can find part chain even if multiple files with same names exist
* Verification of each part
* Templates for sending different content
* Address book integration
* Custom part size can be set to match the CPU speed

## Initial setup and first run

Put `BinSend.exe`, `CookComputing.XmlRpcV2.dll` and `Newtonsoft.Json.dll`
into the same directory and just launch the exe file.
Check out the "[Releases](https://github.com/AyrA/BinSend/releases)" section to get precompiled versions of these files.
During startup it will ask you for your API settings from the official PyBitmessage client.
You can find instructions on how to enable the API here: [API Reference](https://bitmessage.org/wiki/API)

Once you have set up the API, you are ready to go.

## Usage

Most windows have a "Help" button with help topics for the currently selected function.
Check out the help for usage instructions.

## Handling invalid parts

If you decide to send files from a shared address (DML),
everyone with the address can modify the file.
If you happen to run into this issue, delete all invalid "Part 1" segments from the bitmessage window
To prevent others from sending fake part-1 messages,
always send from an address only you own (not a DML address).

## Template Placeholders

You can put placeholders into the template.
Placeholders are numbers inside curly braces.
You don't need to use all numbers and can use numbers multiple times.
The numbers are as follows:

- 0: File name
- 1: Current part number
- 2: Total number of parts
- 3: Encoding
- 4: Length of encoded content
- 5: Hash List
- 6: Encoded content

## BINSEND Format

The BINSEND format can be used to automatically decode chunks into complete files.
To use, put this content into the message: #BEGIN#{BINSEND:CHUNK}#END#

The #END# marker is optional if no content follows the binsend chunk.
Any number of whitespace is allowed between the chunk and the markers.

Only the first occurrence of the Binsend placeholder is replaced with actual data.

The format is a flat JSON with this structure:

	{
		"SameOrigin": bool,
		"Name": string
		"Part": Int32,
		"List": [string, ...],
		"Encoding": Int32,
		"Content": string
	}

### Explanation of fields

#### SameOrigin

This indicates if all parts must come from the same address as the first part.
You usually want this enabled unless you send parts from multiple clients with different addresses
or if you use the BM-MRND "From" address.
Binsend uses SHA1 for file segment validation which will soon be no longer cryptographically secure.
Using SameOrigin adds an additional layer of protection.

#### Name

This is the file name.
It's only needed in the first part and ignored if present in others.

#### Part

This is the number of the part (starting from 1)

#### List
This is a list of SHA1 hashes.
It's only needed in the first part and ignored if present in others.
It's recommended to only include this in the first part especially for large number of hashes.

#### Encoding

This is an integer specifying the encoding. Supported by default are:

- Raw = 0
- Base64 = 1
- Ascii85 = 2
- Hex = 3

Negative numbers are reserved for user defined encodings

#### Content

This is the content, encoded using the specified Encoding

## Supported Encoding schemes

Different encodings fit different use cases. See below for an explanation

### Raw

- This will use the content "as-is".
- Uses: Sending pre-encoded content or UTF-8 text.
- Ratio: 1 input byte -> 1 output byte

### Base64

- Encodes your content as Base64.
- Widely used in other applications and protocols, easily available on all platforms.
- Uses: Sending binary content, data URL
- Ratio: 3 input bytes -> 4 output bytes

### Ascii85

- Similar to Base64 but with better ratio.
- Uses: Keeping size smaller than with Base64
- Ratio: 4 input bytes -> 5 output bytes

### Hex

- Writes the bytes as a hexadecimal string.
- Doubles the size of the data.
- Uses: Allowing the user to copy/paste into a hex editor directly
- Ratio: 1 input byte -> 2 output bytes

## Custom Encodings

Encodings with negative numbers are reserved for custom use.
Any unknown encoding is interpreted as "Raw" by this client.
You can assemble the file and then use any tool of your choice to decode the concatenated data.

## Large files

Bitmessage is not made to handle large files.
Before sening a file, check if a link to the file is not enough instead of attaching it.
The recommended segment size is 50-100 Kb.
You can speed up the POW process by reducing the TTL but this will remove the message sooner from the network.
