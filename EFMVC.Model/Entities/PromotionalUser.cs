using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFMVC.Model
{
    public class PromotionalUser
    {
        [Key]
        public int ID { get; set; }
        public string MSISDN { get; set; }
        public int BatchID { get; set; }
        public int DailyPlay { get; set; }
        public int WeeklyPlay { get; set; }
        public int Status { get; set; }
        //public string DeliveryServerConnectionString { get; set; }
        //public string DeliveryServerIpAddress { get; set; }
    }
}
