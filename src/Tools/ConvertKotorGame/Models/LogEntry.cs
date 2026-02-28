using Avalonia.Media;

namespace ConvertKotorGame.Models
{
    public enum LogLevelKind
    {
        Trace,
        Info,
        Warning,
        Error,
    }

    public sealed class LogEntry
    {
        public LogEntry(string message, LogLevelKind level)
        {
            Message = message;
            Level = level;
            Foreground = ResolveForeground(level);
        }

        public string Message { get; private set; }
        public LogLevelKind Level { get; private set; }
        public IBrush Foreground { get; private set; }

        private static IBrush ResolveForeground(LogLevelKind level)
        {
            switch (level)
            {
                case LogLevelKind.Trace:
                    return new SolidColorBrush(Color.Parse("#6495ED"));
                case LogLevelKind.Warning:
                    return new SolidColorBrush(Color.Parse("#CC4E00"));
                case LogLevelKind.Error:
                    return new SolidColorBrush(Color.Parse("#DC143C"));
                default:
                    return Brushes.Black;
            }
        }
    }
}
