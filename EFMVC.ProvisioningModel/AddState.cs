//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.ProvisioningModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddState
    {
        public AddState()
        {
            this.BUCKET_ITEM = new HashSet<BucketItem>();
        }
    
        public int ID { get; set; }
        public string STATE { get; set; }
    
        public virtual ICollection<BucketItem> BUCKET_ITEM { get; set; }
    }
}
