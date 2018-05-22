using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BeatTheNotes.Framework
{
    public static class Texture2DExtensions
    {
        public static Texture2D Crop(this Texture2D texture, Point position, Size2 size)
        {
            return Crop(texture, position.X, position.Y, (int)size.Width, (int)size.Height);
        }

        public static Texture2D Crop(this Texture2D texture, int x, int y, int w, int h)
        {
            Texture2D dummy = new Texture2D(texture.GraphicsDevice, w, h);
            Color[] data = new Color[w * h];
            texture.GetData(0, new Rectangle(x, y, w, h), data, 0, w * h);
            dummy.SetData(data);
            return dummy;
        }
    }
}