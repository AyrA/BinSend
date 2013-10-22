using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BinSend
{
    /// <summary>
    /// Provides predefined values for various file types.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// From address. or null/empty for none
        /// </summary>
        public string From;
        /// <summary>
        /// To address. or null/empty for none
        /// </summary>
        public string To;
        /// <summary>
        /// Name of template. Must be valid filename
        /// </summary>
        public string Name;
        /// <summary>
        /// Subject line
        /// </summary>
        public string Subject;
        /// <summary>
        /// Text to insert into each part
        /// </summary>
        public string Text;
        /// <summary>
        /// Proposed format for encoding
        /// </summary>
        public EncodingFormat ProposedFormat;
        /// <summary>
        /// Size in KB of a part
        /// </summary>
        public int ChunkSize;

        /// <summary>
        /// Creates an empty template
        /// </summary>
        public Template()
        {
            From = To = string.Empty;
            Name = "EMPTY";
            Text = Templates.DEFAULT;
            ProposedFormat = EncodingFormat.Base64;
            ChunkSize = 100;
        }

        /// <summary>
        /// loads an existing template
        /// </summary>
        /// <param name="name">Template name</param>
        public Template(string name)
        {
            if (!File.Exists(string.Format(@"{0}\{1}.txt", Templates.DIR_TEMPLATE, name)))
            {
                throw new FileNotFoundException("Template not found");
            }
            string[] parts = File.ReadAllText(string.Format(@"{0}\{1}.txt", Templates.DIR_TEMPLATE, name)).Split(';');
            if (parts.Length != 7)
            {
                throw new InvalidDataException("Template lacks required number of parts");
            }
            try
            {
                Name = JsonConverter.B64dec(parts[0]);
                Subject = JsonConverter.B64dec(parts[1]);
                Text = JsonConverter.B64dec(parts[2]);
                ProposedFormat = (EncodingFormat)Enum.Parse(typeof(EncodingFormat), parts[3]);
                if ((ChunkSize = int.Parse(parts[4])) < 0)
                {
                    throw new InvalidDataException("Size is negative");
                }
                From = JsonConverter.B64dec(parts[5]);
                To = JsonConverter.B64dec(parts[6]);
            }
            catch(Exception eBase)
            {
                throw new InvalidDataException("Parts are not valid base64 data",eBase);
            }
        }

        /// <summary>
        /// Creates a template in one go
        /// </summary>
        /// <param name="source">string from the ToString() method</param>
        public static Template FromString(string source)
        {
            throw new NotImplementedException();
        }

        ~Template()
        {
            Name = null;
            Text = null;
        }

        /// <summary>
        /// Saves the current template to file
        /// </summary>
        /// <returns>true, if successfully written</returns>
        public bool Save()
        {
            if (!Directory.Exists(Templates.DIR_TEMPLATE))
            {
                Directory.CreateDirectory(Templates.DIR_TEMPLATE);
            }
            try
            {
                File.WriteAllText(string.Format(@"{0}\{1}.txt", Templates.DIR_TEMPLATE, Name), ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates an encoded string representation with all values
        /// </summary>
        /// <returns>string?!</returns>
        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6}",
                JsonConverter.B64enc(Name),
                JsonConverter.B64enc(Subject),
                JsonConverter.B64enc(Text),
                ProposedFormat,
                ChunkSize,
                JsonConverter.B64enc(From),
                JsonConverter.B64enc(To));
        }
    }
}
