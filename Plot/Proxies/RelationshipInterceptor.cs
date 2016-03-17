using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Plot.Metadata;

namespace Plot.Proxies
{
    public class RelationshipInterceptor : IInterceptor
    {
        private readonly IDictionary<RelationshipMetadata, RelationshipState> _relationshipState;

        private readonly IMetadataFactory _metadataFactory;

        private readonly IEntityStateCache _state;

        public RelationshipInterceptor(IMetadataFactory metadataFactory, IEntityStateCache state)
        {
            _relationshipState = new Dictionary<RelationshipMetadata, RelationshipState>();
            _metadataFactory = metadataFactory;
            _state = state;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!IsSetter(invocation))
            {
                invocation.Proceed();
                return;
            }
            var item = invocation.Arguments[0];
            var metadata = _metadataFactory.Create(invocation.TargetType);
            var property = metadata[invocation.Method.Name.Substring(4)];
            if (property.IsList || !property.HasRelationship)
            {
                invocation.Proceed();
                return;
            }
            if (Contains(property))
            {
                var state = _state.Get(invocation.InvocationTarget);
                if (!state.IsLocked)
                {
                    Get(property).Push(item);
                }
            }
            else
            {
                Create(property, item);
            }
            RegisterDependencies(property.Relationship, invocation.InvocationTarget, invocation.Arguments[0]);
            invocation.Proceed();
        }

        public ITrackableRelationship GetTrackableRelationship(RelationshipMetadata relationship)
        {
            return _relationshipState[relationship];
        }

        private void RegisterDependencies(RelationshipMetadata relationship, object parentItem, object item)
        {
            if (item == null || !_state.Contains(item))
            {
                return;
            }
            var child = _state.Get(item);
            var parent = _state.Get(parentItem);
            if (relationship.IsReverse)
            {
                return;
            }
            child.Dependencies.Register(parent.Dependencies);
        }
        
        private void Create(PropertyMetadata property, object item)
        {
            var state = new RelationshipState(item);
            _relationshipState.Add(property.Relationship, state);
        }

        private RelationshipState Get(PropertyMetadata property)
        {
            return _relationshipState[property.Relationship];
        }

        private bool Contains(PropertyMetadata property)
        {
            return _relationshipState.ContainsKey(property.Relationship);
        }

        private bool IsSetter(IInvocation invocation)
        {
            return invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && _state.Contains(invocation.InvocationTarget);
        }

        private class RelationshipState : ITrackableRelationship
        {
            private readonly List<object> _items;

            private object _current;

            public RelationshipState(object current)
            {
                _items = new List<object>();
                _current = current;
            }

            public IEnumerable Flush()
            {
                var items = _items.ToList();
                _items.Clear();
                return items;
            }
            public void Push(object item)
            {
                if (_current != null)
                {
                    _items.Add(_current);
                }
                _current = item;
            }

            public object Current => _current;
        }
    }
}
