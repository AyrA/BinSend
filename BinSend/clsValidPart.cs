using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BinSend
{
    public class ValidPart
    {
        public const string SEQ_BEGIN = "#BEGIN#";

        /// <summary>
        /// for user stored data
        /// </summary>
        public object Tag
        { get; set; }

        public string From
        { get; private set; }
        public string To
        { get; private set; }
        public string ID
        { get; private set; }

        public string[] Hashes
        { get; private set; }
        public byte[] data
        { get; private set; }
        public string FileName
        { get; private set; }
        public int Part
        { get; private set; }
        public int MaxParts
        { get; private set; }
        public int PartLength
        { get; private set; }
        public EncodingFormat Format
        { get; private set; }
        public string CurrentHash
        { get; private set; }
        public bool SameOrigin
        { get; private set; }
        public bool validHash
        {
            get
            {
                SHA1Managed SA1 = new SHA1Managed();
                return CurrentHash.ToUpper() == ThreadedSender.Hex(SA1.ComputeHash(data));
            }
        }

        public ValidPart(string source,string FromAddr,string ToAddr,string MessageID)
        {
            //Bitmessage objects for sameOrigin and message removal
            From = FromAddr;
            To = ToAddr;
            ID = MessageID;

            //Default values
            SameOrigin = true;
            FileName = "unknown.bin";
            Part = MaxParts = 0;
            Format = EncodingFormat.Base64;
            CurrentHash = null;
            Hashes = null;
            data = null;


            if (source.Contains(SEQ_BEGIN))
            {
                int temp = 0;
                bool ContentMode = false;
                string encData = string.Empty;
                source = source.Substring(source.IndexOf(SEQ_BEGIN) + SEQ_BEGIN.Length).Trim();
                foreach (string Line in source.Split('\n'))
                {
                    if (Line.Length > 0 && (ContentMode || !Line.StartsWith(";")))
                    {
                        if (ContentMode)
                        {
                            encData += Line.Trim();
                        }
                        else if (Line.Contains("="))
                        {
                            switch (Line.Split('=')[0].ToUpper())
                            {
                                case "NAME":
                                    FileName = Line.Split('=')[1].Trim();
                                    break;
                                case "SAMEORIGIN":
                                    SameOrigin = (Line.Split('=')[1].ToUpper().Trim() == "YES");
                                    break;
                                case "PART":
                                    if (Line.Split('=')[1].Split(';').Length == 2)
                                    {
                                        if (int.TryParse(Line.Split('=')[1].Split(';')[1].Trim(), out temp) && temp > 0)
                                        {
                                            MaxParts = temp;
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Part specification");
                                        }
                                        if (int.TryParse(Line.Split('=')[1].Split(';')[0].Trim(), out temp) && temp > 0 && temp <= MaxParts)
                                        {
                                            Part = temp;
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid Part specification");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid Part specification");
                                    }
                                    break;
                                case "FORMAT":
                                    switch (Line.Split('=')[1].ToUpper().Trim())
                                    {
                                        case "YENC":
                                            //Format = EncodingFormat.yEnc;
                                            break;
                                        case "BASE64":
                                            Format = EncodingFormat.Base64;
                                            break;
                                        case "ASCII85":
                                            Format = EncodingFormat.Ascii85;
                                            break;
                                        case "HEX":
                                            Format = EncodingFormat.Hex;
                                            break;
                                        case "BITENC":
                                            //Format = EncodingFormat.BitEnc;
                                            break;
                                        case "":
                                            throw new Exception("No format specified");
                                        default:
                                            //Format we do not know yet
                                            Format = EncodingFormat.Unknown;
                                            break;
                                    }
                                    break;
                                case "LENGTH":
                                    if (int.TryParse(Line.Split('=')[1].Trim(), out temp) && temp > 0)
                                    {
                                        PartLength = temp;
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid Length specification");
                                    }
                                    break;
                                case "HASHLIST":
                                    Hashes = Line.Split('=')[1].Trim().Split(',');
                                    for (temp = 0; temp < Hashes.Length; temp++)
                                    {
                                        Hashes[temp] = Hashes[temp].Trim();
                                    }
                                    break;
                                case "CONTENT":
                                    ContentMode = true;
                                    encData += Line.Split(new char[] { '=' }, 2)[1].Trim();
                                    break;
                                default:
                                    //Unsupported value. Ignoring
                                    break;
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Line");
                        }
                    }
                }

                if (MaxParts != Hashes.Length)
                {
                    throw new Exception("hash count does not matches");
                }
                else
                {
                    CurrentHash = Hashes[Part - 1];
                }
                data = ThreadedSender.decode(encData, Format);
            }
            else
            {
                throw new Exception("No chunked header found");
            }
        }
    }
}
