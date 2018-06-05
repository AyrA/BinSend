using System.Collections.Generic;
using System.Linq;

namespace BinSend
{
    public class Fragment
    {
        public string Name;
        public bool SameOrigin;
        public int Part;
        public string[] List;
        public EncodingType Encoding;
        public string Content;

        public byte[] Decode()
        {
            switch (Encoding)
            {
                case EncodingType.Raw:
                    return Content.UTF();
                case EncodingType.Hex:
                    return Content.HEX();
                case EncodingType.Base64:
                    return Content.B64();
                case EncodingType.Ascii85:
                    return Content.A85();
                default:
                    Tools.Log($"Unspecified Encoding: {Encoding}");
                    return Content.UTF();
            }
        }

        public void Encode(byte[] Data)
        {
            switch (Encoding)
            {
                case EncodingType.Raw:
                    Content = Data.UTF();
                    break;
                case EncodingType.Hex:
                    Content = Data.HEX();
                    break;
                case EncodingType.Base64:
                    Content = Data.B64();
                    break;
                case EncodingType.Ascii85:
                    Content=Data.A85();
                    break;
                default:
                    Content = Data.UTF();
                    break;
            }
        }

        public bool Validate(string Hash)
        {
            return Content.SHA1().ToLower() == Hash.ToLower();
        }
    }

    public class FragmentHandler
    {
        private Fragment[] Fragments;

        public FragmentHandler(Fragment[] F)
        {
            Fragments = F;
        }

        /// <summary>
        /// Gets the ordered fragment List
        /// </summary>
        /// <param name="Hash">First Part Hash. If null, it will whatever part is number 1</param>
        /// <returns>Ordered list</returns>
        public Fragment[] GetOrdered(string Hash = null)
        {
            var F = Hash == null ?
                Fragments.FirstOrDefault(m => m.Part == 1) :
                Fragments.FirstOrDefault(m => m.List != null && m.List.Length > 0 && m.List.FirstOrDefault().ToLower() == Hash.ToLower());
            if (F == null)
            {
                return null;
            }
            return F.List.Select(m => Fragments.FirstOrDefault(n => n.Validate(m))).ToArray();
        }

        /// <summary>
        /// Joins all available fragments together
        /// </summary>
        /// <param name="Fragments">Fragment list</param>
        /// <returns>Combined data</returns>
        public byte[] Join(IEnumerable<Fragment> Fragments)
        {
            return Fragments.Where(m => m != null).SelectMany(m => m.Decode()).ToArray();
        }
    }
}
