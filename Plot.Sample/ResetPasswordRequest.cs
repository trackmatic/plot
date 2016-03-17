using System;
using Plot.Attributes;
using Plot.Sample.Exceptions;

namespace Plot.Sample
{
    public class ResetPasswordRequest
    {
        public ResetPasswordRequest()
        {
            Created = DateTime.UtcNow;
            ValidUntil = Created.AddHours(24);
        }

        public virtual string Id { get; set; }

        public virtual DateTime Created { get; set; }
        
        public virtual DateTime ValidUntil { get; set; }

        public virtual bool IsComplete { get; set; }

        [Relationship(Relationships.OnBehalfOf)]
        public virtual User User { get; set; }

        [Relationship(Relationships.RequestedBy)]
        public virtual User RequestedBy { get; set; }

        [Relationship(Relationships.Reset)]
        public virtual Password Password { get; set; }

        public virtual void Complete(Password password)
        {
            /*if (!IsValid())
            {
                throw new InvalidResetPasswordRequest();
            }*/
            Password = password;
            IsComplete = true;
        }

        public virtual bool IsValid()
        {
            if (IsComplete)
            {
                return false;
            }

            return DateTime.UtcNow <= ValidUntil;
        }
    }
}
