using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class HostelPic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HostelPicsId { get; set; }
        public string HostelPicUrl { get; set; }
        public int HostelId { get; set; }

        public virtual Hostel Hostel { get; set; }
    }
}
