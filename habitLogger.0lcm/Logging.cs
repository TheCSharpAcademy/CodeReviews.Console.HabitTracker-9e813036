using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace habitLogger._0lcm
{
    public class CustomFormatter : ConsoleFormatter
    {
        public CustomFormatter() : base("CustomFormatter") {}

        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider? scopeProvider,
            TextWriter textWriter
            )
        {
            string message = logEntry.Formatter(logEntry.State, logEntry.Exception);
            if (string.IsNullOrEmpty(message)) return;

            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = GetLogLevelColor(logEntry.LogLevel);

                textWriter.Write($"[{DateTimeOffset.Now:HH:mm:ss}]");

                textWriter.Write($"[{logEntry.LogLevel, -12}]");

                textWriter.Write($"[{logEntry.Category}]");

                textWriter.Write(message);

                if (logEntry.Exception != null)
                {
                    textWriter.Write(logEntry.Exception.ToString());
                }
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        private static ConsoleColor GetLogLevelColor(LogLevel loglevel) => loglevel switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Information => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.Magenta,
            _ => ConsoleColor.White,
        };
    }
    public class CustomFormatterOptions : ConsoleFormatterOptions {}

    internal class AppLogger
    {
        private static readonly ILoggerFactory AppLoggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddConsole(options =>
                    {
                        options.FormatterName = "CustomFormatter";
                    })
                    .AddConsoleFormatter<CustomFormatter, CustomFormatterOptions>();
            });
        public static ILogger<T> CreateLogger<T>() => AppLoggerFactory.CreateLogger<T>();
    }
}
