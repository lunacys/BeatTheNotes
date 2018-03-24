using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.GameSystems
{
    public class Splash
    {
        public Texture2D Texture { get; }
        public int MsBeforeExpire { get; private set; }

        public Splash(Texture2D texture)
        {
            Texture = texture;
            MsBeforeExpire = 900;
        }

        public void Update(GameTime gameTime)
        {
            MsBeforeExpire -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
