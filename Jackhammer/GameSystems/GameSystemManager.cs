using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Jackhammer.GameSystems
{
    public class GameSystemManager : DrawableGameComponent, IGameSystemManager
    {
        private readonly List<GameSystem> _gameSystems;

        public GameSystemManager(Game game, IEnumerable<GameSystem> systems) : base(game)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public GameSystemManager(Game game) : base(game) 
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
