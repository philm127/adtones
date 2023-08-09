using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class Reward
    {
        public int RewardId { get; set; }
        public string RewardName { get; set; }
        public string RewardValue { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
      
        public int OperatorId { get; set; }
        public int? AdtoneServerRewardId { get; set; }
        public virtual Operator Operator { get; set; }
    }
}
