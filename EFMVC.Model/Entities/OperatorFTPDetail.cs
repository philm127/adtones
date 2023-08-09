using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model.Entities
{
    public class OperatorFTPDetail
    {
        public int OperatorFTPDetailId { get; set; }

        [StringLength(100)]
        public string Host { get; set; }

        [StringLength(10)]
        public string Port { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FtpRoot { get; set; }

        public int OperatorId { get; set; }
        public virtual Operator Operator { get; set; }
    }
}
