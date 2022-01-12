
namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Password { get; set; }
        public string Answer { get; set; }
        public bool Active { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Costumer {Id}:\n";
            result += $"Name - {Name}\n";
            result += $"Phone number - {Phone},\n";
            result += $"Location - {Location.Lat(Latitude)} {Location.Lng(Longitude)}\n";
            return result;
        }
    }

}

