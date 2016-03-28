namespace Plot.Sample.Data.Nodes
{
    public class VehicleNode : AssetTypeNode
    {
        public VehicleNode()
        {
            
        }

        public VehicleNode(Vehicle vehicle)
        {
            Id = vehicle.Id;
            TypeName = vehicle.TypeName;
        }

        public string TypeName { get; set; }

        public Vehicle AsVehicle()
        {
            return new Vehicle
            {
                Id = Id,
                TypeName = TypeName
            };
        }
    }
}
