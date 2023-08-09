using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class UserReward
    {
        public int UserRewardId { get; set; }

        public int UserId { get; set; }

        public int? RewardId { get; set; }

        public int? AdtoneServerUserRewardId { get; set; }

        public virtual User User { get; set; }

        public virtual Reward Reward { get; set; }
    }
}
