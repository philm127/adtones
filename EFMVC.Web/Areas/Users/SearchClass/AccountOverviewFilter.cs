using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Users.SearchClass
{
    public class AccountOverviewFilter
    {
        public string AdvertRef { get; set; }
        public string AdvertName { get; set; }

        public string Brand { get; set; }
        //public DateTime ? FromDateTimePlayed { get; set; }
        //public DateTime ? ToDateTimePlayed { get; set; }
        public string FromDateTimePlayed { get; set; }
        public string ToDateTimePlayed { get; set; }
        public string FromCreditReceived { get; set; }
        public string ToCreditReceived { get; set; }
    }
}