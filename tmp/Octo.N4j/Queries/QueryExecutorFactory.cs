using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo4jClient;
using Octo.Core;
using Octo.Core.Queries;

namespace Octo.N4j.Queries
{
    public class QueryExecutorFactory : IQueryExecutorFactory
    {
        private readonly Dictionary<Type, Func<IGraphSession, IQueryExecutor>> _queries;

        public QueryExecutorFactory()
        {
            _queries = new Dictionary<Type, Func<IGraphSession, IQueryExecutor>>();
        }


        public QueryExecutorFactory(GraphClient db, params Assembly[] assemblies) : this()
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => typeof(IQueryExecutor).IsAssignableFrom(x));

                foreach (var type in types)
                {
                    if (type.BaseType == null)
                    {
                        continue;
                    }

                    var arguments = type.BaseType.GetGenericArguments();

                    if (arguments.Length == 3)
                    {
                        var local = type;

                        Register(session => (IQueryExecutor) Activator.CreateInstance(local, db, session), arguments[2]);
                    }
                }
            }
        }

        public QueryExecutorFactory Register<T>(Func<IGraphSession, IQueryExecutor> factory) where T : IQuery
        {
            Register(factory, typeof (T));

            return this;
        }

        public void Register(Func<IGraphSession, IQueryExecutor> factory, Type type)
        {
            _queries.Add(type, factory);
        }

        public IQueryExecutor<TResult> Create<TResult>(IQuery<TResult> query, IGraphSession session)
        {
            if (!_queries.ContainsKey(query.GetType()))
            {
                throw new InvalidOperationException("Query executor has not been registered with the query executor factory");
            }

            return (IQueryExecutor<TResult>)_queries[query.GetType()](session);
        }
    }
}
