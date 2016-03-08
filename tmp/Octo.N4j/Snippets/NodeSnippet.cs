using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octo.Core;

namespace Octo.N4j.Snippets
{
    public class NodeSnippet
    {
        private readonly ParamSnippet _name;

        private readonly string _label;

        public NodeSnippet(ParamSnippet name, string label)
        {
            _name = name;

            _label = label;
        }

        public override string ToString()
        {
            return new StringBuilder().Append(_name).Append(":").Append(_label).ToString();
        }

        public ParamSnippet Param
        {
            get { return _name; }
        }

        public string Label
        {
            get { return _label; }
        }
    }

    public class NodeSnippet<T> : NodeSnippet
    {
        private readonly T _data;

        public NodeSnippet(T model) : this(model, new List<string>())
        {
            
        }

        public NodeSnippet(T model, IEnumerable<string> segments)
            : base(new ParamSnippet<T>(model, segments.ToArray()), model.GetType().Name)
        {
            _data = model;
        }

        public T Data
        {
            get { return _data; }
        }
    }
}
