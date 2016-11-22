using System;
using System.Collections.Generic;
using System.Linq;

namespace Plot.Neo4j.Cypher
{
    public class IdentifierNameSnippet
    {
        private readonly List<string> _segments;
        
        public IdentifierNameSnippet(IdentifierNameSnippet parent, params string[] segments)
        {
            _segments = new List<string>();

            foreach (var segment in parent._segments)
            {
                _segments.Add(segment);
            }

            if (segments.Any(string.IsNullOrEmpty))
            {
                throw new FormatException("Segments cannot be null or empty");
            }

            foreach (var segment in segments)
            {
                _segments.Add(segment);
            }
        }
        
        public IdentifierNameSnippet(params string[] segments)
        {
            if (segments.Any(string.IsNullOrEmpty))
            {
                throw new FormatException("Segments cannot be null or empty");
            }

            _segments = segments.ToList();
        }
        
        public override string ToString()
        {
            return string.Join("_", _segments
                .Select(x => x
                .Replace("/", "_")
                .Replace("-", "_")
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Replace(":", string.Empty)
                .Replace("#", string.Empty)
                .Replace("(", string.Empty)
                .Replace("?", string.Empty)
                .Replace(")", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("=", string.Empty)));
        }

        protected void Add(string segment)
        {
            _segments.Add(segment);
        }
    }
}
