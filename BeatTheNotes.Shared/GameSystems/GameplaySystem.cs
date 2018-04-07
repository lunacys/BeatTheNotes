using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins;
using BeatTheNotes.Input;
using BeatTheNotes.Framework;
using BeatTheNotes.Framework.Audio;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Shared.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BeatTheNotes.GameSystems
{
    public class GameplaySystem : GameSystem
    {
        public Beatmap Beatmap { get; }
        public Skin Skin { get; }
        //public long Time { get; private set; }

        public GameSettings Settings => _game.Services.GetService<GameSettings>();

        public float ScrollingSpeed => Settings.ScrollingSpeedF;
        public bool IsUpsideDown => Settings.IsReversedDirection;

        public List<HitObject>[] SeparatedLines { get; private set; }

        private readonly GameRoot _game;
        private Texture2D _background;
        //private readonly Song _song;

        private SpriteBatch _spriteBatch;
        private Music _music;

        private InputHandler _input;

        //private VorbisWaveReader _waveReader;
        //private WaveOutEvent _wave;

        //private Music _music;

        private string _beatmapName;

        public GameplaySystem(GameRoot game, string beatmapName)
        {
            _game = game;
            Beatmap = LoadBeatmap(beatmapName);
            //CurrentTimingPoint = Beatmap.TimingPoints[0];
            //_song = LoadSong(beatmapName);
            //_music.PlaybackRate = 1.5f;

            //_music.Play();
            _beatmapName = beatmapName;

            Skin = _game.Services.GetService<Skin>();

            _input = new InputHandler();

            _input.RegisterKeyCommand(Settings.N1, new KeyLineCommand(this, 1));
            _input.RegisterKeyCommand(Settings.N2, new KeyLineCommand(this, 2));
            _input.RegisterKeyCommand(Settings.N3, new KeyLineCommand(this, 3));
            _input.RegisterKeyCommand(Settings.N4, new KeyLineCommand(this, 4));
            //Time = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            //MediaPlayer.Play(_song);
            FindSystem<MusicSystem>().Music = _music;
            MediaPlayer.Volume = Settings.SongVolumeF;

            FindSystem<HealthSystem>().HpDrainRate = Beatmap.Settings.Difficulty.HpDrainRate;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            var time = FindSystem<GameTimeSystem>().Time;

            if (InputManager.WasKeyPressed(Keys.F4))
                if (ScrollingSpeed < 20.0f)
                    Settings.ScrollingSpeedF += 0.1f;
            if (InputManager.WasKeyPressed(Keys.F3))
                if (ScrollingSpeed > 0.2f)
                    Settings.ScrollingSpeedF -= 0.1f;
            /*if (InputManager.WasKeyPressed(Keys.F5))
                Settings.IsReversedDirection = !IsUpsideDown;*/

            
            // Handle Input for gameplay keys
            _input.Update();
            var command = _input.HandleInput();
            command?.Execute();

            // If one of keys was pressed, play a hit sound
            // TODO: Link to TimingPoint hit sound
            if (InputManager.WasKeyPressed(Settings.N1) || InputManager.WasKeyPressed(Settings.N2) ||
                InputManager.WasKeyPressed(Settings.N3) || InputManager.WasKeyPressed(Settings.N4))
            {
                Skin.HitNormal.Play();
            }

            // If last object of the beatmap has been reached..
            if (Beatmap.HitObjects.Last().Position + 1000 < time)
            {
                // ..The beatmap is over, so smoothely low the music volume and reset if volume is too low

                if (FindSystem<MusicSystem>().Volume > 0.01f)
                    FindSystem<MusicSystem>().Volume -= gameTime.ElapsedGameTime.Milliseconds / 5000.0f;

                if (FindSystem<MusicSystem>().Volume <= 0.05f)
                    Reset();
                //MediaPlayer.Stop();

            }

            /*foreach (var tp in Beatmap.TimingPoints)
            {
                if (Math.Abs(time - tp.Position) <= 10)
                    CurrentTimingPoint = tp;
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var time = (long)FindSystem<GameTimeSystem>().Time;

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
                        (ScrollingSpeed * (time - o.Position) +
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
                                (ScrollingSpeed * (time - (o.Position + i)) +
                                 (Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                                Skin.Settings.HitPosition
                            );



                            _spriteBatch.Draw(hitObjectTexture, position, null, o.IsPressed ? Color.Black : Color.White);
                        }
                    }

                    if (o.IsPressed) continue;

                    var scoreSys = GameSystemManager.FindSystem<ScoreSystem>();

                    if (o.Position + scoreSys.HitThresholds[scoreSys.ScoreMiss] < time)
                    {
                        scoreSys.Calculate(o, null);
                    }
                }
            }

            DrawBeatDivisors();

            // Debug things
            _spriteBatch.DrawString(Skin.Font, time.ToString(), new Vector2(12, 12), Color.Red);
            _spriteBatch.DrawString(Skin.Font, IsUpsideDown.ToString(), new Vector2(12, 30), Color.Red);
            _spriteBatch.DrawString(Skin.Font, ScrollingSpeed.ToString("F1"), new Vector2(12, 48), Color.Red);

            _spriteBatch.DrawString(Skin.Font,
                $"Song Speed: {FindSystem<MusicSystem>().PlaybackRate:F1}",
                new Vector2(12, 64),
                Color.DarkRed);

            var scoreSystem = GameSystemManager.FindSystem<ScoreSystem>();

            string scores = $"Marvelous: {scoreSystem.MarvelousCount}\n" +
                            $"Perfect: {scoreSystem.PerfectCount}\n" +
                            $"Great: {scoreSystem.GreatCount}\n" +
                            $"Good: {scoreSystem.GoodCount}\n" +
                            $"Bad: {scoreSystem.BadCount}\n" +
                            $"Miss: {scoreSystem.MissCount}";
            _spriteBatch.DrawString(Skin.Font, scores, new Vector2(800, 10), Color.Black);
            _spriteBatch.DrawString(Skin.Font,
                $"Score: {scoreSystem.Score}\n" +
                $"Combo: {scoreSystem.Combo}\n" +
                $"Acc: {scoreSystem.Accuracy * 100:F2}%\n" +
                $"HP: {FindSystem<HealthSystem>().Health}",
                new Vector2(15, 300), Color.Black);


            float hpX = Skin.Settings.PlayfieldPositionX +
                        Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount;
            float hpY = _game.Services.GetService<GameSettings>().WindowHeight;
            float hpW = 20;
            float hpH = _game.Services.GetService<GameSettings>().WindowHeight;

            //_spriteBatch.FillRectangle(hpX, hpY, hpW, hpH, Color.Gray);
            _spriteBatch.Draw(Skin.HealthBarBg, new Vector2(hpX, hpY - Skin.HealthBarBg.Height), null, Color.White,
                0.0f, Vector2.One, new Vector2(1.0f, 1.0f), SpriteEffects.None, 0.0f);

            var curVal = FindSystem<HealthSystem>().Health;
            var maxVal = FindSystem<HealthSystem>().MaxHealth;
            var minVal = FindSystem<HealthSystem>().MinHealth;

            //Console.WriteLine(curVal);

            var srcRect = new Rectangle(0, 0, (int)hpW, (int)(Skin.HealthBar.Height * (curVal / (maxVal - minVal))));

            Color col = Color.Lerp(Color.Red, Color.Green, curVal / 100.0f);

            _spriteBatch.Draw(Skin.HealthBar, new Vector2(hpX, hpY), srcRect, col, (float)Math.PI, new Vector2(hpW, 0),
                Vector2.One, SpriteEffects.None, 0.0f);

            _spriteBatch.End();
        }

        public override void Reset()
        {
            base.Reset();

            foreach (var o in Beatmap.HitObjects)
            {
                o.IsPressed = false;
            }

            FindSystem<MusicSystem>().Volume = _game.Services.GetService<GameSettings>().SongVolumeF;
        }

        private void HandleInput(InputHandler inputHandler)
        {
            
        }

        private void DrawBeatDivisors()
        {
            // TODO: Skip unnecessary loops
            var tp = GetCurrentTimingPoint();
            for (int i = tp.Position; i <= Beatmap.HitObjects.Last().Position; i += (int)Math.Floor(tp.MsPerBeat * 4))
            {
                float posY = (ScrollingSpeed * ((long)FindSystem<GameTimeSystem>().Time - i) +
                              (Settings.WindowHeight - Skin.ButtonTexture.Height) + Skin.NoteClickTexture.Height +
                              Skin.Settings.HitPosition);
                _spriteBatch.DrawLine(new Vector2(Skin.Settings.PlayfieldPositionX, posY),
                    new Vector2(Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount, posY), Color.Gray,
                    3.0f);
            }
        }

        public TimingPoint GetCurrentTimingPoint()
        {
            // TODO: This
            // If there's no timing point at the time, return the first timing point
            if (Beatmap.TimingPoints[0].Position < FindSystem<GameTimeSystem>().Time)
                return Beatmap.TimingPoints[0];

            // If there's the only one timing point on the map, return that timing point
            if (Beatmap.TimingPoints.Count == 1)
                return Beatmap.TimingPoints[0];

            /*foreach (var tp in Beatmap.TimingPoints)
            {
                if (tp.Position < Time)
                {
                    
                    return tp;
                }
            }*/

            for (int i = 0; i < Beatmap.TimingPoints.Count - 1; i++)
            {
                var cur = Beatmap.TimingPoints[i].Position;
                var next = Beatmap.TimingPoints[i + 1].Position;

                //if ()
                {
                    //    Console.WriteLine($"returning tp: {Beatmap.TimingPoints[i].Position}");
                    //    return Beatmap.TimingPoints[i];
                }
            }

            return Beatmap.TimingPoints[0];
        }

        /// <summary>
        /// Find and return the nearest object on the specified line. The nearest object is the first object on the line.
        /// </summary>
        /// <param name="line">Line starting from 1 to KeyAmount</param>
        /// <returns>Nearest object</returns>
        public HitObject GetNearestHitObjectOnLine(int line, bool isLongNote = false)
        {
            if (SeparatedLines[line - 1].Count == 0)
                return null;

            if (SeparatedLines[line - 1].Count(o => !o.IsPressed) == 0)
                return null;

            var first = SeparatedLines[line - 1].First(o => !o.IsPressed);

            var ss = GameSystemManager.FindSystem<ScoreSystem>();

            if (!isLongNote)
                return Math.Abs(FindSystem<GameTimeSystem>().Time - first.Position) <= (ss.HitThresholds[ss.ScoreMiss]) ? first : null;

            return Math.Abs(FindSystem<GameTimeSystem>().Time - first.EndPosition) <= (ss.HitThresholds[ss.ScoreMiss]) ? first : null;
        }

        private Beatmap LoadBeatmap(string beatmapName)
        {
            Beatmap beatmap;

            // Load Beatmap
            try
            {
                beatmap = BeatmapReader.LoadFromFile(_game.GraphicsDevice, _game.Services.GetService<GameSettings>().BeatmapFolder, beatmapName, out _background, out _music);
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
    }
}
