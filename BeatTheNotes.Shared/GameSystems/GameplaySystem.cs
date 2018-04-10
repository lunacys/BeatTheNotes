using System;
using System.Collections.Generic;
using System.Linq;
using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins;
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

        public GameSettings Settings => _game.Services.GetService<GameSettings>();

        public float ScrollingSpeed => Settings.ScrollingSpeedF;
        public bool IsUpsideDown => Settings.IsReversedDirection;

        public List<HitObject>[] SeparatedLines { get; private set; }

        private readonly GameRoot _game;
        private Texture2D _background;

        private SpriteBatch _spriteBatch;
        private Music _music;

        private readonly InputHandler _input;


        public GameplaySystem(GameRoot game, string beatmapName)
        {
            _game = game;
            Beatmap = LoadBeatmap(beatmapName);

            Skin = _game.Services.GetService<Skin>();

            _input = new InputHandler();

            _input.RegisterKeyCommand(Settings.GameKeys["KL1"], new KeyLineCommand(this, 1));
            _input.RegisterKeyCommand(Settings.GameKeys["KL2"], new KeyLineCommand(this, 2));
            _input.RegisterKeyCommand(Settings.GameKeys["KL3"], new KeyLineCommand(this, 3));
            _input.RegisterKeyCommand(Settings.GameKeys["KL4"], new KeyLineCommand(this, 4));
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            FindSystem<MusicSystem>().Music = _music;
            MediaPlayer.Volume = Settings.SongVolumeF;

            FindSystem<HealthSystem>().HpDrainRate = Beatmap.Settings.Difficulty.HpDrainRate;

            InitializeAllHitObjects();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = FindSystem<GameTimeSystem>().Time;

            _input.Update(_game);

            if (_input.WasKeyPressed(Settings.GameKeys["BeatmapScrollingSpeedUp"]))
                if (ScrollingSpeed < 20.0f)
                    Settings.ScrollingSpeedF += 0.1f;
            if (_input.WasKeyPressed(Settings.GameKeys["BeatmapScrollingSpeedDown"]))
                if (ScrollingSpeed > 0.2f)
                    Settings.ScrollingSpeedF -= 0.1f;
            /*if (_input.WasKeyPressed(Keys.F5))
                Settings.IsReversedDirection = !IsUpsideDown;*/


            // Handle Input for gameplay keys
            var command = _input.HandleInput(_input.WasKeyPressed);
            foreach (var input in command)
                input.Execute();


            // If one of keys was pressed, play a hit sound
            // TODO: Link to TimingPoint hit sound
            /*if (_input.WasKeyPressed(Settings.N1) || _input.WasKeyPressed(Settings.N2) ||
                _input.WasKeyPressed(Settings.N3) || _input.WasKeyPressed(Settings.N4))
            {
                Skin.HitNormal.Play();
            }*/

            // If last object of the beatmap has been reached..
            if (Beatmap.HitObjects.Last().Position + 1000 < time)
            {
                // ..The beatmap is over, so smoothly low the music volume and reset if volume is too low
                if (FindSystem<MusicSystem>().Volume > 0.01f)
                    FindSystem<MusicSystem>().Volume -= gameTime.ElapsedGameTime.Milliseconds / 5000.0f;

                if (FindSystem<MusicSystem>().Volume <= 0.05f)
                    Reset();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            // Draw background before all the screen components
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Settings.WindowWidth, Settings.WindowHeight), Color.White);

            // Draw screen components in the order
            DrawPlayfield();
            DrawBeatDivisors();
            DrawHitObjects();

            // Draw UI
            DrawUi();

            _spriteBatch.End();
        }

        /// <summary>
        /// Reset the game state
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            foreach (var o in Beatmap.HitObjects)
                o.Reset();

            FindSystem<MusicSystem>().Volume = _game.Services.GetService<GameSettings>().SongVolumeF;
        }

        private void DrawPlayfield()
        {
            // Draw as many playfield lines as needed from the beatmap key amount setting
            for (int i = 0; i < Beatmap.Settings.Difficulty.KeyAmount; i++)
            {
                int x = Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * i;
                var destRect = new Rectangle(x, 0, Skin.PlayfieldLineTexture.Width, Settings.WindowHeight);

                _spriteBatch.Draw(Skin.PlayfieldLineTexture, destRect, Color.White);
            }

            // Draw buttons
            _spriteBatch.Draw(Skin.ButtonTexture,
            IsUpsideDown
                ? new Vector2(Skin.Settings.PlayfieldPositionX, 0)
                : new Vector2(Skin.Settings.PlayfieldPositionX,
                    Settings.WindowHeight - Skin.ButtonTexture.Height),
            Color.White);


            // Init health bar settings
            float hpX = Skin.Settings.PlayfieldPositionX +
                        Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount;
            float hpY = _game.Services.GetService<GameSettings>().WindowHeight;
            float hpW = 20; // TODO: Link to GameSettings

            // Draw health bar background
            _spriteBatch.Draw(Skin.HealthBarBg, new Vector2(hpX, hpY - Skin.HealthBarBg.Height), null, Color.White,
                0.0f, Vector2.One, new Vector2(1.0f, 1.0f), SpriteEffects.None, 0.0f);

            var curVal = FindSystem<HealthSystem>().Health;
            var maxVal = FindSystem<HealthSystem>().MaxHealth;
            var minVal = FindSystem<HealthSystem>().MinHealth;

            // Set source rectangle for making health bar dynamic
            var srcRect = new Rectangle(0, 0, (int)hpW, (int)(Skin.HealthBar.Height * (curVal / (maxVal - minVal))));

            // Interpolate color from Green (max health) to Red (min health)
            Color col = Color.Lerp(Color.Red, Color.Green, curVal / 100.0f);

            // Draw health bar
            // We should rotate the health bar by 180 degrees (PI in radians) because rectangle can't get negative height
            _spriteBatch.Draw(Skin.HealthBar, new Vector2(hpX, hpY), srcRect, col, (float)Math.PI, new Vector2(hpW, 0),
                Vector2.One, SpriteEffects.None, 0.0f);
        }

        private void DrawUi()
        {
            var time = (long)FindSystem<GameTimeSystem>().Time;

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
        }

        private void DrawHitObjects()
        {
            var time = (long)FindSystem<GameTimeSystem>().Time;

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

                    // if it is a 'Click' note, just draw its texture
                    if (o.Position == o.EndPosition)
                    {
                        _spriteBatch.Draw(hitObjectTexture, position, o.IsPressed ? Color.Black : Color.White);
                    }
                    else // if it is a 'Hold' note, draw its texure one time per 5 pixels
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

                    // Proceed miss
                    // It handles in draw method because of optimization
                    if (o.IsPressed) continue;

                    var scoreSys = GameSystemManager.FindSystem<ScoreSystem>();

                    if (o.Position + scoreSys.HitThresholds[scoreSys.ScoreMiss] < time)
                    {
                        o.DoHit();
                    }
                }
            }
        }

        private void DrawBeatDivisors()
        {
            // TODO: Skip unnecessary loops
            var tp = GetCurrentTimingPoint();

            //Console.WriteLine($"current TP pos: {tp.Position}");

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
            if (Beatmap.TimingPoints[0].Position > FindSystem<GameTimeSystem>().Time)
                return Beatmap.TimingPoints[0];

            // If there's the only one timing point on the map, return that timing point
            if (Beatmap.TimingPoints.Count == 1)
                return Beatmap.TimingPoints[0];

            return FindTimingPointByTime(FindSystem<GameTimeSystem>().Time);
        }

        private TimingPoint FindTimingPointByTime(double time)
        {
            // TODO: This
            var timingPoint = Beatmap.TimingPoints[0];
            var cur = Beatmap.TimingPoints[0];
            var next = Beatmap.TimingPoints[1];
            foreach (var tp in Beatmap.TimingPoints)
            {
                if (cur.Position < time && next.Position > time)
                {
                    timingPoint = cur;
                }

                cur = next;
            }

            return timingPoint;
        }

        /// <summary>
        /// Find and return the nearest object on the specified line. The nearest object is the first object on the line.
        /// </summary>
        /// <param name="line">Line starting from 1 to KeyAmount</param>
        /// <param name="isLongNote">If using Hold Note</param>
        /// <returns>Nearest object on specified line within the threshold if found one, otherwise return null</returns>
        public HitObject GetNearestHitObjectOnLine(int line, bool isLongNote = false)
        {
            // If there are no objects on the line, return null
            if (SeparatedLines[line - 1].Count == 0)
                return null;

            // If there are only already pressed objects on the line, return null
            if (SeparatedLines[line - 1].Count(o => !o.IsPressed) == 0)
                return null;

            // Find first unpressed object on the line
            var first = SeparatedLines[line - 1].First(o => !o.IsPressed);

            var ss = GameSystemManager.FindSystem<ScoreSystem>();

            if (!isLongNote)
                return Math.Abs(FindSystem<GameTimeSystem>().Time - first.Position) <= (ss.HitThresholds[ss.ScoreMiss]) ? first : null;

            // If we're using "Click" notes, we checking the end position of the object
            return Math.Abs(FindSystem<GameTimeSystem>().Time - first.EndPosition) <= (ss.HitThresholds[ss.ScoreMiss]) ? first : null;
        }

        /// <summary>
        /// Load beatmap from a file with background texture and music file into memory
        /// </summary>
        /// <param name="beatmapName">Name of the beatmap</param>
        /// <returns>Beatmap class</returns>
        private Beatmap LoadBeatmap(string beatmapName)
        {
            Beatmap beatmap;

            // Load Beatmap
            try
            {
                beatmap = BeatmapReader.LoadFromFile(_game.GraphicsDevice,
                    _game.Services.GetService<GameSettings>().BeatmapFolder, beatmapName, out _background, out _music);
            }
            catch (Exception e)
            {
                // TODO: If there was an error while loading map, return to the song select menu and pop up a notification to user
                throw new Exception(e.Message);
            }

            // Create separated lines collection and fill them
            SeparatedLines = new List<HitObject>[beatmap.Settings.Difficulty.KeyAmount];
            for (int i = 0; i < SeparatedLines.Length; i++)
                SeparatedLines[i] = beatmap.HitObjects.FindAll(o => o.Line == i + 1);

            return beatmap;
        }

        /// <summary>
        /// Add event handlers to all the hit objects from all the game systems if it should process hit objects
        /// </summary>
        private void InitializeAllHitObjects()
        {
            foreach (var separatedLine in SeparatedLines)
                foreach (var hitObject in separatedLine)
                    foreach (var system in GameSystemManager.GetAllGameSystems())
                        if (system is IGameSystemProcessHitObject)
                            hitObject.OnPress += (system as IGameSystemProcessHitObject).OnHitObjectHit;
        }
    }
}
