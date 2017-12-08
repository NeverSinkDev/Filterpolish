using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using FilterPolish.Modules.Command;

namespace FilterPolish
{
    [Serializable]
    public class Entry
    {
        public string Raw;
        public List<Line> Lines = new List<Line>();
        public int Type = 0;
        public int N = 0;
        public int id = 0;
        public List<string> BuildTags = new List<string>();

        public Filter Filter { get; set; }

        public Entry()
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Entry(Filter f)
        {
            this.Lines = new List<Line>();
            this.Filter = f;
        }

        /// <summary>
        /// Construct Entry with a list of Lines
        /// </summary>
        /// <param name="Lines"></param>
        public Entry(List<Line> Lines)
        {
            this.Lines = new List<Line>(Lines);
        }

        /// <summary>
        /// Construct from a list of Lines, but only using a certain subsection
        /// </summary>
        /// <param name="Lines"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public Entry(List<Line> Lines, int start, int length)
        {
            N = start;
            this.Lines = new List<Line>();
            for (int n = start; n <= length; n++)
                this.Lines.Add(Lines[n]);
        }

        /// <summary>
        /// Modify thea attribute every line with a specific identifier
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public List<Line> ModifyAttribute(string mod, string op = "default", string change = "default", string com = "default")
        {
            List<Line> changedLines = new List<Line>();
            for(int n = 0; n < this.Lines.Count; n++)
            {
                bool changes = false;
                if (this.Lines[n].Identifier == mod)
                {
                    if (op != "default")
                    {
                        changes = true;
                        this.Lines[n].Oper = op;
                    }

                    if (change != "default")
                    {
                        changes = true;
                        string comment = this.Lines[n].Comment;
                        this.Lines[n].Raw = change + comment;
                        this.Lines[n].Identify();
                    }

                    if (com != "default")
                    {
                        changes = true;
                        this.Lines[n].Comment = com;
                    }
                    
                    if (changes == true) {
                        this.Lines[n].UpdateRaw();
                        changedLines.Add(this.Lines[n]);
                    };
                }
            }
            return changedLines;
        }

        /// <summary>
        /// Modify thea attribute every line with a specific identifier
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public void ModifyAttribute2(string mod, string op = "default", string change = "default", string com = "default")
        {
            bool changes = false;
            List<Line> changedLines = new List<Line>();
            for (int n = 0; n < this.Lines.Count; n++)
            {
                changes = false;
                if (this.Lines[n].Identifier == mod)
                {
                    if (op != "default")
                    {
                        changes = true;
                        this.Lines[n].Oper = op;
                    }

                    if (change != "default")
                    {
                        changes = true;
                        this.Lines[n].Value = change;
                    }

                    if (com != "default")
                    {
                        changes = true;
                        this.Lines[n].Comment = com;
                    }

                    if (changes == true)
                    {
                        this.Lines[n].RebuildLine(true);
                        this.Lines[n].Identify();
                        break;
                    };
                }
            }

            if (changes == false)
            {
                Line l = new Line(mod + " " + (op != "default" ? op + " " : "") + (change != "default" ? change + " " : "") + (com != "default" ? com + " " : ""));
                l.Identify();
                this.Lines.Add(l);
            }
        }

        public void ModifyAttributeSimple(string mod, string change)
        {
            List<Line> changedLines = new List<Line>();
            for (int n = 0; n < this.Lines.Count; n++)
            {
                bool changes = false;
                if (this.Lines[n].Identifier == mod)
                {
                    if (change != "")
                    {
                        changes = true;
                        string comment = this.Lines[n].Comment;
                        this.Lines[n].Raw = mod + " " + change + comment;
                        this.Lines[n].Identify();
                    }
                    else
                    {
                        changes = true;
                        string comment = this.Lines[n].Comment;
                        this.Lines[n].Raw = mod + comment;
                        this.Lines[n].Identify();
                        MessageBox.Show("WARNING: NO PARAMS!"); 
                    }

                    if (changes == true)
                    {
                        this.Lines[n].UpdateRaw();
                        changedLines.Add(this.Lines[n]);
                    };
                }
            }
        }

        /// <summary>
        /// Get the first version tag of an entry
        /// </summary>
        public void FindFirstVersionTag()
        {
            if (this.Type == 1 || this.Type == 2)
            {
                if (this.Lines[0].BuildTags.Count > 0)
                {
                    foreach (string s in this.Lines[0].BuildTags)
                    {
                        BuildTags.Add(s);
                    }
                }
            }
        }

