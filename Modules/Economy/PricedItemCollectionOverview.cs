using FilterPolish.Extract.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules.Economy
{
    public class PricedItemCollectionOverview
    {
        public List<Func<NinjaItem, bool>> Selectors { get; set; } = new List<Func<NinjaItem, bool>>();
        public List<NinjaItem> InitialList { get; set; } = new List<NinjaItem>();

        public PricedItemCollectionOverview(List<NinjaItem> initlist)
        {
            this.InitialList = initlist;
        }

        public Dictionary<string,List<NinjaItem>> InitializeStructure()
        {
            return this.InitialList.Where(x => Selectors.All(func => func(x)))
                .GroupBy(x => x.BaseType)
                .ToDictionary(group => group.Key, group => group.OrderBy(x => x.LevelRequired).ToList());
        }

        public PricedItemCollectionOverview AddSelector(Func<NinjaItem,bool> selector)
        {
            this.Selectors.Add(selector);
            return this;
        }

        public List<KeyValuePair<string, List<NinjaItem>>> ProcessBaseTypeData(Dictionary<string, List<NinjaItem>> input, int minAvgChaosValue, int minMaxChaosValue, int minIndexedQuantity)
        {
            // Remove items that are way too cheap to be bothered with (note ilvl 86 tier is very expensive, so we gotta start high)
            //var adjustedPrices = input.Where(x => x.Value.Sum(z => z.CVal * z.IndexedCount) / x.Value.Sum(j => j.IndexedCount) > minAvgChaosValue).ToList();
            //adjustedPrices.ForEach(x =>
            //{
            //    if (x.Value.Select(z => z.IndexedCount).Sum() < 25)
            //    {
            //        var max = x.Value.Max(c => c.CVal);
            //        var avg = x.Value.Where(c => c.CVal != max).Sum(z => z.CVal * z.IndexedCount) / x.Value.Sum(j => j.IndexedCount);
            //        x.Value.RemoveAll(z => z.CVal > avg * 50);
            //    }
            //});

            //var solidMinPrices = input.Where(x => x.Value.Min(z => z.CVal > minAvgChaosValue)).ToList();
            var solidMinPrices = input.Where(x => x.Value.Sum(z => z.CVal * z.IndexedCount) / x.Value.Sum(j => j.IndexedCount) >= minAvgChaosValue).ToList();
            var solidMaxPrices = solidMinPrices.Where(x => x.Value.Max(z => z.CVal) > minMaxChaosValue).ToList();

            // Remove extreme anomalies -> usually super low level items
            //var normalBases = solidMaxPrices.Where(x => x.Value.Sum(z => z.IndexedCount) < minIndexedQuantity).ToList();

            // Remove weird cases where high level items are not quite as expensive as lower tier ones (random offers)
            var removedAnomalies = solidMaxPrices
                .Where(x => (x.Value.Sum(z => z.CVal * z.IndexedCount) / x.Value.Sum(j => j.IndexedCount)) >= minMaxChaosValue || 
                x.Value.Sum(j => j.IndexedCount) >= minIndexedQuantity * 4).ToList();

            var relevantCount = removedAnomalies.Where(x => x.Value.Select(y => y.IndexedCount).Sum() >= minIndexedQuantity).ToList();

            return relevantCount;
        }
    }
}
