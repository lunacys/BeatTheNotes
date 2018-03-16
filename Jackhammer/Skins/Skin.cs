using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.Skins
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

        public List<Texture2D> NoteClickTextures { get; } = new List<Texture2D>(8);
        public List<Texture2D> NoteHoldTextures { get; } = new List<Texture2D>(8);

    }
}
