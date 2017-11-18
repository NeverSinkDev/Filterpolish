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

            if (entry.Lines[0].Raw[0] == '#')
            {
                commented = true;
                entry.Lines.ForEach(x => x.Uncomment());
            }

            entry.ModifyAttribute2("SetTextColor", change: "255 190 0", com: "# TEXTCOLOR:	 Rares - Level - 75+");
            entry.ModifyAttribute2("ItemLevel", op: ">=", change: "75");

            if (commented)
            {
                foreach (var l in entry.Lines)
                {
                    l.Intro = l.Intro.Contains("#") ? l.Intro : "#" + l.Intro;
                    l.RebuildLine(true);
                    l.Identify();
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
