using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace yEnc
{
    public static class yEnc
    {
        public static void Encode(Stream Source, Stream Dest)
        {
            byte[] iBuffer=new byte[1024];
            byte[] oBuffer = new byte[1024];
            int readed = 0;
            while ((readed = Source.Read(iBuffer, 0, iBuffer.Length)) > 0)
            {
                oBuffer = Encode(iBuffer,0,readed);
                Dest.Write(oBuffer,0,oBuffer.Length);
            }
        }

        public static byte[] Encode(Stream Source)
        {
            MemoryStream MS = new MemoryStream();
            Encode(Source, MS);
            byte[] b=MS.ToArray();
            MS.Close();
            MS.Dispose();
            return b;
        }

        public static byte[] Encode(byte[] Source)
        {
            return Encode(Source, 0, Source.Length);
        }

        public static byte[] Encode(byte[] Source, int Start, int Length)
        {
            MemoryStream MS = new MemoryStream();
            for (int i = 0; i < Length; i++)
            {
                byte nb = (byte)((Source[Start + i] + 42) % 256);
                switch (nb)
                {
                    case 0:
                    case 9:
                    case 10:
                    case 13:
                    case 32:
                    case 46:
                    case 61:
                        MS.Write(new byte[] { (byte)'=', (byte)((nb + 64) % 256) }, 0, 2);
                        break;
                    default:
                        MS.WriteByte(nb);
                        break;
                }
            }
            byte[] b = MS.ToArray();
            MS.Close();
            MS.Dispose();
            return b;
        }

        public static void Encode(byte[] Source, Stream Dest)
        {
            MemoryStream MS = new MemoryStream(Source, false);
            Encode(MS, Dest);
            MS.Close();
            MS.Dispose();
        }

        public static void Decode(Stream Source, Stream Dest)
        {
            byte[] iBuffer = new byte[1024];
            byte[] oBuffer = new byte[1024];
            int readed = 0;
            while ((readed = Source.Read(iBuffer, 0, iBuffer.Length)) > 0)
            {
                oBuffer = Decode(iBuffer, 0, readed);
                Dest.Write(oBuffer, 0, oBuffer.Length);
            }
        }

        public static byte[] Decode(Stream Source)
        {
            MemoryStream MS = new MemoryStream();
            Decode(Source, MS);
            byte[] b = MS.ToArray();
            MS.Close();
            MS.Dispose();
            return b;
        }

        public static byte[] Decode(byte[] Source)
        {
            return Decode(Source, 0, Source.Length);
        }

        public static byte[] Decode(byte[] Source,int Index,int Length)
        {
            MemoryStream MS = new MemoryStream();
            bool escape = false;
            for (int i = 0; i < Length; i++)
            {
                if (Source[Index + i] == 61)
                {
                    escape = true;
                }
                else if (Source[Index + i] != 13 && Source[Index + i] != 10)
                {
                    byte nb = (byte)((Source[Index + i] + (256 - 42)) % 256);
                    if (escape)
                    {
                        escape = false;
                        MS.WriteByte((byte)((nb + (256 - 64)) % 256));
                    }
                    else
                    {
                        MS.WriteByte(nb);
                    }
                }
            }
            byte[] b = MS.ToArray();
            MS.Close();
            MS.Dispose();
            return b;
        }

        public static void Decode(byte[] Source, Stream Dest)
        {
            MemoryStream MS = new MemoryStream(Source, false);
            Decode(MS, Dest);
            MS.Close();
            MS.Dispose();
        }
    }
}
