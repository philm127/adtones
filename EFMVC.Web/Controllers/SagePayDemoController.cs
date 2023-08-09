using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;

namespace EFMVC.Web.Controllers
{
    public class SagePayDemoController : Controller
    {
        // GET: SagePayDemo
        public class Card
        {
            public string Expiry { get; set; }
            public string MerchantSessionKey { get; set; }
            public string StatusCode { get; set; }
        }

        public class CardIdentifiers
        {
            public string CardIdentifier { get; set; }
            public DateTime Expiry { get; set; }
            public string CardType { get; set; }
        }

        public class Output
        {
            public string Status { get; set; }
            public string StatusDetail { get; set; }
            public string TransactionId { get; set; }
            public string TransactionType { get; set; }
            public string Currency { get; set; }
            
        }
        public ActionResult Index()
        {
            var test = TempData["StatusDetail"];
            return View();
        }
        public ActionResult Add()
        {
            SagePayDemoModel model = new SagePayDemoModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Add(SagePayDemoModel model)
        {
            if (ModelState.IsValid)
            {
               var deserial = new JsonDeserializer();

            IRestResponse response1;
            IRestResponse response2;
            var client = new RestClient("https://pi-test.sagepay.com/api/v1/merchant-session-keys/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Basic MnI5TlZUR29yWm5tY29GN2RMUnBGeHpOdWs5NFprZ0J1TGRzOVVuYmdQVnRRSTFzOG86UEk1SUowM3ZhMHRibEJrRDlhNzJWTERFQ0RnUTZTTWVYUjVrYUlGZHNmbEc3ckhxU05oZ3h4ZEduRDhQTGpLcEY=");
            request.AddParameter("application/json", "{\r\n\t\"vendorName\":\"adtoneslimited\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);          
            var OfferJ = deserial.Deserialize<List<Card>>(response);


            if (response.StatusCode.ToString() == "Created")
            {
                client = new RestClient("https://pi-test.sagepay.com/api/v1/card-identifiers/");
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Bearer " + OfferJ[0].MerchantSessionKey.ToString());
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"cardDetails\": {\r\n    \"cardholderName\": \"" + model.CardholderName + "\",\r\n    \"cardNumber\": \"" + model.CardNumber + "\",\r\n    \"expiryDate\": \"" + model.ExpiryDate + "\",\r\n    \"securityCode\": \"" + model.SecurityCode + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                response1 = client.Execute(request);
                dynamic jsonResponse1 = JsonConvert.DeserializeObject(response1.Content);              
                var OfferJ1 = deserial.Deserialize<List<CardIdentifiers>>(response1);

                if (response1.StatusCode.ToString() == "Created")
                {
                    Random rand = new Random();
                    long randnum2 = (long)(rand.NextDouble() * 9000000000) + 1000000000;
                    client = new RestClient("https://pi-test.sagepay.com/api/v1/transactions/");
                    request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", "Basic MnI5TlZUR29yWm5tY29GN2RMUnBGeHpOdWs5NFprZ0J1TGRzOVVuYmdQVnRRSTFzOG86UEk1SUowM3ZhMHRibEJrRDlhNzJWTERFQ0RnUTZTTWVYUjVrYUlGZHNmbEc3ckhxU05oZ3h4ZEduRDhQTGpLcEY=");
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", "{\r\n    \"paymentMethod\": {\r\n     \"card\": {\r\n      \"merchantSessionKey\":\"" + OfferJ[0].MerchantSessionKey.ToString() + "\",\r\n      \"cardIdentifier\":\"" + OfferJ1[0].CardIdentifier.ToString() + "\"\r\n     }\r\n    },\r\n    \"transactionType\":\"Payment\",\r\n    \"vendorTxCode\":\"adtoneslimited-" + randnum2.ToString() + "\",\r\n    \"amount\":" + model.Amount + ",\r\n    \"currency\":\"GBP\",\r\n    \"customerFirstName\":\"Sam\",\r\n    \"customerLastName\":\"Jones\",\r\n    \"billingAddress\":{\r\n        \"address1\":\"88\",\r\n        \"city\":\"London\",\r\n        \"postalCode\":\"412\",\r\n        \"country\":\"GB\"\r\n    },\r\n    \"entryMethod\":\"Ecommerce\",\r\n    \"apply3DSecure\":\"Disable\",\r\n    \"applyAvsCvcCheck\":\"Disable\",\r\n    \"description\":\"Testing\",\r\n    \"customerEmail\":\"test.emaili@domain.com\",\r\n    \"customerPhone\":\"0845 111 4455\",\r\n    \"shippingDetails\":{\r\n        \"recipientFirstName\":\"Sam\",\r\n        \"recipientLastName\":\"Jones\",\r\n        \"shippingAddress1\":\"407 St John Street\",\r\n        \"shippingCity\":\"London\",\r\n        \"shippingPostalCode\":\"EC1V 4AB\",\r\n        \"shippingCountry\":\"GB\"\r\n    }\r\n}\r\n", ParameterType.RequestBody);
                    response2 = client.Execute(request);
                    var finalstatus = deserial.Deserialize<List<Output>>(response2);

                    if (response2.StatusCode.ToString() == "Created")
                    {
                        TempData["Status"] = finalstatus[0].Status.ToString();
                        TempData["StatusDetail"] = finalstatus[0].StatusDetail.ToString();
                        TempData["TransactionId"] = finalstatus[0].TransactionId.ToString();
                        TempData["TransactionType"] = finalstatus[0].TransactionType.ToString();
                        TempData["Currency"] = finalstatus[0].Currency.ToString();
                    }
                    else
                    {
                        TempData["Status"] = "Card details is invalid.";
                        TempData["StatusDetail"] = null;
                        TempData["TransactionId"] = null;
                        TempData["TransactionType"] = null;
                        TempData["Currency"] = null;
                    }
                    return RedirectToAction("Index", "SagePayDemo");


                    }
                    else
                    {
                        TempData["Status"] = "Card details is invalid.";
                        TempData["StatusDetail"] = null;
                        TempData["TransactionId"] = null;
                        TempData["TransactionType"] = null;
                        TempData["Currency"] = null;
                        return RedirectToAction("Index", "SagePayDemo");
                    }
                }
                else
                {
                    TempData["Status"] = "Card details is invalid.";
                    TempData["StatusDetail"] = null;
                    TempData["TransactionId"] = null;
                    TempData["TransactionType"] = null;
                    TempData["Currency"] = null;
                    return RedirectToAction("Index", "SagePayDemo");
                }
            }
            return View(model);
        }
    }
}