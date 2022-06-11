using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.BusinessObject
{
    public partial class IdentityCard
    {
        public IdentityCard()
        {
            Accounts = new HashSet<Account>();
        }

        public string IdCardNumber { get; set; }
        public string FrontIdPicUrl { get; set; }
        public string BackIdPicUrl { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
