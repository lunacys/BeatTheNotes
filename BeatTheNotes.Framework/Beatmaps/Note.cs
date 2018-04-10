using System;

namespace BeatTheNotes.Framework.Beatmaps
{
    public abstract class Note : HitObject
    {
        public event EventHandler<HitObjectOnHitEventArgs> OnPress;

        public bool IsExpired => _isExpired;

        private bool _isExpired;

        public Note(int column) 
            : base(column)
        {
            _isExpired = false;
        }

        public virtual void Reset()
        {
            _isExpired = false;
        }

        public virtual void DoHit()
        {
            _isExpired = true;
            OnPress?.Invoke(this, new HitObjectOnHitEventArgs(this));
        }
    }
}