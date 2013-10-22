using System;
using System.Collections.Generic;
using System.Threading;
using CookComputing.XmlRpc;
using System.Text;
using System.Security.Cryptography;

namespace BinSend
{
    public delegate void chunkSentHandler(int part,int maxParts,TimeSpan duration);
    public delegate void taskFinishedHandler();

    public enum EncodingFormat
    {
        Base64,
        //yEnc,
        Ascii85,
        Hex,
        //BitEnc,
        Raw,
        Unknown
    }

    public class Part
    {
        public byte[] Data
        { get; private set; }

        public string SHA1
        { get; private set; }

        public Part(byte[] data)
        {
            Data = data;
            SHA1Managed SM = new SHA1Managed();
            SHA1=ToString(SM.ComputeHash(data));
            SM.Clear();
        }

        private string ToString(byte[] p)
        {
            return ThreadedSender.Hex(p);
        }

        ~Part()
        {
            Data = null;
            SHA1 = null;
        }
    }

    public class ThreadedSender
    {
        private const string HEX = "0123456789ABCDEF";

        public event chunkSentHandler chunkSent;
        public event taskFinishedHandler taskFinished;

        public string HashList
        {
            get
            {
                string[] ss = new string[Parts.Count];
                for (int i = 0; i < Parts.Count; i++)
                {
                    ss[i] = Parts[i].SHA1;
                }
                return string.Join(",", ss);
            }
        }

        private Thread t;
        private List<Part> Parts;
        private string FromAddress;
        private string ToAddress;
        private string Subject;
        private string TextTemplate;
        private EncodingFormat EF;
        private string fName;

        /// <summary>
        /// Asynchronously sends messages in multiple parts
        /// </summary>
        /// <param name="From">From address</param>
        /// <param name="To">To address</param>
        /// <param name="Subj">Subject</param>
        /// <param name="Text">Text template: {0}=index {1}=NumberOfParts {2}=Base64part</param>
        /// <param name="data">byte data to send</param>
        /// <param name="chunkLength">Length of a chunk in bytes</param>
        public ThreadedSender(string FileName,EncodingFormat ef ,string From,string To,string Subj,string Text,byte[] data,int chunkLength)
        {
            FromAddress = From;
            ToAddress = To;
            Subject = Subj;
            TextTemplate = Text;
            fName = FileName;
            EF = ef;

            Parts = new List<Part>();
            for (int i = 0; i < data.Length; i += chunkLength)
            {
                byte[] b = new byte[chunkLength > data.Length - i ? data.Length - i : chunkLength];
                Array.Copy(data, i, b, 0, b.Length);
                Parts.Add(new Part((byte[])b.Clone()));
            }

            chunkSent += new chunkSentHandler(ThreadedSender_chunkSent);
            taskFinished += new taskFinishedHandler(ThreadedSender_taskFinished);
        }

        ~ThreadedSender()
        {
            FromAddress = ToAddress = Subject = TextTemplate = fName = null;
            EF = EncodingFormat.Hex;
            if (t != null)
            {
                try
                {
                    t.Abort();
                }
                catch
                {
                    //NOOP
                }
                t = null;
            }
            if (Parts != null)
            {
                Parts.Clear();
                Parts = null;
            }
        }

        private void ThreadedSender_taskFinished()
        {
            Parts.Clear();
        }

        private void ThreadedSender_chunkSent(int part, int maxParts,TimeSpan duration)
        {
            t = null;
            if (part < maxParts)
            {
                sendPart(part + 1);
            }
            else
            {
                taskFinished();
            }
        }

        private void sendPart(int index)
        {
            if (t == null)
            {
                t = new Thread(new ParameterizedThreadStart(t_send));
                t.IsBackground = true;
                t.Start(index);
            }
            else
            {
                throw new Exception("Already sending a part");
            }
        }

        private void t_send(object o)
        {
            int index = (int)o;
            BitAPI BA = (BitAPI)XmlRpcProxyGen.Create(typeof(BitAPI));
            BA.Url = string.Format("http://{0}/", QuickSettings.Get("API-ADDR"));
            BA.Headers.Add("Authorization", "Basic " + JsonConverter.B64enc(string.Format("{0}:{1}", QuickSettings.Get("API-NAME"), QuickSettings.Get("API-PASS"))));

            string encString=encode(Parts[index-1].Data,EF);
            string ackData = BA.sendMessage(ToAddress, FromAddress, JsonConverter.B64enc(Subject), JsonConverter.B64enc(
                string.Format(TextTemplate,
                /*0*/fName,
                /*1*/index,
                /*2*/Parts.Count,
                /*3*/EF.ToString(),
                /*4*/encString.Length,
                /*5*/HashList,
                /*6*/encString)));
            encString=null;
            string AMSG = BA.getStatus(ackData).ToLower().Trim();
            Console.Write("Waiting for POW...");

            DateTime StartTime = DateTime.Now;

            while (AMSG == "msgqueued" ||
            AMSG == "broadcastqueued" ||
            AMSG == "doingpubkeypow" ||
            AMSG == "awaitingpubkey" ||
            AMSG == "doingmsgpow" ||
            AMSG == "doingpow" ||
            AMSG == "findingPubkey")
            {
                Thread.Sleep(1000);
                AMSG = BA.getStatus(ackData).ToLower().Trim();
            }
            //Remove Message from outbox (GUI will not update but at least free up some space)
            BA.trashSentMessageByAckData(ackData);
            chunkSent(index, Parts.Count, DateTime.Now.Subtract(StartTime));
        }

