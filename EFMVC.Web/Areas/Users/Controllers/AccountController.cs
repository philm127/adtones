using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.CompanyDetails;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Domain.Commands.Security;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Mailer;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;
using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("Account")]
    public class AccountController : Controller
    {
        // GET: Users/Account
        private ISendEmailMailer _sendMailer = new SendEmailMailer();
        public ISendEmailMailer SendMailer
        {
            get { return _sendMailer; }
            set { _sendMailer = value; }
        }
        public void SendemailusingMVCMailer()
        {
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

        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        private readonly IUserTokenLinkRepository _userTokenLinkRepository;

        private readonly IUserProfileRepository _userProfileRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        private readonly IRewardRepository _rewardRepository;

        private readonly IUserRewardRepository _userRewardRepository;

        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="contactsRepository">The contacts repository.</param>
        /// <param name="companydetailsRepository">The companydetails repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// 
        static string strCode1 = null;
        static string strCode2 = null;
        static string strCode3 = null;

        static string strEmail = null;
        static string strFirstName = null;
        static string strLastName = null;
        static string strMSISDN = null;

        //Add 15-02-2019
        static int? strRewardId = null;

        public AccountController(ICommandBus commandBus, IContactsRepository contactsRepository, ICompanyDetailsRepository companydetailsRepository, IUserRepository userRepository, ICountryRepository countryRepository, IFormsAuthentication formAuthentication, IUserTokenLinkRepository userTokenLinkRepository, IUserProfileRepository userProfileRepository, ICampaignAuditRepository campaignAuditRepository, IRewardRepository rewardRepository, IUserRewardRepository userRewardRepository, IOperatorRepository operatorRepository)
        {
            _commandBus = commandBus;
            _contactsRepository = contactsRepository;
            _companydetailsRepository = companydetailsRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _userTokenLinkRepository = userTokenLinkRepository;
            _userProfileRepository = userProfileRepository;
            _campaignAuditRepository = campaignAuditRepository;
            this.formAuthentication = formAuthentication;
            _rewardRepository = rewardRepository;
            _userRewardRepository = userRewardRepository;
            _operatorRepository = operatorRepository;
        }
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
        [Route("Index")]
        public ActionResult Index()
        {

            FillCountryList();
            AccountInfo _accountInfo = new AccountInfo();
            UserProfileInfo _profileinfo = new UserProfileInfo();
            ChangePasswordFormModel _changepassword = new ChangePasswordFormModel();

            //Add 19-02-2019
            RewardInfoFormModel _rewardInfoFormModel = new RewardInfoFormModel();

            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var userinfo = _userRepository.GetById(efmvcUser.UserId);

            //Add 19-02-2019
            //List<int> rewardId = new List<int>();
            string rewardId = "";
            var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == userinfo.UserId);
            if (userRewardInfo.Count() > 0)
            {
                foreach (var item in userRewardInfo)
                {
                    //rewardId.Add(item.RewardId);
                    if (rewardId == "")
                    {
                        rewardId = item.RewardId.ToString();
                    }
                    else
                    {
                        rewardId = rewardId + "," + item.RewardId;
                    }
                }
            }

            if (userinfo != null)
            {
                ViewBag.emailAddress = userinfo.Email;
                _profileinfo.Email = userinfo.Email;
                _profileinfo.FirstName = userinfo.FirstName;
                _profileinfo.LastName = userinfo.LastName;
                _profileinfo.Organisation = userinfo.Organisation;

                //Add 19-02-2019
                if (userRewardInfo.Count() > 0)
                {
                    ViewBag.rewardCheck = true;
                }
                else
                {
                    ViewBag.rewardCheck = false;
                }

                var userTokenData = _userTokenLinkRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                if (userTokenData != null)
                    ViewBag.Provider = userTokenData.Provider.ToUpper();
                else
                    ViewBag.Provider = "";
                var userProfileData = _userProfileRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                if (userProfileData != null)
                {
                    _profileinfo.MSISDN = userProfileData.MSISDN;
                }

                ViewBag.Mobile = _profileinfo.MSISDN;

                _profileinfo.IsEmailVerfication = userinfo.IsEmailVerfication;

                ViewBag.IsEmailVerified = userinfo.IsEmailVerfication;
                ViewBag.IsMobileVerfication = userinfo.IsMobileVerfication;

                strEmail = _profileinfo.Email;
                strFirstName = _profileinfo.FirstName;
                strLastName = _profileinfo.LastName;
                strMSISDN = _profileinfo.MSISDN;

                //Add 19-02-2019
                strRewardId = _rewardInfoFormModel.RewardId;

                string strUserId = null;
                if (string.IsNullOrEmpty(userinfo.PhoneticAlphabet))
                {
                    EFMVCDataContex db = new EFMVCDataContex();
                    var str1 = RandomString(3);
                    var str2 = RandomString(3);
                    var str3 = RandomString(3);

                    string phonetic = phonetic = str1.ToLower() + "-" + str2.ToLower() + "-" + str3.ToLower();

                    var userData = db.Users.Where(s => s.UserId == userinfo.UserId).FirstOrDefault();
                    if (userData != null)
                    {
                        userData.PhoneticAlphabet = phonetic;
                        db.SaveChanges();
                    }
                    strUserId = phonetic;
                }
                else
                {
                    strUserId = userinfo.PhoneticAlphabet;
                }

                ViewBag.UniqueId = strUserId;

                var temp = ConvertPhoneticAlphabet(strUserId);
                ViewBag.Phonetic = temp;
            }

            //Add 14-02-2019
            FillRewardList(rewardId, userinfo.OperatorId);

            //Code commented on 06-08-2018

            //ContactsFormModel contactmodel = new ContactsFormModel();
            //CompanyDetailsFormModel companymodel = new CompanyDetailsFormModel();
            //var _contactinfo = _contactsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            //if (_contactinfo != null)
            //{
            //    contactmodel = Mapper.Map<Contacts, ContactsFormModel>(_contactinfo);
            //    _accountInfo.ContactsFormModel = contactmodel;
            //}
            //else
            //{
            //    _accountInfo.ContactsFormModel = contactmodel;
            //}
            //var _companyInfo = _companydetailsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            //if (_companyInfo != null)
            //{
            //    companymodel = Mapper.Map<CompanyDetails, CompanyDetailsFormModel>(_companyInfo);
            //    _accountInfo.CompanyDetailsFormModel = companymodel;

            //    _accountInfo.CompanyDetailsFormModel.CountryId = companymodel.CountryId;
            //}
            //else
            //{
            //    _accountInfo.CompanyDetailsFormModel = companymodel;
            //}
            _accountInfo.UserProfileInfo = _profileinfo;
            _accountInfo.ChangePasswordFormModel = _changepassword;

            //Add 19-02-2019
            _accountInfo.RewardInfoFormModel = _rewardInfoFormModel;

            return View(_accountInfo);
        }

        //Add 14-02-2019
        private void FillRewardList(string rewardId, int OperatorId)
        {
            var reward = (from action in _rewardRepository.GetAll().Where(top => top.OperatorId == OperatorId)
                          select new
                          {
                              Text = action.RewardName,
                              Value = action.RewardId.ToString()
                          }).ToList();

            if (rewardId != "")
            {
                string[] a = rewardId.Split(',');
                int[] id = Array.ConvertAll(a, int.Parse);
                //ViewBag.reward = new MultiSelectList(reward, "Value", "Text", new[] { 8, 9 });
                ViewBag.reward = new MultiSelectList(reward, "Value", "Text", id);
            }
            else
            {
                ViewBag.reward = new MultiSelectList(reward, "Value", "Text");
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            //  const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string ConvertPhoneticAlphabet(string uniqueId)
        {
            var phoneticValue = "";
            var splitData = uniqueId.Split('-');
            var i = 0;
            foreach (var item in splitData)
            {
                char[] charArr = item.ToCharArray();
                foreach (var ch in charArr)
                {
                    switch (ch)
                    {
                        case 'a': phoneticValue += " ALPHA"; break;
                        case 'b': phoneticValue += " BRAVO"; break;
                        case 'c': phoneticValue += " CHARLIE"; break;
                        case 'd': phoneticValue += " DELTA"; break;
                        case 'e': phoneticValue += " ECHO"; break;
                        case 'f': phoneticValue += " FOXTROT"; break;
                        case 'g': phoneticValue += " GOLF"; break;
                        case 'h': phoneticValue += " HOTEL"; break;
                        case 'i': phoneticValue += " INDIA"; break;
                        case 'j': phoneticValue += " JULIET"; break;
                        case 'k': phoneticValue += " KILO"; break;
                        case 'l': phoneticValue += " LIMA"; break;
                        case 'm': phoneticValue += " MIKE"; break;
                        case 'n': phoneticValue += " NOVEMBER"; break;
                        case 'o': phoneticValue += " OSCAR"; break;
                        case 'p': phoneticValue += " PAPA"; break;
                        case 'q': phoneticValue += " QUEBEC"; break;
                        case 'r': phoneticValue += " ROMEO"; break;
                        case 's': phoneticValue += " SIERRA"; break;
                        case 't': phoneticValue += " TANGO"; break;
                        case 'u': phoneticValue += " UNIFORM"; break;
                        case 'v': phoneticValue += " VICTOR"; break;
                        case 'w': phoneticValue += " WHISKEY"; break;
                        case 'x': phoneticValue += " XRAY"; break;
                        case 'y': phoneticValue += " YANKEE"; break;
                        case 'z': phoneticValue += " ZULU"; break;
                        default: break;


                    }
                }
                i++;

                if (i < 3)
                {
                    phoneticValue += " - ";
                }

            }

            return phoneticValue.TrimStart();
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
                        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                        CreateOrUpdateContactsCommand command =
                            Mapper.Map<ContactsFormModel, CreateOrUpdateContactsCommand>(ContactsFormModel);
                        command.UserId = efmvcUser.UserId;
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
        public ActionResult UpdateUserInfo(string email, string oldemail, string firstname, string lastname, string Organisation, string phoneNumber)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var _existsemail = _userRepository.GetMany(top => top.Email.Trim().ToLower() == email.Trim().ToLower() && top.UserId != efmvcUser.UserId && top.RoleId == 2).FirstOrDefault();
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

                    if (strEmail == email && strFirstName == firstname && strLastName == lastname && strMSISDN == phoneNumber)
                    {
                        return Json("You have not updated any information.");
                    }

                    // phoneNumber = phoneNumber.Replace("+", "");

                    strEmail = email;
                    strFirstName = firstname;
                    strLastName = lastname;
                    strMSISDN = phoneNumber;

                    var command = new ChangeUserProfileInfoCommand
                    {
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname,
                        Organisation = Organisation,
                        UserId = efmvcUser.UserId
                    };

                    ICommandResult commandResult = _commandBus.Submit(command);



                    //using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    //{                        
                    //    var GetUserProfileById = mySQLEntities.usermatches.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                    //    if (GetUserProfileById != null)
                    //    {
                    //        var usermatch = mySQLEntities.usermatches.Where(s => s.UserProfileId == GetUserProfileById.UserProfileId).FirstOrDefault();
                    //        usermatch.Email = email;
                    //        mySQLEntities.SaveChanges();
                    //    }
                    //}


                    using (var SQLServerEntities = new EFMVCDataContex())
                    {

                        var usermatch = SQLServerEntities.UserMatch.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch != null)
                        {
                            //  var usermatch = mySQLEntities.usermatches.Where(s => s.UserProfileId == GetUserProfileById.UserProfileId).FirstOrDefault();
                            usermatch.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch2 = SQLServerEntities.UserMatch2.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch2 != null)
                        {
                            usermatch2.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch3 = SQLServerEntities.UserMatch3.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch3 != null)
                        {
                            usermatch3.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch4 = SQLServerEntities.UserMatch4.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch4 != null)
                        {
                            usermatch4.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch5 = SQLServerEntities.UserMatch5.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch5 != null)
                        {
                            usermatch5.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch6 = SQLServerEntities.UserMatch6.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch6 != null)
                        {
                            usermatch6.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch7 = SQLServerEntities.UserMatch7.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch7 != null)
                        {
                            usermatch7.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch8 = SQLServerEntities.UserMatch8.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch8 != null)
                        {
                            usermatch8.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch9 = SQLServerEntities.UserMatch9.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch9 != null)
                        {
                            usermatch9.Email = email;
                            SQLServerEntities.SaveChanges();
                        }

                        var usermatch10 = SQLServerEntities.UserMatch10.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (usermatch10 != null)
                        {
                            usermatch10.Email = email;
                            SQLServerEntities.SaveChanges();
                        }
                    }


                    if (commandResult.Success)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();

                        if (email != oldemail)
                        {
                            //Code commented on 10/08/2018
                            //sendemailtonewuser(email, oldemail);
                            //sendemailtoolduser(email, oldemail);

                            SendMail(email);
                            var userInfo = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                            if (userInfo != null)
                            {
                                userInfo.IsEmailVerfication = false;
                                db.SaveChanges();
                            }
                        }

                        var userProfileData = db.Userprofiles.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();

                        if (userProfileData != null)
                        {

                            var isMobileExist = db.Userprofiles.Where(s => s.MSISDN == phoneNumber && s.UserId != userProfileData.UserId).Any();
                            if (isMobileExist)
                            {
                                return Json("MobileExist", JsonRequestBehavior.AllowGet);
                            }
                            if (userProfileData.MSISDN != phoneNumber)
                            {
                                var usersData = _userRepository.GetById(efmvcUser.UserId);
                                if (usersData != null)
                                {
                                    if (usersData.OperatorId == 2)
                                    {
                                        string msg = RandomNumberGenerator();
                                        ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                                        var senegalStatusCode = activExpertSMS.SendSMS(phoneNumber, msg);
                                        if (senegalStatusCode.Result == "Success")
                                        {
                                            return Json("DifferentMobile", JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            return Json("Fail", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        var statusCode = SendSms(phoneNumber);
                                        var result = statusCode.Result;
                                        if (result == "OK")
                                        {
                                            return Json("DifferentMobile", JsonRequestBehavior.AllowGet);
                                            //return RedirectToAction("VerifyCode", "Account", new { Areas = "Users" });
                                        }
                                        else
                                        {
                                            return Json("Fail", JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                                else
                                {
                                    return Json("Fail", JsonRequestBehavior.AllowGet);
                                }

                            }
                            userProfileData.MSISDN = phoneNumber;
                            db.SaveChanges();
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

        //Old Add 19-02-2019
        //[Route("UpdateUserInfo")]
        //[HttpPost]
        //public ActionResult UpdateUserInfo(string email, string oldemail, string firstname, string lastname, string Organisation, string phoneNumber, string rewardId)
        //{
        //    try
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {

        //            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
        //            var _existsemail = _userRepository.GetMany(top => top.Email.Trim() == email.Trim() && top.UserId != efmvcUser.UserId && top.RoleId == 2).FirstOrDefault();
        //            if (_existsemail != null)
        //            {
        //                if (_existsemail.Activated == 3)
        //                {
        //                    //use deleted email address
        //                }
        //                else
        //                {
        //                    return Json("The Email Address is already exists in database. so please choose another one.");
        //                }

        //            }

        //            //Add 15-02-2019
        //            int? RewardId = null;
        //            if (rewardId != null || rewardId != "")
        //            {
        //                RewardId = int.Parse(rewardId);
        //            }

        //            if (strEmail == email && strFirstName == firstname && strLastName == lastname && strMSISDN == phoneNumber && strRewardId == RewardId)
        //            {
        //                return Json("You have not updated any information.");
        //            }

        //            // phoneNumber = phoneNumber.Replace("+", "");

        //            strEmail = email;
        //            strFirstName = firstname;
        //            strLastName = lastname;
        //            strMSISDN = phoneNumber;

        //            var command = new ChangeUserProfileInfoCommand
        //            {
        //                Email = email,
        //                FirstName = firstname,
        //                LastName = lastname,
        //                Organisation = Organisation,
        //                UserId = efmvcUser.UserId,
        //                RewardId = RewardId
        //            };

        //            ICommandResult commandResult = _commandBus.Submit(command);



        //            //using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
        //            //{                        
        //            //    var GetUserProfileById = mySQLEntities.usermatches.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //            //    if (GetUserProfileById != null)
        //            //    {
        //            //        var usermatch = mySQLEntities.usermatches.Where(s => s.UserProfileId == GetUserProfileById.UserProfileId).FirstOrDefault();
        //            //        usermatch.Email = email;
        //            //        mySQLEntities.SaveChanges();
        //            //    }
        //            //}


        //            using (var SQLServerEntities = new EFMVCDataContex())
        //            {

        //                var usermatch = SQLServerEntities.UserMatch.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch != null)
        //                {
        //                    //  var usermatch = mySQLEntities.usermatches.Where(s => s.UserProfileId == GetUserProfileById.UserProfileId).FirstOrDefault();
        //                    usermatch.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch2 = SQLServerEntities.UserMatch2.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch2 != null)
        //                {
        //                    usermatch2.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch3 = SQLServerEntities.UserMatch3.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch3 != null)
        //                {
        //                    usermatch3.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch4 = SQLServerEntities.UserMatch4.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch4 != null)
        //                {
        //                    usermatch4.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch5 = SQLServerEntities.UserMatch5.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch5 != null)
        //                {
        //                    usermatch5.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch6 = SQLServerEntities.UserMatch6.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch6 != null)
        //                {
        //                    usermatch6.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch7 = SQLServerEntities.UserMatch7.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch7 != null)
        //                {
        //                    usermatch7.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch8 = SQLServerEntities.UserMatch8.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch8 != null)
        //                {
        //                    usermatch8.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch9 = SQLServerEntities.UserMatch9.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch9 != null)
        //                {
        //                    usermatch9.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }

        //                var usermatch10 = SQLServerEntities.UserMatch10.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                if (usermatch10 != null)
        //                {
        //                    usermatch10.Email = email;
        //                    SQLServerEntities.SaveChanges();
        //                }
        //            }


        //            if (commandResult.Success)
        //            {
        //                EFMVCDataContex db = new EFMVCDataContex();

        //                if (email != oldemail)
        //                {
        //                    //Code commented on 10/08/2018
        //                    //sendemailtonewuser(email, oldemail);
        //                    //sendemailtoolduser(email, oldemail);

        //                    SendMail(email);
        //                    var userInfo = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
        //                    if (userInfo != null)
        //                    {
        //                        userInfo.IsEmailVerfication = false;
        //                        db.SaveChanges();
        //                    }
        //                }

        //                var userProfileData = db.Userprofiles.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();

        //                if (userProfileData != null)
        //                {

        //                    var isMobileExist = db.Userprofiles.Where(s => s.MSISDN == phoneNumber && s.UserId != userProfileData.UserId).Any();
        //                    if (isMobileExist)
        //                    {
        //                        return Json("MobileExist", JsonRequestBehavior.AllowGet);
        //                    }
        //                    if (userProfileData.MSISDN != phoneNumber)
        //                    {
        //                        var statusCode = SendSms(phoneNumber);
        //                        var result = statusCode.Result;
        //                        if (result == "OK")
        //                        {
        //                            return Json("DifferentMobile", JsonRequestBehavior.AllowGet);
        //                            //return RedirectToAction("VerifyCode", "Account", new { Areas = "Users" });
        //                        }
        //                        else
        //                        {
        //                            return Json("Fail", JsonRequestBehavior.AllowGet);
        //                        }

        //                    }
        //                    userProfileData.MSISDN = phoneNumber;
        //                    db.SaveChanges();
        //                }

        //                return Json("success");
        //            }
        //            return Json("fail");
        //        }
        //        else
        //        {
        //            return Json("notauthorise");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(ex.InnerException.Message.ToString());
        //    }

        //}

        [Route("UpdateRewardInfo")]
        [HttpPost]
        public ActionResult UpdateRewardInfo(string[] rewardId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var operatorId = _userRepository.GetById(efmvcUser.UserId).OperatorId;
                    var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == efmvcUser.UserId);
                    if (userRewardInfo.Count() > 0)
                    {
                        foreach (var userRewardId in userRewardInfo)
                        {
                            var command = new DeleteUserRewardCommand
                            {
                                UserRewardId = userRewardId.UserRewardId,
                                OperatorId = operatorId
                            };
                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                //return Json("success");
                            }
                        }
                    }
                    if (rewardId.Count() > 0)
                    {
                        foreach (var reward in rewardId)
                        {
                            var command = new ChangeUserRewardInfoCommand
                            {
                                UserRewardId = 0,
                                UserId = efmvcUser.UserId,
                                RewardId = int.Parse(reward),
                                OperatorId = operatorId
                            };
                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                //return Json("success");
                            }
                        }
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

        [Route("ResendVerificationCode")]
        public ActionResult ResendVerificationCode(string phoneNumber, string countryCode)
        {
            var countryId = _countryRepository.Get(top => top.CountryCode == String.Concat("+", countryCode)).Id;
            var operatorId = _operatorRepository.Get(top => top.CountryId == countryId).OperatorId;
            if (operatorId == 2)
            {
                string msg = RandomNumberGenerator();
                ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                var senegalStatusCode = activExpertSMS.SendSMS(phoneNumber, msg);
                if (senegalStatusCode.Result == "Success")
                {
                    return Json("DifferentMobile", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var statusCode = SendSms(phoneNumber);
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
        }


        private async Task<string> SendSms(string phoneNumber)
        {
            var client = new HttpClient();

            strCode1 = null;
            strCode2 = null;
            strCode3 = null;

            string uid = "adtones_sms_api";
            string pass = "2x8whkr";
            // string dest = "447980720250";
            string dest = phoneNumber;

            string orig = "Adtone"; // number
            //string orig = "447860064145";
            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            strCode1 = code1;
            strCode2 = code2;
            strCode3 = code3;


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

        [Route("VerifyCode")]
        public ActionResult VerifyCode(string phoneNumber)
        {
            var number = phoneNumber.Replace(" ", "+");
            ViewBag.PhoneNumber = number;
            return View();
        }

        [Route("ConfirmVerifyCode")]
        public ActionResult ConfirmVerifyCode(string confirmCode, string phoneNumber)
        {
            var code = confirmCode.Split('-');

            var code1 = strCode1;
            var code2 = strCode2;
            var code3 = strCode3;

            var receivedCode1 = code[0];
            var receivedCode2 = code[1];
            var receivedCode3 = code[2];

            if (code1 != null && code1 != null && code1 != null)
            {
                if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    EFMVCDataContex db = new EFMVCDataContex();
                    var userProfileData = db.Userprofiles.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                    if (userProfileData != null)
                    {
                        userProfileData.MSISDN = phoneNumber;
                        db.SaveChanges();

                        var userData = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                        userData.IsMobileVerfication = true;
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

        [Route("ConfirmEmailVerificationCode")]
        public ActionResult ConfirmEmailVerificationCode(string confirmEmailCode)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                EFMVCDataContex db = new EFMVCDataContex();
                var verificationDetail = db.EmailVerificationCode.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();
                if (verificationDetail != null)
                {
                    if (verificationDetail.VerificationCode == confirmEmailCode)
                    {
                        db.EmailVerificationCode.Remove(verificationDetail);
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

        [Route("CancelAccount")]
        [HttpPost]
        public ActionResult CancelAccount()
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var userinfo = db.Users.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();


                string phoneNum = "";
                var userProfile = userinfo.UserProfiles.FirstOrDefault();
                if (userProfile != null)
                    phoneNum = userProfile.MSISDN;

                if (userinfo.OperatorId == (int)OperatorTableId.Safaricom)
                {
                    //271191
                     var returnCode = SoapApiProcess.DeleteSoapUser(efmvcUser.UserId, phoneNum);
                   // var returnCode = SoapApiProcess.DeleteCorpUser(userinfo);
                    if (returnCode == "000000")
                    {
                        UserDeleteProcess(userinfo, db);
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
                else if (userinfo.OperatorId == (int)OperatorTableId.Expresso)
                {
                    var expressoResult = ExpressoOperator.ExpressoProvision(phoneNum, "false");
                    if (expressoResult.Count() > 0)
                    {
                        if (expressoResult.FirstOrDefault().code == "0001")
                        {
                            UserDeleteProcess(userinfo, db);
                        }
                        else
                        {
                            return Json("Fail");
                        }

                    }
                    else
                    {
                        return Json("Fail");
                    }
                        
                }

                return Json("Success");

            }
            catch (Exception ex)
            {
                return Json("Fail");
            }

        }

        private void UserDeleteProcess(User userinfo, EFMVCDataContex db)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            #region User Data Delete
            //var userTokenInfo = db.UserTokenLink.Where(s => s.UserId == efmvcUser.UserId).ToList();
            //if (userTokenInfo.Count() > 0)
            //{
            //    foreach(var item in userTokenInfo)
            //    {
            //        db.UserTokenLink.Remove(item);
            //        db.SaveChanges();
            //    }

            //}

            var userProfileData = db.Userprofiles.Where(s => s.UserId == efmvcUser.UserId).FirstOrDefault();

            if (userProfileData != null)
            {

                var userProfileId = userProfileData.UserProfileId;
                DeletePrematchData(userProfileId, userinfo.OperatorId);


                // DeleteUserMatchData(efmvcUser.UserId, userinfo.OperatorId);

                //var userProfilePrefe = db.UserProfilePreference.Where(s => s.UserProfileId == userProfileId).FirstOrDefault();
                //if (userProfilePrefe != null)
                //{
                //    db.UserProfilePreference.Remove(userProfilePrefe);
                //    db.SaveChanges();
                //}

                var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == efmvcUser.UserId).ToList();
                foreach (var item in emailVerificationCode)
                {
                    db.EmailVerificationCode.Remove(item);
                    db.SaveChanges();
                }

                //if(emailVerificationCode != null)
                //{
                //    db.EmailVerificationCode.Remove(emailVerificationCode);
                //    db.SaveChanges();
                //}

                var ConnString = ConnectionString.GetConnectionStringByOperatorId(userinfo.OperatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db2 = new EFMVCDataContex(item);
                        //var userProfilePreference = db2.UserProfilePreference.Where(s => s.UserProfileId == userProfileId).FirstOrDefault();
                        //if (userProfilePreference != null)
                        //{
                        //    db2.UserProfilePreference.Remove(userProfilePreference);
                        //    db2.SaveChanges();
                        //}

                        var userinfofromOP = db2.Users.Where(s => s.AdtoneServerUserId == efmvcUser.UserId).FirstOrDefault();




                        if (userinfofromOP != null)
                        {
                            var userProfileDatafromOP = db2.Userprofiles.Where(s => s.UserId == userinfofromOP.UserId).FirstOrDefault();
                            if (userProfileDatafromOP != null)
                            {
                                //db2.Userprofiles.Remove(userProfileDatafromOP);
                                userProfileDatafromOP.MSISDN = null;
                                db2.SaveChanges();
                            }

                            userinfofromOP.Activated = (int)UserStatus.Deleted;
                            userinfofromOP.IsMsisdnMatch = false;
                            //db2.Users.Remove(userinfofromOP);
                            db2.SaveChanges();
                        }

                    }
                }

                //var phoneNumber = userProfileData.MSISDN;

                //db.Userprofiles.Remove(userProfileData);

                //userProfileData.MSISDN = null;
                userinfo.Activated = (int)UserStatus.Deleted;
                userinfo.IsMsisdnMatch = false;
                // db.Users.Remove(userinfo);
                db.SaveChanges();


                //var CountryCode = phoneNumber.Substring(0, 2);
                //if (CountryCode == "44")
                //{
                //    // DeleteRecordsFromNumberBeginingTable(phoneNumber); // Remove code when Socket connection is live or for testing
                //}

            }
            #endregion
        }

        private void DeleteUserMatchData(int userId, int operatorId)
        {
            if (operatorId != 0)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var userMatchData = db.UserMatch.Where(s => s.UserId == userId).ToList();
                        if (userMatchData.Count() > 0)
                        {
                            db.UserMatch.RemoveRange(userMatchData);
                            db.SaveChanges();
                        }

                        var userMatchData2 = db.UserMatch2.Where(s => s.UserId == userId).ToList();
                        if (userMatchData2.Count() > 0)
                        {
                            db.UserMatch2.RemoveRange(userMatchData2);
                            db.SaveChanges();
                        }

                        var userMatchData3 = db.UserMatch3.Where(s => s.UserId == userId).ToList();
                        if (userMatchData3.Count() > 0)
                        {
                            db.UserMatch3.RemoveRange(userMatchData3);
                            db.SaveChanges();
                        }

                        var userMatchData4 = db.UserMatch4.Where(s => s.UserId == userId).ToList();
                        if (userMatchData4.Count() > 0)
                        {
                            db.UserMatch4.RemoveRange(userMatchData4);
                            db.SaveChanges();
                        }

                        var userMatchData5 = db.UserMatch5.Where(s => s.UserId == userId).ToList();
                        if (userMatchData5.Count() > 0)
                        {
                            db.UserMatch5.RemoveRange(userMatchData5);
                            db.SaveChanges();
                        }

                        var userMatchData6 = db.UserMatch6.Where(s => s.UserId == userId).ToList();
                        if (userMatchData6.Count() > 0)
                        {
                            db.UserMatch6.RemoveRange(userMatchData6);
                            db.SaveChanges();
                        }

                        var userMatchData7 = db.UserMatch7.Where(s => s.UserId == userId).ToList();
                        if (userMatchData7.Count() > 0)
                        {
                            db.UserMatch7.RemoveRange(userMatchData7);
                            db.SaveChanges();
                        }

                        var userMatchData8 = db.UserMatch8.Where(s => s.UserId == userId).ToList();
                        if (userMatchData8.Count() > 0)
                        {
                            db.UserMatch8.RemoveRange(userMatchData8);
                            db.SaveChanges();
                        }

                        var userMatchData9 = db.UserMatch9.Where(s => s.UserId == userId).ToList();
                        if (userMatchData9.Count() > 0)
                        {
                            db.UserMatch9.RemoveRange(userMatchData9);
                            db.SaveChanges();
                        }

                        var userMatchData10 = db.UserMatch10.Where(s => s.UserId == userId).ToList();
                        if (userMatchData10.Count() > 0)
                        {
                            db.UserMatch10.RemoveRange(userMatchData10);
                            db.SaveChanges();
                        }
                    }
                }
            }

        }

        private void DeletePrematchData(int userProfId, int operatorId)
        {
            if (operatorId != 0)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userProfileData = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == userProfId).FirstOrDefault();
                        if (userProfileData != null)
                        {
                            var userProfileId = userProfileData.UserProfileId.ToString();
                            var PrematchData = db.PreMatch.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData.Count() > 0)
                            {
                                db.PreMatch.RemoveRange(PrematchData);
                                db.SaveChanges();
                            }

                            var PrematchData2 = db.PreMatch2.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData2.Count() > 0)
                            {
                                db.PreMatch2.RemoveRange(PrematchData2);
                                db.SaveChanges();
                            }

                            var PrematchData3 = db.PreMatch3.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData3.Count() > 0)
                            {
                                db.PreMatch3.RemoveRange(PrematchData3);
                                db.SaveChanges();
                            }

                            var PrematchData4 = db.PreMatch4.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData4.Count() > 0)
                            {
                                db.PreMatch4.RemoveRange(PrematchData4);
                                db.SaveChanges();
                            }

                            var PrematchData5 = db.PreMatch5.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData5.Count() > 0)
                            {
                                db.PreMatch5.RemoveRange(PrematchData5);
                                db.SaveChanges();
                            }

                            var PrematchData6 = db.PreMatch6.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData6.Count() > 0)
                            {
                                db.PreMatch6.RemoveRange(PrematchData6);
                                db.SaveChanges();
                            }

                            var PrematchData7 = db.PreMatch7.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData7.Count() > 0)
                            {
                                db.PreMatch7.RemoveRange(PrematchData7);
                                db.SaveChanges();
                            }

                            var PrematchData8 = db.PreMatch8.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData8.Count() > 0)
                            {
                                db.PreMatch8.RemoveRange(PrematchData8);
                                db.SaveChanges();
                            }

                            var PrematchData9 = db.PreMatch9.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData9.Count() > 0)
                            {
                                db.PreMatch9.RemoveRange(PrematchData9);
                                db.SaveChanges();
                            }

                            var PrematchData10 = db.PreMatch10.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData10.Count() > 0)
                            {
                                db.PreMatch10.RemoveRange(PrematchData10);
                                db.SaveChanges();
                            }

                            //var isCampaignAuditData = db.CampaignAudits.Where(s => s.UserProfileId == userProfileData.UserProfileId).Any();
                            //if (isCampaignAuditData)
                            //{
                            //    userProfileData.DOB = null;
                            //    userProfileData.Gender = null;
                            //    userProfileData.IncomeBracket = null;
                            //    userProfileData.WorkingStatus = null;
                            //    userProfileData.RelationshipStatus = null;
                            //    userProfileData.Education = null;
                            //    userProfileData.HouseholdStatus = null;
                            //    userProfileData.Location = null;
                            //    db.SaveChanges();
                            //}

                           // userProfileData.MSISDN = null;
                            db.SaveChanges();
                        }

                    }

                }

            }

        }

        private void DeleteRecordsFromNumberBeginingTable(string mobile)
        {
            string externalServerConnString = ConfigurationManager.AppSettings["externalserverconnectionstring"];
            string numberbeginning = mobile.Substring(0, 5);
            string Query = @"Delete FROM  " + numberbeginning + "table WHERE MobileNumber = '" + mobile + "'";
            MySqlConnection MyConn2 = new MySqlConnection(externalServerConnString);
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MySqlDataReader MyReader2;
            MyConn2.Open();
            MyReader2 = MyCommand2.ExecuteReader();
            MyConn2.Close();

            string tableRecords = @"select count(*) as cnt FROM  " + numberbeginning + "table";
            MyCommand2 = new MySqlCommand(tableRecords, MyConn2);
            MyConn2.Open();
            MyReader2 = MyCommand2.ExecuteReader();

            while (MyReader2.Read())
            {
                var count = Convert.ToString(MyReader2["cnt"]);
                if (count == "0")
                {
                    MyConn2.Close();
                    string deleteTable = @"DROP Table " + numberbeginning + "table";
                    MyCommand2 = new MySqlCommand(deleteTable, MyConn2);
                    MyConn2.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                }
            }

            MyConn2.Close();

        }

        [Route("ResendEmail")]
        public ActionResult ResendEmail(string emailAddress)
        {
            try
            {
                SendMail(emailAddress);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }

        public void SendMail(string emailAddress)
        {
            User user = _userRepository.Get(u => u.Email == emailAddress);
            //string email = EncryptionHelper.EncryptSingleValue(user.Email);
            //string url = string.Format("{0}?activationCode={1}",
            //                           ConfigurationManager.AppSettings["UsersResendVerificationUrl"], email);


            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["UsersResendVerificationEmailTemplate"]));
            string emailContent = reader.ReadToEnd();

            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            string verifyCode = code1 + " " + code2 + " " + code3 + " ";


            emailContent = string.Format(emailContent, verifyCode);




            MailMessage mail = new MailMessage();
            mail.To.Add(user.Email);
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

            EFMVCDataContex db = new EFMVCDataContex();
            var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == user.UserId).ToList();
            foreach (var item in emailVerificationCode)
            {
                db.EmailVerificationCode.Remove(item);
                db.SaveChanges();
            }

            EmailVerificationCode emailVerification = new EmailVerificationCode();

            emailVerification.UserId = user.UserId;
            emailVerification.VerificationCode = code1 + "-" + code2 + "-" + code3;
            emailVerification.DateCreated = DateTime.Now;
            db.EmailVerificationCode.Add(emailVerification);
            db.SaveChanges();

        }



        //private string CheckUserExistSoapApi(User usr)
        //{
        //try
        //{
        //    EFMVCDataContex db = new EFMVCDataContex();
        //    //if (usr.OperatorId == 1) //Safaricom
        //    //{
        //    var portalAccount = "adtones";
        //    var portalPassword = "TBD";
        //    var portalType = "1";
        //    var role = "4";
        //    var roleCode = "100";
        //    var corpId = "440114";
        //    // var phoneNumber = "254000000000";
        //    var phoneNumber = "";
        //    var userProfile = usr.UserProfiles.FirstOrDefault();
        //    if (userProfile != null)
        //        phoneNumber = userProfile.MSISDN;

        //    string soapUIUrl = ConfigurationManager.AppSettings["SoapUIUrl"];
        //    var client = new RestClient(soapUIUrl);

        //    var request = new RestRequest(Method.POST);
        //    request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:delCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n         <event xsi:type=\"even:DelCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n            " +
        //        "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
        //        "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
        //        "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
        //        "<moduleCode xsi:type=\"xsd:string\">" + "?" + "</moduleCode>\r\n" +
        //        "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
        //        "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n " +
        //        "<corpID xsi:type=\"xsd:string\">" + corpId + "</corpID>\r\n" +
        //        "<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
        //        "</event>\r\n      </cor:delCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    var responseContent = response.Content;

        //    if (!string.IsNullOrEmpty(responseContent))
        //    {
        //        XmlDocument xmldoc = new XmlDocument();
        //        xmldoc.LoadXml(responseContent);
        //        XmlNodeList nodeList = xmldoc.GetElementsByTagName("delCorpUserReturn");
        //        if (nodeList.Count > 0)
        //        {
        //            foreach (XmlNode node in nodeList)
        //            {
        //                var returnCode = node.SelectSingleNode("returnCode").InnerXml;
        //                if (returnCode == "000000")
        //                {
        //                    var getUser = db.Users.Where(s => s.UserId == usr.UserId).FirstOrDefault();
        //                    getUser.IsMsisdnMatch = false;
        //                    getUser.Activated = 2;
        //                    db.SaveChanges();
        //                }
        //                else if (returnCode == "100002")
        //                {
        //                    //SYSTEM_BUSY
        //                }
        //                else if (returnCode == "100003")
        //                {
        //                    //OPERATION_OVERTIME
        //                }
        //                else if (returnCode == "100004")
        //                {
        //                    //NETWORK_EXCEPTION
        //                }
        //                else if (returnCode == "100006")
        //                {
        //                    //DATABASE_OPERATION_FAILED
        //                }
        //                else if (returnCode == "100007")
        //                {
        //                    //HAS_NOT_SERVICE
        //                }

        //                return returnCode;
        //                //var temp2 = node.SelectSingleNode("operationID").InnerXml;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return "000000"; // remove this when soap is live, it called when soap app is not open
        //    }
        //    return "100007";
        //}
        //catch (Exception ex)
        //{
        //    return "100007";
        //}


        // }


        [Route("LogOff")]
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Login", new { area = "Users" });
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

        [Route("SuspendedAccount")]
        [HttpPost]
        public ActionResult SuspendedAccount()
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
                command.UserId = efmvcUser.UserId;
                command.Activated = 2;
                ICommandResult result = _commandBus.Submit(command);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        public string RandomNumberGenerator()
        {
            TempData["code1"] = null;
            TempData["code2"] = null;
            TempData["code3"] = null;

            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            TempData["code1"] = code1;
            TempData["code2"] = code2;
            TempData["code3"] = code3;

            string msg = code1 + " " + code2 + " " + code3 + " is your Adtone verification code. It will expire in 24 hours";
            return msg;
        }
    }
}