using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Category
    {
        public Category()
        {
            Hostels = new HashSet<Hostel>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Hostel> Hostels { get; set; }
    }
}
