using System.Collections.Generic;
using System.Linq;
using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.Screens
{
    public class PlaySongSelectScreen : Screen
    {
        private Game _game;
        private BeatmapProcessor _beatmapProcessor;
        private SpriteBatch _spriteBatch;
        private InputHandler _input;
        private List<BeatmapProcessorContainerEntry> _bmEntries;

        public PlaySongSelectScreen(Game game)
        {
            LogHelper.Log("PlaySongSelectScreen: Constructor");
            _game = game;
            _input = new InputHandler(game);
            _beatmapProcessor = new BeatmapProcessor(game.Services.GetService<GameSettings>());
            _bmEntries = _beatmapProcessor.GetContainerEntries().ToList();
            LogHelper.Log("PlaySongSelectScreen: End Constructor");
        }

        public override void Initialize()
        {
            base.Initialize();

            LogHelper.Log("PlaySongSelectScreen: Initialize");
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            LogHelper.Log("PlaySongSelectScreen: End Initialize");
        }

        public override void LoadContent()
        {
            LogHelper.Log("PlaySongSelectScreen: Load Content");
            LogHelper.Log("PlaySongSelectScreen: End Load Content");
        }

        public override void Update(GameTime gameTime)
        {
            if (_input.WasKeyPressed(Keys.Escape))
                Show<GameplayScreen>(true);

            _input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            for (int i = 0; i < _bmEntries.Count; i++)
            {
                _spriteBatch.DrawString(_game.Services.GetService<Skin>().Font, $"Found beatmap:\n{_bmEntries[i]}", new Vector2(15 + i * 400, 15), Color.Black);
            }
            _spriteBatch.End();
        }
    }
}
