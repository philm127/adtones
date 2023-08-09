﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class CampaignAdminResult
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int AdvertId { get; set; }
        public string AdvertName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
      
        public decimal TotalBudget { get; set; }
        public int finaltotalplays { get; set; }
        public decimal FundsAvailable { get; set; }
        public decimal totalaveragebid { get; set; }
        public decimal totalspend { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string DisplayCreatedDateTime { get; set; }

        public int TicketCount { get; set; }
        public bool IsAdminApproval { get; set; }

    }
}