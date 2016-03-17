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

        [Relationship(Relationships.RequestedBy)]
        public virtual User User { get; set; }

        public virtual void Complete(Password password)
        {
            if (!IsValid())
            {
                throw new InvalidResetPasswordRequest();
            }
            User.Password = password;
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
