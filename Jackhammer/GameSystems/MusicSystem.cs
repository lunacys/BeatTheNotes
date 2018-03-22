using System;
using System.Collections.Generic;
using System.Text;

namespace Jackhammer.GameSystems
{
    public class MusicSystem : GameSystem
    {
        public MusicSystem()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
            
            GC.SuppressFinalize(this);
        }
    }
}
