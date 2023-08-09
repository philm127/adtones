using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class UserSMSCount
    {
        [Key]
        public int UserSMSCountId { get; set; }

        public int UserId { get; set; }

        public string MSISDN { get; set; }

        public int SubscribeSMSCount { get; set; }

        public int UnSubscribeSMSCount { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int RewardSMSCount { get; set; }

        public int CreditReceivedSMSCount { get; set; }

        public virtual User User { get; set; }
    }
}
