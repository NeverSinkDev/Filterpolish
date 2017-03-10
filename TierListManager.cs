using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class TierListManager
    {
        public Filter filter;
        public List<Tier> tierList;

        public TierListManager(Filter filter)
        {
            this.filter = filter;
            this.tierList = new List<Tier>();
            this.init();
        }

        public void init()
        {
            for(int i = 0; i<filter.EntryList.Count; i++)
            {
                Entry currentEntry = this.filter.EntryList[i];
                currentEntry.BuildTags.Clear();
                currentEntry.FindAllVersionTag();

                if (currentEntry.BuildTags.Count > 0)
                {
                 
                }

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
                    if (this.tierList.Any(fe => fe.GroupName == groupName))
                    {
                        this.tierList.Where(fe => fe.GroupName == groupName).Single().FilterEntries.Add(currentEntry);
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
            filter.AddFilterProgressToLogBox("Tierlist Generated with: " + this.tierList.Count + " " + "tags!");
        }

        public void createTier(Entry e, string coreIdent, string groupName)
        {
            Tier t = new Tier();
            t.FilterEntries.Add(e);
            t.TierRows = coreIdent;
            t.GroupName = groupName;
            t.Value = e.GetLines(coreIdent).FirstOrDefault().Value;
            this.tierList.Add(t);
        }

    }
}
