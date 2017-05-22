using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    /// <summary>
    /// A single tier as described in a filter.
    /// Contains several entries and a certain Value of an identifier, such as BaseType/Class..
    /// </summary>
    public class Tier
    {
        public string oldValue = "";

        public bool Changed { get; set; }
        public int Uses { get { return this.FilterEntries.Count; } }
        public string GroupName { get; set; }
        public List<Entry> FilterEntries { get; set; }
        public string TierRows { get; set; }
        public string Value { get; set; }

        public Tier()
        {
            this.GroupName = "";
            this.FilterEntries = new List<Entry>();
            this.TierRows = "";
            this.Value = "";
            this.Changed = false;
        }

        public void modifyEntryParams(string ident, string param)
        {
            foreach (Entry e in this.FilterEntries)
            {
                e.ModifyAttributeSimple(ident, change: param);
                this.Value = param;
                if (this.Value != this.oldValue)
                {
                    this.Changed = true;
                }
                else
                {
                    this.Changed = false;
                }
            }
        }
    }
}