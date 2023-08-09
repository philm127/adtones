using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class CampaignProfile
    {
        public CampaignProfile()
        {
            CampaignProfileAdverts = new HashSet<CampaignProfileAdvert>();
            CampaignProfileCinemas = new HashSet<CampaignProfileCinema>();
            CampaignProfileInternets = new HashSet<CampaignProfileInternet>();
            CampaignProfileMobiles = new HashSet<CampaignProfileMobile>();
            CampaignProfilePresses = new HashSet<CampaignProfilePress>();
            CampaignProfileProductsServices = new HashSet<CampaignProfileProductsService>();
            CampaignProfileRadios = new HashSet<CampaignProfileRadio>();
            CampaignProfileTimeSettings = new HashSet<CampaignProfileTimeSetting>();
            CampaignProfileTvs = new HashSet<CampaignProfileTv>();
            CampaignProfileAttitudes = new HashSet<CampaignProfileAttitude>();
            CampaignAdverts = new HashSet<CampaignAdvert>();
            Adverts = new HashSet<Advert>();
            CampaignProfileDemographics = new HashSet<CampaignProfileDemographics>();
            CampaignAudits = new HashSet<CampaignAudit>();
            CampaignProfilePreferences = new HashSet<CampaignProfilePreference>();
        }

        [Key]
        public int CampaignProfileId { get; set; }

        public int UserId { get; set; }

        public int? ClientId { get; set; }

        public string CampaignName { get; set; }
        public string CampaignDescription { get; set; }

        
        public decimal TotalBudget { get; set; }
        public float MaxDailyBudget { get; set; }
        public float MaxBid { get; set; }

        public float MaxMonthBudget { get; set; }

        public float MaxWeeklyBudget { get; set; }
        public float MaxHourlyBudget { get; set; }
        public decimal TotalCredit { get; set; }

        public float SpendToDate { get; set; }
        public decimal AvailableCredit { get; set; }
        public int PlaysToDate { get; set; }
        public int PlaysLastMonth { get; set; }
        public int PlaysCurrentMonth { get; set; }
        public int CancelledToDate { get; set; }
        public int CancelledLastMonth { get; set; }
        public int CancelledCurrentMonth { get; set; }
        public int SmsToDate { get; set; }
        public int SmsLastMonth { get; set; }
        public int SmsCurrentMonth { get; set; }
        public int EmailToDate { get; set; }
        public int EmailsLastMonth { get; set; }
        public int EmailsCurrentMonth { get; set; }
        public bool Active { get; set; }
        public int NumberOfPlays { get; set; }
        public int AverageDailyPlays { get; set; }
        public int SmsRequests { get; set; }
        public int EmailsDelievered { get; set; }
        public string EmailSubject { get; set; }

        public string EmailFileLocation { get; set; }
        public int Status { get; set; }

        [DataType(DataType.MultilineText)]
        public string EmailBody { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailSenderAddress { get; set; }

        public string SmsOriginator { get; set; }
        public string SmsBody { get; set; }

        public string SMSFileLocation { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int NumberInBatch { get; set; }

        public int? CountryId { get; set; }
        public bool IsAdminApproval { get; set; }

        public float RemainingMaxMonthBudget { get; set; }
        public float RemainingMaxDailyBudget { get; set; }
        public float RemainingMaxWeeklyBudget { get; set; }
        public float RemainingMaxHourlyBudget { get; set; }
        public decimal ProvidendSpendAmount { get; set; }
        public int BucketCount { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool NextStatus { get; set; }
        //public string ConnectionString { get; set; }

        public string CurrencyCode { get; set; }

        public int? AdtoneServerCampaignProfileId { get; set; }
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<CampaignProfileAdvert> CampaignProfileAdverts { get; set; }
        public virtual ICollection<CampaignProfileCinema> CampaignProfileCinemas { get; set; }
        public virtual ICollection<CampaignProfileInternet> CampaignProfileInternets { get; set; }
        public virtual ICollection<CampaignProfileMobile> CampaignProfileMobiles { get; set; }
        public virtual ICollection<CampaignProfilePress> CampaignProfilePresses { get; set; }
        public virtual ICollection<CampaignProfileProductsService> CampaignProfileProductsServices { get; set; }
        public virtual ICollection<CampaignProfileRadio> CampaignProfileRadios { get; set; }
        public virtual ICollection<CampaignProfileTimeSetting> CampaignProfileTimeSettings { get; set; }
        public virtual ICollection<CampaignProfileTv> CampaignProfileTvs { get; set; }
        public virtual ICollection<CampaignProfileAttitude> CampaignProfileAttitudes { get; set; }
        public virtual ICollection<CampaignAdvert> CampaignAdverts { get; set; }
        public virtual ICollection<CampaignProfileDateSettings> CampaignProfileDateSettings { get; set; }
        public virtual ICollection<CampaignProfileDemographics> CampaignProfileDemographics { get; set; }
        public virtual ICollection<CampaignAudit> CampaignAudits { get; set; }

       // public virtual ICollection<CampaignAudit2> CampaignAudit2 { get; set; }

        public virtual ICollection<CampaignProfilePreference> CampaignProfilePreferences { get; set; }
    }
}
