using System.Runtime.CompilerServices;

namespace Habit_Tracker
{
    internal class Logger
    {
        private static readonly string _path = "log.txt";

        public static void Log(string message,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = "")
        {
            string fileName = Path.GetFileName(filePath);
            File.AppendAllText(_path, $"{DateTime.Now} [{fileName}:{lineNumber} {memberName}()]: {message}{Environment.NewLine}");
        }
    }
}