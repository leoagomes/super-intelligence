using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SuperIntelligence.Game;
using Newtonsoft.Json;

namespace SuperIntelligence.Data
{
    public class RunSettingsController
    {
        public string settingsFile { get; set; }
        public RunSettings s { get; set; }

        #region Methods
        public RunSettingsController()
        {
            settingsFile = Path.Combine(Directory.GetCurrentDirectory(), "runsettings.json");
            this.s = new RunSettings();

            if (File.Exists(settingsFile))
            {
                LoadSettings();
            }
            else
            {
                SaveSettings();
            }
        }

        /// <summary>
        /// Loads 's' from a file.
        /// </summary>
        public void LoadSettings()
        {
            string content = File.ReadAllText(settingsFile);
            this.s = JsonConvert.DeserializeObject<RunSettings>(content);
        }

        /// <summary>
        /// Writes 's' into a file.
        /// </summary>
        public void SaveSettings()
        {
            using (StreamWriter file = File.CreateText(settingsFile))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(file, this.s);
            }
        }
        #endregion
    }
}
