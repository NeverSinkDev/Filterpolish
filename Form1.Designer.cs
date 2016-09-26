namespace FilterPolish
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tidyUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateStyleListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subversionConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateSubversionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChangelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.announceOnTwitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugAndTestingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateDebugVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ts_label1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ts_label2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ts_label3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.OutputText = new System.Windows.Forms.TextBox();
            this.OutputTransform = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.logBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.basicToolStripMenuItem,
            this.advancedToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(805, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.openFilterToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.toolStripMenuItem1.Text = "Do it All";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.doItAllToolStripMenuItem_Click);
            // 
            // openFilterToolStripMenuItem
            // 
            this.openFilterToolStripMenuItem.Name = "openFilterToolStripMenuItem";
            this.openFilterToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.openFilterToolStripMenuItem.Text = "Open Filter";
            this.openFilterToolStripMenuItem.Click += new System.EventHandler(this.openFilterToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // basicToolStripMenuItem
            // 
            this.basicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tidyUpToolStripMenuItem,
            this.sortEntriesToolStripMenuItem,
            this.generateNotesToolStripMenuItem,
            this.generateStyleListToolStripMenuItem});
            this.basicToolStripMenuItem.Name = "basicToolStripMenuItem";
            this.basicToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.basicToolStripMenuItem.Text = "Basic";
            // 
            // tidyUpToolStripMenuItem
            // 
            this.tidyUpToolStripMenuItem.Name = "tidyUpToolStripMenuItem";
            this.tidyUpToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.tidyUpToolStripMenuItem.Text = "Tidy Up";
            this.tidyUpToolStripMenuItem.Click += new System.EventHandler(this.tidyUpToolStripMenuItem_Click);
            // 
            // sortEntriesToolStripMenuItem
            // 
            this.sortEntriesToolStripMenuItem.Name = "sortEntriesToolStripMenuItem";
            this.sortEntriesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.sortEntriesToolStripMenuItem.Text = "Sort Entries";
            this.sortEntriesToolStripMenuItem.Click += new System.EventHandler(this.sortEntriesToolStripMenuItem_Click);
            // 
            // generateNotesToolStripMenuItem
            // 
            this.generateNotesToolStripMenuItem.Name = "generateNotesToolStripMenuItem";
            this.generateNotesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.generateNotesToolStripMenuItem.Text = "Generate Notes";
            // 
            // generateStyleListToolStripMenuItem
            // 
            this.generateStyleListToolStripMenuItem.Name = "generateStyleListToolStripMenuItem";
            this.generateStyleListToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.generateStyleListToolStripMenuItem.Text = "Generate StyleList";
            this.generateStyleListToolStripMenuItem.Click += new System.EventHandler(this.generateStyleListToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.subversionConfigToolStripMenuItem,
            this.generateSubversionsToolStripMenuItem,
            this.uploadToolStripMenuItem,
            this.addChangelogToolStripMenuItem,
            this.updateThreadToolStripMenuItem,
            this.announceOnTwitterToolStripMenuItem,
            this.debugAndTestingToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // subversionConfigToolStripMenuItem
            // 
            this.subversionConfigToolStripMenuItem.Name = "subversionConfigToolStripMenuItem";
            this.subversionConfigToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.subversionConfigToolStripMenuItem.Text = "Subversion Config";
            // 
            // generateSubversionsToolStripMenuItem
            // 
            this.generateSubversionsToolStripMenuItem.Name = "generateSubversionsToolStripMenuItem";
            this.generateSubversionsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.generateSubversionsToolStripMenuItem.Text = "Generate Subversions";
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.uploadToolStripMenuItem.Text = "Upload";
            // 
            // addChangelogToolStripMenuItem
            // 
            this.addChangelogToolStripMenuItem.Name = "addChangelogToolStripMenuItem";
            this.addChangelogToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.addChangelogToolStripMenuItem.Text = "Add changelog";
            // 
            // updateThreadToolStripMenuItem
            // 
            this.updateThreadToolStripMenuItem.Name = "updateThreadToolStripMenuItem";
            this.updateThreadToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.updateThreadToolStripMenuItem.Text = "Update thread";
            // 
            // announceOnTwitterToolStripMenuItem
            // 
            this.announceOnTwitterToolStripMenuItem.Name = "announceOnTwitterToolStripMenuItem";
            this.announceOnTwitterToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.announceOnTwitterToolStripMenuItem.Text = "Announce on Twitter";
            // 
            // debugAndTestingToolStripMenuItem
            // 
            this.debugAndTestingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateDebugVersionToolStripMenuItem});
            this.debugAndTestingToolStripMenuItem.Name = "debugAndTestingToolStripMenuItem";
            this.debugAndTestingToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.debugAndTestingToolStripMenuItem.Text = "Debug and Testing";
            // 
            // generateDebugVersionToolStripMenuItem
            // 
            this.generateDebugVersionToolStripMenuItem.Name = "generateDebugVersionToolStripMenuItem";
            this.generateDebugVersionToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.generateDebugVersionToolStripMenuItem.Text = "Generate Debug-Version";
            this.generateDebugVersionToolStripMenuItem.Click += new System.EventHandler(this.generateDebugVersionToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_label1,
            this.ts_label2,
            this.ts_label3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 352);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(805, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ts_label1
            // 
            this.ts_label1.Name = "ts_label1";
            this.ts_label1.Size = new System.Drawing.Size(118, 17);
            this.ts_label1.Text = "toolStripStatusLabel1";
            // 
            // ts_label2
            // 
            this.ts_label2.Name = "ts_label2";
            this.ts_label2.Size = new System.Drawing.Size(118, 17);
            this.ts_label2.Text = "toolStripStatusLabel2";
            // 
            // ts_label3
            // 
            this.ts_label3.Name = "ts_label3";
            this.ts_label3.Size = new System.Drawing.Size(118, 17);
            this.ts_label3.Text = "toolStripStatusLabel3";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 327);
            this.treeView1.TabIndex = 2;
            // 
            // OutputText
            // 
            this.OutputText.AcceptsReturn = true;
            this.OutputText.Location = new System.Drawing.Point(0, 0);
            this.OutputText.Margin = new System.Windows.Forms.Padding(2);
            this.OutputText.Multiline = true;
            this.OutputText.Name = "OutputText";
            this.OutputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputText.Size = new System.Drawing.Size(351, 294);
            this.OutputText.TabIndex = 3;
            // 
            // OutputTransform
            // 
            this.OutputTransform.AcceptsReturn = true;
            this.OutputTransform.Location = new System.Drawing.Point(355, 0);
            this.OutputTransform.Margin = new System.Windows.Forms.Padding(2);
            this.OutputTransform.Multiline = true;
            this.OutputTransform.Name = "OutputTransform";
            this.OutputTransform.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputTransform.Size = new System.Drawing.Size(318, 294);
            this.OutputTransform.TabIndex = 4;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(124, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(681, 322);
            this.tabControl.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.OutputText);
            this.tabPage1.Controls.Add(this.OutputTransform);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(673, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.logBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(673, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // logBox
            // 
            this.logBox.AcceptsReturn = true;
            this.logBox.Location = new System.Drawing.Point(2, 2);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(669, 294);
            this.logBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 374);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "NeverSink\'s Filterhelper";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem basicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tidyUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateStyleListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subversionConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateSubversionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addChangelogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem announceOnTwitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel ts_label1;
        private System.Windows.Forms.ToolStripStatusLabel ts_label2;
        private System.Windows.Forms.ToolStripStatusLabel ts_label3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox OutputText;
        private System.Windows.Forms.TextBox OutputTransform;
        private System.Windows.Forms.ToolStripMenuItem debugAndTestingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateDebugVersionToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox logBox;
    }
}

