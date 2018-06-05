using System;
using System.IO;
using System.Linq;
using System.Net;

namespace BinSend
{
    public enum EncodingType : int
    {
        Raw = 0,
        Base64 = 1,
        Ascii85 = 2,
        Hex = 3
    }

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

        public bool Save(string Dir)
        {
            if (string.IsNullOrEmpty(Dir))
            {
                Dir = Environment.CurrentDirectory;
            }

            var P = Path.Combine(Dir, CONF_NAME);

            try
            {
                //Serialize with formatting because this file is likely edited by the user
                File.WriteAllText(P, this.ToJson(true));
            }
            catch
            {
                return false;
            }
            return true;
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

        public static Config GetDefaults()
        {
            return new Config()
            {
                ApiSettings = ApiConfig.GetDefaults(),
                Templates = Template.GetDefaults()
            };
        }
    }

    public struct Template
    {
        #region Template Defaults
        /// <summary>
        /// The default Template
        /// </summary>
        public const string DEFAULT = @"BINSEND FORMAT
==================
This is a chunked file transfer over bitmessage.
You can reassemble the parts using BinSend: https://github.com/AyrA/BinSend
#BEGIN#
{BINSEND:CHUNK}";
        /// <summary>
        /// The default Template (short form)
        /// </summary>
        public const string DEFAULT_SHORT = "#BEGIN#{BINSEND:CHUNK}";

        /// <summary>
        /// Raw data. No header or other stuff
        /// </summary>
        public const string RAW = "{6}";
        /// <summary>
        /// HTML5 video (ogg)
        /// </summary>
        public const string VIDEO = @"<div style='font-family:Sans-Serif;'>
Right click this message and save it with extension <code>.html</code><br />
Works in all major browsers.<br />
<br />
<video width='480' height='360' controls>
<source src='data:video/ogg;base64,{6}' type='video/ogg' />
</video>";
        /// <summary>
        /// HTML5 audio (mp3)
        /// </summary>
        public const string AUDIO = @"<div style='font-family:Sans-Serif;'>
Right click this message and save it with extension <code>.html</code><br />
Works in all major browsers.<br />
<br />
<audio controls>
<source src='data:audio/mp3;base64,{6}' type='audio/mp3' />
</audio>";
        #endregion

        public string Name;
        public string Content;
        public EncodingType Encoding;

        public bool Valid()
        {
            return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Content);
        }

        public Template(string TemplateName, string TemplateContent, EncodingType TemplateEncoding)
        {
            Name = TemplateName;
            Content = TemplateContent;
            Encoding = TemplateEncoding;
        }

        public static Template[] GetDefaults()
        {
            return new Template[]
            {
                new Template("Default", DEFAULT, EncodingType.Base64),
                new Template("Default (Short)", DEFAULT_SHORT, EncodingType.Base64),
                new Template("Raw", RAW, EncodingType.Raw),
                new Template("Video (ogg)", VIDEO, EncodingType.Base64),
                new Template("Audio (mp3)", AUDIO, EncodingType.Base64)
            };
        }
    }

    public struct ApiConfig
    {
        public string IpOrHostname;
        public int Port;
        public string Username;
        public string Password;

        public static ApiConfig GetDefaults()
        {
            return new ApiConfig()
            {
                IpOrHostname = "127.0.0.1",
                Port = 8442,
                Username = "username",
                Password = "password"
            };
        }

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
