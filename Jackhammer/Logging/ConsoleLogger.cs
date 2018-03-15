using System.Diagnostics;
using System.Threading.Tasks;

namespace Jackhammer.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Log(string message)
        {
            lock (LockObject)
            {
                Debug.WriteLine(message);
            }
        }

        public override async Task LogAsync(string message)
        {
            await Task.Run(() => Debug.WriteLine(message));
        }
    }
}
