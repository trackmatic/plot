using System.Collections.Generic;
using System.Reflection;

namespace Plot.Metadata
{
    public class NodeMetadata
    {
        private readonly IDictionary<string, PropertyMetadata> _properties;

        public NodeMetadata(IEnumerable<PropertyMetadata> properties)
        {
            _properties = new Dictionary<string, PropertyMetadata>();
            foreach (var property in properties)
            {
                Add(property.Name, property);
            }
        }

        public string Name { get; set; }

        public IEnumerable<PropertyMetadata> Properties => _properties.Values;

        public PropertyMetadata this[string name] => _properties[name];

        public void Add(string name, PropertyMetadata property)
        {
            _properties.Add(name, property);
        }
    }
}
