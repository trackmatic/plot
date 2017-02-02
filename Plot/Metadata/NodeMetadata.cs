using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Plot.Metadata
{
    public class NodeMetadata
    {
        private readonly ConcurrentDictionary<string, PropertyMetadata> _properties;
        private readonly string _name;
        private readonly bool _isIgnored;

        public NodeMetadata(string name, bool isIgnored)
        {
            _name = name;
            _isIgnored = isIgnored;
            _properties = new ConcurrentDictionary<string, PropertyMetadata>();
        }

        public string Name => _name;

        public IEnumerable<PropertyMetadata> Properties => _properties.Values;

        public PropertyMetadata this[string name] => _properties[name];

        public bool IsIgnored => _isIgnored;

        public void SetProperties(IEnumerable<PropertyMetadata> properties)
        {
            foreach (var property in properties)
            {
                _properties.GetOrAdd(property.Name, property);
            }
        }
        
        public bool Contains(string property)
        {
            return _properties.ContainsKey(property);
        }
    }
}
