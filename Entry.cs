using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class Entry
    {
        public string Raw;
        public List<Line> Lines;
        public int Type = 0;
        public int N = 0;
        public List<string> BuildTags = new List<string>();

        public Entry()
        {
            this.Lines = new List<Line>();
        }

        public Entry(List<Line> Lines)
        {
            this.Lines = new List<Line>(Lines);
        }

        public Entry(List<Line> Lines, int start, int length)
        {
            N = start;
            this.Lines = new List<Line>();
            for (int n = start; n <= length; n++)
                this.Lines.Add(Lines[n]);
        }

        public void ModifyAttribute(string mod, string op, string change)
        {
            foreach (Line l in this.Lines)
            {
                if (l.Identifier == mod)
                {
                    l.Oper = op;
                    l.Value = change;
                }
            }
        }

        public void FindFirstVersionTag()
        {
            if (this.Type == 1 || this.Type == 2)
            {
                if (this.Lines[0].BuildTags.Count > 0)
                { 
                BuildTags.Add(this.Lines[0].BuildTags.First());
                }
            }
        }

        public bool HandleVersionTags(int strictness)
        {
            bool changed = false;
            if(this.BuildTags.Count > 0)
            {
                foreach (string Tag in BuildTags)
                {
                    string innertag = ReturnTagIfApplies(strictness, Tag);
                    if(innertag.Length > 0)
                    {
                        HandleInnerTag(innertag);
                        changed = true;
                    }
                }
            }
            return changed;
        }

        public void HandleInnerTag(string tag)
        {
            if (tag=="D")
            {
                this.DisableEntry();
            }
            else if (tag=="H")
            {
                this.SwitchToHide();
                if (this.Lines.Any(i => i.Identifier == "PlayAlertSound"))
                {
                    this.Lines.Remove(Lines.Single(s => s.Identifier == "PlayAlertSound"));
                }
            }
            else if (tag=="HB")
            {
                this.Lines.Remove(Lines.Single(s => s.Identifier == "SetBackgroundColor"));
            }
            else if (tag=="HBR")
            {
                this.Lines.Where(l => l.Identifier == "SetBackgroundColor").ToList().ForEach(l => l.ChangeValueAndApplyToRaw(4,"200"));
            }
        }

        public string ReturnTagIfApplies(int strictness, string tag)
        {
            int index = tag.IndexOfAny(new char[]{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (index>=0)
            {
                int m = Int32.Parse(tag.Substring(index, 1));
                if (strictness >= m)
                {
                    if (strictness > m)
                    {
                        if (tag.Substring(0, index) == "HB")
                        {
                            return ("D");
                        }
                    }
                    return tag.Substring(0, index);
                }
            }
            return "";
        }

        public void ModifyAttribute(string mod, string change)
        {

        }

        public void EntryModifyOrAddLine(string mod, string op, List<string> change)
        {

        }

        public void ModifyAttributeRemoveLine(string mod, string op, List<string> change)
        {

        }

        public void DisableEntry()
        {
            foreach (Line l in this.Lines)
            {
                l.Raw = l.Raw.Insert(0,"#");
                l.Identify();
                l.RebuildLine();
            }
        }

        public void SwitchToHide()
        {
            if (this.Type == 1)
            {
                this.Lines[0].Identifier = "Hide";
                this.Lines[0].RebuildLine(true);
                this.Type = 2;
            }
        }

        public void EnableEntry()
        {
            
        }

        public void RemoveAllTags()
        {

        }

        public void AddTagToList()
        {

        }

        public void CheckEntryByLineFilter(string mod, string op, List<List<string>> change)
        {
            
        }

        public static Entry operator +(Entry a, Line b)
        {
            a.Lines.Add(b);
            return a;
        }

        public void SortEntry()
        {
            this.Lines = this.Lines.OrderBy(l => l.LinePriority).ToList();
        }

        public void SetType(string setTo)
        {
            if (setTo == "show") { Type = 1; return; }
            else if (setTo == "Show") { Type = 1; return; }
            else if (setTo == "hide") { Type = 2; return; }
            else if (setTo == "Hide") { Type = 2; return; }
            else if (setTo == "comment") { Type = 3; return; }
            else if (setTo == "Comment") { Type = 3; return; }
            else if (setTo == "filler") { Type = 4; return; }
            else if (setTo == "Filler") { Type = 4; return; };
        }

        public string getType()
        {
            if (Type == 0) { return "Undefined"; }
            if (Type == 1) { return "Show"; }
            if (Type == 2) { return "Hide"; }
            if (Type == 3) { return "Comment"; }
            if (Type == 4) { return "Filler"; }
            return "error";
        }
    }
}
