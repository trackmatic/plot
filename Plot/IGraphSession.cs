using System;
using System.Collections.Generic;
using Plot.Queries;

namespace Plot
{
    public interface IGraphSession : IDisposable
    {
        T Store<T>(T item);

        void Delete<T>(T item);

        IEnumerable<T> Get<T>(params string[] id);

        T Get<T>(IQuery<T> query);

        T Get<T>(string id);

        void Evict<T>(T item);

        IPagedGraphCollection<TResult> Query<TResult>(IQuery<TResult> query);
        
        bool Register(object model, EntityState state);

        void SaveChanges();

        void Register(IListener listener);

        IUnitOfWork Uow { get; }

        IEntityStateCache StateCache { get; }

        event EventHandler<ItemRegisteredEventArgs> ItemRegistered;

        event EventHandler<GraphSessionFlushedEventArgs> Flushed;

        event EventHandler<GraphSessionDisposedEventArgs> Disposed;
    }
}
