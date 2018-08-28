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

using Newtonsoft.Json;

namespace SuperIntelligence
{
    public partial class ApplicationSettingsForm : Form
    {
        public string GameFile
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public string DataDirectory
        {
            get => textBox2.Text;
            set => textBox2.Text = value;
        }
        public string GraphVizBinDirectory
        {
            get => textBox3.Text;
            set => textBox3.Text = value;
        }

        public ApplicationSettingsForm()
        {
            InitializeComponent();
        }

        public ApplicationSettingsForm(string gameFile, string dataDirectory) : this()
        {
            GameFile = gameFile;
            DataDirectory = dataDirectory;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                GameFile = dialog.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = DataDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
                DataDirectory = dialog.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = DataDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
                GraphVizBinDirectory = dialog.SelectedPath;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))
                {
                    MessageBox.Show("Invalid file!");
                    return;
                }

                using (StreamReader reader = new StreamReader(dialog.FileName))
                {
                    var data = JsonConvert.DeserializeObject<Data.AplicationSettingsData>(reader.ReadToEnd());
                    LoadData(data);
                }
            }
        }

        public Data.AplicationSettingsData ToData()
        {
            return new Data.AplicationSettingsData()
            {
                GameFile = GameFile,
                GraphVizBinDirectory = GraphVizBinDirectory,
                DataDirectory = DataDirectory
            };
        }

        public void LoadData(Data.AplicationSettingsData data)
        {
            GameFile = data.GameFile;
            DataDirectory = data.DataDirectory;
            GraphVizBinDirectory = data.GraphVizBinDirectory;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.FileName = "settings.json";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(dialog.FileName))
                {
                    writer.Write(JsonConvert.SerializeObject(ToData()));
                }
            }
        }
    }
}
