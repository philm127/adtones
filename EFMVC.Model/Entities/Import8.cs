using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class Import8
    {
        [Key]
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string CallingNumber { get; set; }
        public string CalledNumber { get; set; }
        public string RBTCode { get; set; }
        public string SPCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CallScheme { get; set; }
        public string DTMF { get; set; }
        public bool Proceed { get; set; }
        public int CampaignProfileId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
