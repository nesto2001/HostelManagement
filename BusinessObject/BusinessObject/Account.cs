using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 5)]
        public string UserEmail { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        [Required]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 9)]
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
