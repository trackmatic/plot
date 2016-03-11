using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class User
    {
        public User()
        {
            AccessGroups = AccessGroups ?? new List<AccessGroup>();
        }

        public virtual string Id { get; set; }
        
        public virtual string Username { get; set; }

        [Relationship(Relationships.IsA, Reverse = true)]
        public virtual Person Person { get; set; }

        [Relationship(Relationships.HasAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        public virtual void Set(Person person)
        {
            if (Person == person)
            {
                return;
            }
            Person = person;
            person?.Set(this);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
