using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public static class StatementFactory
    {
        public static string With(Node node)
        {
            return $"{Parameter(node)}";
        }

        public static string Match(Node node, string parameter)
        {
            return $"({Parameter(node)}:{node.Metadata.Name} {{ Id:{{{parameter}}}}})";
        }

        public static string Merge(Node node, string parameter)
        {
            return $"({Parameter(node)}:{node.Metadata.Name} {{ Id:{{{parameter}}}}})";
        }
        
        public static string Set(Node node, string parameter)
        {
            return $"{Parameter(node)} = {{{parameter}}}";
        }

        public static string Node(Node node)
        {
            return $"({Parameter(node)}:{node.Metadata.Name})";
        }

        public static string Parameter(Node node)
        {
            var parameter = Parameter(node.Metadata.Name, node.Id);
            return parameter;
        }

        public static string IdParameter(Node node)
        {
            var parameter = Parameter(node.Metadata.Name, $"{node.Id}_id");
            return parameter;
        }

        public static string Parameter(string name, string property)
        {
            return $"{name}_{MakeSafe(property)}";
        }

        public static string Relationship(Node source, Node destination, RelationshipMetadata relationship, string name = null)
        {
            return $"{ExistingNode(source)}{Relationship(relationship, name)}{ExistingNode(destination)}";
        }

        public static string ExistingNode(Node entity)
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
