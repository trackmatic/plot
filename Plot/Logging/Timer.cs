using System;

namespace Plot.Logging
{
    public class Timer : IDisposable
    {
        private readonly ILogger _logger;

        private readonly DateTime _start = DateTime.Now;

        private readonly string _name;

        public Timer(string name, ILogger logger)
        {
            _name = name;
            _logger = logger;
        }

        public void Dispose()
        {
            var duration = DateTime.Now.Subtract(_start);
            _logger?.Info($"Executed {_name} in {duration}");
        }

        public static Timer Start(string name, ILogger logger)
        {
            return new Timer(name, logger);
        }
    }
}
