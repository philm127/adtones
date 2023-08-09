using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Model;
using EFMVC.Web.CustomValidation;

namespace EFMVC.Web.ViewModels
{
    public class ClientModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Name contains invalid character.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ContactInfo { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
       
        public int Status { get; set; }

        /*[RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Invalid Budget Format.")]*/
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Budget Format.")]
        [Required]
        public decimal Budget { get; set; }
        public int CountryId { get; set; }
        public virtual ICollection<CampaignProfile> CampaignProfiles { get; set; }

        public List<SelectListItem> StatusListItem = new List<SelectListItem>();

        public ClientModel()
        {
            StatusListItem = new List<SelectListItem>();
        }

        public int CurrencyId { get; set; }
    }
}