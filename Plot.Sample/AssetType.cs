namespace Plot.Sample
{
    public abstract class AssetType
    {
        protected AssetType(string typeName)
        {
            TypeName = typeName;
        }

        public virtual string Id { get; set; }

        public virtual string TypeName { get; set; }
    }
}
