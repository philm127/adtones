using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.OperatorAdmin.Models
{
    public class OperatorAccountInfo
    {
        public UserProfileDetails UserProfileInfo { get; set; }
        public CompanyDetailsFormModel CompanyDetailsFormModel { get; set; }

        public ContactsFormModel ContactsFormModel { get; set; }
    }
}