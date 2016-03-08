using System.Collections.Generic;
using Octo.Core.Attributes;

namespace Octo.Tests.Model
{
    public class Person
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Name = "LINKED_TO")]
        public virtual IList<Contact> Contacts { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Person;
            if (other == null)
            {
                return false;
            }
            return GetHashCode() == other.GetHashCode();
        }
    }
}
