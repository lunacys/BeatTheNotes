using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jackhammer.GameSystems;
using Jackhammer.Skins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;
using Jackhammer.Input;

namespace Jackhammer.Screens
{
    public class GameplayScreen : Screen
    {
        private readonly Jackhammer _game;

        public GameSystemComponent GameSystemComponent { get; }

        private readonly string _beatmapName;

        public GameplayScreen(Jackhammer game, string beatmapName)
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
            GameSystemComponent.Register(new ScoreSystem(_game.GraphicsDevice));
            GameSystemComponent.Register(new ScoremeterSystem(_game.GraphicsDevice));

            GameSystemComponent.Initialize();

            LogHelper.Log("GameplayScreen: Sucessfully Initialized");
        }

        public override void LoadContent()
        {
            LogHelper.Log("GameplayScreen: Loading Content");
            base.LoadContent();
            LogHelper.Log("GameplayScreen: Sucessfully Loaded Content");
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
