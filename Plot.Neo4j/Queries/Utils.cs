using Neo4jClient.Cypher;
using Plot.Metadata;
using System.Linq;

namespace Plot.Neo4j.Queries
{
    public static class QueryUtils
    {
        public static string CamelCase(string value)
        {
            if (char.IsUpper(value[0]))
            {
                var start = new[] { char.ToLower(value[0]) };
                var remainder = value.Skip(1).Take(value.Length).ToArray();
                return new string(start.Concat(remainder).ToArray());
            }
            return value;
        }

        public static ICypherFluentQuery MatchById(this ICypherFluentQuery cypher, NodeMetadata metadata)
        {
            return cypher.Match($"({CamelCase(metadata.Name)}:{metadata.Name})").Where($"{CamelCase(metadata.Name)}.Id in {{id}}");
        }

        public static ICypherFluentQuery IncludeRelationships(this ICypherFluentQuery cypher, NodeMetadata metadata)
        {
            return metadata.Properties.Where(x => x.HasRelationship && !x.Relationship.Lazy).Aggregate(cypher, (current, property) => current.OptionalMatch($"(({CamelCase(metadata.Name)}){RelationshipSyntax.Create(property.Relationship)}({CamelCase(property.Name)}:{property.Type.Name}))"));
        }
    }
}
