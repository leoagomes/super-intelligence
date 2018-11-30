using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Immutable;

using Newtonsoft.Json;

using Shields.GraphViz.Models;
using Shields.GraphViz.Components;

using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

using SuperIntelligence.NEAT;
using SuperIntelligence.Other;
using SuperIntelligence.Data;
using SuperIntelligence.Game;
using static SuperIntelligence.Data.Constants;

namespace SuperIntelligence
{
    public partial class GenomeViewForm : Form
    {
        Data.AplicationSettingsData applicationSettings;
        DateTime runStartTime = DateTime.Now;
        bool runStarted = false;
        Game.GameModes gameMode;

        ChartValues<ObservablePoint> runMaxFitnessValues = new ChartValues<ObservablePoint>();
        ChartValues<ObservablePoint> runAvgFitnessValues = new ChartValues<ObservablePoint>();
        ChartValues<ObservablePoint> runAvgGenomeSizeValues = new ChartValues<ObservablePoint>();
        ChartValues<ObservablePoint> runMaxGenomeSizeValues = new ChartValues<ObservablePoint>();
        ChartValues<ObservablePoint> runBestGenomeSizeValues = new ChartValues<ObservablePoint>();

        Dictionary<Genome, TreeNode> genomeMap = new Dictionary<Genome, TreeNode>();
        Runner runner;
        Individual bestIndividual = null;
        RunSettingsController settings;

        public GenomeViewForm()
        {
            InitializeComponent();
            settings = new RunSettingsController();
        }

