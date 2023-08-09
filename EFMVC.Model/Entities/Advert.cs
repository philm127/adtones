using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class Advert
    {
        public Advert()
        {
            this.CampaignAdverts = new HashSet<CampaignAdvert>();
           
        }

        [Key]
        [Display(Name = "ID")]
        public int AdvertId { get; set; }

        public  int UserId { get; set; }

        public int? ClientId { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "Name")]
        public string AdvertName { get; set; }

        [Display(Name = "Description")]
        public string AdvertDescription { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Script")]
        public string Script { get; set; }

        [Display(Name = "ScriptFileLocation")]
        public string ScriptFileLocation { get; set; }

        [Display(Name = "File")]
        public string MediaFileLocation { get; set; }

        public bool UploadedToMediaServer { get; set; }

        [Display(Name = "Created Date/Time")]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Updated Date/Time")]
        public DateTime UpdatedDateTime { get; set; }

        public virtual ICollection<CampaignAdvert> CampaignAdverts { get; set; }

        public virtual Client Clients { get; set; }

        public int Status { get; set; }
        public bool IsAdminApproval { get; set; }

        [StringLength(50)]
        public string SoapToneId { get; set; }

        public int? AdvertCategoryId { get; set; }
        public virtual AdvertCategory AdvertCategory { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool NextStatus { get; set; }

        public int? CampProfileId { get; set; }

        [StringLength(100)]
        public string SoapToneCode { get; set; }

        public int? UpdatedBy { get; set; }

        public int? OperatorId { get; set; }
        public virtual Operator Operator { get; set; }

        public int? AdtoneServerAdvertId { get; set; }
    }
}
