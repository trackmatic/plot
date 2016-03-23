using System;
using System.Collections.Generic;

namespace Plot.Tests.Utility
{
    public static class Utils
    {
        public static int GetHashCode(object value)
        {
            if (value == null)
            {
                return 0;
            }
            return value.GetHashCode();
        }

        public static bool Equals<T>(T entity, object obj) where T : class
        {
            var other = obj as T;
            if (other == null)
            {
                return false;
            }
            return entity.GetHashCode() == other.GetHashCode();
        }

        public static void Map<T>(this IEnumerable<T> items, Action<T> map)
        {
            foreach (var item in items)
            {
                map(item);
            }
        }

        public static void Add<T>(IList<T> list, T item, Action then = null)
        {
            if (list.Contains(item))
            {
                return;
            }
            list.Add(item);
            then?.Invoke();
        }
    }
}
