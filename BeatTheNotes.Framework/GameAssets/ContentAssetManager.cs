using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.GameAssets
{
    public class ContentAssetManager : AssetManager
    {
        public ContentManager Content { get; }

        public ContentAssetManager(GraphicsDevice graphicsDevice, string assetDirectory, ContentManager contentManager)
            : base(graphicsDevice, assetDirectory)
        {
            Content = contentManager;
        }

        public override T Load<T>(string assetName)
        {
            return Content.Load<T>(Path.Combine(AssetDirectory, assetName));
        }
    }
}