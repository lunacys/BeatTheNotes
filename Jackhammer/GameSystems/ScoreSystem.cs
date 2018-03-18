using System;
using System.Collections.Generic;
using System.Text;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public float Od => _gameplay.Beatmap.Settings.Difficulty.OverallDifficutly;

        public float MarvelousThreshold { get; }
        public float PerfectThreshold { get; } 
        public float GreatThreshold { get; }
        public float GoodThreshold { get; }
        public float BadThreshold { get; }
        public float MissThreshold { get; }

        private readonly SpriteBatch _spriteBatch;

        public Splash CurrentSplash { get; private set; }

        public ScoreSystem(GameplayScreen gameplay, GraphicsDevice graphicsDevice)
        {
            _gameplay = gameplay;
            Accuracy = 1.0f;

            _spriteBatch = new SpriteBatch(graphicsDevice);

            MarvelousThreshold = 16.0f;
            PerfectThreshold = 64 - (3 * Od);
            GreatThreshold = 97 - (3 * Od);
            GoodThreshold = 127 - (3 * Od);
            BadThreshold = 151 - (3 * Od);
            MissThreshold = 188 - (3 * Od);
        }

        public override void Update(GameTime gameTime)
        {
            CurrentSplash?.Update(gameTime);

            // TODO: Rework this
            foreach (var line in _gameplay.SeparatedLines)
            {
                foreach (var o in line)
                {
                    if (o.IsPressed) continue;

                    if (o.Position + MissThreshold < _gameplay.Time)
                    {
                        o.IsPressed = true;
                        MissCount++;
                        Combo = 0;
                        CalculateAccuracy();
                        
                        CurrentSplash = new Splash(_gameplay.Skin.ScoreMissTexture);

                        GameSystemManager.FindSystem<ScoremeterSystem>().AddScore(_gameplay.Time, o.Position);
                    }
                }
            }
            
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

        public override void Reset()
        {
            Score = Combo =
                MaxCombo = MarvelousCount = PerfectCount = GreatCount = GoodCount = BadCount = MissCount = 0;

            Accuracy = 1.0f;

            CurrentSplash = null;
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
            
            int score = 0;
            
            if (absTimeOffset <= MarvelousThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreMarvelousTexture);
                MarvelousCount++;
                score = 320;
            }
            else if (absTimeOffset <= PerfectThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScorePerfectTexture);
                PerfectCount++;
                score = 300;
            }
            else if (absTimeOffset <= GreatThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreGreatTexture);
                GreatCount++;
                score = 200;
            }
            else if (absTimeOffset <= GoodThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreGoodTexture);
                GoodCount++;
                score = 100;
            }
            else if (absTimeOffset <= BadThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreBadTexture);
                BadCount++;
                score = 50;
            }
            else if (timeOffset <= MissThreshold)
            {
                CurrentSplash = new Splash(_gameplay.Skin.ScoreMissTexture);
                MissCount++;
                Combo = 0;
                score = 0;
            }
            
            ProceedCombo(score);

            return score;
        }
    }
}
