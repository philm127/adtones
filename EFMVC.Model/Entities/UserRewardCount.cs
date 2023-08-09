using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class UserRewardCount
    {
        public int UserRewardCountId { get; set; }

        public int UserId { get; set; }

        public string MSISDN { get; set; }

        public int Count { get; set; }

        public DateTime AddedDate { get; set; }

        public virtual User User { get; set; }
    }
}
