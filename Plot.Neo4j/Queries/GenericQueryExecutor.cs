using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Queries;

namespace Plot.Neo4j.Queries
{
    public abstract class GenericQueryExecutor<T, TDataset> : GetQueryExecutorBase<T, TDataset>
        where TDataset : IQueryResult
    {
        private readonly IMetadataFactory _metadataFactory;

        protected GenericQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db)
        {
            _metadataFactory = metadataFactory;
        }

        protected override ICypherFluentQuery<TDataset> GetDataset(IGraphClient db, GetAbstractQuery<T> abstractQuery)
        {
            var metadata = _metadataFactory.Create(typeof (T));
            var cypher = db.Cypher.Match($"({CamelCase(metadata.Name)}:{metadata.Name})").Where($"{CamelCase(metadata.Name)}.Id in {{id}}");
            cypher = metadata.Properties.Where(x => x.HasRelationship && !x.Relationship.Lazy).Aggregate(cypher, (current, property) => current.OptionalMatch($"(({CamelCase(metadata.Name)}){RelationshipSyntax.Create(property.Relationship)}({CamelCase(property.Name)}:{property.Type.Name}))"));
            cypher = cypher.WithParam("id", abstractQuery.Id);
            return (ICypherFluentQuery<TDataset>)OnExecute(cypher);
        }

        protected abstract ICypherFluentQuery OnExecute(ICypherFluentQuery cypher);

        private static string CamelCase(string value)
        {
            if (char.IsUpper(value[0]))
            {
                var start =  new[] { char.ToLower(value[0]) };
                var remainder = value.Skip(1).Take(value.Length).ToArray();
                return new string(start.Concat(remainder).ToArray());
            }
            return value;
        }

        private class RelationshipSyntax
        {
            private readonly RelationshipMetadata _relationship;

            private RelationshipSyntax(RelationshipMetadata relationship)
            {
                _relationship = relationship;
            }

            public override string ToString()
            {
                if (_relationship.IsReverse)
                {
                    return $"<-[:{_relationship.Name}]-";
                }

                return $"-[:{_relationship.Name}]->";
            }

            public static RelationshipSyntax Create(RelationshipMetadata relationship)
            {
                return new RelationshipSyntax(relationship);
            }
        }
    }
}
