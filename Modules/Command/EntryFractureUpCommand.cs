using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules.Command
{
    public class EntryFractureUpCommand : ICommand
    {
        public Entry e { get; set; }

        public void Execute()
        {
            var entry = e.Filter.CopyEntry(e);
            bool commented = false;

            if (e.Lines[0].Raw[0] == '#')
            {
                return;
            }

                //entry.ModifyAttribute("ItemLevel", op: ">=", change: "75");
            entry.ModifyAttribute2("SetTextColor", change: "255 190 0", com: "# TEXTCOLOR:	 Rares - Level - 75+");
            entry.ModifyAttribute2("ItemLevel", op: ">=", change: "75");

            if (commented)
            {
                foreach (var l in entry.Lines)
                {
                    if (l.Raw[0] != '#')
                    {
                        l.Raw = l.Raw.Insert(0, "#");
                        l.Identify();
                        l.RebuildLine();
                    }
                }
            }

            e.Filter.InsertNewEntry(entry, e);

            foreach (Line l in entry.Lines)
            {
                l.CalculateLinePriority();
            }

            e.RemoveAnyTags();

        }
    }
}
