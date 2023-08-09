using EFMVC.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EFMVC.Web.Areas.Users.Models
{
    public class UserProfileAdvertiserResult
    {
        public UserProfileAdvertAdvertiserFormModel UserProfileAdvertAdvertiserFormModel { get; set; }

        public UserProfileSkizaProfileAdvertiserFormModel UserProfileSkizaProfileAdvertiserFormModel { get; set; }

        public UserProfileMobileAdvertiserFormModel UserProfileMobileAdvertiserFormModel { get; set; }

        public UserProfileTimesettingAdvertiserFormModel UserProfileTimesettingAdvertiserFormModel { get; set; }

        public UserProfileDemographicAdvertiserFormModel UserProfileDemographicAdvertiserFormModel { get; set; }

        public UserProfileAdvertiserResult()
        {
            UserProfileAdvertAdvertiserFormModel = new UserProfileAdvertAdvertiserFormModel();
            UserProfileSkizaProfileAdvertiserFormModel = new UserProfileSkizaProfileAdvertiserFormModel();
            UserProfileMobileAdvertiserFormModel = new UserProfileMobileAdvertiserFormModel();
            UserProfileTimesettingAdvertiserFormModel = new UserProfileTimesettingAdvertiserFormModel();
            UserProfileDemographicAdvertiserFormModel = new UserProfileDemographicAdvertiserFormModel();
        }
    }
}