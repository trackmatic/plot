using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Person
    {
        public virtual string Id { get; set; }

        public virtual Names Names { get; set; }

        [Relationship(Relationships.IsA)]
        public virtual User User { get; set; }

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
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
