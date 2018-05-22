
using System.Threading.Tasks;

namespace BeatTheNotes.Framework.Debugging.Logging
{
    public abstract class Logger
    {
        protected readonly object LockObject = new object();

        public abstract void Log(string message);
        public abstract Task LogAsync(string message);
    }
}
