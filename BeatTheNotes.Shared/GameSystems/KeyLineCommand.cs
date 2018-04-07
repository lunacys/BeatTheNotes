using System;
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
                _gameplay.FindSystem<ScoreSystem>().Calculate(nearest, null);
        }
    }
}
