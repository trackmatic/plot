using System.Collections.Generic;

namespace Plot.Metadata
{
    public class NodeMetadata
    {
        private readonly IDictionary<string, PropertyMetadata> _properties;

        public NodeMetadata()
        {
            _properties = new Dictionary<string, PropertyMetadata>();
        }

        public string Name { get; set; }

        public IEnumerable<PropertyMetadata> Properties => _properties.Values;

        public PropertyMetadata this[string name] => _properties[name];

        public void Add(string name, PropertyMetadata property)
        {
            _properties.Add(name, property);
        }

        public bool Contains(string property)
        {
            return _properties.ContainsKey(property);
        }

        public void Set(IEnumerable<PropertyMetadata> properties)
        {
            foreach (var property in properties)
            {
                Add(property.Name, property);
            }
        }
    }
}
