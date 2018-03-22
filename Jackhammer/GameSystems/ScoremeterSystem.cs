using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public class ScoremeterSystem : GameSystem
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; private set; }
        private float _od;

        private SpriteBatch _spriteBatch;

        public float SizeMultiplier
        {
            get;
            set;
        }

        private List<Score> _scores;

        public ScoremeterSystem(GraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();

            GameplaySystem gs = GameSystemManager.FindSystem<GameplaySystem>();

            
            _scores = new List<Score>();
            _od = gs.Beatmap.Settings.Difficulty.OverallDifficutly;

            SizeMultiplier = 1.0f;

            Size = new Vector2((188 - (3 * _od)) * 2, 8);
            Position = new Vector2(
                gs.Skin.Settings.PlayfieldPositionX +
                gs.Skin.PlayfieldLineTexture.Width * gs.Beatmap.Settings.Difficulty.KeyAmount / 2.0f -
                Size.X * SizeMultiplier / 2.0f, 400);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var score in _scores)
            {
                score.Update(gameTime);
            }

            _scores = _scores.Where(score => !score.IsExpired).ToList();
        }

        public void AddScore(int currentTime, int hitObjectPosition)
        {
            _scores.Add(new Score(currentTime - hitObjectPosition, 3000));
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            DrawBaseRect();

            foreach (var t in _scores)
            {
                // TODO: Add colors for every score
                Vector2 pos = new Vector2((Position.X + Size.X / 2 + t.Position) * SizeMultiplier - 1, Position.Y - 15);
                Vector2 off = new Vector2(Position.X + Size.X / 2, Position.Y);
                _spriteBatch.FillRectangle(pos, new Vector2(2, 40),
                    ((pos.X > off.X - ((151 - (3 * _od))) && pos.X < off.X + ((151 - (3 * _od)))
                        ? Color.Orange * (t.MsBeforeExpire / 1000.0f)
                        : Color.Red * (t.MsBeforeExpire / 1000.0f))));
            }

            _spriteBatch.End();
        }

        private void DrawBaseRect()
        {
            // miss
            _spriteBatch.FillRectangle(Position, Size, new Color(199, 0, 0));
            // bad
            var offset = new Vector2((Size.X / 2 - (151 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(229, 0, 151));
            // good
            offset = new Vector2((Size.X / 2 - (127 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(0, 185, 231));
            // great
            offset = new Vector2(Size.X / 2 - ((97 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(0, 231, 33));
            // perfect
            offset = new Vector2(Size.X / 2 - ((64 - (3 * _od))), 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(233, 201, 27));
            // marvelous
            offset = new Vector2(Size.X / 2 - 16, 0);
            _spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(255, 255, 255));

            _spriteBatch.FillRectangle(Position + new Vector2(Size.X / 2.0f - 1, -18), new Vector2(2, 48), new Color(200, 200, 200));
        }

        public override void Reset()
        {
            _scores.Clear();
        }
    }
}
