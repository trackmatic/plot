using System;
using System.Collections.Generic;
using Plot.Proxies;

namespace Plot.Testing
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<Type, Func<IGraphSession, IMapper>> _mappers;
        
        private readonly IProxyFactory _proxyFactory;
        
        public RepositoryFactory(IProxyFactory proxyFactory)
        {
            _mappers = new Dictionary<Type, Func<IGraphSession, IMapper>>();
            _proxyFactory = proxyFactory;
        }

        public IRepository<T> Create<T>(IGraphSession session)
        {
            return Create(session, typeof (T)) as IRepository<T>;
        }

        public IRepository Create(IGraphSession session, Type type)
        {
            if (!_mappers.ContainsKey(type))
            {
                throw new InvalidOperationException($"A mapper for type {type} has not been supplied");
            }
            var generic = typeof (GenericAbstractRepository<>).MakeGenericType(type);
            return (IRepository)Activator.CreateInstance(generic, _mappers[type](session), session, _proxyFactory);
        }

        public RepositoryFactory Register<T>(Func<IGraphSession, IMapper> factory)
        {
            Register(factory, typeof(T));
            return this;
        }

        public void Register(Func<IGraphSession, IMapper> factory, Type type)
        {
            _mappers.Add(type, factory);
        }
    }
}
