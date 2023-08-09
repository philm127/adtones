
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.TempProfileFix
{
    public interface IProfileDataFix
    {
        string GetLocationData(int Id, UserProfileDemographicAdvertiserFormModel userProfileDemographic);
    }
}
