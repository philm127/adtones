using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class Order2
    {
        [Key]
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public Product2 Product { get; set; }
    }
}
