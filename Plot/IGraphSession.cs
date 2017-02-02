using System;
using System.Collections.Generic;
using Plot.Proxies;
using Plot.Queries;

namespace Plot
{
    public interface IGraphSession : IDisposable
    {
        T Create<T>(T item);

        void Delete<T>(T item);

        IEnumerable<T> Get<T>(params object[] id);

        T Get<T>(IQuery<T> query, bool enlist = true);

        T Get<T>(object id);

        void Evict<T>(T item);

        IPagedGraphCollection<TResult> Query<TResult>(IQuery<TResult> query, bool enlist = false);

        bool Register(object model);

        bool Register(object model, EntityState state);

        void SaveChanges();

        void Register(IListener listener);

        IUnitOfWork Uow { get; }

        IEntityStateCache State { get; }

        IProxyFactory ProxyFactory { get; }

        event EventHandler<ItemRegisteredEventArgs> ItemRegistered;

        event EventHandler<GraphSessionFlushedEventArgs> Flushed;

        event EventHandler<GraphSessionDisposedEventArgs> Disposed;
    }
}