        /// <summary>
        /// Get the first version tag of an entry
        /// </summary>
        public void FindAllVersionTag()
        {
            if (this.Type == 1 || this.Type == 2)
            {
                if (this.Lines[0].BuildTags.Count > 0)
                {
                    foreach (string s in this.Lines[0].BuildTags)
                    {
                        BuildTags.Add(s);
                    }
                }
            }
        }

        public bool FullMatch(object item)
        {
            if (this.getType() == "Show" || this.getType() == "Hide")
            {

            }

            return false;
        }

        /// <summary>
        /// Foreach loop that handles all version tags in the entry
        /// </summary>
        /// <param name="strictness"></param>
        /// <returns></returns>
        public bool HandleVersionTags(int strictness)
        {
            bool changed = false;
            if(this.BuildTags.Count > 0)
            {
                foreach (string tag in BuildTags)
                {
                    if (tag == "%UP")
                    {
                        var c = new EntryFractureUpCommand();
                        c.e = this;
                        this.Filter.CommandList.Add(c);
                        continue;
                    }

                    if (tag == "%SHELDER")
                    {
                        var c = new ElderShapedCommand();
                        c.e = this;
                        this.Filter.CommandList.Add(c);
                        continue;
                    }

                    if (tag == "%RVR")
                    {
                        var c = new RarityVariationRuleFractureUpCommand();
                        c.e = this;
                        this.Filter.CommandList.Add(c);
                        continue;
                    }

                    string innertag = ReturnTagIfApplies(strictness, tag);
                    if(innertag.Length > 0)
                    {
                        HandleInnerTag(innertag);
                        changed = true;
                    }
                }
            }
            return changed;
        }

        /// <summary>
        /// Defines the action to be performed with every tag. Should be moved to it's own class if the vtag system becomes more complex
        /// </summary>
        /// <param name="tag"></param>
        public void HandleInnerTag(string tag)
        {
            if (tag=="%D")
            {
                this.DisableEntry();
            }
            else if (tag=="%H")
            {
                this.SwitchToHide();
                if (this.Lines.Any(i => i.Identifier == "PlayAlertSound"))
                {
                    this.Lines.Remove(Lines.Single(s => s.Identifier == "PlayAlertSound"));
                }
            }
            else if (tag=="%HB")
            {
                this.Lines.Remove(Lines.Single(s => s.Identifier == "SetBackgroundColor"));
            }
            else if (tag == "%RF")
            {
                this.Lines.Where(l => l.Identifier == "SetFontSize").ToList().ForEach(l => l.ChangeValueAndApplyToRaw(1,"36"));
            }
            else if (tag == "%HS")
            {
                this.Lines.Remove(Lines.Single(s => s.Identifier == "PlayAlertSound"));
            }
            else if (tag=="%HBR")
            {
                this.Lines.Where(l => l.Identifier == "SetBackgroundColor").ToList().ForEach(l => l.ChangeValueAndApplyToRaw(4,"200"));
            }
        }

        /// <summary>
        /// Wow. I did a booboo, that's some fugly code. Anyway, this thing tests if the strictness value applies to the current filter
        /// </summary>
        /// <param name="strictness"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
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
                        if (tag.Substring(0, index) == "%HB")
                        {
                            return ("%D");
                        }

