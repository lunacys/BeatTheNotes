using System.Threading.Tasks;

namespace Jackhammer.Logging
{
    public static class LogHelper
    {
        private static Logger _logger;

        public static LogTarget Target { get; set; } = LogTarget.File;

        public static void Log(string message)
        {
            switch (Target)
            {
                case LogTarget.Console:
                    _logger = new ConsoleLogger();
                    _logger.Log(message);
                    break;
                case LogTarget.File:
                    _logger = new FileLogger();
                    _logger.Log(message);
                    break;
            }
        }

        public static async void LogAsync(string message, LogLevel level = LogLevel.Info)
        {
            await Task.Run((async () =>
            {
                string str = "";
                switch (level)
                {
                    case LogLevel.Info: str += "[INFO]:\t"; break;
                    case LogLevel.Warning: str += "[WARNING]:\t"; break;
                    case LogLevel.Error: str += "[ERROR]:\t"; break;
                    case LogLevel.Critical: str += "[CRITICAL]:\t"; break;
                }
                str += message;

                switch (Target)
                {
                    case LogTarget.Console:
                        _logger = new ConsoleLogger();
                        await _logger.LogAsync(str);
                        break;

                    case LogTarget.File:
                        _logger = new FileLogger();
                        await _logger.LogAsync(str);
                        break;
                }
            }));
        }
    }
}
