using BeatTheNotes.Framework.Settings;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins
{
    public class SkinAssetDefaultManager : SkinAssetManager
    {
        public ContentManager ContentManager { get; }

        public SkinAssetDefaultManager(
            GraphicsDevice graphicsDevice,
            GameSettings gameSettings,
            ContentManager contentManager)
            : base(graphicsDevice, gameSettings)
        {
            ContentManager = contentManager;
        }

        public override T Load<T>(string assetName)
        {
            return ContentManager.Load<T>(assetName);
        }
    }
}