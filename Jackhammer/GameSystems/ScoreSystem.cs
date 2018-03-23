using System;
using System.Collections.Generic;
using System.IO;
using Jackhammer.Framework.Beatmaps;
using Jackhammer.Framework.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public class ScoreSystem : GameSystem
    {
        public string ScoreMarvelous => "Marvelous";
        public string ScorePerfect => "Perfect";
        public string ScoreGreat => "Great";
        public string ScoreGood => "Good";
        public string ScoreBad => "Bad";
        public string ScoreMiss => "Miss";

        public const int MaxScore = 1000000;

        private double _score;
        public int Score => (int)Math.Ceiling(_score);

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }

        public Dictionary<string, double> HitThresholds;
        public Dictionary<string, int> HitValues;
        public Dictionary<string, int> HitPunishments;
        public Dictionary<string, int> HitBonuses;
        public Dictionary<string, int> HitBonusValues;

        public int CurrentBonus;

        public int MarvelousCount { get; private set; }
        public int PerfectCount { get; private set; }
        public int GreatCount { get; private set; }
        public int GoodCount { get; private set; }
        public int BadCount { get; private set; }
        public int MissCount { get; private set; }

        public float Accuracy { get; private set; }

        public float Od => _gameplay.Beatmap.Settings.Difficulty.OverallDifficutly;

        public Splash CurrentSplash { get; private set; }

        private readonly SpriteBatch _spriteBatch;

        private GameplaySystem _gameplay;

        public ScoreSystem(GraphicsDevice graphicsDevice)
        {
            Accuracy = 1.0f;

            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();

            _gameplay = GameSystemManager.FindSystem<GameplaySystem>();

            InitValues();
        }

        private void InitValues()
        {
            CurrentBonus = 100;

            HitThresholds = new Dictionary<string, double>()
            {
                { ScoreMarvelous, 16.0},
                { ScorePerfect, 64 - (3 * Od)},
                { ScoreGreat, 97 - (3 * Od)},
                { ScoreGood, 127 - (3 * Od)},
                { ScoreBad, 151 - (3 * Od)},
                { ScoreMiss, 188 - (3 * Od)},
            };

            HitValues = new Dictionary<string, int>()
            {
                { ScoreMarvelous, 320 },
                { ScorePerfect, 300 },
                { ScoreGreat, 200 },
                { ScoreGood, 100 },
                { ScoreBad, 50 },
                { ScoreMiss, 0 }
            };

            HitBonusValues = new Dictionary<string, int>()
            {
                { ScoreMarvelous, 32 },
                { ScorePerfect, 32 },
                { ScoreGreat, 16 },
                { ScoreGood, 8 },
                { ScoreBad, 4 },
                { ScoreMiss, 0 }
            };

            HitBonuses = new Dictionary<string, int>()
            {
                { ScoreMarvelous, 2 },
                { ScorePerfect, 1 },
                { ScoreGreat, 0 },
                { ScoreGood, 0 },
                { ScoreBad, 0 },
                { ScoreMiss, 0 }
            };

            HitPunishments = new Dictionary<string, int>()
            {
                { ScoreMarvelous, 0 },
                { ScorePerfect, 0 },
                { ScoreGreat, 8 },
                { ScoreGood, 24 },
                { ScoreBad, 44 },
                { ScoreMiss, Int32.MaxValue } // Infinity
            };
        }

        public override void Update(GameTime gameTime)
        {
            CurrentSplash?.Update(gameTime); 
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (CurrentSplash != null)
            {
                var pos = new Vector2(
                    _gameplay.Skin.Settings.PlayfieldPositionX +
                    (_gameplay.Skin.PlayfieldLineTexture.Width * _gameplay.Beatmap.Settings.Difficulty.KeyAmount) / 2,
                    300);
                var color = Color.White * (CurrentSplash.MsBeforeExpire / 1000.0f);
                var origin = new Vector2(CurrentSplash.Texture.Width / 2.0f, CurrentSplash.Texture.Height / 2.0f);
                var size = new Vector2(0.9f * ((CurrentSplash.MsBeforeExpire / 1000.0f)), 0.9f * ((CurrentSplash.MsBeforeExpire / 1000.0f)));

                _spriteBatch.Draw(CurrentSplash.Texture, pos, null, color, 0.0f, origin, size, SpriteEffects.None, 0.0f);
            }

            _spriteBatch.End();
        }

        public void Calculate(HitObject hitObject)
        {
            int hitVal = GetHitValue(hitObject);
            DoScore(hitObject, hitVal);
        }

        public override void Reset()
        {
            _score = Combo = MaxCombo = 0;
            MarvelousCount = PerfectCount = GreatCount = GoodCount = BadCount = MissCount = 0;
            CurrentBonus = 100;

            Accuracy = 1.0f;

            CurrentSplash = null;
        }

        private void CalculateScore(string hitValueName)
        {
            var hitValue = HitValues[hitValueName];

            var totalNotes = _gameplay.Beatmap.HitObjects.Count;

            var baseScore = (MaxScore * 0.5 / totalNotes) * (hitValue / 320.0);
            var bonus = CurrentBonus + HitBonuses[hitValueName] - HitPunishments[hitValueName];

            CurrentBonus -= HitPunishments[hitValueName];
            bonus = MathHelper.Clamp(bonus, 1, 100);

            var bonusScore = (MaxScore * 0.5f / totalNotes) * (HitBonusValues[hitValueName] * Math.Sqrt(bonus) / 320.0);
            var totalScore = baseScore + bonusScore;

            _score += totalScore;
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
            int timeOffset = hitObject.Position - (int)_gameplay.Time;
            int absTimeOffset = Math.Abs(timeOffset);
            
            int score = 0;
            
            if (absTimeOffset <= HitThresholds[ScoreMarvelous])
                score = HitValues[ScoreMarvelous];
            else if (absTimeOffset <= HitThresholds[ScorePerfect]) 
                score = HitValues[ScorePerfect];
            else if (absTimeOffset <= HitThresholds[ScoreGreat]) 
                score = HitValues[ScoreGreat];
            else if (absTimeOffset <= HitThresholds[ScoreGood])
                score = HitValues[ScoreGood];
            else if (absTimeOffset <= HitThresholds[ScoreBad])
                score = HitValues[ScoreBad];
            else if (timeOffset <= HitThresholds[ScoreMiss])
                score = HitValues[ScoreMiss];

            return score;
        }

        private void DoScore(HitObject hitObject, int hitValue)
        {
            if (hitValue < 0) return;

            string hitValueName;

            hitObject.IsPressed = true;

            if (hitValue == HitValues[ScoreMarvelous])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreMarvelousTexture);
                MarvelousCount++;
                hitValueName = ScoreMarvelous;
            }
            else if (hitValue == HitValues[ScorePerfect])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScorePerfectTexture);
                PerfectCount++;
                hitValueName = ScorePerfect;
            }
            else if (hitValue == HitValues[ScoreGreat])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreGreatTexture);
                GreatCount++;
                hitValueName = ScoreGreat;
            }
            else if (hitValue == HitValues[ScoreGood])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreGoodTexture);
                GoodCount++;
                hitValueName = ScoreGood;
            }
            else if (hitValue == HitValues[ScoreBad])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreBadTexture);
                BadCount++;
                hitValueName = ScoreBad;
            }
            else if (hitValue == HitValues[ScoreMiss])
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreMissTexture);
                MissCount++;
                DoBreakCombo();
                hitValueName = ScoreMiss;
            }
            else throw new InvalidDataException("Score not found");
            
            GameSystemManager.FindSystem<ScoremeterSystem>()?.AddScore((int)_gameplay.Time, hitObject.Position);

            ProceedCombo(hitValue);
            CalculateScore(hitValueName);
            CalculateAccuracy();
        }

        private void DoBreakCombo()
        {
            if (Combo >= 30)
                _gameplay.Skin.ComboBreak.Play();
            Combo = 0;
        }
    }
}
