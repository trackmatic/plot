using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class User
    {
        public User()
        {
            AccessGroups = new List<AccessGroup>();
            Modules = new List<ModulePermission>();
        }

        public virtual string Id { get; set; }
        
        public virtual string Username { get; set; }

        [Relationship(Relationships.IsA, Reverse = true)]
        public virtual Person Person { get; set; }

        [Relationship(Relationships.HasAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        [Relationship(Relationships.HasAccessTo)]
        public virtual IList<ModulePermission> Modules { get; set; }

        public virtual void Set(Person person)
        {
            if (Person == person)
            {
                return;
            }
            Person = person;
            person?.Set(this);
        }

        public virtual void Add(ModulePermission module)
        {
            Utils.Add(Modules, module);
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
