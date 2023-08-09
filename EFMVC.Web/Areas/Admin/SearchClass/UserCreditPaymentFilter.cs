using System;

namespace EFMVC.Web.Areas.Admin.SearchClass
{
    public class UserCreditPaymentFilter
    {
        public string InvoiceNumber { get; set; }

        public string Fromamount { get; set; }

        public string Toamount { get; set; }


        //public DateTime? Fromdate { get; set; }
        //public DateTime? Todate { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }
    }
}