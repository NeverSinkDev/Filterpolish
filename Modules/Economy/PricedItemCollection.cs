using FilterPolish.Extract.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class PricedItemCollection
    {
        public Dictionary<string, List<NinjaItem>> sortedCollection { get; set; }

        public bool BaseTypeExists { get; set; }

        public PricedItemCollection(List<NinjaItem> initlist)
        {
            this.sortedCollection = new Dictionary<string, List<NinjaItem>>();

            foreach (NinjaItem item in initlist)
            {

                string keyTag;

                if (item.BaseType == "Unknown" || item.BaseType == null)
                {
                    keyTag = item.Name;
                    this.BaseTypeExists = false;
                    
                }
                else
                {
                    keyTag = item.BaseType;
                    this.BaseTypeExists = true;
                }

                if (this.sortedCollection.Keys.Contains(keyTag))
                {
                    this.sortedCollection[keyTag].Add(item);
                }
                else
                {
                    this.sortedCollection[keyTag] = new List<NinjaItem>();
                    this.sortedCollection[keyTag].Add(item);
                }
            }
        }

        public List<NinjaItem> getItemsByName (string BaseType)
        {
            if(this.sortedCollection.ContainsKey(BaseType))
            {
                return this.sortedCollection[BaseType]; 
            }
            else
            {
                return null;
            }
        }
    }
}
