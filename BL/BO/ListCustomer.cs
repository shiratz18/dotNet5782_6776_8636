
namespace BO
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
            string str = $"\nCustomer {Id}:";
            str += $"\n Name - {Name}";
            str += $"\n Phone - {Phone}";
            str += $"\n Parcels delivered from cusotmer - {DeliveredFromCustomer}";
            str += $"\n Parcels not yet delivered from customer - {NotDeliveredFromCustomer}";
            str += $"\n Parcels delivered to customer - {DeliveredToCustomer}";
            str += $"\n Parcels on the way to customer - {NotDeliveredToCustomer}";
            return str;
        }
    }
}
