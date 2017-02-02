using System;

namespace Plot
{
    public static class Conventions
    {
        public static string IdPropertyName = "Id";
        public static Func<object, string> ConvertIdToString = id => id.ToString();
    }
}
