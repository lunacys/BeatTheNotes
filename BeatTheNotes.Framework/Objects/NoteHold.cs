namespace BeatTheNotes.Framework.Objects
{
    public class NoteHold : HitObject
    {
        /// <summary>
        /// End note position in Ms, cannot be lower than <see cref="HitObject.Position"/>
        /// </summary>
        public int EndPosition { get; private set; }

        public NoteHold(int column, int position, int endPosition)
            : base(column, position)
        {
            if (endPosition < position)
                endPosition = position;

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