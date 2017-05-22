using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilterPolish.Modules.TierLists
{
    /// <summary>
    /// Connects associated single filter tiers and also provides the link to the priced
    /// Item list from PoE.ninja. Provides the means required to edit the connected tiers
    /// </summary>
    public class ConnectedTiers
    {
        public List<Tier> Tiers = new List<Tier>();
        public TierListManager TLM;
        public PricedItemCollection PIC;

        public ConnectedTiers(TierListManager tlm, PricedItemCollection pic, string common)
        {
            this.TLM = tlm;
            this.PIC = pic;
            this.Tiers = TLM.tierList.Where(i => i.GroupName.Contains(common)).ToList();
        }

        /// <summary>
        /// Scans all connected tiers
        /// </summary>
        /// <param name="BaseType"></param>
        /// <returns></returns>
        public string GetItemTier(string BaseType)
        {
            foreach (var t in this.Tiers)
            {
                if (t.Value != null)
                {
                    List<string> str = new List<string>();
                    if (t.Value.Contains(" "))
                    {
                        var str1 = Regex.Matches(t.Value, @"[\""].+?[\""]|[^ ]+")
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToList();

                        foreach (string s in str1)
                        {
                            str.Add(s.Replace("\"", ""));
                        }
                    }
                    else
                    {
                        str.Add(t.Value.Replace("\"", ""));
                    }

                    foreach (var s in str)
                    {
                        if (BaseType.Contains(s))
                        {
                            return t.GroupName;
                        }
                    }
                }
            }
            return null;
        }

        public string AssignBaseTypeToTier(string BaseType, string DestinationTier)
        {
            string resConcrete = "";
            foreach (var t in this.Tiers)
            {

                bool res = false;
                List < string > str = new List<string>();
                if (t.Value.Length > 0)
                {
                    // Break the basetype list into a string
                    if (t.Value.Contains(" "))
                    {
                        var str1 = Regex.Matches(t.Value, @"[\""].+?[\""]|[^ ]+")
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToList();

                        foreach (string s in str1)
                        {
                            str.Add(s.Replace("\"", ""));
                        }
                    }
                    else
                    {
                        str.Add(t.Value.Replace("\"", ""));
                    }

                    // If the list contains the basetype we're looking for... remove it
                    if (str.Contains(BaseType))
                    {
                        str.RemoveAll(i => i.Contains(BaseType));
                    }

                    // If we're in the right entry, add the basetype there
                    if (t.GroupName == DestinationTier)
                    {
                        str.Add(BaseType);
                        res = true;
                    }

                    for (int i = 0; i < str.Count; i++)
                    {

                        string s = str[i];
                        if (s[0] != '"')
                        {
                            s = '"' + s;
                        }

                        if (s[s.Length - 1] != '"')
                        {
                            s = s + '"';
                        }
                        str[i] = s;
                    }

                    //Sort the list alphabetically
                    if (str.Count > 1)
                    { 
                    str.Sort();
                    }

                    //Add the BaseType to the list
                    t.Value = string.Join(" ", str);
                    t.modifyEntryParams(t.TierRows, t.Value);

                    if (res)
                    {
                        resConcrete = t.Value;
                    }
                }
            }

            return resConcrete;
        }
    }
}
