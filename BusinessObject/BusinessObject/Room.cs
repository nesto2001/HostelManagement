using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Room
    {
        public Room()
        {
            Rents = new HashSet<Rent>();
            RoomPics = new HashSet<RoomPic>();
        }

        public int RoomId { get; set; }
        public int HostelId { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Room Title")]
        public string RoomTitle { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 10)]
        public string RoomDescription { get; set; }
        public int RomMaxCapacity { get; set; }
        public int? RoomCurrentCapacity { get; set; }
        public int Status { get; set; }
        public decimal? Deposit { get; set; }
        public decimal Price { get; set; }

        public virtual Hostel Hostel { get; set; }
        public virtual ICollection<Rent> Rents { get; set; }
        public virtual ICollection<RoomPic> RoomPics { get; set; }
    }
}
