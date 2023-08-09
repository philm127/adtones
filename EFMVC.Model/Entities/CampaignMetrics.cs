using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class CampaignMetrics
    {
        public int CampaignMetricsId { get; set; }
        public float PlaystoDate { get; set; }
        public double SpendToDate { get; set; }
        public double SMSCost { get; set; }
        public double EmailCost { get; set; }
        public double AverageBid { get; set; }
        public int TotalPlayed { get; set; }
        public double MaxBid { get; set; }
        public double AvgMaxBid { get; set; }
        public float TotalBudget { get; set; }
        public int FreePlays { get; set; }
        public double AveragePlayTime { get; set; }
        public double MaxPlayLength { get; set; }
        public double MaxPlayLengthPercantage { get; set; }
        public double MaxBidPercantage { get; set; }
        public double SMSCampaignCost { get; set; }
        public double EmailCampaignCost { get; set; }
        public double CancelledCampaignCost { get; set; }
        public double Cancelled { get; set; }
        public double TotalBudgetPercantage { get; set; }
        public double FreePlaysPercantage { get; set; }
        public int UserId { get; set; }
    }
}
