using FilterPolish.Modules.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterPolish
{
    public class Filter
    {
        bool _loaded = false;
        public string RawFilter = "";
        public string RawFilterRebuilt = "";
        public List<Entry> EntryList = new List<Entry>();
        public List<Line> LineList = new List<Line>();
        public FilterPolish form1;
        public FilterSettings settings;
        public StyleSheet CurrentStyle;
        public StyleSheet ImportedStyle;
        public TableOfContentsEntry TOC;

        public List<ICommand> CommandList { get; set; }

        char[] _delimiterChars = { ' ', '\t' };

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="input"></param>
        public Filter(string input)
        {
            RawFilter = input;
            _loaded = true;
        }

        /// <summary>
        /// Copy From Constructor
        /// </summary>
        /// <param name="copyFromFilter"></param>
        public Filter(Filter copyFromFilter)
        {
            Filter filter = new Filter(copyFromFilter);
            filter.ReadLines(copyFromFilter.form1);
        }

        /// <summary>
        /// Strings from file + Settings
        /// </summary>
        /// <param name="copyFromFilter"></param>
        public Filter(string input, FilterSettings set)
        {
            RawFilter = input;       
            this.settings = set;
            _loaded = true;
        }

        /// <summary>
        /// Generates the Entrylist for the current filter
        /// </summary>
        public void GenerateEntries()
        {
            this.EntryList.Clear();
            int n = 0;
            string lastEntry = "";
            string lastLine = "";
            foreach (Line l in LineList)
            {

                //CHECK LASTLINE+LASTENTRY
                if (EntryList.Count >= 1)
                {
                    lastEntry = EntryList.Last().getType();

                    if (EntryList.Last().Lines.Count >= 1)
                    {
                        lastLine = this.EntryList.Last().Lines.Last().TypeLine;
                    }
                }

                //HANDLE FILLERS
                if (l.TypeLine == "Filler" && lastLine != "Filler")
                {
                    this.EntryList.Add(new Entry(this));
                    this.EntryList.Last().SetType(l.TypeLine);
                    this.EntryList.Last().Lines.Add(l);
                }

                //HANDLE COMMENTS
                if (l.TypeLine == "Comment" && lastEntry == "Comment")
                {
                    this.EntryList.Last().Lines.Add(l);
                }

                if ((l.TypeLine == "Comment") && lastEntry != "Comment")
                {
                    this.EntryList.Add(new Entry(this));
                    this.EntryList.Last().SetType(l.TypeLine);
                    this.EntryList.Last().Lines.Add(l);
                }

                //HANDLE NEW ENTRIES
                if (l.TypeLine == "Show" || l.TypeLine == "Hide")
                {
                    this.EntryList.Add(new Entry(this));
                    this.EntryList.Last().id = n;
                    n++;
                    this.EntryList.Last().SetType(l.TypeLine);
                    this.EntryList.Last().Lines.Add(l);
                }

                //HANDLE ENTRY DATA
                if ((l.TypeLine == "AttributeVisual" || l.TypeLine == "AttributeClass") && (lastEntry == "Show" || lastEntry == "Hide"))
                {
                    this.EntryList.Last().Lines.Add(l);
                }
            }
            AddFilterProgressToLogBox("Entries generated " + this.EntryList.Count.ToString());
        }

        //public Entry MatchItem(Item i)
        //{
        //    for (int n=0; n<EntryList.Count; n++)
        //    {
        //        if (EntryList[n].getType() == "Show" || EntryList[n].getType() == "Hide")
        //        {

        //        }
        //    }
        //}

        /// <summary>
        /// Reads a filter from file
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool ReadLines(FilterPolish form)
        {
            this.form1 = form;
            int parsed = 0;
            using (StringReader reader = new StringReader(RawFilter))
            {
                string stringLine = "";
                this.LineList.Clear();
                while ((stringLine = reader.ReadLine()) != null)
                {
                    Line line = new Line(stringLine);

                    if (line.Identify() == "ERROR")
                    {
                        LineList.Add(line);
                        MessageBox.Show("ERROR in line: " + (parsed + 1).ToString() + " Raw: " + line.Raw);
                    }
                    else
                    {
                        LineList.Add(line);
                    }
                    parsed++;

                }
            }
            form1.control_ts_label3("PARSED:" + parsed.ToString() + "//");
            AddFilterProgressToLogBox("Initial line processing done. Lines: " + parsed.ToString());
            return true;
        }

        public Entry CopyEntry(Entry e)
        {
            Entry e2 = new Entry(e.Filter);
            foreach(var line in e.Lines)
            {
                line.RebuildLine(true);

                var newLine = new Line(line.Raw);
                newLine.Identify();
                e2.Lines.Add(newLine);
            }

            e2.Type = e.Type;

            return e2;
        }

        public void InsertNewEntry(Entry e, Entry above)
        {
            Entry emptyEntry = new Entry(this);
            Line l = new Line("\r\n");
            l.Identify();
            l.RebuildLine(true);
            emptyEntry.Lines.Add(l);

            this.EntryList.Insert(this.EntryList.IndexOf(above), e);
            this.EntryList.Insert(this.EntryList.IndexOf(above),emptyEntry);
        }

        /// <summary>
        /// Reads a filter from file
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool ReadLinesWithoutOutput(FilterPolish form)
        {
            this.form1 = form;
            int parsed = 0;
            using (StringReader reader = new StringReader(RawFilter))
            {
                string stringLine = "";
                while ((stringLine = reader.ReadLine()) != null)
                {
                    Line line = new Line(stringLine);

                    if (line.Identify() == "ERROR")
                    {
                        LineList.Add(line);
                        MessageBox.Show("ERROR in line: " + (parsed + 1).ToString() + " Raw: " + line.Raw);
                    }
                    else
                    {
                        LineList.Add(line);
                    }
                    parsed++;

                }
            }
            return true;
        }

        /// <summary>
        /// DEBUG: Displays EntryDebug and LineDebug infos.
        /// Rebuilds the filter and entry list. Requires ReadLines/GenerateEntryList first
        /// </summary>
        /// <returns></returns>
        public bool RebuildFilterAndEntriesDebug()
        {
            RawFilterRebuilt = "";
            int i = 0;
            foreach (Entry e in this.EntryList)
            {

                foreach (Line s in e.Lines)
                {
                    RawFilterRebuilt += (i + "\t" + e.getType() + "\t\t" + s.RebuildLineTypeDebug());
                    RawFilterRebuilt += "\r\n";
                }
                i++;
            }
            return true;
        }

        /// <summary>
        /// Rebuilds the filter and entry list. Requires ReadLines/GenerateEntryList first
        /// </summary>
        /// <returns></returns>
        public bool RebuildFilterFromEntries(bool RebuildLines = true)
        {
            AddFilterProgressToLogBox("Rebuilding filter...!");
            RawFilterRebuilt = "";
            int i = 0;
            foreach (Entry e in this.EntryList)
            {

                foreach (Line s in e.Lines)
                {
                    if (RebuildLines)
                    { 
                        s.Identify();
                    }

                    RawFilterRebuilt += (s.RebuildLine());
                    RawFilterRebuilt += "\r\n";
                }
                i++;
            }
            AddFilterProgressToLogBox("Done rebuilding filter!");
            return true;
        }

        /// <summary>
        /// Rebuilds the filter from the LineList
        /// DEBUG: Display LineTypes, Enum numbers etc
        /// </summary>
        /// <returns></returns>
        public bool RebuildFilterDebug()
        {
            RawFilterRebuilt = "";
            foreach (Line l in LineList)
            {
                RawFilterRebuilt += l.RebuildLineTypeDebug();
                RawFilterRebuilt += "\r\n";
            }
            return true;
        }

        /// <summary>
        /// Rebuilds the filter from the LineList
        /// </summary>
        /// <returns></returns>
        public bool RebuildFilter()
        {
            RawFilterRebuilt = "";
            foreach (Line l in LineList)
            {
                RawFilterRebuilt += l.RebuildLine();
                RawFilterRebuilt += "\r\n";
            }
            return true;
        }

        /// <summary>
        /// Sorts WITHIN the entries. Requires EntryGeneration first
        /// </summary>
        /// <returns></returns>
        public bool SortEntries()
        {
            CalculateLinePriorities();
            foreach (Entry e in this.EntryList)
            {
                e.SortEntry();
            }
            AddFilterProgressToLogBox("Done sorting entries: " + this.EntryList.Count.ToString());
            return true;
        }

        /// <summary>
        /// Looks for %-marked version-tags in the entries
        /// </summary>
        /// <returns></returns>
        public bool FindAndHandleVersionTags()
        {
            this.CommandList = new List<ICommand>();
            int i = 0;
            AddFilterProgressToLogBox("Generating filterversion based on Version-Tags!");
            foreach (Entry e in this.EntryList)
            {
                e.FindFirstVersionTag();
                if (e.HandleVersionTags(this.settings.strictLevel))
                {
                    i++;
                }
            }

            foreach(var c in this.CommandList)
            {
                c.Execute();
            }

            AddFilterProgressToLogBox("Done adjusting filter based on version tags. CHANGES: " + i.ToString());
            return true;
        }

        /// <summary>
        /// Calculates the priority of each line (for sorting/filtering)
        /// </summary>
        /// <returns></returns>
        public bool CalculateLinePriorities()
        {
            foreach (Line i in this.LineList)
            {
                i.CalculateLinePriority();
                if (i.TypeLine == "ERROR")
                {
                    MessageBox.Show("ERROR in while calculating priorities: " + i.Raw);
                }
            }
            return true;
        }

        /// <summary>
        /// Label Style information
        /// </summary>
        /// <returns></returns>
        public StyleSheet GenerateStyleSheet(bool labelNewStyles)
        {
            this.CurrentStyle = new StyleSheet(this);
            this.CurrentStyle.Init();
            this.CurrentStyle.CheckFilterForStyles(labelNewStyles);
            return this.CurrentStyle;
        }

        public void RemoveTags()
        {
            foreach (var e in this.EntryList)
            {
                e.RemoveAnyTags();
            }
        }

        public void ApplyStyleSheet(StyleSheet s)
        {
            s.AppliedFilter = this;
            s.ApplyStyleSheetCommentsToData();
        }

        /// <summary>
        /// Changes the version number of the filter within the file.
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="lineNumber"></param>
        public void AdjustVersionNumber(int entryNumber, int lineNumber)
        {
            AddFilterProgressToLogBox("Adjusting version number...");
            if (this.EntryList[entryNumber].getType() == "Comment")
            {
                this.EntryList[entryNumber].Lines[lineNumber].Raw = "# VERSION:\t\t" + this.settings.versionNumber;
            }
        }

        /// <summary>
        /// Changes the subversion name of the filter within the file
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="lineNumber"></param>
        public void AdjustVersionName(int entryNumber, int lineNumber)
        {
            AddFilterProgressToLogBox("Adjusting version name...");
            if (this.EntryList[entryNumber].getType() == "Comment")
            {
                this.EntryList[entryNumber].Lines[lineNumber].Raw = "# TYPE:\t\t\t" + this.settings.subVersionName;
            }
        }

        /// <summary>
        /// Changes the subversion name of the filter within the file
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="lineNumber"></param>
        public void AdjustStyleName(int entryNumber, int lineNumber, string Text)
        {
            AddFilterProgressToLogBox("Adjusting style name...");
            if (this.EntryList[entryNumber].getType() == "Comment")
            {
                this.EntryList[entryNumber].Lines[lineNumber].Raw = "# STYLE:\t\t\t" + Text;
            }
        }

        /// <summary>
        /// Logs progression in the logbox
        /// </summary>
        /// <param name="line"></param>
        public void AddFilterProgressToLogBox(string line)
        {
            if (this.settings.subVersionName == null || this.settings.versionNumber == null)
            {
                this.settings.subVersionName = "UNNAMED";
                this.settings.versionNumber = "X";
            }
            form1.AddTextToLogBox("FLOG: " + this.settings.subVersionName + " " + this.settings.versionNumber + " :" + line);
        }

        /// <summary>
        /// Saves the filter to a file.
        /// </summary>
        public void SaveToFile()
        {
            this.AddFilterProgressToLogBox("Creating filter file...");
            System.IO.File.WriteAllText(Util.GetOutputPath()
                                            + "NeverSink's filter - "
                                                                    + this.settings.subVersionName + ".filter",
                                                                    this.RawFilterRebuilt);
            this.AddFilterProgressToLogBox("Done! Size:" + RawFilterRebuilt.Length);
        }

        public void SaveToFile(string foldersuffix, string stylename)
        {
            this.AddFilterProgressToLogBox("Creating filter file...");

            System.IO.FileInfo folder = new System.IO.FileInfo(Util.GetOutputPath() + foldersuffix + @"\");
            folder.Directory.Create();
            System.IO.File.WriteAllText(folder 
                                            + "NeverSink's filter - "
                                                                    + this.settings.subVersionName
                                                                     + " (" + stylename + ") "
                                                                    + ".filter",
                                                                    this.RawFilterRebuilt);
            this.AddFilterProgressToLogBox("Done! Size:" + RawFilterRebuilt.Length);
        }

        /// <summary>
        /// Save file to loaction
        /// </summary>
        /// <param name="dialog"></param>
        public void SaveToFile(FileDialog dialog)
        {
            this.AddFilterProgressToLogBox("Creating filter file...");
            System.IO.File.WriteAllText(dialog.FileName,this.RawFilterRebuilt);
            this.AddFilterProgressToLogBox("Done! Size:" + RawFilterRebuilt.Length);
        }

        public void GenerateTOC(bool ApplyToc = true)
        {
            this.TOC = new TableOfContentsEntry(this);
            this.TOC.RunTOCParsing(ApplyToc);
        }

        /// <summary>
        /// Gets entries that contain certain attributes
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public List<Entry> GetEntriesContaining(string attributes, string comment)
        {
            List<Entry> results = new List<Entry>();
            Line tempLine = new Line(attributes);
            tempLine.Identify();
            foreach (Entry e in this.EntryList)
            {
                if (e.FindLineSimilarities(tempLine) >= 1)
                {
                    results.Add(e);
                }
            }

            return results;
        }

   
    }

}
