using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules
{
    public class ChangeCollection
    {
        public List<ChangeGroup> AllChangeGroups { get; set; }

        public ChangeCollection()
        {
            this.AllChangeGroups = new List<ChangeGroup>();
        }

        public void AddGroup(string name, string untieredTier = "")
        {
            this.AllChangeGroups.Add(new ChangeGroup(name, untieredTier));
        }

        public void AddChange(string group, string ident, string value)
        {
            if (value == null)
            {
                value = "*untiered*";
            }

            this.AllChangeGroups.Where(i => i.Name == group).Single().ModifyChange(ident, value);
        }

        public void AddInitials(string group, string ident, string value)
        {
            this.AllChangeGroups.Where(i => i.Name == group).Single().AddOrResetInitialValues(ident, value);
        }

        public string GetAllChanges(bool redditFormatting)
        {
            redditFormatting = true;

            string result = "";
            foreach (var i in AllChangeGroups)
            {
                result += "\r\n";
                result += "[ **CHANGES IN " + i.Name.ToUpper() + "** ]";
                result += "\r\n\r\n";

                if (redditFormatting)
                {
                    result += "[BaseType]|[Old Tier]|[New Tier]";
                    result += "\r\n";
                    result += ":-:|:-:|:-:";
                    result += "\r\n";
                }

                List<string> r;
                if (redditFormatting)
                {
                    r = i.GetAllChanges(true);
                }
                else
                {
                    r = i.GetAllChanges(false);
                }

                foreach (var str in r)
                {
                    result += str;
                }
            }
            return result;
        }
    }
}
