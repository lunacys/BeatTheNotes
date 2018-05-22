using System;
using System.Threading.Tasks;

namespace BeatTheNotes.Framework.Debug.Logging
{
    /// <summary>
    /// Provides a logging system which might work on either sync or async.
    /// Implements an event queue.
    /// Do not forget to call "Update()" method.
    /// </summary>
    public static class LogHelper
    {
        private static Logger _logger;

        public static LogTarget Target
        {
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

        public static bool UsingAsync { get; set; }

        private static LogTarget _target;

        private static int _head;
        private static int _tail;

        private static readonly int MaxPending = 16;

        private static int _numPending;

        private static LogEntry[] _pending = new LogEntry[MaxPending];

        static LogHelper()
        {
            Target = LogTarget.File;
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            if ((_tail + 1) % MaxPending == _head) return;

            _pending[_tail] = new LogEntry(message, level);
            _tail = (_tail + 1) % MaxPending;
        }

        /*public static async Task LogAsync(string message, LogLevel level = LogLevel.Info)
        {
            //await _logger.LogAsync(AppendLogLevel(message, level));
        }*/

        public static void Update()
        {
            if (_head == _tail) return;

            string msg = $"{AppendLogLevel(_pending[_head].Level)}:\t{_pending[_head].Message}";

            if (UsingAsync)
            {
                Task.Run(() =>
                {
                    _logger.LogAsync(msg);
                });
            }
            else
            {
                _logger.Log(msg);
            }

            _head = (_head + 1) % MaxPending;
        }

        private static string AppendLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "[INFO]";
                case LogLevel.Warning:
                    return "[WARNING]";
                case LogLevel.Error:
                    return "[ERROR]";
                case LogLevel.Critical:
                    return "[CRITICAL]";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}
