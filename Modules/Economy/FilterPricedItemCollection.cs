using FilterPolish.Extract.Item;
using FilterPolish.Modules.TierLists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules.Economy
{
    /// <summary>
    /// Provides a visual representation of connectedTiers and their economic prices.
    /// </summary>
    public class FilterPricedItemCollection
    {
        public BindingList<EconomyItem> TierList {get; set;}

        public ConnectedTiers ConnectedTiers { get; set;}

        public string Group { get; set; }

        public FilterPricedItemCollection(ConnectedTiers CT, bool TierSortingAscending, bool IncludeRelic, string group)
        {
            this.ConnectedTiers = CT;
            this.Group = group;

            List<EconomyItem> eList = new List<EconomyItem>();
            foreach (var i in this.ConnectedTiers.PIC.sortedCollection)
            {
                NinjaItem item;
                if (!TierSortingAscending)
                {
                // Get the cheapest item in a Tier
                item = i.Value.Where(j => (!j.isItemRelic() || IncludeRelic)).Aggregate((i1, i2) => i1.CVal < i2.CVal ? i1 : i2);
                }
                else
                {
                item = i.Value.Where(j => (!j.isItemRelic() || IncludeRelic)).Aggregate((i1, i2) => i1.CVal < i2.CVal  ? i2 : i1);
                }

                // Get the list of all item Names in a Tier
                string names = string.Join(", ", this.ConnectedTiers.PIC.getItemsByName(i.Key).Select( it => (it.isItemRelic() ? "[L]" : "") + it.Name + " (" + it.CVal + ")").ToList());

                // Create the economy item
                EconomyItem eI = new EconomyItem(i.Key, this.ConnectedTiers.GetItemTier(i.Key), item.CVal, i.Value.Count, names, item.ExplicitModifiers.ToString());
                eList.Add(eI);
            }

                this.TierList = new BindingList<EconomyItem>(eList.OrderByDescending(i => i.MinC).ToList());
        }

        public string UpdateValueInConectedTiers(string BaseType, string NewGroup)
        {
            var result = this.ConnectedTiers.AssignBaseTypeToTier(BaseType, NewGroup);
            this.TierList.Where(i => i.Name == BaseType).Single().Group = NewGroup;
            //var affected = this.ConnectedTiers.Tiers.Select(i => i.GroupName).ToList();

            return result;
        }

        public void CompileAllChanges(ChangeCollection c)
        {
            if (!c.AllChangeGroups.Any(i => i.Name == this.Group))
            {
                var untiered = "";

                if (this.Group.Contains("Divination"))
                {
                    untiered = "T4";
                }

                c.AddGroup(this.Group, untiered);
            }

            foreach (var t in this.TierList)
            {
                c.AddChange(this.Group, t.Name, t.Group);
            }
        }
    }
}
