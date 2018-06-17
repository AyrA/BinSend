using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinSend
{
    public class Section : IEnumerable<Pair>
    {
        private string _Name;

        public string Name { get { return _Name; } set { _Name = R(value); } }
        public Pair[] Settings
        {
            get
            {
                return _Settings.ToArray();
            }
            set
            {
                _Settings = new List<Pair>(value == null ? new Pair[0] : value);
            }
        }
        public int Count
        {
            get
            {
                return _Settings.Count;
            }
        }

        private List<Pair> _Settings;

        public Section(string Name)
        {
            if (Name == null)
            {
                throw new ArgumentNullException("Name");
            }
            this.Name = Name;
            _Settings = new List<Pair>();
        }

        public Pair Add(Pair P)
        {
            if (P != null && !_Settings.Contains(P))
            {
                _Settings.Add(P);
            }
            return P;
        }

        public override int GetHashCode()
        {
            int Code = Name.GetHashCode();
            if (_Settings != null)
            {
                for (var i = 0; i < _Settings.Count; i++)
                {
                    Code ^= i;
                    Code ^= _Settings[i].GetHashCode();
                }
            }
            return Code;
        }

        public override bool Equals(object obj)
        {
            return
                obj != null &&
                obj.GetType() == typeof(Section) &&
                obj.GetHashCode() == GetHashCode();
        }

        public override string ToString()
        {
            return $"[{R(Name)}]";
        }

        public Pair this[string Name]
        {
            get
            {
                return _Settings.FirstOrDefault(m => m.Name == Name);
            }
        }

        public Pair Set(string Name, string Value, bool Spaced = false)
        {
            var P = _Settings.FirstOrDefault(m => m.Name == Name);
            if (P == null)
            {
                P = new Pair(Spaced ? $"{Name} = {Value}" : $"{Name}={Value}");
                _Settings.Add(P);
            }
            else
            {
                P.Name = Name;
                P.Value = Value;
                P.Spaced = Spaced;
            }
            return P;
        }

        public IEnumerator<Pair> GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Settings.GetEnumerator();
        }

        private static string R(string Value)
        {
            if (Value == null)
            {
                throw new ArgumentNullException(Value);
            }
            //Replace what would mess up the ini file
            return Value
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\0", @"\0");
        }
    }

    public class Pair
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Spaced { get; set; }

        public Pair(string Line)
        {
            if (string.IsNullOrEmpty(Line) || Line.IndexOf('=') == 0)
            {
                throw new ArgumentException("Line can't be empty, null or start with '='");
            }
            if (Line.Contains("="))
            {
                var Segment = Line.Substring(0, Line.IndexOf('='));
                Name = Segment.TrimEnd();
                Spaced = Name != Segment;
                Value = Line.Substring(Line.IndexOf('=') + 1).TrimStart();
            }
            else
            {
                Name = Line;
                Value = null;
                Spaced = false;
            }
        }

        public string GetLine()
        {
            return Spaced ? $"{R(Name)} = {R(Value)}" : $"{R(Name)}={R(Value)}";
        }

        public override int GetHashCode()
        {
            int Code = Spaced.GetHashCode();
            if (Name != null)
            {
                Code ^= Name.GetHashCode();
            }
            if (Value != null)
            {
                Code ^= Value.GetHashCode();
            }
            return Code;
        }

        public override bool Equals(object obj)
        {
            return
                obj != null &&
                obj.GetType() == typeof(Pair) &&
                obj.GetHashCode() == GetHashCode();
        }

        public override string ToString()
        {
            return GetLine();
        }

        private static string R(string Value)
        {
            //Replace what would mess up the ini file
            return Value
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\0", @"\0");
        }
    }

    public class INI : IEnumerable<Section>
    {
        public Section[] Sections
        {
            get
            {
                return _Sections.ToArray();
            }
        }
        public int Count
        {
            get
            {
                return _Sections.Count;
            }
        }

        private List<Section> _Sections;

        public INI(StreamReader SR)
        {
            _Sections = new List<Section>();
            Section Current = null;

            while (!SR.EndOfStream)
            {
                var Line = SR.ReadLine();
                if (!string.IsNullOrEmpty(Line))
                {
                    if (Line.StartsWith("[") && Line.EndsWith("]"))
                    {
                        if (Current != null)
                        {
                            _Sections.Add(Current);
                        }
                        Current = new Section(Line.Substring(1, Line.Length - 2));
                    }
                    else
                    {
                        Current.Add(new Pair(Line));
                    }
                }
            }
        }

        public void Write(StreamWriter SW)
        {
            foreach (var Sec in _Sections)
            {
                SW.WriteLine($"[{Sec.Name}]");
                foreach (var Set in Sec)
                {
                    SW.WriteLine(Set.GetLine());
                }
                SW.WriteLine();
            }
        }

        public Section this[string Name]
        {
            get
            {
                return _Sections.FirstOrDefault(m => m.Name == Name);
            }
        }

        public IEnumerator<Section> GetEnumerator()
        {
            return _Sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Sections.GetEnumerator();
        }
    }
}
