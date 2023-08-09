using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.Models
{
    public class PaymentModel
    {
        public BillingInfoDetails BillingInfoDetails { get; set; }
        public BillingPaymentInfoDetails BillingPaymentInfoDetailswithPaypalCreditCard { get; set; }

        public BillingPaymentInfoDetailsWithPaypal BillingPaymentInfoDetailswithPaypal { get; set; }

        public SagePayBillingDetails SagePayBillingDetails { get; set; }

        public BillingPaymentInfoDetailsWithMpesa MpesaBillingDetails { get; set; }
    }
}