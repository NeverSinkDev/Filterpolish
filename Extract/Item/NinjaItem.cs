using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Extract.Item
{
    /// <summary>
    /// The extracted preprocessed item and price info from PoE.ninja
    /// </summary>
    public class NinjaItem
    {
        [JsonProperty("itemType")]
        public string Class { get; set; }

        [JsonProperty("baseType")]
        public string BaseType { get; set; }

        [JsonProperty("links")]
        public float Links { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("explicitModifiers")]
        public string ExplicitModifiers { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("chaosValue")]
        public float CVal { get; set; }

        [JsonProperty("exaltedValue")]
        public float XVal { get; set; }

        [JsonProperty("levelRequired")]
        public float LevelRequired { get; set; }

        [JsonProperty("count")]
        public float IndexedCount { get; set; }

        public bool isItemRelic()
        {
            return this.Icon.Contains("relic=1");
        }
    }

}
