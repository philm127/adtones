﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class CurrencyConvertModel
    {
        public double value { get; set; }
        public string text { get; set; }
        public int timestamp { get; set; }
    }
}