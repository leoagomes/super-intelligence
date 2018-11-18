using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SuperIntelligence.Game;

using static SuperIntelligence.Data.Constants;

namespace SuperIntelligence
{
    class AIRunner
    {
        public static slf4net.ILogger logger = slf4net.LoggerFactory.GetLogger(typeof(AIRunner));
        public static double ButtonDownThreshold = 0.7;
        public static int DefaultTicksPerSecond = 50;

        public GameManager Manager;
        public GameInstance Game;
        public Individual Individual;
        public GameModes Mode;
        public int TicksPerSecond = DefaultTicksPerSecond;
        public int CurrentTick = 0;

        public AIRunner(GameManager manager, Individual individual, GameModes mode)
        {
            Manager = manager;
            Game = manager.Game;
            Individual = individual;
            Mode = mode;
        }

        public void PopulateInputWithGameState(ref List<double> input)
        {
            // network input is the distance between each slot's closest wall and the middle of
            // the screen and the angle between the player ship and the (center of) the slot
            Wall[] walls = Game.Walls;

            var closer = walls
                .Where(wall => wall.Distance > 100 && wall.Enabled != 0)
                .OrderBy(wall => wall.Distance);

            int lastPlayerSlot = Game.GetCurrentPlayerSlot();
            int slotCount = Game.SlotCount;

            for (int i = 0; i < slotCount; i++)
            {
                // add slot distance to input
                double distance = 4.0;
                foreach (Wall wall in closer)
                {
                    if (wall.Slot == (lastPlayerSlot + i) % slotCount)
                    {
                        distance = (wall.Distance - 150) / 1000f;
                        break;
                    }
                }
                input[(lastPlayerSlot + i) % slotCount] = distance;
            }

            // add the final bias parameter
            input[(lastPlayerSlot + NetworkInputs - 1) % slotCount] = 1;
        }

        private int FarthestWallSlot(List<double> input)
        {
            int farthestSlot = 0;
            double farthestDistance = input[0];
            
            for (int i = 1; i < Game.SlotCount; i++)
            {
                if (input[i] > farthestDistance)
                    farthestSlot = i;
            }

            return farthestSlot;
        }

        public void DoSafeRun()
        {
            try
            {
                DoGameRun();
            }
            catch (Exception e)
            {
                logger.Error("Exception caught trying to do an AIRunner Run.\nIndividual was: " + Individual);
                logger.Error("Exception caught was: " + e);
                logger.Warn("Setting failed individual fitness to minimum.");
                Individual.Fitness = int.MinValue;
            }
        }

        public void DoGameRun()
        {
            Individual.Prepare();

            // Set the game window's title
            Game.SetWindowTitle("Gen " + Individual.Generation + ":" + Individual.Index);

            Manager.PrepareForMode(Mode);

            List<double> input = new List<double>(NetworkInputs);
            for (int i = 0; i < NetworkInputs; i++)
                input.Add(0);

            // start game
            Manager.StartGame();

            // fitness function calculation helper variables
            int lastPlayerSlot = Game.GetCurrentPlayerSlot();
            int fitness = 0;

            CurrentTick = 0;
            while (!Game.GameEnded && !Game.GameEnded2)
            {
                // populate the list of inputs with game data
                PopulateInputWithGameState(ref input);

                // run the inputs through the network
                List<double> output = Individual.Forward(input);

                // make sure the number of outputs is correct
                if (output.Count != NetworkOutputs)
                    throw new Exception("Unexpected amount of outputs: " + output.Count);

                // check if mouse left/right should be pressed
                bool mouseLeftDown = output[0] > ButtonDownThreshold;
                bool mouseRightDown = output[1] > ButtonDownThreshold;

                // write it out to game memory
                Game.MouseLeft = mouseLeftDown;
                Game.MouseRight = mouseRightDown;
                Game.MouseDown = mouseRightDown || mouseLeftDown;

                // do fitness calculations
                int slotCount = Game.SlotCount;
                int playerSlot = Game.GetCurrentPlayerSlot();
                int farthestWallSlot = FarthestWallSlot(input);

                // if it is maintaining the slot of the farthest wall, give it more points
                if (playerSlot == lastPlayerSlot && playerSlot == farthestWallSlot)
                    fitness += 10;
                else if (playerSlot == lastPlayerSlot)
                    fitness -= 2;

                // if it just changed slots
                if (playerSlot != lastPlayerSlot)
                {
                    // and the current slot's wall is farther than the last's, git yet more points
                    if (input[lastPlayerSlot] < input[playerSlot])
                        fitness += 10;
                    // if the current slot's wall is closer than the last's, remove some points
                    else if (input[lastPlayerSlot] > input[playerSlot])
                        fitness -= 12;
                }
                lastPlayerSlot = playerSlot;

                // check if it survived a wall closing in
                bool hasCloseWall = false;
                for (int i = 0; i < slotCount; i++)
                {
                    if (input[i] <= 0.0)
                    {
                        hasCloseWall = true;
                        break;
                    }
                }
                if (hasCloseWall)
                    fitness += 50;

                // check if it passed through a small gap
                if (input[(slotCount + (playerSlot - 1)) % slotCount] <= 0.0)
                    fitness += 25;
                if (input[(slotCount + (playerSlot + 1)) % slotCount] <= 0.0)
                    fitness += 25;

                // wait for next tick
                CurrentTick++;
                // increase fitness since it has lived longer
                Thread.Sleep(1000 / TicksPerSecond);
            }

            // we lost, so let's update the game state
            Manager.UpdateState(GameStates.GameOver);
            Thread.Sleep(2000);

            // remove input set before
            Game.MouseLeft = false;
            Game.MouseRight = false;
            Game.MouseDown = false;

            // save the individual's fitness
            //Individual.Fitness = fitness + Game.GameTime;
            Individual.Fitness = (Game.GameTime + fitness) / 60;
        }
    }
}
