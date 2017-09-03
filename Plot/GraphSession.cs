﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Plot.Proxies;
using Plot.Queries;

namespace Plot
{
    public class GraphSession : IGraphSession
    {
        private readonly IDictionary<Type, IRepository> _repositories;
        private readonly IUnitOfWork _uow;
        private readonly List<IListener> _listeners;
        private readonly IQueryExecutorFactory _queryExecutorFactory;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IEntityStateCache _state;
        private readonly IProxyFactory _proxyFactory;
        private bool _disposed;

        public GraphSession(IUnitOfWork uow, IEnumerable<IListener> listeners, IQueryExecutorFactory queryExecutorFactory, IRepositoryFactory repositoryFactory, IEntityStateCache state, IProxyFactory proxyFactory)
        {
            _repositories = new ConcurrentDictionary<Type, IRepository>();
            _uow = uow;
            _listeners = listeners.ToList();
            _queryExecutorFactory = queryExecutorFactory;
            _repositoryFactory = repositoryFactory;
            _state = state;
            _proxyFactory = proxyFactory;
        }

        public T Create<T>(T item)
        {
            if (item == null)
            {
                throw new NullReferenceException("Object cannot be null");
            }
            var repository = GetRepository(item);
            return (T)repository.Create(item);
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

        public IEnumerable<T> Get<T>(params object[] id)
        {
            var repository = GetRepositoryOfType<T>();
            return repository.Get(id.Select(x => Conventions.ConvertIdToString(x)).ToArray());
        }

        public T Get<T>(object id)
        {
            var items = Get<T>(new[] {id});
            return items.FirstOrDefault();
        }

        public T Get<T>(IQuery<T> query, bool enlist = true)
        {
            var results = Query(query, enlist);

            if (results.Data.Count() > 1)
            {
                throw new InvalidOperationException("A query must return only 1 result to be used in the Map<T> method");
            }

            return results.Data.FirstOrDefault();
        }

        public IPagedGraphCollection<TResult> Query<TResult>(IQuery<TResult> query, bool enlist = false)
        {
            var executor = _queryExecutorFactory.Create(query, this);
            return executor.ExecuteWithPaging(this, query, enlist);
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
            _state.Dispose();
            OnDisposed();
            _disposed = true;
            Disposed(this, new GraphSessionDisposedEventArgs(this));
        }
        
        public IUnitOfWork Uow => _uow;

        public IEntityStateCache State => _state;

        public IProxyFactory ProxyFactory => _proxyFactory;

        public bool Register(object item)
        {
            var state = GetState(item);
            var result = Register(item, state);
            state.New();
            return result;
        }

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
            _state.Remove(item);
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
                var state = _state.Get(item);
                if (state.Status == EntityStatus.Deleted)
                {
                    mapper.Delete(item, state);
                    Publish(ProxyUtils.GetTargetEntityType(item), x => x.Delete(aggregate, this));
                }
                else if (state.Status == EntityStatus.Dirty)
                {
                    mapper.Update(item, state);
                    Publish(ProxyUtils.GetTargetEntityType(item), x => x.Update(aggregate, this));
                }
                else if (state.Status == EntityStatus.New)
                {
                    mapper.Insert(item, state);
                    Publish(ProxyUtils.GetTargetEntityType(item), x => x.Create(aggregate, this));
                }
                state.Clean();
            }
            Flushed(this, new GraphSessionFlushedEventArgs(this));
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

        public event EventHandler<GraphSessionFlushedEventArgs> Flushed = delegate { };

        public event EventHandler<GraphSessionDisposedEventArgs> Disposed  = delegate { };

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
            var type = ProxyUtils.GetTargetType(item);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = _repositoryFactory.Create(this, type);
            }
            return _repositories[type];
        }

        private EntityState GetState(object proxy)
        {
            return _state.Contains(proxy) ? _state.Get(proxy) : _state.Create(proxy);
        }
    }
}
