using System;
using System.Linq;

namespace Plot.Sample
{
    [Serializable]
    public class Names
    {
        public string First { get; set; }

        public string Last { get; set; }

        public string Full => string.Join(" ", new[] {First, Last}.Where(x => !string.IsNullOrEmpty(x)));
    }
}
