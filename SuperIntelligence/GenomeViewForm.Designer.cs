﻿namespace SuperIntelligence
{
    partial class GenomeViewForm
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
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speciesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastGenerationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDevToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.runTreeView = new System.Windows.Forms.TreeView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.runTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.runGenomeSizeChart = new LiveCharts.WinForms.CartesianChart();
            this.runFitnessChart = new LiveCharts.WinForms.CartesianChart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.runTimeLabel = new System.Windows.Forms.Label();
            this.startStopRunButton = new System.Windows.Forms.Button();
            this.generationTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.generationFitnessChart = new LiveCharts.WinForms.CartesianChart();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.generationGenomePerSpeciesChart = new LiveCharts.WinForms.PieChart();
            this.generationGenomeSizeChart = new LiveCharts.WinForms.CartesianChart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.generationBestGenomeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.genTabGenomeCountLabel = new System.Windows.Forms.Label();
            this.genTabGenerationNameLabel = new System.Windows.Forms.Label();
            this.genTabSpeciesCountLabel = new System.Windows.Forms.Label();
            this.speciesTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.speciesTabFitnessChart = new LiveCharts.WinForms.CartesianChart();
            this.panel3 = new System.Windows.Forms.Panel();
            this.speciesTabBestGenomeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.speciesTabGenomeCountLabel = new System.Windows.Forms.Label();
            this.speciesTabSpeciesCountLabel = new System.Windows.Forms.Label();
            this.genomeTabPage = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.genomePictureBox = new System.Windows.Forms.PictureBox();
            this.genomeTabFitnessLabel = new System.Windows.Forms.Label();
            this.genomeNameLabel = new System.Windows.Forms.Label();
            this.runSettingsTabPage = new System.Windows.Forms.TabPage();
            this.algorithmsGroupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nBestUpDown = new System.Windows.Forms.NumericUpDown();
            this.reproductionsPerGenomeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.reproductionSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.resetButtonRunSettings = new System.Windows.Forms.Button();
            this.runVariablesGroupBox = new System.Windows.Forms.GroupBox();
            this.eitherDisabledUpDown = new System.Windows.Forms.NumericUpDown();
            this.connectionCreationUpDown = new System.Windows.Forms.NumericUpDown();
            this.nodeCreationUpDown = new System.Windows.Forms.NumericUpDown();
            this.weightPerturbanceUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.weightMutationUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.luaScriptTextBox = new System.Windows.Forms.TextBox();
            this.saveButtonRunSettings = new System.Windows.Forms.Button();
            this.gameInstancesUpDown = new System.Windows.Forms.NumericUpDown();
            this.luaScriptBrowseButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.populationSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.generationFileTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.randomFirstGenCheckBox = new System.Windows.Forms.CheckBox();
            this.browseFirstGenButton = new System.Windows.Forms.Button();
            this.gameModeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cefTabPage = new System.Windows.Forms.TabPage();
            this.runBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.openFirstGenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openLuaScriptDialog = new System.Windows.Forms.OpenFileDialog();
            this.reloadCEFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runJSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.runTabPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.generationTabPage.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.speciesTabPage.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.genomeTabPage.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.genomePictureBox)).BeginInit();
            this.runSettingsTabPage.SuspendLayout();
            this.algorithmsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nBestUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reproductionsPerGenomeUpDown)).BeginInit();
            this.runVariablesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eitherDisabledUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionCreationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeCreationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightPerturbanceUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightMutationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameInstancesUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.populationSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.applicationToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(829, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.genomeToolStripMenuItem,
            this.speciesToolStripMenuItem,
            this.generationToolStripMenuItem,
            this.runToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // genomeToolStripMenuItem
            // 
            this.genomeToolStripMenuItem.Name = "genomeToolStripMenuItem";
            this.genomeToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.genomeToolStripMenuItem.Text = "Genome";
            // 
            // speciesToolStripMenuItem
            // 
            this.speciesToolStripMenuItem.Name = "speciesToolStripMenuItem";
            this.speciesToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.speciesToolStripMenuItem.Text = "Species";
            // 
            // generationToolStripMenuItem
            // 
            this.generationToolStripMenuItem.Name = "generationToolStripMenuItem";
            this.generationToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.generationToolStripMenuItem.Text = "Generation";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(95, 22);
            this.runToolStripMenuItem1.Text = "Run";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearDataToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // clearDataToolStripMenuItem
            // 
            this.clearDataToolStripMenuItem.Name = "clearDataToolStripMenuItem";
            this.clearDataToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.clearDataToolStripMenuItem.Text = "Clear Data";
            this.clearDataToolStripMenuItem.Click += new System.EventHandler(this.clearDataToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lastGenerationToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // lastGenerationToolStripMenuItem
            // 
            this.lastGenerationToolStripMenuItem.Name = "lastGenerationToolStripMenuItem";
            this.lastGenerationToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.lastGenerationToolStripMenuItem.Text = "Last Generation";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.applicationToolStripMenuItem.Text = "Application";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDevToolsToolStripMenuItem,
            this.reloadCEFToolStripMenuItem,
            this.openToolStripMenuItem,
            this.runJSToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // showDevToolsToolStripMenuItem
            // 
            this.showDevToolsToolStripMenuItem.Name = "showDevToolsToolStripMenuItem";
            this.showDevToolsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showDevToolsToolStripMenuItem.Text = "Show DevTools";
            this.showDevToolsToolStripMenuItem.Click += new System.EventHandler(this.showDevToolsToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.runTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mainTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(829, 551);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 1;
            // 
            // runTreeView
            // 
            this.runTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runTreeView.Location = new System.Drawing.Point(0, 0);
            this.runTreeView.Name = "runTreeView";
            this.runTreeView.Size = new System.Drawing.Size(240, 551);
            this.runTreeView.TabIndex = 0;
            this.runTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.runTreeView_AfterSelect);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.runTabPage);
            this.mainTabControl.Controls.Add(this.generationTabPage);
            this.mainTabControl.Controls.Add(this.speciesTabPage);
            this.mainTabControl.Controls.Add(this.genomeTabPage);
            this.mainTabControl.Controls.Add(this.runSettingsTabPage);
            this.mainTabControl.Controls.Add(this.cefTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(585, 551);
            this.mainTabControl.TabIndex = 0;
            // 
            // runTabPage
            // 
            this.runTabPage.Controls.Add(this.tableLayoutPanel1);
            this.runTabPage.Controls.Add(this.panel1);
            this.runTabPage.Location = new System.Drawing.Point(4, 22);
            this.runTabPage.Name = "runTabPage";
            this.runTabPage.Size = new System.Drawing.Size(577, 525);
            this.runTabPage.TabIndex = 5;
            this.runTabPage.Text = "Run";
            this.runTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.runGenomeSizeChart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.runFitnessChart, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(577, 495);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // runGenomeSizeChart
            // 
            this.runGenomeSizeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runGenomeSizeChart.Location = new System.Drawing.Point(3, 250);
            this.runGenomeSizeChart.Name = "runGenomeSizeChart";
            this.runGenomeSizeChart.Size = new System.Drawing.Size(571, 242);
            this.runGenomeSizeChart.TabIndex = 2;
            this.runGenomeSizeChart.Text = "+";
            // 
            // runFitnessChart
            // 
            this.runFitnessChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runFitnessChart.Location = new System.Drawing.Point(3, 3);
            this.runFitnessChart.Name = "runFitnessChart";
            this.runFitnessChart.Size = new System.Drawing.Size(571, 241);
            this.runFitnessChart.TabIndex = 0;
            this.runFitnessChart.Text = "Fitness";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.runTimeLabel);
            this.panel1.Controls.Add(this.startStopRunButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 495);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 30);
            this.panel1.TabIndex = 3;
            // 
            // runTimeLabel
            // 
            this.runTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runTimeLabel.AutoSize = true;
            this.runTimeLabel.Location = new System.Drawing.Point(381, 8);
            this.runTimeLabel.Name = "runTimeLabel";
            this.runTimeLabel.Size = new System.Drawing.Size(112, 13);
            this.runTimeLabel.TabIndex = 3;
            this.runTimeLabel.Text = "Run time: 00.00:00:00";
            // 
            // startStopRunButton
            // 
            this.startStopRunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startStopRunButton.Location = new System.Drawing.Point(499, 3);
            this.startStopRunButton.Name = "startStopRunButton";
            this.startStopRunButton.Size = new System.Drawing.Size(75, 23);
            this.startStopRunButton.TabIndex = 0;
            this.startStopRunButton.Text = "Start";
            this.startStopRunButton.UseVisualStyleBackColor = true;
            this.startStopRunButton.Click += new System.EventHandler(this.startStopRunButton_Click);
            // 
            // generationTabPage
            // 
            this.generationTabPage.Controls.Add(this.tableLayoutPanel3);
            this.generationTabPage.Controls.Add(this.panel2);
            this.generationTabPage.Location = new System.Drawing.Point(4, 22);
            this.generationTabPage.Name = "generationTabPage";
            this.generationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generationTabPage.Size = new System.Drawing.Size(577, 525);
            this.generationTabPage.TabIndex = 2;
            this.generationTabPage.Text = "Generation";
            this.generationTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.generationFitnessChart, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 24);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(571, 498);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // generationFitnessChart
            // 
            this.generationFitnessChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generationFitnessChart.Location = new System.Drawing.Point(3, 3);
            this.generationFitnessChart.Name = "generationFitnessChart";
            this.generationFitnessChart.Size = new System.Drawing.Size(565, 243);
            this.generationFitnessChart.TabIndex = 0;
            this.generationFitnessChart.Text = "cartesianChart1";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.generationGenomePerSpeciesChart, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.generationGenomeSizeChart, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 252);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(565, 243);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // generationGenomePerSpeciesChart
            // 
            this.generationGenomePerSpeciesChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generationGenomePerSpeciesChart.Location = new System.Drawing.Point(3, 3);
            this.generationGenomePerSpeciesChart.Name = "generationGenomePerSpeciesChart";
            this.generationGenomePerSpeciesChart.Size = new System.Drawing.Size(276, 237);
            this.generationGenomePerSpeciesChart.TabIndex = 0;
            this.generationGenomePerSpeciesChart.Text = "pieChart1";
            // 
            // generationGenomeSizeChart
            // 
            this.generationGenomeSizeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generationGenomeSizeChart.Location = new System.Drawing.Point(285, 3);
            this.generationGenomeSizeChart.Name = "generationGenomeSizeChart";
            this.generationGenomeSizeChart.Size = new System.Drawing.Size(277, 237);
            this.generationGenomeSizeChart.TabIndex = 1;
            this.generationGenomeSizeChart.Text = "cartesianChart1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.generationBestGenomeLinkLabel);
            this.panel2.Controls.Add(this.genTabGenomeCountLabel);
            this.panel2.Controls.Add(this.genTabGenerationNameLabel);
            this.panel2.Controls.Add(this.genTabSpeciesCountLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(571, 21);
            this.panel2.TabIndex = 2;
            // 
            // generationBestGenomeLinkLabel
            // 
            this.generationBestGenomeLinkLabel.AutoSize = true;
            this.generationBestGenomeLinkLabel.Location = new System.Drawing.Point(271, 0);
            this.generationBestGenomeLinkLabel.Name = "generationBestGenomeLinkLabel";
            this.generationBestGenomeLinkLabel.Size = new System.Drawing.Size(155, 13);
            this.generationBestGenomeLinkLabel.TabIndex = 2;
            this.generationBestGenomeLinkLabel.TabStop = true;
            this.generationBestGenomeLinkLabel.Text = "Best Genome Fitness: 0000000";
            this.generationBestGenomeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.generationBestGenomeLinkLabel_LinkClicked);
            // 
            // genTabGenomeCountLabel
            // 
            this.genTabGenomeCountLabel.AutoSize = true;
            this.genTabGenomeCountLabel.Location = new System.Drawing.Point(183, 0);
            this.genTabGenomeCountLabel.Name = "genTabGenomeCountLabel";
            this.genTabGenomeCountLabel.Size = new System.Drawing.Size(82, 13);
            this.genTabGenomeCountLabel.TabIndex = 2;
            this.genTabGenomeCountLabel.Text = "Genomes: 0000";
            // 
            // genTabGenerationNameLabel
            // 
            this.genTabGenerationNameLabel.AutoSize = true;
            this.genTabGenerationNameLabel.Location = new System.Drawing.Point(3, 0);
            this.genTabGenerationNameLabel.Name = "genTabGenerationNameLabel";
            this.genTabGenerationNameLabel.Size = new System.Drawing.Size(99, 13);
            this.genTabGenerationNameLabel.TabIndex = 0;
            this.genTabGenerationNameLabel.Text = "Generation #00000";
            // 
            // genTabSpeciesCountLabel
            // 
            this.genTabSpeciesCountLabel.AutoSize = true;
            this.genTabSpeciesCountLabel.Location = new System.Drawing.Point(108, 0);
            this.genTabSpeciesCountLabel.Name = "genTabSpeciesCountLabel";
            this.genTabSpeciesCountLabel.Size = new System.Drawing.Size(69, 13);
            this.genTabSpeciesCountLabel.TabIndex = 1;
            this.genTabSpeciesCountLabel.Text = "Species: 000";
            // 
            // speciesTabPage
            // 
            this.speciesTabPage.Controls.Add(this.tableLayoutPanel5);
            this.speciesTabPage.Controls.Add(this.panel3);
            this.speciesTabPage.Location = new System.Drawing.Point(4, 22);
            this.speciesTabPage.Name = "speciesTabPage";
            this.speciesTabPage.Size = new System.Drawing.Size(577, 525);
            this.speciesTabPage.TabIndex = 3;
            this.speciesTabPage.Text = "Species";
            this.speciesTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.speciesTabFitnessChart, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 18);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(577, 507);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // speciesTabFitnessChart
            // 
            this.speciesTabFitnessChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.speciesTabFitnessChart.Location = new System.Drawing.Point(3, 3);
            this.speciesTabFitnessChart.Name = "speciesTabFitnessChart";
            this.speciesTabFitnessChart.Size = new System.Drawing.Size(571, 247);
            this.speciesTabFitnessChart.TabIndex = 0;
            this.speciesTabFitnessChart.Text = "cartesianChart1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.speciesTabBestGenomeLinkLabel);
            this.panel3.Controls.Add(this.speciesTabGenomeCountLabel);
            this.panel3.Controls.Add(this.speciesTabSpeciesCountLabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(577, 18);
            this.panel3.TabIndex = 0;
            // 
            // speciesTabBestGenomeLinkLabel
            // 
            this.speciesTabBestGenomeLinkLabel.AutoSize = true;
            this.speciesTabBestGenomeLinkLabel.Location = new System.Drawing.Point(170, 2);
            this.speciesTabBestGenomeLinkLabel.Name = "speciesTabBestGenomeLinkLabel";
            this.speciesTabBestGenomeLinkLabel.Size = new System.Drawing.Size(149, 13);
            this.speciesTabBestGenomeLinkLabel.TabIndex = 2;
            this.speciesTabBestGenomeLinkLabel.TabStop = true;
            this.speciesTabBestGenomeLinkLabel.Text = "Best Genome Fitness: 000000";
            this.speciesTabBestGenomeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.speciesTabBestGenomeLinkLabel_LinkClicked);
            // 
            // speciesTabGenomeCountLabel
            // 
            this.speciesTabGenomeCountLabel.AutoSize = true;
            this.speciesTabGenomeCountLabel.Location = new System.Drawing.Point(82, 2);
            this.speciesTabGenomeCountLabel.Name = "speciesTabGenomeCountLabel";
            this.speciesTabGenomeCountLabel.Size = new System.Drawing.Size(82, 13);
            this.speciesTabGenomeCountLabel.TabIndex = 1;
            this.speciesTabGenomeCountLabel.Text = "Genomes: 0000";
            // 
            // speciesTabSpeciesCountLabel
            // 
            this.speciesTabSpeciesCountLabel.AutoSize = true;
            this.speciesTabSpeciesCountLabel.Location = new System.Drawing.Point(3, 2);
            this.speciesTabSpeciesCountLabel.Name = "speciesTabSpeciesCountLabel";
            this.speciesTabSpeciesCountLabel.Size = new System.Drawing.Size(73, 13);
            this.speciesTabSpeciesCountLabel.TabIndex = 0;
            this.speciesTabSpeciesCountLabel.Text = "Species #000";
            // 
            // genomeTabPage
            // 
            this.genomeTabPage.Controls.Add(this.panel4);
            this.genomeTabPage.Controls.Add(this.genomeTabFitnessLabel);
            this.genomeTabPage.Controls.Add(this.genomeNameLabel);
            this.genomeTabPage.Location = new System.Drawing.Point(4, 22);
            this.genomeTabPage.Name = "genomeTabPage";
            this.genomeTabPage.Size = new System.Drawing.Size(577, 525);
            this.genomeTabPage.TabIndex = 4;
            this.genomeTabPage.Text = "Genome";
            this.genomeTabPage.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.AutoScroll = true;
            this.panel4.Controls.Add(this.genomePictureBox);
            this.panel4.Location = new System.Drawing.Point(94, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(484, 525);
            this.panel4.TabIndex = 3;
            // 
            // genomePictureBox
            // 
            this.genomePictureBox.Location = new System.Drawing.Point(0, 0);
            this.genomePictureBox.Name = "genomePictureBox";
            this.genomePictureBox.Size = new System.Drawing.Size(116, 159);
            this.genomePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.genomePictureBox.TabIndex = 2;
            this.genomePictureBox.TabStop = false;
            // 
            // genomeTabFitnessLabel
            // 
            this.genomeTabFitnessLabel.AutoSize = true;
            this.genomeTabFitnessLabel.Location = new System.Drawing.Point(3, 13);
            this.genomeTabFitnessLabel.Name = "genomeTabFitnessLabel";
            this.genomeTabFitnessLabel.Size = new System.Drawing.Size(76, 13);
            this.genomeTabFitnessLabel.TabIndex = 1;
            this.genomeTabFitnessLabel.Text = "Fitness: 00000";
            // 
            // genomeNameLabel
            // 
            this.genomeNameLabel.AutoSize = true;
            this.genomeNameLabel.Location = new System.Drawing.Point(3, 0);
            this.genomeNameLabel.Name = "genomeNameLabel";
            this.genomeNameLabel.Size = new System.Drawing.Size(75, 13);
            this.genomeNameLabel.TabIndex = 0;
            this.genomeNameLabel.Text = "Genome #000";
            // 
            // runSettingsTabPage
            // 
            this.runSettingsTabPage.Controls.Add(this.algorithmsGroupBox);
            this.runSettingsTabPage.Controls.Add(this.label10);
            this.runSettingsTabPage.Controls.Add(this.resetButtonRunSettings);
            this.runSettingsTabPage.Controls.Add(this.runVariablesGroupBox);
            this.runSettingsTabPage.Controls.Add(this.luaScriptTextBox);
            this.runSettingsTabPage.Controls.Add(this.saveButtonRunSettings);
            this.runSettingsTabPage.Controls.Add(this.gameInstancesUpDown);
            this.runSettingsTabPage.Controls.Add(this.luaScriptBrowseButton);
            this.runSettingsTabPage.Controls.Add(this.label5);
            this.runSettingsTabPage.Controls.Add(this.groupBox1);
            this.runSettingsTabPage.Controls.Add(this.gameModeComboBox);
            this.runSettingsTabPage.Controls.Add(this.label2);
            this.runSettingsTabPage.Location = new System.Drawing.Point(4, 22);
            this.runSettingsTabPage.Name = "runSettingsTabPage";
            this.runSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.runSettingsTabPage.Size = new System.Drawing.Size(577, 525);
            this.runSettingsTabPage.TabIndex = 6;
            this.runSettingsTabPage.Text = "Run Settings";
            this.runSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // algorithmsGroupBox
            // 
            this.algorithmsGroupBox.Controls.Add(this.label13);
            this.algorithmsGroupBox.Controls.Add(this.label12);
            this.algorithmsGroupBox.Controls.Add(this.nBestUpDown);
            this.algorithmsGroupBox.Controls.Add(this.reproductionsPerGenomeUpDown);
            this.algorithmsGroupBox.Controls.Add(this.label11);
            this.algorithmsGroupBox.Controls.Add(this.reproductionSelectionComboBox);
            this.algorithmsGroupBox.Location = new System.Drawing.Point(242, 188);
            this.algorithmsGroupBox.Name = "algorithmsGroupBox";
            this.algorithmsGroupBox.Size = new System.Drawing.Size(305, 99);
            this.algorithmsGroupBox.TabIndex = 10;
            this.algorithmsGroupBox.TabStop = false;
            this.algorithmsGroupBox.Text = "Algorithms";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "N best:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(138, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Reproductions per genome:";
            // 
            // nBestUpDown
            // 
            this.nBestUpDown.Location = new System.Drawing.Point(179, 71);
            this.nBestUpDown.Name = "nBestUpDown";
            this.nBestUpDown.Size = new System.Drawing.Size(120, 20);
            this.nBestUpDown.TabIndex = 3;
            // 
            // reproductionsPerGenomeUpDown
            // 
            this.reproductionsPerGenomeUpDown.Location = new System.Drawing.Point(179, 45);
            this.reproductionsPerGenomeUpDown.Name = "reproductionsPerGenomeUpDown";
            this.reproductionsPerGenomeUpDown.Size = new System.Drawing.Size(120, 20);
            this.reproductionsPerGenomeUpDown.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Reproduction selection:";
            // 
            // reproductionSelectionComboBox
            // 
            this.reproductionSelectionComboBox.FormattingEnabled = true;
            this.reproductionSelectionComboBox.Location = new System.Drawing.Point(178, 18);
            this.reproductionSelectionComboBox.Name = "reproductionSelectionComboBox";
            this.reproductionSelectionComboBox.Size = new System.Drawing.Size(121, 21);
            this.reproductionSelectionComboBox.TabIndex = 0;
            this.reproductionSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.reproductionSelectionComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Lua script:";
            // 
            // resetButtonRunSettings
            // 
            this.resetButtonRunSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetButtonRunSettings.Location = new System.Drawing.Point(415, 496);
            this.resetButtonRunSettings.Name = "resetButtonRunSettings";
            this.resetButtonRunSettings.Size = new System.Drawing.Size(75, 23);
            this.resetButtonRunSettings.TabIndex = 9;
            this.resetButtonRunSettings.Text = "Reset";
            this.resetButtonRunSettings.UseVisualStyleBackColor = true;
            this.resetButtonRunSettings.Click += new System.EventHandler(this.resetButtonRunSettings_Click);
            // 
            // runVariablesGroupBox
            // 
            this.runVariablesGroupBox.Controls.Add(this.eitherDisabledUpDown);
            this.runVariablesGroupBox.Controls.Add(this.connectionCreationUpDown);
            this.runVariablesGroupBox.Controls.Add(this.nodeCreationUpDown);
            this.runVariablesGroupBox.Controls.Add(this.weightPerturbanceUpDown);
            this.runVariablesGroupBox.Controls.Add(this.label9);
            this.runVariablesGroupBox.Controls.Add(this.weightMutationUpDown);
            this.runVariablesGroupBox.Controls.Add(this.label8);
            this.runVariablesGroupBox.Controls.Add(this.label7);
            this.runVariablesGroupBox.Controls.Add(this.label6);
            this.runVariablesGroupBox.Controls.Add(this.label1);
            this.runVariablesGroupBox.Location = new System.Drawing.Point(9, 188);
            this.runVariablesGroupBox.Name = "runVariablesGroupBox";
            this.runVariablesGroupBox.Size = new System.Drawing.Size(227, 150);
            this.runVariablesGroupBox.TabIndex = 8;
            this.runVariablesGroupBox.TabStop = false;
            this.runVariablesGroupBox.Text = "Variables";
            // 
            // eitherDisabledUpDown
            // 
            this.eitherDisabledUpDown.DecimalPlaces = 4;
            this.eitherDisabledUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.eitherDisabledUpDown.Location = new System.Drawing.Point(119, 123);
            this.eitherDisabledUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eitherDisabledUpDown.Name = "eitherDisabledUpDown";
            this.eitherDisabledUpDown.Size = new System.Drawing.Size(97, 20);
            this.eitherDisabledUpDown.TabIndex = 1;
            this.eitherDisabledUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.eitherDisabledUpDown.ValueChanged += new System.EventHandler(this.eitherDisabledUpDown_ValueChanged);
            // 
            // connectionCreationUpDown
            // 
            this.connectionCreationUpDown.DecimalPlaces = 4;
            this.connectionCreationUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.connectionCreationUpDown.Location = new System.Drawing.Point(119, 97);
            this.connectionCreationUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.connectionCreationUpDown.Name = "connectionCreationUpDown";
            this.connectionCreationUpDown.Size = new System.Drawing.Size(97, 20);
            this.connectionCreationUpDown.TabIndex = 1;
            this.connectionCreationUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.connectionCreationUpDown.ValueChanged += new System.EventHandler(this.connectionCreationUpDown_ValueChanged);
            // 
            // nodeCreationUpDown
            // 
            this.nodeCreationUpDown.DecimalPlaces = 4;
            this.nodeCreationUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nodeCreationUpDown.Location = new System.Drawing.Point(119, 71);
            this.nodeCreationUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nodeCreationUpDown.Name = "nodeCreationUpDown";
            this.nodeCreationUpDown.Size = new System.Drawing.Size(97, 20);
            this.nodeCreationUpDown.TabIndex = 1;
            this.nodeCreationUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nodeCreationUpDown.ValueChanged += new System.EventHandler(this.nodeCreationUpDown_ValueChanged);
            // 
            // weightPerturbanceUpDown
            // 
            this.weightPerturbanceUpDown.DecimalPlaces = 4;
            this.weightPerturbanceUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.weightPerturbanceUpDown.Location = new System.Drawing.Point(119, 45);
            this.weightPerturbanceUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.weightPerturbanceUpDown.Name = "weightPerturbanceUpDown";
            this.weightPerturbanceUpDown.Size = new System.Drawing.Size(97, 20);
            this.weightPerturbanceUpDown.TabIndex = 1;
            this.weightPerturbanceUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.weightPerturbanceUpDown.ValueChanged += new System.EventHandler(this.weightPerturbanceUpDown_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Either disabled:";
            // 
            // weightMutationUpDown
            // 
            this.weightMutationUpDown.DecimalPlaces = 4;
            this.weightMutationUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.weightMutationUpDown.Location = new System.Drawing.Point(119, 19);
            this.weightMutationUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.weightMutationUpDown.Name = "weightMutationUpDown";
            this.weightMutationUpDown.Size = new System.Drawing.Size(97, 20);
            this.weightMutationUpDown.TabIndex = 1;
            this.weightMutationUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.weightMutationUpDown.ValueChanged += new System.EventHandler(this.weightMutationUpDown_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Connection creation:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Node creation:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Weight perturbance:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Weight mutation:";
            // 
            // luaScriptTextBox
            // 
            this.luaScriptTextBox.Location = new System.Drawing.Point(68, 58);
            this.luaScriptTextBox.Name = "luaScriptTextBox";
            this.luaScriptTextBox.Size = new System.Drawing.Size(198, 20);
            this.luaScriptTextBox.TabIndex = 6;
            // 
            // saveButtonRunSettings
            // 
            this.saveButtonRunSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButtonRunSettings.Location = new System.Drawing.Point(496, 496);
            this.saveButtonRunSettings.Name = "saveButtonRunSettings";
            this.saveButtonRunSettings.Size = new System.Drawing.Size(75, 23);
            this.saveButtonRunSettings.TabIndex = 0;
            this.saveButtonRunSettings.Text = "Save";
            this.saveButtonRunSettings.Click += new System.EventHandler(this.saveButtonRunSettings_Click);
            // 
            // gameInstancesUpDown
            // 
            this.gameInstancesUpDown.Location = new System.Drawing.Point(98, 32);
            this.gameInstancesUpDown.Name = "gameInstancesUpDown";
            this.gameInstancesUpDown.Size = new System.Drawing.Size(49, 20);
            this.gameInstancesUpDown.TabIndex = 7;
            this.gameInstancesUpDown.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // luaScriptBrowseButton
            // 
            this.luaScriptBrowseButton.Location = new System.Drawing.Point(272, 56);
            this.luaScriptBrowseButton.Name = "luaScriptBrowseButton";
            this.luaScriptBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.luaScriptBrowseButton.TabIndex = 4;
            this.luaScriptBrowseButton.Text = "Browse";
            this.luaScriptBrowseButton.UseVisualStyleBackColor = true;
            this.luaScriptBrowseButton.Click += new System.EventHandler(this.luaScriptBrowseButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Game instances:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.populationSizeUpDown);
            this.groupBox1.Controls.Add(this.generationFileTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.randomFirstGenCheckBox);
            this.groupBox1.Controls.Add(this.browseFirstGenButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 97);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "First Generation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Initial population size:";
            // 
            // populationSizeUpDown
            // 
            this.populationSizeUpDown.Location = new System.Drawing.Point(119, 42);
            this.populationSizeUpDown.Name = "populationSizeUpDown";
            this.populationSizeUpDown.Size = new System.Drawing.Size(52, 20);
            this.populationSizeUpDown.TabIndex = 7;
            this.populationSizeUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // generationFileTextBox
            // 
            this.generationFileTextBox.Enabled = false;
            this.generationFileTextBox.Location = new System.Drawing.Point(93, 68);
            this.generationFileTextBox.Name = "generationFileTextBox";
            this.generationFileTextBox.Size = new System.Drawing.Size(198, 20);
            this.generationFileTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Generation File:";
            // 
            // randomFirstGenCheckBox
            // 
            this.randomFirstGenCheckBox.AutoSize = true;
            this.randomFirstGenCheckBox.Checked = true;
            this.randomFirstGenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.randomFirstGenCheckBox.Location = new System.Drawing.Point(6, 19);
            this.randomFirstGenCheckBox.Name = "randomFirstGenCheckBox";
            this.randomFirstGenCheckBox.Size = new System.Drawing.Size(165, 17);
            this.randomFirstGenCheckBox.TabIndex = 3;
            this.randomFirstGenCheckBox.Text = "Auto-generate first generation";
            this.randomFirstGenCheckBox.UseVisualStyleBackColor = true;
            this.randomFirstGenCheckBox.CheckedChanged += new System.EventHandler(this.randomFirstGenCheckBox_CheckedChanged);
            // 
            // browseFirstGenButton
            // 
            this.browseFirstGenButton.Enabled = false;
            this.browseFirstGenButton.Location = new System.Drawing.Point(297, 66);
            this.browseFirstGenButton.Name = "browseFirstGenButton";
            this.browseFirstGenButton.Size = new System.Drawing.Size(75, 23);
            this.browseFirstGenButton.TabIndex = 4;
            this.browseFirstGenButton.Text = "Browse";
            this.browseFirstGenButton.UseVisualStyleBackColor = true;
            this.browseFirstGenButton.Click += new System.EventHandler(this.browseFirstGenButton_Click);
            // 
            // gameModeComboBox
            // 
            this.gameModeComboBox.FormattingEnabled = true;
            this.gameModeComboBox.Location = new System.Drawing.Point(80, 5);
            this.gameModeComboBox.Name = "gameModeComboBox";
            this.gameModeComboBox.Size = new System.Drawing.Size(125, 21);
            this.gameModeComboBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Game Mode:";
            // 
            // cefTabPage
            // 
            this.cefTabPage.Location = new System.Drawing.Point(4, 22);
            this.cefTabPage.Name = "cefTabPage";
            this.cefTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.cefTabPage.Size = new System.Drawing.Size(577, 525);
            this.cefTabPage.TabIndex = 7;
            this.cefTabPage.Text = "CEF";
            this.cefTabPage.UseVisualStyleBackColor = true;
            // 
            // runBackgroundWorker
            // 
            this.runBackgroundWorker.WorkerReportsProgress = true;
            this.runBackgroundWorker.WorkerSupportsCancellation = true;
            this.runBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.runBackgroundWorker_DoWork);
            // 
            // runTimer
            // 
            this.runTimer.Interval = 1050;
            this.runTimer.Tick += new System.EventHandler(this.runTimer_Tick);
            // 
            // openFirstGenFileDialog
            // 
            this.openFirstGenFileDialog.Filter = "JSON files|*.json|All files|*.*";
            // 
            // openLuaScriptDialog
            // 
            this.openLuaScriptDialog.Filter = "Lua files|*.lua|All files|*.*";
            // 
            // reloadCEFToolStripMenuItem
            // 
            this.reloadCEFToolStripMenuItem.Name = "reloadCEFToolStripMenuItem";
            this.reloadCEFToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reloadCEFToolStripMenuItem.Text = "Reload CEF";
            this.reloadCEFToolStripMenuItem.Click += new System.EventHandler(this.reloadCEFToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // runJSToolStripMenuItem
            // 
            this.runJSToolStripMenuItem.Name = "runJSToolStripMenuItem";
            this.runJSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runJSToolStripMenuItem.Text = "Run JS";
            this.runJSToolStripMenuItem.Click += new System.EventHandler(this.runJSToolStripMenuItem_Click);
            // 
            // GenomeViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 575);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "GenomeViewForm";
            this.Text = "SuperIntelligence";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenomeViewForm_FormClosing);
            this.Load += new System.EventHandler(this.GenomeViewForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mainTabControl.ResumeLayout(false);
            this.runTabPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.generationTabPage.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.speciesTabPage.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.genomeTabPage.ResumeLayout(false);
            this.genomeTabPage.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.genomePictureBox)).EndInit();
            this.runSettingsTabPage.ResumeLayout(false);
            this.runSettingsTabPage.PerformLayout();
            this.algorithmsGroupBox.ResumeLayout(false);
            this.algorithmsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nBestUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reproductionsPerGenomeUpDown)).EndInit();
            this.runVariablesGroupBox.ResumeLayout(false);
            this.runVariablesGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eitherDisabledUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionCreationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeCreationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightPerturbanceUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightMutationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameInstancesUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.populationSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speciesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView runTreeView;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage generationTabPage;
        private System.Windows.Forms.TabPage speciesTabPage;
        private System.Windows.Forms.TabPage genomeTabPage;
        private System.Windows.Forms.TabPage runTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LiveCharts.WinForms.CartesianChart runFitnessChart;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastGenerationToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label genTabGenerationNameLabel;
        private System.Windows.Forms.Label genTabSpeciesCountLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label genTabGenomeCountLabel;
        private LiveCharts.WinForms.CartesianChart generationFitnessChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private LiveCharts.WinForms.PieChart generationGenomePerSpeciesChart;
        private LiveCharts.WinForms.CartesianChart generationGenomeSizeChart;
        private System.Windows.Forms.LinkLabel generationBestGenomeLinkLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private LiveCharts.WinForms.CartesianChart speciesTabFitnessChart;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.LinkLabel speciesTabBestGenomeLinkLabel;
        private System.Windows.Forms.Label speciesTabGenomeCountLabel;
        private System.Windows.Forms.Label speciesTabSpeciesCountLabel;
        private System.Windows.Forms.PictureBox genomePictureBox;
        private System.Windows.Forms.Label genomeTabFitnessLabel;
        private System.Windows.Forms.Label genomeNameLabel;
        private System.Windows.Forms.ToolStripMenuItem clearDataToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker runBackgroundWorker;
        private System.Windows.Forms.Timer runTimer;
        private System.Windows.Forms.OpenFileDialog openFirstGenFileDialog;
        private System.Windows.Forms.TabPage runSettingsTabPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox gameModeComboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label runTimeLabel;
        private System.Windows.Forms.Button startStopRunButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown populationSizeUpDown;
        private System.Windows.Forms.TextBox generationFileTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox randomFirstGenCheckBox;
        private System.Windows.Forms.Button browseFirstGenButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown gameInstancesUpDown;
        private System.Windows.Forms.Label label5;
        private LiveCharts.WinForms.CartesianChart runGenomeSizeChart;
        private System.Windows.Forms.Button saveButtonRunSettings;
        private System.Windows.Forms.GroupBox runVariablesGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown weightMutationUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown connectionCreationUpDown;
        private System.Windows.Forms.NumericUpDown nodeCreationUpDown;
        private System.Windows.Forms.NumericUpDown weightPerturbanceUpDown;
        private System.Windows.Forms.NumericUpDown eitherDisabledUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox luaScriptTextBox;
        private System.Windows.Forms.Button luaScriptBrowseButton;
        private System.Windows.Forms.OpenFileDialog openLuaScriptDialog;
        private System.Windows.Forms.Button resetButtonRunSettings;
        private System.Windows.Forms.GroupBox algorithmsGroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox reproductionSelectionComboBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nBestUpDown;
        private System.Windows.Forms.NumericUpDown reproductionsPerGenomeUpDown;
        private System.Windows.Forms.TabPage cefTabPage;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDevToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadCEFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runJSToolStripMenuItem;
    }
}