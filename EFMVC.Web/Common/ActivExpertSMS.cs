using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace EFMVC.Web.Common
{
    public class ActivExpertSMSResponse
    {
        public string d { get; set; } 
    }
    public class ActivExpertSMS
    {
        static string url = ConfigurationManager.AppSettings["ActivExpertSMSUrl"];
        public async Task<string> SendSMS(string mobileNumber, string message)
        {
            try
            {
                var fullUrl = url + "create/default.aspx/SendSMS";

                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\n\tmobileNumber: '+" + mobileNumber + "',\n\tmessage: '" + message + "'\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var baseObj = JsonConvert.DeserializeObject<ActivExpertSMSResponse>(response.Content);
                return baseObj.d;
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}