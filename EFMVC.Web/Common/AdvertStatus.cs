using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public enum AdvertStatus
    {
        Live = 1,
        Suspended = 2,
        Archived = 3,
        Waitingforapproval=4,
        Rejected=5,
        Draft = 6,
        InProgress = 7,
        CampaignPausedDueToInsufficientFunds = 8,
        Pending = 9
    }
   
}