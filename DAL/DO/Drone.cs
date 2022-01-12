
namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Drone {Id}:\n";
            result += $"Model - {Model},\n";
            result += $"Maximum weight - {MaxWeight},\n";
            return result;
        }
    }
}
