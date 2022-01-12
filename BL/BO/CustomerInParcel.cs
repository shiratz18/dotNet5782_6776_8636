
namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            string str = $"ID: {Id} Name: {Name}\n";
            return str;
        }
    }
}