using System;
using System.Collections.Generic;
using System.Linq;
using BeatTheNotes.Framework;
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
            _beatmapProcessor = new BeatmapProcessor(game.Services.GetService<GameSettings>(), game.GraphicsDevice);
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
            //if (_input.WasKeyPressed(Keys.Escape))
            //    Show<GameplayScreen>(true);

            _input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            DrawBeatmapEntries();
            
            _spriteBatch.End();
        }

        private void DrawBeatmapEntries()
        {
            var font = _game.Services.GetService<Skin>().Font;
            for (int i = 0; i < _bmEntries.Count; i++)
            {
                var e = _bmEntries[i];
                _spriteBatch.Draw(e.BackgroundTexture, new Rectangle(15, 0 + i * 102, 178, 100), Color.White);

                var str =
                    $"{e.BeatmapSettings.Metadata.Title}\n" +
                    $" {e.BeatmapSettings.Metadata.Artist} // {e.BeatmapSettings.Metadata.Creator}\n" +
                    $" > {e.BeatmapSettings.Metadata.Version}";
                _spriteBatch.DrawString(font, str, new Vector2(15 + 184, 15 + i * 102), Color.Black);

                var rect = new Rectangle(15, 0 + i * 102, 500, 100);
                if (rect.Intersects(_input.MouseRect))
                {
                    rect.Width = 520;
                    if (_input.WasMouseButtonPressed(MouseButton.Left))
                    {
                        Console.WriteLine($"Starting beatmap {e.BeatmapSettings.Metadata.Version}");
                        var gameplayScreen = FindScreen<GameplayScreen>();
                        gameplayScreen.Beatmap = _beatmapProcessor.CreateBeatmapFromEntry(e);
                        gameplayScreen.Show();
                    }
                }
                else
                    rect.Width = 500;
                _spriteBatch.DrawRectangle(rect, Color.Black, 2.0f);
            }
        }
    }
}
