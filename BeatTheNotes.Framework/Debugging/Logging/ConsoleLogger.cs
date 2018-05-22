using System.Threading.Tasks;

namespace BeatTheNotes.Framework.Debug.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Log(string message)
        {
            lock (LockObject)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        public override async Task LogAsync(string message)
        {
            await Task.Run(() => System.Diagnostics.Debug.WriteLine(message));
        }
    }
}
