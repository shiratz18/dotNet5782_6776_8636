using System.Collections.Generic;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelAtCustomer> FromCustomer { get; set; }
        public List<ParcelAtCustomer> ToCustomer { get; set; }
        public string Password { get; set; }
        public string Answer{ get; set; }
        public bool Active { get; set; }

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
