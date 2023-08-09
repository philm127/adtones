using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class CoinBase
    {
    }

    public class Addresses
    {
        public string bitcoincash { get; set; }
        public string bitcoin { get; set; }
        public string ethereum { get; set; }
        public string litecoin { get; set; }
    }

    public class Metadata
    {
        public string customer_id { get; set; }
        public string customer_name { get; set; }
    }

    public class Local
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Ethereum
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Bitcoin
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Bitcoincash
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Litecoin
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Pricing
    {
        public Local local { get; set; }
        public Ethereum ethereum { get; set; }
        public Bitcoin bitcoin { get; set; }
        public Bitcoincash bitcoincash { get; set; }
        public Litecoin litecoin { get; set; }
    }

    public class Timeline
    {
        public string status { get; set; }
        public DateTime time { get; set; }
    }

    public class DataCharge
    {
        public Addresses addresses { get; set; }
        public string cancel_url { get; set; }
        public string code { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
        public DateTime expires_at { get; set; }
        public string hosted_url { get; set; }
        public string id { get; set; }
        public Metadata metadata { get; set; }
        public string name { get; set; }
        public List<object> payments { get; set; }
        public Pricing pricing { get; set; }
        public string pricing_type { get; set; }
        public string redirect_url { get; set; }
        public string resource { get; set; }
        public List<Timeline> timeline { get; set; }
    }

    public class RootObject1
    {
        public DataCharge data { get; set; }
    }


    public class LocalPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class DataCheckOut
    {
        public string id { get; set; }
        public string resource { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string> requested_info { get; set; }
        public string pricing_type { get; set; }
        public LocalPrice local_price { get; set; }
    }

    public class RootObjectCheckOut
    {
        public DataCheckOut data { get; set; }
    }
}