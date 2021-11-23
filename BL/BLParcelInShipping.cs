using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL:IBL
    {
        private IEnumerable<ParcelInShipping> getListOfNoDroneParcelsInShipping()
        {
            IEnumerable<ListParcel> parcels = GetNoDroneParcelList();
            List<ParcelInShipping> pis = new List<ParcelInShipping>();

            foreach(ListParcel p in parcels)
            {
                pis.Add(new ParcelInShipping
                {
                    Id=p.Id,
                    IsPickedUp=false,
                    Priority=p.Priority,
                    Weight=p.Weight
                });
            }

            foreach(ParcelInShipping p in pis)
            {
                
            }
        }
    }
}
