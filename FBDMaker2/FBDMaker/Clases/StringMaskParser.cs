using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FBDMaker
{
    public class StringMaskParser
    {
        public StringMaskParser(string inMask = "")
        {
            Mask = (inMask == "" ? string.Empty : inMask);
            Dict = listKey.ToDictionary(s => s, s => string.Empty);
        }

        public StringMaskParser(string inMask,List<string> KeyList)
        {
            Mask = (inMask == "" ? string.Empty : inMask);
            Dict = KeyList.ToDictionary(s => s, s => string.Empty);
        }

        public void ClearResult()
        {
            List<string> keys = Dict.Keys.ToList<string>();
            foreach (string s in keys)
            {
               Dict[s] = string.Empty ;
            }
        }

        public void FillDictionary(List<string> KeyList)
        {
            Dict.Clear();
            Dict = KeyList.ToDictionary(s => s, s => string.Empty);
        }


        public string Mask { get; set; }
        public Dictionary<string, string> Dict {get; private set ;}

        public void Parse(string Source)
        {
            List<string> keys = Dict.Keys.ToList<string>();
            foreach (string s in keys)
            {
                Dict[s] = string.Empty;
            }

            string lMask = Mask;
            foreach (string s in Dict.Keys)
            {
                string s1 = "%" + s;
                string s2 = "(?<" + s.ToLower() + ">.+)";
                lMask = lMask.Replace(s1, s2);
            }
            Regex rx = new Regex(lMask, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            Match matches = rx.Match(Source);

            GroupCollection groups = matches.Groups;
            //List<string> keys = Dict.Keys.ToList<string>();
            foreach (string s in keys.Where(s => groups[s].Success))
            {
                Dict[s] = groups[s].Value;
            }
            if (string.IsNullOrEmpty(Dict["tl"]))
                Dict["tl"] = Source;
        }

        protected static List<string> listKey;

        static StringMaskParser()
        {
            string[] keys = { "tl", 
                                "st", "sn", 
                                "av", 
                                "nf1", "nl1", "nm1", "nn1",
                                "nf2", "nl2", "nm2", "nn2", 
                                "nf3", "nl3", "nm3", "nn3",
                                "nf4", "nl4", "nm4", "nn4",
                                "nf5", "nl5", "nm5", "nn5",
                                "ed", "ct","ye"};
            listKey = new List<string>(keys);
        }

        public static void ParseWithDict(String Source, string Mask, Dictionary<string, string> Dict)
        {
            foreach (string s in Dict.Keys)
            {
                string s1 = "%" + s;
                string s2 = "(?<" + s.ToLower() + ">.+)";
                Mask=Mask.Replace(s1, s2);
            }
            Regex rx = new Regex(Mask,RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            Match matches = rx.Match(Source);

            GroupCollection groups = matches.Groups;
            List <string> keys = Dict.Keys.ToList<string>();
            foreach(string s in keys)
            {
                if (groups[s].Success)
                    Dict[s] = groups[s].Value;
            }
               
           
        }
        public static Dictionary<string, string> ParseWithMask(String Source, string Mask  )
        {
            Dictionary<string, string> Dict = listKey.ToDictionary(s => s, s => string.Empty);

            ParseWithDict(Source, Mask, Dict);
            return Dict;

        }

        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^ \w\.@-]", " ",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}
