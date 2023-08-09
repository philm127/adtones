using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class ManagementReportModel
    {
        public int NumOfTextFile { get; set; }
        public int NumOfTextLine { get; set; }
        public int NumOfUpdateToAudit { get; set; }
        public int NumOfPlay { get; set; }
        public int NumOfPlayUnder6secs { get; set; }
        public int NumOfSMS { get; set; }
        public int NumOfEmail { get; set; }
        public int NumOfTotalUser { get; set; }
        public int NumOfRemovedUser { get; set; }
        public int NumOfLiveCampaign { get; set; }
        public int NumberOfAdsProvisioned { get; set; }
        public double TotalSpend { get; set; }
        public double TotalCredit { get; set; }
        public int NumOfCancel { get; set; }

        public int AveragePlaysPerUser { get; set; }
    }
}