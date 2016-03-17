using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class User
    {
        public User()
        {
            Memberships = new List<Membership>();
        }

        public virtual string Id { get; set; }
        
        public virtual string Username { get; set; }

        public virtual bool IsVerfied { get; set; }

        [Relationship(Relationships.AuthenticatesWith)]
        public virtual Password Password { get; set; }

        [Relationship(Relationships.IsA, Reverse = true)]
        public virtual Person Person { get; set; }

        [Relationship(Relationships.MemberOf, DeleteOrphan = true)]
        public virtual IList<Membership> Memberships { get; set; }

        public virtual void Add(Membership membership)
        {
            Utils.Add(Memberships, membership);
        }

        public virtual void Remove(Membership membership)
        {
            Utils.Remove(Memberships, membership);
        }

        public virtual void Set(Person person)
        {
            if (Person == person)
            {
                return;
            }
            Person = person;
            person?.Set(this);
        }

        public virtual void Set(Password password)
        {
            Password = password;
            password.User = this;
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