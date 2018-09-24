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

        public GenomeViewForm()
        {
            InitializeComponent();
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

        private void AddGeneration(Generation generation)
        {
            TreeNode generationNode = runTreeView.Nodes.Add("Generation " + generation.Number);
            generationNode.Tag = generation;

            foreach (Species species in generation.Species)
            {
                TreeNode speciesNode = generationNode.Nodes.Add("Species " + species.Id);
                speciesNode.Tag = species;

                foreach (Genome genome in species.Members)
                {
                    TreeNode genomeNode = speciesNode.Nodes.Add("Genome " + genome.Id);
                    genomeNode.Tag = genome;

                    genomeMap[genome] = genomeNode;
                }
            }
        }

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

        private void MarkUntested(Individual individual)
        {
            if (genomeMap.ContainsKey(individual.Genome))
            {
                genomeMap[individual.Genome].BackColor = Color.Yellow;
            }
        }

        private void MarkTested(Individual individual)
        {
            if (genomeMap.ContainsKey(individual.Genome))
            {
                genomeMap[individual.Genome].BackColor = Color.Transparent;
            }
        }

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
        #endregion

        #region UI Methods
        private void GenomeViewForm_Load(object sender, EventArgs e)
        {
            EnsureApplicationSettings();

            // populate game modes
            Enum.GetNames(typeof(Game.GameModes))
                .ToList()
                .ForEach(name => gameModeComboBox.Items.Add(name));
            gameModeComboBox.SelectedIndex = (int)Game.GameModes.Hexagonest;

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
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationSettingsForm form = new ApplicationSettingsForm();
            form.LoadData(applicationSettings);
            if (form.ShowDialog() == DialogResult.OK)
            {
                applicationSettings = form.ToData();
            }
        }

        private void randomFirstGenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;

            browseFirstGenButton.Enabled = !box.Checked;
            generationFileTextBox.Enabled = !box.Checked;

            populationSizeUpDown.Enabled = box.Checked;
        }

        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e) =>
            ClearData();

        private void startStopRunButton_Click(object sender, EventArgs e)
        {
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

                startStopRunButton.Text = "Stop";
                runStarted = true;
            }
            else
            {
                runTimer.Stop();

                runBackgroundWorker.CancelAsync();

                startStopRunButton.Text = "Start";
                runStarted = false;
            }
        }

        private void runTimer_Tick(object sender, EventArgs e)
        {
            var difference = DateTime.Now - runStartTime;
            runTimeLabel.Text = String.Format("Run time: {0:D2}.{1:D2}:{2:D2}:{3:D2}", difference.Days, difference.Hours,
                difference.Minutes, difference.Seconds);
        }

        private void browseFirstGenButton_Click(object sender, EventArgs e)
        {
            if (openFirstGenFileDialog.ShowDialog() == DialogResult.OK)
                generationFileTextBox.Text = openFirstGenFileDialog.FileName;
        }
        #endregion

        private void runBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string generationFileName = e.Argument as string;
            Generation firstGen;

            Runner runner = new Runner(applicationSettings.GameFile, (int)gameInstancesUpDown.Value);

            if (generationFileName != string.Empty)
            {
                firstGen = JsonConvert.DeserializeObject<Generation>(File.ReadAllText(generationFileName));
            }
            else
            {
                firstGen = Runner.MakeFirstGeneration(runner.InnovationGenerator, (int)populationSizeUpDown.Value);
            }

            Runner_OnNextGeneration(firstGen);

            runner.OnNextGeneration += Runner_OnNextGeneration;
            runner.OnGenerationFinished += Runner_OnGenerationFinished;
            runner.OnUntestedIndividual += Runner_OnUntestedIndividual;
            runner.OnIndividualTested += Runner_OnIndividualTested;

            runner.DoRun(gameMode, firstGen);
        }

        private void Runner_OnIndividualTested(Individual individual)
        {
            runTreeView.Invoke(new Runner.IndividualDelegate(MarkTested), new object[] { individual });
        }

        private void Runner_OnUntestedIndividual(Individual individual)
        {
            runTreeView.Invoke(new Runner.IndividualDelegate(MarkUntested), new object[] { individual });
        }

        private void Runner_OnGenerationFinished(Generation generation)
        {
            runFitnessChart.Invoke(new Runner.GenerationDelegate(GenerationFinished), new object[] { generation });
        }

        private void Runner_OnNextGeneration(Generation generation)
        {
            runTreeView.Invoke(new Runner.GenerationDelegate(AddGeneration), new object[] { generation });
        }

        private void runTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Tag is Genome)
            {
                Genome genome = node.Tag as Genome;
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
            else if (node.Tag is Species)
            {
                Species species = node.Tag as Species;

                Dictionary<double, int> counts = new Dictionary<double, int>();
                foreach (Genome g in species.Members)
                {
                    double fitnessBucket = Math.Floor(g.Fitness / 10.0);

                    if (!counts.ContainsKey(fitnessBucket))
                        counts[fitnessBucket] = 0;
                    counts[fitnessBucket]++;
                }

                speciesTabFitnessChart.Series.Clear();
                speciesTabFitnessChart.AxisX.Clear();

                ColumnSeries series = new ColumnSeries();
                series.Title = "Species Fitness Distribution";

                var labels = counts.Keys.OrderBy(d => d);

                speciesTabFitnessChart.AxisX.Add(new Axis
                {
                    Title = "Fitness",
                    Labels = labels.Select(f => f.ToString()).ToList()
                });

                series.Values = new ChartValues<int>();

                foreach (var index in labels)
                {
                    series.Values.Add(counts[index]);
                }

                speciesTabFitnessChart.Series.Add(series);
            }
            else if (node.Tag is Generation)
            {

                Generation generation = node.Tag as Generation;

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

            if (e.Node.Parent != null)
            {
                runTreeView_AfterSelect(sender, new TreeViewEventArgs(e.Node.Parent));
            }
        }
    }
}
