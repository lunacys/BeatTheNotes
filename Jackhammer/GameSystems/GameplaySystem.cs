using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jackhammer.Input;
using Jackhammer.Screens;
using Jackhammer.Skins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;

namespace Jackhammer.GameSystems
{
    public class GameplaySystem : GameSystem
    {
        public Beatmap Beatmap { get; }
        public Skin Skin { get; }
        public int Time { get; private set; }

        public GameSettings Settings => _game.Services.GetService<GameSettings>();

        public float ScrollingSpeed => Settings.ScrollingSpeedF;
        public bool IsUpsideDown => Settings.IsReversedDirection;

        public List<HitObject>[] SeparatedLines { get; private set; }

        private readonly Jackhammer _game;
        private readonly Texture2D _background;
        private readonly Song _song;

        private SpriteBatch _spriteBatch;

        public GameplaySystem(Jackhammer game, string beatmapName)
        {
            _game = game;
            Beatmap = LoadBeatmap(beatmapName);
            _background = LoadBackground(beatmapName);
            _song = LoadSong(beatmapName);
            
            Skin = _game.Services.GetService<Skin>();
            Time = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            MediaPlayer.Play(_song);
            MediaPlayer.Volume = Settings.SongVolumeF;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Time += gameTime.ElapsedGameTime.Milliseconds;

            if (InputManager.WasKeyPressed(Keys.F4))
                if (ScrollingSpeed < 20.0f)
                    Settings.ScrollingSpeedF += 0.1f;
            if (InputManager.WasKeyPressed(Keys.F3))
                if (ScrollingSpeed > 0.2f)
                    Settings.ScrollingSpeedF -= 0.1f;
            /*if (InputManager.WasKeyPressed(Keys.F5))
                Settings.IsReversedDirection = !IsUpsideDown;*/

            HandleInput();

            if (Beatmap.HitObjects.Last().Position + 1000 < Time)
            {
                // The beatmap is over
                MediaPlayer.Volume -= gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
                //MediaPlayer.Stop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_background, new Rectangle(0, 0, Settings.WindowWidth, Settings.WindowHeight), Color.White);

            for (int i = 0; i < Beatmap.Settings.Difficulty.KeyAmount; i++)
            {
                int x = Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * i;
                var destRect = new Rectangle(x, 0, Skin.PlayfieldLineTexture.Width, Settings.WindowHeight);

                _spriteBatch.Draw(Skin.PlayfieldLineTexture, destRect, Color.White);
            }


            _spriteBatch.Draw(Skin.ButtonTexture,
                IsUpsideDown
                    ? new Vector2(Skin.Settings.PlayfieldPositionX, 0)
                    : new Vector2(Skin.Settings.PlayfieldPositionX,
                        Settings.WindowHeight - Skin.ButtonTexture.Height),
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
                         (Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                        Skin.Settings.HitPosition
                    );

                    Texture2D hitObjectTexture = Skin.NoteClickTexture;

                    if (Skin.Settings.NoteType.ToLower() == "arrow")
                    {
                        // For 'arrow' skin type use texture collection
                        hitObjectTexture = Skin.NoteClickTextures[o.Line - 1];
                    }

                    // if it is a 'Click' note
                    if (o.Position == o.EndPosition)
                    {
                        _spriteBatch.Draw(hitObjectTexture, position, o.IsPressed ? Color.Black : Color.White);
                    }
                    else // if it is a 'Hold' note
                    {
                        for (int i = 0; i <= o.EndPosition - o.Position; i += 5)
                        {
                            position = new Vector2(
                                // x
                                Skin.Settings.PlayfieldPositionX +
                                ((o.Line - 1) * (Skin.PlayfieldLineTexture.Width)),
                                // y
                                (ScrollingSpeed * (Time - (o.Position + i)) +
                                 (Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                                Skin.Settings.HitPosition
                            );

                            _spriteBatch.Draw(hitObjectTexture, position, null, o.IsPressed ? Color.Black : Color.White);
                        }
                    }

                    if (o.IsPressed) continue;

                    var scoreSys = GameSystemManager.FindSystem<ScoreSystem>();
                    
                    if (o.Position + scoreSys.HitThresholds["Miss"] < Time)
                    {
                        scoreSys.Calculate(o);
                    }
                }
            }
            
            DrawBeatDivisors();

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
        }
        
        public override void Reset()
        {
            base.Reset();

            foreach (var o in Beatmap.HitObjects)
            {
                o.IsPressed = false;
            }

            Time = 0;
            MediaPlayer.Stop();
            MediaPlayer.Volume = Settings.SongVolumeF;
            MediaPlayer.Play(_song);
        }

        private void HandleInput()
        {
            ScoreSystem scoreSystem = GameSystemManager.FindSystem<ScoreSystem>();

            if (InputManager.WasKeyPressed(Settings.N1))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(1);

                if (nearest != null)
                    scoreSystem.Calculate(nearest);
            }
            if (InputManager.WasKeyPressed(Settings.N2))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(2);

                if (nearest != null)
                    scoreSystem.Calculate(nearest);
            }
            if (InputManager.WasKeyPressed(Settings.N3))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(3);

                if (nearest != null)
                    scoreSystem.Calculate(nearest);
            }
            if (InputManager.WasKeyPressed(Settings.N4))
            {
                Skin.HitNormal.Play();

                var nearest = GetNearestHitObjectOnLine(4);

                if (nearest != null)
                    scoreSystem.Calculate(nearest);
            }
        }

        private void DrawBeatDivisors()
        {
            // TODO: Skip unnecessary loops
            var tp = Beatmap.TimingPoints[0];
            for (int i = tp.Position; i <= Beatmap.HitObjects.Last().Position; i += (int)Math.Floor(tp.MsPerBeat * 4))
            {
                float posY = (ScrollingSpeed * (Time - i) +
                              (Settings.WindowHeight - Skin.ButtonTexture.Height) + Skin.NoteClickTexture.Height +
                              Skin.Settings.HitPosition);
                _spriteBatch.DrawLine(new Vector2(Skin.Settings.PlayfieldPositionX, posY),
                    new Vector2(Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount, posY), Color.Gray,
                    3.0f);
            }
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

            return Math.Abs(Time - first.Position) <= (GameSystemManager.FindSystem<ScoreSystem>().HitThresholds["Miss"]) ? first : null;
        }

        private Beatmap LoadBeatmap(string beatmapName)
        {
            Beatmap beatmap;

            // Load Beatmap
            try
            {
                beatmap = BeatmapReader.LoadFromFile(beatmapName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            // Create separated lines collections and fill it
            SeparatedLines = new List<HitObject>[beatmap.Settings.Difficulty.KeyAmount];
            for (int i = 0; i < SeparatedLines.Length; i++)
                SeparatedLines[i] = beatmap.HitObjects.FindAll(o => o.Line == i + 1);

            return beatmap;
        }

        private Texture2D LoadBackground(string beatmapName)
        {
            Texture2D bg;
            try
            {
                FileStream fs =
                    new FileStream(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", beatmapName,
                            Beatmap.Settings.General.BackgroundFileName), FileMode.Open);
                bg = Texture2D.FromStream(_game.GraphicsDevice, fs);
                fs.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.Log($"GameplayScreen: Error while opening Background file. Using empty background instead: {e}");
                bg = Skin.DefaultBackground;
            }

            return bg;
        }

        private Song LoadSong(string beatmapName)
        {
            Song song = Song.FromUri("BrainPower.ogg",
                new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Maps", beatmapName, "BrainPower.ogg")));

            return song;
        }
    }
}
