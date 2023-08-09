﻿using System;

namespace EFMVC.Web.Areas.Admin.Models
{
    public class UserCreditPaymentResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string Organisation { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal Amount { get; set; }
        

        public DateTime CreatedDateSort { get; set; }
        public string CreatedDate { get; set; }
        public int Status { get; set; }

    }
}