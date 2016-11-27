using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher.Commands
{
    internal class CommandFactory<T>
    {
        private readonly IMetadataFactory _metadataFactory;
        private readonly List<ICommand> _commands;

        public CommandFactory(IMetadataFactory metadataFactory)
        {
            _metadataFactory = metadataFactory;
            _commands = new List<ICommand>();
        }

        public IList<ICommand> Commands => _commands;

        public void Insert(T item, object data)
        {
            var command = new CreateNodeCommand(CreateNode(item), () => data);

            _commands.Add(command);
            
            var metadata = _metadataFactory.Create(item);

            foreach (var property in metadata.Properties)
            {
                if (property.IsReadOnly)
                {
                    continue;
                }

                if (property.IsList)
                {
                    var collection = property.GetValue<IEnumerable>(item);
                    CreateRelationship(item, collection, property.Relationship);
                    continue;
                }

                if (property.HasRelationship)
                {
                    CreateRelationship(item, property.Relationship);
                }
            }
        }

        public void Delete(T item)
        {
            var command = new DeleteNodeCommand(CreateNode(item));
            _commands.Add(command);
        }

        private void CreateRelationship(object source, RelationshipMetadata relationship)
        {
            if (relationship.IsReverse)
            {
                return;
            }

            foreach (var trackableRelationship in ProxyUtils.Flush(source, relationship))
            {
                foreach (var destination in trackableRelationship.Flush())
                {
                    DeleteRelationship(source, destination, relationship);
                }

                if (trackableRelationship.Current == null)
                {
                    continue;
                }

                CreateRelationship(source, trackableRelationship.Current, relationship);
            }
        }

        private void CreateRelationship(object source, IEnumerable collection, RelationshipMetadata relationship)
        {
            if (relationship == null || relationship.IsReverse)
            {
                return;
            }

            var destinations = collection as object[] ?? collection.Cast<object>().ToArray();

            foreach (var destination in destinations)
            {
                CreateRelationship(source, destination, relationship);
            }
            
            if (!ProxyUtils.IsTrackable(destinations))
            {
                return;
            }

            foreach (var destination in ProxyUtils.Flush(destinations))
            {
                DeleteRelationship(source, destination, relationship);
            }
        }
        
        private void CreateRelationship(object source, object destination, RelationshipMetadata relationship)
        {
            var command = new CreateRelationshipCommand(CreateNode(source), CreateNode(destination), relationship);
            _commands.Add(command);
        }

        private void DeleteRelationship(object source, object destination, RelationshipMetadata relationship)
        {
            var command = new DeleteRelationshipCommand(CreateNode(source), CreateNode(destination), relationship);
            _commands.Add(command);
        }
        
        private Node CreateNode(object value)
        {
            return new Node(_metadataFactory.Create(value), value);
        }
    }
}
