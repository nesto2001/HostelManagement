using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Ward
    {
        public Ward()
        {
            Locations = new HashSet<Location>();
        }

        public int WardId { get; set; }
        public string WardName { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
