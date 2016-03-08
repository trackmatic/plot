using System;
using System.Collections.Generic;

namespace Plot
{
    public interface IUnitOfWork : IDisposable
    {
        void Register(object item);

        void Merge(object item);

        IEnumerable<object> Items { get; }

        IEnumerable<object> Flush();

        bool Contains(object item);
        
        T Get<T>(string id);

        object Get(string id, Type type);

        void Remove(object item);
    }
}