        /// <summary>
        /// Encodes a byte array into a string
        /// </summary>
        /// <param name="data">source data</param>
        /// <param name="ef">encoding format</param>
        /// <returns>string representation of data</returns>
        public static string encode(byte[] data, EncodingFormat ef)
        {
            switch (ef)
            {
                case EncodingFormat.Ascii85:
                    return new Ascii85().Encode(data);
                case EncodingFormat.Base64:
                    return splitLine(Convert.ToBase64String(data),78);
                case EncodingFormat.Hex:
                    return Hex(data);
                //case EncodingFormat.yEnc:
                    //return Encoding.GetEncoding("IBM437").GetString(yEnc.yEnc.Encode(data));
                //case EncodingFormat.BitEnc:
                    //return bitEnc.encode(data);
                case EncodingFormat.Raw:
                    return Encoding.UTF8.GetString(data);
            }
            throw new NotImplementedException("Encoding algorithm not yet available or set to unknown");
        }

        /// <summary>
        /// Decodes a string using the specific format
        /// </summary>
        /// <param name="data">string data</param>
        /// <param name="ef">encoding format</param>
        /// <returns>byte data</returns>
        public static byte[] decode(string data, EncodingFormat ef)
        {
            switch (ef)
            {
                case EncodingFormat.Ascii85:
                    return new Ascii85().Decode(data);
                case EncodingFormat.Base64:
                    return Convert.FromBase64String(data);
                case EncodingFormat.Hex:
                    return Unhex(data);
                //case EncodingFormat.yEnc:
                    //return yEnc.yEnc.Decode(Encoding.GetEncoding("IBM437").GetBytes(data));
                //case EncodingFormat.BitEnc:
                    //return bitEnc.decode(data);
                case EncodingFormat.Raw:
                    return Encoding.UTF8.GetBytes(data);

            }
            throw new NotImplementedException("Encoding algorithm not yet available");
        }

        /// <summary>
        /// Converts a string hex representation to a byte array
        /// </summary>
        /// <param name="data">source data</param>
        /// <returns>byte array</returns>
        public static byte[] Unhex(string data)
        {
            byte[] retVal;

            //Remove everything non-hex
            string src = string.Empty;
            StringBuilder SB = new StringBuilder(data.Length);
            foreach (char c in data.ToUpper())
            {
                if (HEX.Contains(c.ToString()))
                {
                    SB.Append(c);
                }
            }
            src = SB.ToString().Trim();

            if (src.Length % 2 == 0)
            {
                retVal = new byte[src.Length / 2];
                for (int i = 0; i < src.Length; i += 2)
                {
                    retVal[i / 2] = byte.Parse(src.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
                return retVal;
            }
            throw new Exception("WTF?");
        }

        /// <summary>
        /// Adds linebreaks to an oversized line
        /// </summary>
        /// <param name="content">text string (without linebreaks)</param>
        /// <param name="partLength">number of chars per line</param>
        /// <returns>formatted lines</returns>
        public static string splitLine(string content, int partLength)
        {
            StringBuilder SB = new StringBuilder(content.Length + content.Length / partLength);
            for (int i = 0; i < content.Length; i++)
            {
                SB.Append(content[i]);
                if (i % partLength == 0 && i > 0)
                {
                    SB.Append('\n');
                }
            }
            return SB.ToString();
        }

        /// <summary>
        /// Converts a byte array to a text representation in hex
        /// </summary>
        /// <param name="data">Byte array</param>
        /// <returns></returns>
        public static string Hex(byte[] data)
        {
            StringBuilder SB = new StringBuilder(data.Length * 2 + data.Length / 78);

            for (int i = 0; i < data.Length; i++)
            {
                SB.Append(data[i].ToString("X2"));
                if (i % 78 == 0 && i > 0)
                {
                    SB.Append("\n");
                }
            }

            return SB.ToString();

        }

        /// <summary>
        /// Start sending the first part.
        /// </summary>
        public void send()
        {
            if (t == null)
            {
                sendPart(1);
            }
        }
    }
}
