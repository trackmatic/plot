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
            var metadata = _metadataFactory.Create(invocation.TargetType);
            var property = metadata[invocation.Method.Name.Substring(4)];
            if (property.IsList || !property.HasRelationship)
            {
                invocation.Proceed();
                return;
            }
            var state = GetState(property);
            state.Push(invocation.Arguments[0]);
            RegisterDependencies(property.Relationship, invocation.InvocationTarget, invocation.Arguments[0]);
            invocation.Proceed();
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


        public ITrackableRelationship GetTrackableRelationship(RelationshipMetadata relationship)
        {
            return _relationshipState[relationship];
        }

        private RelationshipState GetState(PropertyMetadata property)
        {
            if (!_relationshipState.ContainsKey(property.Relationship))
            {
                _relationshipState.Add(property.Relationship, new RelationshipState());
            }
            return _relationshipState[property.Relationship];
        }

        private bool IsSetter(IInvocation invocation)
        {
            return invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && _state.Contains(invocation.InvocationTarget);
        }

        private class RelationshipState : ITrackableRelationship
        {
            private readonly List<object> _items;

            private object _current;

            public RelationshipState()
            {
                _items = new List<object>();
            }

            public IEnumerable Flush()
            {
                var items = _items.ToList();
                _items.Clear();
                return items;
            }
            public void Push(object item)
            {
                if (_current == null)
                {
                    _current = item;
                    return;
                }
                _items.Add(_current);
                _current = item;
            }

            public object Current => _current;
        }
    }
}
