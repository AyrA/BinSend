using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BinSend
{
    public class SentFile
    {
        public string FileName
        { get; private set; }
        public string FirstHash
        { get; private set; }
        public string[] AllHashes
        { get; private set; }
        public string ID
        { get; set; }

        public SentFile(string Name, string Hash, string[] PartHashes,string MessageID)
        {
            FileName = Name;
            FirstHash = Hash;
            AllHashes = PartHashes;
            ID = MessageID;
        }
    }
}
