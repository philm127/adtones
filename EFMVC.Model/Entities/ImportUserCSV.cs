using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class ImportUserCSV
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(1)]
        public string OperationType { get; set;}

        [StringLength(100)]
        public string Email { get; set; }

   
        public DateTime? DateCreated { get; set; }

        public DateTime AddedDate { get; set; }
        public bool Proceed { get; set; }

    }
}
