using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class BillDetail
    {
        public int BillDetailId { get; set; }
        public int BillId { get; set; }
        public string BillDescription { get; set; }
        public decimal Fee { get; set; }
        public DateTime DateIssued { get; set; }

        public virtual Bill Bill { get; set; }
    }
}
