//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class CountryTax
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<decimal> TaxPercantage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int Status { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }
    }
}