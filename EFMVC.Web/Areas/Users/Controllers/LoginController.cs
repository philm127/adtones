using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Security;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Mailer;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [RouteArea("Users")]
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        private ISendEmailMailer _sendEmailMailer = new SendEmailMailer();
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ISoapApiResponseCodeRepository _soapApiResponseCodeRepository;
        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        //Add 22-02-2019
        private readonly IRewardRepository _rewardRepository;

        //Add 01-03-2019
        private readonly IUserProfileRepository _userProfileRepository;

        private readonly ICopyRightRepository _copyRightRepository;

        private readonly IContactsRepository _contactsRepository;

        private readonly ICountryRepository _countryRepository;

        //static string FirstName = null;
        //static string LastName = null;
        //static string Email = null;
        //static string Organisation = null;
        //static string Password = null;
        //static string CountryCode = null;
        //static string MSISDN = null;
        //static int OperatorId = 0;

        public ISendEmailMailer sendEmailMailer
        {
            get { return _sendEmailMailer; }
            set { _sendEmailMailer = value; }
        }

        //static string strCode1 = null;
        //static string strCode2 = null;
        //static string strCode3 = null;

        public LoginController(ICommandBus commandBus, IUserRepository userRepository, IFormsAuthentication formAuthentication, IOperatorRepository operatorRepository, IProfileRepository profileRepository, ISoapApiResponseCodeRepository soapApiResponseCodeRepository, IRewardRepository rewardRepository, IUserProfileRepository userProfileRepository, ICopyRightRepository copyRightRepository, IContactsRepository contactsRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _operatorRepository = operatorRepository;
            _profileRepository = profileRepository;
            _soapApiResponseCodeRepository = soapApiResponseCodeRepository;
            _rewardRepository = rewardRepository;
            _userProfileRepository = userProfileRepository;
            this.formAuthentication = formAuthentication;
            _copyRightRepository = copyRightRepository;
            _contactsRepository = contactsRepository;
            _countryRepository = countryRepository;
        }
        // GET: Admin/Login

        [Route("Index")]
        public ActionResult Index()
        {

            // var expressoCode = ExpressoOperator.ExpressoProvision("221706077071", "true");

            var url = Request.UrlReferrer;
            if (url != null)
            {
                var urlValue = url.ToString();
                //if (urlValue.Contains("https://adtones.api.oneall.com/socialize/redirect.html"))
                if (urlValue.Contains("//adtones.api.oneall.com/") && (Request.Form.AllKeys.Where(x => x.ToLower().Equals("oa_social_login_token")).FirstOrDefault() != null && !String.IsNullOrWhiteSpace(Request.Form["oa_social_login_token"])))
                {
                    //  var connectionToken = url.Query.Split('=')[1];
                    var connectionToken = Request.Form["oa_social_login_token"];

                    var client = new RestClient("https://adtones.api.oneall.com/connections/" + connectionToken + ".json");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("postman-token", "959ecc3a-e1df-570e-81bc-df3133a0add2");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", "Basic NzdiMmJlOGYtMDFkMi00ZDdkLWExYmItOTExMjgzNzkwNDIzOjk2YmM1YWFmLTM5NTUtNGQxMy05NThmLTM3MjM0ZThkYjM1NA==");
                    //authorization BasicAuth UserName:publicKey Password: privateKey  
                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        var desearlizeObj = JsonConvert.DeserializeObject<SocialLoginModel.RootObject>(content);

                        var userToken = desearlizeObj.response.result.data.user.user_token;
                        var identityToken = desearlizeObj.response.result.data.user.identity.identity_token;
                        var provider = desearlizeObj.response.result.data.user.identity.provider;
                        EFMVCDataContex db = new EFMVCDataContex();

                        var userTokenData = db.UserTokenLink.Where(s => s.UserToken == userToken).FirstOrDefault();
                        if (userTokenData == null)
                        {
                            if (desearlizeObj.response.result.data.user.identity.emails != null)
                            {
                                var email = desearlizeObj.response.result.data.user.identity.emails.FirstOrDefault().value;
                                var userData = _userRepository.GetMany(top => top.Email.ToLower() == email.ToLower()).FirstOrDefault();
                                if (userData != null)
                                {
                                    ViewBag.Error = email + " Email Address already exists in database so please choose another one.";

                                    //Comment 18-04-2019
                                    //"The email is already exists.";

                                    return View("Index");
                                }
                            }
                            //string userToken,string connectionToken
                            ViewBag.UserToken = userToken;
                            ViewBag.ConnectionToken = connectionToken;
                            ViewBag.Provider = provider;
                            return View("Callback");
                        }
                        else
                        {
                            var userId = userTokenData.UserId;

                            User user = _userRepository.Get(u => u.UserId == userId && u.RoleId == 2);
                            if (user != null)
                            {
                                if (user.Activated != 1)
                                {
                                    var errorMsg = user.Activated == 0 ? "Your account is not approved by adtones administrator so please contact adtones admin."
                                             : user.Activated == 2 ? "Your account is  suspended by adtones administrator so please contact adtones admin."
                                             : user.Activated == 3 ? "Your account is  deleted by adtones administrator so please contact adtones admin."
                                             : "Something went wrong! please try again";
                                    ViewBag.Error = errorMsg;
                                    //ModelState.AddModelError("", errorMsg);
                                    return View("Index");
                                }
                                else
                                {
                                    formAuthentication.SetAuthCookie(HttpContext,
                                           UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                               user));
                                }
                                var urls = ConfigurationManager.AppSettings["userURL"].ToString();
                                var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                                return Redirect(siteaddress + urls);
                            }
                            else
                            {
                                ViewBag.Error = "The email is already exists for different user role.";
                                return View("Index");
                            }
                        }
                    }

                }
            }


            return View();
        }


        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        private bool ValidatePassword(User user, string password)
        {
            string encoded = Md5Encrypt.Md5EncryptPassword(password);
            return user.PasswordHash.Equals(encoded);
        }
        [Route("CheckLogin")]
        [HttpPost]
        public ActionResult CheckLogin(UserLogOnFormModel form)
        {
            try
            {
                int usererror = 0;
                if (ModelState.IsValid)
                {
                    if (form.UserName == null)
                    {
                        ModelState.AddModelError("", "The Email Address Or Mobile Number field is required.");
                        usererror = 1;
                    }
                    if (form.Password == null)
                    {
                        ModelState.AddModelError("", "The Password field is required.");
                        usererror = 1;
                    }
                    if (usererror != 1)
                    {
                        int emailError = 0;
                        bool IsEmail = IsValidEmail(form.UserName);
                        if (IsEmail == true)
                        {
                            emailError = 1;
                        }
                        if (emailError != 1)
                        {
                            User userData = _userRepository.Get(u => u.UserProfiles.FirstOrDefault().MSISDN == form.UserName && u.RoleId == 2 && u.Activated != 3);
                            if (userData != null)
                            {
                                USSDUserFormModel model = new USSDUserFormModel();
                                if (userData.PasswordHash != "?\u0006r!nk?\u000f2|}?\u0019xO?" && userData.PasswordHash != "Pa55w0rd")
                                {
                                    if (ValidatePassword(userData, form.Password))
                                    {

                                        formAuthentication.SetAuthCookie(HttpContext,
                                            UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                userData));

                                        var urls = string.Empty;
                                        if (userData.RoleId == 2)
                                        {
                                            urls = ConfigurationManager.AppSettings["userURL"].ToString();
                                            var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                                            return Redirect(siteaddress + urls);
                                        }
                                        else ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                    }
                                    else ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                }
                                else
                                {
                                    if (userData.OperatorId == 2)
                                    {
                                        string msg = RandomNumberGenerator();
                                        ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                                        var senegalStatusCode = activExpertSMS.SendSMS(userData.UserProfiles.FirstOrDefault().MSISDN, msg);
                                        if (senegalStatusCode.Result == "Success")
                                        {
                                            TempData["MSISDN"] = form.UserName;
                                            model.MSISDN = userData.UserProfiles.FirstOrDefault().MSISDN;
                                            return View("USSDRegisterVerifyCode");
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", senegalStatusCode.Result);
                                            return View("Index", form);
                                        }
                                    }
                                    else
                                    {
                                        var MSISDN = userData.UserProfiles.FirstOrDefault().MSISDN;
                                        string CountryCode = Left(MSISDN, 3);
                                        var statusCode = SendSms(MSISDN, CountryCode);
                                        var resultData = statusCode.Result;
                                        if (resultData == "OK")
                                        {
                                            TempData["MSISDN"] = form.UserName;
                                            model.MSISDN = userData.UserProfiles.FirstOrDefault().MSISDN;
                                            return View("USSDRegisterVerifyCode");
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "An unknown error occurred.");
                                            return View("Index", form);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your account is not found so please Sign up.");
                                usererror = -1;
                            }
                        }
                        else
                        {
                            User user = _userRepository.Get(u => (u.Email == form.UserName || u.UserProfiles.FirstOrDefault().MSISDN == form.UserName) && u.RoleId == 2 && u.Activated != 3);
                            if (user != null)
                            {
                                if (user.Activated == 0)
                                {
                                    ModelState.AddModelError("", "Your account is not approved by adtones administrator so please contact adtones admin.");
                                    usererror = -1;
                                }
                                else if (user.Activated == 2)
                                {
                                    //ModelState.AddModelError("", "Your account is  suspended by adton administrator.so please contact adtones admin.");
                                    ModelState.AddModelError("", "Your account is suspended so please contact adtones admin.");
                                    usererror = -1;
                                }
                                else if (user.Activated == 3)
                                {
                                    //ModelState.AddModelError("", "Your account is  suspended by adton administrator.so please contact adtones admin.");
                                    ModelState.AddModelError("", "Your account is deleted so please contact adtones admin.");
                                    usererror = -1;
                                }
                            }

                            else
                            {
                                if (user == null)
                                {
                                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                    usererror = -1;
                                }
                                else
                                {
                                    User userdeleted = _userRepository.Get(u => u.Email == form.UserName && u.Activated == 3);
                                    if (userdeleted != null)
                                    {
                                        ModelState.AddModelError("", "Your account is  deleted so please contact adtones admin.");
                                        usererror = -1;
                                    }
                                }
                            }
                            if (usererror == 0)
                            {
                                if (ValidatePassword(user, form.Password))
                                {

                                    formAuthentication.SetAuthCookie(HttpContext,
                                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                            user));

                                    var urls = string.Empty;
                                    if (user.RoleId == 2)
                                    {
                                        urls = ConfigurationManager.AppSettings["userURL"].ToString();
                                        var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                                        return Redirect(siteaddress + urls);
                                    }
                                    else ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                }
                                else ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            }
                        }
                    }
                }

                // If we got this far, something failed
                return View("Index", form);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void FillOperator()
        {
            var clientdetails = _operatorRepository.GetMany(s => s.IsActive == true).Select(top => new
            {
                Name = top.OperatorName,
                Id = top.OperatorId
            }).ToList();
            ViewBag.operators = new MultiSelectList(clientdetails, "Id", "Name");

        }

        [Route("GetOperator")]
        public ActionResult GetOperator(string countryName)
        {
            var operatorList = _operatorRepository.GetMany(s => s.Country.Name.ToLower() == countryName.ToLower() && s.IsActive == true).Select(s => new SelectListItem { Text = s.OperatorName, Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
            if (operatorList.Count() > 0)
            {
                return Json(operatorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }

        }


        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [Route("USSDRegisterUser")]
        public ActionResult USSDRegisterUser(USSDUserFormModel model)
        {
            var code = model.VerifcationCode.Split('-');
            var code1 = TempData["code1"].ToString();
            var code2 = TempData["code2"].ToString();
            var code3 = TempData["code3"].ToString();
            var receivedCode1 = code[0];
            var receivedCode2 = code[1];
            var receivedCode3 = code[2];
            if (code1 != null && code1 != null && code1 != null)
            {
                if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                {
                    model.MSISDN = TempData["MSISDN"].ToString();
                    return View(model);
                }
                else
                {
                    TempData["code1"] = TempData["code1"].ToString();
                    TempData["code2"] = TempData["code2"].ToString();
                    TempData["code3"] = TempData["code3"].ToString();
                    TempData["Error"] = "Please enter the correct confirm code";
                    return View("USSDRegisterVerifyCode");
                }
            }
            else
            {
                TempData["code1"] = TempData["code1"].ToString();
                TempData["code2"] = TempData["code2"].ToString();
                TempData["code3"] = TempData["code3"].ToString();
                TempData["Error"] = "Please enter the confirm code";
                return View("USSDRegisterVerifyCode");
            }
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult SaveRegister(UserFormModel form)
        {
            //if (ModelState.IsValid)
            //{
            try
            {

                if (form.OperatorId == null || form.OperatorId == 0)
                {
                    TempData["Error"] = "Operator is not exist for this country.";
                    FillOperator();
                    return View("Register", form);
                }

                var number = form.CountryCode + form.MSISDN;
                var isMobileExist = _profileRepository.GetMany(s => s.MSISDN == number).Any();
                if (isMobileExist)
                {
                    //TempData["Error"] = "The phone number is already exists.";

                    //Add 18-04-2019
                    TempData["Error"] = form.MSISDN + " MSISDN already exists in database so please choose another one.";
                    FillOperator();
                    return View("Register", form);
                }

                //var isEmailExist = _userRepository.GetMany(s => s.Email == form.Email && s.RoleId == 2 && s.Activated == 1).Any();
                var userData = _userRepository.GetMany(s => s.Email.ToLower() == form.Email.ToLower()).FirstOrDefault();
                if (userData != null)
                {
                    //Add 18-04-2019
                    TempData["Error"] = form.Email + " Email Address already exists in database so please choose another one.";
                    FillOperator();
                    return View("Register", form);
                }
                if (userData != null)
                {
                    if (userData.Activated == 2)
                    {
                        TempData["Error"] = "Your account is suspended so please contact adtones admin.";
                        FillOperator();
                        return View("Register", form);
                    }
                    else if (userData.Activated == 3)
                    {
                        TempData["Error"] = "Your account is deleted so please contact adtones admin.";
                        FillOperator();
                        return View("Register", form);
                    }
                }

                //FirstName = form.FirstName;
                //LastName = form.LastName;
                //Email = form.Email;
                //Organisation = form.Organisation;
                //Password = form.Password;
                //CountryCode = form.CountryCode;
                //MSISDN = form.MSISDN;
                //OperatorId = (int)form.OperatorId;

                if (form.OperatorId == 2)
                {
                    string msg = RandomNumberGenerator();
                    ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                    var senegalStatusCode = activExpertSMS.SendSMS(String.Concat(form.CountryCode, form.MSISDN), msg);
                    if (senegalStatusCode.Result == "Success")
                    {
                        return View("RegisterVerifyCode", form);
                    }
                    else
                    {
                        ModelState.AddModelError("", senegalStatusCode.Result);
                        FillOperator();
                        return View("Register", form);
                    }
                }
                else
                {
                    var statusCode = SendSms(form.MSISDN, form.CountryCode);
                    var resultData = statusCode.Result;
                    if (resultData == "OK")
                    {
                        //ViewBag.PhoneNumber = MSISDN;
                        //ViewBag.CountryCode = CountryCode;
                        return View("RegisterVerifyCode", form);
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unknown error occurred.");
                        FillOperator();
                        return View("Register", form);
                    }
                }

            }
            catch (Exception ex)
            {
                return View("Register", form);
            }

        }

        // Normal Register Verification method
        [Route("NormalRegistration")]
        [HttpPost]
        public ActionResult NormalRegistration(UserFormModel userFormModel)
        {

            var code = userFormModel.VerifcationCode.Split('-');

            //var code1 = strCode1;
            //var code2 = strCode2;
            //var code3 = strCode3;

            var code1 = TempData["code1"].ToString();
            var code2 = TempData["code2"].ToString();
            var code3 = TempData["code3"].ToString();

            var receivedCode1 = code[0];
            var receivedCode2 = code[1];
            var receivedCode3 = code[2];

            if (code1 != null && code1 != null && code1 != null)
            {

                if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                {
                    //271191
                    //var returnCode = SoapApiProcess.AddSoapUser(MSISDN);

                    if (userFormModel.OperatorId == (int)OperatorTableId.Safaricom)
                    {
                        var userId = SafaricomOperator(userFormModel);
                        if (userId != 0)
                        {
                            var user = _userRepository.GetById(userId);
                            formAuthentication.SetAuthCookie(HttpContext,
                                    UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                        user));
                            return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                        }
                        else
                        {
                            TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
                            return View("RegisterVerifyCode", userFormModel);
                        }
                    }
                    else if (userFormModel.OperatorId == (int)OperatorTableId.Expresso)
                    {
                        var expressoResult = ExpressoOperator.ExpressoProvision(String.Concat(userFormModel.CountryCode, userFormModel.MSISDN), "true");
                        if (expressoResult.Count() > 0)
                        {
                            if (expressoResult.FirstOrDefault().code == "0001")
                            {
                                var userId = OperatorProcess(userFormModel);
                                if (userId != 0)
                                {
                                    var user = _userRepository.GetById(userId);
                                    formAuthentication.SetAuthCookie(HttpContext,
                                            UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                user));
                                    return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                                }
                                else
                                {
                                    TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
                                    return View("RegisterVerifyCode", userFormModel);
                                }
                            }
                            else
                            {
                                TempData["Error"] = expressoResult.FirstOrDefault().message;
                                return View("RegisterVerifyCode", userFormModel);
                            }

                        }
                        else
                        {
                            TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
                            return View("RegisterVerifyCode", userFormModel);
                        }

                    }
                    else
                    {
                        TempData["Error"] = "This operators implementation is under process. So please contact adtones admin.";
                        return View("RegisterVerifyCode", userFormModel);
                    }

                }// End Here 
                else
                {
                    TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below";
                    return View("RegisterVerifyCode", userFormModel);
                }

            }
            else
            {
                TempData["Error"] = "Something went wrong. Please try again.";
                return View("RegisterVerifyCode", userFormModel);
            }


            //return Json(new { status = "Fail", Message = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below" }, JsonRequestBehavior.AllowGet);

        }

        [Route("SaveUSSDUserRegister")]
        [HttpPost]
        public ActionResult SaveUSSDUserRegister(string msisdn, string email, string password)
        {
            USSDUserFormModel userFormModel = new USSDUserFormModel();
            userFormModel.MSISDN = msisdn;
            userFormModel.Email = email;
            userFormModel.Password = password;
            EFMVCDataContex db = new EFMVCDataContex();
            User userData = db.Users.Where(u => u.UserProfiles.FirstOrDefault().MSISDN == userFormModel.MSISDN && u.RoleId == 2 && u.Activated != 3).FirstOrDefault();
            if (userData != null)
            {
                if (userData.OperatorId == (int)OperatorTableId.Safaricom)
                {
                    userData.Email = userFormModel.Email;
                    userData.PasswordHash = Md5Encrypt.Md5EncryptPassword(userFormModel.Password);
                    db.SaveChanges();
                    var ConnString = ConnectionString.GetConnectionStringByOperatorId(userData.OperatorId);
                    if (ConnString != null && ConnString.Count > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db1 = new EFMVCDataContex(item);
                            User userData1 = db1.Users.Where(u => u.UserProfiles.FirstOrDefault().MSISDN == userFormModel.MSISDN && u.RoleId == 2 && u.Activated != 3).FirstOrDefault();
                            if (userData1 != null)
                            {
                                if (userData1.OperatorId == 1)
                                {
                                    userData1.Email = userFormModel.Email;
                                    userData1.PasswordHash = Md5Encrypt.Md5EncryptPassword(userFormModel.Password);
                                    db1.SaveChanges();
                                }
                            }
                        }
                    }
                    int userId = userData.UserId;
                    if (userId != 0)
                    {
                        SensEmailToUser(email, password);
                        formAuthentication.SetAuthCookie(HttpContext,
                                UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                    userData));
                        return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong. Please try again.";
                        return View("USSDRegisterUser", userFormModel);
                    }
                }
                else if (userData.OperatorId == (int)OperatorTableId.Expresso)
                {
                    userData.Email = userFormModel.Email;
                    userData.PasswordHash = Md5Encrypt.Md5EncryptPassword(userFormModel.Password);
                    db.SaveChanges();
                    var ConnString = ConnectionString.GetConnectionStringByOperatorId(userData.OperatorId);
                    if (ConnString != null && ConnString.Count > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db1 = new EFMVCDataContex(item);
                            User userData1 = db1.Users.Where(u => u.UserProfiles.FirstOrDefault().MSISDN == userFormModel.MSISDN && u.RoleId == 2 && u.Activated != 3).FirstOrDefault();
                            if (userData1 != null)
                            {
                                if (userData1.OperatorId == 1)
                                {
                                    userData1.Email = userFormModel.Email;
                                    userData1.PasswordHash = Md5Encrypt.Md5EncryptPassword(userFormModel.Password);
                                    db1.SaveChanges();
                                }
                            }
                        }
                    }
                    var userId = userData.UserId;
                    if (userId != 0)
                    {
                        SensEmailToUser(email, password);
                        formAuthentication.SetAuthCookie(HttpContext,
                                UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                    userData));
                        return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                    }
                    else
                    {
                        TempData["Error"] = "Something went wrong. Please try again.";
                        return View("USSDRegisterUser", userFormModel);
                    }
                }
                else
                {
                    TempData["Error"] = "Something went wrong. Please try again.";
                    return View("RegisterVerifyCode", userFormModel);
                }
            }
            else
            {
                TempData["Error"] = "Something went wrong. Please try again.";
                return View("RegisterVerifyCode", userFormModel);
            }
        }

        //Social login Verification method
        [Route("SocialLoginRegistration")]
        public ActionResult SocialLoginRegistration(string confirmCode, string phoneNumber, string countryCode, string connectionToken, string userToken, int operatorId)
        {

            var code = confirmCode.Split('-');

            //var code1 = strCode1;
            //var code2 = strCode2;
            //var code3 = strCode3;

            var code1 = TempData["code1"].ToString();
            var code2 = TempData["code2"].ToString();
            var code3 = TempData["code3"].ToString();

            var receivedCode1 = code[0];
            var receivedCode2 = code[1];
            var receivedCode3 = code[2];

            if (code1 != null && code1 != null && code1 != null)
            {
                if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                {
                    var client = new RestClient("https://adtones.api.oneall.com/connections/" + connectionToken + ".json");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("postman-token", "959ecc3a-e1df-570e-81bc-df3133a0add2");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", "Basic NzdiMmJlOGYtMDFkMi00ZDdkLWExYmItOTExMjgzNzkwNDIzOjk2YmM1YWFmLTM5NTUtNGQxMy05NThmLTM3MjM0ZThkYjM1NA==");
                    //authorization BasicAuth UserName:publicKey Password: privateKey  
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var firstName = "";
                        var lastName = "";
                        var content = response.Content;
                        var desearlizeObj = JsonConvert.DeserializeObject<SocialLoginModel.RootObject>(content);

                        //Add 07-02-2019
                        if (desearlizeObj.response.result.data.user.identity.name == null)
                        {
                            firstName = "Tjeep";
                            lastName = "Tjeep";
                        }
                        else
                        {
                            firstName = desearlizeObj.response.result.data.user.identity.name.givenName;
                            lastName = desearlizeObj.response.result.data.user.identity.name.familyName;
                        }

                        if (firstName == null)
                        {
                            firstName = desearlizeObj.response.result.data.user.identity.preferredUsername;
                        }
                        if (lastName == null)
                        {
                            lastName = desearlizeObj.response.result.data.user.identity.preferredUsername;
                        }
                        //var email = desearlizeObj.response.result.data.user.identity.emails.FirstOrDefault().value;

                        string email = null;
                        var emailData = desearlizeObj.response.result.data.user.identity.emails;
                        if (emailData != null)
                        {
                            email = emailData.FirstOrDefault().value;
                        }

                        var provider = desearlizeObj.response.result.data.user.identity.provider;

                        var MSISDN = countryCode + phoneNumber;

                        EFMVCDataContex db = new EFMVCDataContex();
                        UserMatchTableProcess objUserMatch = new UserMatchTableProcess();
                        var userProfileInfo = db.Userprofiles.Where(s => s.MSISDN == MSISDN).FirstOrDefault();
                        if (userProfileInfo == null)
                        {
                            UserFormModel userFormModel = new UserFormModel();
                            userFormModel.Email = email;
                            userFormModel.FirstName = firstName;
                            userFormModel.LastName = lastName;
                            userFormModel.Organisation = "Adtones";
                            userFormModel.OperatorId = operatorId;
                            userFormModel.CountryCode = countryCode;
                            userFormModel.MSISDN = phoneNumber;

                            if (userFormModel.OperatorId == (int)OperatorTableId.Safaricom)
                            {
                                var userId = SafaricomOperator(userFormModel);
                                if (userId != 0)
                                {
                                    var user = _userRepository.GetById(userId);

                                    UserTokenLink tokenLink = new UserTokenLink();
                                    tokenLink.UserId = userId;
                                    tokenLink.UserToken = userToken;
                                    tokenLink.Provider = provider;
                                    db.UserTokenLink.Add(tokenLink);
                                    db.SaveChanges();
                                }
                            }
                            else if (userFormModel.OperatorId == (int)OperatorTableId.Expresso)
                            {
                                var expressoResult = ExpressoOperator.ExpressoProvision(userFormModel.MSISDN, "true");
                                if (expressoResult.Count() > 0)
                                {
                                    if (expressoResult.FirstOrDefault().code == "0001")
                                    {
                                        var userId = OperatorProcess(userFormModel);
                                        if (userId != 0)
                                        {
                                            var user = _userRepository.GetById(userId);
                                            UserTokenLink tokenLink = new UserTokenLink();
                                            tokenLink.UserId = userId;
                                            tokenLink.UserToken = userToken;
                                            tokenLink.Provider = provider;
                                            db.UserTokenLink.Add(tokenLink);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { status = "Fail", Message = "Something went wrong. Please try again!" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    return Json(new { status = "Fail", Message = "Something went wrong. Please try again!" }, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else
                            {
                                TempData["Error"] = "This operators implementation is under process. So please contact adtones admin.";
                                return View("RegisterVerifyCode", userFormModel);
                            }

                        }
                        else
                        {
                            var userId = userProfileInfo.UserId;
                            var userTokenData = db.UserTokenLink.Where(s => s.UserId == userId).FirstOrDefault();
                            if (userTokenData != null)
                            {
                                userTokenData.UserToken = userToken;
                                userTokenData.Provider = provider;
                                db.SaveChanges();
                            }

                        }

                        if (countryCode == "44")
                        {
                            // CreateNumberBeginingTable(MSISDN); // Remove code when Socket connection is live or for testing
                        }

                        User userData = _userRepository.Get(u => u.Email.ToLower() == email.ToLower() && u.RoleId == 2 && u.Activated == 1 && u.Activated != 3);
                        formAuthentication.SetAuthCookie(HttpContext,
                                UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                    userData));

                        //var urls = ConfigurationManager.AppSettings["userURL"].ToString();
                        //var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                        return Json(new { status = "Success", Message = "Success" }, JsonRequestBehavior.AllowGet);

                    }
                    return Json(new { status = "Fail", Message = "Something went wrong. Please try again!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { status = "Fail", Message = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = "Fail", Message = "Something went wrong. Please try again!" }, JsonRequestBehavior.AllowGet);
            }

        }

        private int OperatorProcess(UserFormModel userFormModel)
        {

            OperatorFunctionality objOperator = new OperatorFunctionality(_userRepository, _operatorRepository, _rewardRepository, _userProfileRepository, formAuthentication, _profileRepository, _commandBus);
            var reader =
             new StreamReader(
                 Server.MapPath(ConfigurationManager.AppSettings["UsersResendVerificationEmailTemplate"]));
            var userId = objOperator.AddUser(userFormModel, reader);
            return userId;
        }

        private int SafaricomOperator(UserFormModel userFormModel)
        {
            var returnCode = SoapApiProcess.AddSoapUser(userFormModel.MSISDN);
            //var returnCode = SoapApiProcess.AddCorpUser(userFormModel.MSISDN);
            var soapResonseCodeData = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == returnCode).FirstOrDefault();
            if (returnCode.Contains("?"))
            {
                //100001 is Unknown error
                GenerateTicket(returnCode, userFormModel.Email, userFormModel.FirstName + " " + userFormModel.LastName);
                //TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
            }
            else if (soapResonseCodeData != null)
            {
                if (soapResonseCodeData.ReturnCode == "000000")
                {
                    var userId = OperatorProcess(userFormModel);
                    return userId;
                }
                else
                {
                    GenerateTicket(returnCode, userFormModel.Email, userFormModel.FirstName + " " + userFormModel.LastName);
                    //TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
                }
            }
            else
            {
                GenerateTicket(returnCode, userFormModel.Email, userFormModel.FirstName + " " + userFormModel.LastName);
                // TempData["Error"] = "I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience.if the interface is generating multiple time outs contact safaricom support at support@adtones.xyz or call  +44 (0) 7711 760 222";
            }
            return 0;
        }

        public string GetErrorMessage(string errorCode)
        {

            var emailAddress = ConfigurationManager.AppSettings["SiteEmailAddress"];
            var phoneNumber = ConfigurationManager.AppSettings["SiteContactNumber"];
            if (errorCode == "0")
                return @"I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience - Internal message 'error " + errorCode + " - " + "Unable to connect to the remote server - if the interface is generating multiple time outs contact safaricom support at " + emailAddress + " or call " + phoneNumber + "'";

            string errorMessage = "Something went wrong. Please try again!";
            var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == errorCode).FirstOrDefault();
            if (responseCodeDetail != null)
                errorMessage = responseCodeDetail.Description;
            return @"I'm sorry we have experienced a technical error - please try again in 15 minutes - apologies for the inconvenience - Internal message 'error " + errorCode + " - " + errorMessage + " - if the interface is generating multiple time outs contact safaricom support at " + emailAddress + " or call " + phoneNumber + "'";
        }

        public void GenerateTicket(string responseCode, string email, string userName)
        {
            string message = "";
            if (responseCode == "0" || responseCode.Contains("?"))
            {
                message = responseCode + " - Unable to connect to the remote server";
            }
            else
            {
                var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
                if (responseCodeDetail != null)
                {
                    message = responseCode + " - " + responseCodeDetail.Description;
                }
                else
                {
                    message = responseCode + " - please add this response code";
                }
            }
            string ticketCode = LiveAgent.CreateTicket("User registration error", message, email);
            QuestionFormModel _model = new QuestionFormModel();

            _model.UserId = null;
            _model.QNumber = ticketCode;
            _model.SubjectId = (int)QuestionSubjectStatus.Other;
            _model.ClientId = null;
            _model.CampaignProfileId = null;
            _model.PaymentMethodId = null;
            _model.Title = "User registration error";
            _model.Description = message;
            _model.CreatedDate = DateTime.Now;
            _model.UpdatedDate = DateTime.Now;
            _model.LastResponseDateTime = null;
            _model.LastResponseDateTimeByUser = null;
            _model.Status = (int)QuestionStatus.New;
            _model.UpdatedBy = null;
            //   _model.UserName = userName;
            _model.Email = email;

            CreateOrUpdateQuestionCommand command =
               Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(_model);
            ICommandResult result = _commandBus.Submit(command);

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
                var senegalStatusCode = activExpertSMS.SendSMS(String.Concat(countryCode, phoneNumber), msg);
                if (senegalStatusCode.Result == "Success")
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var statusCode = SendSms(phoneNumber, countryCode);
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

        //Code commented on 09/08/2018 for sms api 
        //[Route("Register")]
        //[HttpPost]
        //public ActionResult SaveRegister(UserFormModel form)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
        //        command.Activated = 0;
        //        command.RoleId = (Int32)UserRoles.User;
        //        command.Outstandingdays = 0;
        //        IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
        //        ModelState.AddModelErrors(errors);
        //        if (ModelState.IsValid)
        //        {
        //            ICommandResult result = _commandBus.Submit(command);
        //            if (result.Success)
        //            {
        //                User user = _userRepository.Get(u => u.Email == form.Email);
        //                string email = EncryptionHelper.EncryptSingleValue(user.Email);
        //                string url = string.Format("{0}?activationCode={1}",
        //                                           ConfigurationManager.AppSettings["UsersVerificationUrl"], email);

        //                var reader =
        //                    new StreamReader(
        //                        Server.MapPath(ConfigurationManager.AppSettings["UsersVerificationEmailTemplate"]));
        //                string emailContent = reader.ReadToEnd();
        //                emailContent = string.Format(emailContent, url);

        //                //MailHelper.SendMail(ConfigurationManager.AppSettings["SiteEmailAddress"], user.Email, null, null,
        //                //                    "Email Verification", emailContent,
        //                //                    ConfigurationManager.AppSettings["SmtpServerAddress"],
        //                //                    int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]));


        //                MailMessage mail = new MailMessage();
        //                mail.To.Add(user.Email);
        //                //mail.To.Add("xxx@gmail.com");
        //                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
        //                mail.Subject = "Email Verification";

        //                mail.Body = emailContent;

        //                mail.IsBodyHtml = true;
        //                SmtpClient smtp = new SmtpClient();
        //                smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
        //                smtp.Credentials = new System.Net.NetworkCredential
        //                     (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
        //                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

        //                //Or your Smtp Email ID and Password
        //                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
        //                smtp.Send(mail);                       
        //                return RedirectToAction("RegisterUsersSuccess", "Login");
        //            }

        //            ModelState.AddModelError("", "An unknown error occurred.");
        //        }
        //        else
        //        {
        //            FillOperator();
        //            return View("Register", form);
        //        }
        //        // If we got this far, something failed, redisplay form

        //    }
        //    return View("Register", form);
        //    // If we got this far, something failed

        //}



        [Route("RegisterUsersSuccess")]
        public ActionResult RegisterUsersSuccess()
        {

            return View();
        }

        public void sendEmail(string to, string fname, string lname, string subject, string formatId, string[] mailTo, string[] mailCC, string[] mailBcc, string[] attachment, bool isBodyHTML, string completedDatetime)
        {
            var EmailContent = new SendEmailModel();
            if (mailTo != null)
            {
                EmailContent.To = mailTo;
            }
            if (mailCC != null)
            {
                EmailContent.CC = mailCC;
            }
            if (mailBcc != null)
            {
                EmailContent.Bcc = mailBcc;
            }
            if (attachment != null)
            {
                EmailContent.attachment = attachment;
            }
            EmailContent.Link = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Admin/UserManagement/Index";
            EmailContent.isBodyHTML = isBodyHTML;
            EmailContent.Fname = fname;
            EmailContent.Lname = lname;
            EmailContent.Subject = subject;
            EmailContent.FormatId = 1;
            EmailContent.CompletedDatetime = completedDatetime;
            sendEmailMailer.SendEmail(EmailContent).SendAsync();
        }
        [Route("VerifyUser")]
        public ActionResult VerifyUser()
        {
            ConfirmCodeModel model = new ConfirmCodeModel();
            try
            {
                string email = Request.QueryString["activationCode"];
                email = EncryptionHelper.DecryptSingleValue(email);

                User user = _userRepository.Get(x => x.Email == email);

                if (user != null && user.UserId != 0)
                {
                    model.UserId = user.UserId;
                }

            }
            catch (Exception)
            {
                // ViewData.Add("VerifyAdvertiserResult", "failed");
            }

            return View(model);
        }

        [HttpPost]
        [Route("VerifyUser")]
        public ActionResult VerifyUser(ConfirmCodeModel model)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var verificationDetail = db.EmailVerificationCode.Where(s => s.UserId == model.UserId).FirstOrDefault();
                if (verificationDetail != null)
                {
                    if (verificationDetail.VerificationCode == model.Confirm)
                    {
                        db.EmailVerificationCode.Remove(verificationDetail);
                        db.SaveChanges();

                        User user = db.Users.Where(s => s.UserId == model.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            user.IsEmailVerfication = true;
                            user.VerificationStatus = true;
                            db.SaveChanges();

                            formAuthentication.SetAuthCookie(HttpContext,
                             UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                           user));
                            return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                        }
                        else
                        {
                            TempData["Error"] = "User not found.";
                        }


                    }
                    else
                    {
                        TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
                    }

                }
                else
                {
                    TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
            }
            return View(model);

        }

        [Route("VerifyRegisterUser")]
        public ActionResult VerifyRegisterUser()
        {
            ConfirmCodeModel model = new ConfirmCodeModel();
            try
            {

                string email = Request.QueryString["activationCode"];
                email = EncryptionHelper.DecryptSingleValue(email);

                User user = _userRepository.Get(x => x.Email == email);

                if (user != null && user.UserId != 0)
                {
                    model.UserId = user.UserId;
                    ViewBag.Result = "Success";
                }
                else
                {
                    ViewBag.Result = "Fail";
                }

            }
            catch (Exception)
            {
                ViewBag.Result = "Fail";
            }

            return View(model);
        }

        [HttpPost]
        [Route("VerifyRegisterUser")]
        public ActionResult VerifyRegisterUser(ConfirmCodeModel model)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var verificationDetail = db.EmailVerificationCode.Where(s => s.UserId == model.UserId).FirstOrDefault();
                if (verificationDetail != null)
                {
                    if (verificationDetail.VerificationCode == model.Confirm)
                    {
                        db.EmailVerificationCode.Remove(verificationDetail);
                        db.SaveChanges();

                        User user = db.Users.Where(s => s.UserId == model.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            user.IsEmailVerfication = true;
                            user.VerificationStatus = true;
                            db.SaveChanges();

                            formAuthentication.SetAuthCookie(HttpContext,
                             UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                           user));
                            return RedirectToAction("Index", "AccountOverview", new { Area = "Users" });
                        }
                        else
                        {
                            TempData["Error"] = "User not found.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
                    }

                }
                else
                {
                    TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code.";
            }
            return View(model);

        }

        [Route("Callback")]
        public ActionResult Callback()
        {
            return View();
        }

        //[Route("Callback")]
        //public ActionResult Callback(string userToken,string connectionToken)
        //{
        //    if (!string.IsNullOrEmpty(connectionToken))
        //    {
        //        //var client = new RestClient("https://adtones.api.oneall.com/connections/" + connectionToken + ".json");
        //        //var request = new RestRequest(Method.GET);
        //        //request.AddHeader("postman-token", "959ecc3a-e1df-570e-81bc-df3133a0add2");
        //        //request.AddHeader("cache-control", "no-cache");
        //        //request.AddHeader("authorization", "Basic NzdiMmJlOGYtMDFkMi00ZDdkLWExYmItOTExMjgzNzkwNDIzOjk2YmM1YWFmLTM5NTUtNGQxMy05NThmLTM3MjM0ZThkYjM1NA==");
        //        ////authorization BasicAuth UserName:publicKey Password: privateKey  
        //        //IRestResponse response = client.Execute(request);
        //        //if(response.StatusCode == System.Net.HttpStatusCode.OK)
        //        //{
        //        //    var content = response.Content;
        //        //    var desearlizeObj = JsonConvert.DeserializeObject<SocialLoginModel.RootObject>(content);
        //        //    var firstName = desearlizeObj.response.result.data.user.identity.name.givenName;
        //        //    var lastName = desearlizeObj.response.result.data.user.identity.name.familyName;
        //        //    var email =  desearlizeObj.response.result.data.user.identity.emails.FirstOrDefault().value;

        //        //    var provider = desearlizeObj.response.result.data.user.identity.provider;

        //        //    EFMVCDataContex db = new EFMVCDataContex();
        //        //    var isUserTokenExist = db.UserTokenLink.Where(s => s.UserToken == userToken).Any();
        //        //    if(!isUserTokenExist)
        //        //    {
        //        //        User user = new User();
        //        //        user.Email = email;
        //        //        user.FirstName = firstName;
        //        //        user.LastName = lastName;
        //        //        user.DateCreated = DateTime.Now;
        //        //        user.Organisation = "Adtones";
        //        //        user.LastLoginTime = DateTime.Now;
        //        //        user.Activated = 1;
        //        //        user.RoleId = 2;
        //        //        user.VerificationStatus = true;
        //        //        user.Outstandingdays = 0;
        //        //        user.OperatorId = 1;
        //        //        user.IsMsisdnMatch = true;
        //        //        db.Users.Add(user);
        //        //        db.SaveChanges();

        //        //        UserTokenLink tokenLink = new UserTokenLink();
        //        //        tokenLink.UserId = user.UserId;
        //        //        tokenLink.UserToken = userToken;
        //        //        tokenLink.Provider = provider;
        //        //        db.UserTokenLink.Add(tokenLink);
        //        //        db.SaveChanges();

        //        //        User userData = _userRepository.Get(u => u.Email == email && u.Activated != 3);
        //        //        formAuthentication.SetAuthCookie(HttpContext,
        //        //               UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
        //        //                   userData));

        //        //        var urls = ConfigurationManager.AppSettings["userURL"].ToString();
        //        //        var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
        //        //        return Redirect(siteaddress + urls);
        //        //    }
        //        //    else
        //        //    {
        //        //        User user = _userRepository.Get(u => u.Email == email && u.Activated != 3);
        //        //        formAuthentication.SetAuthCookie(HttpContext,
        //        //               UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
        //        //                   user));
        //        //        var urls = ConfigurationManager.AppSettings["userURL"].ToString();
        //        //        var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
        //        //        return Redirect(siteaddress + urls);
        //        //    }

        //        //}

        //        EFMVCDataContex db = new EFMVCDataContex();
        //        var userTokenData = db.UserTokenLink.Where(s => s.UserToken == userToken).FirstOrDefault();
        //        if (userTokenData == null)
        //        {
        //            //string userToken,string connectionToken
        //            ViewBag.UserToken = userToken;
        //            ViewBag.ConnectionToken = connectionToken;
        //            return View();
        //        }
        //        else
        //        {
        //            var userId = userTokenData.UserId;
        //            User user = _userRepository.Get(u => u.UserId == userId && u.Activated != 3);
        //            formAuthentication.SetAuthCookie(HttpContext,
        //                   UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
        //                       user));
        //            var urls = ConfigurationManager.AppSettings["userURL"].ToString();
        //            var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
        //            return Redirect(siteaddress + urls);
        //        }


        //    }
        //    else
        //    {
        //        //return Redirect("Index");
        //        return View();
        //    }

        //}

        //[Route("Test")]
        //public ActionResult Test()
        //{
        //    var url = Request.UrlReferrer;
        //    var connectionToken = url.Query.Split('=')[1];

        //    var client = new RestClient("https://adtones.api.oneall.com/connections/" + connectionToken + ".json");
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("postman-token", "959ecc3a-e1df-570e-81bc-df3133a0add2");
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("authorization", "Basic NzdiMmJlOGYtMDFkMi00ZDdkLWExYmItOTExMjgzNzkwNDIzOjk2YmM1YWFmLTM5NTUtNGQxMy05NThmLTM3MjM0ZThkYjM1NA==");
        //    //authorization BasicAuth UserName:publicKey Password: privateKey  
        //    IRestResponse response = client.Execute(request);


        //    return Json(true);
        //}


        [Route("Verification")]
        public ActionResult Verification(string phoneNumber, string countryCode)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            string number = countryCode + phoneNumber;
            var isMobileExist = db.Userprofiles.Where(s => s.MSISDN == number).Any();
            if (isMobileExist)
            {
                return Json("MobileExist", JsonRequestBehavior.AllowGet);
            }

            var countryId = _countryRepository.Get(top => top.CountryCode == String.Concat("+", countryCode)).Id;
            var operatorId = _operatorRepository.Get(top => top.CountryId == countryId).OperatorId;
            if (operatorId == 2)
            {
                string msg = RandomNumberGenerator();
                ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                var senegalStatusCode = activExpertSMS.SendSMS(String.Concat(countryCode, phoneNumber), msg);
                if (senegalStatusCode.Result == "Success")
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var statusCode = SendSms(phoneNumber, countryCode);
                var result = statusCode.Result;
                if (result == "OK")
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Route("ChangeSocialLogin")]
        public ActionResult ChangeSocialLogin(string phoneNumber, string countryCode)
        {

            EFMVCDataContex db = new EFMVCDataContex();
            string number = countryCode + phoneNumber;
            var userProfileData = db.Userprofiles.Where(s => s.MSISDN == number).FirstOrDefault();
            if (userProfileData != null)
            {
                var countryId = _countryRepository.Get(top => top.CountryCode == String.Concat("+", countryCode)).Id;
                var operatorId = _operatorRepository.Get(top => top.CountryId == countryId).OperatorId;
                if (operatorId == 2)
                {
                    string msg = RandomNumberGenerator();
                    ActivExpertSMS activExpertSMS = new ActivExpertSMS();
                    var senegalStatusCode = activExpertSMS.SendSMS(String.Concat(countryCode, phoneNumber), msg);
                    if (senegalStatusCode.Result == "Success")
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Fail", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var statusCode = SendSms(phoneNumber, countryCode);
                    var result = statusCode.Result;
                    if (result == "OK")
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
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

        [Route("SameSocialLogin")]
        public ActionResult SameSocialLogin(string phoneNumber, string countryCode)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var number = countryCode + phoneNumber;
            var userProfileData = db.Userprofiles.Where(s => s.MSISDN == number).FirstOrDefault();
            if (userProfileData != null)
            {
                var userId = userProfileData.UserId;

                User userData = _userRepository.Get(u => u.UserId == userId && u.Activated != 3);
                if (userData != null)
                {
                    formAuthentication.SetAuthCookie(HttpContext,
                            UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                userData));
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Cancel", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }


        [Route("VerificationFail")]
        public ActionResult VerificationFail()
        {
            return View();
        }




        //[Route("ConfirmCode")]
        //public ActionResult ConfirmCode(string phoneNumber, string countryCode, string connectionToken, string userToken)
        //{
        //    ConfirmCodeModel model = new ConfirmCodeModel();
        //    // var cntCode = countryCode.Replace(" ", "+");
        //    ViewBag.PhoneNumber = phoneNumber;
        //    ViewBag.CountryCode = countryCode;
        //    ViewBag.ConnectionToken = connectionToken;
        //    ViewBag.UserToken = userToken;

        //    model.PhoneNumber = phoneNumber;
        //    model.CountryCode = countryCode;
        //    model.ConnectionToken = connectionToken;
        //    model.UserToken = userToken;

        //    return View(model);
        //}

        //[Route("ConfirmCode")]
        //[HttpPost]
        //public ActionResult ConfirmCode(ConfirmCodeModel model)
        //{
        //    var code = model.Confirm.Split('-');

        //    var code1 = strCode1;
        //    var code2 = strCode2;
        //    var code3 = strCode3;

        //    var receivedCode1 = code[0];
        //    var receivedCode2 = code[1];
        //    var receivedCode3 = code[2];

        //    if (code1 != null && code1 != null && code1 != null)
        //    {
        //        if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
        //        {
        //            var client = new RestClient("https://adtones.api.oneall.com/connections/" + model.ConnectionToken + ".json");
        //            var request = new RestRequest(Method.GET);
        //            request.AddHeader("postman-token", "959ecc3a-e1df-570e-81bc-df3133a0add2");
        //            request.AddHeader("cache-control", "no-cache");
        //            request.AddHeader("authorization", "Basic NzdiMmJlOGYtMDFkMi00ZDdkLWExYmItOTExMjgzNzkwNDIzOjk2YmM1YWFmLTM5NTUtNGQxMy05NThmLTM3MjM0ZThkYjM1NA==");
        //            //authorization BasicAuth UserName:publicKey Password: privateKey  
        //            IRestResponse response = client.Execute(request);
        //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                var content = response.Content;
        //                var desearlizeObj = JsonConvert.DeserializeObject<SocialLoginModel.RootObject>(content);
        //                var firstName = desearlizeObj.response.result.data.user.identity.name.givenName;
        //                var lastName = desearlizeObj.response.result.data.user.identity.name.familyName;
        //                if (firstName == null)
        //                {
        //                    firstName = desearlizeObj.response.result.data.user.identity.preferredUsername;
        //                }
        //                if (lastName == null)
        //                {
        //                    lastName = desearlizeObj.response.result.data.user.identity.preferredUsername;
        //                }
        //                var email = desearlizeObj.response.result.data.user.identity.emails.FirstOrDefault().value;

        //                var provider = desearlizeObj.response.result.data.user.identity.provider;

        //                var MSISDN = model.CountryCode + model.PhoneNumber;


        //                EFMVCDataContex db = new EFMVCDataContex();

        //                var userProfileInfo = db.Userprofiles.Where(s => s.MSISDN == MSISDN).FirstOrDefault();
        //                if (userProfileInfo == null)
        //                {
        //                    UserMatchTableProcess obj = new UserMatchTableProcess();
        //                    User user = new User();
        //                    user.Email = email;
        //                    user.FirstName = firstName;
        //                    user.LastName = lastName;
        //                    user.DateCreated = DateTime.Now;
        //                    user.Organisation = "Adtones";
        //                    user.LastLoginTime = DateTime.Now;
        //                    user.Activated = 1;
        //                    user.RoleId = 2;
        //                    user.VerificationStatus = true;
        //                    user.Outstandingdays = 0;
        //                    user.OperatorId = 1;
        //                    user.IsMsisdnMatch = true; // when soap ui code uncomment then make it false
        //                    user.IsMobileVerfication = true;
        //                    user.UserMatchTableName = obj.GetUserMatchTableNumber(OperatorId);
        //                    db.Users.Add(user);
        //                    db.SaveChanges();

        //                    UserTokenLink tokenLink = new UserTokenLink();
        //                    tokenLink.UserId = user.UserId;
        //                    tokenLink.UserToken = model.UserToken;
        //                    tokenLink.Provider = provider;
        //                    db.UserTokenLink.Add(tokenLink);
        //                    db.SaveChanges();

        //                    UserProfile objUserProfile = new UserProfile();
        //                    objUserProfile.UserId = user.UserId;
        //                    objUserProfile.DOB = null;
        //                    objUserProfile.Gender = null;
        //                    objUserProfile.IncomeBracket = null;
        //                    objUserProfile.WorkingStatus = null;
        //                    objUserProfile.RelationshipStatus = null;
        //                    objUserProfile.Education = null;
        //                    objUserProfile.HouseholdStatus = null;
        //                    objUserProfile.Location = null;
        //                    objUserProfile.MSISDN = MSISDN;
        //                    db.Userprofiles.Add(objUserProfile);
        //                    db.SaveChanges();

        //                    var reader =
        //                        new StreamReader(
        //                            Server.MapPath(ConfigurationManager.AppSettings["UsersResendVerificationEmailTemplate"]));
        //                    string emailContent = reader.ReadToEnd();

        //                    Random generator = new Random();
        //                    String emailcode1 = generator.Next(0, 99).ToString("D2");
        //                    String emailcode2 = generator.Next(0, 99).ToString("D2");
        //                    String emailcode3 = generator.Next(0, 99).ToString("D2");

        //                    string verifyCode = emailcode1 + " " + emailcode2 + " " + emailcode3 + " ";

        //                    emailContent = string.Format(emailContent, verifyCode);

        //                    MailMessage mail = new MailMessage();
        //                    mail.To.Add(user.Email);
        //                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
        //                    mail.Subject = "Email Verification";

        //                    mail.Body = emailContent;

        //                    mail.IsBodyHtml = true;
        //                    SmtpClient smtp = new SmtpClient();
        //                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
        //                    smtp.Credentials = new System.Net.NetworkCredential
        //                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
        //                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

        //                    //Or your Smtp Email ID and Password
        //                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
        //                    smtp.Send(mail);

        //                    var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == user.UserId).ToList();
        //                    foreach (var item in emailVerificationCode)
        //                    {
        //                        db.EmailVerificationCode.Remove(item);
        //                        db.SaveChanges();
        //                    }

        //                    EmailVerificationCode emailVerification = new EmailVerificationCode();
        //                    emailVerification.UserId = user.UserId;
        //                    emailVerification.VerificationCode = emailcode1 + "-" + emailcode2 + "-" + emailcode3;
        //                    emailVerification.DateCreated = DateTime.Now;
        //                    db.EmailVerificationCode.Add(emailVerification);
        //                    db.SaveChanges();


        //                }
        //                else
        //                {
        //                    var userId = userProfileInfo.UserId;
        //                    var userTokenData = db.UserTokenLink.Where(s => s.UserId == userId).FirstOrDefault();
        //                    if (userTokenData != null)
        //                    {
        //                        userTokenData.UserToken = model.UserToken;
        //                        userTokenData.Provider = provider;
        //                        db.SaveChanges();
        //                    }

        //                }



        //                User userData = _userRepository.Get(u => u.Email == email && u.RoleId == 2 && u.Activated == 1 && u.Activated != 3);
        //                formAuthentication.SetAuthCookie(HttpContext,
        //                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
        //                            userData));

        //                //var urls = ConfigurationManager.AppSettings["userURL"].ToString();
        //                //var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
        //                return RedirectToAction("Index", "AccountOverview", new { area = "Users" });

        //            }
        //            TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below.";
        //            return View(model);

        //        }
        //        else
        //        {
        //            TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below.";
        //            return View(model);
        //        }
        //    }
        //    else
        //    {
        //        TempData["Error"] = "I'm sorry this code was not recognised - please re-enter the code or press the code re-send button below.";
        //        return View(model);
        //    }
        //}



        [Route("SoapUIError")]
        public ActionResult SoapUIError(string errorCode)
        {
            ViewBag.ErrorCode = errorCode;
            return View();
        }

        //private async Task<string> ReceiveSms(string confirmCode)
        //{
        //    var code = confirmCode.Split('-');
        //    var code1 = code[0];
        //    var code2 = code[1];
        //    var code3 = code[2];

        //    //Get the parameters, either GET or POST request
        //    string uid = "adtones_sms_api";
        //    string pass = "2x8whkr";
        //    string from2 = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
        //    string to = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    string keyword = "Adtones";
        //    string vmn = "447860064145";
        //    //Exit if one or more parameters is missing
        //    //if (String.IsNullOrEmpty(uid) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(from2)
        //    //|| String.IsNullOrEmpty(to) || String.IsNullOrEmpty(keyword)) System.Environment.Exit(1);

        //    // Send the async request
        //    //HttpResponseMessage response = await client.GetAsync(
        //    //"https://www.voodoosms.com/vapi/server/getSMS?uid=" + uid + "&pass=" + pass
        //    //+ "&from=" + from2 + "&to=" + to + "&keyword=" + keyword);



        //    //HttpResponseMessage response = await client.GetAsync(
        //    //"https://www.voodoosms.com/vapi/server/getSMS?uid=" + uid + "&pass=" + pass
        //    //+ "&from=" + from2 + "&to=" + to + "&vmn=" + vmn);
        //    //// Get the response content
        //    //HttpContent responseContent = response.Content;

        //    //using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
        //    //{
        //    //    var testt = await reader.ReadToEndAsync();
        //    //    //Console.WriteLine(await reader.ReadToEndAsync());
        //    //}




        //    var client2 = new RestClient("https://www.voodoosms.com/vapi/server/getSMS?uid=" + uid + "&pass=" + pass + "&from=" + from2 + "&to=" + to + "&vmn=" + vmn);
        //    var request = new RestRequest(Method.GET);
        //    IRestResponse response = client2.Execute(request);

        //    var responseContent = response.Content;

        //    XmlDocument xmldoc = new XmlDocument();
        //    xmldoc.LoadXml(responseContent);
        //    XmlNodeList nodeList = xmldoc.GetElementsByTagName("message");

        //    if (nodeList.Count > 0)
        //    {
        //        foreach (XmlNode node in nodeList)
        //        {
        //            var returnCode = node.SelectSingleNode("Message").InnerXml;
        //        }
        //    }
        //   return response.StatusCode.ToString();
        //}

        private async Task<string> SendSms(string number, string countryCode)
        {
            var client = new HttpClient();

            //https://www.voodoosms.com/vapi/server/sendSMS?uid=adtones_sms_api&pass=2x8whkr&dest=447980720250&orig=Adtones&msg=your%20code%20is%2012%2012%2012&format=JSON
            //Get the parameters, either GET or POST request

            //strCode1 = null;
            //strCode2 = null;
            //strCode3 = null;

            TempData["code1"] = null;
            TempData["code2"] = null;
            TempData["code3"] = null;

            string uid = "adtones_sms_api";
            string pass = "2x8whkr";
            // string dest = "447980720250";
            string dest = countryCode + number;

            string orig = "Adtone"; // number
            //string orig = "447860064145";
            Random generator = new Random();
            String code1 = generator.Next(0, 99).ToString("D2");
            String code2 = generator.Next(0, 99).ToString("D2");
            String code3 = generator.Next(0, 99).ToString("D2");

            TempData["code1"] = code1;
            TempData["code2"] = code2;
            TempData["code3"] = code3;

            //strCode1 = code1;
            //strCode2 = code2;
            //strCode3 = code3;


            string msg = code1 + " " + code2 + " " + code3 + " is your Adtone verification code. It will expire in 24 hours";

            string format = "JSON";



            //Exit if one or more parameters is missing

            if (String.IsNullOrEmpty(uid) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(dest)
            || String.IsNullOrEmpty(orig) || String.IsNullOrEmpty(msg) || String.IsNullOrEmpty(format))
                System.Environment.Exit(1);

            // Send the async request

            //HttpResponseMessage response = await client.GetAsync(
            //"https://www.voodoosms.com/vapi/server/sendSMS?uid=" + uid + "&pass=" + pass
            //+ "&dest=" + dest + "&orig=" + orig + "&msg=" + msg + "&format=" + format);


            var client2 = new RestClient("https://www.voodoosms.com/vapi/server/sendSMS?uid=" + uid + "&pass=" + pass + "&dest=" + dest + "&orig=" + orig + "&msg=" + msg + "&format=" + format);
            var request = new RestRequest(Method.GET);
            //REMOVE THIS COMMENT
            IRestResponse response = client2.Execute(request);
            return response.StatusCode.ToString();
            // return "OK";



            // Get the response content

            //HttpContent responseContent = response.Content;
            //using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            //{
            //    var testt = await reader.ReadToEndAsync();
            //    //Console.WriteLine(await reader.ReadToEndAsync());
            //}



            //return "200";
        }

        private void CheckUserExistSoapApi(User usr)
        {
            #region old
            //try
            //{
            //    EFMVCDataContex db = new EFMVCDataContex();
            //    //if (usr.OperatorId == 1) //Safaricom
            //    //{
            //        var portalAccount = "adtones";
            //        var portalPassword = "TBD";
            //        var portalType = "1";
            //        var role = "4";
            //        var roleCode = "100";
            //        var corpId = "440114";
            //        // var phoneNumber = "254000000000";
            //        var phoneNumber = "";
            //        var userProfile = usr.UserProfiles.FirstOrDefault();
            //        if (userProfile != null)
            //            phoneNumber = userProfile.MSISDN;

            //        string soapUIUrl = ConfigurationManager.AppSettings["SoapUIUrl"];
            //        var client = new RestClient(soapUIUrl);
            //        var request = new RestRequest(Method.POST);

            //        request.AddParameter("undefined",
            //            "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
            //            "<event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n           \t" +
            //                "<portalAccount>" + portalAccount + "</portalAccount>\r\n\t\t\t<portalPwd>" + portalPassword + "</portalPwd>\r\n\t\t\t" +
            //                "<portalType>" + portalType + "</portalType>\r\n\t\t\t<role>" + role + "</role>\r\n\t\t\t" +
            //                "<roleCode>" + roleCode + "</roleCode>\r\n\t\t\t" +
            //                "<corpID>" + corpId + "</corpID>\r\n\t\t\t<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
            //            "</event>\r\n      </cor:addCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
            //        IRestResponse response = client.Execute(request);
            //        var responseContent = response.Content;

            //        XmlDocument xmldoc = new XmlDocument();
            //        xmldoc.LoadXml(responseContent);
            //        XmlNodeList nodeList = xmldoc.GetElementsByTagName("addCorpUserReturn");
            //        if (nodeList.Count > 0)
            //        {
            //            foreach (XmlNode node in nodeList)
            //            {
            //                var returnCode = node.SelectSingleNode("returnCode").InnerXml;
            //                if (returnCode == "000000")
            //                {
            //                    var getUser = db.Users.Where(s => s.UserId == usr.UserId).FirstOrDefault();
            //                    getUser.IsMsisdnMatch = true;
            //                    //getUser.Activated = 1;
            //                    //getUser.IsEmailVerfication = true;
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


            //                //var temp2 = node.SelectSingleNode("operationID").InnerXml;
            //            }
            //        }
            //   // }
            //}
            //catch (Exception ex)
            //{

            //}

            #endregion
        }
        public void sendemailtoadmin(string email, string fname, string lname)
        {

            //send email to admin
            var adminaddress = ConfigurationManager.AppSettings["adminemailaddress"].ToString();
            DateTimeOffset uktimezone = TimeZoneInfo.ConvertTime(
            DateTime.Now, TimeZoneInfo.Local,
            TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            // DateTime utc = uktimezone.UtcDateTime;

            string current_time = uktimezone.DateTime.ToString("HH:mm dd/MM/yyyy");
            // string current_time = utc.ToString("HH:mm dd/MM/yyyy");
            string subject = "Campaign Manager Registration-" + current_time + "-" + fname + " " + lname;
            string[] mailto = new string[1];
            mailto[0] = adminaddress;
            sendEmail(adminaddress, fname, lname, subject, "1", mailto, null, null, null, true, current_time);
            //var adminreader =
            //    new StreamReader(
            //        Server.MapPath(ConfigurationManager.AppSettings["AdminTemplete"]));
            //string adminemailContent = adminreader.ReadToEnd();
            //adminemailContent = string.Format(adminemailContent, email,fname,lname);
            //MailHelper.SendMail(ConfigurationManager.AppSettings["SiteEmailAddress"], ConfigurationManager.AppSettings["adminemailaddress"].ToString(), null, null,
            //                   "Registration", adminemailContent,
            //                   ConfigurationManager.AppSettings["SmtpServerAddress"],
            //                   int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]));
        }

        public void CreateNumberBeginingTable(string mobile)
        {
            string externalServerConnString = ConfigurationManager.AppSettings["externalserverconnectionstring"];

            string numberbeginning = mobile.Substring(0, 5);

            string Query = @"SELECT * FROM information_schema.tables WHERE table_name = '" + numberbeginning + "table'";
            MySqlConnection MyConn2 = new MySqlConnection(externalServerConnString);
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MySqlDataReader MyReader2;
            MyConn2.Open();
            MyReader2 = MyCommand2.ExecuteReader();

            if (!MyReader2.HasRows)
            {
                using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                {
                    using (MySqlCommand cmd = new MySqlCommand("create_number_table", conn))
                    {
                        cmd.CommandTimeout = 3600;
                        cmd.Parameters.Add(new MySqlParameter("numberbeginning", numberbeginning));
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        InsertRecord(numberbeginning, mobile, externalServerConnString);
                        conn.Close();
                    }
                }
            }
            else
            {
                InsertRecord(numberbeginning, mobile, externalServerConnString);
            }
            MyConn2.Close();

        }

        private void InsertRecord(string numberbeginning, string mobile, string externalServerConnString)
        {
            string Query = @"INSERT INTO " + numberbeginning + "table (MobileNumber,ServerDetail) VALUES ('" + mobile + "', 'Kenya external 8'" + ")";
            MySqlConnection MyConn2 = new MySqlConnection(externalServerConnString);
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MySqlDataReader MyReader2;
            MyConn2.Open();
            MyReader2 = MyCommand2.ExecuteReader();
            MyConn2.Clone();
        }

        private void AddRecordsOnPrematch(int userId, string userMatchTableName)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            AddUserProfilePreference(db, userId, userMatchTableName);

        }

        private void AddUserProfilePreference(EFMVCDataContex db, int userId, string userMatchTableName)
        {
            var profilePref = _profileRepository.GetMany(s => s.UserId == userId).FirstOrDefault();
            if (profilePref != null)
            {
                #region UserProfilePreference
                UserProfilePreference objPref = new UserProfilePreference();

                objPref.UserProfileId = profilePref.UserProfileId;
                objPref.Gender_Demographics = "C";
                objPref.WorkingStatus_Demographics = "I";
                objPref.RelationshipStatus_Demographics = "G";
                objPref.Education_Demographics = "E";
                objPref.HouseholdStatus_Demographics = "D";
                objPref.Location_Demographics = "A"; // null will not check on CampaignUserMatch Process
                objPref.Food_Advert = "B";
                objPref.SweetSaltySnacks_Advert = "B";
                objPref.AlcoholicDrinks_Advert = "B";
                objPref.NonAlcoholicDrinks_Advert = "B";
                objPref.Householdproducts_Advert = "B";
                objPref.ToiletriesCosmetics_Advert = "B";
                objPref.PharmaceuticalChemistsProducts_Advert = "B";
                objPref.TobaccoProducts_Advert = "B";
                objPref.PetsPetFood_Advert = "B";
                objPref.ShoppingRetailClothing_Advert = "B";
                objPref.DIYGardening_Advert = "B";
                objPref.ElectronicsOtherPersonalItems_Advert = "B";
                objPref.CommunicationsInternet_Advert = "B";
                objPref.FinancialServices_Advert = "B";
                objPref.HolidaysTravel_Advert = "B";
                objPref.SportsLeisure_Advert = "B";
                objPref.Motoring_Advert = "B";
                objPref.Newspapers_Advert = "B";
                objPref.TV_Advert = "B";
                objPref.Cinema_Advert = "B";
                objPref.SocialNetworking_Advert = "B";
                objPref.Shopping_Advert = "B";
                objPref.Fitness_Advert = "B";
                objPref.Environment_Advert = "B";
                objPref.GoingOut_Advert = "B";
                objPref.Religion_Advert = "B";
                objPref.Music_Advert = "B";
                objPref.BusinessOrOpportunities_AdType = "B";
                objPref.Gambling_AdType = "B";
                objPref.Restaurants_AdType = "B";
                objPref.Insurance_AdType = "B";
                objPref.Furniture_AdType = "B";
                objPref.InformationTechnology_AdType = "B";
                objPref.Energy_AdType = "B";
                objPref.Supermarkets_AdType = "B";
                objPref.Healthcare_AdType = "B";
                objPref.JobsAndEducation_AdType = "B";
                objPref.Gifts_AdType = "B";
                objPref.AdvocacyOrLegal_AdType = "B";
                objPref.DatingAndPersonal_AdType = "B";
                objPref.RealEstate_AdType = "B";
                objPref.Games_AdType = "B";
                objPref.Hustlers_AdType = "A";
                objPref.Youth_AdType = "A";
                objPref.DiscerningProfessionals_AdType = "A";
                objPref.Mass_AdType = "A";
                objPref.ContractType_Mobile = "A";
                objPref.Spend_Mobile = "A";

                db.UserProfilePreference.Add(objPref);
                db.SaveChanges();
                #endregion

                UserMatchTableProcess obj = new UserMatchTableProcess();
                var operatorId = _userRepository.GetById(userId).OperatorId;
                //obj.AddUserMatchData(userMatchTableName, profilePref, userData, db);
                //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                //PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userMatchTableName, conn);

                var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        db = new EFMVCDataContex(item);
                        using (db)
                        {
                            var userData = db.Users.Where(s => s.AdtoneServerUserId == userId).FirstOrDefault();

                            var profilePref2 = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == profilePref.UserProfileId).FirstOrDefault();
                            if (profilePref2 != null)
                            {
                                #region External UserProfilePreference
                                UserProfilePreference objUserPref = new UserProfilePreference();
                                var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, profilePref.UserProfileId);
                                objUserPref.UserProfileId = externalServerUserProfileId;
                                objUserPref.Gender_Demographics = "C";
                                objUserPref.WorkingStatus_Demographics = "I";
                                objUserPref.RelationshipStatus_Demographics = "G";
                                objUserPref.Education_Demographics = "E";
                                objUserPref.HouseholdStatus_Demographics = "D";
                                objUserPref.Location_Demographics = "A";
                                objUserPref.Food_Advert = "B";
                                objUserPref.SweetSaltySnacks_Advert = "B";
                                objUserPref.AlcoholicDrinks_Advert = "B";
                                objUserPref.NonAlcoholicDrinks_Advert = "B";
                                objUserPref.Householdproducts_Advert = "B";
                                objUserPref.ToiletriesCosmetics_Advert = "B";
                                objUserPref.PharmaceuticalChemistsProducts_Advert = "B";
                                objUserPref.TobaccoProducts_Advert = "B";
                                objUserPref.PetsPetFood_Advert = "B";
                                objUserPref.ShoppingRetailClothing_Advert = "B";
                                objUserPref.DIYGardening_Advert = "B";
                                objUserPref.ElectronicsOtherPersonalItems_Advert = "B";
                                objUserPref.CommunicationsInternet_Advert = "B";
                                objUserPref.FinancialServices_Advert = "B";
                                objUserPref.HolidaysTravel_Advert = "B";
                                objUserPref.SportsLeisure_Advert = "B";
                                objUserPref.Motoring_Advert = "B";
                                objUserPref.Newspapers_Advert = "B";
                                objUserPref.TV_Advert = "B";
                                objUserPref.Cinema_Advert = "B";
                                objUserPref.SocialNetworking_Advert = "B";
                                objUserPref.Shopping_Advert = "B";
                                objUserPref.Fitness_Advert = "B";
                                objUserPref.Environment_Advert = "B";
                                objUserPref.GoingOut_Advert = "B";
                                objUserPref.Religion_Advert = "B";
                                objUserPref.Music_Advert = "B";
                                objUserPref.BusinessOrOpportunities_AdType = "B";
                                objUserPref.Gambling_AdType = "B";
                                objUserPref.Restaurants_AdType = "B";
                                objUserPref.Insurance_AdType = "B";
                                objUserPref.Furniture_AdType = "B";
                                objUserPref.InformationTechnology_AdType = "B";
                                objUserPref.Energy_AdType = "B";
                                objUserPref.Supermarkets_AdType = "B";
                                objUserPref.Healthcare_AdType = "B";
                                objUserPref.JobsAndEducation_AdType = "B";
                                objUserPref.Gifts_AdType = "B";
                                objUserPref.AdvocacyOrLegal_AdType = "B";
                                objUserPref.DatingAndPersonal_AdType = "B";
                                objUserPref.RealEstate_AdType = "B";
                                objUserPref.Games_AdType = "B";
                                objUserPref.Hustlers_AdType = "A";
                                objUserPref.Youth_AdType = "A";
                                objUserPref.DiscerningProfessionals_AdType = "A";
                                objUserPref.Mass_AdType = "A";
                                objUserPref.ContractType_Mobile = "A";
                                objUserPref.Spend_Mobile = "A";

                                db.UserProfilePreference.Add(objUserPref);
                                db.SaveChanges();
                                #endregion
                                obj.AddUserMatchData(userData.UserMatchTableName, profilePref2, userData, db);
                                PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                            }

                        }
                    }
                }

            }

        }

        [Route("GetCopyRight")]
        [HttpGet]
        public ActionResult GetCopyRight()
        {
            string copyRightText = _copyRightRepository.GetAll().LastOrDefault().CopyRightText;
            return Json(copyRightText, JsonRequestBehavior.AllowGet);
        }

        [Route("Keepalive")]
        public ActionResult Keepalive()
        {
            UpdateSessionTimeOut updateSessionTimeOut = new UpdateSessionTimeOut(formAuthentication, _userRepository);
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            updateSessionTimeOut.SessionTimeout(efmvcUser);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [Route("LogOff")]
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Login", new { area = "Users" });
        }

        //Add 05-07-2019
        #region ForgotPassword

        [Route("ForgotPassword")]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            return View(forgotPassword);
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.Get(u => u.Email == model.Email && u.RoleId == 2);
                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed.";
                    return View("ForgotPassword", model);
                }
                else if (user.VerificationStatus == false)
                {
                    TempData["error"] = "Please verify your email account.";
                    return View("ForgotPassword", model);
                }
                else if (user.Activated == 0)
                {
                    TempData["error"] = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return View("ForgotPassword", model);
                }
                else if (user.Activated == 2)
                {
                    TempData["error"] = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return View("ForgotPassword", model);
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                string email = EncryptionHelper.EncryptSingleValue(user.Email);
                string url = string.Format("{0}?activationCode={1}",
                                           ConfigurationManager.AppSettings["AdminResetPassword"], email);

                var reader =
                    new StreamReader(
                        Server.MapPath(ConfigurationManager.AppSettings["ResetPasswordEmailTemplate"]));
                string emailContent = reader.ReadToEnd();
                emailContent = string.Format(emailContent, url);

                MailMessage mail = new MailMessage();
                mail.To.Add(user.Email);
                //mail.To.Add("xxx@gmail.com");
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                mail.Subject = "Email Verification";

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

                return View("ForgotPasswordConfirmation");
            }

            return View("ForgotPasswordConfirmation");

        }

        [Route("ForgotPasswordConfirmation")]
        [HttpGet]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [Route("ResetPassword")]
        [HttpGet]
        public ActionResult ResetPassword()
        {
            try
            {
                TempData["ResetPassword"] = null;
                string email = Request.QueryString["activationCode"];
                email = EncryptionHelper.DecryptSingleValue(email);

                User user = _userRepository.Get(x => x.Email == email && x.RoleId == 2);

                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed.";
                    return View("ResetPassword");
                }
                else if (user.VerificationStatus == false)
                {
                    TempData["error"] = "Please verify your email account.";
                    return View("ResetPassword");
                }
                else if (user.Activated == 0)
                {
                    TempData["error"] = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword");
                }
                else if (user.Activated == 2)
                {
                    TempData["error"] = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return View("ResetPassword");
            }

            return View();
        }

        [Route("ResetPassword")]
        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordConfirmation model)
        {
            try
            {
                User user = _userRepository.Get(x => x.Email == model.Email && x.RoleId == 2);

                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed";
                    return View("ResetPassword", model);
                }
                else if (user.VerificationStatus == false)
                {
                    TempData["error"] = "Please verify your email account.";
                    return View("ResetPassword", model);
                }
                else if (user.Activated == 0)
                {
                    TempData["error"] = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword", model);
                }
                else if (user.Activated == 2)
                {
                    TempData["error"] = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword", model);
                }
                var command = new ResetPasswordCommand
                {
                    UserId = user.UserId,
                    NewPassword = model.NewPassword
                };
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["ResetPassword"] = "1";
                }
                else
                {
                    TempData["error"] = "Internal server error.Please try again.";
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.InnerException.Message;
            }
            return View();
        }

        #endregion

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

        public void SensEmailToUser(string email, string password)
        {
            if (!(string.IsNullOrEmpty(email)))
            {
                var reader = new StreamReader(Server.MapPath(ConfigurationManager.AppSettings["USSDUsersEmailTemplate"]));
                string emailContent = reader.ReadToEnd();

                var siteAddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                var userLoginUrl = ConfigurationManager.AppSettings["userLoginURL"].ToString();
                var url = siteAddress + userLoginUrl;
                Random generator = new Random();
                string verifyCode = password;
                emailContent = string.Format(emailContent, url, email, password);

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                mail.Subject = "User Registration";
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
        }

        public bool IsValidEmail(string email)
        {
            var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            return !string.IsNullOrEmpty(email) && r.IsMatch(email);
        }

        public string PermissiveSubstring(string input, int startIndex, int length)
        {
            var output = input.Substring(startIndex, input.Length - startIndex <= length ? input.Length - startIndex : length);
            return output;
        }

        public string Left(string input, int length)
        {
            string output = PermissiveSubstring(input, 0, length);
            return output;
        }
    }
}