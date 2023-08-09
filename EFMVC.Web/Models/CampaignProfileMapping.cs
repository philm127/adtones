
using System.Collections.Generic;
using EFMVC.Web.SearchClass;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.Models
{
    public class CampaignProfileMapping
    {
        public CampaignProfileGeographicFormModel CampaignProfileGeographicModel { get; set; }
        public CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel { get; set; }
        public CampaignProfileAdvertFormModel CampaignProfileAd { get; set; }
        public CampaignProfileAttitudeFormModel campaignProfileAttitude { get; set; }
        public CampaignProfileCinemaFormModel CampaignProfileCinemaFormModel { get; set; }

        public CampaignProfileInternetFormModel CampaignProfileInternetFormModel { get; set; }
        public CampaignProfileMobileFormModel CampaignProfileMobileFormModel { get; set; }

        public CampaignProfilePressFormModel CampaignProfilePressFormModel { get; set; }

        public CampaignProfileProductsServiceFormModel CampaignProfileProductsServiceFormModel { get; set; }

        public CampaignProfileRadioFormModel CampaignProfileRadioFormModel { get; set; }

        public CampaignProfileTvFormModel CampaignProfileTvFormModel { get; set; }
        public CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingFormModel { get; set; }

        public CampaignProfileSkizaFormModel CampaignProfileSkizaFormModel { get; set; }

        public IEnumerable<AdvertFormModel> AdvertFormModel { get; set; }

        public List<CampaignAuditResult> CampaignAudit { get; set; }

        public CampaignAuditFilter CampaignAuditFilter { get; set; }

        public CampaignDashboardChartResult CampaignDashboardChartResult { get; set; }
    }
}