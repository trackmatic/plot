using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    [Ignore]
    public class MovieId : Identity
    {
        public MovieId(string id) : base(id)
        {
            
        }


        public static implicit operator MovieId(string value)
        {
            if (value == null)
            {
                return null;
            }

            return new MovieId(value);
        }
    }

    public class Movie
    {
        public Movie()
        {
            People = new List<Person>();
        }

        public virtual MovieId Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string TagLine { get; set; }

        [Relationship(Relationships.ActedIn, Reverse = true)]
        public virtual IList<Person> People { get; set; }

        public virtual void Add(Person person)
        {
            Utils.Add(People, person, () => person.Add(this));
        }

        public virtual void Remove(Person person)
        {
            Utils.Remove(People, person, () => person.Remove(this));
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
