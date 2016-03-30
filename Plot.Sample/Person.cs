using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Person
    {
        public Person()
        {
            Names = new Names();
            Organisations = new List<Organisation>();
            Sites = new List<Site>();
        }

        public virtual string Id { get; set; }

        [Ignore]
        public virtual Names Names { get; set; }

        [Ignore]
        public virtual Numbers Numbers { get; set; }

        public virtual string Email { get; set; }

        public virtual string IdentityNumber { get; set; }

        public virtual string Title { get; set; }

        public virtual string Position { get; set; }

        public virtual string Department { get; set; }

        public virtual string Gender { get; set; }

        [Relationship(Relationships.IsA, DeleteOrphan = true)]
        public virtual Crew Crew { get; set; }

        [Relationship(Relationships.IsA, DeleteOrphan = true)]
        public virtual Driver Driver { get; set; }

        [Relationship(Relationships.IsA, DeleteOrphan = true)]
        public virtual User User { get; set; }

        [Relationship(Relationships.MemberOf)]
        public virtual IList<Organisation> Organisations { get; set; }

        [Relationship(Relationships.Contracts, Reverse = true)]
        public virtual IList<Site> Sites { get; set; }

        public virtual void Add(Organisation organisation)
        {
            Utils.Add(Organisations, organisation, () => organisation.Add(this));
        }

        public virtual void Add(Site site)
        {
            Utils.Add(Sites, site, () => site.Add(this));
        }

        public virtual void Remove(Site site)
        {
            Utils.Remove(Sites, site, () => site.Remove(this));
        }

        public virtual void Set(User user)
        {
            if (User == user)
            {
                return;
            }
            User = user;
            user?.Set(this);
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
