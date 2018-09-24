using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SuperIntelligence;
using SuperIntelligence.Game;

using static SuperIntelligence.Data.Constants;

namespace SuperIntelligence
{
    class AIRunner
    {
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
                .Where(wall => wall.Distance > 160 && wall.Enabled != 0)
                .OrderBy(wall => wall.Distance);

            int slotCount = Game.SlotCount;
            for (int i = 0; i < slotCount; i++)
            {
                int index = 2 * i;

                // add slot distance to input
                double distance = 4.0;
                foreach (Wall wall in closer)
                {
                    if (wall.Slot == i)
                    {
                        distance = (wall.Distance - 150) / 1000f;
                        break;
                    }
                }
                input[index] = distance;

                // add angle between player and slot to input
                int slotAngle = (i * (360/slotCount)) + (180/slotCount);
                int playerAngle = Game.PlayerAngle;
                int playerSlotAngle = Math.Max(slotAngle, playerAngle) - Math.Min(slotAngle, playerAngle);
                if (playerSlotAngle > 180)
                    playerSlotAngle = 360 - playerSlotAngle;

                double playerSlotRad = ((Math.PI / 180) * playerSlotAngle);
                double playerSlotRadSigned = playerAngle - playerSlotAngle == slotAngle ?
                    playerSlotRad
                    : -playerSlotRad;

                input[index + 1] = playerSlotRadSigned;
            }

            // in case slotCount was lesser than 6, we copy the first values for the last
            // slots, making it "cyclical"
            for (int i = slotCount * 2; i < 12; i++)
                input[i] = input[i - (slotCount * 2)];


            double playerAngleRad = ((Math.PI / 180) * Game.PlayerAngle);
            input[NetworkInputs - 2] = playerAngleRad;

            // add the final bias parameter
            input[NetworkInputs - 1] = 1;
        }

        private int FarthestWallSlot(List<double> input)
        {
            int farthestSlot = 0;
            double farthestDistance = input[0];
            
            for (int i = 1; i < Game.SlotCount; i++)
            {
                if (input[i * 2] > farthestDistance)
                    farthestSlot = i;
            }

            return farthestSlot;
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
            while (!Game.GameEnded)
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
                    if (input[lastPlayerSlot * 2] < input[playerSlot * 2])
                        fitness += 10;
                    // if the current slot's wall is closer than the last's, remove some points
                    else if (input[lastPlayerSlot] > input[playerSlot * 2])
                        fitness -= 12;
                }
                lastPlayerSlot = playerSlot;

                // wait for next tick
                CurrentTick++;
                // increase fitness since it has lived longer
                Thread.Sleep(1000 / TicksPerSecond);
            }

            // we lost, so let's update the game state
            Manager.UpdateState(GameStates.GameOver);

            // remove input set before
            Game.MouseLeft = false;
            Game.MouseRight = false;
            Game.MouseDown = false;

            // save the individual's fitness
            //Individual.Fitness = fitness + Game.GameTime;
            Individual.Fitness = Game.GameTime + fitness;
        }
    }
}
