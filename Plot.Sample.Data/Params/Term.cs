using System;

namespace Plot.Sample.Data.Params
{
    public class Term : Parameter
    {
        public Term(Func<string> value) : base(Name, () => $"(?i){value()}.*", () => value().NotNullOrEmpty())
        {
        }

        private const string Name = "term";

        public static string Key = $"{{{Name}}}";
    }
}
