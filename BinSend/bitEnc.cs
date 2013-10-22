using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BinSend
{
    /// <summary>
    /// Provides a Bitmessage-safe encoding sheme
    /// </summary>
    public static class bitEnc
    {
        public static string encode(byte[] src)
        {
            MemoryStream MS = new MemoryStream();
            foreach (byte b in src)
            {
                //Only 0x0A - 0x0D are replaced by bitmessage.
                //We escape these chars (plus the escaping char)
                //with 0x7C. Giving us an approx overhead of only
                //2% - 3%
                switch (b)
                {
                    case 0x09:
                    case 0x0A:
                    case 0x0B:
                    case 0x0C:
                    case 0x0D:
                    case 0x7C:
                        MS.WriteByte(0x7C);
                        MS.WriteByte((byte)(b + 0x40));
                        break;
                    default:
                        MS.WriteByte(b);
                        break;
                }
            }
            byte[] result = (byte[])MS.ToArray().Clone();
            MS.Close();
            MS.Dispose();
            MS = null;
            return Encoding.GetEncoding("IBM437").GetString(result);
        }

        public static byte[] decode(string src)
        {
            //Original DOS codepage used,
            //since it is 8-bit (single byte) only
            byte[] source = Encoding.GetEncoding("IBM437").GetBytes(src);
            MemoryStream MS = new MemoryStream();
            for(int i=0;i<source.Length;i++)
            {
                if (source[i] == 0x7C)
                {
                    MS.WriteByte((byte)(source[++i] - 0x40));
                }
                else
                {
                    MS.WriteByte(source[i]);
                }
            }
            source = null;
            byte[] result = (byte[])MS.ToArray().Clone();
            MS.Close();
            MS.Dispose();
            MS = null;

            return result;
        }
    }
}
