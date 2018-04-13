using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Objects;
using BeatTheNotes.GameSystems;

namespace BeatTheNotes.Shared.GameSystems
{
    public class KeyLineCommand : IInputCommand
    {
        public int Line { get; }

        private readonly HitObjectContainer _hitObjectContainer;
        private readonly GameplaySystem _gameplay;

        public KeyLineCommand(GameplaySystem gameplay, HitObjectContainer hitObjectContainer, int line)
        {
            Line = line;
            _hitObjectContainer = hitObjectContainer;
            _gameplay = gameplay;
        }

        public void Execute()
        {
            // TODO: Create SoundEffectsSystem
            //_gameplay.Skin.HitNormal.Play();

            var nearest = _hitObjectContainer.GetNearestHitObjectOnLine(Line,
                _gameplay.FindSystem<GameTimeSystem>().Time,
                _gameplay.FindSystem<ScoreV1System>().HitThresholds["Miss"]);

            if (nearest != null)
                DoHit(nearest);
        }

        private void DoHit(HitObject hitObject)
        {
            hitObject.DoHit();
        }
    }
}
