using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Plot.Metadata
{
    public class NodeMetadata
    {
        private readonly IDictionary<string, PropertyMetadata> _properties;
        private readonly string _name;

        public NodeMetadata(string name, IEnumerable<PropertyMetadata> properties)
        {
            _name = name;
            _properties = new ConcurrentDictionary<string, PropertyMetadata>();
            foreach (var property in properties)
            {
                _properties.Add(name, property);
            }
        }

        public string Name => _name;

        public IEnumerable<PropertyMetadata> Properties => _properties.Values;

        public PropertyMetadata this[string name] => _properties[name];
        
        public bool Contains(string property)
        {
            return _properties.ContainsKey(property);
        }
    }
}
