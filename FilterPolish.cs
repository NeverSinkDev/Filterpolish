using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;
using FilterPolish.Extract;
using Newtonsoft.Json;

namespace FilterPolish
{
    /// <summary>
    /// This is the main class of the application and also the execution base for most modules
    /// I've underestimated the scope of the project, so it has become a tiny bit cluttered
    /// </summary>
    public partial class FilterPolish : Form
    {
        Filter Fregular;
        Filter Fsemistrict;
        Filter Fverystrict;
        Filter Fstrict;
        Filter Fuberstrict;
        Configuration config;
        TierListManager TLM;

        /// <summary>
        /// Ye' generic on load configuration
        /// </summary>
        public FilterPolish()
        {
            InitializeComponent();
            ts_label1.Text = "Ready";
            ts_label2.Text = "Lines = 0";
            ts_label3.Text = "Errors = 0";
            this.config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            var appSettings = ConfigurationManager.AppSettings;
            //this.ConfigView.DataSource = appSettings;

            foreach (string key in appSettings)
            {

                this.ConfigView.Rows.Add(key, appSettings[key], "Set");
            }
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
        /// Rebuild the filter from Entries. This routine also removes redundant whitespaces and cleans up the layout a tiny bit.
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
        /// Open the filter, generate lines, entries and the Stylesheet, ToC and Tierlists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFilterAndPerformQuickOperations(object sender, EventArgs e)
        {
            FilterSettings f = new FilterSettings();
            Fregular = new Filter(OpenFilterFileAndGetText("unnamed"), f);
            Fregular.ReadLines(this);
            Fregular.GenerateEntries();
            Fregular.SortEntries();
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
            this.GenerateStyleSheetFromFilter(false);
            this.GenerateToC(false);
            this.generateTierList();
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
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
        /// Perform the filter Entry generation, while crating debug information
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

                string version = Util.getConfigValue("Version Number");

                // Generates the filter-strictness subversions. I'Ll move this to a file-based system if it becomes more complex
                FilterSettings Sregular = new FilterSettings("1-REGULAR", version, 0);
                FilterSettings Ssemistrict = new FilterSettings("2-SEMI-STRICT", version, 1);
                FilterSettings Sstrict = new FilterSettings("3-STRICT", version, 2);
                FilterSettings Sverystrict = new FilterSettings("4-VERY-STRICT", version, 3);
                FilterSettings Suberstrict = new FilterSettings("5-UBER-STRICT", version, 4);

                Fregular = new Filter(text, Sregular);
                Fstrict = new Filter(text, Sstrict);
                Fsemistrict = new Filter(text, Ssemistrict);
                Fverystrict = new Filter(text, Sverystrict);
                Fuberstrict = new Filter(text, Suberstrict);

                // Initializes the stylesheets
                StyleSheet SSdef = new StyleSheet("default");
                StyleSheet SSBlue = new StyleSheet("Blue");
                StyleSheet SSSlick = new StyleSheet("Slick");
                // StyleSheet SSPurple = new StyleSheet("Purple");
                StyleSheet SSStreamer = new StyleSheet("StreamSound");

                // Adds filters and stylesheets to their relevant arrays
                List<Filter> FilterArray = new List<Filter>();
                List<StyleSheet> StyleSheetArray = new List<StyleSheet>();

                FilterArray.Add(Fregular);
                FilterArray.Add(Fstrict);
                FilterArray.Add(Fsemistrict);
                FilterArray.Add(Fverystrict);
                FilterArray.Add(Fuberstrict);

                StyleSheetArray.Add(SSdef);
                StyleSheetArray.Add(SSBlue);
                StyleSheetArray.Add(SSSlick);
                StyleSheetArray.Add(SSStreamer);
                // StyleSheetArray.Add(SSPurple);

                // For every strictness and stylesheet...
                foreach (Filter f in FilterArray)
                {
                    foreach (StyleSheet s in StyleSheetArray)
                    {
                        AddTextToLogBox("GENERATING SUBVERSION: " + f.settings.subVersionName);
                        AddTextToLogBox("APPLYING STYLE: " + s.Name);

                        // First read the file again (to prevent problems)
                        f.ReadLines(this);

                        // Perform all non-style based operations
                        f.GenerateEntries();
                        f.GenerateTOC(true);
                        f.AdjustVersionName(0, 4);
                        f.AdjustVersionNumber(0, 3);
                        f.FindAndHandleVersionTags();
                        f.SortEntries();

                        // Adjust the style for all non-default stylesheets
                        if (s.Name != "default")
                        {
                            s.Init();
                            s.LoadStyle(true, Util.GetOutputPath() + @"ADDITIONAL-FILES\StyleSheets\" + s.Name + ".fsty");
                            f.AdjustStyleName(0, 5, s.Name.ToUpper());
                            s.AppliedFilter = f;
                            s.ApplyStyleSheetDataToAttributes();
                        }

                        // Generate the filter
                        f.RebuildFilterFromEntries();

                        // Perform file and logging operations
                        if (s.Name != "default")
                        {
                            f.SaveToFile("(STYLE) " + s.Name.ToUpper().ToString(), s.Name);
                        }
                        else
                        {
                            f.SaveToFile();
                        }
                        AddTextToLogBox("---------------------------------");
                    }

                    AddTextToLogBox("STRICTNESS SUBVERSION COMPLETE: " + f.settings.subVersionName);
                    AddTextToLogBox("---------------------------------");
                }
                AddTextToLogBox("---------------------------------");
                AddTextToLogBox("All Operations Completed!!! Generated " + StyleSheetArray.Count * FilterArray.Count + " Filterfiles.");

                OutputTransform.Text = Fregular.RawFilterRebuilt;
                ts_label1.Text = "Ready";

                // Open the folder. QOL confirmed.
                Process.Start(Util.GetOutputPath());
            }

        }

        /// <summary>
        /// Add logtext and update the view
        /// </summary>
        /// <param name="line"></param>
        public void AddTextToLogBox(string line)
        {
            logBox.Text += line;
            logBox.Text += System.Environment.NewLine;
            logBox.Invalidate();
            logBox.Update();
            logBox.Refresh();
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        /// <summary>
        /// Generates the TOC (table of contents), without applying it to the filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateToC(false, true);
            tabControl.SelectTab(4);
        }

        /// <summary>
        /// Generates the TOC and applies it to the filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gatherAndApplyToCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GenerateToC(true, true);
            tabControl.SelectTab(4);
        }

