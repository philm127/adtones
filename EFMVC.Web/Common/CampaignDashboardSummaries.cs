using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EFMVC.Data;
using EFMVC.Web.Core;

namespace EFMVC.Web.Common
{
    public class CampaignDashboardSummaries
    {
        public int UserId { get; set; }
        public int CampaignProfileId { get; set; }
        public int? AdvertId { get; set; }
        public string CampaignHolder { get; set; }
        public string CampaignName { get; set; }
        public string AdvertName { get; set; }
        public decimal Budget { get; set; }
        public decimal Spend { get; set; }
        public decimal AvgBid { get; set; }
        public decimal MaxBid { get; set; }
        public decimal FundsAvailable { get; set; }
        public long TotalPlays { get; set; }
        public long MoreSixSecPlays { get; set; }
        public long FreePlays { get; set; }
        public long AvgPlayLength { get; set; }
        public long MaxPlayLength { get; set; }
        public long Reach { get; set; }
        public string CurrencyCode { get; set; }

        public long TotalSMS { get; set; }
        public decimal TotalSMSCost { get; set; }
        public long TotalEmail { get; set; }
        public decimal TotalEmailCost { get; set; }
    }

    public class CampaignDashboardSummariesProvider
    {
        private readonly ICacheService _cache;

        public CampaignDashboardSummariesProvider(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task<List<CampaignDashboardSummaries>> GetCampaignDashboardSummariesForUser(int userId, int? campaignId=null)
        {
            string key = $"DASHBOARD_STATS_{userId}~{campaignId ?? 0}";
            return await _cache.GetOrSetAsync<List<CampaignDashboardSummaries>>(key, ()=>GetFromDb(userId, campaignId), TimeSpan.FromMinutes(30))
                ?? new List<CampaignDashboardSummaries>();
        }

        public async Task<List<CampaignDashboardSummaries>> GetCampaignDashboardSummariesForCampaign(int campaignId)
        {
            string key = $"DASHBOARD_STATS_CAMP_{campaignId}";
            return await _cache.GetOrSetAsync<List<CampaignDashboardSummaries>>(key, () => GetFromDb(null, campaignId), TimeSpan.FromMinutes(30))
                   ?? new List<CampaignDashboardSummaries>();
        }

        public async Task<CampaignDashboardSummaries> GetCampaignDashboardTotalReachSummaryForUser(int userId)
        {
            string key = $"DASHBOARD_STATS_TOTALREACH_{userId}";
            return await _cache.GetOrSetAsync<CampaignDashboardSummaries>(key, () => GetTotalReachFromDb(userId), TimeSpan.FromMinutes(30));
        }

        private async Task<CampaignDashboardSummaries> GetTotalReachFromDb(int userId)
        {
            using (EFMVCDataContex db =
                new EFMVCDataContex(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
            {
                try
                {
                    string query = @"select cast(count(distinct UserProfileId) as bigint) as Reach from CampaignAudit where CampaignProfileId in (select campaignProfileId from campaignProfile where UserId=@userId) and Status='Played' and PlayLengthTicks >= 6000";
                    return await db.Database.SqlQuery<CampaignDashboardSummaries>(query, new SqlParameter("@userId", SqlDbType.Int) { Value = userId }).FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed to get Dashboard Stats. Error {e}");
                    return null;
                }
            }
        }

        private async Task<List<CampaignDashboardSummaries>> GetFromDb(int? userId, int? campaignId)
        {
            using (EFMVCDataContex db =
                new EFMVCDataContex(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
            {
                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@userId", SqlDbType.Int),
                        new SqlParameter("@campaignId", SqlDbType.Int),
                    };
                    if (userId.HasValue)
                        parameters[0].Value = userId.Value;
                    else
                        parameters[0].Value = DBNull.Value;
                    if (campaignId.HasValue)
                        parameters[1].Value = campaignId;
                    else
                        parameters[1].Value = DBNull.Value;

                    return await db.Database.SqlQuery<CampaignDashboardSummaries>("exec GetCampaignDashboardSummaries @userId, @campaignId", parameters[0], parameters[1]).ToListAsync();
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Failed to get Dashboard Stats. Error {e}");
                    return null;
                }
            }
        }
    }

    public static class SummariesExtentionss
    {
        public static decimal TotalBudget(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion=null)
        {
            if (currencyConversion == null)
                return summarieses.Sum(s => s.Budget);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Sum(s=>s.Budget.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Sum(s=>s.Budget).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }

        public static decimal TotalSpend(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion = null)
        {
            if (currencyConversion == null)
                return summarieses.Sum(s => s.Spend);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Sum(s => s.Spend.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Sum(s => s.Spend).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }

        public static decimal TotalBudgetPercentage(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            var totalBudget = summarieses.TotalBudget();
            return totalBudget == 0 ? 0 : Math.Round(Convert.ToDecimal(summarieses.TotalSpend() / totalBudget)*100, 2, MidpointRounding.ToEven);
        }

        public static decimal AverageBid(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion = null)
        {
            if (currencyConversion == null)
                return summarieses.Average(s => s.AvgBid);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Where(s=>s.Spend > 0M).Average(s => s.AvgBid.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Where(s => s.Spend > 0M).Average(s => s.AvgBid).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }

        public static long TotalPlays(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.TotalPlays);
        }
        
        public static long TotalReach(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.Reach);
        }

        public static long TotalFreePlays(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.FreePlays);
        }
        
        public static long TotalValuablePlays(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.MoreSixSecPlays);
        }

        public static decimal FreePlaysPercentage(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            var totalPlays = summarieses.TotalPlays();
            return totalPlays == 0 ? 0 : Math.Round(Convert.ToDecimal((decimal)summarieses.TotalFreePlays() / totalPlays)*100, 2, MidpointRounding.ToEven);
        }

        public static decimal AveragePlayLengthSeconds(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return Math.Round(Convert.ToDecimal(summarieses.Average(s=>s.AvgPlayLength)/1000), 2, MidpointRounding.ToEven);
        }
        
        public static decimal MaxPlayLengthSeconds(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return Math.Round(Convert.ToDecimal(summarieses.Max(s=>s.AvgPlayLength)/1000), 2, MidpointRounding.ToEven);
        }

        public static decimal TotalMaxBid(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion = null)
        {
            if (currencyConversion == null)
                return summarieses.Max(s => s.MaxBid);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Max(s => s.MaxBid.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Max(s => s.MaxBid).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }

        public static decimal TotalSmsCost(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion = null)
        {
            if (currencyConversion == null)
                return summarieses.Sum(s => s.TotalSMSCost);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Sum(s => s.TotalSMSCost.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Sum(s => s.TotalSMSCost).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }
        public static decimal TotalEmailCost(this IEnumerable<CampaignDashboardSummaries> summarieses, CurrencyConversion currencyConversion = null)
        {
            if (currencyConversion == null)
                return summarieses.Sum(s => s.TotalEmailCost);
            List<string> currenciesUsed = summarieses.Select(s => s.CurrencyCode).Distinct().ToList();
            if (currenciesUsed.Count > 1)
                return summarieses.Sum(s => s.TotalEmailCost.ConvertToDisplay(currencyConversion, s.CurrencyCode));
            return summarieses.Sum(s => s.TotalEmailCost).ConvertToDisplay(currencyConversion, currenciesUsed[0]);
        }

        public static long TotalSms(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.TotalSMS);
        }

        public static long TotalEmail(this IEnumerable<CampaignDashboardSummaries> summarieses)
        {
            return summarieses.Sum(s => s.TotalEmail);
        }
    }
}