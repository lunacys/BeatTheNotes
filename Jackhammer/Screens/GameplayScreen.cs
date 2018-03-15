using System;
using System.IO;
using Microsoft.Xna.Framework;
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

        public float ScrollingSpeed => _game.Settings.ScrollingSpeed;
        public bool ReverseDirection => _game.Settings.IsReversedDirection;

        private SpriteBatch _spriteBatch;
        private readonly Jackhammer _game;

        private Texture2D _background;

        private Song _song;
 
        public GameplayScreen(Jackhammer game, string beatmapName)
        {
            _game = game;
            BeatmapName = beatmapName;
            CurrentSkin = game.UsedSkin;
            CurrentTime = 0;
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
                LogHelper.Log($"GameplayScreen: Error while loading Beatmap. Backing to the Song Choose Screen: {e}", LogLevel.Error);
                ScreenManager.FindScreen<PlaySongSelectScreen>().Show();
            }

            // Load Background
            try
            {
                FileStream fs =
                    new FileStream(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", BeatmapName,
                            CurrentBeatmap.Settings.General.BackgroundFileName), FileMode.Open);
                _background = Texture2D.FromStream(_game.GraphicsDevice, fs);
                fs.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameplayScreen: Error while opening Background file. Using empty background instead: {e}");
                _background = CurrentSkin.DefaultBackground;
            }
            
            _song = Song.FromUri("BrainPower.ogg",
                new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", BeatmapName, "BrainPower.ogg")));
            
            MediaPlayer.Play(_song);
            MediaPlayer.Volume = 0.1f; // TODO: FIX 

            base.Initialize();

            LogHelper.Log("GameplayScreen: Sucessfully Initialized");
        }
        
        public override void Update(GameTime gameTime)
        {
            CurrentTime += gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.WasKeyPressed(Keys.F4))
                if (ScrollingSpeed < 10.0f)
                    _game.Settings.ScrollingSpeed += 0.1f;
            if (InputManager.WasKeyPressed(Keys.F3))
                if (ScrollingSpeed > 0.2f)
                    _game.Settings.ScrollingSpeed -= 0.1f;
            if (InputManager.WasKeyPressed(Keys.F5))
            {
                _game.Settings.IsReversedDirection = !ReverseDirection;
            }

            if (InputManager.WasKeyPressed(Keys.OemTilde))
            {
                CurrentTime = 0;
                MediaPlayer.Stop();
                MediaPlayer.Play(_song);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            
            _spriteBatch.Draw(CurrentSkin.PlayfieldLineTexture, new Vector2(200, 0), Color.White);
            // TODO: Remake 720 literal to the window width
            _spriteBatch.Draw(CurrentSkin.ButtonTexture,
                ReverseDirection ? new Vector2(200, 0) : new Vector2(200, 720 - CurrentSkin.ButtonTexture.Height),
                Color.White);

            foreach (var o in CurrentBeatmap.HitObjects)
            {
                int k = ReverseDirection ? -1 : 1;
                // TODO: Remake 720 literal to the window width
                _spriteBatch.Draw(CurrentSkin.NoteClickTexture,
                    new Vector2((o.Line + 1) * 100,
                        k * (CurrentTime - o.Position + 720 - CurrentSkin.ButtonTexture.Height) * ScrollingSpeed),
                    Color.White);
            }

            _spriteBatch.DrawString(CurrentSkin.Font, CurrentTime.ToString(), new Vector2(12, 12), Color.Red);
            _spriteBatch.DrawString(CurrentSkin.Font, ReverseDirection.ToString(), new Vector2(12, 30), Color.Red);
            _spriteBatch.DrawString(CurrentSkin.Font, ScrollingSpeed.ToString("F1"), new Vector2(12, 48), Color.Red);

            _spriteBatch.End();
        }
    }
}
