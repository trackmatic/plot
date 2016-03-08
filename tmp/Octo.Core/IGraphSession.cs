using System;
using System.Collections.Generic;
using Octo.Core.Queries;

namespace Octo.Core
{
    public interface IGraphSession : IDisposable
    {
        void Store<T>(T item);

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
        
        event EventHandler<ItemRegisteredEventArgs> ItemRegistered;
    }
}