        #region Helper Methods
        private void EnsureApplicationSettings()
        {
            string settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            if (File.Exists(settingsFilePath))
            {
                using (StreamReader reader = new StreamReader(settingsFilePath))
                {
                    applicationSettings = JsonConvert.DeserializeObject<Data.AplicationSettingsData>(reader.ReadToEnd());
                }
            }
            else
            {
                ApplicationSettingsForm form = new ApplicationSettingsForm();
                form.DataDirectory = Directory.GetCurrentDirectory();
                do
                {
                    MessageBox.Show("Initial configuration needed, please provide the following information.", "Setup needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } while (form.ShowDialog() != DialogResult.OK);
                applicationSettings = form.ToData();
            }
        }

        /// <summary>
        /// Clears datum showed at the program (e.g. charts).
        /// </summary>
        private void ClearData()
        {
            runTreeView.Nodes.Clear();
            speciesTabFitnessChart.Series.Clear();
            generationGenomeSizeChart.Series.Clear();
            generationFitnessChart.Series.Clear();
            generationGenomePerSpeciesChart.Series.Clear();

            runAvgFitnessValues.Clear();
            runMaxFitnessValues.Clear();
            runBestGenomeSizeValues.Clear();

            runAvgGenomeSizeValues.Clear();
            runMaxGenomeSizeValues.Clear();
        }

        /// <summary>
        /// Adds the generation, species and genomes to the 'Run' tab.
        /// </summary>
        /// <param name="generation"></param>
        private void AddGeneration(Generation generation)
        {
            // creates the generation node
            TreeNode generationNode = new TreeNode("Generation " + generation.Number);
            generationNode.Tag = generation;

            foreach (Species species in generation.Species)
            {
                // creates the specie node
                TreeNode speciesNode = generationNode.Nodes.Add("Species " + species.Id);
                speciesNode.Tag = species;

                foreach (Genome genome in species.Members)
                {
                    // creates the genome node
                    TreeNode genomeNode = speciesNode.Nodes.Add("Genome " + genome.Id);
                    genomeNode.Tag = genome;

                    genomeMap[genome] = genomeNode;
                }
            }

            runTreeView.Nodes.Add(generationNode);
        }

        /// <summary>
        /// Adds generation info to the program (e.g. charts).
        /// </summary>
        /// <param name="generation"></param>
        private void GenerationFinished(Generation generation)
        {
            // add fitness data to run graphs
            var allGenomes = generation.Species.SelectMany(s => s.Members);
            var bestGenome = allGenomes.Max();
            double meanFitness = allGenomes.Average(g => g.Fitness);
            double maxFitness = bestGenome.Fitness;

            runAvgFitnessValues.Add(new ObservablePoint(generation.Number, meanFitness));
            runMaxFitnessValues.Add(new ObservablePoint(generation.Number, maxFitness));

            // add genome size data to run graphs
            double meanSize = allGenomes.Average(g => g.Size);
            double maxSize = allGenomes.Max(g => g.Size);

            runAvgGenomeSizeValues.Add(new ObservablePoint(generation.Number, meanSize));
            runMaxGenomeSizeValues.Add(new ObservablePoint(generation.Number, maxSize));
            runBestGenomeSizeValues.Add(new ObservablePoint(generation.Number, bestGenome.Size));
        }

        /// <summary>
        /// Marks an untested individual at 'Run' tab by coloring its label text background with yellow.
        /// </summary>
        /// <param name="individual"></param>
        private void MarkUntested(Individual individual)
        {
            if (genomeMap.ContainsKey(individual.Genome))
            {
                genomeMap[individual.Genome].BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// Marks a tested individual at 'Run' tab by removing the yellow background.
        /// </summary>
        /// <param name="individual"></param>
        private void MarkTested(Individual individual)
        {
            if (genomeMap.ContainsKey(individual.Genome))
            {
                genomeMap[individual.Genome].BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// Updates the 'bestIndividual' variable and shows its info at 'Generation' and 'Species' tabs.
        /// </summary>
        /// <param name="individual"></param>
        private void BestGenome(Individual individual)
        {
            if (bestIndividual == null)
            {
                bestIndividual = individual;
            }

            if (individual.Fitness > bestIndividual.Fitness)
            {
                bestIndividual = individual;
            }

            // labels
            // generation tab
            generationBestGenomeLinkLabel.Text = "Best Genome Fitness: " + bestIndividual.Genome.Id.ToString();
            // species tab
            speciesTabBestGenomeLinkLabel.Text = "Best Genome Fitness: " + bestIndividual.Genome.Id.ToString();
        }

        /// <summary>
        /// Generates the genome graph to be shown at 'Genome' tab.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        private async Task<MemoryStream> MakeGenomeGraphImage(Genome genome)
        {
            List<Statement> statements = new List<Statement>();
            Dictionary<Id, Id> dict = new Dictionary<Id, Id>();
            Id labelId = new Id("label");

            foreach (var connection in genome.Connections.Values)
            {
                if (!connection.Expressed)
                    continue;

                dict[labelId] = new Id(connection.Weight.ToString("#.####"));
                statements.Add(new EdgeStatement(connection.InputNode.ToString(), connection.OutputNode.ToString(),
                    dict.ToImmutableDictionary()));
            }

            Dictionary<Id, Id> inputNodeAttributes = new Dictionary<Id, Id>();
            inputNodeAttributes[new Id("shape")] = new Id("invtriangle");

            foreach (var node in genome.Nodes.Values)
            {
                ImmutableDictionary<Id, Id> attributes;

                switch (node.Type)
                {
                    case NodeType.Input:
                        attributes = inputNodeAttributes.ToImmutableDictionary();
                        break;
                    default:
                        attributes = ImmutableDictionary<Id, Id>.Empty;
                        break;
                }

                statements.Add(new NodeStatement(new Id(node.Id.ToString()), attributes));
            }

            statements.Add(new GraphPropertyStatement(new Id("splines"), new Id("false"), ImmutableDictionary<Id, Id>.Empty));

            Graph graph = new Graph(GraphKinds.Directed, new Id("Genome"), statements.ToImmutableList());

            Renderer renderer = new Renderer(applicationSettings.GraphVizBinDirectory);
            MemoryStream memoryStream = new MemoryStream();
            await renderer.RunAsync(graph, memoryStream, Shields.GraphViz.Services.RendererLayouts.Dot,
                Shields.GraphViz.Services.RendererFormats.Png, System.Threading.CancellationToken.None);
            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Changes the text and enables/disables the Start/Stop button at 'Rub' tab.
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="text"></param>
        private void ChangeStartStopButton(bool enabled, string text)
        {
            startStopRunButton.Text = text;
            startStopRunButton.Enabled = enabled;
        }

        /// <summary>
        /// Changes the StartStop button from "Wait..." to "Start" again, stops the timer and cancels async worker.
        /// runBackgroundWorker async.
        /// </summary>
        private void ChangeWaitButton()
        {
            if (runner != null && runner.ShouldStop)
            {
                ChangeStartStopButton(true, "Start");
                runTimer.Stop();
                runBackgroundWorker.CancelAsync();
            }
        }
        #endregion

        #region UI Methods
        /// <summary>
        /// Populate the UI with information (e.g. create the gamemodes' list, allocate space for charts).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenomeViewForm_Load(object sender, EventArgs e)
        {
            EnsureApplicationSettings();

            // populate game modes
            Enum.GetNames(typeof(Game.GameModes))
                .ToList()
                .ForEach(name => gameModeComboBox.Items.Add(name));

            // populate selection algorithms
            Enum.GetNames(typeof(SelectionAlgorithms))
                .ToList()
                .ForEach(name => reproductionSelectionComboBox.Items.Add(name));

            // bootstrap run fitness chart
            runFitnessChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = runAvgFitnessValues,
                    Title = "Average Fitness"
                },
                new LineSeries
                {
                    Values = runMaxFitnessValues,
                    Title = "Max Fitness"
                }
            };

            // bootstrap run genome size chart
            runGenomeSizeChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = runAvgGenomeSizeValues,
                    Title = "Average Genome Size"
                },
                new LineSeries
                {
                    Values = runMaxGenomeSizeValues,
                    Title = "Maximum Genome Size"
                },
                new LineSeries
                {
                    Values = runBestGenomeSizeValues,
                    Title = "Best Genome Size"
                }
            };

            SetUIRunSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationSettingsForm form = new ApplicationSettingsForm();
            form.LoadData(applicationSettings);
            if (form.ShowDialog() == DialogResult.OK)
            {
                applicationSettings = form.ToData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e) =>
            ClearData();

        /// <summary>
        /// Controls the timer from 'Run' tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runTimer_Tick(object sender, EventArgs e)
        {
            var difference = DateTime.Now - runStartTime;
            runTimeLabel.Text = String.Format("Run time: {0:D2}.{1:D2}:{2:D2}:{3:D2}", difference.Days, difference.Hours,
                difference.Minutes, difference.Seconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseFirstGenButton_Click(object sender, EventArgs e)
        {
            if (openFirstGenFileDialog.ShowDialog() == DialogResult.OK)
                generationFileTextBox.Text = openFirstGenFileDialog.FileName;
        }

        /// <summary>
        /// Sets RunSettings variables to the UI.
        /// </summary>
        private void SetUIRunSettings()
        {
            gameModeComboBox.SelectedIndex = settings.s.gameMode;
            gameInstancesUpDown.Value = settings.s.gameInstances;
            luaScriptTextBox.Text = settings.s.luaScriptFile;

            // Variables
            weightMutationUpDown.Value = settings.s.weightMutation;
            Genome.WeightMutationProbability = (double)settings.s.weightMutation;
            weightPerturbanceUpDown.Value = settings.s.weightPerturbance;
            Genome.WeightPerturbanceProbability = (double)settings.s.weightPerturbance;
            nodeCreationUpDown.Value = settings.s.nodeCreation;
            Genome.NodeCreationProbability = (double)settings.s.nodeCreation;
            connectionCreationUpDown.Value = settings.s.connectionCreation;
            Genome.ConnectionCreationProbability = (double)settings.s.connectionCreation;
            eitherDisabledUpDown.Value = settings.s.eitherDisabled;
            Genome.EitherDisabledChance = (double)settings.s.eitherDisabled;

            // First generation
            randomFirstGenCheckBox.Checked = settings.s.autoGenerate;
            populationSizeUpDown.Value = settings.s.initPopSize;
            generationFileTextBox.Text = settings.s.generationFile;

            // Algorithms
            reproductionSelectionComboBox.SelectedIndex = settings.s.selectionAlgorithm;
            reproductionsPerGenomeUpDown.Value = settings.s.reproductionsPerGenome;
            nBestUpDown.Value = settings.s.nBest;
        }

        /// <summary>
        /// Gets 'Run Settings' tab variables.
        /// </summary>
        private void getUIRunSettings()
        {
            settings.s.gameMode = gameModeComboBox.SelectedIndex;
            settings.s.gameInstances = int.Parse(gameInstancesUpDown.Value.ToString());
            settings.s.luaScriptFile = luaScriptTextBox.Text;

            // Variables
            settings.s.weightMutation = weightMutationUpDown.Value;
            settings.s.weightPerturbance = weightPerturbanceUpDown.Value;
            settings.s.nodeCreation = nodeCreationUpDown.Value;
            settings.s.connectionCreation = connectionCreationUpDown.Value;
            settings.s.eitherDisabled = eitherDisabledUpDown.Value;

            // First generation
            settings.s.autoGenerate = randomFirstGenCheckBox.Checked;
            settings.s.initPopSize = int.Parse(populationSizeUpDown.Value.ToString());
            settings.s.generationFile = generationFileTextBox.Text;

            // Algorithms
            settings.s.selectionAlgorithm = reproductionSelectionComboBox.SelectedIndex;
            settings.s.reproductionsPerGenome = reproductionsPerGenomeUpDown.Value;
            settings.s.nBest = nBestUpDown.Value;
        }
        #endregion

        #region Runner Delegate Methods
        /// <summary>
        /// Called when an individual is tested at Runner.
        /// </summary>
        /// <param name="individual"></param>
        private void Runner_OnIndividualTested(Individual individual)
        {
            runTreeView.Invoke(new Runner.IndividualDelegate(BestGenome), new object[] { individual });
            runTreeView.Invoke(new Runner.IndividualDelegate(MarkTested), new object[] { individual });
        }

        /// <summary>
        /// Called from Runner to change a individual to unmarked at the UI.
        /// </summary>
        /// <param name="individual"></param>
        private void Runner_OnUntestedIndividual(Individual individual)
        {
            runTreeView.Invoke(new Runner.IndividualDelegate(MarkUntested), new object[] { individual });
        }

        /// <summary>
        /// Called when Runner finishes the tests on a generation.
        /// </summary>
        /// <param name="generation"></param>
        private void Runner_OnGenerationFinished(Generation generation)
        {
            runFitnessChart.Invoke(new Runner.GenerationDelegate(GenerationFinished), new object[] { generation });
        }

        /// <summary>
        /// Called at Runner to generate the next generation to be tested.
        /// </summary>
        /// <param name="generation"></param>
        private void Runner_OnNextGeneration(Generation generation)
        {
            runTreeView.Invoke(new Runner.GenerationDelegate(AddGeneration), new object[] { generation });
            runTreeView.Invoke(new Runner.GenerationDelegate(GenerateGenerationTabDesign), new object[] { generation });
        }

        /// <summary>
        /// Called when the current generation is tested and the Stop button had been pressed. 
        /// </summary>
        private void Runner_OnLoopFinish()
        {
            runTreeView.Invoke(new Runner.LoopFinishDelegate(ChangeWaitButton));
        }
        #endregion

        #region Tab Design Methods
        /// <summary>
        /// Adds info to 'Genome' tab (e.g.: charts, genome id, its fitness).
        /// </summary>
        /// <param name="genome"></param>
        private void GenerateGenomeTabDesign (Genome genome)
        {
            // statistics labels
            genomeNameLabel.Text = "Genome " + genome.Id;
            genomeTabFitnessLabel.Text = "Fitness: " + genome.Fitness;

            genomePictureBox.UseWaitCursor = true;
            MakeGenomeGraphImage(genome)
                .ContinueWith(mem =>
                {
                    genomePictureBox.Invoke(new Action(() =>
                    {
                        genomePictureBox.Image = Image.FromStream(mem.Result);
                        genomePictureBox.UseWaitCursor = false;
                    }));
                });
        }

        /// <summary>
        /// Adds info to 'Species' tab (e.g.: charts).
        /// </summary>
        /// <param name="species"></param>
        private void GenerateSpeciesTabDesign(Species species)
        {
            Dictionary<double, int> counts = new Dictionary<double, int>();
            ColumnSeries series = new ColumnSeries();

            foreach (Genome g in species.Members)
            {
                double fitnessBucket = Math.Floor(g.Fitness / 10.0);

                if (!counts.ContainsKey(fitnessBucket))
                    counts[fitnessBucket] = 0;
                counts[fitnessBucket]++;
            }

            speciesTabFitnessChart.Series.Clear();
            speciesTabFitnessChart.AxisX.Clear();

            var labels = counts.Keys.OrderBy(d => d);

            speciesTabFitnessChart.AxisX.Add(new Axis
            {
                Title = "Fitness",
                Labels = labels.Select(f => f.ToString()).ToList()
            });

            // creating the series
            series.Values = new ChartValues<int>();
            series.Title = "Species Fitness Distribution";
            foreach (var index in labels)
            {
                series.Values.Add(counts[index]);
            }

            speciesTabFitnessChart.Series.Add(series);
        }

        /// <summary>
        /// Adds info to 'Generation' tab (e.g.: generation number, # of species and genomes).
        /// </summary>
        /// <param name="generation"></param>
        private void GenerateGenerationTabDesign(Generation generation)
        {
            int genCount = 0;

            foreach (Species s in generation.Species)
            {
                genCount += s.Members.Count;
            }

            // statistics labels
            // since other tabs need general statistics from the generation, we modify them too 
            // generation tab
            genTabGenerationNameLabel.Text = "Generation " + generation.Number.ToString();
            genTabSpeciesCountLabel.Text = "Species: " + generation.Species.Count.ToString();
            genTabGenomeCountLabel.Text = "Genomes: " + genCount.ToString();
            // species tab
            speciesTabSpeciesCountLabel.Text = "Species: " + generation.Species.Count.ToString();
            speciesTabGenomeCountLabel.Text = "Genomes: " + genCount.ToString();

            // pie chart
            generationGenomePerSpeciesChart.Series.Clear();
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            foreach (Species s in generation.Species)
            {
                generationGenomePerSpeciesChart.Series.Add(new PieSeries

                {
                    Title = s.Id.ToString(),
                    Values = new ChartValues<double> { s.Members.Count },
                    DataLabels = true,
                    LabelPoint = labelPoint
                });
            }

            // fitness chart
            generationFitnessChart.Series.Clear();
            generationFitnessChart.AxisX.Clear();
            generationFitnessChart.AxisY.Clear();

            OhlcSeries series = new OhlcSeries
            {
                Title = "Fitness Distribution",
                Values = new ChartValues<OhlcPoint>()
            };
            double globalMean = 0;

            foreach (Species s in generation.Species)
            {
                double speciesAverageFitness = s.Members.Sum(g => g.Fitness) / s.Members.Count;
                globalMean += speciesAverageFitness;

                double speciesMax = s.Members.Max(g => g.Fitness);
                double speciesMin = s.Members.Min(g => g.Fitness);

                series.Values.Add(new OhlcPoint(speciesAverageFitness, speciesMax, speciesMin, speciesAverageFitness));
            }

            globalMean /= generation.Species.Count == 0 ? 1 : generation.Species.Count;

            generationFitnessChart.AxisY.Add(new Axis
            {
                Sections = new SectionsCollection
                    {
                        new AxisSection
                        {
                            Value = globalMean,
                            Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 49, 49))
                        }
                    }
            });
            generationFitnessChart.Series = new SeriesCollection { series };

            // genome size chart
            generationGenomeSizeChart.Series.Clear();
            generationGenomeSizeChart.AxisX.Clear();
            generationGenomeSizeChart.AxisY.Clear();

            series = new OhlcSeries
            {
                Values = new ChartValues<OhlcPoint>()
            };
            globalMean = 0;

            foreach (Species s in generation.Species)
            {
                double avgGenomeSize = s.Members.Sum(g => g.Connections.Count + g.Nodes.Count) / s.Members.Count;
                double minGenomeSize = s.Members.Min(g => g.Connections.Count + g.Nodes.Count);
                double maxGenomeSize = s.Members.Max(g => g.Connections.Count + g.Nodes.Count);

                series.Values.Add(new OhlcPoint(avgGenomeSize, maxGenomeSize, minGenomeSize, avgGenomeSize));
            }

            globalMean /= generation.Species.Count == 0 ? 1 : generation.Species.Count;

            generationGenomeSizeChart.AxisY.Add(new Axis
            {
                Sections = new SectionsCollection
                    {
                        new AxisSection
                        {
                            Value = globalMean,
                            Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 49, 49))
                        }
                    }
            });
            generationGenomeSizeChart.Series = new SeriesCollection { series };
        }
        #endregion

        #region Interface Click/Selection Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Tag is Genome)
            {
                Genome genome = node.Tag as Genome;
                GenerateGenomeTabDesign(genome);
                // changing to the genome tab
                mainTabControl.SelectedTab = genomeTabPage;
                Console.WriteLine("Genome id: {0}", genome.Id);
            }
            else if (node.Tag is Species)
            {
                Species species = node.Tag as Species;
                GenerateSpeciesTabDesign(species);

            }
            else if (node.Tag is Generation)
            {
                Generation generation = node.Tag as Generation;
                GenerateGenerationTabDesign(generation);
            }

            if (e.Node.Parent != null)
            {
                runTreeView_AfterSelect(sender, new TreeViewEventArgs(e.Node.Parent));
            }
        }

        #region Generation
        /// <summary>
        /// 'Generation' tab -> 'Best Genome Fitness' click handler, changes the active tab to 'Genome'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generationBestGenomeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // changing to the genome tab
            mainTabControl.SelectedTab = genomeTabPage;
            // regenerating genome tab
            GenerateGenomeTabDesign(bestIndividual.Genome);
        }
        #endregion

