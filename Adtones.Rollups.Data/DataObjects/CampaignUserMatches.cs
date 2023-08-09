using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adtones.Rollups.Data.DataObjects
{
    public class CampaignUserMatches : LeveledStatsObjectDaoBase
    {
        public int MatchedUsers { get; set; }
        public int CampaignProfileId { get; set; }
    }
}
