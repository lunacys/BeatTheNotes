using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins
{
    public class Skin
    {
        public SkinSettings Settings { get; set; }

        public Texture2D DefaultBackground { get; set; }

        public Texture2D NoteClickTexture
        {
            get => NoteClickTextures[0];
            set => NoteClickTextures.Add(value);
        }
        
        public Texture2D NoteHoldTexture
        {
            get => NoteHoldTextures[0];
            set => NoteHoldTextures.Add(value);
        }

        public Texture2D PlayfieldLineTexture { get; set; }
        public Texture2D ButtonTexture { get; set; }
        public SpriteFont Font { get; set; }

        public SoundEffect HitNormal { get; set; }
        public SoundEffect ComboBreak { get; set; }

        public List<Texture2D> NoteClickTextures { get; } = new List<Texture2D>(8);
        public List<Texture2D> NoteHoldTextures { get; } = new List<Texture2D>(8);

        public Texture2D ScoreMarvelousTexture { get; set; }
        public Texture2D ScorePerfectTexture { get; set; }
        public Texture2D ScoreGreatTexture { get; set; }
        public Texture2D ScoreGoodTexture { get; set; }
        public Texture2D ScoreBadTexture { get; set; }
        public Texture2D ScoreMissTexture { get; set; }

        public Texture2D HealthBarBg { get; set; }
        public Texture2D HealthBar { get; set; }
    }
}
