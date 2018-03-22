using System;
using System.Threading.Tasks;

namespace Jackhammer
{
    public static class LogHelper
    {
        private static Logger _logger;

        public static LogTarget Target {
            get => _target;
            set
            {
                switch (value)
                {
                    case LogTarget.Console:
                        _logger = new ConsoleLogger();
                        break;
                    case LogTarget.File:
                        _logger = new FileLogger();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                _target = value;
            }
        }

        private static LogTarget _target;

        static LogHelper()
        {
            Target = LogTarget.File;
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            _logger.Log(AppendLogLevel(message, level));
        }

        public static async Task LogAsync(string message, LogLevel level = LogLevel.Info)
        {
            await _logger.LogAsync(AppendLogLevel(message, level));
        }

        private static string AppendLogLevel(string msg, LogLevel level)
        {
            var str = "";
            switch (level)
            {
                case LogLevel.Info: str += "[INFO]:\t"; break;
                case LogLevel.Warning: str += "[WARNING]:\t"; break;
                case LogLevel.Error: str += "[ERROR]:\t"; break;
                case LogLevel.Critical: str += "[CRITICAL]:\t"; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
            str += msg;
            return str;
        }
    }
}
