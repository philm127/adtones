using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class GravityForm
    {
        public partial class Welcome
        {
            [JsonProperty("response")]
            public Response Response { get; set; }

            [JsonProperty("status")]
            public long Status { get; set; }
        }

        public partial class Response
        {
            [JsonProperty("entries")]
            public Entry[] Entries { get; set; }

            [JsonProperty("total_count")]
            public string TotalCount { get; set; }
        }

        public partial class Entry
        {
            [JsonProperty("created_by")]
            public string CreatedBy { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("date_created")]
            public string DateCreated { get; set; }

            [JsonProperty("form_id")]
            public string FormId { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("is_fulfilled")]
            public object IsFulfilled { get; set; }

            [JsonProperty("is_read")]
            public long IsRead { get; set; }

            [JsonProperty("is_starred")]
            public long IsStarred { get; set; }

            [JsonProperty("payment_amount")]
            public object PaymentAmount { get; set; }

            [JsonProperty("payment_date")]
            public object PaymentDate { get; set; }

            [JsonProperty("payment_method")]
            public object PaymentMethod { get; set; }

            [JsonProperty("payment_status")]
            public object PaymentStatus { get; set; }

            [JsonProperty("post_id")]
            public object PostId { get; set; }

            [JsonProperty("source_url")]
            public string SourceUrl { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("1")]
            public string The1 { get; set; }

            [JsonProperty("12")]
            public string The12 { get; set; }

            [JsonProperty("13")]
            public string The13 { get; set; }

            [JsonProperty("14")]
            public string The14 { get; set; }

            [JsonProperty("16")]
            public string The16 { get; set; }

            [JsonProperty("17")]
            public string The17 { get; set; }

            [JsonProperty("18")]
            public string The18 { get; set; }

            [JsonProperty("19")]
            public string The19 { get; set; }

            [JsonProperty("2")]
            public string The2 { get; set; }

            [JsonProperty("21")]
            public string The21 { get; set; }

            [JsonProperty("22")]
            public string The22 { get; set; }

            [JsonProperty("23")]
            public string The23 { get; set; }

            [JsonProperty("24")]
            public string The24 { get; set; }

            [JsonProperty("25")]
            public string The25 { get; set; }

            [JsonProperty("26")]
            public string The26 { get; set; }

            [JsonProperty("27")]
            public string The27 { get; set; }

            [JsonProperty("28")]
            public string The28 { get; set; }

            [JsonProperty("29")]
            public string The29 { get; set; }

            [JsonProperty("30")]
            public string The30 { get; set; }

            [JsonProperty("32")]
            public string The32 { get; set; }

            [JsonProperty("33")]
            public string The33 { get; set; }

            [JsonProperty("34")]
            public string The34 { get; set; }

            [JsonProperty("35")]
            public string The35 { get; set; }

            [JsonProperty("37")]
            public string The37 { get; set; }

            [JsonProperty("38.1")]
            public string The381 { get; set; }

            [JsonProperty("40")]
            public string The40 { get; set; }

            [JsonProperty("41")]
            public string The41 { get; set; }

            [JsonProperty("42")]
            public string The42 { get; set; }

            [JsonProperty("43")]
            public string The43 { get; set; }

            [JsonProperty("44.1")]
            public string The441 { get; set; }

            [JsonProperty("44.2")]
            public string The442 { get; set; }

            [JsonProperty("44.3")]
            public string The443 { get; set; }

            [JsonProperty("44.4")]
            public string The444 { get; set; }

            [JsonProperty("46.1")]
            public string The461 { get; set; }

            [JsonProperty("47")]
            public string The47 { get; set; }

            [JsonProperty("48")]
            public string The48 { get; set; }

            [JsonProperty("49.1")]
            public string The491 { get; set; }

            [JsonProperty("50")]
            public string The50 { get; set; }

            [JsonProperty("51.1")]
            public string The511 { get; set; }

            [JsonProperty("52")]
            public string The52 { get; set; }

            [JsonProperty("53")]
            public string The53 { get; set; }

            [JsonProperty("55")]
            public string The55 { get; set; }

            [JsonProperty("56")]
            public string The56 { get; set; }

            [JsonProperty("58")]
            public string The58 { get; set; }

            [JsonProperty("59")]
            public string The59 { get; set; }

            [JsonProperty("6")]
            public string The6 { get; set; }

            [JsonProperty("60")]
            public string The60 { get; set; }

            [JsonProperty("61.1")]
            public string The611 { get; set; }

            [JsonProperty("62")]
            public string The62 { get; set; }

            [JsonProperty("63.1")]
            public string The631 { get; set; }

            [JsonProperty("64")]
            public string The64 { get; set; }

            [JsonProperty("65")]
            public string The65 { get; set; }

            [JsonProperty("66")]
            public string The66 { get; set; }

            [JsonProperty("67")]
            public string The67 { get; set; }

            [JsonProperty("68")]
            public string The68 { get; set; }

            [JsonProperty("69.1")]
            public string The691 { get; set; }

            [JsonProperty("7")]
            public string The7 { get; set; }

            [JsonProperty("70")]
            public string The70 { get; set; }

            [JsonProperty("71.1")]
            public string The711 { get; set; }

            [JsonProperty("72")]
            public string The72 { get; set; }

            [JsonProperty("73.1")]
            public string The731 { get; set; }

            [JsonProperty("74")]
            public string The74 { get; set; }

            [JsonProperty("75")]
            public string The75 { get; set; }

            [JsonProperty("76")]
            public string The76 { get; set; }

            [JsonProperty("77")]
            public string The77 { get; set; }

            [JsonProperty("78")]
            public string The78 { get; set; }

            [JsonProperty("79")]
            public string The79 { get; set; }

            [JsonProperty("8")]
            public string The8 { get; set; }

            [JsonProperty("transaction_id")]
            public object TransactionId { get; set; }

            [JsonProperty("transaction_type")]
            public object TransactionType { get; set; }

            [JsonProperty("user_agent")]
            public string UserAgent { get; set; }

            [JsonProperty("84.1")]
            public string The841 { get; set; }

            [JsonProperty("84.2")]
            public string The842 { get; set; }

            [JsonProperty("84.3")]
            public string The843 { get; set; }

            [JsonProperty("84.4")]
            public string The844 { get; set; }

            [JsonProperty("84.5")]
            public string The845 { get; set; }

            [JsonProperty("84.6")]
            public string The846 { get; set; }

            [JsonProperty("87.1")]
            public string The871 { get; set; }

            [JsonProperty("89.1")]
            public string The891 { get; set; }

            [JsonProperty("91.1")]
            public string The911 { get; set; }

            [JsonProperty("93.1")]
            public string The931 { get; set; }

            [JsonProperty("96.1")]
            public string The961 { get; set; }

            [JsonProperty("98.1")]
            public string The981 { get; set; }

            [JsonProperty("100.1")]
            public string The1001 { get; set; }

            [JsonProperty("102.1")]
            public string The1021 { get; set; }

            [JsonProperty("104.1")]
            public string The1041 { get; set; }


        }

        public partial class Welcome
        {
            public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Converter.Settings);
        }

        //public static class Serialize
        //{
        //    public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, Converter.Settings);
        //}

        public class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
        }
    }
}