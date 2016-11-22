using System.Collections.Generic;

namespace Plot.Sample.Data.Nodes
{
    public class PersonNode
    {
        public PersonNode()
        {
            
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
    }
}
