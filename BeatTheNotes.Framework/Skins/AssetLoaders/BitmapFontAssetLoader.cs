using MonoGame.Extended.BitmapFonts;

namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    public class BitmapFontAssetLoader : ISkinAssetLoader<BitmapFont>
    {
        public string AssetSubdirectory { get; }

        public BitmapFontAssetLoader()
        {
            AssetSubdirectory = "BitmapFonts";
        }

        public BitmapFont LoadAsset(string filename)
        {
            //BitmapFont bmFont = new BitmapFont(filename, );

            throw new System.NotImplementedException();
        }
    }
}