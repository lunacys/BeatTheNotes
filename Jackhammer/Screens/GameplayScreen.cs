using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;

namespace Jackhammer.Screens
{
    public class GameplayScreen : Screen
    {
        public string BeatmapName { get; }
        public Beatmap CurrentBeatmap { get; private set; }
        public Skin.Skin CurrentSkin { get; }

        public int CurrentTime { get; private set; }

        public float ScrollingSpeed { get; private set; }

        public bool IsUpsideDown { get; private set; }

        private SpriteBatch _spriteBatch;
        private readonly Game _game;

        private Texture2D _background;

        private SoundEffect _song;
 
        public GameplayScreen(Game game, string beatmapName, Skin.Skin skin)
        {
            _game = game;
            BeatmapName = beatmapName;
            CurrentSkin = skin;
            CurrentTime = 0;
            
            ScrollingSpeed = 1.4f;

            IsUpsideDown = false;
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            try
            {
                CurrentBeatmap = BeatmapReader.LoadFromFile(BeatmapName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR WHILE OPENING BEATMAP FILE: {e}");
                throw;
            }

            FileStream fs =
                new FileStream(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", BeatmapName,
                        CurrentBeatmap.Settings.General.BackgroundFileName), FileMode.Open);
            _background = Texture2D.FromStream(_game.GraphicsDevice, fs);
            fs.Dispose();

            FileStream fs2 = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", BeatmapName,
                CurrentBeatmap.Settings.General.AudioFileName), FileMode.Open);
            _song = SoundEffect.FromStream(fs2);

            _song.Play();

            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            CurrentTime += gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.WasKeyPressed(Keys.F4))
                if (ScrollingSpeed < 10.0f)
                    ScrollingSpeed += 0.1f;
            if (InputManager.WasKeyPressed(Keys.F3))
                if (ScrollingSpeed > 0.2f)
                    ScrollingSpeed -= 0.1f;
            if (InputManager.WasKeyPressed(Keys.F5))
            {
                IsUpsideDown = !IsUpsideDown;
            }

            if (InputManager.WasKeyPressed(Keys.OemTilde))
            {
                
                CurrentTime = 0;
                _song.Play();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            
            _spriteBatch.Draw(CurrentSkin.PlayfieldLineTexture, new Vector2(200, 0), Color.White);
            // TODO: Remake 720 literal to the window width
            _spriteBatch.Draw(CurrentSkin.ButtonTexture, IsUpsideDown ? new Vector2(200, 0) : new Vector2(200, 720 - CurrentSkin.ButtonTexture.Height), Color.White);

            foreach (var o in CurrentBeatmap.HitObjects)
            {
                int k = IsUpsideDown ? -1 : 1;
                // TODO: Remake 720 literal to the window width
                _spriteBatch.Draw(CurrentSkin.NoteClickTexture, new Vector2((o.Line + 1) * 100, k * (CurrentTime - o.Position + 720 - CurrentSkin.ButtonTexture.Height) * ScrollingSpeed), Color.White);
            }

            _spriteBatch.DrawString(CurrentSkin.Font, CurrentTime.ToString(), new Vector2(12, 12), Color.Red);
            _spriteBatch.DrawString(CurrentSkin.Font, IsUpsideDown.ToString(), new Vector2(12, 30), Color.Red);
            _spriteBatch.DrawString(CurrentSkin.Font, ScrollingSpeed.ToString("F1"), new Vector2(12, 48), Color.Red);

            _spriteBatch.End();
        }
    }
}
