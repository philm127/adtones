using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFMVC.Model;

namespace EFMVC.Web.ViewModels
{
    public class UsersCreditFormModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal AssignCredit { get; set; }

        public int CurrencyId { get; set; }

        public decimal AvailableCredit { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

     
    }
}