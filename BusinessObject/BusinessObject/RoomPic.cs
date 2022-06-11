using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class RoomPic
    {
        public int RoomPicsId { get; set; }
        public string RoomPicUrl { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
