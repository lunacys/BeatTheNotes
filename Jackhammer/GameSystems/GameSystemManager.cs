using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public class GameSystemManager : IGameSystemManager
    {
        private readonly List<GameSystem> _gameSystems;

        public GameSystemManager(IEnumerable<GameSystem> systems)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public GameSystemManager() 
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

        public void Update(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
            {
                gameSystem.Update(gameTime);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
            {
                gameSystem.Draw(spriteBatch);
            }
        }
    }
}
