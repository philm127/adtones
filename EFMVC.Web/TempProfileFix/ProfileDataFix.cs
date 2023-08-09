using EFMVC.Data.Repositories;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.TempProfileFix
{
    public class ProfileDataFix// : IProfileDataFix
    {
        private readonly ICampaignProfilePreferenceRepository _profilePreferenceRepository;

        public ProfileDataFix(ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository)
        {
            _profilePreferenceRepository = campaignProfilePreferenceRepository;
        }
        public static string GetLocationData(Controller controller, ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository, int Id, UserProfileDemographicAdvertiserFormModel userProfileDemographic)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Location_Demographics).FirstOrDefault();
            var userLocation = userProfileDemographic.Location_Demographics;

            if (userLocation == null || advertData.IndexOf(userLocation) == -1)
                return advertData.Substring(0, 1);
            else
                return userLocation;
        }
    }
}