                        if (tag.Substring(0, index) == "%HS")
                        {
                            return ("%H");
                        }
                    }
                    return tag.Substring(0, index);
                }
            }
            return "";
        }

        /// <summary>
        /// Removes all matching lines
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public void RemoveLines(Line line)
        {
                this.Lines.RemoveAll(l => l.CompareLine(line) > 0);
        }
        /// <summary>
        /// Removes matching lines that also match in their comment
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public void RemoveLinesFullMatch(Line line)
        {
                this.Lines.RemoveAll(l => l.CompareLine(line) == 3);
        }

        /// <summary>
        /// Removes all matching lines that contain a certain common comment Tag
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public void RemoveLineWithCommentTag(Line line, string removeTag)
        {
            if (removeTag != null)
            {
                this.Lines.RemoveAll(l => l.CompareLine(line, removeTag) == 2);
            }
        }

        /// <summary>
        /// Removes all matching lines containing a Tag or matching (including Comments)
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public void RemoveLineWithCommentTagOrMatch(Line line, string removeTag)
        {
            if (removeTag != null)
            {
                this.Lines.RemoveAll(l => l.CompareLine(line, removeTag) >= 2);
            }
        }


        /// <summary>
        /// Checks if a line with the requested identifier exists
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public bool Any(string ident)
        {
            if (this.Lines != null && this.Lines.Count >= 0)
            {
                return this.Lines.Any(i => i.Identifier == ident);
            }
            else return false;
        }

        /// <summary>
        /// Test if there are similar lines in the entry:
        /// -1: Error
        /// 0: The line is Unique
        /// 1   : line has same params, line: NEW, this: ?
        /// 2   : line has same params, comments are the same, no tags
        /// 3   : line has same params, line: STABLE, this: NEW
        /// 4   : line has same params, line: STABLE, this: STABLE
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public int FindLineSimilarities(Line line, string commentTag = "")
        {
            // NO INFORMATION ABOUT THE LINE, RETURN NOPE
            if (line == null)
            {
                return -1;
            }

            // TEST IF THE ENTRY IS EMPTY - THE LINE iS AUTOMATAICALLY UNIQUE IF IT IS
            if ( this.Lines.Count == 0)
            {
                return 0;
            }

            // NOW THEN: LET'S GET EVERY LINE WITH THE SAME IDENTIFIER
            List<Line> lines = this.GetLines(line.Identifier);
            if( lines == null )
            {
                return -1;
            }

            // LETS PARSE ALL LINES. WE'RE COMPARING THE PAIRS, UNTIL WE MANAGE TO FIND A MATCH
            foreach (Line l in lines)
            {
                int result = l.CompareLine(line, commentTag);
                if (result >= 1)
                {
                    return result;
                }
            }
            // Case: Line is unique.
            return 0;
            
        }

        internal void RemoveAnyTags()
        {
            if (this.getType() == "Show" || this.getType() == "Hide")
            {
                this.Lines[0].Raw = this.Lines[0].Raw.Substring(0,this.Lines[0].Raw.LastIndexOf("#"));
                this.Lines[0].Identify();
            }
        }

        public Line FindLineWithSameComment(Line line)
        {
            Line newLine = null;

            if (line == null)
            {
                return newLine;
            }

            if (this.Lines.Count == 0)
            {
                return newLine;
            }

            foreach(Line l in this.Lines)
            {
                if (l.Identifier == line.Identifier)
                {
                    if (l.Comment == line.Comment)
                    {
                        return l;
                    }
                }
            }
            return newLine;
        }


        /// <summary>
        /// Returns the first Line with the requested identifier
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public Line GetLine(string ident)
        {
            if (this.Any(ident))
            { return this.Lines.Find(i => i.Identifier == ident); }
            return null;
        }

        /// <summary>
        /// Returns the first Line number with the requested identifier and params
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public int GetLineIndex(Line line)
        {
            foreach (Line l in this.Lines)
            {
                if (line != null)
                {
                    if (line.CompareLine(l) > 0)
                    {
                        return this.Lines.IndexOf(l);
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the commend of the first Line with the requested identifier and params
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public string GetLineComment(Line line)
        {
            foreach (Line l in this.Lines)
            {
                if (line != null)
                {
                    if (line.CompareLine(l) > 0)
                    {
                        return l.Comment;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Returns the commend of the first Line with the requested identifier and params
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public Line GetLineByFullString(string attributes)
        {
            foreach (Line l in this.Lines)
            {
                if (String.Join(" ",l.Attributes) == attributes)
                    {
                        return l;
                    }
            }
            return null;
        }

        
        /// <summary>
        /// Returns a list of lines, with the request identifier
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="op"></param>
        /// <param name="change"></param>
        public List<Line> GetLines(string ident)
        {
            if (this.Any(ident))
            { return this.Lines.FindAll(i => i.Identifier == ident); }
            return null;
        }

        /// <summary>
        /// comments the entry out
        /// </summary>
        public void DisableEntry()
        {
            foreach (Line l in this.Lines)
            {
                l.Raw = l.Raw.Insert(0,"#");
                l.Identify();
                l.RebuildLine();
            }
        }

        /// <summary>
        /// switches the "Show" of an entry to "Hide"
        /// </summary>
        public void SwitchToHide()
        {
            if (this.Type == 1)
            {
                this.Lines[0].Identifier = "Hide";
                this.Lines[0].RebuildLine(true);
                this.Type = 2;
            }
        }

        /// <summary>
        /// Sorts all lines in an entry, by their weighting
        /// </summary>
        public void SortEntry()
        {
            this.Lines = this.Lines.OrderBy(l => l.LinePriority).ToList();
        }

        /// <summary>
        /// Sets the type of an entry, can be "Show", "Hide", "Comment" and "Filler". Ugly method
        /// </summary>
        /// <param name="setTo"></param>
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

        /// <summary>
        /// Returns the Entrytype.
        /// </summary>
        /// <returns></returns>
        public string getType()
        {
            if (Type == 0) { return "Undefined"; }
            if (Type == 1) { return "Show"; }
            if (Type == 2) { return "Hide"; }
            if (Type == 3) { return "Comment"; }
            if (Type == 4) { return "Filler"; }
            if (Type == 5) { return "TestItem"; }
            return "error";
        }
    }
}
