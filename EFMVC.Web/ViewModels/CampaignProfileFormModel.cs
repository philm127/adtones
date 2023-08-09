using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFMVC.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.Data.Repositories;

namespace EFMVC.Web.ViewModels
{
    public sealed class CampaignProfileFormModel
    {
       
        [Key]
        public int UserId { get; set; }

        
        public int? ClientId { get; set; }
        public int? CampaignId { get; set; }
        public int CampaignProfileId { get; set; }
      
        [Display(Name = "Campaign Name")]
        [Required]
        public string CampaignName { get; set; }

        [Display(Name = "Campaign Description")]
        [Required]
        public string CampaignDescription { get; set; }
     

        [Display(Name = "Total Budget")]
        public decimal TotalBudget { get; set; }

        [Display(Name = "Max Daily Budget")]
        public float MaxDailyBudget { get; set; }

        [Display(Name = "Max Bid")]
        public float MaxBid { get; set; }

        [Display(Name = "Available Credit")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        public float AvailableCredit { get; set; }

        [Display(Name = "Plays To Date")]
        public int PlaysToDate { get; set; }

        [Display(Name = "Plays Last Month")]
        public int PlaysLastMonth { get; set; }

        [Display(Name = "Plays Current Month")]
        public int PlaysCurrentMonth { get; set; }

        [Display(Name = "Cancelled To Date")]
        public int CancelledToDate { get; set; }

        [Display(Name = "Cancelled Last Month")]
        public int CancelledLastMonth { get; set; }

        [Display(Name = "Cancelled Current Month")]
        public int CancelledCurrentMonth { get; set; }

        [Display(Name = "SMS To Date")]
        public int SmsToDate { get; set; }

        [Display(Name = "SMS Last Month")]
        public int SmsLastMonth { get; set; }

        [Display(Name = "SMS Current Month")]
        public int SmsCurrentMonth { get; set; }

        [Display(Name = "Email To Date")]
        public int EmailToDate { get; set; }

        [Display(Name = "Emails Last Month")]
        public int EmailsLastMonth { get; set; }

        [Display(Name = "Emails Current Month")]
        public int EmailsCurrentMonth { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        [Display(Name = "Number Of Plays")]
        public int NumberOfPlays { get; set; }

        [Display(Name = "Average Daily Plays")]
        public int AverageDailyPlays { get; set; }

        [Display(Name = "SMS Requests")]
        public int SmsRequests { get; set; }

        [Display(Name = "Max Month Budget")]
        public float MaxMonthBudget { get; set; }
        [Display(Name = "Max Weekly Budget")]
        public float MaxWeeklyBudget { get; set; }
        [Display(Name = "Max Hourly Budget")]
        public float MaxHourlyBudget { get; set; }
        public float TotalCredit { get; set; }

        public float SpendToDate { get; set; }


        [Display(Name = "Emails Delivered")]
        public int EmailsDelievered { get; set; }

        [Display(Name = "Email Subject")]
        [MaxLength(250, ErrorMessage = "Maximum Length Exceeded. Email Subject cannot be more than 250 charaters")]
        public string EmailSubject { get; set; }

        [AllowHtml]
        [Display(Name = "Email Body Text")]
        [DataType(DataType.MultilineText)]
        public string EmailBody { get; set; }

        public string EmailFileLocation { get; set; }

        public string SMSFileLocation { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Email From Address")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessage = "Email address provided is not in the correct format.")]
        [DataType(DataType.EmailAddress)]
        public string EmailSenderAddress { get; set; }

       // [RegularExpression(@"^(0[\d]{12,12})$", ErrorMessage = "Please enter a valid mobile phone number.")]
        [DataType(DataType.Text)]
        [Display(Name = "SMS Originator")]
        public string SmsOriginator { get; set; }

        [AllowHtml]
        [MaxLength(160, ErrorMessage = "Maximum Length Exceeded. SMS Messages are limited to 160 characters")]
        [Display(Name = "SMS Text")]
        public string SmsBody { get; set; }

        [Display(Name = "Created Date/Time")]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Updated Date/Time")]
        public DateTime UpdatedDateTime { get; set; }

        [Display(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Number In Batch")]
        [Required]        
        public int NumberInBatch { get; set; }
        [Required]
        public int CountryId { get; set; }

        public string CurrencyCode { get; set; }

        public Client Client { get; set; }
        public User User { get; set; }
        public ICollection<AdvertFormModel> Adverts { get; set; }
      
        public ICollection<CampaignProfileAdvertFormModel> CampaignProfileAdverts { get; set; }
        public ICollection<CampaignProfileAttitudeFormModel> CampaignProfileAttitudes { get; set; }
        public ICollection<CampaignProfileCinemaFormModel> CampaignProfileCinemas { get; set; }
        public ICollection<CampaignProfileInternetFormModel> CampaignProfileInternets { get; set; }
        public ICollection<CampaignProfileMobileFormModel> CampaignProfileMobiles { get; set; }
        public ICollection<CampaignProfilePressFormModel> CampaignProfilePresses { get; set; }
        public ICollection<CampaignProfileProductsServiceFormModel> CampaignProfileProductsServices { get; set; }
        public ICollection<CampaignProfileRadioFormModel> CampaignProfileRadios { get; set; }
        public ICollection<CampaignProfileTimeSettingFormModel> CampaignProfileTimeSettings { get; set; }
        public ICollection<CampaignProfileTvFormModel> CampaignProfileTvs { get; set; }
        public ICollection<CampaignAdvertFormModel> CampaignAdverts { get; set; }
        public ICollection<CampaignProfileDateSettingsFormModel> CampaignProfileDateSettings { get; set; }
        public ICollection<CampaignProfileDemographicsFormModel> CampaignProfileDemographicsFormModels { get; set; }

        public ICollection<CampaignAuditFormModel> GetCampaignAudits(ICampaignAuditRepository auditRepo)
        {
            return auditRepo.AsQueryable().Where(a => a.CampaignProfileId == CampaignProfileId).Select(a=>Mapper.Map<CampaignAudit, CampaignAuditFormModel>(a)).ToList();
        }
        public IQueryable<CampaignAudit> GetDomainCampaignAudits(ICampaignAuditRepository auditRepo)
        {
            return auditRepo.AsQueryable().Where(a => a.CampaignProfileId == CampaignProfileId);
        }

        public  ICollection<CampaignProfilePreference> CampaignProfilePreferences { get; set; }

        //public ICollection<CampaignAuditFormModel> CampaignAudit2 { get; set; }

        public int CurrencyId { get; set; }
    }
}