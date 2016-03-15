using System;

namespace Plot.Metadata
{
    public class RelationshipMetadata
    {
        public RelationshipMetadata()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public bool IsReverse { get; set; }

        public string Name { get; set; }

        public bool DeleteOrphan { get; set; }

        public bool Lazy { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as RelationshipMetadata;
            if (other == null)
            {
                return false;
            }
            return GetHashCode() == other.GetHashCode();
        }
    }
}
