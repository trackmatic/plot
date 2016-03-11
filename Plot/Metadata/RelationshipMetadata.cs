namespace Plot.Metadata
{
    public class RelationshipMetadata
    {
        public bool IsReverse { get; set; }

        public string Name { get; set; }

        public bool DeleteOrphan { get; set; }

        public bool Lazy { get; set; }
    }
}
