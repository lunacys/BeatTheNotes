namespace BeatTheNotes.Framework.Debugging.Logging
{
    public class LogEntry
    {
        public string Message { get; }
        public LogLevel Level { get; }

        public LogEntry(string message, LogLevel level)
        {
            Message = message;
            Level = level;
        }
    }
}