using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Settings
{
    public class GameSettings
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool IsFullscreen { get; set; }

        public bool IsUsedVSync { get; set; }
        public int TargetFramesPerSecond { get; set; }


        public bool IsReversedDirection { get; set; }

        [JsonIgnore]
        public float HitsoundVolumeF
        {
            get => HitsoundVolume / 100.0f;
            set => HitsoundVolume = (int)(value * 100.0f);
        }

        [JsonIgnore]
        public float SongVolumeF
        {
            get => SongVolume / 100.0f;
            set => SongVolume = (int)(value * 100.0f);
        }

        [JsonIgnore]
        public float ScrollingSpeedF
        {
            get => ScrollingSpeed / 10.0f;
            set => ScrollingSpeed = (int)(value * 10.0f);
        }

        public int HitsoundVolume { get; set; }
        public int SongVolume { get; set; }
        public int ScrollingSpeed { get; set; }

        public string Skin { get; set; }

        public string BeatmapFolder { get; set; }

        /// <summary>
        /// Key for the first line
        /// </summary>
        //public Keys N1 { get; set; }
        /// <summary>
        /// Key for the second line
        /// </summary>
        //public Keys N2 { get; set; }
        /// <summary>
        /// Key for the third line
        /// </summary>
        //public Keys N3 { get; set; }
        /// <summary>
        /// Key for the fourth line
        /// </summary>
        //public Keys N4 { get; set; }

        public Dictionary<string, Keys> GameKeys = new Dictionary<string, Keys>();

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

            ScrollingSpeedF = 1.0f;
            IsReversedDirection = false;

            HitsoundVolumeF = 1.0f;
            SongVolumeF = 1.0f;

            Skin = "Default";

            BeatmapFolder = "Maps";

            //N1 = Keys.Z;
            //N2 = Keys.X;
            //N3 = Keys.OemPeriod;
            //N4 = Keys.OemQuestion;

            GameKeys["KL1"] = Keys.Z;
            GameKeys["KL2"] = Keys.X;
            GameKeys["KL3"] = Keys.OemPeriod;
            GameKeys["KL4"] = Keys.OemQuestion;

            GameKeys["MusicVolumeUp"] = Keys.Up;
            GameKeys["MusicVolumeDown"] = Keys.Down;
            GameKeys["SFXVolumeUp"] = Keys.Right;
            GameKeys["SFXVolumeDown"] = Keys.Down;

            GameKeys["BeatmapMusicBPMUp"] = Keys.F2;
            GameKeys["BeatmapMusicBPMDown"] = Keys.F1;
            GameKeys["BeatmapScrollingSpeedUp"] = Keys.F4;
            GameKeys["BeatmapScrollingSpeedDown"] = Keys.F3;

            GameKeys["BeatmapRestart"] = Keys.OemTilde;
        }
    }
}
