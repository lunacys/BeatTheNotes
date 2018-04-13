using System;
using System.Collections.Generic;
using System.Text;
using BeatTheNotes.Framework.GameSystems;

namespace BeatTheNotes.Shared.GameSystems
{
    public class ScoreV2System : GameSystem
    {
        public string Score320 => "Marvelous";
        public string Score300 => "Perfect";
        public string Score200 => "Great";
        public string Score100 => "Good";
        public string Score50 => "Bad";
        public string Score0 => "Miss";
        
        public int Score { get; private set; }

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }

        public ScoreV2System()
        {
            Combo = 0;
            MaxCombo = 0;
            Score = 0;
        }

        public override void Reset()
        {
            Score = 0;
            Combo = 0;
            MaxCombo = 0;

            base.Reset();
        }
    }
}
