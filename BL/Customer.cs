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
            string str = $"Customer {Id}:\n";
            str += $"Name - {Name}\n";
            str += $"Phone - {Phone}\n";
            str += $"Location - {Location}\n";
            str += $"Parcels from the customer - {FromCustomer}\n";
            str += $"Parcels to customer - {ToCustomer}\n";
            return str;
        }
    }
}
