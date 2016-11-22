using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Movie
    {
        public Movie()
        {
            People = new List<Person>();
        }

        public virtual string Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string TagLine { get; set; }

        [Relationship(Relationships.ActedIn, Reverse = true)]
        public virtual IList<Person> People { get; set; }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
