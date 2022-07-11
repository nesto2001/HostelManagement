using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Renter")]
        public string RentedBy { get; set; }
        [Display(Name = "Deposited")]
        public int IsDeposited { get; set; }
        public int Status { get; set; }
        [Display(Name = "Monthly Price")]
        public decimal Total { get; set; }
        [Display(Name = "Rental Start Date")]
        public DateTime StartRentDate { get; set; }
        [Display(Name = "Rental End Date")]
        public DateTime EndRentDate { get; set; }
        public virtual Account RentedByNavigation { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<RoomMember> RoomMembers { get; set; }
    }
}
