using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.Framework.GameAssets.AssetLoaders
{
    public interface IGraphicalAsset
    {
        GraphicsDevice GraphicsDevice { get; set; }
    }
}