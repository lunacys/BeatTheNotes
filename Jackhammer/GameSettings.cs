using Jackhammer.Skin;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        }
    }
}
