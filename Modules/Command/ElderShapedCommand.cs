using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Modules.Command
{
    class ElderShapedCommand : ICommand
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

            e.Lines.RemoveAll(x => x.Identifier == "ShaperItem" || x.Identifier == "ElderItem");
            entry.Lines.RemoveAll(x => x.Identifier == "ShaperItem" || x.Identifier == "ElderItem");

            e.ModifyAttribute2("ShaperItem", change: "True");
            entry.ModifyAttribute2("ElderItem", change: "True");

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
