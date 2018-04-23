using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    public class SpriteFontAssetLoader : ISkinAssetLoader<SpriteFont>
    {
        public string AssetSubdirectory { get; }

        public SpriteFontAssetLoader()
        {
            AssetSubdirectory = "Fonts";
        }

        public SpriteFont LoadAsset(string filename)
        {
            throw new System.NotImplementedException();
        }
    }
}