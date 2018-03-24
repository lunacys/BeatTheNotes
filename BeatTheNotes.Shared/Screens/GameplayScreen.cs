using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.GameSystems;
using BeatTheNotes.Input;
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

        public GameplayScreen(GameRoot game, string beatmapName)
        {
            _game = game;
            _beatmapName = beatmapName;

            GameSystemComponent = new GameSystemComponent(game);
        }

        public override void Initialize()
        {
            LogHelper.Log("GameplayScreen: Initializing");

            base.Initialize();

            
            GameSystemComponent.Register(new GameplaySystem(_game, _beatmapName));
            GameSystemComponent.Register(new MusicSystem(_beatmapName,
                GameSystemComponent.FindSystem<GameplaySystem>().Beatmap.Settings.General.AudioFileName));
            GameSystemComponent.Register(new ScoreSystem(_game.GraphicsDevice));
            GameSystemComponent.Register(new ScoremeterSystem(_game.GraphicsDevice));

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
            InputManager.Update(_game);

            if (InputManager.WasKeyPressed(Keys.Escape))
            {
                Show<PauseScreen>(true);
                MediaPlayer.Pause();
            }
            
            if (InputManager.WasKeyPressed(Keys.OemTilde))
            {
                Restart();
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
