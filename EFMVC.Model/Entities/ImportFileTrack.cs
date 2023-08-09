using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class ImportFileTrack
    {
        [Key]
        public int Id { get; set; }
        public int NumOfTextFile { get; set; }
        public int NumOfTextLine { get; set; }
        public DateTime AddedDate { get; set; }
        public int? OperatorId { get; set; }
        public int NumOfUpdateToAudit { get; set; }
        public bool Proceed { get; set; }
        public int NumOfPlay { get; set; }
        public int NumOfSMS { get; set; }
        public int NumOfEmail { get; set; }
        public int NumOfUser { get; set; }
        public int NumOfRemovedUser { get; set; }
        public int NumOfNewUser { get; set; }
        public int NumOfLiveCampaign { get; set; }
        public int NumOfBucket { get; set; }

        public virtual Operator Operator { get; set; }
    }
}
