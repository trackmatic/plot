using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Organisation
    {
        public Organisation()
        {
            //Sites = new List<Site>();
            //AccessGroups = new List<AccessGroup>();
            //People = new List<Person>();
            //Contacts = new List<Contact>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string LegalName { get; set; }

        public virtual string RegistrationNo { get; set; }

        [Ignore]
        public virtual RegionalSettings RegionalSettings { get; set; }
        
        //[Relationship(Relationships.LocatedAt)]
        public virtual Address Address { get; set; }

        //[Relationship(Relationships.Runs, DeleteOrphan = true)]
        public virtual IList<Site> Sites { get; set; }

        //[Relationship(Relationships.Maintains, DeleteOrphan = true)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        //[Relationship(Relationships.MemberOf, Reverse = true)]
        public virtual IList<Person> People { get; set; }

        //[Relationship(Relationships.Notifies)]
        public virtual IList<Contact> Contacts { get; set; }

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
