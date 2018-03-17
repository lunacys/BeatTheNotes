using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jackhammer.GameSystems
{
    public abstract class GameSystem : IDisposable
    {
        protected GameSystem() { }

        public IGameSystemManager GameSystemManager { get; internal set; }
        public bool IsWorking { get; set; }
        
        public T FindSystem<T>() where T : GameSystem
        {
            return GameSystemManager?.FindSystem<T>();
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Reset() { }

        public virtual void Dispose() { }
    }
}
