using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class Tier
    {
        public int t0lvl;
        public string GroupName = "";
        public List<Entry> FilterEntries = new List<Entry>();
        public List<string> DisplayRows;
        public List<string> TierRows;

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