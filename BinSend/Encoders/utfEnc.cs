using System.IO;
using System.Text;
using System;
namespace BinSend
{
    public static class utfEnc
    {
        /// <summary>
        /// encodes a binary file to a valid UTF-8 string
        /// </summary>
        /// <param name="src">source bytes</param>
        /// <returns>UTF-8 string</returns>
        public static string encode(byte[] src)
        {
            MemoryStream MS = new MemoryStream();
            foreach (byte b in src)
            {
                if (b < 128)
                {
                    MS.WriteByte(b);
                }
                else
                {
                    MS.WriteByte((byte)(0xC0 | ((b & 0xC0) >> 6))); //leftmost 2 bits + Header
                    MS.WriteByte((byte)(0x80 | (b & 0x3F))); //rightmost 6 bits + cont value
                }
            }
            byte[] tmp = MS.ToArray();
            MS.Close();
            MS.Dispose();
            return Encoding.UTF8.GetString(tmp);
        }

        /// <summary>
        /// Decodes an encoded UTF-8 segment.
        /// </summary>
        /// <param name="src">Source string</param>
        /// <returns>source bytes</returns>
        public static byte[] decode(string src)
        {
            MemoryStream MS = new MemoryStream();
            byte[] source = Encoding.UTF8.GetBytes(src);
            for (int i = 0; i < source.Length; i++)
            {
                //checks for header
                if ((source[i] & 0xC0) == 0xC0)
                {
                    //grab 2 bits from header
                    //grab 6 continuation bits and put it into the left
                    byte b = (byte)(((source[i] & 0x03) << 6) | (source[i+1] & 0x3F));
                    MS.WriteByte(b);
                    i++;
                }
                else
                {
                    MS.WriteByte(source[i]);
                }
            }
            byte[] tmp = MS.ToArray();
            MS.Close();
            MS.Dispose();
            return tmp;
        }

#if DEBUG
        public static void test(int size)
        {
            //Allocate a 20 MB array
            byte[] b = new byte[size];

            //we fill the array in sequence, resulting in 50% encoding efficiency for an even distribution of bytes,
            //values over 127 cause a double byte in the result, thus expanding it, values below 128 are left as-is
            for (int i = 0; i < size; i++)
            {
                b[i] = (byte)(i % 256);
            }

            //encode and decode the array
            byte[] result = decode(encode(b));

            if (result.Length == size)
            {
                for (int j = 0; j < size; j++)
                {
                    if (b[j] != result[j])
                    {
                        throw new Exception(string.Format("byte at position {0} invalid", j));
                    }
                }
            }
            else
            {
                throw new Exception("Encoding error: some bytes are lost...");
            }
        }
#endif
    }
}
