using System;
using System.Collections.Generic;

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
        public string RoomTitle { get; set; }
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
