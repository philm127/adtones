﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class UserCreditResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public string Email { get; set; }
        public string Name { get; set; }

        public string Organisation { get; set; }
        public decimal Credit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalUsed { get; set; }
        public decimal TotalPayed { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime? CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
    }
}