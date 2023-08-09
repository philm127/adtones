using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class NewAdvertFormModel
    {
        [Key]
        public int AdvertId { get; set; }

        public int UserId { get; set; }

        //[Required(ErrorMessage = "The Client field is required.")]
        public int? AdvertClientId { get; set; }

        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [Required(ErrorMessage = "The Advert Name field is required.")]
        [Display(Name = "Advert Name")]
        public string AdvertName { get; set; }

        [Required(ErrorMessage = "The Advert Category field is required.")]
        public int AdvertCategoryId { get; set; }

        [Required(ErrorMessage = "Please upload media file.")]
        public string UploadMediaFile { get; set; }

        public string UploadScriptFile { get; set; }

        [Display(Name = "Script")]
        public string Script { get; set; }

        public string Numberofadsinabatch { get; set; }

        public string PhoneticAlphabet { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please Accept Terms & Condition.")]
        public bool IsTermChecked { get; set; }


        public string ScriptFileLocation { get; set; }

        public string MediaFileLocation { get; set; }

        public bool UploadedToMediaServer { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public int Status { get; set; }

        public bool IsAdminApproval { get; set; }

        public int CountryId { get; set; }

        public int SoapToneId { get; set; }

        public bool NextStatus { get; set; }

        public int? CampProfileId { get; set; }

        public int? AdtoneServerAdvertId { get; set; }

        [Required(ErrorMessage = "The Operator field is required.")]
        public int OperatorId { get; set; }
    }
}