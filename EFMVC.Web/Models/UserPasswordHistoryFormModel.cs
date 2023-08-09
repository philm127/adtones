using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class UserPasswordHistoryFormModel
    {
        #region User Password History Data

        public int UserPasswordHistoryId { get; set; }

        public int UserId { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateCreated { get; set; }

        public int? AdtoneServerUserPasswordHistoryId { get; set; }

        #endregion
    }
}