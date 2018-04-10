using System;

namespace BeatTheNotes.Framework.Beatmaps
{
    public abstract class HitObject
    {
        /// <summary>
        /// On which column this hit object must be placed
        /// </summary>
        public int Column { get; }

        public HitObject(int column)
        {
            Column = column;
        }
    }
}
