#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

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

        public Jackhammer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            fps = new FramesPerSecondCounter();
            graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 240.0f);


#if WIN || LINUX
            graphics.IsFullScreen = false;
#elif ANDROID
            graphics.IsFullScreen = true;
#endif
        }

        protected override void Initialize()
        {


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>(@"Fonts/MainFont");
            bm = BeatmapReader.LoadFromFile("test");
            BeatmapWriter.WriteToFile(bm, "test-saved");
        }

        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif
            fps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < bm.HitObjects.Count; i++)
            {
                spriteBatch.DrawString(font, $"{bm.HitObjects[i]}", new Vector2(2, 2 + i * 14), Color.Black);
            }

            //spriteBatch.DrawString(font, $"Artist: {bm.SettingsMetadata.Artist}", new Vector2(200, 14), Color.Black);

            spriteBatch.End();


            fps.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
