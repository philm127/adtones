using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class CampaignDashboardChartResult
    {
        public double PlaystoDate { get; set; }
        public double SpendToDate { get; set; }

        public double AverageBid { get; set; }

        public double AveragePlayTime { get; set; }

        public int FreePlays { get; set; }
        public int TotalPlayed { get; set; }
        public double FreePlaysPercantage { get; set; }

        public decimal TotalBudget { get; set; }
        public double TotalSpend { get; set; }

        public double TotalBudgetPercantage { get; set; }

        public double MaxBid { get; set; }

        public double AvgMaxBid { get; set; }

        public double MaxBidPercantage { get; set; }

        public decimal MaxPlayLength { get; set; }

        public double MaxPlayLengthPercantage { get; set; }
        public double SMSCost { get; set; }

        public double EmailCost { get; set; }

        public double Cancelled { get; set; }

        public string CurrencyCode { get; set; }

        public int TotalReach { get; set; }

        public CurrencyConvertModel currencyConvertModels { get; set; }

        public CampaignDashboardChartResult()
        {
            currencyConvertModels = new CurrencyConvertModel();
        }
    }
}