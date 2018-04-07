using System;

namespace BeatTheNotes.Framework.Beatmaps
{
    public class HitObject
    {
        /// <summary>
        /// Returns on which line this hit object must be placed
        /// </summary>
        public int Line { get; }
        /// <summary>
        /// Returns this object's position on the line in Ms
        /// </summary>
        public int Position { get; }
        public int EndPosition { get; }

        public event EventHandler<HitObjectOnPressEventArgs> OnPress;

        public bool IsPressed => _isPressed;

        private bool _isPressed;

        public bool IsLongNote => Position != EndPosition;

        public HitObject(int line, int position, int endPosition = 0)
        {
            Line = line;
            Position = position;
            if (endPosition == 0 || endPosition < position)
                EndPosition = position;
            else
                EndPosition = endPosition;

            _isPressed = false;
        }

        public void Reset()
        {
            _isPressed = false;
        }

        public void DoHit()
        {
            _isPressed = true;
            OnPress?.Invoke(this, new HitObjectOnPressEventArgs(this));
        }

        public override string ToString()
        {
            return $"{Line} {Position} {EndPosition}";
        }
    }
}
