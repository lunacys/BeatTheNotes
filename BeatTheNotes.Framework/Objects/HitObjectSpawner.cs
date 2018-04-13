namespace BeatTheNotes.Framework.Objects
{
    public class HitObjectSpawner
    {


        public HitObject Spawn()
        {
            return new NoteClick(1, 0);
        }
    }
}