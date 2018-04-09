using System;
using BeatTheNotes.Framework.Beatmaps;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework.GameSystems
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
        
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Reset() { }

        public virtual void Dispose() { }
    }
}
