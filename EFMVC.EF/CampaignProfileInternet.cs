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
    
    public partial class CampaignProfileInternet
    {
        public int CampaignProfileInternetId { get; set; }
        public int CampaignProfileId { get; set; }
        public string SocialNetworking { get; set; }
        public string Video { get; set; }
        public string Research { get; set; }
        public string Auctions { get; set; }
        public string Shopping { get; set; }
    
        public virtual CampaignProfile CampaignProfile { get; set; }
    }
}
