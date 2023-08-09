using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.SearchClass
{
    public class BillingFilter
    {
        public string Id { get; set; }
        public string InvoiceNO { get; set; }
        public string PONumber { get; set; }

        public string ClienId { get; set; }
        //public DateTime ? Fromdate { get; set; }
        //public DateTime ? Todate { get; set; }
        //public DateTime ? SettedFromdate { get; set; }
        //public DateTime ? SettedTodate { get; set; }

        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string SettedFromdate { get; set; }
        public string SettedTodate { get; set; }
        public string InvoiceFromTotal { get; set; }

        public string InvoiceToTotal { get; set; }

        public string Status { get; set; }

        public string MethodOfPayment { get; set; }
    }
}