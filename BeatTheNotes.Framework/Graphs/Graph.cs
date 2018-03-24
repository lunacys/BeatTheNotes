using System;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework.Graphs
{
    public abstract class Graph : IDisposable
    {
        protected Graph() { }

        public virtual void Dispose() { }

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void Draw(GameTime gameTime)
        { }
    }
}
