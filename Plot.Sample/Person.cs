using Plot.Attributes;

namespace Plot.Sample
{
    public class Person
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual int Born { get; set; }
        

        [Relationship(Relationships.ActedIn)]
        public virtual Movie Organisation { get; set; }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}