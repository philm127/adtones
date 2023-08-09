using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class CampaignProfileDateSettings
    {
        [Key]
        public int CampaignDateSettingsId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }
        public DateTime CampaignDate { get; set; }
        public bool Active { get; set; }
    }
}
