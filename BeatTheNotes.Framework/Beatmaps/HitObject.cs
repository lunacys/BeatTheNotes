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

        public bool IsPressed { get; set; }

        public HitObject(int line, int position, int endPosition = 0)
        {
            Line = line;
            Position = position;
            if (endPosition == 0 || endPosition < position)
                EndPosition = position;
            else
                EndPosition = endPosition;

            IsPressed = false;
        }

        public override string ToString()
        {
            return $"{Line} {Position} {EndPosition}";
        }
    }
}
