using System.Collections.Generic;
using System.Linq;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeatTheNotes.GameSystems
{
    public class ScoremeterSystem : GameSystem
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; private set; }
        private float _od;

        private SpriteBatch _spriteBatch;
        private Dictionary<string, Color> _hitColors;

        public float SizeMultiplier
        {
            get;
            set;
        }

        private List<ScoremeterScore> _scores;

        public ScoremeterSystem(GraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();

            GameplaySystem gs = GameSystemManager.FindSystem<GameplaySystem>();

            
            _scores = new List<ScoremeterScore>();
            _od = gs.Beatmap.Settings.Difficulty.OverallDifficutly;

            SizeMultiplier = 1.0f;

            var scoreSys = FindSystem<ScoreV1System>();

            Size = new Vector2((float)scoreSys.HitThresholds[scoreSys.ScoreMiss] * 2.0f, 8);
            Position = new Vector2(
                gs.Skin.Settings.PlayfieldPositionX +
                gs.Skin.PlayfieldLineTexture.Width * gs.Beatmap.Settings.Difficulty.KeyAmount / 2.0f -
                Size.X * SizeMultiplier / 2.0f, 400);

            _hitColors = new Dictionary<string, Color>
            {
                { scoreSys.ScoreMarvelous, new Color(255, 255, 255) },
                { scoreSys.ScorePerfect, new Color(233, 201, 27) },
                { scoreSys.ScoreGreat, new Color(0, 231, 33) },
                { scoreSys.ScoreGood, new Color(0, 185, 231) },
                { scoreSys.ScoreBad, new Color(229, 0, 151) },
                { scoreSys.ScoreMiss, new Color(199, 0, 0) }
            };


        }

        public override void Update(GameTime gameTime)
        {
            foreach (var score in _scores)
            {
                score.Update(gameTime);
            }

            _scores = _scores.Where(score => !score.IsExpired).ToList();
        }

        public void AddScore(long currentTime, long hitObjectPosition, string hit)
        {
            _scores.Add(new ScoremeterScore(currentTime - hitObjectPosition, 3000, hit));
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            DrawBaseRect();

            foreach (var t in _scores)
            {
                Vector2 pos = new Vector2((Position.X + Size.X / 2 + t.Position) * SizeMultiplier - 1, Position.Y - 15);
                
                Color color = _hitColors[t.HitName];

                _spriteBatch.FillRectangle(pos, new Vector2(2, 40), color * 0.5f * (t.MsBeforeExpire / 1000.0f));
            }

            _spriteBatch.End();
        }

        private void DrawBaseRect()
        {
            var scoreSys = FindSystem<ScoreV1System>();

            // miss
            _spriteBatch.FillRectangle(Position, Size, _hitColors[scoreSys.ScoreMiss]);
            // bad
            var offset = new Vector2((Size.X / 2 - (151 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), _hitColors[scoreSys.ScoreBad]);
            // good
            offset = new Vector2((Size.X / 2 - (127 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), _hitColors[scoreSys.ScoreGood]);
            // great
            offset = new Vector2(Size.X / 2 - ((97 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), _hitColors[scoreSys.ScoreGreat]);
            // perfect
            offset = new Vector2(Size.X / 2 - ((64 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), _hitColors[scoreSys.ScorePerfect]);
            // marvelous
            offset = new Vector2(Size.X / 2 - 16, 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), _hitColors[scoreSys.ScoreMarvelous]);

            _spriteBatch.FillRectangle(Position + new Vector2(Size.X / 2.0f - 1, -18), new Vector2(2, 48), new Color(200, 200, 200));
        }

        public override void Reset()
        {
            _scores.Clear();
        }
    }
}
