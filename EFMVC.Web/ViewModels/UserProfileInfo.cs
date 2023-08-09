using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class UserProfileInfo
    {
        [EmailAddress]
        public string Email { get; set; }

       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Organisation { get; set; }

        public string MSISDN { get; set; }

        public bool IsEmailVerfication { get; set; }

        //Commented on 19-02-2019
        //Add 14-02-2019
        //public int? RewardId { get; set; }
    }
}