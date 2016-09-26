using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterPolish
{
    public partial class Form1 : Form
    {
        Filter Fregular;
        Filter Fsemistrict;
        Filter Fstrict;
        Filter Fslick;
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

        private void openFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "NeverSink"; // Default file name
            dlg.DefaultExt = ".filter"; // Default file extension
            dlg.Filter = "Path of Exile Filter File (.filter)|*.filter"; // Filter files by extension
            dlg.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\My Games\\Path of Exile";

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Open document
                string filename = dlg.FileName;

                //                try
                //                {
                ts_label1.Text = "Loading";
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(filename))
                {
                    string text = sr.ReadToEnd();
                    Fregular = new Filter(text);

                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    if (text != null)
                    {
                        OutputText.Text = text;
                        Fregular.ReadLines(this);
                    }
                    Fregular.GenerateEntries();
                }
                ts_label1.Text = "Ready";
            }
        }

        private void tidyUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        private void sortEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.SortEntries();
            Fregular.RebuildFilterFromEntries();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        private void generateStyleListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void generateDebugVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fregular.RebuildFilterAndEntriesDebug();
            OutputTransform.Text = Fregular.RawFilterRebuilt;
        }

        private void doItAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.Clear();
            AddTextToLogBox("OPENING FILE...");

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "NeverSink"; // Default file name
            dlg.DefaultExt = ".filter"; // Default file extension
            dlg.Filter = "Path of Exile Filter File (.filter)|*.filter"; // Filter files by extension
            dlg.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\My Games\\Path of Exile";

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                tabControl.SelectTab(1);
                AddTextToLogBox("GENERATING FILTER SETTINGS...");

                // Open document
                string filename = dlg.FileName;
                ts_label1.Text = "Loading";

                string version = "4.22";

                FilterSettings Sregular = new FilterSettings("REGULAR", version, 0);
                FilterSettings Ssemistrict = new FilterSettings("SEMI-STRICT", version, 1);
                FilterSettings Sstrict = new FilterSettings("STRICT", version, 2);
                FilterSettings Suberstrict = new FilterSettings("UBER-STRICT", version, 3);
                FilterSettings Sslick = new FilterSettings("SLICK", version, 0);

                using (StreamReader sr = new StreamReader(filename))
                {
                    AddTextToLogBox("READING FILE...");
                    string text = sr.ReadToEnd();
                    Fregular = new Filter(text,Sregular);
                    Fstrict = new Filter(text,Sstrict);
                    Fsemistrict = new Filter(text,Ssemistrict);
                    Fuberstrict = new Filter(text, Suberstrict);
                }

                List<Filter> FilterArray = new List<Filter>();

                FilterArray.Add(Fregular);
                FilterArray.Add(Fstrict);
                FilterArray.Add(Fsemistrict);
                FilterArray.Add(Fuberstrict);

                foreach(Filter f in FilterArray)
                {
                    f.ReadLines(this);
                    f.GenerateEntries();
                    f.AdjustVersionName();
                    f.AdjustVersionName();
                    f.FindAndHandleVersionTags();
                    f.SortEntries();

                    f.RebuildFilterFromEntries();
                    f.SaveToFile();
                }

                OutputTransform.Text = Fregular.RawFilterRebuilt;
                ts_label1.Text = "Ready";
                
            }

        }
        public void AddTextToLogBox(string line)
            {
                logBox.Text += line;
                logBox.Text += System.Environment.NewLine;
            }
        
    }
}

