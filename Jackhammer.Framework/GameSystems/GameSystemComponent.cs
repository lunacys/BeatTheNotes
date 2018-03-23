using System;
using System.Collections.Generic;
using System.Linq;
using Jackhammer.Framework.Logging;
using Microsoft.Xna.Framework;

namespace Jackhammer.Framework.GameSystems
{
    public class GameSystemComponent : DrawableGameComponent, IGameSystemManager
    {
        private readonly List<GameSystem> _gameSystems;

        public event EventHandler<SystemAddedEventArgs> SystemAdded;

        public GameSystemComponent(Game game, IEnumerable<GameSystem> systems) : this(game)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public GameSystemComponent(Game game) : base(game) 
        {
            _gameSystems = new List<GameSystem>();
        }

        public T FindSystem<T>() where T : GameSystem
        {
            var system = _gameSystems.OfType<T>().FirstOrDefault();

            if (system == null)
                throw new InvalidOperationException($"{typeof(T).Name} not registered");

            return system;
        }

        public T Register<T>(T system) where T : GameSystem
        {
            LogHelper.Log($"GameSystemManager: Registering System {typeof(T)}");
            SystemAdded?.Invoke(this, new SystemAddedEventArgs(this));

            system.GameSystemManager = this;
            system.IsWorking = true;
            _gameSystems.Add(system);

            return system;
        }

        public void Reset()
        {
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Reset();
            }
        }

        public override void Initialize()
        {
            foreach (var gameSystem in _gameSystems)
            {
                LogHelper.Log($"GameSystemManager: Initializing System {gameSystem}");
                gameSystem.Initialize();
                LogHelper.Log($"GameSystemManager: End Initializing System {gameSystem}");
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
            {
                gameSystem.Update(gameTime);
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
            {
                gameSystem.Draw(gameTime);
            }
        }
    }
}
