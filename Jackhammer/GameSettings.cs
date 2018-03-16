using Jackhammer.Skins;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jackhammer
{
    public class GameSettings
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool IsFullscreen { get; set; }

        public bool IsUsedVSync { get; set; }
        public int TargetFramesPerSecond { get; set; }

        public float ScrollingSpeed { get; set; }
        public bool IsReversedDirection { get; set; }

        public float HitsoundVolume { get; set; }
        public float SongVolume { get; set; }

        public string Skin { get; set; }

        /// <summary>
        /// Key for the first line
        /// </summary>
        public Keys N1 { get; set; }
        /// <summary>
        /// Key for the second line
        /// </summary>
        public Keys N2 { get; set; }
        /// <summary>
        /// Key for the third line
        /// </summary>
        public Keys N3 { get; set; }
        /// <summary>
        /// Key for the fourth line
        /// </summary>
        public Keys N4 { get; set; }

        public GameSettings()
        {
            LoadDeafaults();
        }

        public void LoadDeafaults()
        {
            WindowWidth = 1280;
            WindowHeight = 720;
            IsFullscreen = false;

            IsUsedVSync = false;
            TargetFramesPerSecond = 240;

            ScrollingSpeed = 1.0f;
            IsReversedDirection = false;

            HitsoundVolume = 1.0f;
            SongVolume = 1.0f;

            Skin = "Default";

            N1 = Keys.Z;
            N2 = Keys.X;
            N3 = Keys.OemPeriod;
            N4 = Keys.OemQuestion;
        }
    }
}
