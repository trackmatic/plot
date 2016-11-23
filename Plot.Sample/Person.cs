using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Person
    {
        public Person()
        {
            Movies = new List<Movie>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int Born { get; set; }
        

        [Relationship(Relationships.ActedIn)]
        public virtual IList<Movie> Movies { get; set; }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(Id);
        }
        public virtual void Add(Movie movie)
        {
            Utils.Add(Movies, movie, () => movie.Add(this));
        }

        public virtual void Remove(Movie movie)
        {
            Utils.Remove(Movies, movie, () => movie.Remove(this));
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}