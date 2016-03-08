using System;
using System.Reflection;
using Castle.DynamicProxy;
using Plot.Metadata;
using System.Collections.Generic;

namespace Plot.Proxies
{
    public class ProxyInterceptorSelector : IInterceptorSelector
    {
        private readonly IMetadataFactory _metadataFactory;

        public ProxyInterceptorSelector(IMetadataFactory metadataFactory)
        {
            _metadataFactory = metadataFactory;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var results = new List<IInterceptor>();
            var metadata = _metadataFactory.Create(type);
            if (!IsSetter(method))
            {
                return null;
            }
            var propertyName = GetPropertyName(method);
            AppendRelationshipTracker(results, metadata, propertyName);
            AppendStateTracker(results, method);
            return results.ToArray();
        }

        private void AppendRelationshipTracker(IList<IInterceptor> interceptors, NodeMetadata metadata, string propertyName)
        {
            if (!metadata.Contains(propertyName))
            {
                return;
            }
            var property = metadata[propertyName];
            if (property.IsList)
            {
                return;
            }
            if (!property.HasRelationship)
            {
                return;
            }
            interceptors.Add(new RelationshipTrackerInterceptor(propertyName));
        }

        private void AppendStateTracker(IList<IInterceptor> interceptors, MethodInfo method)
        {
            if (!method.IsSpecialName)
            {
                return;
            }

            if (!method.Name.StartsWith("set_"))
            {
                return;
            }
            interceptors.Add(new EntityStateTrackerInterceptor());
        }

        private string GetPropertyName(MethodInfo method)
        {
            if (method.IsSpecialName)
            {
                return method.Name.Substring(4);
            }
            return method.Name;
        }

        private bool IsSetter(MethodInfo method)
        {
            return method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        }
    }
}
