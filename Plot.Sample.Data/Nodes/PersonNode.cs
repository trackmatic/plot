using Neo4j.Driver.V1;

namespace Plot.Sample.Data.Nodes
{
    public class PersonNode
    {
        public PersonNode(INode node)
        {
            Id = node[Keys.Id].As<string>();
            Name = node[Keys.Name].As<string>();
            Born = node[Keys.Born].As<int>();
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
