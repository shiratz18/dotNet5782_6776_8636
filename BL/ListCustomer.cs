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
            string str = $"Customer {Id}:\n";
            str += $"Name - {Name}\n";
            str += $"Phone - {Phone}\n";
            str += $"{DeliveredFromCustomer} delivered parcels sent from the customer\n";
            str += $"{NotDeliveredFromCustomer} not yet delivered parcels sent from the customer\n";
            str += $"Recieved {DeliveredToCustomer} parcels\n";
            str += $"{NotDeliveredToCustomer} parcels on the way to customer\n";
            return str;
        }
    }
}
