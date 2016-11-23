﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace FilterPolish
{
    public partial class Form1 : Form
    {
        Filter Fregular;
        Filter Fsemistrict;
        Filter Fstrict;
        Filter Fuberstrict;

        public Form1()
        {
            InitializeComponent();
            ts_label1.Text = "Ready";
            ts_label2.Text = "Lines = 0";
            ts_label3.Text = "Errors = 0";
            return;
        }

        public void control_ts_label1(string s)
        {
            ts_label1.Text = s;
        }
        public void control_ts_label2(string s)
        {
            ts_label2.Text = s;
        }
        public void control_ts_label3(string s)
        {
            ts_label3.Text = s;
        }

        /// <summary>
        /// Open filter Default Routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string OpenFilterFileAndGetText(string FileName)
        {
            string text = "";
            logBox.Clear();
            AddTextToLogBox("OPENING FILE...");

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = FileName; // Default file name
            dlg.DefaultExt = ".filter"; // Default file extension
            dlg.Filter = "Path of Exile Filter File (.filter)|*.filter"; // Filter files by extension
            dlg.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\My Games\\Path of Exile";

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = dlg.FileName;
                ts_label1.Text = "Loading";

                using (StreamReader sr = new StreamReader(filename))
                {
                    tabControl.SelectTab(1);
                    // Open document
                    AddTextToLogBox("READING FILE...");
                    text = sr.ReadToEnd();

                    if (text.Length <= 1)
                    {
                        MessageBox.Show("NO TEXT IN FILE!!!");
                    }

                    OutputText.Text = text;
                }
            }

            return text;
        }

        /// <summary>
        /// Rebuild the filter from Entries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tidyUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        /// <summary>
        /// Open the filter, generate lines, entries and the Stylesheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFilter(object sender, EventArgs e)
        {

            FilterSettings f = new FilterSettings();
            Fregular = new Filter(OpenFilterFileAndGetText("unnamed"), f);
            Fregular.ReadLines(this);
            Fregular.GenerateEntries();
            this.GenerateStyleSheetFromFilter(false);
        }

        /// <summary>
        /// Perform the default sorting routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.SortEntries();
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        /// <summary>
        /// Perform 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateDebugVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterAndEntriesDebug();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        /// <summary>
        /// Perform 1-Click operation for NeverSink's filter generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doItAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = OpenFilterFileAndGetText("NeverSink");

            if (text != "")
            {
                tabControl.SelectTab(1);
                AddTextToLogBox("GENERATING FILTER SETTINGS...");
                ts_label1.Text = "Loading";

                string version = "4.24";

                //  rework this!
                FilterSettings Sregular = new FilterSettings("REGULAR", version, 0);
                FilterSettings Ssemistrict = new FilterSettings("SEMI-STRICT", version, 1);
                FilterSettings Sstrict = new FilterSettings("STRICT", version, 2);
                FilterSettings Suberstrict = new FilterSettings("UBER-STRICT", version, 3);
                FilterSettings Sslick = new FilterSettings("SLICK", version, 0);

                Fregular = new Filter(text, Sregular);
                Fstrict = new Filter(text, Sstrict);
                Fsemistrict = new Filter(text, Ssemistrict);
                Fuberstrict = new Filter(text, Suberstrict);

                List<Filter> FilterArray = new List<Filter>();

                FilterArray.Add(Fregular);
                FilterArray.Add(Fstrict);
                FilterArray.Add(Fsemistrict);
                FilterArray.Add(Fuberstrict);

                foreach (Filter f in FilterArray)
                {
                    f.ReadLines(this);
                    f.GenerateEntries();
                    f.GenerateTOC(true);
                    f.AdjustVersionName(0, 2);
                    f.AdjustVersionNumber(0, 3);
                    f.FindAndHandleVersionTags();
                    f.SortEntries();
                    

                    f.RebuildFilterFromEntries();
                    f.SaveToFile();
                }

                OutputTransform.Text = Fregular.RawFilterRebuilt;
                ts_label1.Text = "Ready";
                Process.Start(@"C:\FilterOutput");
            }

        }

        /// <summary>
        /// Add logtext
        /// </summary>
        /// <param name="line"></param>
        public void AddTextToLogBox(string line)
        {
            logBox.Text += line;
            logBox.Text += System.Environment.NewLine;
        }

        /// <summary>
        /// Generate comments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateToC(false, true);
            tabControl.SelectTab(4);
        }

        private void gatherAndApplyToCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateToC(true, true);
            tabControl.SelectTab(4);
        }

        private void GenerateToC(bool ApplyToC = true, bool GenerateLines = true)
        {
            Fregular.GenerateTOC(ApplyToC);

            if (GenerateLines)
            {
                ToCText.Clear();
                foreach (Line l in Fregular.TOC.Lines)
                {
                    ToCText.Text += l.RebuildLine(true);
                    ToCText.Text += "\r\n";
                }
            }
        }

        /// <summary>
        /// Save File.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterFromEntries();
            Fregular.SaveToFile();

            OutputTransform.Text = Fregular.RawFilterRebuilt;
            ts_label1.Text = "Ready";
            Process.Start(@"C:\FilterOutput");
        }

        /// <summary>
        /// Exit App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Generate the Style Entries, while applying New Tags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gatherUniqueStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateStyleSheetFromFilter(true);
            tabControl.SelectTab(3);
        }

        /// <summary>
        /// Generates the Style Entries and applies new tags if demanded
        /// </summary>
        /// <param name="labelNewStyles"></param>
        public void GenerateStyleSheetFromFilter(bool labelNewStyles)
        {
            StyleListView.Items.Clear();
            StyleSheet s = Fregular.GenerateStyleSheet(labelNewStyles);
            ListView list = Fregular.CurrentStyle.GenerateListViewFromStyleSheet();

            foreach (ListViewGroup lvg in list.Groups)
            {
                ListViewGroup CloneGroup = Util.DeepCopy(lvg);
                StyleListView.Groups.Add(CloneGroup);
            }
                                    
            foreach (ListViewItem lv in list.Items)
            {
                ListViewItem toclone = new ListViewItem();
                toclone = (ListViewItem)lv.Clone();
                toclone.Group = StyleListView.Groups[toclone.Group.Name];
                StyleListView.Items.Add(toclone);
            }

        }

        /// <summary>
        /// Opens a style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openStylesheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StyleSheet s = new StyleSheet(Fregular);
            s.Init();
            s.LoadStyle();
            //s.ApplyStyleSheetDataToComments();
            s.ApplyStyleSheetDataToAttributes();
            Fregular.RebuildFilterFromEntries();
            this.GenerateStyleSheetFromFilter(false);
        }

        /// <summary>
        /// Saves the style.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Fregular.CurrentStyle != null && Fregular.CurrentStyle.initialized != false)
            {
                Fregular.CurrentStyle.SaveStyle();
            }
        }

        /// <summary>
        /// Applies the current StyleSheet to the filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyStylesheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Fregular.CurrentStyle != null && Fregular.CurrentStyle.initialized != false)
            {
                Fregular.CurrentStyle.ApplyStyleSheetCommentsToData();
            }
        }

        /// <summary>
        /// Clears the Stylesheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Fregular.CurrentStyle != null && Fregular.CurrentStyle.initialized != false)
            {
                Fregular.CurrentStyle.RemoveAllStyles(true, false);
            }

        }

        /// <summary>
        /// Removes style comments from the filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeAllStyleEntryMarkupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StyleSheet s = new StyleSheet(Fregular);
            Fregular.CurrentStyle = s;
            s.Init();
            if (Fregular.CurrentStyle != null)
            {
                Fregular.CurrentStyle.RemoveAllStyles(false, true);
            }
        }

        /// <summary>
        /// EVENT: On StyleSheetListViewItemClick Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClicked(object sender, EventArgs e)
        {
            ListView item = sender as ListView;
            if (item!=null)
            {
                if (item.SelectedItems.Count >= 1)
                {
                    ListViewItem LastItem = item.SelectedItems[item.SelectedItems.Count - 1];
                    this.UpdateStylePreview(LastItem);
                }
            } 
        }

        /// <summary>
        /// Grabs the entries that are used by the StyleItem and also updates the fields.
        /// </summary>
        /// <param name="LastItem"></param>
        public void UpdateStylePreview(ListViewItem LastItem)
        {
            List<Entry> EList = Fregular.GetEntriesContaining(LastItem.SubItems[0].Text, LastItem.SubItems[1].Text);

            string preview = "";

            foreach (Entry entry in EList)
            {
                foreach (Line l in entry.Lines)
                {
                    preview += l.RebuildLine() + "\r\n"; ;
                }
                preview += "\r\n";
            }

            string ident = T1.Text = LastItem.Group.Name;

            T1.Text = "";
            T2.Text = "";
            T3.Text = "";
            T4.Text = "";
            C1.Text = "";

            if (ident != "PlayAlertSound")
            {
                T1.Text = EList.First().GetLine(ident).R.ToString();
                T2.Text = EList.First().GetLine(ident).G.ToString();
                T3.Text = EList.First().GetLine(ident).B.ToString();
                T4.Text = EList.First().GetLine(ident).O.ToString();
                C1.Text = EList.First().GetLine(ident).Comment.ToString();
            }
            else
            {
                T1.Text = EList.First().GetLine(ident).DropSound.ToString();
                T2.Text = EList.First().GetLine(ident).DropVolume.ToString();

                if (T2.Text == "0")
                {
                    T2.Text = "";
                }
            }

            EntryPreview.Text = preview;
        }

        /// <summary>
        /// Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterFromEntries();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PoE Filter File|*.filter";
            saveFileDialog1.Title = "Save the POE filter File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Fregular.SaveToFile(saveFileDialog1);
            }

            OutputTransform.Text = Fregular.RawFilterRebuilt;
            ts_label1.Text = "Ready";
            Process.Start(Path.GetDirectoryName(saveFileDialog1.FileName));
        }

        /// <summary>
        /// Applies the comment to the entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetComment_Click(object sender, EventArgs e)
        {
            if (StyleListView.SelectedItems.Count != 0)
            {
                ListViewItem LastItem = StyleListView.SelectedItems[StyleListView.SelectedItems.Count - 1];
                int LastItemIndex = LastItem.Index;

                string commenttext = !C1.Text.Contains("#") ? ("# " + Fregular.CurrentStyle.Styles.Where(c => c.identifier == LastItem.Group.Name.ToString()).First().commentIdentifierDescription + " " + C1.Text) : C1.Text;
                Fregular.CurrentStyle.Styles.Where(i => i.identifier == LastItem.Group.Name).First().GetLineByFullString(LastItem.SubItems[0].Text).Comment = commenttext ;
                StyleListView.Items[LastItemIndex].SubItems[1].Text = commenttext;
                C1.Focus();
                C1.SelectAll();
            }
        }

        /// <summary>
        /// Manages the up/down/enter keypressevents in the StyleSheet area for smoother navigation and editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void C1_KeyUp (object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (StyleListView.SelectedItems.Count >= 1)
            {
                int index = StyleListView.SelectedItems[0].Index;

                if (index < StyleListView.Items.Count - 1)
                {
                    if (e.KeyCode == Keys.Down)
                    {

                        StyleListView.Items[index + 1].Selected = true;
                        UpdateStylePreview(StyleListView.Items[index + 1]);
                    }
                }

                if (index > 0)
                {
                    if (e.KeyCode == Keys.Up)
                    {

                        StyleListView.Items[index - 1].Selected = true;
                        UpdateStylePreview(StyleListView.Items[index - 1]);
                    }
                }

                if (e.KeyCode == Keys.Enter)
                {
                    this.SetComment_Click(null, null);
                }

                C1.Focus();
                

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    C1.SelectAll();
                }


            }
        }
    }
}
