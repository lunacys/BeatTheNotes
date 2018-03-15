#region Using Statements
using System;
using Jackhammer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Jackhammer.Skin;
using MonoGame.Extended.Screens;

#endregion

namespace Jackhammer
{
    public class Jackhammer : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private FramesPerSecondCounter fps;
        Beatmap bm;
        private ScreenGameComponent _screenComponent;
        

        public Skin.Skin DefaultSkin { get; private set; }

        public Jackhammer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fps = new FramesPerSecondCounter();
            graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 240.0f);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;


#if WIN || LINUX
            graphics.IsFullScreen = false;
#elif ANDROID
            graphics.IsFullScreen = true;
#endif

            _screenComponent = new ScreenGameComponent(this);
            Components.Add(_screenComponent);
        }

        protected override void Initialize()
        {
            

            base.Initialize();

            _screenComponent.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            DefaultSkin = SkinLoader.Load(Content, GraphicsDevice, "Default");
            font = Content.Load<SpriteFont>(@"Fonts/MainFont");
            //bm = BeatmapReader.LoadFromFile("test");
            //BeatmapWriter.WriteToFile(bm, "test-saved");
            
            _screenComponent.Register(new GameplayScreen(this, "test", DefaultSkin));
            _screenComponent.FindScreen<GameplayScreen>().Show();
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
