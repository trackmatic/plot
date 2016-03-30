namespace Plot.Sample.Data.Nodes
{
    public class VehicleNode
    {
        public VehicleNode()
        {
            
        }

        public VehicleNode(Vehicle vehicle)
        {
            Id = vehicle.Id;
        }

        public string Id { get; set; }
        
        public Vehicle AsVehicle()
        {
            return new Vehicle
            {
                Id = Id,
            };
        }
    }
}
