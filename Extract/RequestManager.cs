using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using FilterPolish.Extract.Item;
using Newtonsoft.Json.Linq;

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

            var UniquePricedCollection = new List<NinjaItem>();
            var DiviPricedCollection = new List<NinjaItem>();
            var MapPricedCollection = new List<NinjaItem>();

            

            foreach (string s in urlSuffix)
            {

                NinjaRequest n; 
                
                if (System.IO.File.Exists(Util.GetRootPath() + "/EcoData/" + Util.GetTodayDateTimeExtension() + "/" + s + ".json"))
                {     
                    n = new NinjaRequest(s, "Reading from File", true);
                }
                else
                { 
                    n = new NinjaRequest(s, Util.GetNinjaApi() + s + "?league=" + Util.getConfigValue("Ninja League"), false);
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
                        MapPricedCollection.Add(ni);
                    }
                    else if (s.Contains("Divination"))
                    {
                        DiviPricedCollection.Add(ni);
                    }
                    else
                    {
                        if (ni.Links >= 5)
                        {
                            continue;
                        }

                        UniquePricedCollection.Add(ni);
                    }
                }
            }
            TLM.pricedUniques = new PricedItemCollection(UniquePricedCollection);
            TLM.pricedDivinationCards = new PricedItemCollection(DiviPricedCollection);
            TLM.pricedMaps = new PricedItemCollection(MapPricedCollection);
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
