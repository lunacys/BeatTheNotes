using Jackhammer.Input;
using Jackhammer.Skins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;

namespace Jackhammer.Screens
{
    public class PauseScreen : Screen
    {
        private readonly Jackhammer _game;
        private SpriteBatch _spriteBatch;
        private Skin Skin => _game.UsedSkin;

        public PauseScreen(Jackhammer game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(_game);

            if (InputManager.WasKeyPressed(Keys.Escape))
            {
                Show<GameplayScreen>(true);
                MediaPlayer.Resume();
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Skin.DefaultBackground, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
