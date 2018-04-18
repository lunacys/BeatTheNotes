using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.Screens
{
    public class PlaySongSelectScreen : Screen
    {
        private Game _game;
        private BeatmapProcessor _beatmapProcessor;
        private SpriteBatch _spriteBatch;

        public PlaySongSelectScreen(Game game)
        {
            _game = game;
            _beatmapProcessor = new BeatmapProcessor(game.Services.GetService<GameSettings>());
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.End();
        }
    }
}
