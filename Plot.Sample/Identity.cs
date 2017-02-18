using System;

namespace Plot.Sample
{
    public abstract class Identity
    {
        private string _value;

        protected Identity(string value)
        {
            _value = value;
        }

        protected Identity()
        {
            _value = IdentityGenerator.NewSequentialId().ToString();
        }

        public virtual string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            if (obj is Identity)
                return _value.Equals(((Identity)obj)._value);

            if (obj is string)
                return _value.Equals((string)obj);

            return false;
        }

        public static bool operator ==(Identity lhs, Identity rhs)
        {
            return Object.Equals(lhs, rhs);
        }

        public static bool operator !=(Identity lhs, Identity rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
