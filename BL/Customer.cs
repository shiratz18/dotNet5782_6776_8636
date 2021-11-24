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
            str += $"\nName - {Name}";
            str += $"\nPhone - {Phone}";
            str += $"\nLocation - {Location}";
            str += $"\nParcels from customer - ";
            FromCustomer.ForEach(p =>
            {
                str += $"\n\t{p}";
            });
            str += $"\nParcels to customer - ";
            ToCustomer.ForEach(p =>
            {
                str += $"\n\t{p}";
            });
            return str;
        }
    }
}
