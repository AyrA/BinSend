using System.Linq;
using System.Net;

namespace BinSend
{
    public struct Config
    {
        public ApiConfig ApiSettings;
        public Template[] Templates;

        public bool Valid()
        {
            return Templates != null &&
                Templates.All(m => m.Valid()) &&
                ApiSettings.Valid();
        }
    }

    public struct Template
    {
        public string Name;
        public string Value;

        public bool Valid()
        {
            return !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Value);
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
