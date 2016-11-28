using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterPolish
{
    public class StyleSheet
    {
        public Filter AppliedFilter { get; set; }

        public List<VisualEntry> Styles;

        VisualEntry UBackgrounds = new VisualEntry("SetBackgroundColor", "BACKGROUND:\t", "");
        VisualEntry UBorders = new VisualEntry("SetBorderColor", "BORDERCOLOR:\t", "");
        VisualEntry UTexts = new VisualEntry("SetTextColor", "TEXTCOLOR:\t", "");
        VisualEntry USounds = new VisualEntry("PlayAlertSound", "DROPSOUND:\t", "");
        VisualEntry USizes = new VisualEntry("SetFontSize", "FONTSIZE:\t", "");

        public  bool initialized = false;

        public StyleSheet(Filter filter)
        {
            this.AppliedFilter = filter;
        }

        /// <summary>
        /// Init function for StyleSheets
        /// </summary>
        public void Init()
        {
            this.Styles = new List<VisualEntry>();
            Styles.Add(UBackgrounds);
            Styles.Add(UBorders);
            Styles.Add(UTexts);
            Styles.Add(USounds);
            // Styles.Add(USizes);

            this.initialized = true;
        }

        /// <summary>
        /// Generates this Stylesheet, based on the applied filter
        /// </summary>
        public void CheckFilterForStyles(bool labelNewStyles)
        {
            this.AppliedFilter.AddFilterProgressToLogBox("Generating Stylesheet from filter...");
            foreach (VisualEntry v in this.Styles)
            {
                v.configuration_LabelNewStyles = labelNewStyles;
                foreach (Entry e in this.AppliedFilter.EntryList)
                {
                    v.AddStylesFromEntryToList(e);
                }
                v.SortByRGB();
                if (labelNewStyles)
                { 
                v.OptimizeEntry();
                }


                this.AppliedFilter.AddFilterProgressToLogBox("STYLES: " + v.commentIdentifierDescription + "COUNT: " + v.Lines.Count);
            }

        }

        /// <summary>
        /// Applies this StyleSheet to the current filter
        /// </summary>
        public void ApplyStyleSheetCommentsToData()
        {
            foreach (VisualEntry v in this.Styles)
            {
                v.OptimizeEntry();
                foreach (Entry e in this.AppliedFilter.EntryList)
                {
                    e.ModifyAttribute(v.identifier, com: v.GetLineComment(e.GetLine(v.identifier)));
                }
            }
            this.AppliedFilter.AddFilterProgressToLogBox("Applying Style Labeling to filter...");
        }

        /// <summary>
        /// POSSIBLY BROKEN
        /// </summary>
        public void ApplyStyleSheetDataToComments()
        {
            foreach (VisualEntry v in this.Styles)
            {
                foreach (Entry e in this.AppliedFilter.EntryList)
                {
                    Line l = e.GetLine(v.identifier);
                    {
                        if (v.FindLineSimilarities(l) >= 1)
                        {
                            v.ModifyAttribute(v.identifier, com: l.Comment);
                        }
                    }
                }
            }
            this.AppliedFilter.AddFilterProgressToLogBox("Applying Style Comments to filter...");
        }

        /// <summary>
        /// Changes in the filter the attributes based on the comment similarities
        /// </summary>
        public void ApplyStyleSheetDataToAttributes()
        {
            foreach (VisualEntry v in this.Styles)
            {
                foreach (Entry e in this.AppliedFilter.EntryList)
                {
                    Line l = e.GetLine(v.identifier);
                    {
                        Line result = v.FindLineWithSameComment(l);
                        if (result != null)
                        {

                            e.ModifyAttribute(v.identifier, change: string.Join(" ",result.Attributes));
                        }
                    }
                }
            }
            this.AppliedFilter.AddFilterProgressToLogBox("Applying Style Params to filter...");
        }

        /// <summary>
        /// Purges the visualentry Collection and/or removes all Stylesheet marks in the filter
        /// </summary>
        /// <param name="ClearCurrentStyle"></param>
        /// <param name="RemoveStylesFromFilter"></param>
        public void RemoveAllStyles(bool ClearCurrentStyle, bool RemoveStylesFromFilter)
        {
            if (RemoveStylesFromFilter)
            {
                foreach (VisualEntry v in this.Styles)
                {
                    foreach (Entry e in this.AppliedFilter.EntryList)
                    {
                        e.ModifyAttribute(v.identifier, com: "");
                    }
                }
                this.AppliedFilter.AddFilterProgressToLogBox("Clearing styles from filter...");
            }

            if (ClearCurrentStyle)
            {
                foreach (VisualEntry v in this.Styles)
                {
                    v.Lines.Clear();
                }
                this.AppliedFilter.AddFilterProgressToLogBox("Clearing VisualEntry collections...");
            }
        }

        /// <summary>
        /// Saves the stylesheet in a JSON file. Not very effective
        /// </summary>
        public void SaveStyleAsJson()
        {
            string json = JsonConvert.SerializeObject(this.Styles);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JSON|*.json";
            dlg.Title = "Save the JSON";
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                System.IO.File.WriteAllText(dlg.FileName, json);
            }

            this.AppliedFilter.AddFilterProgressToLogBox("JSON preset saved!");
        }

        /// <summary>
        /// Saves the style in a small "ftsy" file.
        /// </summary>
        public void SaveStyle()
        {
            string text = "";

            foreach (VisualEntry v in this.Styles)
            {
                text +="\r\n" + "START: " + v.identifier.ToUpper() + "\r\n";
                foreach (Line l in v.Lines)
                {
                    text += (string.Join(" ",l.Attributes)).PadRight(35) + " // " + ( l.Comment != "" ? l.Comment : "# <NOTHING>" ) + "\r\n";
                }
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "fsty|*.fsty";
            dlg.Title = "Save the Filter Style";
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                System.IO.File.WriteAllText(dlg.FileName, text);
            }

            this.AppliedFilter.AddFilterProgressToLogBox("Style Preset Saved");
        }

        /// <summary>
        /// Loads style from JSON
        /// </summary>
        /// <returns></returns>
        public bool LoadStyleAsJson()
        {
            string text = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JSON|*.json";
            dlg.Title = "Save the JSON";
            dlg.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\My Games\\Path of Exile\\";

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = dlg.FileName;
                using (StreamReader sr = new StreamReader(filename))
                {
                    this.AppliedFilter.AddFilterProgressToLogBox("Loading JSON Preset!");
                    text = sr.ReadToEnd();

                    if (text.Length <= 1)
                    {
                        MessageBox.Show("NO TEXT IN FILE!!!");
                    }
                }

                this.Styles = JsonConvert.DeserializeObject<List<VisualEntry>>(text);

                if (this.Styles!=null)
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Loads style from FTSY
        /// </summary>
        /// <returns></returns>
        public bool LoadStyle(bool skipdialog = false, string path = "")
        {
            string text = "";

            if (skipdialog)
            {
                text = Util.ReadFileToString(path);
            }
            else
            {
                text = Util.ReadFileToString("fsty|*.fsty", "Open fsty file", Environment.SpecialFolder.MyDocuments + "\\My Games\\Path of Exile\\" + path);
            }

            List<string> Result = Regex.Split(text, "\r\n|\r|\n").ToList() ;

            int section = -1;
            foreach (string s in Result)
            {
                // Check if a new seciton is starting!

                if (Styles.Count > section+1)
                {

                    if (s.Contains("START: " + Styles[section+1].identifier.ToUpper()))
                    {
                        section++;
                    }
                }
                if (section >= 0)
                {
                    
                    if (s.Contains("//"))
                    {
                        
                        string comment = s.Substring(s.IndexOf("//") + 2);
                        if (comment.Contains("<NOTHING>"))
                        {
                            comment = "#" + this.Styles[section].newEntryMarker;
                        }
                        string data = s.Substring(0, s.IndexOf("//"));

                        Line l = new Line(data + comment);
                        l.Identify();

                        this.Styles[section].Lines.Add(l);
                    }
                }
                    
            }

            if (this.Styles != null)
            { return true; }

            return false;
        }

        /// <summary>
        /// Generates the a datacollection in ListViewForm from the current stylesheet.
        /// </summary>
        /// <returns></returns>
        public ListView GenerateListViewFromStyleSheet()
        {
            ListView LV = new ListView();
            foreach (VisualEntry v in this.Styles)
            {
                LV.Groups.Add(v.identifier, v.identifier);

                foreach (Line l in v.Lines)
                {
                    ListViewItem item = new ListViewItem(string.Join(" ", l.Attributes));
                    item.SubItems.Add(l.Comment);
                    item.Group = LV.Groups[v.identifier];

                    if (l.Comment.Contains("NEW"))
                    {
                        item.ForeColor = Color.Red;
                    }

                    if (v.identifier != "PlayAlertSound" && v.identifier != "SetFontSize")
                    {
                        int transparency = (l.O == 0 ? 240 : l.O);

                        int R = (int)Math.Round(((float)l.R * ((float)transparency) / (float)255));
                        int G = (int)Math.Round(((float)l.G * ((float)transparency) / (float)255));
                        int B = (int)Math.Round(((float)l.B * ((float)transparency) / (float)255));
                        Color color = Color.FromArgb(l.O, R, G, B);
                        item.BackColor = color;
                    }

                    LV.Items.Add(item);

                }
            }
            return LV;
        }
    }
}
