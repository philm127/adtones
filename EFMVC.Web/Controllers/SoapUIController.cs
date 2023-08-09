using EFMVC.Data;
using EFMVC.Model;
using EFMVC.Web.Models;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace EFMVC.Web.Controllers
{

    public class SoapUIController : Controller
    {
        EFMVCDataContex db = new EFMVCDataContex();
        // GET: SoapUI
        public ActionResult Index()
        {
            var userList = db.Users.Where(s => s.IsMsisdnMatch == false && s.RoleId == 2).ToList();
            var portalAccount = "adtones";
            var portalPassword = "TBD";
            var portalType = "1";
            var role = "4";
            var roleCode = "100";
            var corpId = "440114";
            string soapUIUrl = ConfigurationManager.AppSettings["SoapUIUrl"];

            List<int> userIdList = new List<int>();
            foreach (var item in userList)
            {
                //var userData = db.Userprofiles.Where(s=>s.UserId == item.UserId).FirstOrDefault();
                var userData = item.UserProfiles.FirstOrDefault();
               
                
                if (userData != null)
                {
                    var phoneNumber = userData.MSISDN;

                    var client = new RestClient(soapUIUrl);
                    var request = new RestRequest(Method.POST);

                    //request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n         <event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n            <portalAccount xsi:type=\"xsd:string\">?</portalAccount>\r\n            <portalPwd xsi:type=\"xsd:string\">?</portalPwd>\r\n            <portalType xsi:type=\"xsd:string\">?</portalType>\r\n            <moduleCode xsi:type=\"xsd:string\">?</moduleCode>\r\n            <role xsi:type=\"xsd:string\">?</role>\r\n            <roleCode xsi:type=\"xsd:string\">?</roleCode>\r\n            <contractNum xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n            <corpCode xsi:type=\"xsd:string\">?</corpCode>\r\n            <corpID xsi:type=\"xsd:string\">?</corpID>\r\n            <personalAccount xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n            <phoneNumber xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n            <region xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n            <userName xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n            <userStatus xsi:type=\"cor:ArrayOf_xsd_string\" soapenc:arrayType=\"xsd:string[]\"/>\r\n         </event>\r\n      </cor:addCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    request.AddParameter("undefined",
                        "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
                        "<event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n           \t" +
                            "<portalAccount>" + portalAccount + "</portalAccount>\r\n\t\t\t<portalPwd>" + portalPassword + "</portalPwd>\r\n\t\t\t" +
                            "<portalType>" + portalType + "</portalType>\r\n\t\t\t<role>" + role + "</role>\r\n\t\t\t" +
                            "<roleCode>" + roleCode + "</roleCode>\r\n\t\t\t" +
                            "<corpID>" + corpId + "</corpID>\r\n\t\t\t<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
                        "</event>\r\n      </cor:addCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseContent = response.Content;

                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(responseContent);
                        XmlNodeList nodeList = xmldoc.GetElementsByTagName("addCorpUserReturn");
                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                            {
                                //var text = node.InnerText;
                                var returnCode = node.SelectSingleNode("returnCode").InnerXml;
                                if (returnCode == "000000")
                                {
                                    item.Activated = 1;
                                    item.IsMsisdnMatch = true;
                                    db.SaveChanges();
                                    //userIdList.Add(item.UserId);
                                }
                            }

                        }
                    }

                }
            }

            //var userDataList = userList.Where(s => userIdList.Contains(s.UserId)).ToList();
            //foreach(var item in userDataList)
            //{
            //    item.Activated = 1;
            //    item.IsMsisdnMatch = true;
            //}
            //db.SaveChanges();
            ViewBag.Success = "Success";


            return View();
        }
    }
}