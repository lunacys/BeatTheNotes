using System;
using System.Collections.Generic;
using System.Linq;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public class ScoremeterSystem : GameSystem
    {
        private GameplayScreen _game;
        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
        private float Od => _game.Beatmap.Settings.Difficulty.OverallDifficutly;

        public float SizeMultiplier
        {
            get;
            set;
        }

        private List<Score> _scores;

        public ScoremeterSystem(GameplayScreen game)
        {
            _game = game;
            _scores = new List<Score>();

            SizeMultiplier = 1.0f;
            
            Size = new Vector2((188 - (3 * Od)) * 2, 8);
            Position = new Vector2(
                game.Skin.Settings.PlayfieldPositionX + game.Skin.PlayfieldLineTexture.Width / 2.0f -
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

        public void AddScore(HitObject hitObject)
        {
            _scores.Add(new Score(_game.Time - hitObject.Position, 3000));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBaseRect(spriteBatch);

            foreach (var t in _scores)
            {
                // TODO: Add colors for every score
                Vector2 pos = new Vector2((Position.X + Size.X / 2 + t.Position) * SizeMultiplier, Position.Y - 15);
                Vector2 off = new Vector2(Position.X + Size.X / 2, Position.Y);
                spriteBatch.FillRectangle(pos, new Vector2(3, 40),
                    ((pos.X > off.X - ((151 - (3 * Od))) && pos.X < off.X + ((151 - (3 * Od)))
                        ? Color.Orange * (t.MsBeforeExpire / 1000.0f)
                        : Color.Red * (t.MsBeforeExpire / 1000.0f))));
            }
        }

        private void DrawBaseRect(SpriteBatch spriteBatch)
        {
            Vector2 offset;

            // miss
            spriteBatch.FillRectangle(Position, Size, new Color(199, 0, 0));
            // bad
            offset = new Vector2((Size.X / 2 - (151 - (3 * Od))), 0);
            spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(229, 0, 151));
            // good
            offset = new Vector2((Size.X / 2 - (127 - (3 * Od))), 0);
            spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(0, 185, 231));
            // great
            offset = new Vector2(Size.X / 2 - ((97 - (3 * Od))), 0);
            spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(0, 231, 33));
            // perfect
            offset = new Vector2(Size.X / 2 - ((64 - (3 * Od))), 0);
            spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(233, 201, 27));
            // marvelous
            offset = new Vector2(Size.X / 2 - 16, 0);
            spriteBatch.FillRectangle(Position + offset, new Vector2(Size.X - offset.X * 2, Size.Y), new Color(255, 255, 255));

            spriteBatch.FillRectangle(Position + new Vector2(Size.X / 2.0f - 1, -6), new Vector2(2, 24), Color.Black);
        }

        public override void Reset()
        {
            _scores.Clear();
        }
    }
}
