using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    [SkinAssetLoader("Texture", ".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class TextureLoader : ISkinAssetLoader<Texture2D>, ISkinAssetGraphic
    {
        public string AssetSubdirectory { get; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public TextureLoader()
        {
            AssetSubdirectory = "Images";
        }

        public Texture2D LoadAsset(string filename)
        {
            if (GraphicsDevice == null)
                throw new InvalidOperationException("Please initialize GrapicsDevice first");

            Texture2D t;

            using (var fs = new FileStream(filename, FileMode.Open))
                t = Texture2D.FromStream(GraphicsDevice, fs);

            return t;
        }
    }
}