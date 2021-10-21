using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                string result = " ";
                result += $"ID: {Id},\n";
                result += $"Sender ID: {SenderId},\n";
                result += $"Target ID: {TargetId},\n";
                result += $"Weight: {Weight},\n";
                result += $"Priority: {Priority},\n";
                result += $"Requested: {Requested},\n";
                result += $"Drone ID: {DroneId},\n";
                result += $"Scheduled: {Scheduled},\n";
                result += $"Picked up: {PickedUp},\n";
                result += $"Delivered: {Delivered}\n";
                return result;
            }
        }
    }
}
