namespace Octo.Tests.Model
{
    public class Contact
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Contact;
            if (other == null)
            {
                return false;
            }
            return GetHashCode() == other.GetHashCode();
        }
    }
}
