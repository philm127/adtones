using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.ViewModels
{
    public class NewBudgetFormModel
    {
        [Display(Name = "Monthly Budget")]
        public string MonthlyBudget { get; set; }

        [Display(Name = "Weekly Budget")]
        public string WeeklyBudget { get; set; }

        [Display(Name = "Daily Budget")]
        public string DailyBudget { get; set; }

        [Display(Name = "Hourly Budget")]
        public string HourlyBudget { get; set; }

        [Display(Name = "Maximum Bid")]
        [Required]
        public string MaximumBid { get; set; }

        public int CurrencyId { get; set; }
    }
}