using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class TableOfContentsEntry : Entry
    {
        public Filter AppliedFilter { get; set; }

        List<string> beginsigns = new List<string>();
        List<string> endsigns = new List<string>();
        List<string> sep = new List<string>();
        List<int> sectionlength = new List<int>();
        List<int> counters = new List<int>();
        string TOCHead = "";
        int tocheadindex = -1;

        string intro = "# ";

        bool ApplyToC = true;

        /// <summary>
        /// Special Entry that models the Table of Contents. Implements methods to generate, sort, number etc the headers.
        /// </summary>
        /// <param name="f"></param>
        public TableOfContentsEntry(Filter f)
        {
            this.AppliedFilter = f;
            this.TOCHead = "TABLE OF CONTENTS + QUICKJUMP TABLE";

            this.beginsigns.Add("[[");
            this.endsigns.Add("]]");
            this.sectionlength.Add(2);
            this.sep.Add("#===============================================================================================================");
            this.counters.Add(0);

            this.beginsigns.Add("[");
            this.endsigns.Add("]");
            this.sectionlength.Add(2);
            this.sep.Add("#------------------------------------");

            this.counters.Add(0);

        }

        /// <summary>
        /// Perform all TOC operations
        /// </summary>
        public void RunTOCParsing(bool ApplyToC = true)
        {
            this.ApplyToC = ApplyToC;
            this.AppliedFilter.AddFilterProgressToLogBox("Running TOC generation...");
            Line sepline = new Line(sep[0]);

            this.Lines.Clear();
            this.Lines.Add(sepline);
            this.Lines.Add(new Line("#[WELCOME]" + " " + TOCHead));
            this.Lines.Add(sepline);
            this.Lines.Add(new Line("#"));

            foreach(Line l in this.Lines)
            {
                l.Identify();
            }

            for (int n = 0; n< this.AppliedFilter.EntryList.Count;n++)
            {
                Entry e = ParseEntry(this.AppliedFilter.EntryList[n]);
                if (this.ApplyToC)
                { 
                this.AppliedFilter.EntryList[n] = e;
                }
            }

            if (tocheadindex != -1)
            {
                if (this.ApplyToC)
                { 
                this.AppliedFilter.EntryList[tocheadindex] = this;
                this.AppliedFilter.AddFilterProgressToLogBox("Applying ToC: " + this.Lines.Count + " entries!");
                }
            }
            else
            {
                this.Lines.Clear();
                this.AppliedFilter.AddFilterProgressToLogBox("ERROR: NO TOC HEAD FOUND");
            }

        }

        /// <summary>
        /// Parses a single entry for TOC contents.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AddToTOC"></param>
        /// <returns></returns>
        public Entry ParseEntry(Entry e, bool AddToTOC = true)
        {
            for (int level = 0; level < sectionlength.Count; level++)
            {
                int linenumber = IsEntryHeader(e, level);
                if ( linenumber >= 0 )
                {
                    this.AdjustCounters(level);

                    Line l = BuildNewHeader(e, linenumber, level);
                    Line sepline = new Line( sep[level] );
                    sepline.Identify();

                    if (e.Lines.Count <= 3)
                    {
                        e.Lines.Clear();
                        e.Lines.Add(sepline);
                        e.Lines.Add(l);
                        e.Lines.Add(sepline);
                    }
                    else
                    {
                        e.Lines[0] = sepline;
                        e.Lines[1] = l;
                        e.Lines[2] = sepline;
                    }

                    if (AddToTOC == true)
                    {
                        this.Lines.Add(l);
                    }

                    return e;
                }
            }
            return e;

        }

        /// <summary>
        /// Creates a new header for a TOC entry.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="linenumber"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public Line BuildNewHeader(Entry e, int linenumber, int level)
        {
            string commentcontent = e.Lines[linenumber].Comment.Substring(e.Lines[linenumber].Comment.IndexOf(endsigns[level]) + endsigns[level].Length);

            //commentcontent = (level == 0 ? commentcontent.ToUpper() : commentcontent.ToLower());

            string content = intro + (level == 1 ? "  " : "") + beginsigns[level];

            int n = 0;
            foreach (int counter in counters)
            {
                string countersection = counter.ToString();
                if (counter.ToString().Length < sectionlength[n])
                { 
                    countersection = countersection.PadLeft(sectionlength[n], '0');
                }
                content += countersection;
            }

            content += endsigns[level];
            content += commentcontent;
            Line l = new Line(content);
            l.Identify();
            return l;
        }

        /// <summary>
        /// Increments the TOC counters, based upon the entry data.
        /// </summary>
        /// <param name="level"></param>
        public void AdjustCounters(int level)
        {
            for (int c=0; c<this.counters.Count;c++)
            {
                if (level == c)
                {
                    this.counters[c] ++;
                }
                else if (c<level)
                {
                    
                }
                else if (c>level)
                {
                    this.counters[c] = 0;
                }
            }
        }

        /// <summary>
        /// Checks what kind of entry we're dealing with here.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int IsEntryHeader(Entry e, int level)
        {
            if (e.getType() != "Comment")
            {
                return -1;
            }

            foreach (Line l in e.Lines)
            {
                if (l.Comment.Contains(this.TOCHead))
                {
                    tocheadindex = AppliedFilter.EntryList.IndexOf(e);
                    return -1;
                }

                if (l.Comment.Contains(beginsigns[level]) && l.Comment.Contains(endsigns[level]))
                {
                    return e.Lines.IndexOf(l);
                }
            }
            return -1;
        }
    }
}
