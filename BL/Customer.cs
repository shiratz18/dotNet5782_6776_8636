using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelAtCustomer> FromCustomer { get; set; }
        public List<ParcelAtCustomer> ToCustomer { get; set; }

        public override string ToString()
        {
            string str = $"\nCustomer {Id}:";
            str += $"\n Name - {Name}";
            str += $"\n Phone - {Phone}";
            str += $"\n Location - {Location}";
            str += $"\n Parcels from customer - ";
            FromCustomer.ForEach(p =>
            {
                str += $"\n\t{p}";
            });
            str += $"\n Parcels to customer - ";
            ToCustomer.ForEach(p =>
            {
                str += $"\n\t{p}";
            });
            return str;
        }
    }
}
