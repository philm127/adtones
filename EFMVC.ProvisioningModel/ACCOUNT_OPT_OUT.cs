//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.ProvisioningModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACCOUNT_OPT_OUT
    {
        public int ID { get; set; }
        public int ACCOUNT_ID { get; set; }
        public string MSISDN { get; set; }
    
        public virtual ACCOUNT ACCOUNT { get; set; }
    }
}
