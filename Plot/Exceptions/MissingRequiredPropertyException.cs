using System;

namespace Plot.Exceptions
{
    public class MissingRequiredPropertyException : Exception
    {
        public MissingRequiredPropertyException() : base(string.Format(Text.MissingIdProperty, Conventions.IdPropertyName))
        {
            
        }
    }
}
