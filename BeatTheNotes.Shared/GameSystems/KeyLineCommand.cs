using System;
using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.GameSystems;

namespace BeatTheNotes.Shared.GameSystems
{
    public class KeyLineCommand :IInputCommand
    {
        public int Line { get; }

        private GameplaySystem _gameplay;

        public KeyLineCommand(GameplaySystem gameplay, int line)
        {
            Line = line;
            _gameplay = gameplay;
        }

        public void Execute()
        {
            var nearest = _gameplay.GetNearestHitObjectOnLine(Line);

            if (nearest != null)
                DoHit(nearest);//_gameplay.FindSystem<ScoreSystem>().Calculate(nearest);
        }

        public void DoHit(HitObject hitObject)
        {
            hitObject.IsPressed = true;
        }
    }
}
