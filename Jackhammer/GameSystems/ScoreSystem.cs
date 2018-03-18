using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public class ScoreSystem : GameSystem
    {
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

        public float MarvelousThreshold { get; private set; }
        public float PerfectThreshold { get; private set; } 
        public float GreatThreshold { get; private set; }
        public float GoodThreshold { get; private set; }
        public float BadThreshold { get; private set; }
        public float MissThreshold { get; private set; }

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
                score = 320;
            else if (absTimeOffset <= PerfectThreshold) 
                score = 300;
            else if (absTimeOffset <= GreatThreshold) 
                score = 200;
            else if (absTimeOffset <= GoodThreshold)
                score = 100;
            else if (absTimeOffset <= BadThreshold)
                score = 50;
            else if (timeOffset <= MissThreshold)
                score = 0;

            return score;
        }

        private void DoScore(HitObject hitObject, int hitValue)
        {
            if (hitValue < 0) return;

            hitObject.IsPressed = true;

            switch (hitValue)
            {
                case 320:
                    CurrentSplash = new Splash(_gameplay.Skin.ScoreMarvelousTexture);
                    MarvelousCount++;
                    break;
                case 300:
                    CurrentSplash = new Splash(_gameplay.Skin.ScorePerfectTexture);
                    PerfectCount++;
                    break;
                case 200:
                    CurrentSplash = new Splash(_gameplay.Skin.ScoreGreatTexture);
                    GreatCount++;
                    break;
                case 100:
                    CurrentSplash = new Splash(_gameplay.Skin.ScoreGoodTexture);
                    GoodCount++;
                    break;
                case 50:
                    CurrentSplash = new Splash(_gameplay.Skin.ScoreBadTexture);
                    BadCount++;
                    break;
                case 0:
                    CurrentSplash = new Splash(_gameplay.Skin.ScoreMissTexture);
                    MissCount++;
                    DoBreakCombo();
                    break;
                default: throw new InvalidDataException("Score not found");
            }

            GameSystemManager.FindSystem<ScoremeterSystem>()?.AddScore(_gameplay.Time, hitObject.Position);

            ProceedCombo(hitValue);
            CalculateScore(hitValue);
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
