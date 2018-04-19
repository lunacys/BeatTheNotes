using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace BeatTheNotes.Framework.GameSystems
{
    public abstract class GameSystem : IDisposable
    {
        public event EventHandler OnReset;

        protected GameSystem()
        { }

        public IGameSystemManager GameSystemManager { get; internal set; }
        public bool IsWorking { get; set; }

        public T FindSystem<T>() where T : GameSystem
        {
            return GameSystemManager?.FindSystem<T>();
        }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Reset()
        {
            OnReset?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Dispose() { }
    }
}
