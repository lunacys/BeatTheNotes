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
        
        public Skin UsedSkin { get; private set; }
        public GameSettings Settings { get; }

        InputListenerComponent _ilc;

        private int _minFps = Int32.MaxValue, _maxFps = 0;

        public Jackhammer()
        {
            LogHelper.Log($"======= Starting Jackhammer at {DateTime.Now} =======");

            if (File.Exists("settings.json"))
            {
                LogHelper.Log($"Found settings.json file. Loading settings from the file.");
                Settings = GameSettingsDeserializer.Deserialize();
            }
            else
            {
                LogHelper.Log($"settings.json file not found. Taking default settings.");
                Settings = new GameSettings();
            }

            MediaPlayer.Volume = Settings.SongVolume;
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fps = new FramesPerSecondCounter();

            _graphics.SynchronizeWithVerticalRetrace = Settings.IsUsedVSync;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Settings.TargetFramesPerSecond);

            _graphics.PreferredBackBufferWidth = Settings.WindowWidth;
            _graphics.PreferredBackBufferHeight = Settings.WindowHeight;

            KeyboardListener kl = new KeyboardListener();
            kl.KeyPressed += (sender, args) =>
            {
                //Console.WriteLine($"Pressed Key '{args.Key}'");
                
            };
            _ilc = new InputListenerComponent(this, kl);
            
            Components.Add(_ilc);

#if WIN || LINUX
            _graphics.IsFullScreen = Settings.IsFullscreen;
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
                UsedSkin = SkinLoader.Load(Content, GraphicsDevice, Settings.Skin);
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameRoot: Error while opening Skin, using Default skin instead: {e}");
                UsedSkin = SkinLoader.Load(Content, GraphicsDevice, "Default");
                Settings.Skin = "Default";
            }

            //bm = BeatmapReader.LoadTextureFromFile("test");
            //BeatmapWriter.WriteToFile(bm, "test-saved");
            _screenComponent = new ScreenGameComponent(this);
            LogHelper.Log($"Game Root: Add Component ScreenComponent");
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

            GameSettingsSerializer.Serialize(Settings);

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
                Settings.SongVolumeF -= 0.1f;
                Settings.SongVolumeF = MathHelper.Clamp(Settings.SongVolumeF, 0.0f, 1.0f);
                MediaPlayer.Volume = Settings.SongVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Right))
            {
                Settings.SongVolumeF += 0.1f;
                Settings.SongVolumeF = MathHelper.Clamp(Settings.SongVolumeF, 0.0f, 1.0f);
                MediaPlayer.Volume = Settings.SongVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Down))
            {
                Settings.HitsoundVolumeF -= 0.1f;
                Settings.HitsoundVolumeF = MathHelper.Clamp(Settings.HitsoundVolumeF, 0.0f, 1.0f);

                SoundEffect.MasterVolume = Settings.HitsoundVolumeF;
            }

            if (InputManager.WasKeyPressed(Keys.Up))
            {
                Settings.HitsoundVolumeF += 0.1f;
                Settings.HitsoundVolumeF = MathHelper.Clamp(Settings.HitsoundVolumeF, 0.0f, 1.0f);

                SoundEffect.MasterVolume = Settings.HitsoundVolumeF;
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
            _spriteBatch.DrawString(UsedSkin.Font, $"FPS: {fps.FramesPerSecond}\nMin: {_minFps}\nMax: {_maxFps}",
                new Vector2(Settings.WindowWidth - 80, Settings.WindowHeight - 18 * 3), Color.Red);
            _spriteBatch.End();

            
        }
    }
}
