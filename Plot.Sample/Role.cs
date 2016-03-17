namespace Plot.Sample
{
    public class Role
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

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
