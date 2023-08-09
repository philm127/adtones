﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class CampaignCreditResult
    {
        public int CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int CreditPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}