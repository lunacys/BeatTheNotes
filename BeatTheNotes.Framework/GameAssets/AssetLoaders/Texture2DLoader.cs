using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.GameAssets.AssetLoaders
{
    [AssetLoader("Texture2D", "Images", "", ".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class Texture2DLoader : IAssetLoader<Texture2D>, IGraphicalAsset
    {
        public GraphicsDevice GraphicsDevice { get; set; }

        public Texture2D LoadAsset(string assetFilePath)
        {
            if (GraphicsDevice == null)
                throw new InvalidOperationException("Please initialize GraphicsDevice first");

            Texture2D texture;

            using (var fs = new FileStream(assetFilePath, FileMode.Open))
                texture = Texture2D.FromStream(GraphicsDevice, fs);

            return texture;
        }
    }
}