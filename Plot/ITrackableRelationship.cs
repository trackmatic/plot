using System;
using System.Collections.Generic;

namespace Plot
{
    public interface ITrackableRelationship<T>
    {
        IEnumerable<T> Flush();
        
        T Get();

        void Get(Action<T> item, bool returnNull = false);

        void Populate(Action<T> item, bool returnNull = false);

        void Set(T data);

        bool HasValue();

        string Id { get; }

        Type Type { get; }
    }
}
