using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.CompanyDetails;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Mailer;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using EFMVC.Data;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Web.Common;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]

    public class UserProfileController : Controller
    {
        private ISendEmailMailer _sendMailer = new SendEmailMailer();
        public ISendEmailMailer SendMailer
        {
            get { return _sendMailer; }
            set { _sendMailer = value; }
        }
        public void SendemailusingMVCMailer()
        {
            //var EmailContent = new SendEmailModel();
            //EmailContent.To = "jigar@zealousweb.com";
            //EmailContent.Fname = "First";
            //EmailContent.Lname = "Last";
            //EmailContent.Subject = "Testing email";
            //EmailContent.FormatId = 1;

            //SendMailer.SendEmail(EmailContent).SendAsync();
        }
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The contacts repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companydetailsRepository;

        private readonly ICountryRepository _countryRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IUserTokenLinkRepository _userTokenLinkRepository;
        private readonly ICampaignAuditRepository _campaignAuditRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
     
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="contactsRepository">The contacts repository.</param>
        /// <param name="companydetailsRepository">The companydetails repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public UserProfileController(ICommandBus commandBus, IContactsRepository contactsRepository, ICompanyDetailsRepository companydetailsRepository, IUserRepository userRepository, ICountryRepository countryRepository, IUserTokenLinkRepository userTokenLinkRepository, ICampaignAuditRepository campaignAuditRepository, ICurrencyRepository currencyRepository)
        {
            _commandBus = commandBus;
            _contactsRepository = contactsRepository;
            _companydetailsRepository = companydetailsRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _userTokenLinkRepository = userTokenLinkRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _currencyRepository = currencyRepository;
        }

        //string strCode1 = null;
        //string strCode2 = null;
        //string strCode3 = null;
        static string strMobileCode1 = null;
        static string strMobileCode2 = null;
        static string strMobileCode3 = null;

        private void FillCountryList()
        {
            var country = (from action in _countryRepository.GetAll()
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();
            ViewBag.country = country;
        }

        private void FillCountry(ContactsFormModel contactmodel)
        {
            var countryData = _countryRepository.GetAll().OrderBy(top => top.Name);
            var countryResult = Mapper.Map<IEnumerable<Country>, IEnumerable<CountryFormModel>>(countryData);
            ViewBag.countryList = countryResult.Select(s => new SelectListItem { Text = s.Name + " " + s.CountryCode, Value = s.Id.ToString() }).ToList();
        }

        [Route("UserProfile/Index")]
        public ActionResult Index()
        {
            FillCountryList();
            AccountInfo _accountInfo = new AccountInfo();
            UserProfileInfo _profileinfo = new UserProfileInfo();
            ChangePasswordFormModel _changepassword = new ChangePasswordFormModel();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var userinfo = _userRepository.GetById(efmvcUser.UserId);
            if (userinfo != null)
            {
                ViewBag.emailAddress = userinfo.Email;
                _profileinfo.Email = userinfo.Email;
                _profileinfo.FirstName = userinfo.FirstName;
                _profileinfo.LastName = userinfo.LastName;
                _profileinfo.Organisation = userinfo.Organisation;
                ViewBag.IsEmailVerified = userinfo.VerificationStatus;
                ViewBag.IsMobileVerfication = userinfo.IsMobileVerfication;
            }
            ContactsFormModel contactmodel = new ContactsFormModel();
            FillCountry(contactmodel);
            CompanyDetailsFormModel companymodel = new CompanyDetailsFormModel();
            var _contactinfo = _contactsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            if (_contactinfo != null)
            {
                contactmodel = Mapper.Map<Contacts, ContactsFormModel>(_contactinfo);
                if (!string.IsNullOrEmpty(contactmodel.MobileNumber))
                {
                    contactmodel.MobileNumber = "+" + contactmodel.MobileNumber;
                    //contactmodel.MobileNumber = contactmodel.MobileNumber;
                    ViewBag.MobileNumber = contactmodel.MobileNumber;
                    if (_contactinfo.CountryId == null)
                    {
                        var countryCode = "+" + _contactinfo.MobileNumber.Substring(0, 2);
                        var data = _countryRepository.GetAll().Select(top => top.CountryCode == countryCode).FirstOrDefault();
                        if (data == false)
                        {
                            contactmodel.CountryId = 7;
                        }
                        else
                        {
                            contactmodel.CountryId = _countryRepository.GetAll().Where(top => top.CountryCode == countryCode).Select(top => top.Id).FirstOrDefault();
                        }
                    }
                    else
                    {
                        contactmodel.CountryId = _contactinfo.CountryId;
                    }
                }
                else
                {
                    contactmodel.CountryId = 7;
                }
                _accountInfo.ContactsFormModel = contactmodel;
            }
            else
            {
                _accountInfo.ContactsFormModel = contactmodel;
            }
            var _companyInfo = _companydetailsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            if (_companyInfo != null)
            {
                companymodel = Mapper.Map<CompanyDetails, CompanyDetailsFormModel>(_companyInfo);
                _accountInfo.CompanyDetailsFormModel = companymodel;

                _accountInfo.CompanyDetailsFormModel.CountryId = companymodel.CountryId;
            }
            else
            {
                _accountInfo.CompanyDetailsFormModel = companymodel;
            }
            _accountInfo.UserProfileInfo = _profileinfo;
            _accountInfo.ChangePasswordFormModel = _changepassword;

            var userTokenData = _userTokenLinkRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
            if (userTokenData != null)
                ViewBag.Provider = userTokenData.Provider.ToUpper();
            else
                ViewBag.Provider = "";

            return View(_accountInfo);
        }

        [Route("SaveContactInfo")]
        [HttpPost]
        public ActionResult SaveContactInfo(ContactsFormModel ContactsFormModel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        if(!string.IsNullOrEmpty(ContactsFormModel.MobileNumber))
                            ContactsFormModel.MobileNumber = ContactsFormModel.MobileNumber.Replace("+", "");
                        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                        //Add 18-04-2019
                        var userExist = _userRepository.Get(top => top.Email.ToLower().Equals(ContactsFormModel.Email.ToLower()) && top.UserId != efmvcUser.UserId);
                        if (userExist != null)
                        {
                            return Json(ContactsFormModel.Email + " Email Address is already exists in database so please choose another one.");
                        }

                        if (ContactsFormModel.MobileNumber != null && ContactsFormModel.MobileNumber != "")
                        {
                            var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(ContactsFormModel.MobileNumber) && top.UserId != efmvcUser.UserId);
                            if (contactExist != null)
                            {
                                return Json(ContactsFormModel.MobileNumber + " Mobile Number is already exists in database so please choose another one.");
                            }
                        }

                        EFMVCDataContex db = new EFMVCDataContex();
                        var contactInfo = db.Contacts.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if(contactInfo != null)
                        {
                            if(contactInfo.MobileNumber != ContactsFormModel.MobileNumber)
                            {
                                var userData = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                                if(userData != null)
                                {
                                    userData.IsMobileVerfication = false;
                                    db.SaveChanges();
                                }
                            }
                        }

                        CreateOrUpdateContactsCommand command =
                            Mapper.Map<ContactsFormModel, CreateOrUpdateContactsCommand>(ContactsFormModel);
                        command.UserId = efmvcUser.UserId;

                        if(!string.IsNullOrEmpty(contactInfo.CountryId.ToString()))
                        {
                            if(contactInfo.CountryId == ContactsFormModel.CountryId)
                            {
                                if (contactInfo.CountryId == 13 || ContactsFormModel.CountryId == 13)
                                {
                                    var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                    command.CurrencyId = currency.CurrencyId;
                                }
                                //Add 06-02-2019
                                else if (contactInfo.CountryId == 11 || ContactsFormModel.CountryId == 11)
                                {
                                    command.CurrencyId = _currencyRepository.Get(top => top.CurrencyCode.ToLower().Equals("USD".ToLower())).CurrencyId;
                                }
                                else
                                {
                                    command.CurrencyId = ContactsFormModel.CurrencyId;
                                }
                            }
                            else
                            {
                                if (contactInfo.CountryId == 13 || ContactsFormModel.CountryId == 13)
                                {
                                    var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                    command.CurrencyId = currency.CurrencyId;
                                }
                                //Add 06-02-2019
                                else if (contactInfo.CountryId == 11 || ContactsFormModel.CountryId == 11)
                                {
                                    command.CurrencyId = _currencyRepository.Get(top => top.CurrencyCode.ToLower().Equals("USD".ToLower())).CurrencyId;
                                }
                                else
                                {
                                    command.CurrencyId = _currencyRepository.GetAll().Where(top => top.CountryId == ContactsFormModel.CountryId).Select(top => top.CurrencyId).FirstOrDefault();
                                }
                            }
                        }
                        //Add 06-02-2019
                        else if (contactInfo.CountryId == 11 || ContactsFormModel.CountryId == 11)
                        {
                            command.CurrencyId = _currencyRepository.Get(top => top.CurrencyCode.ToLower().Equals("USD".ToLower())).CurrencyId;
                        }
                        else
                        {
                            command.CurrencyId = _currencyRepository.GetAll().Where(top => top.CountryId == ContactsFormModel.CountryId).Select(top => top.CurrencyId).FirstOrDefault();
                        }
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }

                    return Json("fail");
                }
                else
                {
                    return Json("notauthorise");
                }
            }
            catch (Exception ex)
            {

                return Json(ex.InnerException.Message);
            }
        }

        [Route("SaveCompanyInfo")]
        [HttpPost]
        public ActionResult SaveCompanyInfo(CompanyDetailsFormModel CompanyFormModel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                        CreateOrUpdateCompanyDetailsCommand command =
                            Mapper.Map<CompanyDetailsFormModel, CreateOrUpdateCompanyDetailsCommand>(CompanyFormModel);
                        command.UserId = efmvcUser.UserId;
                        command.CountryId = CompanyFormModel.CountryId;
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {

                            return Json("success");
                        }
                    }
                    return Json("fail");
                }
                else
                {
                    return Json("notauthorise");
                }
            }
            catch (Exception ex)
            {

                return Json(ex.InnerException.Message);
            }
        }

        [Route("UpdateUserInfo")]
        [HttpPost]
        public ActionResult UpdateUserInfo(string email, string oldemail, string firstname, string lastname, string Organisation)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var _existsemail = _userRepository.GetMany(top => top.Email.Trim().ToLower() == email.Trim().ToLower() && top.UserId != efmvcUser.UserId).FirstOrDefault();
                    if (_existsemail != null)
                    {
                        //Comment 18-04-2019
                        //if (_existsemail.Activated == 3)
                        //{
                        //    //use deleted email address
                        //}
                        //else
                        //{
                        //return Json("The Email Address is already exists in database. so please choose another one.");
                        return Json(email + " Email Address is already exists in database so please choose another one.");
                        //}
                    }
                    var command = new ChangeUserProfileInfoCommand
                    {
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname,
                        Organisation = Organisation,
                        VerificationStatus = false,
                        UserId = efmvcUser.UserId
                    };

                    ICommandResult commandResult = _commandBus.Submit(command);

                    if (commandResult.Success)
                    {
                        if (email != oldemail)
                        {
                            //sendemailtonewuser(email, oldemail);
                            //sendemailtoolduser(email, oldemail);

                            SendEmailVerificationCode(email);
                            EFMVCDataContex db = new EFMVCDataContex();
                            var userInfo = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                            if (userInfo != null)
                            {
                                userInfo.IsEmailVerfication = false;
                                db.SaveChanges();
                            }

                        }
                        return Json("success");
                    }
                    return Json("fail");
                }
                else
                {
                    return Json("notauthorise");
                }
            }
            catch (Exception ex)
            {

                return Json(ex.InnerException.Message.ToString());
            }

        }

        [Route("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordFormModel form)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var command = new ChangePasswordCommand
                    {
                        UserId = efmvcUser.UserId,
                        OldPassword = form.OldPassword,
                        NewPassword = form.NewPassword
                    };
                    IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
                    ModelState.AddModelErrors(errors);
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            var userinfo = _userRepository.GetById(efmvcUser.UserId);
                            if (userinfo != null)
                            {
                                changepassowrd(userinfo.Email);
                            }
                            return Json("success");
                        }
                        else
                        {
                            return Json("The current password is incorrect or the new password is invalid.");
                        }
                    }
                    else
                    {
                        string error = String.Empty;
                        foreach (var item in errors)
                        {
                            error = error + item.Message + "<br/>";
                        }
                        return Json(error);
                    }
                }
                // If we got this far, something failed, redisplay form
                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        public void changepassowrd(string email)
        {

            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["ChangePassowordTemplate"]));
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, email);
            MailSending("support@adtones.xyz", "Supp0rtPa55w0rd!", "ChangePassword", email, emailContent, "smtp.gmail.com", 587, true);

        }

        [Route("DeleteAdvertiserAccount")]
        public ActionResult DeleteAdvertiserAccount()
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (efmvcUser != null)
                {
                    var userinfo = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                    int countryId = 0;
                    var contactDetails = _contactsRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                    if(contactDetails != null)
                    {
                        countryId = contactDetails.CountryId == null ? 0 : (int)contactDetails.CountryId;
                    }

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);


                    var campaignProfileData = db.CampaignProfiles.Where(s => s.UserId == efmvcUser.UserId).ToList();

                    if (campaignProfileData.Count() > 0)
                    {
                       
                        DeletePrematchData(campaignProfileData);
                       
                        RemoveUserDataFromRefernceTable(efmvcUser.UserId);
                        userinfo.Activated = (int)UserStatus.Deleted;
                        db.SaveChanges();
                        foreach (var camp in campaignProfileData)
                        {
                            camp.Status = (int)CampaignStatus.Archive;
                           // db.CampaignProfiles.RemoveRange(campaignProfileData);                           
                            db.SaveChanges();

                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db2 = new EFMVCDataContex(item);
                                    //var campaignProfilePreference = db2.CampaignProfilePreference.Where(s => campaignProfileIdList.Contains(s.CampaignProfileId)).ToList();
                                    //if (campaignProfilePreference.Count() > 0)
                                    //{
                                    //    db2.CampaignProfilePreference.RemoveRange(campaignProfilePreference);
                                    //    db2.SaveChanges();
                                    //}
                                    var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, efmvcUser.UserId);
                                    var campaignProfileDataOperatorDB = db2.CampaignProfiles.Where(s => s.UserId == externalServerUserId).ToList();
                                    if (campaignProfileDataOperatorDB.Count() > 0)
                                    {
                                        //db2.CampaignProfiles.RemoveRange(campaignProfileDataOperatorDB);
                                        foreach(var camprofileOperator in campaignProfileDataOperatorDB)
                                        {
                                            camprofileOperator.Status = (int)CampaignStatus.Archive;
                                            db2.SaveChanges();
                                        }
                                    }

                                    var userData = db2.Users.Where(s => s.UserId == externalServerUserId).FirstOrDefault();
                                    if (userData != null)
                                    {
                                        userData.Activated = (int)UserStatus.Deleted;
                                        // db2.Users.Remove(userData);
                                    }
                                    db2.SaveChanges();
                                }
                            }
                        }
                        
                        
                    }
                    else
                    {
                        RemoveUserDataFromRefernceTable(efmvcUser.UserId);
                        // db.Users.Remove(userinfo);
                        userinfo.Activated = (int)UserStatus.Deleted;
                        db.SaveChanges();

                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex db2 = new EFMVCDataContex(item);
                                var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, efmvcUser.UserId);
                                var userData = db2.Users.Where(s => s.UserId == externalServerUserId).FirstOrDefault();
                                if (userData != null)
                                {
                                    userData.Activated = (int)UserStatus.Deleted;
                                    //db2.Users.Remove(userData);
                                }
                                db2.SaveChanges();
                            }
                        }
                    }



                    return Json("Success");

                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Error");
            }

        }

        private void DeletePrematchData(List<CampaignProfile> CampaignProfile)
        {
            foreach(var item in CampaignProfile)
            {
                var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var conn in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(conn);
                        var campaignProfileIdList = db.CampaignProfiles.Where(s=>s.AdtoneServerCampaignProfileId == item.CampaignProfileId).Select(s => s.CampaignProfileId).ToList();

                        List<string> strCampaignList = campaignProfileIdList.ConvertAll<string>(x => x.ToString());
                        var PrematchData = db.PreMatch.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData.Count() > 0)
                        {
                            db.PreMatch.RemoveRange(PrematchData);
                            db.SaveChanges();
                        }

                        var PrematchData2 = db.PreMatch2.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData2.Count() > 0)
                        {
                            db.PreMatch2.RemoveRange(PrematchData2);
                            db.SaveChanges();
                        }

                        var PrematchData3 = db.PreMatch3.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData3.Count() > 0)
                        {
                            db.PreMatch3.RemoveRange(PrematchData3);
                            db.SaveChanges();
                        }

                        var PrematchData4 = db.PreMatch4.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData4.Count() > 0)
                        {
                            db.PreMatch4.RemoveRange(PrematchData4);
                            db.SaveChanges();
                        }

                        var PrematchData5 = db.PreMatch5.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData5.Count() > 0)
                        {
                            db.PreMatch5.RemoveRange(PrematchData5);
                            db.SaveChanges();
                        }

                        var PrematchData6 = db.PreMatch6.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData6.Count() > 0)
                        {
                            db.PreMatch6.RemoveRange(PrematchData6);
                            db.SaveChanges();
                        }

                        var PrematchData7 = db.PreMatch7.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData7.Count() > 0)
                        {
                            db.PreMatch7.RemoveRange(PrematchData7);
                            db.SaveChanges();
                        }

                        var PrematchData8 = db.PreMatch8.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData8.Count() > 0)
                        {
                            db.PreMatch8.RemoveRange(PrematchData8);
                            db.SaveChanges();
                        }

                        var PrematchData9 = db.PreMatch9.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData9.Count() > 0)
                        {
                            db.PreMatch9.RemoveRange(PrematchData9);
                            db.SaveChanges();
                        }

                        var PrematchData10 = db.PreMatch10.Where(s => strCampaignList.Contains(s.MSCampaignProfileId)).ToList();
                        if (PrematchData10.Count() > 0)
                        {
                            db.PreMatch10.RemoveRange(PrematchData10);
                            db.SaveChanges();
                        }
                    }
                }
                
            }
           
        }

        private void RemoveUserDataFromRefernceTable(int userId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == userId).ToList();
            foreach (var item in emailVerificationCode)
            {
                db.EmailVerificationCode.Remove(item);
                db.SaveChanges();
            }

            var advertDetails = db.Adverts.Where(s => s.UserId == userId).ToList();
            foreach(var item in advertDetails)
            {
                ChangeAdvertStatus(db, item);
                var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var conn in ConnString)
                    {
                        EFMVCDataContex db2 = new EFMVCDataContex(conn);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                        var advertInfoData = db2.Adverts.Where(s => s.UserId == externalServerUserId).ToList();
                        foreach(var item2 in advertInfoData)
                        {
                            ChangeAdvertStatus(db2, item2);
                        }
                        
                    }
                }
            }

            var contactInfo = db.Contacts.Where(s => s.UserId == userId).FirstOrDefault();
            if (contactInfo != null)
            {

                var ConnString = ConnectionString.GetConnectionStringByCountryId(contactInfo.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var conn in ConnString)
                    {
                        EFMVCDataContex db2 = new EFMVCDataContex(conn);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                        var contactInfoDetail = db2.Contacts.Where(s => s.UserId == externalServerUserId).FirstOrDefault();
                        if (contactInfoDetail != null)
                        {
                            //db2.Contacts.Remove(contactInfoDetail);
                            contactInfoDetail.MobileNumber = null;
                            db2.SaveChanges();
                        }
                    }
                }
                // db.Contacts.Remove(contactInfo);
                contactInfo.MobileNumber = null;
                db.SaveChanges();

            }

            var clientInfo = db.Clients.Where(s => s.UserId == userId).ToList();
            if (clientInfo.Count() > 0)
            {
                foreach(var item in clientInfo)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(contactInfo.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var clientDetail = db2.Clients.Where(s => s.UserId == externalServerUserId).ToList();
                            if (clientDetail.Count() > 0)
                            {
                                foreach(var item2 in clientDetail)
                                {
                                    item2.Status = (int)ClientStatus.Archived;
                                    //db2.Clients.Remove(clientDetail);
                                    db2.SaveChanges();
                                }                           
                            }
                        }
                    }
                    item.Status = (int)ClientStatus.Archived;
                  //  db.Clients.Remove(clientInfo);
                    db.SaveChanges();
                }
                
            }

            //var companyInfo = db.CompanyDetails.Where(s => s.UserId == userId).FirstOrDefault();
            //if (companyInfo != null)
            //{
               
            //    var ConnString = ConnectionString.GetConnectionStringByCountryId(companyInfo.CountryId);
            //    if (ConnString != null && ConnString.Count() > 0)
            //    {
            //        foreach (var conn in ConnString)
            //        {
            //            EFMVCDataContex db2 = new EFMVCDataContex(conn);
            //            var companyInfoDetail = db2.CompanyDetails.Where(s => s.UserId == userId).FirstOrDefault();
            //            if (companyInfoDetail != null)
            //            {
            //                db2.CompanyDetails.Remove(companyInfoDetail);
            //                db2.SaveChanges();
            //            }
            //        }
            //    }
            //    db.CompanyDetails.Remove(companyInfo);
            //    db.SaveChanges();

            //}

          

            var userTokenInfo = db.UserTokenLink.Where(s => s.UserId == userId).FirstOrDefault();
            if (userTokenInfo != null)
            {
                db.UserTokenLink.Remove(userTokenInfo);
                db.SaveChanges();
            }

            var campaignMatchData = db.CampaignMatch.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData.Count() > 0)
            {            
                foreach(var item in campaignMatchData)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch.Where(s => s.UserId == externalServerUserId).ToList(); 
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach(var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                                //db2.CampaignMatch.RemoveRange(campaignMatchDetail);
                                
                            }
                        }
                    }

                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }

                //db.CampaignMatch.RemoveRange(campaignMatchData);
               
            }

            var campaignMatchData2 = db.CampaignMatch2.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData2.Count() > 0)
            {
                foreach (var item in campaignMatchData2)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch2.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                                //db2.CampaignMatch2.RemoveRange(campaignMatchDetail);
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }
                //db.CampaignMatch2.RemoveRange(campaignMatchData2);
               
            }

            var campaignMatchData3 = db.CampaignMatch3.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData3.Count() > 0)
            {
                foreach (var item in campaignMatchData3)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch3.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }

               // db.CampaignMatch3.RemoveRange(campaignMatchData3);
            }

            var campaignMatchData4 = db.CampaignMatch4.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData4.Count() > 0)
            {
                foreach (var item in campaignMatchData4)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch4.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }
                //db.CampaignMatch4.RemoveRange(campaignMatchData4);
            }

            var campaignMatchData5 = db.CampaignMatch5.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData5.Count() > 0)
            {
                foreach (var item in campaignMatchData5)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch5.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }

               // db.CampaignMatch5.RemoveRange(campaignMatchData5);
            }

            var campaignMatchData6 = db.CampaignMatch6.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData6.Count() > 0)
            {
                foreach (var item in campaignMatchData6)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch6.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }
               // db.CampaignMatch6.RemoveRange(campaignMatchData6);
            }

            var campaignMatchData7 = db.CampaignMatch7.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData7.Count() > 0)
            {
                foreach (var item in campaignMatchData7)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch7.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }
                //db.CampaignMatch7.RemoveRange(campaignMatchData7);
            }

            var campaignMatchData8 = db.CampaignMatch8.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData8.Count() > 0)
            {
                foreach (var item in campaignMatchData8)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch8.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }
                //db.CampaignMatch8.RemoveRange(campaignMatchData8);
            }

            var campaignMatchData9 = db.CampaignMatch9.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData9.Count() > 0)
            {
                foreach (var item in campaignMatchData9)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch9.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }

               // db.CampaignMatch9.RemoveRange(campaignMatchData9);
            }

            var campaignMatchData10 = db.CampaignMatch10.Where(s => s.UserId == userId).ToList();
            if (campaignMatchData10.Count() > 0)
            {
                foreach (var item in campaignMatchData10)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(item.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var conn in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(conn);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db2, userId);
                            var campaignMatchDetail = db2.CampaignMatch10.Where(s => s.UserId == externalServerUserId).ToList();
                            if (campaignMatchDetail.Count() > 0)
                            {
                                foreach (var item2 in campaignMatchDetail)
                                {
                                    item2.Status = (int)CampaignStatus.Archive;
                                    db2.SaveChanges();
                                }
                            }
                        }
                    }
                    item.Status = (int)CampaignStatus.Archive;
                    db.SaveChanges();
                }

               // db.CampaignMatch10.RemoveRange(campaignMatchData10);
            }
        }

        private void ChangeAdvertStatus(EFMVCDataContex db, Advert item)
        {
            //var campaignAdvert = db.CampaignAdverts.Where(s => s.AdvertId == item.AdvertId).ToList();
            //foreach (var item2 in campaignAdvert)
            //{
            //    db.CampaignAdverts.Remove(item2);
            //    db.SaveChanges();
            //}

            item.Status = (int)AdvertStatus.Archived;
           // db.Adverts.Remove(item);
            db.SaveChanges();
        }

        [Route("ResendEmail")]
        public ActionResult ResendEmail(string emailAddress)
        {
            try
            {
                SendEmailVerificationCode(emailAddress);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }

        private void SendEmailVerificationCode(string email)
        {
            var reader = new StreamReader(
                                   Server.MapPath(ConfigurationManager.AppSettings["UsersResendVerificationEmailTemplate"]));
            string emailContent = reader.ReadToEnd();

            Random generator = new Random();
            String emailcode1 = generator.Next(0, 99).ToString("D2");
            String emailcode2 = generator.Next(0, 99).ToString("D2");
            String emailcode3 = generator.Next(0, 99).ToString("D2");

            //strCode1 = emailcode1;
            //strCode2 = emailcode2;
            //strCode3 = emailcode3;

            string verifyCode = emailcode1 + " " + emailcode2 + " " + emailcode3 + " ";

            emailContent = string.Format(emailContent, verifyCode);

            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
            mail.Subject = "Email Verification";

            mail.Body = emailContent;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential
                 (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
            smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

            //Or your Smtp Email ID and Password
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
            smtp.Send(mail);

            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            EFMVCDataContex db = new EFMVCDataContex();
            EmailVerificationCode emailVerification = new EmailVerificationCode();

            emailVerification.UserId = efmvcUser.UserId;
            emailVerification.VerificationCode = emailcode1 + "-" + emailcode2 + "-" + emailcode3;
            emailVerification.DateCreated = DateTime.Now;
            db.EmailVerificationCode.Add(emailVerification);
            db.SaveChanges();

        }

        [Route("ConfirmEmailVerificationCode")]
        public ActionResult ConfirmEmailVerificationCode(string confirmEmailCode)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                EFMVCDataContex db = new EFMVCDataContex();
                var verificationDetail = db.EmailVerificationCode.Where(s => s.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id).FirstOrDefault();
                var removeVerificationDetail = db.EmailVerificationCode.Where(s => s.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id).ToList();
                if (verificationDetail != null)
                {
                    if (verificationDetail.VerificationCode == confirmEmailCode)
                    {
                        db.EmailVerificationCode.RemoveRange(removeVerificationDetail);
                        db.SaveChanges();

                        User user = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            user.IsEmailVerfication = true;
                            user.VerificationStatus = true;
                            db.SaveChanges();

                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Fail", JsonRequestBehavior.AllowGet);
                        }


                    }
                    else
                    {
                        return Json("Fail", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("ResendMobileNumber")]
        public ActionResult ResendMobileNumber(string mobileNumber)
        {           
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var contactInfo = _contactsRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
               
                var statusCode = SendSms(mobileNumber);
                var resultData = statusCode.Result;
                if (resultData == "OK")
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }

        private async Task<string> SendSms(string number)
        {
            var client = new HttpClient();

            //https://www.voodoosms.com/vapi/server/sendSMS?uid=adtones_sms_api&pass=2x8whkr&dest=447980720250&orig=Adtones&msg=your%20code%20is%2012%2012%2012&format=JSON
            //Get the parameters, either GET or POST request

        

            string uid = "adtones_sms_api";
            string pass = "2x8whkr";
            // string dest = "447980720250";
            string dest =  number;

            string orig = "Adtone"; // number
            //string orig = "447860064145";
            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            //TempData["code1"] = code1;
            //TempData["code2"] = code2;
            //TempData["code3"] = code3;

            strMobileCode1 = code1;
            strMobileCode2 = code2;
            strMobileCode3 = code3;


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

        [Route("ConfirmMobileVerificationCode")]
        public ActionResult ConfirmMobileVerificationCode(string confirmMobileCode, string mobileNumber)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var code = confirmMobileCode.Split('-');

                var code1 = strMobileCode1;
                var code2 = strMobileCode2;
                var code3 = strMobileCode3;

                var receivedCode1 = code[0];
                var receivedCode2 = code[1];
                var receivedCode3 = code[2];

                if (code1 != null && code1 != null && code1 != null)
                {
                    if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();
                        var userData = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if(userData != null)
                        {
                            userData.IsMobileVerfication = true;
                            db.SaveChanges();
                        }
                        var contactData = db.Contacts.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (contactData != null)
                        {
                            contactData.MobileNumber = mobileNumber;
                            db.SaveChanges();
                        }
                        else
                        {
                            Contacts cont = new Contacts();
                            cont.MobileNumber = mobileNumber;
                            cont.UserId = efmvcUser.UserId;
                            db.Contacts.Add(cont);
                            db.SaveChanges();
                        }
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }




        public void sendemailtonewuser(string newemail, string oldemail)
        {

            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["ChangeEmailAddressTemplate"]));
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, oldemail, newemail);
            MailSending("support@adtones.xyz", "Supp0rtPa55w0rd!", "ChangePassword", newemail, emailContent, "smtp.gmail.com", 587, true);

        }
        public void sendemailtoolduser(string newemail, string oldemail)
        {

            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["ChangeOldEmailAddressTemplate"]));
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, oldemail, newemail);
            MailSending("support@adtones.xyz", "Supp0rtPa55w0rd!", "ChangePassword", oldemail, emailContent, "smtp.gmail.com", 587, true);

        }
        public void MailSending(string Username, string Password, string mailSubject, string mailTo, string mailBody, string host, int port, bool EnableSSL)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            mail.From = new MailAddress(Username, Username);
            mail.Subject = mailSubject;
            mail.To.Add(mailTo);
            mail.Body = mailBody;
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;


            client.Port = port;
            client.EnableSsl = EnableSSL;
            if (String.IsNullOrEmpty(Username) && String.IsNullOrEmpty(Password))
            {
                client.UseDefaultCredentials = true;

            }
            else
            {
                client.UseDefaultCredentials = false;

            }
            client.Host = host;
            client.Credentials = new NetworkCredential(Username, Password);
            client.SendMailAsync(mail);
        }

    }
}
