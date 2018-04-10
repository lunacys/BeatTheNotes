namespace BeatTheNotes.Framework.Beatmaps
{
    public class NoteHold: Note
    {
        /// <summary>
        /// Start note position in Ms
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// End note position in Ms, cannot be lower than <see cref="Position"/>
        /// </summary>
        public int EndPosition { get; private set; }

        public NoteHold(int column, int position, int endPosition) : base(column)
        {
            Position = position;
            EndPosition = endPosition;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void DoHit()
        {
            base.DoHit();
        }
    }
}