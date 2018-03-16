using System;
using System.Collections.Generic;
using System.Text;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;

namespace Jackhammer.GameSystems
{
    public class ScoreSystem : GameSystem
    {
        private GameplayScreen _gameplay;

        public int Score { get; private set; }

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }

        public int MarvelousCount { get; private set; }
        public int PerfectCount { get; private set; }
        public int GreatCount { get; private set; }
        public int GoodCount { get; private set; }
        public int BadCount { get; private set; }
        public int MissCount { get; private set; }

        public float Accuracy { get; private set; }

        public ScoreSystem(GameplayScreen gameplay)
        {
            _gameplay = gameplay;
            Accuracy = 1.0f;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var line in _gameplay.SeparatedLines)
            {
                foreach (var o in line)
                {
                    if (o.IsPressed) continue;

                    if (o.Position + (188 - (3 * _gameplay.Beatmap.Settings.Difficulty.OverallDifficutly)) <
                        _gameplay.Time)
                    {
                        o.IsPressed = true;
                        MissCount++;
                        Combo = 0;
                    }
                }
            }
            
        }

        public bool Calculate(HitObject hitObject)
        {
            int hitVal = GetHitValue(hitObject);
            if (hitVal < 0)
            {
                return false;
            }

            CalculateScore(hitVal);
            CalculateAccuracy();

            return true;
        }

        public void Reset()
        {
            Score = Combo =
                MaxCombo = MarvelousCount = PerfectCount = GreatCount = GoodCount = BadCount = MissCount = 0;

            Accuracy = 1.0f;
        }

        private void CalculateScore(int hitValue)
        {
            float baseScore = (1000000.0f / _gameplay.Beatmap.HitObjects.Count) * (hitValue / 320.0f);
            float totalScore = baseScore;

            Score += (int)totalScore;
        }

        private void ProceedCombo(int hitValue)
        {
            if (hitValue == 0)
            {
                Combo = 0;
            }
            else
            {
                Combo++;
                if (Combo > MaxCombo)
                    MaxCombo = Combo;
            }
        }

        private void CalculateAccuracy()
        {
            float totalPointsOfHits = (BadCount * 50 + GoodCount * 100 +
                                       GreatCount * 200 + PerfectCount * 300 +
                                       MarvelousCount * 300);
            float totalNumberOfHits = (MissCount + BadCount + GoodCount +
                                       GreatCount + PerfectCount + MarvelousCount);

            Accuracy = totalPointsOfHits / (totalNumberOfHits * 300.0f);
        }

        private int GetHitValue(HitObject hitObject)
        {
            int timeOffset = hitObject.Position - _gameplay.Time;
            int absTimeOffset = Math.Abs(timeOffset);
            float od = _gameplay.Beatmap.Settings.Difficulty.OverallDifficutly;

            int score = 0;

            //note->press();
            if (absTimeOffset <= 16)
            {
                MarvelousCount++;
                score = 320;
            }
            else if (absTimeOffset <= 64 - (3 * od))
            {
                PerfectCount++;
                score = 300;
            }
            else if (absTimeOffset <= 97 - (3 * od))
            {
                GreatCount++;
                score = 200;
            }
            else if (absTimeOffset <= 127 - (3 * od))
            {
                GoodCount++;
                score = 100;
            }
            else if (absTimeOffset <= 151 - (3 * od))
            {
                BadCount++;
                score = 50;
            }
            else if (timeOffset <= 188 - (3 * od))
            {
                MissCount++;
                Combo = 0;
                score = 0;
            }
            else if (timeOffset > 188 - (3 * od))
            {
                return -1;
            }
            
            ProceedCombo(score);

            return score;
        }
    }
}
