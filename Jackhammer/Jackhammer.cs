#region Using Statements
using System;
using System.IO;
using Jackhammer.GameSystems;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Jackhammer.Skins;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;
using Jackhammer.Input;
using Exception = System.Exception;

#endregion

namespace Jackhammer
{
    public class Jackhammer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FramesPerSecondCounter fps;
        private ScreenGameComponent _screenComponent;

        private Skin _usedSkin;
        private GameSettings _settings;

        InputListenerComponent _ilc;
        

        private int _minFps = Int32.MaxValue, _maxFps = 0;

        public Jackhammer()
        {
            LogHelper.Log($"======= Starting Jackhammer at {DateTime.Now} =======");

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
            fps = new FramesPerSecondCounter();

            _graphics.SynchronizeWithVerticalRetrace = _settings.IsUsedVSync;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 2000.0f);

            _graphics.PreferredBackBufferWidth = _settings.WindowWidth;
            _graphics.PreferredBackBufferHeight = _settings.WindowHeight;

            KeyboardListener kl = new KeyboardListener();
            kl.KeyPressed += (sender, args) =>
            {
                //Console.WriteLine($"Pressed Key '{args.Key}'");
                
            };
            _ilc = new InputListenerComponent(this, kl);

            

            Components.Add(_ilc);

#if WIN || LINUX
            _graphics.IsFullScreen = _settings.IsFullscreen;
#elif ANDROID
            _graphics.IsFullScreen = true;
#endif
        }

        protected override void Initialize()
        {
            LogHelper.Log("Game Root: Initialize..");

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

            Services.AddService(_usedSkin);

            //bm = BeatmapReader.LoadTextureFromFile("test");
            //BeatmapWriter.WriteToFile(bm, "test-saved");
            _screenComponent = new ScreenGameComponent(this);
            //LogHelper.Log($"Game Root: Add Component ScreenComponent");
            Components.Add(_screenComponent);
            GameplayScreen gameplayScreen = new GameplayScreen(this, "test");
            _screenComponent.Register(gameplayScreen);
            PauseScreen ps = new PauseScreen(this);
            _screenComponent.Register(ps);
            
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

            if (InputManager.WasKeyPressed(Keys.Left))
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
            }
            
            fps.Update(gameTime);
            if (fps.FramesPerSecond < _minFps)
                _minFps = fps.FramesPerSecond;
            if (fps.FramesPerSecond > _maxFps)
                _maxFps = fps.FramesPerSecond;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            fps.Draw(gameTime);
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_usedSkin.Font, $"FPS: {fps.FramesPerSecond}\nMin: {_minFps}\nMax: {_maxFps}",
                new Vector2(_settings.WindowWidth - 80, _settings.WindowHeight - 18 * 3), Color.Red);
            _spriteBatch.End();

            
        }
    }
}
