using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.GameSystems;
using BeatTheNotes.Shared.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.Screens
{
    public class GameplayScreen : Screen
    {
        private readonly GameRoot _game;

        public GameSystemComponent GameSystemComponent { get; }

        private readonly string _beatmapName;

        private readonly InputHandler _input;

        private GameSettings _settings => _game.Services.GetService<GameSettings>();

        public GameplayScreen(GameRoot game, string beatmapName)
        {
            _game = game;
            _beatmapName = beatmapName;

            _input = new InputHandler(game);
            GameSystemComponent = new GameSystemComponent(game);
        }

        public override void Initialize()
        {
            LogHelper.Log("GameplayScreen: Initializing");

            base.Initialize();

            GameSystemComponent.Register(new GameplaySystem(_game, _beatmapName));
            GameSystemComponent.Register(new MusicSystem());
            GameSystemComponent.Register(new ScoreSystem(_game.GraphicsDevice));
            GameSystemComponent.Register(new ScoremeterSystem(_game.GraphicsDevice));
            GameSystemComponent.Register(new GameTimeSystem());
            GameSystemComponent.Register(new HealthSystem(0.0f));

            GameSystemComponent.Initialize();

            LogHelper.Log("GameplayScreen: Successfully Initialized");
        }

        public override void LoadContent()
        {
            LogHelper.Log("GameplayScreen: Loading Content");
            base.LoadContent();
            LogHelper.Log("GameplayScreen: Successfully Loaded Content");
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update(gameTime);

            if (_input.WasKeyPressed(Keys.Escape))
            {
                Show<PauseScreen>(true);
                MediaPlayer.Pause();
            }

            if (_input.WasKeyPressed(_settings.GameKeys["BeatmapRestart"]))
            {
                Restart();
            }

            if (_input.WasKeyPressed(_settings.GameKeys["BeatmapMusicBPMDown"]))
            {
                Restart();
                GameSystemComponent.FindSystem<MusicSystem>().PlaybackRate -= 0.1f;
            }

            if (_input.WasKeyPressed(_settings.GameKeys["BeatmapMusicBPMUp"]))
            {
                Restart();
                GameSystemComponent.FindSystem<MusicSystem>().PlaybackRate += 0.1f;
            }

            /*if (InputManager.IsKeyDown(Keys.Space))
                Restart(23000);*/

            // Updating..

            GameSystemComponent.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameSystemComponent.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Restart the game. It means reset the time and the song
        /// </summary>
        private void Restart()
        {
            GameSystemComponent.Reset();
        }
    }
}
