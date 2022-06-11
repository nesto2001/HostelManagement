using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class RoomMember
    {
        public int RoomMemberId { get; set; }
        public string UserEmail { get; set; }
        public int RoomId { get; set; }
        public bool? IsPresentator { get; set; }
        public int? Status { get; set; }

        public virtual Room Room { get; set; }
        public virtual Account UserEmailNavigation { get; set; }
    }
}
