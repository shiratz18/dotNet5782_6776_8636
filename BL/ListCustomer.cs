using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ListCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int DeliveredFromCustomer { get; set; }
        public int NotDeliveredFromCustomer { get; set; }
        public int DeliveredToCustomer { get; set; }
        public int NotDeliveredToCustomer { get; set; }

        public override string ToString()
        {
            string str = $"Customer {Id}:";
            str += $"\nName - {Name}\n";
            str += $"\nPhone - {Phone}";
            str += $"\nParcels delivered from cusotmer - {DeliveredFromCustomer}";
            str += $"\nParcels not yet delivered from customer - {NotDeliveredFromCustomer}";
            str += $"\nParcels delivered to customer - {DeliveredToCustomer}";
            str += $"\nParcels on the way to customer - {NotDeliveredToCustomer}";
            return str;
        }
    }
}