        #region Species
        /// <summary>
        /// 'Species' tab -> 'Best Genome Fitness' click handler, changes the active tab to 'Genome'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speciesTabBestGenomeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // changing to the genome tab
            mainTabControl.SelectedTab = genomeTabPage;
            // regenerating genome tab
            GenerateGenomeTabDesign(bestIndividual.Genome);
        }
        #endregion

        #region Run
        /// <summary>
        /// Start/Stop button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startStopRunButton_Click(object sender, EventArgs e)
        {
            // when the user press the Start button
            if (!runStarted)
            {
                // clear all data in UI
                ClearData();

                if (!randomFirstGenCheckBox.Checked && generationFileTextBox.Text == string.Empty)
                {
                    MessageBox.Show("Invalid first generation.", "Error starting run.");
                    return;
                }

                // check game mode
                if (!Enum.TryParse(gameModeComboBox.Text, out gameMode))
                {
                    MessageBox.Show("Invalid game mode '" + gameModeComboBox.Text + "'.", "Error starting run.");
                    return;
                }

                // start the work
                runBackgroundWorker.RunWorkerAsync(randomFirstGenCheckBox.Checked ?
                    string.Empty : generationFileTextBox.Text);

                // save the current time and reveal and start the timer
                runStartTime = DateTime.Now;
                runTimer.Start();

                ChangeStartStopButton(true, "Stop");
                runStarted = true;
            }

            // when the user press the Stop button
            else
            {
                // stopping the runner loop
                if (runner != null)
                {
                    runner.ShouldStop = true;
                }

                ChangeStartStopButton(false, "Wait...");
                runStarted = false;
            }
        }
        #endregion

