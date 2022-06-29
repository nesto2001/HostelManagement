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
        public string HostelName { get; set; }
        public string Description { get; set; }
        [Range(0, 1, ErrorMessage = "Only 0 or 1")]
        public int? Status { get; set; }
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
