using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Location
    {
        public Location()
        {
            Hostels = new HashSet<Hostel>();
        }

        public int LocationId { get; set; }
        [Required]
        public string AddressString { get; set; }
        public int WardId { get; set; }

        public virtual Ward Ward { get; set; }
        public virtual ICollection<Hostel> Hostels { get; set; }
    }
}
