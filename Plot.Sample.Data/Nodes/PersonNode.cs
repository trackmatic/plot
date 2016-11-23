using Neo4j.Driver.V1;
using Plot.Neo4j;

namespace Plot.Sample.Data.Nodes
{
    public class PersonNode
    {
        public PersonNode(INode node)
        {
            Id = node.Read<string>(Keys.Id);
            Name = node.Read<string>(Keys.Name);
            Born = node.Read<int>(Keys.Born);
        }

        public PersonNode(Person item)
        {
            Id = item.Id;
            Name = item.Name;
            Born = item.Born;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Born { get; set; }

        public Person AsPerson()
        {
            var site = new Person
            {
                Id = Id,
                Name = Name,
                Born = Born
            };
            return site;
        }

        private static class Keys
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string Born = "Born";
        }
    }
}
