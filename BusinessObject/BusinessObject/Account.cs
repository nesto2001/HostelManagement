using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Account
    {
        public Account()
        {
            Hostels = new HashSet<Hostel>();
            Rents = new HashSet<Rent>();
            RoomMembers = new HashSet<RoomMember>();
        }

        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Dob { get; set; }
        public string ProfilePicUrl { get; set; }
        public string IdCardNumber { get; set; }

        public virtual IdentityCard IdCardNumberNavigation { get; set; }
        public virtual ICollection<Hostel> Hostels { get; set; }
        public virtual ICollection<Rent> Rents { get; set; }
        public virtual ICollection<RoomMember> RoomMembers { get; set; }
    }
}
