using System;

namespace BeatTheNotes.Framework.Objects
{
    public abstract class HitObject
    {
        /// <summary>
        /// On which column this hit object must be placed
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Start note position in Ms
        /// </summary>
        public int Position { get; }

        public event EventHandler<HitObjectOnHitEventArgs> OnHit;

        public bool IsExpired => _isExpired;

        private bool _isExpired;

        public HitObject(int column, int position)
        {
            Column = column;
            Position = position;
        }

        public virtual void Reset()
        {
            _isExpired = false;
        }

        public virtual void DoHit()
        {
            _isExpired = true;
            OnHit?.Invoke(this, new HitObjectOnHitEventArgs(this));
        }
    }
}
