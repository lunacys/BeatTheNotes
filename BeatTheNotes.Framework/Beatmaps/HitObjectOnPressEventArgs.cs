using System;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class HitObjectOnPressEventArgs : EventArgs
    {
        public HitObject HitObject { get; }

        public HitObjectOnPressEventArgs(HitObject hitObject)
        {
            HitObject = hitObject;
        }
    }
}
