using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class Hostel
    {
        public Hostel()
        {
            HostelPics = new HashSet<HostelPic>();
            Rooms = new HashSet<Room>();
        }

        public int HostelId { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Hostel Name")]
        public string HostelName { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be {2} - {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }
        [Range(0, 4, ErrorMessage = "Only 0-4")]
        public int? Status { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public string HostelOwnerEmail { get; set; }
        public int? LocationId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Account HostelOwnerEmailNavigation { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<HostelPic> HostelPics { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
