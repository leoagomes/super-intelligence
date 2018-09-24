using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Binarysharp.MemoryManagement.Native;

namespace SuperIntelligence.Game
{
    public enum GameStates
    {
        MainMenu,
        ModeSelection,
        GameOver,
        InGame,
    }

    public enum GameModes : int
    {
        Hexagon = 0,
        Hexagoner = 1,
        Hexagonest = 2,
        HexagonHyper = 3,
        HexagonerHyper = 4,
        HexagonestHyper = 5,
    }

    public class GameManager
    {
        public static int ThreadSleepAmount = 500;
        public static int AnimationTimeSleep = 2000;

        public GameInstance Game;

        public GameModes CurrentMode { get; private set; }
        public GameStates State { get; private set; }

        public GameManager(GameInstance game, GameStates initialState, GameModes initialMode)
        {
            Game = game;
            State = initialState;
            CurrentMode = initialMode;
        }

        private void PressReleaseKey(Keys key)
        {
            Game.PressKey(key);
            Thread.Sleep(ThreadSleepAmount);
            Game.ReleaseKey(key);
            Thread.Sleep(ThreadSleepAmount);
        }

        public void PrepareForMode(GameModes mode)
        {
            switch(State)
            {
                case GameStates.MainMenu:
                    // make transition from main menu to mode selection
                    PressReleaseKey(Keys.Space);
                    State = GameStates.ModeSelection;

                    // make the next transition
                    PrepareForMode(mode);
                    break;
                case GameStates.ModeSelection:
                    // if we are already in the correct game mode, we don't need to change it
                    if (CurrentMode == mode)
                        return;

                    // if we're currently on another mode we need to change to the correct mode.
                    // TODO: enhance this
                    Keys direction = (int)CurrentMode > (int)mode ? Keys.Left : Keys.Right;
                    int amount = Math.Abs((int)CurrentMode - (int)mode);

                    for (int i = 0; i < amount; i++)
                    {
                        PressReleaseKey(direction);
                    }
                    CurrentMode = mode;
                    break;
                case GameStates.GameOver:
                    // if we are already at the mode we want, just stay at the game over screen
                    if (CurrentMode == mode)
                        return;

                    // else, go back to the mode selection menu
                    PressReleaseKey(Keys.Escape);
                    State = GameStates.ModeSelection;

                    // make next transition
                    PrepareForMode(mode);
                    break;
                case GameStates.InGame:
                    // transition to GameOver by pressing esc
                    PressReleaseKey(Keys.Escape);
                    State = GameStates.GameOver;

                    // make next state transition
                    PrepareForMode(mode);
                    break;
                default:
                    break;
            }
        }

        public bool StartGame()
        {
            if (State != GameStates.GameOver &&
                State != GameStates.ModeSelection)
                return false;

            PressReleaseKey(Keys.Space);
            return true;
        }

        public void UpdateState(GameStates state)
        {
            State = state;
        }

        public void Kill() =>
            Game.Kill();
    }
}
