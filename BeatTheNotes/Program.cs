using System;

namespace BeatTheNotes
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new GameRoot())
                game.Run();
        }
    }
}
