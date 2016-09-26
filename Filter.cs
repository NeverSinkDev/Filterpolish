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
    class Filter
    {
        bool _loaded = false;
        public string RawFilter = "";
        public string RawFilterRebuilt = "";
        public List<Entry> EntryList = new List<Entry>();
        public List<Line> LineList = new List<Line>();
        public Form1 form1;
        public FilterSettings settings;


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
                    this.EntryList.Add(new Entry());
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
                    this.EntryList.Add(new Entry());
                    this.EntryList.Last().SetType(l.TypeLine);
                    this.EntryList.Last().Lines.Add(l);
                }

                //HANDLE NEW ENTRIES
                if (l.TypeLine == "Show" || l.TypeLine == "Hide")
                {
                    this.EntryList.Add(new Entry());
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

        /// <summary>
        /// Reads a filter from file
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool ReadLines(Form1 form)
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
            form1.control_ts_label3("PARSED:" + parsed.ToString() + "//");
            AddFilterProgressToLogBox("Initial line processing done. Lines: " + parsed.ToString());
            return true;
        }

        /// <summary>
        /// Reads a filter from file
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool ReadLinesWithoutOutput(Form1 form)
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

        public void AdjustVersionNumber()
        {
            AddFilterProgressToLogBox("Adjusting version number...");
            if (this.EntryList[0].getType() == "Comment")
            {
                this.EntryList[0].Lines[2].Raw = "# VERSION:\t" + this.settings.versionNumber;
            }
        }

        public void AdjustVersionName()
        {
            AddFilterProgressToLogBox("Adjusting version name...");
            if (this.EntryList[0].getType() == "Comment")
            {
                this.EntryList[0].Lines[3].Raw = "# TYPE:\t\t" + this.settings.subVersionName;
            }
        }

        public void AddFilterProgressToLogBox(string line)
        {
            if (this.settings.subVersionName == null || this.settings.versionNumber == null)
            {
                this.settings.subVersionName = "UNNAMED";
                this.settings.versionNumber = "X";
            }
            form1.AddTextToLogBox("FLOG: " + this.settings.subVersionName + " " + this.settings.versionNumber + " :" + line);
        }

        public void SaveToFile()
        {
            this.AddFilterProgressToLogBox("Creating filter file...");
            System.IO.File.WriteAllText(@"C:\FilterOutput\"
                                            + "NeverSink's filter " + this.settings.versionNumber + " - "
                                                                    + this.settings.subVersionName + ".filter",
                                                                    this.RawFilterRebuilt);
            this.AddFilterProgressToLogBox("Done! Size:" + RawFilterRebuilt.Length);
        }
    }

}
