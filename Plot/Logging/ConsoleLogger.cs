using System;

namespace Plot.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Log(ConsoleColor.Green, message);
        }

        public void Warn(string message)
        {
            Log(ConsoleColor.Yellow, message);
        }

        public void Error(string message, Exception e = null)
        {
            Log(ConsoleColor.Red, message);
        }

        private void Log(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now}] {message}");
        }
    }
}
