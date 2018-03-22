using System;
using System.Collections.Generic;
using System.Text;
using Jackhammer.Audio;

namespace Jackhammer.GameSystems
{
    public class MusicSystem : GameSystem
    {
        public Music Music { get; set; }

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
