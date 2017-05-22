using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Extract.Item
{
    /// <summary>
    /// A BaseType and it's price in the economy
    /// </summary>
    public class EconomyItem
    {
        public string Name { get; set; }

        public string Group { get; set; }

        private string OldGroup { get; set; }

        public float MinC { get; set; }

        public float Variants { get; set; }

        public string AllNames { get; set; }

        public EconomyItem(string Name, string Group, float MinC, float Variants, string AllNames)
        {
            this.Name = Name;
            this.Group = Group;
            this.OldGroup = Group;
            this.MinC = MinC;
            this.Variants = Variants;
            this.AllNames = AllNames;
        }

        public void ResetToOld()
        {
            this.Group = this.OldGroup;
        }
    }
}
