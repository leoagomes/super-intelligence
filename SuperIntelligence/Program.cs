using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;

using Newtonsoft.Json;

using SuperIntelligence.Game;
using SuperIntelligence.NEAT;

using static SuperIntelligence.Random.Random;

namespace SuperIntelligence
{
    class Program
    {
        public Program()
        {
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GenomeViewForm());
        }
    }
}
