using System;
using System.Collections.Generic;

namespace Octo.Core
{
    internal static class EntityStateTracker
    {
        [ThreadStatic] private static readonly IDictionary<object, EntityState> State;

        static EntityStateTracker()
        {
            State = new Dictionary<object, EntityState>();
        }

        public static EntityState Create(object proxy)
        {
            var item = EntityUtils.GetTargetEntity(proxy);
            var state = new EntityState(EntityUtils.GetEntityId(proxy));
            State.Add(item, state);
            return state;
        }

        public static EntityState Get(object proxy)
        {
            var item = EntityUtils.GetTargetEntity(proxy);
            return State[item];
        }

        public static bool Contains(object proxy)
        {
            var item = EntityUtils.GetTargetEntity(proxy);
            return State.ContainsKey(item);
        }

        public static void Remove(object proxy)
        {
            var item = EntityUtils.GetTargetEntity(proxy);
            State.Remove(item);
        }
    }
}