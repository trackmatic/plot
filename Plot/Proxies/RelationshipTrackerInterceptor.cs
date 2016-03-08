using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Plot.Metadata;

namespace Plot.Proxies
{
    public class RelationshipTrackerInterceptor : IInterceptor
    {
        private readonly IDictionary<string, State> _state;

        private readonly IMetadataFactory _metadataFactory;

        public RelationshipTrackerInterceptor(IMetadataFactory metadataFactory)
        {
            _state = new Dictionary<string, State>();
            _metadataFactory = metadataFactory;
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
            var state = GetState(property.Name);
            state.Push(invocation.Arguments[0]);
            invocation.Proceed();
        }

        public IEnumerable<ITrackableRelationship> GetTrackableRelationships()
        {
            return _state.Values;
        }

        private State GetState(string property)
        {
            if (!_state.ContainsKey(property))
            {
                _state.Add(property, new State());
            }
            return _state[property];
        }

        private bool IsSetter(IInvocation invocation)
        {
            return invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase) && EntityStateTracker.Contains(invocation.InvocationTarget);
        }

        public class State : ITrackableRelationship
        {
            private readonly List<object> _items;

            private object _current;

            public State()
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
