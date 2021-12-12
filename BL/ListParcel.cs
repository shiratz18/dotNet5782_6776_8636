using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            string str = $"\nParcel {Id}:";
            str += $"\n Sender - {SenderName}";
            str += $"\n Target - {TargetName}";
            str += $"\n Weight - {Weight}";
            str += $"\n Priority - {Priority}";
            str += $"\n State - {State}";
            return str;
        }
    }
}
