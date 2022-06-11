using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int BillId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int RentId { get; set; }

        public virtual Rent BillNavigation { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
