using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Plot.Exceptions;

namespace Plot
{
    internal static class EntityIdUtils
    {
        private static readonly object Sync = new object();

        private static readonly IDictionary<Type, PropertyInfo> Properties = new ConcurrentDictionary<Type, PropertyInfo>();

        public static PropertyInfo GetId(object source)
        {
            lock (Sync)
            {
                return Properties.ContainsKey(source.GetType()) ? Get(source) : Create(source);
            }
        }

        private static PropertyInfo Get(object other)
        {
            return Properties[other.GetType()];
        }

        private static PropertyInfo Create(object other)
        {
            var type = other.GetType();
            var property = type.GetProperty(Conventions.IdPropertyName);
            if (property == null)
            {
                throw new MissingRequiredPropertyException(other.GetType());
            }
            Properties.Add(type, property);
            return property;
        }
    }
}
