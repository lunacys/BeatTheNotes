using BeatTheNotes.Framework.Beatmaps;

namespace BeatTheNotes.Framework.GameSystems
{
    public interface IGameSystemProcessHitObject
    {
        void OnHitObjectHit(object sender, HitObjectOnPressEventArgs args);
    }
}