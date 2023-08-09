using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class StatusCheckController : Controller
    {
        // GET: StatusCheck
        public ActionResult Index()
        {
            ServiceController sc = new ServiceController("Arthar Provisioning Schedule Service");

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    //return "Running";
                    break;
                case ServiceControllerStatus.Stopped:
                    var checkStatus = SendSms("447860064370");
                    break;
                // return "Stopped";
                case ServiceControllerStatus.Paused:
                    //return "Paused";
                    break;
                case ServiceControllerStatus.StopPending:
                    // return "Stopping";
                    break;
                case ServiceControllerStatus.StartPending:
                    //return "Starting";
                    break;
                default:
                    //return "Status Changing";
                    break;
            }
            return View();
        }

        private async Task<string> SendSms(string number)
        {
            var client = new HttpClient();

            //https://www.voodoosms.com/vapi/server/sendSMS?uid=adtones_sms_api&pass=2x8whkr&dest=447980720250&orig=Adtones&msg=your%20code%20is%2012%2012%2012&format=JSON
            //Get the parameters, either GET or POST request



            string uid = "adtones_sms_api";
            string pass = "2x8whkr";
            // string dest = "447980720250";
            string dest = number;

            string orig = "Adtone"; // number
            //string orig = "447860064145";
            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            //TempData["code1"] = code1;
            //TempData["code2"] = code2;
            //TempData["code3"] = code3;




            string msg = code1 + " " + code2 + " " + code3 + " is your Adtones verification code. It will expire in 24 hours";

            string format = "JSON";



            //Exit if one or more parameters is missing

            if (String.IsNullOrEmpty(uid) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(dest)
            || String.IsNullOrEmpty(orig) || String.IsNullOrEmpty(msg) || String.IsNullOrEmpty(format))
                System.Environment.Exit(1);

            var client2 = new RestClient("https://www.voodoosms.com/vapi/server/sendSMS?uid=" + uid + "&pass=" + pass + "&dest=" + dest + "&orig=" + orig + "&msg=" + msg + "&format=" + format);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client2.Execute(request);

            return response.StatusCode.ToString();
            //return "200";
        }

    }
}