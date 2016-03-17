namespace Plot.Sample.Data.Nodes
{
    public class AddressNode
    {
        public AddressNode()
        {
            
        }

        public AddressNode(Address item)
        {
            Id = item.Id;
            UnitNo = item.UnitNo;
            BuildingName = item.BuildingName;
            StreetNo = item.StreetNo;
            SubDivNo = item.SubDivNo;
            Street = item.Street;
            Suburb = item.Suburb;
            City = item.City;
            Province = item.City;
            PostalCode = item.PostalCode;
            Coords = item.Coords;
        }

        public string Id { get; set; }

        public string UnitNo { get; set; }

        public string BuildingName { get; set; }

        public string StreetNo { get; set; }

        public string SubDivNo { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public double[] Coords { get; set; }

        public Address AsAddress()
        {
            return new Address
            {
                Id = Id,
                UnitNo = UnitNo,
                BuildingName = BuildingName,
                StreetNo = StreetNo,
                SubDivNo = SubDivNo,
                Street = Street,
                Suburb = Suburb,
                City = City,
                Province = Province,
                PostalCode = PostalCode,
                Coords = Coords
            };
        }
    }
}
