using System.Reflection;

namespace Plot.Metadata
{
    public class PropertyMetadata
    {
        private readonly PropertyInfo _property;

        public PropertyMetadata(PropertyInfo property)
        {
            _property = property;
        }

        public string Name { get; set; }

        public RelationshipMetadata Relationship { get; set; }

        public bool IsList { get; set; }

        public bool IsPrimitive { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsIgnored { get; set; }

        public bool HasRelationship => Relationship != null;

        public bool IsReverse()
        {
            if (Relationship == null)
            {
                return false;
            }

            return Relationship.IsReverse;
        }

        public T GetValue<T>(object instance)
        {
            return (T)_property.GetValue(instance);
        }
    }
}
