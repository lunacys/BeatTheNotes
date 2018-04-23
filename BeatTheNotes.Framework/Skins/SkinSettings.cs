using System.Collections.Generic;
using BeatTheNotes.Framework.Settings;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace BeatTheNotes.Framework.Skins
{
    public class SkinSettings
    {
        public string SkinName { get; internal set; }
        public string AuthorName { get; internal set; }

        public string NoteType { get; internal set; }

        public bool IsCursorExpand { get; internal set; }
        public bool HasCursorTrail { get; internal set; }
        public bool IsUpsideDown { get; internal set; }

        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 ScoreSplashPosition { get; internal set; }

        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 ScoreCounterPosition { get; internal set; }

        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 ComboPosition { get; internal set; }

        public List<int> ColumnWidth { get; internal set; }

        public string ImagesDirectory { get; internal set; }
        public string SoundEffectsDirectory { get; internal set; }
        public string SpriteFontsDirectory { get; internal set; }

        public int HitPosition { get; internal set; }
        public int PlayfieldPositionX { get; internal set; }

        /// <summary>
        /// Initialize default settings
        /// </summary>
        public SkinSettings()
        {
            SkinName = "Default";
            AuthorName = "loonacuse";

            NoteType = "Brick";

            IsCursorExpand = false;
            HasCursorTrail = false;
            IsUpsideDown = false;

            ScoreSplashPosition = Vector2.Zero;
            ScoreCounterPosition = Vector2.Zero;
            ComboPosition = Vector2.Zero;

            ColumnWidth = new List<int>()
            {
                0, 0, 0, 0
            };

            ImagesDirectory = "Images";
            SoundEffectsDirectory = "Sfx";
            SpriteFontsDirectory = "Fonts";

            HitPosition = 0;
            PlayfieldPositionX = 200;
        }

        public SkinSettings(GameSettings gameSettings)
        {
            // TODO: Initialize all values by GameSettings
        }
    }
}
