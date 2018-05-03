#region Using Statements
using System;
using System.IO;
using BeatTheNotes.Framework;
using BeatTheNotes.Framework.Graphs;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins;
using BeatTheNotes.Screens;
using BeatTheNotes.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using Exception = System.Exception;

#endregion

namespace BeatTheNotes
{
    public class GameRoot : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FpsCounter _fpsCounter;
        private ScreenGameComponent _screenComponent;

        private Skin _usedSkin;
        private GameSettings _settings;

        private SkinAssetManager _skinAssetManager;

        private GraphCanvas _graphCanvas;

        private float _minFps = float.MaxValue, _maxFps = 0;

        public GameRoot()
        {
            LogHelper.Log($"======= Starting BeatTheNotes at {DateTime.Now} =======");

            if (File.Exists("settings.json"))
            {
                LogHelper.Log($"Found settings.json file. Loading settings from the file.");
                _settings = GameSettingsDeserializer.Deserialize();
            }
            else
            {
                LogHelper.Log($"settings.json file not found. Taking default settings.");
                _settings = new GameSettings();
            }


            Components.ComponentAdded += (sender, args) => { LogHelper.Log($"Game Root: Adding Component {args.GameComponent}"); };

            Services.AddService(_settings);

            MediaPlayer.Volume = _settings.SongVolume;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.SynchronizeWithVerticalRetrace = true;//_settings.IsUsedVSync;
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 2000.0f);

            _graphics.PreferredBackBufferWidth = _settings.WindowWidth;
            _graphics.PreferredBackBufferHeight = _settings.WindowHeight;

            IsMouseVisible = true;
#if WIN || LINUX
            _graphics.IsFullScreen = _settings.IsFullscreen;
#elif ANDROID
            _graphics.IsFullScreen = true;
#endif
        }

        protected override void Initialize()
        {
            LogHelper.Log("Game Root: Initialize..");

            _fpsCounter = new FpsCounter(this);

            Components.Add(_fpsCounter);

            _graphCanvas = new GraphCanvas(this)
            {
                Position = new Vector2(660, 256),
                Size = new Size2(600, 100)
            };
            Components.Add(_graphCanvas);

            _fpsCounter.OnFpsUpdate += (sender, args) =>
            {
                FpsCounter fpsCounter = (FpsCounter)sender;
                _graphCanvas.PushValue(fpsCounter.FramesPerSecond);
            };

            base.Initialize();

            LogHelper.Log("Game Root: End Initialize..");
        }

        protected override void LoadContent()
        {
            LogHelper.Log("Game Root: Load Content..");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                _usedSkin = SkinLoader.Load(Content, GraphicsDevice, _settings.Skin);
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameRoot: Error while opening Skin, using Default skin instead: {e}");
                _usedSkin = SkinLoader.Load(Content, GraphicsDevice, "Default");
                _settings.Skin = "Default";
            }

            _skinAssetManager = new SkinAssetManager(GraphicsDevice, _settings);
            _skinAssetManager.Load<Texture2D>("Button.png");

            Services.AddService(_usedSkin);
            Services.AddService(_skinAssetManager);



            _screenComponent = new ScreenGameComponent(this);
            Components.Add(_screenComponent);
            PlaySongSelectScreen playSongSelectScreen = new PlaySongSelectScreen(this);
            _screenComponent.Register(playSongSelectScreen);
            GameplayScreen gameplayScreen = new GameplayScreen(this);
            _screenComponent.Register(gameplayScreen);
            PauseScreen ps = new PauseScreen(this);
            _screenComponent.Register(ps);

            _graphCanvas.Font = _usedSkin.Font;

            base.LoadContent();

            LogHelper.Log("Game Root: End Load Content");
        }

        protected override void UnloadContent()
        {
            LogHelper.Log("Game Root: Unloading Content");

            GameSettingsSerializer.Serialize(_settings);

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            //if (InputManager.IsKeyDown(Keys.Escape))
            //    Exit();
#endif

            /*if (InputManager.WasKeyPressed(Keys.Left))
            {
                _settings.SongVolumeF -= 0.1f;
                _settings.SongVolumeF = MathHelper.Clamp(_settings.SongVolumeF, 0.0f, 1.0f);
                MediaPlayer.Volume = _settings.SongVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Right))
            {
                _settings.SongVolumeF += 0.1f;
                _settings.SongVolumeF = MathHelper.Clamp(_settings.SongVolumeF, 0.0f, 1.0f);
                MediaPlayer.Volume = _settings.SongVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Down))
            {
                _settings.HitsoundVolumeF -= 0.1f;
                _settings.HitsoundVolumeF = MathHelper.Clamp(_settings.HitsoundVolumeF, 0.0f, 1.0f);

                SoundEffect.MasterVolume = _settings.HitsoundVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Up))
            {
                _settings.HitsoundVolumeF += 0.1f;
                _settings.HitsoundVolumeF = MathHelper.Clamp(_settings.HitsoundVolumeF, 0.0f, 1.0f);

                SoundEffect.MasterVolume = _settings.HitsoundVolumeF;
            }*/

            base.Update(gameTime);

            if (_fpsCounter.FramesPerSecond < _minFps)
                _minFps = _fpsCounter.FramesPerSecond;
            if (_fpsCounter.FramesPerSecond > _maxFps)
                _maxFps = _fpsCounter.FramesPerSecond;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            _graphCanvas.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_usedSkin.Font, $"FPS: {_fpsCounter.FramesPerSecond:F1}\nMin: {_minFps}\nMax: {_maxFps:F1}",
                new Vector2(_settings.WindowWidth - 80, _settings.WindowHeight - 18 * 3), Color.Red);
            _spriteBatch.End();
        }
    }
}
