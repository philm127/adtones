using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFMVC.Web.Core.Models
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Advertiser = "Advertiser";
        public const string UserAdmin = "UserAdmin";
        public const string AdvertAdmin = "AdvertAdmin";
        public const string OperatorAdmin = "OperatorAdmin";
        public const string ProfileAdmin = "ProfileAdmin";
        public const string SalesManager = "SalesManager";
        public const string SalesExec = "SalesExec";
    }
    public enum UserRoles
    {
        Admin = 1,
        User = 2,       
        Advertiser = 3,
        UserAdmin=4,
        AdvertAdmin=5,
        OperatorAdmin=6,
        ProfileAdmin = 7,
        SalesManager = 8,
        SalesExec = 9
    }
}
