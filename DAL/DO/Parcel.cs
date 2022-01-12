using System;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? Requested { get; set; }
        public int DroneId { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Parcel {Id}:\n";
            result += $"Sender ID - {SenderId},\n";
            result += $"Target ID - {TargetId},\n";
            result += $"Weight - {Weight},\n";
            result += $"Rank - {Priority},\n";
            result += $"Requested time - {Requested},\n";
            if (DroneId != 0)
            {
                result += $"Assigned drone ID - {DroneId},\n";
                result += $"Scheduled - {Scheduled},\n";
                if (PickedUp != null)
                {
                    result += $"Picked up - {PickedUp},\n";
                    if (Delivered != null)
                        result += $"Delivered - {Delivered}\n";
                }
            }
            return result;
        }
    }
}

