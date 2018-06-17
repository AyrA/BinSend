using CookComputing.XmlRpc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BinSend
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Tools
    {
        [DllImport("kernel32.dll")]
        private static extern void OutputDebugStringW([MarshalAs(UnmanagedType.LPWStr)]string Message);

        /// <summary>
        /// Default charset for String generator
        /// </summary>
        public const string CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-_";
        /// <summary>
        /// Placeholder for a single random BM address
        /// </summary>
        public const string BM_SRND = "BM-SRND";
        /// <summary>
        /// Placeholder for multiple random source addresses
        /// </summary>
        public const string BM_MRND = "BM-MRND";
        /// <summary>
        /// Binsend chunk placeholder
        /// </summary>
        public const string BINSEND_CHUNK = "{BINSEND:CHUNK}";
        /// <summary>
        /// Binsend Start marker
        /// </summary>
        public const string BINSEND_START = "#BEGIN#";
        /// <summary>
        /// Binsend End marker
        /// </summary>
        public const string BINSEND_END = "#END#";
        /// <summary>
        /// Regular expression to match a binsend chunk declaration in a template string
        /// </summary>
        public const string BINSEND_REGEX = @"#BEGIN#\s*{BINSEND:CHUNK}\s*(?:#END#\s*|$)";


        /// <summary>
        /// Value designating to use all bytes
        /// </summary>
        public const int DATA_EVERYTHING = -1;
        /// <summary>
        /// Regular expression for basic BM Addr format detection
        /// </summary>
        public const string BM_REGEX = @"(BM-[\w]{30,35})";
        /// <summary>
        /// Hex format string
        /// </summary>
        public const string FORMAT_HEX = "X2";
        /// <summary>
        /// Factor for sizes.
        /// </summary>
        /// <remarks>Kilo implies factor 1000 but 1024 is more common</remarks>
        public const int SIZEFACTOR = 1024;
        /// <summary>
        /// Factor to get seconds from the user defined TTL
        /// </summary>
        public const int TIMEFACTOR = 3600;

        /// <summary>
        /// Extracts a fragment from a Binsend formatted message body
        /// </summary>
        /// <param name="MessageBody">Message body</param>
        /// <returns>Fragment, null if not found</returns>
        public static Fragment GetFragment(string MessageBody)
        {
            if (!string.IsNullOrEmpty(MessageBody) && MessageBody.Contains(BINSEND_START))
            {
                //Find start segment
                var Start = MessageBody.IndexOf(BINSEND_START) + BINSEND_START.Length;
                var End = MessageBody.IndexOf(BINSEND_END, Start);
                //End is optional
                if (End < 0)
                {
                    End = MessageBody.Length;
                }
                //Try to decode the fragment
                return MessageBody.Substring(Start, End - Start).Trim().FromJson<Fragment>();
            }
            return null;
        }

        /// <summary>
        /// Processes a fragment and inserts values into a body template
        /// </summary>
        /// <param name="Body">Body template</param>
        /// <param name="F">Fragment</param>
        /// <param name="FileName">File name (no path)</param>
        /// <param name="PartNumber">Part Number (starting at 1)</param>
        /// <param name="TotalParts">Total number of parts</param>
        /// <param name="HashList">List of Hashes</param>
        /// <returns>processed template body</returns>
        public static string ProcessFragmentBody(string Body, Fragment F, string FileName, int PartNumber, int TotalParts, string[] HashList)
        {
            //Must be in the order the template specifies placeholders
            var data = new string[] {
                FileName,
                PartNumber.ToString(),
                TotalParts.ToString(),
                F.Encoding.ToString(),
                F.Content.Length.ToString(),
                string.Join(",",HashList),
                F.Content
            };

            for (var i = 0; i < data.Length; i++)
            {
                Body = Body.Replace("{" + i.ToString() + "}", data[i]);
            }

            var R = new Regex(BINSEND_REGEX);

            if (R.IsMatch(Body))
            {
                //Inserts the binsend fragment
                var M = R.Match(Body);
                Body =
                    Body.Substring(0, M.Index) +
                    ("#BEGIN#" + F.ToJson(true, true) + "#END#") +
                    Body.Substring(M.Index + M.Length);
            }
            return Body;
        }

        /// <summary>
        /// Logs a message to the windows debug log and attached debuggers
        /// </summary>
        /// <param name="Message">Message</param>
        public static void Log(string Message)
        {
#if DEBUG
            OutputDebugStringW(Message);
            System.Diagnostics.Debug.WriteLine(Message);
#endif
        }

        /// <summary>
        /// Checks if the given text contains a valid sender address
        /// </summary>
        /// <param name="Input">Text</param>
        /// <returns>true if valid sender</returns>
        public static bool IsValidFromAddr(string Input)
        {
            return IsMRND(Input) || IsSRND(Input) || !string.IsNullOrEmpty(GetBmAddr(Input));
        }

        /// <summary>
        /// Checks if the given input defines a single random address
        /// </summary>
        /// <param name="Input">Address</param>
        /// <returns>true if SRND address</returns>
        public static bool IsSRND(string Input)
        {
            return Input != null && Regex.IsMatch(Input, $"(<|^){BM_SRND}(>|$)");
        }

        /// <summary>
        /// Checks if the given input defines multiple random addresses
        /// </summary>
        /// <param name="Input">Address</param>
        /// <returns>true if MRND address</returns>
        public static bool IsMRND(string Input)
        {
            return Input != null && Regex.IsMatch(Input, $"(<|^){BM_MRND}(>|$)");
        }

        /// <summary>
        /// Tries to extract the first Bitmessage address found in a string
        /// </summary>
        /// <param name="Input">String with possible BM address in it</param>
        /// <returns>BM Addr (or null if not found)</returns>
        public static string GetBmAddr(string Input)
        {
            if (!string.IsNullOrEmpty(Input))
            {
                var R = new Regex(BM_REGEX);
                if (R.IsMatch(Input))
                {
                    return R.Match(Input).Groups[1].Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Computes the SHA1 hash of this byte array
        /// </summary>
        /// <param name="Data">Binary</param>
        /// <param name="Start">Start</param>
        /// <param name="Count">Number of bytes to parse</param>
        /// <returns>SHA1</returns>
        public static string SHA1(this byte[] Data, int Start = 0, int Count = DATA_EVERYTHING)
        {
            if (Data == null)
            {
                return null;
            }
            if (Count == DATA_EVERYTHING)
            {
                Count = Data.Length - Start;
            }
            using (var Hasher = new SHA1Managed())
            {
                return Hasher.ComputeHash(Data, Start, Count).HEX();
            }
        }

        /// <summary>
        /// Computes the SHA1 hash of this string
        /// </summary>
        /// <param name="Data">String</param>
        /// <returns>SHA1</returns>
        public static string SHA1(this string Data)
        {
            return Data.UTF().SHA1();
        }

        /// <summary>
        /// Decodes this Ascii85 Encoded string into raw bytes
        /// </summary>
        /// <param name="Data">A85 String</param>
        /// <returns>Bytes</returns>
        public static byte[] A85(this string Data)
        {
            var A = new Ascii85();
            return A.Decode(Data);
        }

        /// <summary>
        /// Encodes this byte array into an A85 string
        /// </summary>
        /// <param name="Data">Binary</param>
        /// <param name="Start">Start</param>
        /// <param name="Count">Number of bytes to parse</param>
        /// <returns>Byte array</returns>
        public static string A85(this byte[] Data, int Start = 0, int Count = DATA_EVERYTHING)
        {
            if (Count == DATA_EVERYTHING)
            {
                Count = Data.Length - Start;
            }
            var A = new Ascii85();
            return A.Encode(Data.Skip(Start).Take(Count).ToArray());
        }

        /// <summary>
        /// Converts this byte array to hex string
        /// </summary>
        /// <param name="Data">Binary</param>
        /// <param name="Start">Start</param>
        /// <param name="Count">Number of bytes to parse</param>
        /// <returns>Hex string</returns>
        public static string HEX(this byte[] Data, int Start = 0, int Count = DATA_EVERYTHING)
        {
            if (Count == DATA_EVERYTHING)
            {
                Count = Data.Length - Start;
            }
            return string.Concat(Data.Skip(Start).Take(Count).Select(m => m.ToString(FORMAT_HEX)));
        }

        /// <summary>
        /// Converts this hexadecimal string into a byte array
        /// </summary>
        /// <param name="Data">Hexadecimal string</param>
        /// <returns>Byte array</returns>
        public static byte[] HEX(this string Data)
        {
            if (Data != null && Data.Length % 2 != 1)
            {
                byte[] Ret = new byte[Data.Length / 2];
                for (var i = 0; i < Ret.Length; i++)
                {
                    Ret[i] = byte.Parse(Data.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
                return Ret;
            }
            return null;
        }

        public static void ShowHelp(string Text)
        {
            var F = Application.OpenForms.OfType<frmHelp>().FirstOrDefault();
            if (F == null)
            {
                F = new frmHelp();
            }
            F.Show();
            F.BringToFront();
            F.Focus();
            F.SetHelp(Text);
        }

        public static void CloseHelp()
        {
            ShowHelp(null);
        }

        /// <summary>
        /// Handles the Prompt Dialog
        /// </summary>
        /// <param name="Prompt">Dialog Prompt</param>
        /// <param name="Title">Dialog Title</param>
        /// <param name="Default">Default value to prefill</param>
        /// <returns>User input (null on cancel)</returns>
        public static string Prompt(string Prompt, string Title, string Default)
        {
            using (var F = new frmPrompt(Prompt, Title, Default))
            {
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return F.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the Bitmessage RPC component with the given Configuration vbalues
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static BitmessageRPC GetRPC(ApiConfig C)
        {
            var RPC = (BitmessageRPC)XmlRpcProxyGen.Create(typeof(BitmessageRPC));
            RPC.Url = string.Format("http://{0}:{1}/", C.IpOrHostname, C.Port);
            RPC.Headers.Add("Authorization", "Basic " + string.Format("{0}:{1}", C.Username, C.Password).UTF().B64());
            return RPC;
        }

        /// <summary>
        /// Converts UTF8 Text to bytes
        /// </summary>
        /// <param name="Data">Text</param>
        /// <returns>Binary</returns>
        public static byte[] UTF(this string Data)
        {
            return Encoding.UTF8.GetBytes(Data);
        }

        /// <summary>
        /// Converts binary Data to 
        /// </summary>
        /// <param name="Data">Binary</param>
        /// <param name="Start">Start</param>
        /// <param name="Count">Number of bytes to parse</param>
        /// <returns>String</returns>
        public static string UTF(this byte[] Data, int Start = 0, int Count = DATA_EVERYTHING)
        {
            if (Count == DATA_EVERYTHING)
            {
                Count = Data.Length - Start;
            }
            return Encoding.UTF8.GetString(Data, Start, Count);
        }

        /// <summary>
        /// Converts binary data to Base64
        /// </summary>
        /// <param name="Data">Binary</param>
        /// <param name="Start">Start</param>
        /// <param name="Count">Number of bytes to parse</param>
        /// <returns>Base64 String</returns>
        public static string B64(this byte[] Data, int Start = 0, int Count = DATA_EVERYTHING)
        {
            if (Count == DATA_EVERYTHING)
            {
                Count = Data.Length - Start;
            }
            return Convert.ToBase64String(Data, Start, Count);
        }

        /// <summary>
        /// Converts a Base64 string to Binary
        /// </summary>
        /// <param name="Data">Base64 Data</param>
        /// <returns>Binary</returns>
        public static byte[] B64(this string Data)
        {
            return Convert.FromBase64String(Data.Replace('\0', '\t').Trim());
        }

        /// <summary>
        /// Converts the JSON String into an Object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="S">JSON string</param>
        /// <param name="Default">Default value on error</param>
        /// <returns>Object or default</returns>
        public static T FromJson<T>(this string S, T Default = default(T))
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(S);
            }
            catch (Exception ex)
            {
                Log($"JSON DECODE ERROR: {ex.Message}\r\nJSON DECODE ERROR: {S}");
                return Default;
            }
        }

        /// <summary>
        /// Converts any Object to JSON
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="Pretty">Pretty print</param>
        /// <returns>JSON string</returns>
        public static string ToJson(this object o, bool Pretty = false, bool StripNull = false)
        {
            return JsonConvert.SerializeObject(o, Pretty ? Formatting.Indented : Formatting.None, new JsonSerializerSettings() { NullValueHandling = StripNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }

        /// <summary>
        /// Converts this DateTime Object into a linux timestamp
        /// </summary>
        /// <param name="DT">Date Object</param>
        /// <returns>Linux Timestamp</returns>
        public static long ToLinuxTime(this DateTime DT)
        {
            return (long)DT.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        /// Converts this integer from a Linux timestamp
        /// </summary>
        /// <param name="Timestamp">Timestamp</param>
        /// <returns>Date Object</returns>
        public static DateTime FromLinuxTime(this int Timestamp)
        {
            return FromLinuxTime((long)Timestamp);
        }

        /// <summary>
        /// Converts this integer from a Linux timestamp
        /// </summary>
        /// <param name="Timestamp">Timestamp</param>
        /// <returns>Date Object</returns>
        public static DateTime FromLinuxTime(this long Timestamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(Timestamp);

        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="Length">Length of string</param>
        /// <param name="Charset">Charset to use</param>
        /// <returns>Random string</returns>
        /// <remarks>This is moderately secure but the algorithm is slightly biased if the charset length is not a power of 2</remarks>
        public static string RandomString(int Length, string Charset = CHARSET)
        {
            using (var RNG = RandomNumberGenerator.Create())
            {

                var Data = new byte[Length];
                RNG.GetBytes(Data);

                return new string(Data.Select(m => Charset[m % Charset.Length]).ToArray());

            }
        }
    }
}
