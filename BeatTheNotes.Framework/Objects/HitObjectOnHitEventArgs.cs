using System;

namespace BeatTheNotes.Framework.Objects
{
    public class HitObjectOnHitEventArgs : EventArgs
    {
        public HitObject HitObject { get; }

        public HitObjectOnHitEventArgs(HitObject hitObject)
        {
            HitObject = hitObject;
        }
    }
}
