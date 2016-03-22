using System;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Invitation
    {
        public Invitation()
        {
            Created = DateTime.UtcNow;
            ValidUntil = Created.AddHours(48);
        }

        public virtual string Id { get; set; }

        public virtual string Email { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual DateTime ValidUntil { get; set; }

        public virtual bool Accepted { get; set; }

        public virtual DateTime? AcceptedOn { get; set; }

        [Relationship(Relationships.AcceptedBy)]
        public virtual User AcceptedBy { get; set; }

        [Relationship(Relationships.IssuedBy)]
        public virtual User IssuedBy { get; set; }

        [Relationship(Relationships.InvitesUsersTo)]
        public virtual Membership Membership { get; set; }

        public virtual void Accept(User user)
        {
            if (!IsValid)
            {
                throw new Exception();
            }
            Accepted = true;
            AcceptedOn = DateTime.UtcNow;
            AcceptedBy = user;
            user.Add(Membership);
        }

        public virtual bool IsValid
        {
            get
            {
                if (Accepted)
                {
                    return false;
                }

                return DateTime.UtcNow <= ValidUntil;
            }
        }
    }
}
