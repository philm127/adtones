using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class UserRewardHistory
    {
        public int UserRewardHistoryId { get; set; }

        public int UserId { get; set; }

        public Nullable<int> RewardId { get; set; }

        public int EarnedReward { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string MSISDN { get; set; }

        public int Proceed { get; set; }

        public virtual User User { get; set; }
    }
}
