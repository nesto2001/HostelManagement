using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Rent
    {
        public Rent()
        {
            Bills = new HashSet<Bill>();
            RoomMembers = new HashSet<RoomMember>();
        }

        public int RentId { get; set; }
        public int RoomId { get; set; }
        public string RentedBy { get; set; }
        public int IsDeposited { get; set; }
        public int Status { get; set; }
        public decimal Total { get; set; }
        public DateTime StartRentDate { get; set; }
        public DateTime EndRentDate { get; set; }

        public virtual Account RentedByNavigation { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<RoomMember> RoomMembers { get; set; }
    }
}
