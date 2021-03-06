﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo4j.Driver.V1;
using Plot.Queries;
using Plot.Metadata;

namespace Plot.Neo4j.Queries
{
    public class QueryExecutorFactory : IQueryExecutorFactory
    {
        private readonly IDictionary<Type, Func<IGraphSession, IQueryExecutor>> _queries;

        private readonly IMetadataFactory _metadataFactory;

        public QueryExecutorFactory(IMetadataFactory metadataFactory)
        {
            _queries = new ConcurrentDictionary<Type, Func<IGraphSession, IQueryExecutor>>();
            _metadataFactory = metadataFactory;
        }


        public QueryExecutorFactory(ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory, params Assembly[] assemblies) : this(metadataFactory)
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

                        Register(session => (IQueryExecutor) Activator.CreateInstance(local, transactionFactory, _metadataFactory), arguments[2]);
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
