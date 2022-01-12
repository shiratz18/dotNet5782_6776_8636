
namespace BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelState State { get; set; }
        public CustomerInParcel OtherSide { get; set; }

        public override string ToString()
        {
            string str = $"Parcel {Id}:";
            str += $" Weight - {Weight},";
            str += $" Priority - {Priority},";
            str += $" Other side - {OtherSide}";
            return str;
        }
    }
}
