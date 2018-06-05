using System;
using System.IO;
using System.Linq;
using System.Net;

namespace BinSend
{
    public struct Config
    {
        public const string CONF_NAME = "config.json";

        public ApiConfig ApiSettings;
        public Template[] Templates;

        public bool Valid()
        {
            return Templates != null &&
                Templates.All(m => m.Valid()) &&
                ApiSettings.Valid();
        }

        public static Config Load(string Dir)
        {
            if (string.IsNullOrEmpty(Dir))
            {
                Dir = Environment.CurrentDirectory;
            }

            var P = Path.Combine(Dir, CONF_NAME);

            try
            {
                return File.ReadAllText(P).FromJson<Config>();
            }
            catch
            {

            }
            return default(Config);
        }
    }

    public struct Template
    {
        #region Template Defaults
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
Works in all major browsers.<br />
<br />
<video width='480' height='360' controls>
<source src='data:video/ogg;base64,{6}' type='video/ogg' />
</video>";
        /// <summary>
        /// HTML5 audio (mp3)
        /// </summary>
        public const string AUDIO = @"<div style='font-family:Sans-Serif;'>
Right click this message and save it with extension .html<br />
Works in all major browsers.<br />
<br />
<audio controls>
<source src='data:audio/mp3;base64,{6}' type='audio/mp3' />
</audio>";
        #endregion

        public string Name;
        public string Content;

        public bool Valid()
        {
            return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Content);
        }

        public Template(string TemplateName, string TemplateContent)
        {
            Name = TemplateName;
            Content = TemplateContent;
        }

        public static Template[] GetDefaults()
        {
            return new Template[]
            {
                new Template("Default", DEFAULT),
                new Template("Default (Short)", DEFAULT_SHORT),
                new Template("Raw", RAW),
                new Template("Video (ogg)", VIDEO),
                new Template("Audio (mp3)", AUDIO)
            };
        }
    }

    public struct ApiConfig
    {
        public string IpOrHostname;
        public int Port;
        public string Username;
        public string Password;

        public bool Valid()
        {
            return !string.IsNullOrEmpty(IpOrHostname) &&
                Port >= IPEndPoint.MinPort &&
                Port <= IPEndPoint.MaxPort &&
                !string.IsNullOrEmpty(Username) &&
                !string.IsNullOrEmpty(Password);
        }
    }
}
