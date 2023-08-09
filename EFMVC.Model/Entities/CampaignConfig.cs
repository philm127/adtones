using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class CampaignConfig
    {
        [Key]
        public int Id { get; set; }
        public int CampaignID { get; set; }
        public string GravityID { get; set; }
        public string CampaignText { get; set; }
        public string UserText { get; set; }
    }
}
