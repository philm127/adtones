using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model {
    public class UserProfile {
        public UserProfile()
        {
            this.UserProfileAdverts = new HashSet<UserProfileAdvert>();
            this.UserProfileCinemas = new HashSet<UserProfileCinema>();
            this.UserProfileInternets = new HashSet<UserProfileInternet>();
            this.UserProfileMobiles = new HashSet<UserProfileMobile>();
            this.UserProfilePresses = new HashSet<UserProfilePress>();
            this.UserProfileProductsServices = new HashSet<UserProfileProductsService>();
            this.UserProfileRadios = new HashSet<UserProfileRadio>();
            this.UserProfileTimeSettings = new HashSet<UserProfileTimeSetting>();
            this.UserProfileTvs = new HashSet<UserProfileTv>();
            this.UserProfileAttitudes = new HashSet<UserProfileAttitude>();
            this.UserProfileCreditsReceived = new HashSet<UserProfileCreditsReceived>();
            this.UserProfileAdvertsReceived = new HashSet<UserProfileAdvertsReceived>();
            this.UserProfilePreferences = new HashSet<UserProfilePreference>();
            
        }

        [Key]
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string IncomeBracket { get; set; }
        public string WorkingStatus { get; set; }
        public string RelationshipStatus { get; set; }
        public string Education { get; set; }
        public string HouseholdStatus { get; set; }
        public string Location { get; set; }
        public string MSISDN { get; set; }
        public int? AdtoneServerUserProfileId { get; set; }
        //public string Postcode { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<UserProfileAdvert> UserProfileAdverts { get; set; }
        public virtual ICollection<UserProfileCinema> UserProfileCinemas { get; set; }
        public virtual ICollection<UserProfileInternet> UserProfileInternets { get; set; }
        public virtual ICollection<UserProfileMobile> UserProfileMobiles { get; set; }
        public virtual ICollection<UserProfilePress> UserProfilePresses { get; set; }
        public virtual ICollection<UserProfileProductsService> UserProfileProductsServices { get; set; }
        public virtual ICollection<UserProfileRadio> UserProfileRadios { get; set; }
        public virtual ICollection<UserProfileTimeSetting> UserProfileTimeSettings { get; set; }
        public virtual ICollection<UserProfileTv> UserProfileTvs { get; set; }

        public virtual ICollection<UserProfilePreference> UserProfilePreferences { get; set; }
        public virtual ICollection<UserProfileAttitude> UserProfileAttitudes { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived> UserProfileAdvertsReceived { get; set; }
        public virtual ICollection<UserProfileCreditsReceived> UserProfileCreditsReceived { get; set; }

        public virtual ICollection<UserProfileAdvertsReceived2> UserProfileAdvertsReceived2 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived3> UserProfileAdvertsReceived3 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived4> UserProfileAdvertsReceived4 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived5> UserProfileAdvertsReceived5 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived6> UserProfileAdvertsReceived6 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived7> UserProfileAdvertsReceived7 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived8> UserProfileAdvertsReceived8 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived9> UserProfileAdvertsReceived9 { get; set; }
        public virtual ICollection<UserProfileAdvertsReceived10> UserProfileAdvertsReceived10 { get; set; }
    }
}
