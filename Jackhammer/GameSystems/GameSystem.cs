using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jackhammer.GameSystems
{
    public abstract class GameSystem
    {
        public abstract void Update(GameTime gameTime);
    }
}
