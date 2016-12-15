using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BinSend
{
    public static class Templates
    {
        /// <summary>
        /// The default Template
        /// </summary>
        public const string DEFAULT = @"ENTER DESCRIPTION HERE

==================
This is a chunked file transfer over bitmessage.
You can reassemble the parts using BinSend: https://github.com/AyrA/BinSend

#BEGIN#
;Define if all parts must come from the same address.
SameOrigin=Yes
;This is the file name
Name={0}
;This is CurrentPart;AllParts
Part={1};{2}
;This is the encoding format (base64, Ascii85, etc)
Format={3}
;Length of the encoded Content (not the decoded)
Length={4}
;Hashes of all decoded contents
HashList={5}
;Finally the encoded content.
Content={6}";
        /// <summary>
        /// The default Template (short form)
        /// </summary>
        public const string DEFAULT_SHORT = @"ENTER DESCRIPTION HERE

==================
This is a chunked file transfer over bitmessage.
You can reassemble the parts using BinSend: https://github.com/AyrA/BinSend

#BEGIN#
SameOrigin=Yes
Name={0}
Part={1};{2}
Format={3}
Length={4}
HashList={5}
Content={6}";

        /// <summary>
        /// Raw data. No header or other stuff
        /// </summary>
        public const string RAW = "{6}";
        /// <summary>
        /// HTML5 video (ogg)
        /// </summary>
        public const string VIDEO = @"<div style='font-family:Sans-Serif;'>
Right click this message and save it with extension .html<br />
Has been tested with firefox, chrome and opera.<br />
<br />
<video width='480' height='360' controls>
<source src='data:video/ogg;base64,{6}' type='video/ogg' />
</video>";
        /// <summary>
        /// HTML5 audio (mp3)
        /// </summary>
        public const string AUDIO = @"<div style='font-family:Sans-Serif;'>
Right click this message and save it with extension .html<br />
Has been tested with firefox, chrome and opera.<br />
<br />
<audio controls>
<source src='data:audio/mp3;base64,{6}' type='audio/mp3' />
</audio>";
        /// <summary>
        /// Template directory
        /// </summary>
        public const string DIR_TEMPLATE="templates";

        public static void delete(string Name)
        {
            if (Directory.Exists(DIR_TEMPLATE))
            {
                if (File.Exists(string.Format(@"{0}\{1}.txt", DIR_TEMPLATE, Name)))
                {
                    File.Delete(string.Format(@"{0}\{1}.txt", DIR_TEMPLATE, Name));
                }
            }
        }

        public static void createStructure()
        {
            Template T;
            if (!Directory.Exists(DIR_TEMPLATE))
            {
                Directory.CreateDirectory(DIR_TEMPLATE);
            }

            T = new Template();
            T.ChunkSize = 100;
            T.Name = "Default";
            T.ProposedFormat = EncodingFormat.Base64;
            T.Subject = "Chunked Transfer";
            T.Text = DEFAULT;
            T.Save();

            T = new Template();
            T.ChunkSize = 100;
            T.Name = "Default (short)";
            T.ProposedFormat = EncodingFormat.Base64;
            T.Subject = "Chunked Transfer";
            T.Text = DEFAULT_SHORT;
            T.Save();

            T = new Template();
            T.ChunkSize = 0;
            T.Name = "Audio";
            T.ProposedFormat = EncodingFormat.Base64;
            T.Subject = "Audio file (mp3)";
            T.Text = AUDIO;
            T.Save();

            T = new Template();
            T.ChunkSize = 0;
            T.Name = "Video";
            T.ProposedFormat = EncodingFormat.Base64;
            T.Subject = "video file (ogg)";
            T.Text = VIDEO;
            T.Save();

            T = new Template();
            T.ChunkSize = 0;
            T.Name = "Raw";
            T.ProposedFormat = EncodingFormat.Raw;
            T.Subject = "Raw transfer";
            T.Text = RAW;
            T.Save();
        }

        public static string[] AllTemplates
        {
            get
            {
                string[] s = null;
                try
                {
                    s = Directory.GetFiles(DIR_TEMPLATE, "*.txt");
                }
                catch
                {
                    createStructure();
                    s = Directory.GetFiles(DIR_TEMPLATE, "*.txt");
                }
                for (int i = 0; i < s.Length; i++)
                {
                    s[i] = s[i].Substring(s[i].LastIndexOf('\\') + 1);
                    s[i] = s[i].Substring(0, s[i].Length - 4);
                }
                return s;
            }
        }
    }
}
