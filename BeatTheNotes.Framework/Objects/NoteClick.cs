namespace BeatTheNotes.Framework.Objects
{
    public class NoteClick : HitObject
    {
        public NoteClick(int column, int position) : base(column, position)
        { }

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