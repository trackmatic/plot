using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Person
    {
        public Person()
        {
            Organisations = new List<Organisation>();
            Sites = new List<Site>();
        }

        public virtual string Id { get; set; }

        [Ignore]
        public virtual Names Names { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Email { get; set; }

        [Relationship(Relationships.IsA)]
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
