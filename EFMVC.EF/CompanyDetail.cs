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
    
    public partial class CompanyDetail
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string AdditionalAddress { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> CountryId { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }
    }
}