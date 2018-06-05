﻿using Newtonsoft.Json;
using System;
using System.Text;

namespace BinSend
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Value designating to use all bytes
        /// </summary>
        public const int DATA_EVERYTHING = -1;

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
            catch
            {
                return Default;
            }
        }

        /// <summary>
        /// Converts any Object to JSON
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="Pretty">Pretty print</param>
        /// <returns>JSON string</returns>
        public static string ToJson(this object o, bool Pretty = false)
        {
            return JsonConvert.SerializeObject(o, Pretty ? Formatting.Indented : Formatting.None);
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
    }
}