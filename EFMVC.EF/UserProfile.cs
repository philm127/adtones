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
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.CampaignProfileUserProfiles = new HashSet<CampaignProfileUserProfile>();
            this.UserProfileAdverts = new HashSet<UserProfileAdvert>();
            this.UserProfileAdvertsReceiveds = new HashSet<UserProfileAdvertsReceived>();
            this.UserProfileAttitudes = new HashSet<UserProfileAttitude>();
            this.UserProfileCinemas = new HashSet<UserProfileCinema>();
            this.UserProfileCreditsReceiveds = new HashSet<UserProfileCreditsReceived>();
            this.UserProfileInternets = new HashSet<UserProfileInternet>();
            this.UserProfileMobiles = new HashSet<UserProfileMobile>();
            this.UserProfilePresses = new HashSet<UserProfilePress>();
            this.UserProfileProductsServices = new HashSet<UserProfileProductsService>();
            this.UserProfileRadios = new HashSet<UserProfileRadio>();
            this.UserProfileTimeSettings = new HashSet<UserProfileTimeSetting>();
            this.UserProfileTvs = new HashSet<UserProfileTv>();
        }
    
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string IncomeBracket { get; set; }
        public string WorkingStatus { get; set; }
        public string RelationshipStatus { get; set; }
        public string Education { get; set; }
        public string HouseholdStatus { get; set; }
        public string Location { get; set; }
        public string MSISDN { get; set; }
    
        public virtual ICollection<CampaignProfileUserProfile> CampaignProfileUserProfiles { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserProfileAdvert> UserProfileAdverts { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived> UserProfileAdvertsReceiveds { get; set; }
        public virtual ICollection<UserProfileAttitude> UserProfileAttitudes { get; set; }
        public virtual ICollection<UserProfileCinema> UserProfileCinemas { get; set; }
        public virtual ICollection<UserProfileCreditsReceived> UserProfileCreditsReceiveds { get; set; }
        public virtual ICollection<UserProfileInternet> UserProfileInternets { get; set; }
        public virtual ICollection<UserProfileMobile> UserProfileMobiles { get; set; }
        public virtual ICollection<UserProfilePress> UserProfilePresses { get; set; }
        public virtual ICollection<UserProfileProductsService> UserProfileProductsServices { get; set; }
        public virtual ICollection<UserProfileRadio> UserProfileRadios { get; set; }
        public virtual ICollection<UserProfileTimeSetting> UserProfileTimeSettings { get; set; }
        public virtual ICollection<UserProfileTv> UserProfileTvs { get; set; }
    }
}
