namespace BeatTheNotes.Framework.Skins
{
    public interface ISkinAssetLoader<out T>
    {
        string AssetSubdirectory { get; }

        T LoadAsset(string assetFilePath);
    }
}