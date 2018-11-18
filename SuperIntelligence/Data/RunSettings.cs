using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperIntelligence.Game;
using static SuperIntelligence.Data.Constants;

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
        public int selectionAlgorithm { get; set; }
        public decimal reproductionsPerGenome { get; set; }
        public decimal nBest { get; set; }

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
        /// <param name="luaScriptFile"></param>
        /// <param name="selectionAlgorithm"></param>
        /// <param name="reproductionsPerGenome"></param>
        /// <param name="nBest"></param>
        public RunSettings(string generationFile, int gameMode, int gameInstances, int initPopSize, bool autoGenerate,
            decimal weightMutation, decimal weightPerturbance, decimal nodeCreation, decimal connectionCreation,
            decimal eitherDisabled, string luaScriptFile, int selectionAlgorithm, decimal reproductionsPerGenome,
            decimal nBest)
        {
            this.gameMode = gameMode;
            this.gameInstances = gameInstances;
            this.luaScriptFile = luaScriptFile;

            // First generation
            this.initPopSize = initPopSize;
            this.autoGenerate = autoGenerate;
            this.generationFile = generationFile;

            // Variables
            this.weightMutation = weightMutation;
            this.weightPerturbance = weightPerturbance;
            this.nodeCreation = nodeCreation;
            this.connectionCreation = connectionCreation;
            this.eitherDisabled = eitherDisabled;

            // Algorithms
            this.selectionAlgorithm = selectionAlgorithm;
            this.reproductionsPerGenome = reproductionsPerGenome;
            this.nBest = nBest;
        }

        /// <summary>
        /// Sets the default configuration.
        /// </summary>
        public void ResetSettings()
        {
            gameMode = (int)GameModes.Hexagonest;
            gameInstances = 5;
            luaScriptFile = "";

            // First generation
            initPopSize = 50;
            autoGenerate = true;
            generationFile = "";

            // Variables
            weightMutation = 0.8M;
            weightPerturbance = 0.9M;
            nodeCreation = 0.3M;
            connectionCreation = 0.8M;
            eitherDisabled = 0.25M;

            // Algorithms
            selectionAlgorithm = (int)SelectionAlgorithms.Luhn;
            reproductionsPerGenome = 2;
            nBest = 25;
        }
    }
}
