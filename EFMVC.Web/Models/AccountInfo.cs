using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.Models
{
    public class AccountInfo
    {
        public UserProfileInfo UserProfileInfo { get; set; }

        public ChangePasswordFormModel ChangePasswordFormModel { get; set; }

        public CompanyDetailsFormModel CompanyDetailsFormModel { get; set; }

        public ContactsFormModel ContactsFormModel { get; set; }

        //Add 19-02-2019
        public RewardInfoFormModel RewardInfoFormModel { get; set; }
    }
}