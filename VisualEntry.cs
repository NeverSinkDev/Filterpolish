using Newtonsoft.Json.Linq;
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
            // Get á line with a fitting identifier
            Line line = e.GetLine(this.identifier);
            int similarityTest = -1;

            // Test if the line in the tested entry exists in the VisualEntry
            similarityTest = this.FindLineSimilarities(line, this.newEntryMarker);
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
                        this.Lines.Last().Comment = " # " + this.commentIdentifierDescription + " " + this.Lines.Count + " " + this.newEntryMarker;
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
                    MessageBox.Show("Multipple stable Style-Comments. Memory: " + this.Lines[this.GetLineIndex(l)].Comment + "Line:" + l.Comment);
                    return;
                }
            }
        }

        /// <summary>
        /// Improves the VisualEntry, by adjusting numbers
        /// </summary>
        public void OptimizeEntry()
        {
            int m = 0;
            foreach (Line l in this.Lines)
            {
                if (l.Comment.Contains(newEntryMarker))
                {
                    l.Comment = " # " + this.commentIdentifierDescription + " " + m + " " + this.newEntryMarker;
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
