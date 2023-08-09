using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EFMVC.Web.Common
{
    public class AdvertEmail
    {
        private readonly ICommandBus _commandBus;
        private readonly IUserRepository _userRepository;
        public AdvertEmail(ICommandBus commandBus, IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
        }

        public void SendMail(string AdvertName, int? OperatorAdmin, int UserId, string CampaignName, string CountryName, string OperatorName, DateTime AdvertDateTime)
        {
            try
            {
                string url = "";
                string advertURL = "";
                string siteAddress = ConfigurationManager.AppSettings["siteAddress"];

                int advertAdmin = (int)UserRole.AdvertAdmin;
                int operatorAdmin = (int)UserRole.OperatorAdmin;
                var advertAdminDetails = _userRepository.Get(top => top.RoleId == advertAdmin && top.Activated == 1);
                //var operatorAdminDetails = _userRepository.Get(top => top.OperatorId == OperatorAdmin.Value && top.RoleId == operatorAdmin && top.Activated == 1);
                var operatorAdminDetails = _userRepository.GetMany(top => top.OperatorId == OperatorAdmin.Value && top.RoleId == operatorAdmin && top.Activated == 1).ToList();
                var advertiserDetails = _userRepository.GetById(UserId);

                TimeZone curTimeZone = TimeZone.CurrentTimeZone;
                DateTime advertUTC = curTimeZone.ToUniversalTime(AdvertDateTime);

                string subject = "New Advert: " + AdvertName + " - Campaign: " + CampaignName + " - Advertiser: " + advertiserDetails.FirstName + " " + advertiserDetails.LastName;
                string emailContent = "";
                //var reader =
                //        new StreamReader(
                //            HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AdvertEmailTemplate"]));
                //string emailContent = reader.ReadToEnd();
                //emailContent = string.Format(emailContent, AdvertName);

                if (advertAdminDetails != null)
                {
                    var reader =
                        new StreamReader(
                            HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AdvertEmailTemplateForAdvertAdmin"]));
                    emailContent = reader.ReadToEnd();
                    advertURL = "<a href='" + siteAddress + "AdvertAdmin/UserAdvert/Index'>" + AdvertName + "</a>";
                    url = siteAddress + "AdvertAdmin/UserAdvert/Index";
                    url = "<a href='" + url + "'>" + url + " </a>";

                    //var url = siteAddress + "AdvertAdmin/UserAdvert/Index";
                    //emailContent = string.Format(emailContent, AdvertName, url);
                    emailContent = string.Format(emailContent, advertURL, CampaignName, CountryName, OperatorName, advertUTC, url);

                    MailMessage mail = new MailMessage();
                    mail.To.Add(advertAdminDetails.Email);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                    mail.Subject = subject;

                    mail.Body = emailContent.Replace("\n", "<br/>");

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                    smtp.Send(mail);
                }

                if (operatorAdminDetails.Count() > 0)
                {
                    foreach (var operatorAdminData in operatorAdminDetails)
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient smtp = new SmtpClient();

                        var safaricomOperatorAdminSiteAddress = "";
                        string campaignURL = "";

                        var reader =
                            new StreamReader(
                                HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AdvertEmailTemplateForOperatorAdmin"]));
                        emailContent = reader.ReadToEnd();

                        if (operatorAdminData.OperatorId == (int)OperatorTableId.Safaricom)
                        {
                            safaricomOperatorAdminSiteAddress = ConfigurationManager.AppSettings["SafaricomOperatorAdminSiteAddress"].ToString();
                            advertURL = "<a href='" + safaricomOperatorAdminSiteAddress + "OperatorAdmin/UserAdvert/Index'>" + AdvertName + "</a>";
                            campaignURL = "<a href='" + safaricomOperatorAdminSiteAddress + "OperatorAdmin/UserCampaign/Index'>" + CampaignName + "</a>";
                            url = safaricomOperatorAdminSiteAddress + "OperatorAdmin/UserAdvert/Index";
                            url = "<a href='" + url + "'>" + url + " </a>";

                            mail.To.Add(operatorAdminData.Email);
                            mail.From = new MailAddress(ConfigurationManager.AppSettings["SafaricomSiteEmailAddress"]);

                            emailContent = string.Format(emailContent, advertURL, campaignURL, advertUTC, url);

                            mail.Subject = subject;
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.IsBodyHtml = true;

                            smtp.Host = ConfigurationManager.AppSettings["SafaricomSmtpServerAddress"]; //Or Your SMTP Server Address
                            smtp.Credentials = new System.Net.NetworkCredential
                                 (ConfigurationManager.AppSettings["SafaricomSMTPEmail"].ToString(), ConfigurationManager.AppSettings["SafaricomSMTPPassword"].ToString()); // ***use valid credentials***
                            smtp.Port = int.Parse(ConfigurationManager.AppSettings["SafaricomSmtpServerPort"]);

                            //Or your Smtp Email ID and Password
                            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SafaricomEnableEmailSending"].ToString());
                        }
                        else
                        {
                            advertURL = "<a href='" + siteAddress + "OperatorAdmin/UserAdvert/Index'>" + AdvertName + "</a>";
                            campaignURL = "<a href='" + siteAddress + "OperatorAdmin/UserCampaign/Index'>" + CampaignName + "</a>";
                            url = siteAddress + "OperatorAdmin/UserAdvert/Index";
                            url = "<a href='" + url + "'>" + url + " </a>";

                            mail.To.Add(operatorAdminData.Email);
                            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);

                            emailContent = string.Format(emailContent, advertURL, campaignURL, advertUTC, url);

                            mail.Subject = subject;
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.IsBodyHtml = true;
                            
                            smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                            smtp.Credentials = new System.Net.NetworkCredential
                                 (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                            smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                            //Or your Smtp Email ID and Password
                            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                        }
                        
                        smtp.Send(mail);
                    }
                }
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message.ToString();
            }
        }
    }
}