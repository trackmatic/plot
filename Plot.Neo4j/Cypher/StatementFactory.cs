using System.Linq;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public static class StatementFactory
    {
        public static string With(Entity entity)
        {
            return $"{Parameter(entity)}";
        }

        public static string Match(Entity entity, string parameter)
        {
            return $"({Parameter(entity)}:{entity.Metadata.Name} {{ Id:{{{parameter}}}}})";
        }

        public static string Merge(Entity entity, string parameter)
        {
            return $"({Parameter(entity)}:{entity.Metadata.Name} {{ Id:{{{parameter}}}}})";
        }
        
        public static string Set(Entity entity, string parameter)
        {
            return $"{Parameter(entity)} = {{{parameter}}}";
        }

        public static string Node(Entity entity)
        {
            return $"({Parameter(entity)}:{entity.Metadata.Name})";
        }

        public static string Parameter(Entity entity)
        {
            var parameter = Parameter(entity.Metadata.Name, entity.Id);
            return parameter;
        }

        public static string IdParameter(Entity entity)
        {
            var parameter = Parameter(entity.Metadata.Name, $"{entity.Id}_id");
            return parameter;
        }

        public static string Parameter(string name, string property)
        {
            return $"{name}_{MakeSafe(property)}";
        }

        public static string Relationship(Entity source, Entity destination, RelationshipMetadata relationship, string name = null)
        {
            return $"{ExistingNode(source)}{Relationship(relationship)}{ExistingNode(destination)}";
        }
        public static string ExistingNode(Entity entity)
        {
            return $"({Parameter(entity)})";
        }

        public static string Relationship(RelationshipMetadata relationship, string name = null)
        {
            var statment =  $"-[{name}:{relationship.Name}]-";
            if (relationship.IsReverse)
            {
                statment = "<" + statment;
            }
            else
            {
                statment = statment + ">";
            }
            return statment;
        }

        private static string MakeSafe(string value)
        {
            return value.Replace("/", "_")
                .Replace("-", "_")
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Replace(":", string.Empty)
                .Replace("#", string.Empty)
                .Replace("(", string.Empty)
                .Replace("?", string.Empty)
                .Replace(")", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("=", string.Empty);
        }
    }
}
