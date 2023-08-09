using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class CampaignAuditResult
    {
        public int PlayID { get; set; }
        public int UserID { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public double LengthOfPlay { get; set; }

        public int AdvertId { get; set; }

        public string AdvertName { get; set; }

        public string Status { get; set; }

        public double PlayCost { get; set; }

        public string SMS { get; set; }

        public double SMSCost { get; set; }

        public string Email { get; set; }

        public double EmailCost { get; set; }
        public double TotalCost { get; set; }

        public DateTime DisplayStartDateSort { get; set; }
        public string DisplayStartDate { get; set; }

        public DateTime DisplayEndDateSort { get; set; }
        public string DisplayEndDate { get; set; }

        public string CurrencyCode { get; set; }

        public int? CountryId { get; set; }

        public int? CurrencyId { get; set; }
    }

    public class CampaignAuditResultTR
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string DateTimeFormatJS = "DD/MM/YYYY HH:mm:ss";
        public int PlayID { get; set; }
        public int UserID { get; set; }
        public long LengthOfPlay { get; set; }
        public string AdvertName { get; set; }
        public double PlayCost { get; set; }
        public string SMS { get; set; }
        public double SMSCost { get; set; }
        public string Email { get; set; }
        public double EmailCost { get; set; }
        public double TotalCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DisplayStartDate => DateTime.SpecifyKind(StartDate, DateTimeKind.Utc).ToString("O");
        public string DisplayEndDate => DateTime.SpecifyKind(EndDate, DateTimeKind.Utc).ToString("O");
        public string CurrencyCode { get; set; }
        public int? CurrencyId { get; set; }
    }
}