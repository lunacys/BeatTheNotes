namespace BeatTheNotes.Framework.Beatmaps
{
    public class NoteClick : Note
    {
        /// <summary>
        /// Start note position in Ms
        /// </summary>
        public int Position { get; private set; }

        public NoteClick(int column, int position) : base(column)
        {
            Position = position;
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