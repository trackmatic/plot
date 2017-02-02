using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    [Ignore]
    public class MovieId
    {
        public MovieId(string id)
        {
            Value = id;
        }

        public string Value { get; set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as MovieId;
            if (other == null)
            {
                return false;
            }
            return other.GetHashCode() == GetHashCode();
        }

        public override string ToString()
        {
            return Value;
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
