using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SuperIntelligence.Game;

namespace SuperIntelligence.Data
{
    public class RunSettings
    {
        string settingsFile;
        string generationFile;
        int gameMode;
        int gameInstances;
        int initPopSize;
        bool autoGenerate;

        public delegate void RunSettingsForm();

        public event RunSettingsForm ReadSettings = delegate { };
        public event RunSettingsForm WriteSettings = delegate { };

        #region Methods
        public RunSettings()
        {
            ResetSettings();
        }

        /// <summary>
        /// Sets the default configuration.
        /// </summary>
        public void ResetSettings ()
        {
            settingsFile = "runsettings.json";
            generationFile = "";

            gameMode = (int)GameModes.Hexagonest;
            gameInstances = 5;
            initPopSize = 50;

            autoGenerate = false;
        }
        #endregion
    }
}
