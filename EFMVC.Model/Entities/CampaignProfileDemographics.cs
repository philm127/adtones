    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class CampaignProfileDemographics
    {
        [Key]
        public int CampaignProfileDemographicsId { get; set; }

        public int CampaignProfileId { get; set; }
        public DateTime? DOBStart { get; set; }
        public DateTime? DOBEnd { get; set; }

        [StringLength(50)]
        public string Age { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string IncomeBracket { get; set; }

        [StringLength(50)]
        public string WorkingStatus { get; set; }

        [StringLength(50)]
        public string RelationshipStatus { get; set; }

        [StringLength(50)]
        public string Education { get; set; }

        [StringLength(50)]
        public string HouseholdStatus { get; set; }

        [StringLength(50)]
        public string Location { get; set; }
    }
}
