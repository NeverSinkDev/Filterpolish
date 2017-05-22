using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules
{
    public class ChangeGroup
    {
        public string Name { get; set; }

        public Dictionary<string,Change> Changes { get; set; }

        public string UntieredTier = "";

        public ChangeGroup(string name, string untieredTier = "")
        {
            this.Name = name;
            this.UntieredTier = untieredTier;
            this.Changes = new Dictionary<string, Change>();
        }

        public void AddOrResetInitialValues(string ident, string name)
        {
            this.Changes[ident] = new Change(ident,name, this.UntieredTier);
        }

        public void ModifyChange(string ident, string name)
        {
            if (name == "REMOVE TIER")
            {
                name = "";
            }

            if (this.Changes.ContainsKey(ident))
            {
                this.Changes[ident].NewValue = name;
            }
            else
            {
                AddOrResetInitialValues(ident, name);
            }
        }

        public List<string> GetAllChanges(bool redditMode)
        {
            List<string> res = new List<string>();
            foreach (var i in Changes.Values)
            {
                string c;

                if (redditMode)
                {
                    c = i.CompileChanges();
                }
                else
                {
                    c = i.CompileChangesArrows();
                }

                if (c!= "")
                {
                    res.Add(c);
                    res.Add("\r\n");
                }
            }

            return res;
        }
    }
}
