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
    
    public partial class BUCKET_ITEM
    {
        public int ID { get; set; }
        public int BUCKET_ID { get; set; }
        public int PRIORITY { get; set; }
        public Nullable<int> ADD_STATE_ID { get; set; }
        public string MEDIA_URL { get; set; }
        public double BID_VALUE { get; set; }
        public Nullable<System.DateTime> ADD_START { get; set; }
        public Nullable<System.DateTime> ADD_END { get; set; }
        public string CAMPAIGNID { get; set; }
        public string DTMF_EVENT { get; set; }
        public string SMS_MESSAGE { get; set; }
        public string EMAIL_MESSAGE { get; set; }
        public string ORIGINATOR { get; set; }
        public Nullable<bool> Processed { get; set; }
    
        public virtual ADD_STATE ADD_STATE { get; set; }
        public virtual BUCKET BUCKET { get; set; }
    }
}