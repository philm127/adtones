using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFMVC.Model;

namespace EFMVC.Web.ViewModels
{
    public class UsersCreditPaymentFormModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BillingId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? CampaignProfileId { get; set; }

       
    }
}