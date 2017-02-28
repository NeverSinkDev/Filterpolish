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
            for(int i = 0; i<filter.LineList.Count; i++)
            {
                Entry currentEntry = this.filter.EntryList[i];
                bool LT = currentEntry.BuildTags.Contains("LT-");
                bool CT = currentEntry.BuildTags.Contains("CT-");
                bool BT = currentEntry.BuildTags.Contains("BT-");
                if (LT || CT || BT)
                {

                    string groupName = "";
                    string coreIdent = "";
                    if (this.tierList.Any(fe => fe.GroupName == groupName))
                    {
                        this.tierList.Where(fe => fe.GroupName == groupName).Single().FilterEntries.Add(currentEntry);
                    }
                    else
                    {
                        coreIdent = LT ? "DropLevel" : coreIdent;
                        coreIdent = CT ? "Class" : coreIdent;
                        coreIdent = BT ? "BaseType" : coreIdent;
                        this.createTier(currentEntry, coreIdent, groupName);
                    }
                }
            }
        }

        public void createTier(Entry e, string coreIdent, string groupName)
        {
            Tier t = new Tier();
            t.FilterEntries.Add(e);
            t.TierRows.Add(coreIdent);
            t.GroupName = groupName;
        }

    }
}
