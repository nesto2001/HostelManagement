using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class IdentityCard
    {
        public IdentityCard()
        {
            Accounts = new HashSet<Account>();
        }
        //[Required(ErrorMessage = "Identity Card is required!")]
        [Display(Name = "Identity Card Number")]
        public string IdCardNumber { get; set; }
        //[Required(ErrorMessage = "Identity Card is required!")]
        [Display(Name = "Front Image")]
        public string FrontIdPicUrl { get; set; }
        //[Required(ErrorMessage = "Identity Card is required!")]
        [Display(Name = "Back Image")]
        public string BackIdPicUrl { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
