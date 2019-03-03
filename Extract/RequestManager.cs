using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using FilterPolish.Extract.Item;
using Newtonsoft.Json.Linq;
using FilterPolish.Modules.Economy;

namespace FilterPolish.Extract
{
    /// <summary>
    /// Generates or reads the information provided by todays Poe.ninja stats
    /// and creates the pricedlists
    /// </summary>
    public class RequestManager
    {
        public List<string> urlSuffix = new List<string>();
        public List<NinjaRequest> ExecutedRequests = new List<NinjaRequest>();
        public RequestManager(List<string> urlSuffix)
        {
            this.urlSuffix = urlSuffix;
        }
         
        public void ExecuteAll(TierListManager TLM)
        {
            ExecutedRequests.Clear();

            var urlbase = Util.getConfigValue("Ninja Request URL");

            var uniquePricedCollection = new List<NinjaItem>();
            var diviPricedCollection = new List<NinjaItem>();
            var mapPricedCollection = new List<NinjaItem>();
            var baseTypePriceCollection = new List<NinjaItem>();

            foreach (string s in urlSuffix)
            {

                NinjaRequest n; 
                
                if (System.IO.File.Exists(Util.GetRootPath() + "/EcoData/" + Util.GetTodayDateTimeExtension() + "/" + s.Replace("ItemOverview?type=BaseType", "itemStats") + ".json"))
                {     
                    n = new NinjaRequest(s, "Reading from File", true);
                }
                else
                {
                    var leagueKey = s.Contains("?") ? "&league=" : "?league=";
                    n = new NinjaRequest(s, Util.GetNinjaApi() + s + leagueKey + Util.getConfigValue("Ninja League"), false);
                    n.SaveToFile(s);
                }

                JsonConvert.PopulateObject(n.responseContent, n.DSO);
                dynamic D = JsonConvert.DeserializeObject<dynamic>(n.responseContent, new JsonSerializerSettings() { CheckAdditionalContent = true });

                var lines = D.lines;
                var count  = Enumerable.Count(lines);

                foreach ( JObject res in lines)
                {
                    NinjaItem ni = new NinjaItem();
                    res["explicitModifiers"] = res["explicitModifiers"].ToString();
                    JsonConvert.PopulateObject(res.ToString(), ni);
                    
                    if (s.Contains("Map"))
                    {
                        mapPricedCollection.Add(ni);
                    }
                    else if (s.Contains("Divination"))
                    {
                        diviPricedCollection.Add(ni);
                    }
                    else if (s.Contains("BaseType"))
                    {
                        baseTypePriceCollection.Add(ni);
                    }
                    else
                    {
                        if (ni.Links >= 5)
                        {
                            continue;
                        }

                        uniquePricedCollection.Add(ni);
                    }
                }
            }
            TLM.pricedUniques = new PricedItemCollection(uniquePricedCollection);
            TLM.pricedDivinationCards = new PricedItemCollection(diviPricedCollection);
            TLM.pricedMaps = new PricedItemCollection(mapPricedCollection);
            TLM.pricedBases = new PricedItemCollection(baseTypePriceCollection);
            TLM.pricedBasesOverview = new PricedItemCollectionOverview(baseTypePriceCollection);
        }

        // recursively yield all children of json
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }

}
