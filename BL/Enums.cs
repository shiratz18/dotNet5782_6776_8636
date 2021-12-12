using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum WeightCategories { Light, Medium, Heavy };
    public enum DroneStatuses { Available, Maintenance, Shipping };
    public enum Priorities { Regular, Express, Urgent };
    public enum ParcelState { Requested, Scheduled, PickedUp, Delivered };
}
