using System;
using System.Diagnostics;
using System.IO;

using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;

namespace SuperIntelligence.Game
{
    public enum Offsets : int
    {
        Base1 = 0x29EB90,
        Base2 = 0x29B0D4,
        Base3 = 0x29EC1C,
        NumberOfSlots = 0x1C0,
        FirstWall = 0x228,
        NumberOfWalls = 0x2938,
        PlayerAngle = 0x295C,
        WorldAngle = 0x1BC,
        MouseDown = 0x43E15,
        MouseLeft = 0x43A28,
        MouseRight = 0x43A2A,
        GameTime = 0x2990,
        GameEnded = 0x13BC4,
        GameEndedFaster = 0x296C,
    }

    public class Wall
    {
        public static int WallSize = 0x14;

        public int Slot;
        public int Distance;
        public int Length;

        public Wall(int slot, int distance, int length)
        {
            Slot = slot;
            Distance = distance;
            Length = length;
        }
    }

    public class GameInstance
    {
        public MemorySharp Memory;
        Process Process;

        RemotePointer BasePointer;

        #region Game Properties Getters/Setters
        public Wall[] Walls
        {
            get
            {
                return BuildWallList();
            }
        }

        public int SlotCount
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.NumberOfSlots);
            }
        }

        public int WallCount
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.NumberOfWalls);
            }
        }

        public int PlayerAngle
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.PlayerAngle);
            }
            set
            {
                BasePointer.Write((int)Offsets.PlayerAngle, value);
            }
        }

        public int WorldAngle
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.WorldAngle);
            }
        }

        public bool MouseLeft
        {
            get
            {
                return BasePointer.Read<short>((int)Offsets.MouseLeft) == 1;
            }

            set
            {
                BasePointer.Write((int)Offsets.MouseLeft, (short)(value ? 1 : 0));
            }
        }

        public bool MouseRight
        {
            get
            {
                return BasePointer.Read<short>((int)Offsets.MouseRight) == 1;
            }

            set
            {
                BasePointer.Write((int)Offsets.MouseRight, (short)(value ? 1 : 0));
            }
        }

        public bool MouseDown
        {
            get
            {
                return BasePointer.Read<short>((int)Offsets.MouseDown) == 1;
            }

            set
            {
                BasePointer.Write((int)Offsets.MouseDown, (short)(value ? 1 : 0));
            }
        }

        public bool GameEnded
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.GameEnded) == 1;
            }
        }

        public bool GameEnded2
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.GameEnded) == 1;
            }
        }

        public int GameTime
        {
            get
            {
                return BasePointer.Read<int>((int)Offsets.GameTime);
            }
        }
        #endregion

        public GameInstance(Process process)
        {
            Process = process;
            Memory = new MemorySharp(process);

            // Initialize the base pointer, by getting it from the static offset.
            IntPtr baseStaticAddress = new IntPtr((int)Offsets.Base1);
            IntPtr basePtr = (IntPtr)Memory.Read<int>(baseStaticAddress);
            BasePointer = Memory[basePtr, false];
        }


        private Wall[] BuildWallList()
        {
            int wallCount = WallCount;

            if (wallCount <= 0)
                return new Wall[0];

            Wall[] walls = new Wall[wallCount];

            // copy the memory with wall data
            byte[] wallBuffer = BasePointer.Read<byte>((int)Offsets.FirstWall, wallCount * Wall.WallSize);
            BinaryReader reader = new BinaryReader(new MemoryStream(wallBuffer));

            for (int i = 0; i < wallCount; i++)
            {
                int slot = reader.ReadInt32();
                int distance = reader.ReadInt32();
                int enabled = reader.ReadInt32();

                // discard two unknown wall structure values
                reader.ReadInt32();
                reader.ReadInt32();

                walls[i] = new Wall(slot, distance, enabled);
            }

            return walls;
        }

        public void PressKey(Binarysharp.MemoryManagement.Native.Keys key) =>
            Memory.Windows.MainWindow.Keyboard.Press(key);

        public void ReleaseKey(Binarysharp.MemoryManagement.Native.Keys key) =>
            Memory.Windows.MainWindow.Keyboard.Release(key);

        public void PressReleaseKey(Binarysharp.MemoryManagement.Native.Keys key) =>
            Memory.Windows.MainWindow.Keyboard.PressRelease(key);

        public void SetWindowTitle(string title) =>
            Memory.Windows.MainWindow.Title = title;

        public void Kill() =>
            Process.Kill();

        public int GetCurrentPlayerSlot() =>
            ((PlayerAngle * SlotCount) / 360);

        public void SRand(uint seed)
        {
            Memory["msvcr100"]["srand"].Execute(Binarysharp.MemoryManagement.Assembly.CallingConvention.CallingConventions.Cdecl, seed);
        }
    }
}
