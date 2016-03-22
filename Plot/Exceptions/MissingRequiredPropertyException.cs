using System;

namespace Plot.Exceptions
{
    public class MissingRequiredPropertyException : Exception
    {
        public MissingRequiredPropertyException(Type type) : base(string.Format(Text.MissingIdProperty, Conventions.IdPropertyName, type.FullName))
        {
            
        }
    }
}
