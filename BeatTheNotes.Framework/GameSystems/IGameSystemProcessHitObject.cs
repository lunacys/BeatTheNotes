using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.Objects;

namespace BeatTheNotes.Framework.GameSystems
{
    public interface IGameSystemProcessHitObject
    {
        void OnHitObjectHit(object sender, HitObjectOnHitEventArgs args);
    }
}