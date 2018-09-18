using System;
using System.Linq;
using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.Framework.Skins;
using BeatTheNotes.Framework;
using BeatTheNotes.Framework.Graphs;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Objects;
using BeatTheNotes.Screens;
using BeatTheNotes.Shared.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.GameSystems
{
    public class GameplaySystem : GameSystem
    {
        public event EventHandler OnReachedEnd;

        public Beatmap Beatmap { get; set; }
        public Skin Skin { get; }

        public GraphCanvas ScoreGraph { get; private set; }

        private GameSettings Settings => _game.Services.GetService<GameSettings>();

        private float ScrollingSpeed => Settings.ScrollingSpeedF;

        private readonly GameRoot _game;

        private SpriteBatch _spriteBatch;

        private readonly InputHandler _input;

        public GameplaySystem(GameRoot game, Beatmap beatmap)
        {
            _game = game;

            //Beatmap = LoadBeatmap(beatmapName);
            Beatmap = beatmap;

            Skin = _game.Services.GetService<Skin>();



            _input = new InputHandler(game);

            _input.RegisterKeyCommand(Settings.GameKeys["KL1"], new KeyLineCommand(this, Beatmap.HitObjects, 1));
            _input.RegisterKeyCommand(Settings.GameKeys["KL2"], new KeyLineCommand(this, Beatmap.HitObjects, 2));
            _input.RegisterKeyCommand(Settings.GameKeys["KL3"], new KeyLineCommand(this, Beatmap.HitObjects, 3));
            _input.RegisterKeyCommand(Settings.GameKeys["KL4"], new KeyLineCommand(this, Beatmap.HitObjects, 4));
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            FindSystem<MusicSystem>().Music = Beatmap.Music;
            MediaPlayer.Volume = Settings.SongVolumeF;

            FindSystem<HealthSystem>().HpDrainRate = Beatmap.Settings.Difficulty.HpDrainRate;

            ScoreGraph = new GraphCanvas(_game)
            {
                Position = new Vector2(Skin.Settings.PlayfieldPositionX, 300),
                Size = new Size2(Skin.PlayfieldLineTexture.Width * 2, 100),
                MinValue = -158,
                MaxValue = 158,
                Font = Skin.Font,
                ShouldDrawBackground = false,
                ShouldDrawBars = false,
                CellSize = new Size2(5, 20)
            };
            _game.Components.Add(ScoreGraph);

            FindSystem<ScoreV1System>().OnScoreGet += (sender, handler) =>
            {
                ScoreGraph.PushValue(handler.Offset);
            };

            InitializeAllHitObjects();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = FindSystem<GameTimeSystem>().Time;

            _input.Update(gameTime);

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

            if (_input.IsKeyDown(Keys.A))
            {
                ScoreGraph.Size = new Size2(ScoreGraph.Size.Width + 1.0f, ScoreGraph.Size.Height);
                ScoreGraph.CellSize = new Size2(ScoreGraph.CellSize.Width + 0.1f, ScoreGraph.Size.Height);
            }
            if (_input.IsKeyDown(Keys.S))
            {
                ScoreGraph.Size = new Size2(ScoreGraph.Size.Width, ScoreGraph.Size.Height + 1.0f);
            }

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
                {
                    //HostScreen.Show<PlaySongSelectScreen>(true);
                    OnReachedEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            // Draw background before all the screen components
            _spriteBatch.Draw(Beatmap.BackgroundTexture, new Rectangle(0, 0, Settings.WindowWidth, Settings.WindowHeight), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin();
            // Draw screen components in the order
            DrawPlayfield();
            _spriteBatch.End();
            _spriteBatch.Begin();
            DrawBeatDivisors();
            _spriteBatch.End();
            _spriteBatch.Begin();
            DrawHitObjects();
            _spriteBatch.End();

            _spriteBatch.Begin();
            // Draw UI
            DrawUi();
            _spriteBatch.End();

            //ScoreGraph.Draw(gameTime);


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
                    new Vector2(Skin.Settings.PlayfieldPositionX,
                    Settings.WindowHeight - Skin.ButtonTexture.Height),
            Color.White);


            // Initialize health bar settings
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
            var srcRect = new Rectangle(0, 0, (int)hpW, (int)MathHelperExtensions.InBetween(Skin.HealthBar.Height, curVal, minVal, maxVal));

            // Interpolate color from Red (min health) to Green (max health)
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
            _spriteBatch.DrawString(Skin.Font, ScrollingSpeed.ToString("F1"), new Vector2(12, 48), Color.Red);

            _spriteBatch.DrawString(Skin.Font,
                $"Song Speed: {FindSystem<MusicSystem>().PlaybackRate:F1}",
                new Vector2(12, 64),
                Color.DarkRed);

            var scoreSystem = GameSystemManager.FindSystem<ScoreV1System>();

            string scores = $"Marvelous: {scoreSystem.MarvelousCount}\n" +
                            $"Perfect: {scoreSystem.PerfectCount}\n" +
                            $"Great: {scoreSystem.GreatCount}\n" +
                            $"Good: {scoreSystem.GoodCount}\n" +
                            $"Bad: {scoreSystem.BadCount}\n" +
                            $"Miss: {scoreSystem.MissCount}";
            _spriteBatch.DrawString(Skin.Font, scores, new Vector2(800, 10), Color.Black);
            _spriteBatch.DrawString(Skin.Font,
                $"ScoremeterScore: {scoreSystem.Score}\n" +
                $"Combo: {scoreSystem.Combo}\n" +
                $"Accuracy: {scoreSystem.Accuracy * 100:F2}%\n" +
                $"HP: {FindSystem<HealthSystem>().Health}",
                new Vector2(15, 300), Color.Black);
        }

        private void DrawHitObjects()
        {
            var time = (long)FindSystem<GameTimeSystem>().Time;

            //foreach (var lines in SeparatedLines)
            {
                foreach (var o in Beatmap.HitObjects)
                {
                    // TODO: Make more precise work on the numbers
                    // TODO: Make _isUpsideDown (upside down) work
                    // TODO: Skip unnecessary loops
                    //int k = IsUpsideDown ? -1 : 1;

                    // Set correct position for the object
                    Vector2 position = new Vector2(
                        // x
                        Skin.Settings.PlayfieldPositionX +
                        ((o.Column - 1) * (Skin.PlayfieldLineTexture.Width)),
                        // y
                        (ScrollingSpeed * (time - o.Position) +
                         (Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                        Skin.Settings.HitPosition
                    );

                    Texture2D hitObjectTexture = Skin.NoteClickTexture;

                    if (Skin.Settings.NoteType.ToLower() == "arrow")
                    {
                        // For 'arrow' skin type use texture collection
                        hitObjectTexture = Skin.NoteClickTextures[o.Column - 1];
                    }

                    // if it is a 'Click' note, just draw its texture
                    if (o is NoteClick)
                    {
                        _spriteBatch.Draw(hitObjectTexture, position, o.IsExpired ? Color.Black : Color.White);
                    }
                    else if (o is NoteHold) // if it is a 'Hold' note, draw its texture one time per 5 pixels
                    {
                        var noteHold = o as NoteHold;
                        for (int i = 0; i <= noteHold.EndPosition - noteHold.Position; i += 5)
                        {
                            position = new Vector2(
                                // x
                                Skin.Settings.PlayfieldPositionX +
                                ((noteHold.Column - 1) * (Skin.PlayfieldLineTexture.Width)),
                                // y
                                (ScrollingSpeed * (time - (noteHold.Position + i)) +
                                 (Settings.WindowHeight - Skin.ButtonTexture.Height)) +
                                Skin.Settings.HitPosition
                            );

                            _spriteBatch.Draw(hitObjectTexture, position, null, noteHold.IsExpired ? Color.Black : Color.White);
                        }
                    }

                    // Proceed miss
                    // It handles in draw method because of optimization
                    if (o.IsExpired) continue;

                    var scoreSys = GameSystemManager.FindSystem<ScoreV1System>();

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
            var tp = Beatmap.TimingPoints[0];

            for (int i = tp.Position; i <= Beatmap.HitObjects.Last().Position; i += (int)Math.Floor(tp.MsPerBeat * 4))
            {
                float posY = (ScrollingSpeed * ((long)FindSystem<GameTimeSystem>().Time - i) +
                              (Settings.WindowHeight - Skin.ButtonTexture.Height) + Skin.NoteClickTexture.Height +
                              Skin.Settings.HitPosition);
                Primitives2D.DrawLine(_spriteBatch, new Vector2(Skin.Settings.PlayfieldPositionX, posY),
                    new Vector2(Skin.Settings.PlayfieldPositionX + Skin.PlayfieldLineTexture.Width * Beatmap.Settings.Difficulty.KeyAmount, posY), Color.Gray,
                    3.0f);
            }
        }

        /// <summary>
        /// Load beatmap from a file with background texture and music file into memory
        /// </summary>
        /// <param name="beatmapName">Name of the beatmap</param>
        /// <returns>Beatmap class</returns>
        /*private Beatmap LoadBeatmap(string beatmapName)
        {
            // TODO: Remove this
            Beatmap beatmap;

            // Load Beatmap
            BeatmapReader beatmapReader = new BeatmapReader(_beatmapProcessor.ProcessorSettings);

            try
            {
                beatmap = beatmapReader.ReadBeatmap(_game.GraphicsDevice,
                    beatmapName, "test");
            }
            catch (Exception e)
            {
                LogHelper.Log($"Error while reading beatmap: {e.Message}");
                // TODO: If there was an error while loading map, return to the song select menu and pop up a notification to user
                throw new Exception(e.Message);
            }

            return beatmap;
        }*/

        /// <summary>
        /// Add event handlers to all the hit objects from all the game systems if it should process hit objects
        /// </summary>
        private void InitializeAllHitObjects()
        {
            foreach (var hitObject in Beatmap.HitObjects)
                foreach (var system in GameSystemManager.GetAllGameSystems())
                    if (system is IGameSystemProcessHitObject)
                        hitObject.OnHit += (system as IGameSystemProcessHitObject).OnHitObjectHit;
        }
    }
}
