using System;
using System.IO;
using BeatTheNotes.Framework.Debugging.Logging;
using BeatTheNotes.Framework.GameAssets;
using BeatTheNotes.Framework.GameTimers;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Serialization;
using BeatTheNotes.Framework.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BeatTheNotes
{
    public class GameRoot : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private AssetManager _assets;

        [GameSettingsEntry("MusicVolume", 1.0)]
        public double MusicVolume { get; set; }

        [GameSettingsEntry("SfxVolume", 1.0)]
        public double SfxVolume { get; set; }

        private GameSettingsGameComponent _gameSettings;

        private InputHandler _input;

        private int _x = 0;

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            GameTimer gt = new GameTimer(1.5, true);
            gt.OnTimeElapsed += (sender, args) =>
            {
                LogHelper.Target = LogTarget.Console;
                LogHelper.Log($"SfxVolume: {SfxVolume}, MusicVolume: {MusicVolume}");
            };
            GameTimerManager.Add(gt);

            _gameSettings = new GameSettingsGameComponent(this);
            Components.Add(_gameSettings);

            _input = new InputHandler(this);
            Components.Add(_input);

            _input[Keys.G] = () =>
            {
                _x++;
                Console.WriteLine($"Hello! x: {_x}");
            };
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _assets = new AssetManager(GraphicsDevice, "Assets");

            // TODO: use this.Content to load your game content here
            var txt = _assets.Load<Texture2D>("Test.png");

            SfxVolume = _gameSettings.Get<double>("SfxVolume");
            MusicVolume = _gameSettings.Get<double>("MusicVolume");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            string str = JsonConvert.SerializeObject(_gameSettings.GameSettings, Formatting.Indented, new GameSettingsConverter());

            using (StreamWriter sw = new StreamWriter("Settings.json"))
            {
                sw.WriteLine(str);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (_input.WasKeyPressed(Keys.Escape))
                Exit();

            LogHelper.Update();

            // TODO: Add your update logic here
            GameTimerManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
