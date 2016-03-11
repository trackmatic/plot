using System;

namespace Plot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelationshipAttribute : Attribute
    {
        public RelationshipAttribute()
        {
            
        }

        public RelationshipAttribute(string name)
        {
            Name = name;
        }

        public RelationshipAttribute(string name, bool reverse)
        {
            Name = name;
            Reverse = reverse;
        }

        public bool Reverse { get; set; }

        public string Name { get; set; }

        public bool DeleteOrphan { get; set; }

        public bool Lazy { get; set; }
    }
}
