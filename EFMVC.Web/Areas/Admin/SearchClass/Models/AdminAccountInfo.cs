using EFMVC.Web.ViewModels;


namespace EFMVC.Web.Areas.Admin.Models
{
    public class AdminAccountInfo
    {

        public  UsersAdmin.ViewModel.UserProfile UserProfileInfo { get; set; }
        public CompanyDetailsFormModel CompanyDetailsFormModel { get; set; }

        public ContactsFormModel ContactsFormModel { get; set; }
    }
}