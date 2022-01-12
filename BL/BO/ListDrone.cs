
namespace BO
{
    public class ListDrone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            string str = $"\nDrone {Id}:";
            str += $"\n Model - {Model}";
            str += $"\n Maximum weight - {MaxWeight}";
            str += $"\n Battery - {string.Format("{0:0.0}", Battery)}%";
            str += $"\n Status - {Status}";
            str += $"\n Current location - {CurrentLocation}";
            if (Status == DroneStatuses.Shipping)
                str += $"\n Carrying parcel - {ParcelId}";
            return str;
        }
    }
}
