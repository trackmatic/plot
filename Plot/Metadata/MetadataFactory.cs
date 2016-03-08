using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plot.Attributes;

namespace Plot.Metadata
{
    public class MetadataFactory : IMetadataFactory
    {
        private readonly IDictionary<Type, NodeMetadata> _cache;

        public MetadataFactory()
        {
            _cache = new Dictionary<Type, NodeMetadata>();
        }

        public NodeMetadata Create(Type type)
        {
            return Load(type) ?? New(type);
        }

        public NodeMetadata Create(object instance)
        {
            return Create(ProxyUtils.GetTargetEntity(instance).GetType());
        }

        private NodeMetadata Load(Type type)
        {
            if (!_cache.ContainsKey(type))
            {
                return null;
            }
            return _cache[type];
        }

        private NodeMetadata New(Type type)
        {
            var properties = type.GetProperties().Select(CreateProperty).ToList();
            var node = new NodeMetadata(properties)
            {
                Name = type.Name
            };
            _cache.Add(type, node);
            return node;
        }

        private PropertyMetadata CreateProperty(PropertyInfo propertyInfo)
        {
            var property = new PropertyMetadata(propertyInfo)
            {
                Name = propertyInfo.Name,
                IsList = IsList(propertyInfo.PropertyType),
                IsPrimitive = IsPrimitive(propertyInfo.PropertyType),
                Relationship = CreateRelationship(propertyInfo),
                IsReadOnly = IsReadonly(propertyInfo)
            };
            return property;
        }

        private RelationshipMetadata CreateRelationship(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttributes<RelationshipAttribute>(true).FirstOrDefault();
            var relationship = attribute == null ? null : new RelationshipMetadata
            {
                IsReverse = attribute.Reverse,
                Name = attribute.Name
            };
            return relationship;

        }

        private static bool IsReadonly(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes<ReadonlyAttribute>(true).Any();
        }

        private static bool IsList(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
        }

        private static bool IsPrimitive(Type type)
        {
            return Primitives.Contains(type);
        }

        private static readonly Type[] Primitives =
        {
            typeof (int),
            typeof (decimal),
            typeof (string),
            typeof (DateTime),
            typeof (TimeSpan),
            typeof (double),
            typeof (uint),
            typeof (float),
            typeof (bool)
        };
    }
}
