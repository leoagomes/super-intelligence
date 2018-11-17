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
        public decimal weightMutation { get; set; }
        public decimal weightPerturbance { get; set; }
        public decimal nodeCreation { get; set; }
        public decimal connectionCreation { get; set; }
        public decimal eitherDisabled { get; set; }
        public string luaScriptFile { get; set; }

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
        /// <param name="generationFile"></param>
        /// <param name="gameMode"></param>
        /// <param name="gameInstances"></param>
        /// <param name="initPopSize"></param>
        /// <param name="autoGenerate"></param>
        /// <param name="weightMutation"></param>
        /// <param name="weightPerturbance"></param>
        /// <param name="nodeCreation"></param>
        /// <param name="connectionCreation"></param>
        /// <param name="eitherDisabled"></param>
        public RunSettings(string generationFile, int gameMode, int gameInstances, int initPopSize, bool autoGenerate,
            decimal weightMutation, decimal weightPerturbance, decimal nodeCreation, decimal connectionCreation,
            decimal eitherDisabled, string luaScriptFile)
        {
            this.gameMode = gameMode;
            this.gameInstances = gameInstances;

            this.initPopSize = initPopSize;
            this.autoGenerate = autoGenerate;
            this.generationFile = generationFile;

            this.weightMutation = weightMutation;
            this.weightPerturbance = weightPerturbance;
            this.nodeCreation = nodeCreation;
            this.connectionCreation = connectionCreation;
            this.eitherDisabled = eitherDisabled;

            this.luaScriptFile = luaScriptFile;
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

            weightMutation = 0.8M;
            weightPerturbance = 0.9M;
            nodeCreation = 0.3M;
            connectionCreation = 0.8M;
            eitherDisabled = 0.25M;

            luaScriptFile = "";
        }
    }
}
