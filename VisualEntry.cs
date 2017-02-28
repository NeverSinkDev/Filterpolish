//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterPolish
{
    public class VisualEntry : Entry
    {
        public string identifier;
        public string newEntryMarker;
        public string commentIdentifierDescription;
        public string commentSuffix;

        public bool configuration_RelabelAll;
        public bool configuration_AcceptNonUnique;
        public bool configuration_LabelNewStyles = false;

        public VisualEntry( string id,  string commentIdent, string commentSuf, string newEntry = "<NEW>")
        {
            this.identifier = id;
            this.newEntryMarker = newEntry;
            this.commentIdentifierDescription = commentIdent;
            this.commentSuffix = commentSuf;
        }

        /// <summary>
        /// Takes an entry and looks for fitting lines inside.
        /// </summary>
        /// <param name="e"></param>
        public void AddStylesFromEntryToList(Entry e)
        {
            // Get a line with a fitting identifier
            Line line = e.GetLine(this.identifier);

            // Now here's the tricky part. The similarity test is 
            // our way of figuring out what actions to perform, depending upon the following factors:
            // Based upon the comparison between "this" to the entry in the filter "e"...
            int similarityTest = -1;

            // Test if the line in the tested entry exists in the VisualEntry
            similarityTest = this.FindLineSimilarities(line, this.newEntryMarker);
            
            // 0   The line we've found is new to this entry
            // 1&2 We've already recorded an instance of this line
            // 3   We've previously recorded an instance of the line. However, it didn't have a tag yet.
            // 4   We've recorded 2 different instances of the line, both having different tags. 
            //     This can be handled differently, depending on automation and style error tolerance
            this.PerformInnerStyleOperations(e, line, similarityTest);

        }

        /// <summary>
        /// Performs actions on an entry, depending upon the similarity level of the lines.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="l"></param>
        /// <param name="situation"></param>
        public void PerformInnerStyleOperations(Entry e, Line l, int situation)
        {
            // Found the line in the visual entry
            if (situation > -1 || this.configuration_AcceptNonUnique)
            {
                // Line is absolutly new or has tag
                if (situation == 0)
                {
                    this.Lines.Add(l);
                    if (this.configuration_LabelNewStyles)
                    {
                        if (l.Comment != null && !l.Comment.Contains(this.newEntryMarker) && l.Comment.Trim().Length != 0)
                        {
                            this.Lines.Last().Comment = l.Comment;
                        }
                        else
                        {
                            this.Lines.Last().Comment = " # " + this.commentIdentifierDescription + " " + this.Lines.Count + " " + this.newEntryMarker;
                        }
                    }
                    return;
                }

                // Line found is NEW, but already indexed or exactly the same (and indexed), nothing to do here
                if (situation == 1 || situation == 2)
                {
                    return;
                }

                if (situation == 3)
                {
                    int index = this.GetLineIndex(l);
                    this.Lines[index].Comment = l.Comment;
                    return;
                }

                if (situation == 4)
                {
                    // TODO: This should be handled differently. Next time *whispers* Next tiiime.
                    int index = this.GetLineIndex(l);
                    this.Lines[index].Comment = l.Comment;
                    return;
                }
            }
        }

        /// <summary>
        /// Improves the VisualEntry. This method improves the numbering of new lines
        /// It also trims the comments to prevent silly problems.
        /// </summary>
        public void OptimizeEntry()
        {
            int m = 0;
            foreach (Line l in this.Lines)
            {
                if (l.Comment.Contains(newEntryMarker))
                {
                    l.Comment = " # " + this.commentIdentifierDescription + " " + m + " " + this.newEntryMarker;
                    l.Comment.Trim();
                    m++;
                }
            }
        }

        /// <summary>
        /// Sorts the visual entry by colors.
        /// </summary>
        public void SortByRGB()
        {
            this.Lines = this.Lines.OrderBy(c => c.GetARGB_Hue()).ThenBy(c => c.GetARGB_Brightness()).ToList();
        }
    }
}
