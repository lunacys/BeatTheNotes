using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jackhammer.GameSystems;
using Jackhammer.Skins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;
using Jackhammer.Input;

namespace Jackhammer.Screens
{
    public class GameplayScreen : Screen
    {
        public Beatmap Beatmap { get; private set; }

        private readonly string _beatmapName;
        public Skin Skin { get; }

        /// <summary>
        /// Current time used in the game as starting point for all the hit objects and the song
        /// </summary>
        public int Time { get; private set; }

        public float ScrollingSpeed => _game.Settings.ScrollingSpeedF;
        public bool IsUpsideDown => _game.Settings.IsReversedDirection;
        public GameSettings Settings => _game.Settings;

        private SpriteBatch _spriteBatch;
        private readonly Jackhammer _game;

        private Texture2D _background;
        private Song _song;
        
        public GameSystemManager GameSystemManager { get; }

        public List<HitObject>[] SeparatedLines { get; private set; }

        public GameplayScreen(Jackhammer game, string beatmapName)
        {
            _game = game;
            _beatmapName = beatmapName;
            Skin = game.UsedSkin;
            Time = 0;
            
            
            GameSystemManager = new GameSystemManager(game);
        }

        public override void Initialize()
        {
            LogHelper.Log("GameplayScreen: Initializing");

            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            // Load Beatmap
            try
            {
                Beatmap = BeatmapReader.LoadFromFile(_beatmapName);
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameplayScreen: Error while loading Beatmap. Backing to the Song Choose Screen: {e}", LogLevel.Error);
                ScreenManager.FindScreen<PlaySongSelectScreen>().Show();
            }
            
            GameSystemManager.Register(new ScoreSystem(this, _game.GraphicsDevice));
            GameSystemManager.Register(new ScoremeterSystem(_game.GraphicsDevice, Skin, Beatmap));
            //GameSystems.Add(_beatDivisorSystem);

            // Create separated lines collections and fill it
            SeparatedLines = new List<HitObject>[Beatmap.Settings.Difficulty.KeyAmount];
            for (int i = 0; i < SeparatedLines.Length; i++)
                SeparatedLines[i] = Beatmap.HitObjects.FindAll(o => o.Line == i + 1);

            // Load Background
            try
            {
                FileStream fs =
                    new FileStream(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", _beatmapName,
                            Beatmap.Settings.General.BackgroundFileName), FileMode.Open);
                _background = Texture2D.FromStream(_game.GraphicsDevice, fs);
                fs.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameplayScreen: Error while opening Background file. Using empty background instead: {e}");
                _background = Skin.DefaultBackground;
            }

            // Load and play the song
            _song = Song.FromUri("BrainPower.ogg",
                new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", _beatmapName, "BrainPower.ogg")));

            MediaPlayer.Play(_song);
            MediaPlayer.Volume = _game.Settings.SongVolumeF;

            LogHelper.Log("GameplayScreen: Sucessfully Initialized");
        }

        public override void LoadContent()
        {
            LogHelper.Log("GameplayScreen: Loading Content");
            base.LoadContent();
            LogHelper.Log("GameplayScreen: Sucessfully Loaded Content");
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(_game);
            
            Time += gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.WasKeyPressed(Keys.Escape))
            {
                Show<PauseScreen>(true);
                MediaPlayer.Pause();
            }

            if (InputManager.WasKeyPressed(Keys.F4))
                if (ScrollingSpeed < 10.0f)
                    _game.Settings.ScrollingSpeedF += 0.1f;
            if (InputManager.WasKeyPressed(Keys.F3))
                if (ScrollingSpeed > 0.2f)
                    _game.Settings.ScrollingSpeedF -= 0.1f;
            if (InputManager.WasKeyPressed(Keys.F5))
            {
                _game.Settings.IsReversedDirection = !IsUpsideDown;
            }
            
            if (InputManager.IsKeyDown(Keys.OemTilde))
            {
                Restart();
            }

            var scoreSystem = GameSystemManager.FindSystem<ScoreSystem>();
            var scoremeterSystem = GameSystemManager.FindSystem<ScoremeterSystem>();

            if (InputManager.WasKeyPressed(_game.Settings.N1))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(1);

                if (nearest != null && scoreSystem.Calculate(nearest))
                {
                    nearest.IsPressed = true;
                    scoremeterSystem.AddScore(Time, nearest.Position);
                }
            }
            if (InputManager.WasKeyPressed(_game.Settings.N2))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(2);

