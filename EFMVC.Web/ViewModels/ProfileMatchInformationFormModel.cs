using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class ProfileMatchInformationFormModel
    {
        
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string ProfileName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public string ProfileType { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ProfileMatchLabelFormModel profileMatchLabelFormModel { get; set; }
        public List<ProfileMatchLabelFormModel> profileMatchLabelFormModels { get; set; }

        public ProfileMatchInformationFormModel()
        {
            profileMatchLabelFormModel = new ProfileMatchLabelFormModel();
            profileMatchLabelFormModels = new List<ProfileMatchLabelFormModel>();
        }

    }
}