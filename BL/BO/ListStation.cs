﻿
namespace BO
{
    public class ListStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargeSlots { get; set; }
        public int UnavailableChargeSlots { get; set; }

        public override string ToString()
        {
            string str = $"\nStation {Id}:";
            str += $"\n Name - {Name}";
            str += $"\n Available charge slots - {AvailableChargeSlots}";
            str += $"\n Occupied charge slots - {UnavailableChargeSlots}";
            return str;
        }
    }
}
