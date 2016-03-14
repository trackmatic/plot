using System;

namespace Plot.Sample.Model
{
    [Serializable]
    public class Address
    {
        public virtual string Id { get; set; }

        public virtual string UnitNo { get; set; }

        public virtual string BuildingName { get; set; }

        public virtual string StreetNo { get; set; }

        public virtual string SubDivNo { get; set; }

        public virtual string Street { get; set; }

        public virtual string Suburb { get; set; }

        public virtual string City { get; set; }

        public virtual string Province { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual double[] Coords { get; set; }

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
