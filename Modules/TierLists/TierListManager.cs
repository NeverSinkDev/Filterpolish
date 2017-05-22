using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    /// <summary>
    /// Contains all filter tiers and provides links to the priced items, without connecting
    /// them directly
    /// </summary>
    public class TierListManager
    {
        public Filter filter;
        public BindingList<Tier> tierList;
        private List<Tier> sortedList;
        public int CurrentIndex = -1;

        public PricedItemCollection pricedUniques { get; set; }
        public PricedItemCollection pricedDivinationCards { get; set; }
        public PricedItemCollection pricedMaps { get; set; }

        public TierListManager(Filter filter)
        {
            this.filter = filter;
            this.sortedList = new List<Tier>();
            this.init();
        }

        public void init()
        {
            for(int i = 0; i<filter.EntryList.Count; i++)
            {
                Entry currentEntry = this.filter.EntryList[i];
                currentEntry.BuildTags.Clear();
                currentEntry.FindAllVersionTag();

                foreach (string s in currentEntry.BuildTags)
                { 
                bool TB = s.Contains("%TB-");
                bool TC = s.Contains("%TC-");
                bool TI = s.Contains("%TI-");
                bool TD = s.Contains("%TD-");

                if (TI || TC || TB || TD)
                {

                    string groupName = s;
                    string coreIdent = "";
                    if (this.sortedList.Any(fe => fe.GroupName == groupName))
                    {
                        this.sortedList.Where(fe => fe.GroupName == groupName).Single().FilterEntries.Add(currentEntry);
                    }
                    else
                    {
                        coreIdent = TD ? "DropLevel" : coreIdent;
                        coreIdent = TC ? "Class" : coreIdent;
                        coreIdent = TB ? "BaseType" : coreIdent;
                        coreIdent = TI ? "ItemLevel" : coreIdent;
                        this.createTier(currentEntry, coreIdent, groupName);
                    }
                }
                }
            }

            this.tierList = new BindingList<Tier>(this.sortedList.OrderBy(i => i.GroupName).ThenBy(i => i.TierRows).ToList());
            filter.AddFilterProgressToLogBox("Tierlist Generated with: " + this.sortedList.Count + " " + "tags!");
        }

        public void createTier(Entry e, string coreIdent, string groupName)
        {
            Tier t = new Tier();
            t.FilterEntries.Add(e);
            t.TierRows = coreIdent;
            t.GroupName = groupName;
            t.Value = e.GetLines(coreIdent).FirstOrDefault().Value;
            t.oldValue = t.Value;
            this.sortedList.Add(t);
        }

    }
}
