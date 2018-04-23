using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    [SkinAssetLoader(".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class TextureAssetLoader : ISkinAssetLoader<Texture2D>
    {
        public string AssetSubdirectory { get; }
        public GraphicsDevice GraphicsDevice { get; }

        public TextureAssetLoader(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            AssetSubdirectory = "Images";
        }

        public Texture2D LoadAsset(string filename)
        {
            Texture2D t;

            using (var fs = new FileStream(filename, FileMode.Open))
                t = Texture2D.FromStream(GraphicsDevice, fs);

            return t;
        }
    }
}