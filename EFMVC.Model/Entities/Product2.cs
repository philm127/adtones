﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class Product2
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public ICollection<Order2> Order2s { get; set; }
    }
}
