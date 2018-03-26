using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class HitObjectOnPressEventArgs : EventArgs
    {
        private HitObject HitObject { get; }

        public HitObjectOnPressEventArgs(HitObject hitObject)
        {
            HitObject = hitObject;
        }
    }
}
