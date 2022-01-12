namespace BO
{
    public class ChargingDrone
    {
        public int Id { get; set; }
        public double Battery { get; set; }

        public override string ToString()
        {
            return $"Drone: {Id} Battery - {Battery}";
        }
    }
}
