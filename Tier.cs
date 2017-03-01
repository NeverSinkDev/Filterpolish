using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class Tier
    {
        public string GroupName = "";
        public List<Entry> FilterEntries = new List<Entry>();
        public string TierRows = "";
        public string Value = "";
        public bool MissMatch = false;

        public Tier()
        {

        }

        public void modifyEntryParams(string ident, string param)
        {
            foreach (Entry e in this.FilterEntries)
            {
                e.ModifyAttribute(ident, change: param);
            }
        }
    }
}