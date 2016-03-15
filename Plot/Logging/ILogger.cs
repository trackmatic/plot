using System;

namespace Plot.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Warn(string message);

        void Error(string message, Exception e = null);
    }
}
