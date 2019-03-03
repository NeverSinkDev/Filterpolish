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

            IncludeRelic = false;

            List<EconomyItem> eList = new List<EconomyItem>();
            foreach (var i in this.ConnectedTiers.PIC.sortedCollection)
            {
                NinjaItem item;
                if (!TierSortingAscending)
                {
                    var relicList = i.Value?.Where(j => (!j.isItemRelic() || IncludeRelic));
                    if (relicList.Count() <= 0)
                    {
                        continue;
                    }

                    item = relicList.Aggregate((i1, i2) => i1.CVal < i2.CVal ? i1 : i2);
                }
                else
                {
                    if (i.Value.Count <= 1)
                    {
                        continue;
                    }

                    var relicList = i.Value?.Where(j => (!j.isItemRelic() || IncludeRelic));
                    if (relicList.Count() <= 0)
                    {
                        continue;
                    }

                    item = relicList.Aggregate((i1, i2) => i1.CVal < i2.CVal ? i2 : i1);
                }

                // Get the list of all item Names in a Tier

                if (group.Contains("Divination"))
                {
                    var uniquesPerBaseType = this.ConnectedTiers.PIC.getItemsByName(i.Key).ToList();

                    string names = string.Join(", ", uniquesPerBaseType.Select(
                            x => x.Name + "( " + x.CVal + " )").ToList());

                    EconomyItem eI = new EconomyItem(i.Key, this.ConnectedTiers.GetItemTier(i.Key), item.CVal, i.Value.Count, names, item.ExplicitModifiers.ToString());
                    eList.Add(eI);
                }
                else
                {
                     var uniquesPerBaseType = this.ConnectedTiers.PIC.getItemsByName(i.Key).GroupBy(x => x.Name).ToList();

                    string names = string.Join(", ", uniquesPerBaseType.Select(
                            x => x.Key + string.Join("", (x.Select(y => " (" + (y.isItemRelic() ? "L" : "") + Math.Round(y.CVal) + ")").ToList()))));

                    // Create the economy item
                    EconomyItem eI = new EconomyItem(i.Key, this.ConnectedTiers.GetItemTier(i.Key), item.CVal, i.Value.Count, names, item.ExplicitModifiers.ToString());
                    eList.Add(eI);
                }
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
