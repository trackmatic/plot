using System;
using System.Collections.Generic;
using System.Linq;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class ParamSnippet
    {
        private readonly List<string> _segments;
        public ParamSnippet(NodeMetadata metadata, object item, params string[] segments) : this(metadata, item)
        {
            foreach (var segment in segments)
            {
                Add(segment);
            }
        }

        public ParamSnippet(NodeMetadata metadata, object item)
            : this(metadata.Name, ProxyUtils.GetEntityId(item))
        {

        }

        public ParamSnippet(ParamSnippet parent, params string[] segments)
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
        
        public ParamSnippet(params string[] segments)
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
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":","")
                .Replace("#","")
                .Replace("(","")
                .Replace("?","")
                .Replace(")","")
                .Replace("'", "")
                .Replace("\"","")
                .Replace("=","")));
        }

        protected void Add(string segment)
        {
            _segments.Add(segment);
        }
    }
}
