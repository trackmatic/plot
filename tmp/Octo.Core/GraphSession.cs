using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Octo.Core.Queries;

namespace Octo.Core
{
    public class GraphSession : IGraphSession
    {
        private readonly Dictionary<Type, IRepository> _repositories;

        private readonly IUnitOfWork _uow;

        private readonly List<IListener> _listeners;

        private readonly IQueryExecutorFactory _queryExecutorFactory;

        private readonly IRepositoryFactory _repositoryFactory;

        private bool _disposed;

        public GraphSession(IUnitOfWork uow, IEnumerable<IListener> listeners, IQueryExecutorFactory queryExecutorFactory, IRepositoryFactory repositoryFactory)
        {
            _repositories = new Dictionary<Type, IRepository>();
            _uow = uow;
            _listeners = listeners.ToList();
            _queryExecutorFactory = queryExecutorFactory;
            _repositoryFactory = repositoryFactory;
        }

        public void Store<T>(T item)
        {
            if (item == null)
            {
                throw new NullReferenceException("Object cannot be null");
            }
            var repository = GetRepository(item);
            repository.Store(item);
        }

        public void Delete<T>(T item)
        {
            if (item == null)
            {
                throw new NullReferenceException("Object cannot be null");
            }
            var repository = GetRepository(item);
            repository.Delete(item);
        }

        public IEnumerable<T> Get<T>(params string[] id)
        {
            var repository = GetRepositoryOfType<T>();
            return repository.Get(id);
        }

        public T Get<T>(string id)
        {
            var items = Get<T>(new[] {id});
            return items.FirstOrDefault();
        }

        public T Get<T>(IQuery<T> query)
        {
            var results = Query(query);

            if (results.Data.Count() > 1)
            {
                throw new InvalidOperationException("A query must return only 1 result to be used in the Map<T> method");
            }

            return GetRepositoryOfType<T>().Get(results.Data.Select(x => EntityStateTracker.Get(x).GetIdentifier()).ToArray()).FirstOrDefault();
        }

        public IPagedGraphCollection<TResult> Query<TResult>(IQuery<TResult> query)
        {
            return _queryExecutorFactory.Create(query, this).Execute(query);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _uow.Dispose();
            foreach (var repository in _repositories)
            {
                repository.Value?.Dispose();
            }
            _repositories.Clear();
            _listeners.Clear();
            OnDisposed();
            _disposed = true;
        }
        
        public IUnitOfWork Uow => _uow;
        
        public virtual bool Register(object item, EntityState state)
        {
            if (item == null)
            {
                return false;
            }
            if (_uow.Contains(item))
            {
                return false;
            }
            _uow.Register(item);
            state.Inject(this);
            ItemRegistered(this, new ItemRegisteredEventArgs(item));
            return true;
        }

        public void Evict<T>(T item)
        {
            _uow.Remove(item);
            EntityStateTracker.Remove(item);
        }

        public virtual void SaveChanges()
        {
            foreach (var item in _uow.Flush())
            {
                var repository = GetRepository(item);
                if (repository == null)
                {
                    throw new InvalidOperationException($"A repository for type {item} was not supplied");
                }
                var mapper = repository.Mapper;
                var aggregate = item;
                var state = EntityStateTracker.Get(item);
                if (state.Status == EntityStatus.Deleted)
                {
                    mapper.Delete(item, state);
                    Publish(item.GetType(), x => x.Delete(aggregate, this));
                }
                else if (state.Status == EntityStatus.Dirty)
                {
                    mapper.Update(item, state);
                    Publish(item.GetType(), x => x.Update(aggregate, this));
                }
                else if (state.Status == EntityStatus.New)
                {
                    mapper.Insert(item, state);
                    Publish(item.GetType(), x => x.Create(aggregate, this));
                }
                state.Clean();
            }
        }

        public void Register(IListener listener)
        {
            if (_listeners.Contains(listener))
            {
                return;
            }
            _listeners.Add(listener);
        }
        
        public event EventHandler<ItemRegisteredEventArgs> ItemRegistered = delegate { };

        private void Publish(Type type, Action<IListener> callback)
        {
            foreach (var listener in _listeners.Where(x => x.GetType().GetInterfaces().SelectMany(i => i.GetGenericArguments()).Any(a => a == type)))
            {
                callback(listener);
            }
        }

        protected virtual void OnDisposed()
        {
        }
        
        private IRepository<T> GetRepositoryOfType<T>()
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = _repositoryFactory.Create(this, typeof(T));
            }

            return _repositories[type] as IRepository<T>;
        }

        private IRepository GetRepository(object item)
        {
            var type = GetUnproxiedType(item);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = _repositoryFactory.Create(this, type);
            }
            return _repositories[type];
        }

        private Type GetUnproxiedType(object source)
        {
            var proxy = source as IProxyTargetAccessor;
            if (proxy == null)
            {
                return source.GetType();
            }
            return proxy.GetType().BaseType;
        }
    }
}
