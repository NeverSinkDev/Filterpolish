using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class Item
    {
        public List<KeyValuePair<string,object>> stats;
        public bool Equippable = false;


        public Item(string Name, string Type)
        {
            stats = new List<KeyValuePair<string, object>>();
            this.GenerateDefaultItemType(Name, Type);
        }

        public void GenerateDefaultItemType(string Name, string Type)
        {
            this.AddStat("Name", Name);
            this.AddStat("BaseType", Name);
            this.AddStat("Class", Type);

            switch (Type.ToString())
            {
                case "Gems":
                    this.AddStat("Corrupted", false);
                    this.AddStat("Quality", 0);
                    this.AddStat("Height", 1);
                    this.AddStat("Width", 1);
                    break;

                case "Currency":
                case "Map Fragments":
                case "Labyrinth Item":
                    this.AddStat("Height", 1);
                    this.AddStat("Width", 1);
                    break;

                case "Maps":
                    this.AddStat("Height", 1);
                    this.AddStat("Width", 1);
                    this.AddStat("DropLevel", 68);
                    this.AddStat("ItemLevel", 68);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    break;

                case "Flasks":
                    this.AddStat("Height", 2);
                    this.AddStat("Width", 1);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Identified", false);
                    break;

                case "Rings":
                case "Jewels":
                case "Amulets":
                    this.AddStat("Height", 1);
                    this.AddStat("Width", 1);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    break;

                case "Belts":
                    this.AddStat("Height", 1);
                    this.AddStat("Width", 2);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    break;

                case "Quivers":
                    this.AddStat("Height", 3);
                    this.AddStat("Width", 2);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    break;

                case "Two Hand":
                case "Bows":
                case "Body Armour":
                case "Staves":
                    this.AddStat("Height", 3);
                    this.AddStat("Width", 2);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    this.AddStat("Quality", 0);
                    this.AddStat("Sockets", 1);
                    this.AddStat("Links", 1);
                    this.AddStat("SocketGroup", "W");
                    this.AddStat("MaxSockets", 6);
                    break;

                case "Gloves":
                case "Boots":
                case "Helmets":
                    this.AddStat("Height", 2);
                    this.AddStat("Width", 2);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    this.AddStat("Quality", 0);
                    this.AddStat("Sockets", 1);
                    this.AddStat("Links", 1);
                    this.AddStat("SocketGroup", "W");
                    this.AddStat("MaxSockets", 4);
                    break;

                case "One Hand":
                case "Wands":
                case "Sceptre":
                case "Claws":
                case "Daggers":
                case "Shields":
                    this.AddStat("Height", 2);
                    this.AddStat("Width", 2);
                    this.AddStat("DropLevel", 1);
                    this.AddStat("ItemLevel", 1);
                    this.AddStat("Rarity", "Normal");
                    this.AddStat("Corrupted", false);
                    this.AddStat("Identified", false);
                    this.AddStat("Quality", 0);
                    this.AddStat("Sockets", 1);
                    this.AddStat("Links", 1);
                    this.AddStat("SocketGroup", "W");
                    this.AddStat("MaxSockets", 3);
                    break;
            }
        }

        void AddStat(string key, object value)
        {
            stats.RemoveAll(x => x.Key == key);
            KeyValuePair<string,object> kv = new KeyValuePair<string,object>(key, value);
            stats.Add(kv);
        }

        object GetStat(object key)
        {
           
            object result = stats.Where(x => x.Key.Equals(key.ToString())).SingleOrDefault().Value;

            if (key.ToString() == "Rarity")
            {
                //result = Comparison_Delegates.GetRarityInteger(result.ToString());
            }

            return result;
        }

        bool CompareStat(string Identifier, object EntryParams, Func<object,object,bool> Comparison )
        {
            object ItemStat = this.GetStat(Identifier);

            if (ItemStat != null)
            { 
            return Comparison(EntryParams, ItemStat);
            }

            return false;
        }

        
    }
}
