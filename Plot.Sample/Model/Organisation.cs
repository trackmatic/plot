using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Organisation
    {
        public Organisation()
        {
            Sites = new List<Site>();
            AccessGroups = new List<AccessGroup>();
            People = new List<Person>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.Runs, DeleteOrphan = true)]
        public virtual IList<Site> Sites { get; set; }

        [Relationship(Relationships.Maintains, DeleteOrphan = true)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        [Relationship(Relationships.MemberOf, Reverse = true)]
        public virtual IList<Person> People { get; set; }
        
        public virtual void Add(Person person)
        {
            Utils.Add(People, person, () => person.Add(this));
        }

        public virtual void Add(AccessGroup accessGroup)
        {
            Utils.Add(AccessGroups, accessGroup);
        }

        public virtual void Add(Site site)
        {
            Utils.Add(Sites, site, () => site.Set(this));
        } 

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