        #region Run Settings
        /// <summary>
        /// 'Run Settings' tab -> 'Auto-generate first generation' check box handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randomFirstGenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;

            browseFirstGenButton.Enabled = !box.Checked;
            generationFileTextBox.Enabled = !box.Checked;

            populationSizeUpDown.Enabled = box.Checked;
        }

        /// <summary>
        /// 'Run Settings' tab -> 'Save' button click handler, save settings into a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButtonRunSettings_Click(object sender, EventArgs e)
        {
            getUIRunSettings();
            settings.SaveSettings();
        }

        /// <summary>
        /// 'Run Settings' tab -> 'Reset' button click handler, reset Run Settings on UI without saving into a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButtonRunSettings_Click(object sender, EventArgs e)
        {
            settings.s.ResetSettings();
            SetUIRunSettings();
        }

        /// <summary>
        /// 'Run Settings' tab -> 'Weight mutation' UpDown field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weightMutationUpDown_ValueChanged(object sender, EventArgs e) =>
            Genome.WeightMutationProbability = (double)weightMutationUpDown.Value;

        /// <summary>
        /// 'Run Settings' tab -> 'Weight perturbance' UpDown field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weightPerturbanceUpDown_ValueChanged(object sender, EventArgs e) =>
            Genome.WeightPerturbanceProbability = (double)weightPerturbanceUpDown.Value;

        /// <summary>
        /// 'Run Settings' tab -> 'Node creation' UpDown field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeCreationUpDown_ValueChanged(object sender, EventArgs e) =>
            Genome.NodeCreationProbability = (double)nodeCreationUpDown.Value;

        /// <summary>
        /// 'Run Settings' tab -> 'Connection creation' UpDown field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionCreationUpDown_ValueChanged(object sender, EventArgs e) =>
            Genome.ConnectionCreationProbability = (double)connectionCreationUpDown.Value;

        /// <summary>
        /// 'Run Settings' tab -> 'Either disabled' UpDown field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eitherDisabledUpDown_ValueChanged(object sender, EventArgs e) =>
            Genome.EitherDisabledChance = (double)eitherDisabledUpDown.Value;

        /// <summary>
        /// Lua script browse button handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void luaScriptBrowseButton_Click(object sender, EventArgs e)
        {
            if (openLuaScriptDialog.ShowDialog() == DialogResult.OK)
                luaScriptTextBox.Text = openLuaScriptDialog.FileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reproductionSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (reproductionSelectionComboBox.SelectedIndex)
            {
                case (int)SelectionAlgorithms.Luhn:
                    nBestUpDown.Enabled = true;
                    reproductionsPerGenomeUpDown.Enabled = true;
                    break;

                case (int)SelectionAlgorithms.NWithBest:
                    nBestUpDown.Enabled = true;
                    reproductionsPerGenomeUpDown.Enabled = false;
                    break;

                case (int)SelectionAlgorithms.AllWithBest:
                    nBestUpDown.Enabled = false;
                    reproductionsPerGenomeUpDown.Enabled = false;
                    break;
            }
        }
        #endregion

        #endregion

        #region Misc
        /// <summary>
        /// Activates the background worker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string generationFileName = e.Argument as string;
            Generation firstGen;

            runner = new Runner(applicationSettings.GameFile, (int)gameInstancesUpDown.Value, luaScriptTextBox.Text);

            if (generationFileName != string.Empty)
            {
                firstGen = JsonConvert.DeserializeObject<Generation>(File.ReadAllText(generationFileName));
            }
            else
            {
                firstGen = runner.MakeFirstGeneration(runner.InnovationGenerator, runner.GenomeInnovationGenerator, (int)populationSizeUpDown.Value, settings.s.selectionAlgorithm,
                                                      (int)settings.s.reproductionsPerGenome, (int)settings.s.nBest);
            }

            Runner_OnNextGeneration(firstGen);

            runner.OnNextGeneration += Runner_OnNextGeneration;
            runner.OnGenerationFinished += Runner_OnGenerationFinished;
            runner.OnUntestedIndividual += Runner_OnUntestedIndividual;
            runner.OnIndividualTested += Runner_OnIndividualTested;
            runner.OnLoopFinish += Runner_OnLoopFinish;

            runner.DoRun(gameMode, firstGen, settings.s.selectionAlgorithm, (int)settings.s.reproductionsPerGenome,
                         (int)settings.s.nBest);
        }
        #endregion

        private void setGameSpeedButton_Click(object sender, EventArgs e)
        {
            if (runner != null)
            {
                runner.GameSpeed = (double)gameSpeedUpDown.Value;
            }
        }
    }
}
