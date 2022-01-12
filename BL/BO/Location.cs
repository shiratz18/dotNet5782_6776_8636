
namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            string str = $"{Sexasegimal.Lat(Latitude)}\n{Sexasegimal.Lng(Longitude)}";
            return str;
        }
    }
}
