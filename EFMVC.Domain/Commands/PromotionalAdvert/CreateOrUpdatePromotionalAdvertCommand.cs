using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdatePromotionalAdvertCommand : ICommand
    {
        public int ID { get; set; }
        public Nullable<int> CampaignID { get; set; }

        public Nullable<int> OperatorID { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int? AdtoneServerPromotionalAdvertId { get; set; }
    }
}
