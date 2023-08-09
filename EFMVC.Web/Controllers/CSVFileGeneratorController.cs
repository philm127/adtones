using AutoMapper;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class CSVFileGeneratorController : Controller
    {
        // GET: CSVFileGenerator
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICampaignAuditRepository _campaignAuditRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileAdvertsReceivedRepository _userProfileAdvertsReceivedRepository;
        
        public CSVFileGeneratorController(ICampaignProfileRepository profileRepository, ICampaignAuditRepository campaignAuditRepository, IUserProfileRepository userProfileRepository, ICampaignAdvertRepository campaignAdvertRepository, IOperatorRepository operatorRepository, IUserRepository userRepository, IUserProfileAdvertsReceivedRepository userProfileAdvertsReceivedRepository)
        {
            _profileRepository = profileRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _userProfileRepository = userProfileRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
            _operatorRepository = operatorRepository;
            _userRepository = userRepository;
            _userProfileAdvertsReceivedRepository = userProfileAdvertsReceivedRepository;
        }


        //Campain wise list
        public ActionResult Index()
        {
            var profile = _profileRepository.GetMany(s => s.Active == true && s.Status != 5).ToList();
            //double totalcredit = 0;
            var advertname = string.Empty;
            var advertid = 0;
            var brandname = string.Empty;
            int emailstatus = 0;
            int smsstatus = 0;
            string DailyReportPath = string.Empty;
            string ArchiveReportPath = string.Empty;
            string csv = "";
            bool isCsv = false;
            //foreach (var item in profile)
            //{
                // GetAdvertData(item.CampaignProfileId);
               // totalcredit = item.TotalCredit;
                var yesterday = DateTime.Today.AddDays(-1);


                var OperatorList = _operatorRepository.GetAll().ToList();
                foreach (var operatorData in OperatorList)
                {
                    #region CampainAuditData
                    var OperatorId = operatorData.OperatorId;
                    var OperatorName = operatorData.OperatorName;
                    //var CampaignAuditData = _campaignAuditRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId && s.StartTime >= yesterday && s.EndTime < DateTime.Now).OrderBy(k=>k.CampaignAuditId).ThenBy(j=>j.UserProfileId).ToList();

                    var CampaignAuditData = _campaignAuditRepository.GetMany(s =>  s.StartTime >= yesterday && s.EndTime < DateTime.Now).OrderBy(k => k.CampaignAuditId).ToList();
                    // var csv = new StringBuilder();
                    if (CampaignAuditData.Count() > 0)
                    {


                        var CampaignName = CampaignAuditData.FirstOrDefault().CampaignProfile.User.FirstName;
                        //var CurrentDate = DateTime.Now.ToString("ddMMyy");
                        //var FileName = CurrentDate + "_" + CampaignName + "_" + item.User.UserId + ".csv";

                        var CurrentDate = DateTime.Now.ToString("ddMMyy");
                       var FileName = CurrentDate + "_" + OperatorName + "_" + OperatorId + ".csv";

                        string DailyReportDirectoryName = Server.MapPath("~/ads/" + OperatorName + "/Played/Live");
                        string ArchiveDirectoryName = Server.MapPath("~/ads/" + OperatorName + "/Played/Archive");


                        //string DailyReportDirectoryName =  Server.MapPath("~/Daily Report/");
                        // string ArchiveDirectoryName = Server.MapPath("~/Archive/");

                         DailyReportPath = Path.Combine(DailyReportDirectoryName, FileName);
                         ArchiveReportPath = Path.Combine(ArchiveDirectoryName, FileName);

                        if (!Directory.Exists(DailyReportDirectoryName))
                            Directory.CreateDirectory(DailyReportDirectoryName);

                        if (!Directory.Exists(ArchiveDirectoryName))
                            Directory.CreateDirectory(ArchiveDirectoryName);

                        // AddDirectorySecurity(DailyReportPath, DailyReportPath, FileSystemRights.ReadData, AccessControlType.Allow);

                       
                        //AddFileSecurity(DailyReportPath, DailyReportPath, FileSystemRights.ReadData, AccessControlType.Allow);
                        foreach (var item2 in CampaignAuditData)
                        {
                            var MSISDN = _userProfileRepository.GetMany(k => k.UserProfileId == item2.UserProfileId).FirstOrDefault().MSISDN;
                            //string totalcostvalue;
                            var advertdetails = _campaignAdvertRepository.GetMany(top => top.CampaignProfileId == item2.CampaignProfileId);
                            if (advertdetails != null)
                            {
                                advertname = advertdetails.FirstOrDefault().Advert.AdvertName;
                                advertid = advertdetails.FirstOrDefault().Advert.AdvertId;
                                brandname = advertdetails.FirstOrDefault().Advert.Brand;
                                //totalcostvalue = advertdetails.FirstOrDefault().CreditsReceived.ToLower();
                            }
                            else
                            {
                                advertname = string.Empty;
                                brandname = string.Empty;
                                advertid = 0;
                               // totalcostvalue = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(item2.Email))
                            {
                                if (item2.Email.Trim().ToLower() == "requested")
                                {
                                    emailstatus = 1;
                                }
                                else
                                {
                                    emailstatus = 0;
                                }
                            }
                            if (!String.IsNullOrEmpty(item2.SMS))
                            {
                                if (item2.SMS.Trim().ToLower() == "requested")
                                {
                                    smsstatus = 1;
                                }
                                else
                                {
                                    smsstatus = 0;
                                }
                            }
                            //string startdate = item2.StartTime.ToString("dd-MM-yy hh:mm:ss");
                            //var newLine = string.Format("{0},{1},{2},{3}", MSISDN , item2.CampaignAuditId, startdate, advertname.Replace("\"", "\"\""));
                            //csv.AppendLine(newLine);

                            var smscost = item2.SMSCost.ToString().Split('.');
                            var emailcost = item2.EmailCost.ToString().Split('.');
                            var adcredit = item2.BidValue.ToString().Split('.');

                            int smscostvalue = int.Parse(smscost[0]);
                            int emailcostvalue = int.Parse(emailcost[0]);
                            int adcreditvalue = int.Parse(adcredit[0]);


                            double AdcreditValue = 0;
                            double SMSValue = 0;
                            double EmailValue = 0;
                            double TotalCostValue = 0;

                            double SMScost = 0;
                            double Emailcost = 0;
                            double Adcreditcost = 0;
                            double TotalCost = 0;

                            if (smscostvalue > 9)
                                SMSValue = item2.SMSCost * 1000;
                            else
                                SMSValue = item2.SMSCost * 100;

                            if (emailcostvalue > 9)
                                EmailValue = item2.EmailCost * 1000;
                            else
                                EmailValue = item2.EmailCost * 100;
                            //(float.Parse(rows[j].BidValue) * 90 / 100)
                            if (adcreditvalue > 9)
                                AdcreditValue = ((item2.BidValue * 90) / 100) * 1000;
                            else
                                AdcreditValue = ((item2.BidValue * 90) / 100) * 100;

                            SMScost = RoundUp(SMSValue, 2);
                            Emailcost = RoundUp(EmailValue, 2);
                            Adcreditcost = RoundUp(AdcreditValue, 2);

                            TotalCostValue = SMScost + Emailcost + Adcreditcost;
                            TotalCost = RoundUp(TotalCostValue, 2);

                            //DateTime now = DateTime.UtcNow;
                            //DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.Local);

                            //TimeZone localZone = TimeZone.CurrentTimeZone;
                            var currency = "EUR";
                            var PlayLength = TimeSpan.FromMilliseconds(item2.PlayLengthTicks).Seconds;
                            //csv += MSISDN + "," + item2.CampaignAuditId + "," + item2.StartTime.ToString("ddMMyyhhmmss") + "," + item2.EndTime.ToString("ddMMyyhhmmss") + "," + PlayLength + "," + advertname.Replace(",", " ") + "," + advertid + "," + brandname.Replace(",", " ") + "," + advertid + "," + item2.Status + "," + Adcreditcost + "," + smsstatus + "," + SMScost + "," + emailstatus + "," + Emailcost + "," + TotalCost + "," + currency + "," + "GMT" + "," + "\n";
                            csv += MSISDN + ";" + item2.CampaignAuditId + ";" + item2.StartTime.ToString("ddMMyyhhmmss") + ";" + item2.EndTime.ToString("ddMMyyhhmmss") + ";" + PlayLength + ";" + advertname.Replace(",", " ") + ";" + advertid + ";" + brandname.Replace(",", " ") + ";" + advertid + ";" + item2.Status + ";" + Adcreditcost + ";" + smsstatus + ";" + SMScost + ";" + emailstatus + ";" + Emailcost + ";" + TotalCost + ";" + currency + ";" + "GMT" + "\n";
                            isCsv = true;
                        }

                        //string csv = string.Concat(
                        //             AdvertData.Select(
                        //                    advert =>                                       
                        //                    string.Format("{0},{1},{2}\n",  advert.CampaignAuditId, advert.UserProfileId, advert.BidValue)
                        //                    )

                        //                    );



                        // System.IO.File.WriteAllText(DailyReportPath, csv.ToString());

                        //  AddFileSecurity(DailyReportPath, DailyReportPath, FileSystemRights.ReadData, AccessControlType.Allow);
                        //  AddFileSecurity(FileName, ArchiveReportPath, FileSystemRights.ReadData, AccessControlType.Allow);



                        //AddFileSecurity(DailyReportPath, @"shilpesh", FileSystemRights.Read, AccessControlType.Allow);
                        // AddDirectorySecurity(DailyReportPath, @"shilpesh", FileSystemRights.ReadData, AccessControlType.Allow);
                       

                    }
                    

                    #endregion
                }

                if (isCsv == true)
                {
                    System.IO.File.WriteAllText(DailyReportPath, csv);
                    System.IO.File.WriteAllText(ArchiveReportPath, csv);
                }
                ViewBag.Window = true;

            //}
            return View();
        }



        //User wise list
        //public ActionResult Index()
        //{

        //    //  double totalcredit = 0;
        //    var advertname = string.Empty;
        //    var advertid = 0;
        //    var brandname = string.Empty;
        //    int emailstatus = 0;
        //    int smsstatus = 0;
        //    var OperatorList = _operatorRepository.GetAll().ToList();
        //    foreach (var operatorData in OperatorList)
        //    {
        //        var OperatorId = operatorData.OperatorId;
        //        var OperatorName = operatorData.OperatorName;
        //        var profile = _userRepository.GetMany(s => s.Activated == 1 && s.RoleId == 2 && s.OperatorId == OperatorId).ToList();
        //        foreach (var item in profile)
        //        {
        //            //totalcredit = item.TotalCredit;

        //            var yesterday = DateTime.Today.AddDays(-1);

        //            var UserProfileID = item.UserProfiles.FirstOrDefault().UserProfileId;


        //            #region CampainAuditData

        //            var CampaignAuditData = _campaignAuditRepository.GetMany(s => s.UserProfileId == UserProfileID && s.StartTime >= yesterday && s.EndTime < DateTime.Now);
        //            if (CampaignAuditData.Count() > 0)
        //            {

        //                var CurrentDate = DateTime.Now.ToString("ddMMyy");
        //                var FileName = CurrentDate + "_" + OperatorName + "_" + OperatorId + ".csv";

        //                string DailyReportDirectoryName = Server.MapPath("~/ads/" + OperatorName + "/Played/Live");
        //                string ArchiveDirectoryName = Server.MapPath("~/ads/" + OperatorName + "/Played/Archive");

        //                string DailyReportPath = Path.Combine(DailyReportDirectoryName, FileName);
        //                string ArchiveReportPath = Path.Combine(ArchiveDirectoryName, FileName);

        //                if (!Directory.Exists(DailyReportDirectoryName))
        //                    Directory.CreateDirectory(DailyReportDirectoryName);

        //                if (!Directory.Exists(ArchiveDirectoryName))
        //                    Directory.CreateDirectory(ArchiveDirectoryName);

        //                string csv = "";
        //                foreach (var item2 in CampaignAuditData)
        //                {
        //                    var MSISDN = _userProfileRepository.GetMany(k => k.UserProfileId == item2.UserProfileId).FirstOrDefault().MSISDN.Replace("+", "");

        //                    //var advertdetails = _campaignAdvertRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId);

        //                    var advertdetails = _userProfileAdvertsReceivedRepository.GetMany(top => top.UserProfileId == item2.UserProfileId).OrderByDescending(x => x.DateTimePlayed).ToList();

        //                    #region UserProfileAdvertRecieved Record

        //                    //if (advertdetails.Count() > 0)
        //                    //{
        //                    //    foreach(var useradvert in advertdetails)
        //                    //    {
        //                    //        advertname = useradvert.AdvertName;
        //                    //        advertid = Convert.ToInt32(useradvert.AdvertRef);

        //                    //        brandname = useradvert.Brand;

        //                    //        var CampaignAuditInfoData = _campaignAuditRepository.GetMany(s => s.UserProfileId == useradvert.UserProfileId && s.StartTime >= yesterday && s.EndTime < DateTime.Now);
        //                    //        foreach (var CampaignAuditInfo in CampaignAuditInfoData)
        //                    //        {

        //                    //            if (!String.IsNullOrEmpty(CampaignAuditInfo.Email))
        //                    //            {
        //                    //                if (CampaignAuditInfo.Email.Trim().ToLower() == "requested")
        //                    //                {
        //                    //                    emailstatus = 1;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    emailstatus = 0;
        //                    //                }
        //                    //            }
        //                    //            if (!String.IsNullOrEmpty(CampaignAuditInfo.SMS))
        //                    //            {
        //                    //                if (CampaignAuditInfo.SMS.Trim().ToLower() == "requested")
        //                    //                {
        //                    //                    smsstatus = 1;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    smsstatus = 0;
        //                    //                }
        //                    //            }

        //                    //            var smscost = CampaignAuditInfo.SMSCost.ToString().Split('.');
        //                    //            var emailcost = CampaignAuditInfo.EmailCost.ToString().Split('.');
        //                    //            var adcredit = CampaignAuditInfo.BidValue.ToString().Split('.');

        //                    //            int smscostvalue = int.Parse(smscost[0]);
        //                    //            int emailcostvalue = int.Parse(emailcost[0]);
        //                    //            int adcreditvalue = int.Parse(adcredit[0]);


        //                    //            double AdcreditValue = 0;
        //                    //            double SMSValue = 0;
        //                    //            double EmailValue = 0;
        //                    //            double TotalCostValue = 0;

        //                    //            double SMScost = 0;
        //                    //            double Emailcost = 0;
        //                    //            double Adcreditcost = 0;
        //                    //            double TotalCost = 0;

        //                    //            if (smscostvalue > 9)
        //                    //                SMSValue = CampaignAuditInfo.SMSCost * 1000;
        //                    //            else
        //                    //                SMSValue = CampaignAuditInfo.SMSCost * 100;

        //                    //            if (emailcostvalue > 9)
        //                    //                EmailValue = CampaignAuditInfo.EmailCost * 1000;
        //                    //            else
        //                    //                EmailValue = CampaignAuditInfo.EmailCost * 100;

        //                    //            if (adcreditvalue > 9)
        //                    //                AdcreditValue = ((CampaignAuditInfo.BidValue * 90) / 100) * 1000;
        //                    //            else
        //                    //                AdcreditValue = ((CampaignAuditInfo.BidValue * 90) / 100) * 100;

        //                    //            SMScost = RoundUp(SMSValue, 2);
        //                    //            Emailcost = RoundUp(EmailValue, 2);
        //                    //            Adcreditcost = RoundUp(AdcreditValue, 2);

        //                    //            if (useradvert.CreditsReceived.Trim().ToLower() == "cancelled")
        //                    //            {
        //                    //                TotalCostValue = SMScost + Emailcost;

        //                    //            }
        //                    //            else
        //                    //            {
        //                    //                TotalCostValue = SMScost + Emailcost + Adcreditcost;
        //                    //            }


        //                    //            TotalCost = RoundUp(TotalCostValue, 2);

        //                    //            var currency = "EUR";
        //                    //            var PlayLength = TimeSpan.FromMilliseconds(CampaignAuditInfo.PlayLengthTicks).Seconds;
        //                    //            // csv += MSISDN + "\\;" + item2.CampaignAuditId +"\\;" + item2.StartTime.ToString("dd-MM-yy hh:mm:ss") + "\\;" + item2.EndTime.ToString("dd-MM-yy hh:mm:ss") + "\\;" + item2.PlayLengthTicks + "\\;" + advertname + "\\;" + advertid + "\\;" + brandname + "\\;" + advertid + "\\;" + item2.Status + "\\;" + Adcreditcost + "\\;" + smsstatus + "\\;" + SMScost + "\\;" + emailstatus + "\\;" + Emailcost + "\\;" + TotalCost + "\\;" + currency + "\\;" + localZone.StandardName  + "\n";
        //                    //            //csv += MSISDN + "," + useradvert.CampaignAudit.CampaignAuditId + "," + useradvert.CampaignAudit.StartTime.ToString("ddMMyyhhmmss") + "," + useradvert.CampaignAudit.EndTime.ToString("ddMMyyhhmmss") + "," + PlayLength + "," + advertname.Replace(",", " ") + "," + advertid + "," + brandname.Replace(",", " ") + "," + advertid + "," + item2.Status + "," + Adcreditcost + "," + smsstatus + "," + SMScost + "," + emailstatus + "," + Emailcost + "," + TotalCost + "," + currency + "," + "GMT"  + "\n";

        //                    //            csv += MSISDN + ";" + CampaignAuditInfo.CampaignAuditId + ";" + CampaignAuditInfo.StartTime.ToString("yyyyMMddhhmmss") + ";" + CampaignAuditInfo.EndTime.ToString("yyyyMMddhhmmss") + ";" + PlayLength + ";" + advertname.ToLower() + ";" + advertid + ";" + brandname.ToLower() + ";" + advertid + ";" + item2.Status.ToLower() + ";" + Adcreditcost + ";" + smsstatus + ";" + SMScost + ";" + emailstatus + ";" + Emailcost + ";" + TotalCost + ";" + currency + ";" + "GMT" + "\n";
        //                    //        }
        //                    //    }
        //                    //}

        //                    #endregion

        //                    string totalcostvalue;
        //                    if (advertdetails != null)
        //                    {
        //                        advertname = advertdetails.FirstOrDefault().AdvertName;
        //                        advertid = Convert.ToInt32(advertdetails.FirstOrDefault().AdvertRef);
        //                        brandname = advertdetails.FirstOrDefault().Brand;
        //                        totalcostvalue = advertdetails.FirstOrDefault().CreditsReceived.ToLower();
        //                    }
        //                    else
        //                    {
        //                        advertname = string.Empty;
        //                        brandname = string.Empty;
        //                        advertid = 0;
        //                        totalcostvalue = string.Empty;
        //                    }




        //                    if (!String.IsNullOrEmpty(item2.Email))
        //                    {
        //                        if (item2.Email.Trim().ToLower() == "requested")
        //                        {
        //                            emailstatus = 1;
        //                        }
        //                        else
        //                        {
        //                            emailstatus = 0;
        //                        }
        //                    }
        //                    if (!String.IsNullOrEmpty(item2.SMS))
        //                    {
        //                        if (item2.SMS.Trim().ToLower() == "requested")
        //                        {
        //                            smsstatus = 1;
        //                        }
        //                        else
        //                        {
        //                            smsstatus = 0;
        //                        }
        //                    }

        //                    var smscost = item2.SMSCost.ToString().Split('.');
        //                    var emailcost = item2.EmailCost.ToString().Split('.');
        //                    var adcredit = item2.BidValue.ToString().Split('.');

        //                    int smscostvalue = int.Parse(smscost[0]);
        //                    int emailcostvalue = int.Parse(emailcost[0]);
        //                    int adcreditvalue = int.Parse(adcredit[0]);


        //                    double AdcreditValue = 0;
        //                    double SMSValue = 0;
        //                    double EmailValue = 0;
        //                    double TotalCostValue = 0;

        //                    double SMScost = 0;
        //                    double Emailcost = 0;
        //                    double Adcreditcost = 0;
        //                    double TotalCost = 0;

        //                    if (smscostvalue > 9)
        //                        SMSValue = item2.SMSCost * 1000;
        //                    else
        //                        SMSValue = item2.SMSCost * 100;

        //                    if (emailcostvalue > 9)
        //                        EmailValue = item2.EmailCost * 1000;
        //                    else
        //                        EmailValue = item2.EmailCost * 100;

        //                    if (adcreditvalue > 9)
        //                        AdcreditValue = ((item2.BidValue * 90) / 100) * 1000;
        //                    else
        //                        AdcreditValue = ((item2.BidValue * 90) / 100) * 100;

        //                    SMScost = RoundUp(SMSValue, 2);
        //                    Emailcost = RoundUp(EmailValue, 2);
        //                    Adcreditcost = RoundUp(AdcreditValue, 2);

        //                    if (totalcostvalue == "cancelled")
        //                    {
        //                        TotalCostValue = SMScost + Emailcost;
        //                    }
        //                    else
        //                    {
        //                        TotalCostValue = SMScost + Emailcost + Adcreditcost;
        //                    }

        //                    TotalCost = RoundUp(TotalCostValue, 2);

        //                    var currency = "EUR";
        //                    var PlayLength = TimeSpan.FromMilliseconds(item2.PlayLengthTicks).Seconds;
        //                    // var PlayLength = TimeSpan.FromTicks(item2.PlayLengthTicks).Seconds;
        //                    // csv += MSISDN + "\\;" + item2.CampaignAuditId +"\\;" + item2.StartTime.ToString("dd-MM-yy hh:mm:ss") + "\\;" + item2.EndTime.ToString("dd-MM-yy hh:mm:ss") + "\\;" + item2.PlayLengthTicks + "\\;" + advertname + "\\;" + advertid + "\\;" + brandname + "\\;" + advertid + "\\;" + item2.Status + "\\;" + Adcreditcost + "\\;" + smsstatus + "\\;" + SMScost + "\\;" + emailstatus + "\\;" + Emailcost + "\\;" + TotalCost + "\\;" + currency + "\\;" + localZone.StandardName  + "\n";
        //                    csv += MSISDN + ";" + item2.CampaignAuditId + ";" + item2.StartTime.ToString("ddMMyyhhmmss") + ";" + item2.EndTime.ToString("ddMMyyhhmmss") + ";" + PlayLength + ";" + advertname.Replace(",", " ") + ";" + advertid + ";" + brandname.Replace(",", " ") + ";" + advertid + ";" + item2.Status + ";" + Adcreditcost + ";" + smsstatus + ";" + SMScost + ";" + emailstatus + ";" + Emailcost + ";" + TotalCost + ";" + currency + ";" + "GMT" + "\n";
        //                }


        //                System.IO.File.WriteAllText(DailyReportPath, csv);
        //                System.IO.File.WriteAllText(ArchiveReportPath, csv);


        //                ViewBag.Window = true;
        //            }

        //            #endregion


        //        }
        //    }
        //    return View();
        //}

        //Adds an ACL entry on the specified file for the specified account.
        public static void AddFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {


            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = System.IO.File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            System.IO.File.SetAccessControl(fileName, fSecurity);

        }

        // Adds an ACL entry on the specified directory for the specified account.
        //public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        //{
        //    // Create a new DirectoryInfo object.
        //    DirectoryInfo dInfo = new DirectoryInfo(FileName);

        //    // Get a DirectorySecurity object that represents the 
        //    // current security settings.
        //    DirectorySecurity dSecurity = dInfo.GetAccessControl();

        //    // Add the FileSystemAccessRule to the security settings. 
        //    dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
        //                                                    Rights,
        //                                                    ControlType));

        //    // Set the new access settings.
        //    dInfo.SetAccessControl(dSecurity);

        //}

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        private IEnumerable<Model.CampaignAudit> GetAdvertData(int campaignProfileId)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var AdvertData = _campaignAuditRepository.GetMany(s => s.CampaignProfileId == campaignProfileId && s.StartTime >= yesterday && s.EndTime < DateTime.Now);
            return AdvertData;
        }
    }
}