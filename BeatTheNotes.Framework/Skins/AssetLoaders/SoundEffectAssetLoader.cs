using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    public class SoundEffectAssetLoader : ISkinAssetLoader<SoundEffect>
    {
        public string AssetSubdirectory { get; }

        public SoundEffectAssetLoader()
        {
            AssetSubdirectory = "Sfx";
        }

        public SoundEffect LoadAsset(string assetFilePath)
        {
            SoundEffect sfx;

            using (FileStream fs = new FileStream(assetFilePath, FileMode.Open))
                sfx = SoundEffect.FromStream(fs);

            return sfx;
        }
    }
}