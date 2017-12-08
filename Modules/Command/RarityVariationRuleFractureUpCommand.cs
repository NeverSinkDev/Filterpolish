using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules.Command
{
    public class RarityVariationRuleFractureUpCommand : ICommand
    {

    public Entry e { get; set; }

    public void Execute()
    {
        var entry = e.Filter.CopyEntry(e);
        bool commented = false;

        if (e.Lines[0].Raw[0] == '#')
        {
            commented = true;
            entry.Lines.ForEach(x => x.Uncomment());
                e.Lines.ForEach(x => x.Uncomment());
        }

        e.ModifyAttribute2("SetTextColor", change: "25 95 235 255", com: "# TEXTCOLOR: Magic Items: Strong Highlight");
        e.ModifyAttribute2("Rarity", op: "=", change: "Magic");

        entry.ModifyAttribute2("SetTextColor", change: "255 255 255 255", com: "# TEXTCOLOR:	 Normal Items: Strong Highlight");
        entry.ModifyAttribute2("Rarity", op: "=", change: "Normal");

            if (commented)
            {
                foreach (var l in entry.Lines)
                {
                    l.Intro = l.Intro.Contains("#") ? l.Intro : "#" + l.Intro;
                    l.RebuildLine(true);
                    l.Identify();
                }

                foreach (var l in e.Lines)
                {
                    l.Intro = l.Intro.Contains("#") ? l.Intro : "#" + l.Intro;
                    l.RebuildLine(true);
                    l.Identify();
                }
            }

        foreach (Line l in e.Lines)
        {
            l.CalculateLinePriority();
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
