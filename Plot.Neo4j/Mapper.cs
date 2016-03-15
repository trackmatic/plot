using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Logging;
using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Cypher.Commands;
using Plot.Neo4j.Queries;
using Plot.Proxies;
using Plot.Queries;

namespace Plot.Neo4j
{
    public abstract class Mapper<T> : IMapper<T>
    {
        private readonly IGraphSession _session;

        private readonly ICypherTransactionFactory _transactionFactory;

        protected Mapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory)
        {
            Db = db;
            _session = session;
            _transactionFactory = transactionFactory;
            MetadataFactory = metadataFactory;
        }

        public void Insert(T item, EntityState state)
        {
            Execute(item, OnInsert);
        }

        public void Delete(T item, EntityState state)
        {
            Execute(item, OnDelete);
        }

        public void Update(T item, EntityState state)
        {
            Execute(item, OnUpdate);
        }

        public void Insert(object item, EntityState state)
        {
            Insert((T) item, state);
        }

        public void Delete(object item, EntityState state)
        {
            Delete((T) item, state);
        }

        public void Update(object item, EntityState state)
        {
            Update((T) item, state);
        }

        public IEnumerable<T> Get(string[] id)
        {
            var items = OnGet(id).ToList();
            return items;
        }

        public Type Type => typeof(T);

        protected GraphClient Db { get; }

        protected abstract object GetData(T item);

        protected abstract IQueryExecutor<T> CreateQueryExecutor();

        protected IGraphSession Session => _session;
        
        private void Execute(T item, Func<ICypherFluentQuery, T, IEnumerable<ICommand>> operation)
        {
            var transaction = _transactionFactory.Create(_session);
            transaction.Enlist(this, query =>
            {
                foreach (var command in operation(query, item))
                {
                    query = command.Execute(query);
                }
                return query;
            });
        }

        private IList<ICommand> OnUpdate(ICypherFluentQuery query, T item)
        {
            return OnInsert(query, item);
        }

        private IList<ICommand> OnInsert(ICypherFluentQuery query, T item)
        {
            var commands = new List<ICommand> { new CreateCommandBase(new NodeSnippet(MetadataFactory.Create(item), item), () => GetData(item)) };

            var metadata = MetadataFactory.Create(item);

            foreach (var property in metadata.Properties)
            {
                if (property.IsReadOnly)
                {
                    continue;
                }

                if (property.IsList)
                {
                    var collection = property.GetValue<IEnumerable>(item);
                    commands.AddRange(CreateRelationshipCommands(item, collection, property.Relationship));
                    continue;
                }

                if (property.HasRelationship)
                {
                    commands.AddRange(CreateRelationshipCommands(item, property.Relationship));
                }
            }

            return commands;
        }

        private IList<ICommand> OnDelete(ICypherFluentQuery query, T item)
        {
            var commands = new List<ICommand>
            {
                new DeleteCommand(new NodeSnippet(MetadataFactory.Create(item), item))
            };
            return commands;
        }

        private IList<T> OnGet(params string[] id)
        {
            var executor = CreateQueryExecutor();
            var item = executor.Execute(Session.Uow, new GetAbstractQuery<T>(id));
            return item.ToList();
        }

        protected IMetadataFactory MetadataFactory { get; }

        private IEnumerable<ICommand> CreateRelationshipCommands(object source, RelationshipMetadata relationship)
        {
            var commands = new List<ICommand>();
            if (relationship.IsReverse)
            {
                return commands;
            }
            var trackableRelationships = ProxyUtils.Flush(source, relationship);
            foreach (var trackableRelationship in trackableRelationships)
            {
                commands.AddRange(CreateDeleteRelationshipCommands(source, trackableRelationship, relationship));
                if (trackableRelationship.Current == null)
                {
                    continue;
                }
                commands.Add(CreateRelationship(source, trackableRelationship.Current, relationship));
            }
            return commands;
        }

        private IEnumerable<ICommand> CreateDeleteRelationshipCommands(object source, ITrackableRelationship trackableRelationship, RelationshipMetadata relationship)
        {
            var commands = new List<ICommand>();
            foreach (var destination in trackableRelationship.Flush())
            {
                var command = DeleteRelationship(source, destination, relationship);
                commands.Add(command);
                if (relationship.DeleteOrphan)
                {
                    commands.Add(new DeleteWithCommand(new NodeSnippet(MetadataFactory.Create(destination), destination)));
                }
            }
            return commands;
        }

        private IEnumerable<ICommand> CreateRelationshipCommands(object source, IEnumerable collection, RelationshipMetadata relationship)
        {
            var commands = new List<ICommand>();
            if (relationship == null || relationship.IsReverse)
            {
                return commands;
            }
            commands.AddRange(from object destination in collection select CreateRelationship(source, destination, relationship));
            if (ProxyUtils.IsTrackable(collection))
            {
                foreach (var destination in ProxyUtils.Flush(collection))
                {
                    commands.Add(DeleteRelationship(source, destination, relationship));
                }
            }
            return commands;
        }
        
        private ICommand CreateRelationship(object source, object destination, RelationshipMetadata relationship)
        {
            var sourceMetadata = MetadataFactory.Create(source);
            var destinationMetadata = MetadataFactory.Create(destination);
            var command = new CreateRelationshipCommand(new ParamSnippet(sourceMetadata, source), new NodeSnippet(destinationMetadata, destination), relationship.Name);
            return command;
        }

        private ICommand DeleteRelationship(object source, object destination, RelationshipMetadata relationship)
        {
            var sourceMetadata = MetadataFactory.Create(source);
            var destinationMetadata = MetadataFactory.Create(destination);
            var command = new DeleteRelationshipCommand(new ParamSnippet(sourceMetadata, source), new NodeSnippet(destinationMetadata, destination), relationship.Name, relationship.DeleteOrphan);
            return command;
        }
    }
}
