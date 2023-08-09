using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.ViewModel
{
    public class PromotionalAdvertFormModel
    {
        public int ID { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
    }
}