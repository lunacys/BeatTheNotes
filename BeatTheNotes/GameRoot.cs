using System;
using lunge.Library.GameAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace BeatTheNotes
{
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private AssetManager _assetManager;
        private Texture2D _test;
        private float _rotation;
        private Vector2 _position;
        private InputListenerComponent _listener;

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var listener = new GamePadListener(new GamePadListenerSettings(PlayerIndex.One));
            _listener = new InputListenerComponent(this, listener);
            listener.ButtonDown += (sender, args) =>
            {
                Console.WriteLine($"Button Down: {args.Button}");
            };
            listener.ThumbStickMoved += (sender, args) =>
            {
                //Console.WriteLine($"ThumbStickMoved: {args.ThumbStickState}");
                
                _position = args.ThumbStickState;
            };
            listener.TriggerMoved += (sender, args) =>
            {
                //Console.WriteLine($"TriggerMoved: {args.TriggerState}");
                _rotation = args.TriggerState;
            };

            Components.Add(_listener);
            _assetManager = new AssetManager(GraphicsDevice, Content.RootDirectory);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _test = _assetManager.Load<Texture2D>("Test.png");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(_test, Vector2.One * 128 + _position * 25,
                null, Color.White, _rotation,
                new Vector2(_test.Width / 2.0f, _test.Height / 2.0f), 
                Vector2.One, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}