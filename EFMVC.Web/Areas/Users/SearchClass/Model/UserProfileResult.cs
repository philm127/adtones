using EFMVC.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EFMVC.Web.Areas.Users.Models
{
    public class UserProfileResult
    {
        public IEnumerable<UserProfileFormModel> userProfileFormModels { get; set; }
        public UserProfileFormModel userProfileModel { get; set; }
        public UserProfileAdvertFormModel UserProfileAdvertFormModel { get; set; }
        public UserProfileAttitudeFormModel UserProfileAttitudeFormModel { get; set; }

        public UserProfileCinemaFormModel UserProfileCinemaFormModel { get; set; }

        public UserProfileInternetFormModel UserProfileInternetFormModel { get; set; }

        public UserProfileMobileFormModel UserProfileMobileFormModel { get; set; }

        public UserProfilePressFormModel UserProfilePressFormModel { get; set; }

        public UserProfileProductsServiceFormModel UserProfileProductsServiceFormModel { get; set; }

        public UserProfileRadioFormModel UserProfileRadioFormModel { get; set; }

        public UserProfileTvFormModel UserProfileTvFormModel { get; set; }

        public UserProfileFormModel UserProfileFormModel { get; set; }

        public SkizaProfileFormModel SkizaProfileFormModel { get; set; }

        public UserProfileTimeSettingFormModel UserProfileTimeSettingFormModel { get; set; }
        public IEnumerable<UserProfileAdvertsReceivedFromModel> UserProfileAdvertsReceivedFromModel { get; set; }

        //Add 27-02-2019
        public UserProfileDemographicFormModel UserProfileDemographicFormModel { get; set; }

        public UserProfileResult()
        {
            UserProfileAdvertFormModel = new UserProfileAdvertFormModel();
            SkizaProfileFormModel = new SkizaProfileFormModel();
            UserProfileMobileFormModel = new UserProfileMobileFormModel();
            UserProfileTimeSettingFormModel = new UserProfileTimeSettingFormModel();
            UserProfileDemographicFormModel = new UserProfileDemographicFormModel();
        }
    }
}