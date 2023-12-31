//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Billing
    {
        public Billing()
        {
            this.BillingDetails = new HashSet<BillingDetail>();
            this.UsersCreditPayments = new HashSet<UsersCreditPayment>();
        }
    
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> CampaignProfileId { get; set; }
        public Nullable<int> PaymentMethodId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public Nullable<decimal> FundAmount { get; set; }
        public Nullable<decimal> TaxPercantage { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> SettledDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    
        public virtual CampaignProfile CampaignProfile { get; set; }
        public virtual Client Client { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BillingDetail> BillingDetails { get; set; }
        public virtual ICollection<UsersCreditPayment> UsersCreditPayments { get; set; }
    }
}
