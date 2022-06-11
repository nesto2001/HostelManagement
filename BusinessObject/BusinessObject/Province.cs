using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
