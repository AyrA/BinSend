﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BinSend
{
    public class Fragment : ICloneable
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
                    Tools.Log($"Unspecified Encoding '{Encoding}'. Falling back to Raw");
                    return Content.UTF();
            }
        }

        public void Encode(byte[] Data, int Start, int Count)
        {
            var Temp = Data.Skip(Start).Take(Count).ToArray();
            switch (Encoding)
            {
                case EncodingType.Raw:
                    Content = Temp.UTF();
                    break;
                case EncodingType.Hex:
                    Content = Temp.HEX();
                    break;
                case EncodingType.Base64:
                    Content = Temp.B64();
                    break;
                case EncodingType.Ascii85:
                    Content = Temp.A85();
                    break;
                default:
                    Tools.Log($"Unknown encoding type '{Encoding}'. Falling back to Raw");
                    Content = Temp.UTF();
                    break;
            }
        }

        public void Encode(byte[] Data)
        {
            Encode(Data, 0, Data.Length);
        }

        public bool Validate(string Hash)
        {
            return Content.SHA1().ToLower() == Hash.ToLower();
        }

        public override int GetHashCode()
        {
            var code = SameOrigin.GetHashCode() ^ Part.GetHashCode();
            if (List != null)
            {
                foreach (var S in List)
                {
                    code ^= S.GetHashCode();
                }
            }
            if (Name != null)
            {
                code ^= Name.GetHashCode();
            }
            if (Content != null)
            {
                code ^= Content.GetHashCode();
            }
            return code;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Fragment))
            {
                return false;
            }
            var F = obj as Fragment;
            return GetHashCode() == F.GetHashCode();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name) || List == null)
            {
                return base.ToString();
            }
            return $"{Name} ({List.Length} Parts)";
        }

        public object Clone()
        {
            //Basic clone of this instance
            var Temp = (Fragment)MemberwiseClone();
            //Deep clone array to lose reference
            if (Temp.List != null)
            {
                Temp.List = (string[])Temp.List.Clone();
            }
            return Temp;
        }
    }

    public class FragmentInfo : ICloneable
    {
        public Fragment Fragment;
        public string MessageId;

        public FragmentInfo() : this(null, null)
        {
        }

        public FragmentInfo(Fragment F, string Id)
        {
            Fragment = F == null ? null : (Fragment)F.Clone();
            MessageId = Id;
        }

        public object Clone()
        {
            return new FragmentInfo()
            {
                Fragment = (Fragment)Fragment.Clone(),
                MessageId = MessageId
            };
        }

        public override int GetHashCode()
        {
            var Code = 0;
            if (Fragment == null && MessageId == null)
            {
                Code = base.GetHashCode();
            }
            else
            {
                if (Fragment != null)
                {
                    Code ^= Fragment.GetHashCode();
                }
                if (MessageId != null)
                {
                    Code ^= MessageId.GetHashCode();
                }
            }
            return Code;
        }

        public override bool Equals(object obj)
        {
            return
                obj != null &&
                (obj is FragmentInfo) &&
                GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            if (Fragment == null || string.IsNullOrEmpty(Fragment.Name) || Fragment.List == null)
            {
                return base.ToString();
            }
            return $"{Fragment.Name} ({Fragment.List.Length} Parts)";
        }
    }

    public class FragmentHandler
    {
        private List<FragmentInfo> Fragments;

        public FragmentInfo[] All
        {
            get
            {
                return Fragments
                    .Select(m => m.Clone())
                    .OfType<FragmentInfo>()
                    .ToArray();
            }
        }

        public string FromAddr { get; set; }

        public FragmentHandler()
        {
            Fragments = new List<FragmentInfo>();
        }

        public FragmentHandler(Fragment[] F, string[] Ids)
        {
            Fragments = new List<FragmentInfo>();
            if (F != null && Ids != null && F.Length == Ids.Length)
            {
                for (var i = 0; i < F.Length; i++)
                {
                    Fragments.Add(new FragmentInfo(F[i], Ids[i]));
                }
            }
        }

        public FragmentInfo Add(Fragment F, string MessageId)
        {
            FragmentInfo FI = null;
            if (F != null && !Fragments.Any(m => m.Fragment.Equals(F)))
            {
                Fragments.Add(FI = new FragmentInfo(F, MessageId));
                FI = (FragmentInfo)FI.Clone();
            }
            return FI;
        }

        public void Delete(BitmessageRPC RPC)
        {
            foreach (var Id in Fragments.Select(m => m.MessageId))
            {
                try
                {
                    RPC.trashMessage(Id);
                }
                catch (Exception ex)
                {
                    Tools.Log($"Unable to delete fragment. Reason: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets a list of all unique first parts
        /// </summary>
        /// <returns></returns>
        public FragmentInfo[] GetPrimary()
        {
            return Fragments
                .Where(m => m.Fragment.Part == 1)
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Gets the ordered fragment List
        /// </summary>
        /// <param name="FirstPart">First Part. Part number must be set to 1 and it needs a hash list</param>
        /// <returns>Ordered list. Gaps filled with "null"</returns>
        public FragmentInfo[] GetOrdered(Fragment FirstPart)
        {
            if (FirstPart == null || FirstPart.List == null || FirstPart.Part != 1)
            {
                return null;
            }
            return FirstPart.List
                .Select(m => Fragments.FirstOrDefault(n => n.Fragment.Validate(m)))
                .ToArray();
        }

        /// <summary>
        /// Gets the ordered fragment List disregarding same origin policy
        /// </summary>
        /// <param name="FirstPart">First Part. Part number must be set to 1, it needs a hash list and SameOrigin needs to be false</param>
        /// <param name="AllHandlers">List of all handlers to search parts for</param>
        /// <returns>Ordered list. Gaps filled with "null"</returns>
        public FragmentInfo[] GetOrderedNoOrigin(Fragment FirstPart, FragmentHandler[] AllHandlers)
        {
            if (FirstPart == null || FirstPart.List == null || FirstPart.Part != 1 || FirstPart.SameOrigin)
            {
                return null;
            }
            var GlobalPartList = AllHandlers.SelectMany(m => m.Fragments).Distinct().ToArray();
            return FirstPart.List
                .Select(m => GlobalPartList.FirstOrDefault(n => n.Fragment.Validate(m)))
                .ToArray();
        }

        /// <summary>
        /// Joins all available fragments together in the order supplied
        /// </summary>
        /// <param name="Fragments">Fragment list</param>
        /// <returns>Combined data</returns>
        public byte[] Join(IEnumerable<FragmentInfo> Fragments)
        {
            if (Fragments != null)
            {
                return Join(Fragments.Select(m => m.Fragment));
            }
            return null;
        }

        /// <summary>
        /// Joins all available fragments together in the order supplied
        /// </summary>
        /// <param name="Fragments">Fragment list</param>
        /// <returns>Combined data</returns>
        public byte[] Join(IEnumerable<Fragment> Fragments)
        {
            if (Fragments != null)
            {
                return Fragments
                    .Where(m => m != null)
                    .SelectMany(m => m.Decode())
                    .ToArray();
            }
            return null;
        }
    }
}
