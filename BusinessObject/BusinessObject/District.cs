using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class District
    {
        public District()
        {
            Wards = new HashSet<Ward>();
        }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
