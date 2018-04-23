namespace BeatTheNotes.Framework.Skins.AssetLoaders
{
    public interface ISkinAssetLoader<out T>
    {
        string AssetSubdirectory { get; }

        T LoadAsset(string assetFilePath);
    }
}