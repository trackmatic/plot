using System.Collections.Generic;
using Plot.Attributes;
using Plot.Tests.Utility;

namespace Plot.Tests.Model
{
    public class Person
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Name = "LINKED_TO")]
        public virtual IList<Contact> Contacts { get; set; }

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
