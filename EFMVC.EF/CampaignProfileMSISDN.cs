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
    
    public partial class CampaignProfileMSISDN
    {
        public int CampaignProfileMSISDNId { get; set; }
        public int CampaignProfileReportId { get; set; }
        public string MSISDN { get; set; }
    
        public virtual CampaignProfileReport CampaignProfileReport { get; set; }
    }
}
