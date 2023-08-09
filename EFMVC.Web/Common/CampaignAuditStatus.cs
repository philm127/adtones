using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public enum CampaignAuditStatus
    {
        Played = 1,
        Cancelled = 2,
        Short = 3
    }

    public enum CampaignAuditSMSStatus
    {
        Yes = 1,
        No = 2,
        Both = 3

    }
    
    public static class CampaignAuditStatusExtensions
    {
        public static string PlayedAsLowerCase = CampaignAuditStatus.Played.ToLowerString();
        public static string CancelledAsLowerCase = CampaignAuditStatus.Cancelled.ToLowerString();
        public static string ToLowerString(this CampaignAuditStatus status)
        {
            return status.ToString().ToLowerInvariant();
        }
    }

}