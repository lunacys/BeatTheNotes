using BeatTheNotes.Framework.Beatmaps;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.Framework.Input;
using BeatTheNotes.Framework.Logging;
using BeatTheNotes.Framework.Settings;
using BeatTheNotes.GameSystems;
using BeatTheNotes.Shared.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.Screens
{
    public class GameplayScreen : Screen
    {
        private readonly GameRoot _game;

        public GameSystemComponent GameSystemComponent { get; }

        public Beatmap Beatmap
        {
            get => _beatmap;
            set
            {
                _beatmap = value;
                Initialize();
                GameSystemComponent.FindSystem<GameplaySystem>().Beatmap = value;
            }
        }
        private Beatmap _beatmap;

        private readonly InputHandler _input;

        private GameSettings Settings => _game.Services.GetService<GameSettings>();

        public GameplayScreen(GameRoot game)
        {
            _game = game;

            _input = new InputHandler(game);
            GameSystemComponent = new GameSystemComponent(game);
        }

        public override void Initialize()
        {
            LogHelper.Log("GameplayScreen: Initializing");

            base.Initialize();

            if (GameSystemComponent.GetAllGameSystems().Count == 0)
            {
                GameSystemComponent.Register(new GameplaySystem(_game, Beatmap));
                GameSystemComponent.Register(new MusicSystem());
                GameSystemComponent.Register(new ScoreV1System(_game.GraphicsDevice));
                GameSystemComponent.Register(new ScoremeterSystem(_game.GraphicsDevice));
                GameSystemComponent.Register(new GameTimeSystem());
                GameSystemComponent.Register(new HealthSystem(0.0f)); // TODO: Add health drain rate support

                GameSystemComponent.FindSystem<GameplaySystem>().OnReachedEnd += (sender, args) =>
                {
                    Restart();
                    Show<PlaySongSelectScreen>(true);
                };

                GameSystemComponent.Initialize();
            }
            else
            {
                Restart();
            }

            LogHelper.Log("GameplayScreen: Successfully Initialized");
        }

        public override void LoadContent()
        {
            LogHelper.Log("GameplayScreen: Loading Content");
            base.LoadContent();
            LogHelper.Log("GameplayScreen: Successfully Loaded Content");
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update(gameTime);

            if (_input.WasKeyPressed(Keys.Escape))
            {
                GameSystemComponent.FindSystem<MusicSystem>().Music.Stop();
                Show<PlaySongSelectScreen>(true);
                //Show<PauseScreen>(true);
            }

            if (_input.WasKeyPressed(Settings.GameKeys["BeatmapRestart"]))
            {
                Restart();
            }

            if (_input.WasKeyPressed(Settings.GameKeys["BeatmapMusicBPMDown"]))
            {
                Restart();
                GameSystemComponent.FindSystem<MusicSystem>().PlaybackRate -= 0.1f;
            }

            if (_input.WasKeyPressed(Settings.GameKeys["BeatmapMusicBPMUp"]))
            {
                Restart();
                GameSystemComponent.FindSystem<MusicSystem>().PlaybackRate += 0.1f;
            }

            /*if (InputManager.IsKeyDown(Keys.Space))
                Restart(23000);*/

            // Updating..

            GameSystemComponent.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameSystemComponent.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Restart the game. It means reset the time and the song
        /// </summary>
        private void Restart()
        {
            GameSystemComponent.Reset();
            // TODO: Rework this
            GameSystemComponent.FindSystem<MusicSystem>().Volume = 1.0f;
            GameSystemComponent.FindSystem<MusicSystem>().Music.Play();
        }
    }
}
