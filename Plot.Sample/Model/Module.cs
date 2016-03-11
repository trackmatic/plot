namespace Plot.Sample.Model
{
    public class Module
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

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
