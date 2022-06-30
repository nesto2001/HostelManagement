using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class RoomMember
    {
        public int RoomMemberId { get; set; }
        public string UserEmail { get; set; }
        public int RentId { get; set; }
        public bool? IsPresentator { get; set; }
        public int? Status { get; set; }
        public int RoomId { get; set; }
        public DateTime StartRentDate { get; set; }
        public DateTime EndRentDate { get; set; }

        public virtual Rent Rent { get; set; }
    }
}
