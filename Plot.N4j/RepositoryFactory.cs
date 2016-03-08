using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo4jClient;
using Plot.Metadata;
using Plot.Proxies;

namespace Plot.N4j
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<Type, Func<IGraphSession, IMapper>> _mappers;

        private readonly ICypherTransactionFactory _cypherTransactionFactory;

        private readonly IProxyFactory _proxyFactory;

        private readonly IMetadataFactory _metadataFactory;

        public RepositoryFactory(ICypherTransactionFactory cypherTransactionFactory, IProxyFactory proxyFactory, IMetadataFactory metadataFactory)
        {
            _mappers = new Dictionary<Type, Func<IGraphSession, IMapper>>();
            _cypherTransactionFactory = cypherTransactionFactory;
            _proxyFactory = proxyFactory;
            _metadataFactory = metadataFactory;
        }

        public RepositoryFactory(GraphClient db, ICypherTransactionFactory cypherTransactionFactory, IProxyFactory proxyFactory, IMetadataFactory metadataFactory, params Assembly[] assemblies) 
            : this(cypherTransactionFactory, proxyFactory, metadataFactory)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => typeof(IMapper).IsAssignableFrom(x));
                foreach (var type in types)
                {
                    foreach (var item in type.GetInterfaces())
                    {
                        var arguments = item.GetGenericArguments();
                        if (arguments.Any())
                        {
                            var local = type;
                            Register(session => (IMapper)Activator.CreateInstance(local, db, session, _cypherTransactionFactory, _metadataFactory), arguments[0]);
                        }
                    }
                }
            }
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
