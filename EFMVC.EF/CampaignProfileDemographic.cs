//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampaignProfileDemographic
    {
        public int CampaignProfileDemographicsId { get; set; }
        public int CampaignProfileId { get; set; }
        public Nullable<System.DateTime> DOBStart { get; set; }
        public Nullable<System.DateTime> DOBEnd { get; set; }
        public string Gender { get; set; }
        public string IncomeBracket { get; set; }
        public string WorkingStatus { get; set; }
        public string RelationshipStatus { get; set; }
        public string Education { get; set; }
        public string HouseholdStatus { get; set; }
        public string Location { get; set; }
        public string Age { get; set; }
    
        public virtual CampaignProfile CampaignProfile { get; set; }
    }
}
