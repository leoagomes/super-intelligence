using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.Game;

namespace SuperIntelligence.Data
{
    public class RunSettings
    {
        public string generationFile { get; set; }
        public int gameMode { get; set; }
        public int gameInstances { get; set; }
        public int initPopSize { get; set; }
        public bool autoGenerate { get; set; }

        /// <summary>
        /// Default constructor. Generates a default setting.
        /// </summary>
        public RunSettings()
        {
            ResetSettings();
        }

        /// <summary>
        /// Creates a customised setting.
        /// </summary>
        /// <param name="generationFile">Generation file name.</param>
        /// <param name="gameMode"></param>
        /// <param name="gameInstances">Number of simultaneously game instances.</param>
        /// <param name="initPopSize">Initial population size.</param>
        /// <param name="autoGenerate"></param>
        public RunSettings(string generationFile, int gameMode, int gameInstances, int initPopSize, bool autoGenerate)
        {
            this.generationFile = generationFile;
            this.gameMode = gameMode;
            this.gameInstances = gameInstances;
            this.initPopSize = initPopSize;
            this.autoGenerate = autoGenerate;
        }

        /// <summary>
        /// Sets the default configuration.
        /// </summary>
        public void ResetSettings()
        {
            generationFile = "";

            gameMode = (int)GameModes.Hexagonest;
            gameInstances = 5;
            initPopSize = 50;

            autoGenerate = true;
        }
    }
}
