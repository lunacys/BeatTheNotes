using System;
using System.Collections.Generic;
using System.IO;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Objects;
using BeatTheNotes.Shared.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.GameSystems
{
    /// <summary>
    /// ScoremeterScore System where maximum score possible is one million
    /// </summary>
    public class ScoreV1System : GameSystem, IGameSystemProcessHitObject
    {
        public string ScoreMarvelous => "Marvelous";
        public string ScorePerfect => "Perfect";
        public string ScoreGreat => "Great";
        public string ScoreGood => "Good";
        public string ScoreBad => "Bad";
        public string ScoreMiss => "Miss";

        // Max score is one million
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

        public ScoreSplash CurrentScoreSplash { get; private set; }

        public event EventHandler<OnScoreGetEventHandler> OnScoreGet;

        private readonly SpriteBatch _spriteBatch;

        private GameplaySystem _gameplay;

        public ScoreV1System(GraphicsDevice graphicsDevice)
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
            CurrentScoreSplash?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // If there is a splash set, draw it
            if (CurrentScoreSplash != null)
            {
                var pos = new Vector2(
                    // x
                    _gameplay.Skin.Settings.PlayfieldPositionX +
                    (_gameplay.Skin.PlayfieldLineTexture.Width * _gameplay.Beatmap.Settings.Difficulty.KeyAmount) / 2,
                    // y
                    300);

                var color = Color.White * (CurrentScoreSplash.MsBeforeExpire / 1000.0f);
                var origin = new Vector2(CurrentScoreSplash.Texture.Width / 2.0f, CurrentScoreSplash.Texture.Height / 2.0f);
                var size = new Vector2(0.9f * ((CurrentScoreSplash.MsBeforeExpire / 1000.0f)), 0.9f * ((CurrentScoreSplash.MsBeforeExpire / 1000.0f)));

                _spriteBatch.Draw(CurrentScoreSplash.Texture, pos, null, color, 0.0f, origin, size, SpriteEffects.None, 0.0f);
            }

            _spriteBatch.End();
        }

        private void Calculate(HitObject hitObject)
        {
            int hitVal = GetHitValue(hitObject);
            if (hitVal < 0) return;

            DoScore(hitObject, hitVal);
        }

        public override void Reset()
        {
            _score = Combo = MaxCombo = 0;
            MarvelousCount = PerfectCount = GreatCount = GoodCount = BadCount = MissCount = 0;
            CurrentBonus = 100;

            Accuracy = 1.0f;

            CurrentScoreSplash = null;
        }

        private void CalculateScore(string hitValueName)
        {
            var hitValue = HitValues[hitValueName];

            // Total amount of notes
            var totalNotes = _gameplay.Beatmap.HitObjects.Count;

            // Base score is the half of the total score
            var baseScore = (MaxScore * 0.5 / totalNotes) * (hitValue / 320.0);
            var bonus = CurrentBonus + HitBonuses[hitValueName] - HitPunishments[hitValueName];

            // Proceed punishment
            CurrentBonus -= HitPunishments[hitValueName];
            bonus = MathHelper.Clamp(bonus, 1, 100);

            // Bonus score is the other half of the total score
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
            var timeOffset = ((hitObject as NoteHold)?.EndPosition ?? hitObject.Position) -
                             FindSystem<GameTimeSystem>().Time;
            var absTimeOffset = Math.Abs(timeOffset);

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
            else
                return -1;

            return score;
        }

        private void DoScore(HitObject hitObject, int hitValue)
        {
            if (hitValue < 0) return;

            string hitValueName;

            if (hitValue == HitValues[ScoreMarvelous])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScoreMarvelousTexture);
                MarvelousCount++;
                hitValueName = ScoreMarvelous;
            }
            else if (hitValue == HitValues[ScorePerfect])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScorePerfectTexture);
                PerfectCount++;
                hitValueName = ScorePerfect;
            }
            else if (hitValue == HitValues[ScoreGreat])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScoreGreatTexture);
                GreatCount++;
                hitValueName = ScoreGreat;
            }
            else if (hitValue == HitValues[ScoreGood])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScoreGoodTexture);
                GoodCount++;
                hitValueName = ScoreGood;
            }
            else if (hitValue == HitValues[ScoreBad])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScoreBadTexture);
                BadCount++;
                hitValueName = ScoreBad;
            }
            else if (hitValue == HitValues[ScoreMiss])
            {
                CurrentScoreSplash = new ScoreSplash(_gameplay.Skin.ScoreMissTexture);
                MissCount++;
                DoBreakCombo();
                hitValueName = ScoreMiss;
            }
            else throw new InvalidDataException("ScoremeterScore not found");

            OnScoreGet?.Invoke(this, new OnScoreGetEventHandler(hitValueName, HitValues[hitValueName], (float)FindSystem<GameTimeSystem>().Time - hitObject.Position));
            GameSystemManager.FindSystem<ScoremeterSystem>()?.AddScore((long)FindSystem<GameTimeSystem>().Time,
                hitObject.Position, hitValueName);

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

        public void OnHitObjectHit(object sender, HitObjectOnHitEventArgs args)
        {
            Calculate(args.HitObject);
        }
    }
}
