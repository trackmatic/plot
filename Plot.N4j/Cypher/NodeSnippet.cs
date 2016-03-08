using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plot.Metadata;

namespace Plot.N4j.Cypher
{
    public class NodeSnippet
    {
        private readonly ParamSnippet _name;

        private readonly string _label;

        public NodeSnippet(NodeMetadata metadata, object data) 
            : this(metadata, data, new List<string>())
        {

        }

        public NodeSnippet(NodeMetadata metadata, object data, IEnumerable<string> segments)
            : this(new ParamSnippet(metadata, data, segments.ToArray()), metadata.Name, data)
        {
            Data = data;
            Metadata = metadata;
        }

        public NodeSnippet(ParamSnippet name, string label, object data)
        {
            _name = name;
            _label = label;
            Data = data;
        }

        public override string ToString()
        {
            return new StringBuilder().Append(_name).Append(":").Append(_label).ToString();
        }

        public ParamSnippet Param => _name;

        public string Label => _label;

        public object Data { get; }

        public NodeMetadata Metadata { get; }
    }
}
