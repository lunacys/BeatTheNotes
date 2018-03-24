namespace BeatTheNotes.Framework.Beatmaps
{
    public class GameMode
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        public GameMode(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
