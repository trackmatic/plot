using System;
using System.Linq;

namespace Plot.Neo4j
{
    public static class Conventions
    {
        public static Func<string, string> NamedParameterCase = CamelCase;

        public static string None(string value)
        {
            return value;
        }

        public static string UpperCase(string value)
        {
            return value.ToUpper();
        }

        public static string CamelCase(string value)
        {
            if (char.IsUpper(value[0]))
            {
                var start = new[] { char.ToLower(value[0]) };
                var remainder = value.Skip(1).Take(value.Length).ToArray();
                return new string(start.Concat(remainder).ToArray());
            }
            return value;
        }
    }
}
