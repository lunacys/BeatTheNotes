using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace BeatTheNotes.Framework.GameAssets.AssetLoaders
{
    [AssetLoader("SoundEffect", "Sfx", "", ".wav")]
    public class SoundEffectLoader : IAssetLoader<SoundEffect>
    {
        public SoundEffect LoadAsset(string assetFilePath)
        {
            SoundEffect sfx;

            using (FileStream fs = new FileStream(assetFilePath, FileMode.Open))
                sfx = SoundEffect.FromStream(fs);

            return sfx;
        }
    }
}