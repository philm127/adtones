//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.ProvisioningModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class campaignmatchusermatch
    {
        public int Id { get; set; }
        public int CampaignProfileId { get; set; }
        public int UserProfileId { get; set; }
    
        public virtual usermatch usermatch { get; set; }
        public virtual campaignmatch campaignmatch { get; set; }
    }
}