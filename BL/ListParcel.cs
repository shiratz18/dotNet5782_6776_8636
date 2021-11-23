using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ListParcel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelState State { get; set; }

        public override string ToString()
        {
            string str = $"Parcel {Id}:";
            str += $"\nSender - {SenderName}";
            str += $"\nTarget - {TargetName}";
            str += $"\nWeight -{Weight}";
            str += $"\nPriority - {Priority}";
            str += $"\nState - {State}";
            return str;
        }
    }
}
