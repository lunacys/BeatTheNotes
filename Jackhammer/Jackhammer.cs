#region Using Statements
using System;
using System.IO;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Jackhammer.Skin;
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

        public Skin.Skin DefaultSkin { get; private set; }
        public Skin.Skin UsedSkin { get; private set; }
        public GameSettings Settings { get; }

        public Jackhammer()
        {
            LogHelper.Log($"======= Starting Jackhammer at {DateTime.Now} =======");

            if (File.Exists("settings.json"))
                Settings = GameSettingsDeserializer.Deserialize();
            else
                Settings = new GameSettings();

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fps = new FramesPerSecondCounter();

            _graphics.SynchronizeWithVerticalRetrace = Settings.IsUsedVSync;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Settings.TargetFramesPerSecond);

            _graphics.PreferredBackBufferWidth = Settings.WindowWidth;
            _graphics.PreferredBackBufferHeight = Settings.WindowHeight;

#if WIN || LINUX
            _graphics.IsFullScreen = Settings.IsFullscreen;
#elif ANDROID
            graphics.IsFullScreen = true;
#endif
        }

        protected override void Initialize()
        {
            LogHelper.Log("Game Root: Initialize..");

            _screenComponent = new ScreenGameComponent(this);
            LogHelper.Log($"Game Root: Add Component ScreenComponent");
            Components.Add(_screenComponent);

            base.Initialize();
            
            LogHelper.Log("Game Root: End Initialize..");
        }

        protected override void LoadContent()
        {
            LogHelper.Log("Game Root: Load Content..");

            _spriteBatch = new SpriteBatch(GraphicsDevice);


            DefaultSkin = SkinLoader.Load(Content, GraphicsDevice, "Default");
            try
            {
                UsedSkin = SkinLoader.Load(Content, GraphicsDevice, Settings.Skin);
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameRoot: Error while opening Skin, using Default skin instead: {e}");
                UsedSkin = DefaultSkin;
                Settings.Skin = "Default";
            }
            
            //bm = BeatmapReader.LoadFromFile("test");
            //BeatmapWriter.WriteToFile(bm, "test-saved");
            _screenComponent.Register(new GameplayScreen(this, "test"));
            _screenComponent.FindScreen<GameplayScreen>().Show();

            LogHelper.Log("Game Root: End Load Content");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            GameSettingsSerializer.Serialize(Settings);

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(this);

            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
#endif



            fps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            fps.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
