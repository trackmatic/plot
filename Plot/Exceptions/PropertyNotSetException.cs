using System;

namespace Plot.Exceptions
{
    public class PropertyNotSetException : Exception
    {
        public PropertyNotSetException(string message, object source) : base(string.Format(message, source))
        {
            
        }
    }
}