        /// <summary>
        /// Initiatates the TOC routine and displays the output
        /// </summary>
        /// <param name="ApplyToC"></param>
        /// <param name="GenerateLines"></param>
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
            Process.Start(Util.GetOutputPath());
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
            if (item != null)
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
        /// Applies the comment to all entries of the applied style
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
                Fregular.CurrentStyle.Styles.Where(i => i.identifier == LastItem.Group.Name).First().GetLineByFullString(LastItem.SubItems[0].Text).Comment = commenttext;
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
        private void C1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
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

        /// <summary>
        /// This method is supposed to adjust the comments in the filter in order to utilize a certain stylesheet. I didn't get to test it very much.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyCommentsFromStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StyleSheet s = new StyleSheet(Fregular);
            s.Init();
            s.LoadStyle();
            s.ApplyStyleSheetDataToComments();
            //s.ApplyStyleSheetDataToAttributes();
            //Fregular.RebuildFilterFromEntries();
            this.GenerateStyleSheetFromFilter(false);
        }

        /// <summary>
        /// Potentially the most horrible about dialog in the world.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Uh. Oh. This is one poor dialog box. Anyway, app created by NeverSink - that's me. I'm sorry for all the bugs and the messy code, I'll promise I'll clean it up soon O.O. For now, you can open your filter, clean and sort it (cleaning tab) and observe it's glory in the StyleSheets tab. I'm also going to add a small instruction and make the whole app a tiiiiny bit more userfriendly. \n https://github.com/NeverSinkDev ");
        }

        /// <summary>
        /// Saves the changes done to the config file, through the use of the config data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && (this.ConfigView[0, e.RowIndex].Value != null &&
                this.ConfigView[1, e.RowIndex].Value != null))
            {

                config.AppSettings.Settings[this.ConfigView[0, e.RowIndex].Value.ToString()].Value = this.ConfigView[1, e.RowIndex].Value.ToString();
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                this.ConfigView.Refresh();


            }
        }

        /// <summary>
        /// Gathers the tierlists in the filter, based upon the comment tags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gatherListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.generateTierList();
            tabControl.SelectTab(5);
        }

        /// <summary>
        /// Executes the actual tier list generation logic and updates the datagrid
        /// </summary>
        private void generateTierList()
        {
            TLM = new TierListManager(this.Fregular);   
            int n = 0;

            foreach (Tier t in TLM.tierList)
            {
                this.TierListView.Rows.Add(n, t.GroupName, t.FilterEntries.Count, t.TierRows, !t.MissMatch, t.Value);
                n++;
            }
        }

        /// <summary>
        /// Triggers upon focus change in the tierlist datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TierListView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.ChangeTierListSelection(e.RowIndex);
        }

        /// <summary>
        /// Adjusts the relevant entries to the currently selected tierlist
        /// </summary>
        /// <param name="index"></param>
        public void ChangeTierListSelection(int index)
        {
            string preview = "";

            if (TLM == null || index < 0)
            {
                return;
            }

            if (index >= TLM.tierList.Count)
            {
                return;
            }

            foreach (Entry entry in TLM.tierList[index].FilterEntries)
            {
                foreach (Line l in entry.Lines)
                {
                    preview += l.RebuildLine() + "\r\n"; ;
                }
                preview += "\r\n";
            }

            TierListValueBox.Text = TLM.tierList[index].Value;
            this.TierListLines.Text = preview;
        }

        /// <summary>
        /// Adjusts the parameters of all entries of the currently selected tier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTierListButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles keyboard clicks in the tierlist datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TierList_KeyUp(object sender, KeyEventArgs e)
        {
            if (TierListView.Rows.Count >= 1 && TierListView.SelectedCells.Count >= 1)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (TierListView.Rows.Count >= 1 && TierListView.SelectedCells.Count >= 1)
                {
                    int index = TierListView.SelectedCells[0].RowIndex;

                    if (index < TierListView.Rows.Count - 1)
                    {
                        if (e.KeyCode == Keys.Down)
                        {
                            TierListView.Rows[index + 1].Selected = true; 
                            ChangeTierListSelection(index - 1);
                        }
                    }

                    if (index > 0)
                    {
                        if (e.KeyCode == Keys.Up)
                        {
                            TierListView.Rows[index - 1].Selected = true;
                            ChangeTierListSelection(index - 1);
                        }
                    }

                    if (e.KeyCode == Keys.Enter)
                    {
                        this.SetComment_Click(null, null);
                        TierListView.Select();
                    }

                    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    {
                        TierListValueBox.Select();
                        TierListValueBox.SelectionStart = TierListValueBox.Text.Length;
                    }
                }
            }
        }

        /// <summary>
        /// Opens a file and executes all modules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openAndFillFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterSettings f = new FilterSettings();
            Fregular = new Filter(OpenFilterFileAndGetText("unnamed"), f);
            Fregular.ReadLines(this);
            Fregular.GenerateEntries();
            Fregular.SortEntries();
            this.GenerateStyleSheetFromFilter(false);
            this.GenerateToC(false);
            this.generateTierList();
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        /// <summary>
        /// Initiates a HTTP request to poe.ninja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getNinjaJSONDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> urlSuffix1 = new List<string>
            {
                //"GetUniqueMapOverview",
                //"GetDivinationCardsOverview",
                //"GetEssenceOverview",
                //"GetUniqueJewelOverview",
                //"GetUniqueFlaskOverview",
                //"GetUniqueWeaponOverview",
                //"GetUniqueArmourOverview",
                "GetUniqueAccessoryOverview"
            };

            RequestManager rm = new RequestManager(urlSuffix1);
            rm.ExecuteAll();

            return;
        }
    }
}