                if (nearest != null && scoreSystem.Calculate(nearest))
                {
                    nearest.IsPressed = true;
                    scoremeterSystem.AddScore(Time, nearest.Position);
                }
            }
            if (InputManager.WasKeyPressed(_game.Settings.N3))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(3);

                if (nearest != null && scoreSystem.Calculate(nearest))
                {
                    nearest.IsPressed = true;
                    scoremeterSystem.AddScore(Time, nearest.Position);
                }
            }
            if (InputManager.WasKeyPressed(_game.Settings.N4))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(4);

                if (nearest != null && scoreSystem.Calculate(nearest))
                {
                    nearest.IsPressed = true;
                    scoremeterSystem.AddScore(Time, nearest.Position);
                }
            }

            if (InputManager.IsKeyDown(Keys.Space))
                Restart(23000);

            // Updating..

            if (Beatmap.HitObjects.Last().Position + 1000 < Time)
            {
                // The beatmap is over
                MediaPlayer.Stop();
            }

            GameSystemManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            for (int i = 0; i < Beatmap.Settings.Difficulty.KeyAmount; i++)
            {
                _spriteBatch.Draw(Skin.PlayfieldLineTexture, new Vector2(Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * i, 0), Color.White);
            }
            

            _spriteBatch.Draw(Skin.ButtonTexture,
                IsUpsideDown
                    ? new Vector2(Skin.Settings.PlayfieldPositionX, 0)
                    : new Vector2(Skin.Settings.PlayfieldPositionX,
                        _game.Settings.WindowHeight - Skin.ButtonTexture.Height),
                Color.White);

            foreach (var lines in SeparatedLines)
            {
                foreach (var o in lines)
                {
                    // TODO: Make more precise work on the numbers
                    // TODO: Make _isUpsideDown (upside down) work
                    // TODO: Skip unnecessary loops
                    //int k = IsUpsideDown ? -1 : 1;

                    // Set correct position for the object
                    Vector2 position = new Vector2(
                        // x
                        Skin.Settings.PlayfieldPositionX +
                        ((o.Line - 1) * (Skin.PlayfieldLineTexture.Width)),
                        // y
                        (ScrollingSpeed * (Time - o.Position) +
                         (_game.Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                        Skin.Settings.HitPosition
                    );

                    Texture2D hitObjectTexture = Skin.NoteClickTexture;

                    if (Skin.Settings.NoteType.ToLower() == "arrow")
                    {
                        // For 'arrow' skin type use texture collection
                        switch (o.Line)
                        {
                            case 1: hitObjectTexture = Skin.NoteClickTextures[0]; break;
                            case 2: hitObjectTexture = Skin.NoteClickTextures[1]; break;
                            case 3: hitObjectTexture = Skin.NoteClickTextures[2]; break;
                            case 4: hitObjectTexture = Skin.NoteClickTextures[3]; break;
                        }
                    }
                    
                    /*if (o.Position - Beatmap.TimingPoints[0].Position % (int)Math.Floor(Beatmap.TimingPoints[0].MsPerBeat * 4) <= 10)
                    {
                        Console.WriteLine($"Line!");
                        //_spriteBatch.DrawLine(position, position + (Vector2.UnitX * Skin.PlayfieldLineTexture.Width), Color.Gray, 3.0f);
                    }*/

                    _spriteBatch.Draw(hitObjectTexture, position, o.IsPressed ? Color.Black : Color.White);
                }
            }
            
            // Draw Beat Divisors
            // TODO: Skip unnecessary loops
            var tp = Beatmap.TimingPoints[0];
            for (int i = tp.Position; i <= Beatmap.HitObjects.Last().Position; i += (int) Math.Floor(tp.MsPerBeat * 4))
            {
                float posY = (ScrollingSpeed * (Time - i) +
                              (_game.Settings.WindowHeight - Skin.ButtonTexture.Height) + Skin.NoteClickTexture.Height +
                              Skin.Settings.HitPosition);
                _spriteBatch.DrawLine(new Vector2(Skin.Settings.PlayfieldPositionX, posY),
                    new Vector2(Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount, posY), Color.Gray,
                    3.0f);
            }

            // Debug things
            _spriteBatch.DrawString(Skin.Font, Time.ToString(), new Vector2(12, 12), Color.Red);
            _spriteBatch.DrawString(Skin.Font, IsUpsideDown.ToString(), new Vector2(12, 30), Color.Red);
            _spriteBatch.DrawString(Skin.Font, ScrollingSpeed.ToString("F1"), new Vector2(12, 48), Color.Red);

            var scoreSystem = GameSystemManager.FindSystem<ScoreSystem>();

            string scores = $"Marvelous: {scoreSystem.MarvelousCount}\n" +
                            $"Perfect: {scoreSystem.PerfectCount}\n" +
                            $"Great: {scoreSystem.GreatCount}\n" +
                            $"Good: {scoreSystem.GoodCount}\n" +
                            $"Bad: {scoreSystem.BadCount}\n" +
                            $"Miss: {scoreSystem.MissCount}";
            _spriteBatch.DrawString(Skin.Font, scores, new Vector2(800, 10), Color.Black);
            _spriteBatch.DrawString(Skin.Font, $"Score: {scoreSystem.Score}\nCombo: {scoreSystem.Combo}\nAcc: {scoreSystem.Accuracy * 100:F2}%", new Vector2(15, 300), Color.Black);

            _spriteBatch.End();

            GameSystemManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Restart the game. It means reset the time and the song
        /// </summary>
        private void Restart(int ms = 0)
        {
            foreach (var o in Beatmap.HitObjects)
            {
                o.IsPressed = false;
            }
            
            Time = ms;
            MediaPlayer.Stop();
            MediaPlayer.Play(_song, TimeSpan.FromMilliseconds(ms + Skin.Settings.HitPosition * 2));

            GameSystemManager.Reset();
        }

        /// <summary>
        /// Find and return the nearest object on the specified line. The nearest object is the first object on the line.
        /// </summary>
        /// <param name="line">Line starting from 1 to KeyAmount</param>
        /// <returns>Neares object</returns>
        private HitObject GetNearestHitObjectOnLine(int line)
        {
            if (SeparatedLines[line - 1].Count == 0)
                return null;

            if (SeparatedLines[line - 1].Count(o => !o.IsPressed) == 0)
                return null;

            var first = SeparatedLines[line - 1].First(o => !o.IsPressed);
            
            return Math.Abs(Time - first.Position) <= (188 - (3 * Beatmap.Settings.Difficulty.OverallDifficutly)) ? first : null;
        }
    }
}
