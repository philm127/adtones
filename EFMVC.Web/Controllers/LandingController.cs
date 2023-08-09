using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using EFMVC.CommandProcessor.Dispatcher;
using System.Web;
using System.Web.Security;
using RestSharp;
using Newtonsoft.Json;
using EFMVC.Data;
using EFMVC.Web.Models;
using System.IO;
using System.Net.Mail;
using EFMVC.Domain.Commands;
using AutoMapper;
using EFMVC.Web.Core.Models;
using EFMVC.Model.Entities;
using EFMVC.Web.Common;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.CommandProcessor.Command;
using EFMVC.Web.Core.Extensions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using log4net.Repository.Hierarchy;
using log4net;
using System.Reflection;

namespace EFMVC.Web.Controllers
{
    public class LandingController : Controller
    {

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        /// <summary>
        /// The _copyRight Repository
        /// </summary>
        private readonly ICopyRightRepository _copyRightRepository;

        private readonly IContactsRepository _contactsRepository;

        // static string Email = null;
        // static string FirstName = null;
        //static string LastName = null;
        // static string Organisation = null;
        // static int? OrganisationTypeId = null;
        // static string CountryCode = null;
        // static string PhoneNumber = null;

        //static string UserToken = null;
        //static string ConnectionToken = null;
        //static string Provider = null;

        //static string strCode1 = null;
        //static string strCode2 = null;
        //static string strCode3 = null;

        public LandingController(ICommandBus commandBus, IUserRepository userRepository, IFormsAuthentication formAuthentication, ICountryRepository countryRepository, ICurrencyRepository currencyRepository, ICopyRightRepository copyRightRepository, IContactsRepository contactsRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            this.formAuthentication = formAuthentication;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _copyRightRepository = copyRightRepository;
            _contactsRepository = contactsRepository;
        }
        //
        // GET: /Landing/

        public ActionResult Index()
        {
            string querystring = Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrEmpty(querystring))
            {
                if (querystring.Contains("/Admin/"))
                {
                    return RedirectToAction("Index", "Login", new { Area = "Admin" });
                }
                else if (querystring.Contains("/UsersAdmin/"))
                {
                    return RedirectToAction("Index", "Login", new { Area = "Admin" });
                }
                else if (querystring.Contains("/Users/"))
                {
                    return RedirectToAction("Index", "Login", new { Area = "Users" });
                }
                else if (querystring.Contains("/AdvertAdmin/"))
                {
                    return RedirectToAction("Index", "Login", new { Area = "Admin" });
                }
                else if (querystring.Contains("/OperatorAdmin/"))
                {
                    return RedirectToAction("Index", "Login", new { Area = "Admin" });
                }
            }

            var url = Request.UrlReferrer;
            if (url != null)
            {
                var urlValue = url.ToString();
                //if (urlValue.Contains("https://adtones.api.oneall.com/socialize/redirect.html"))
                if(urlValue.Contains("//adtones.api.oneall.com/") && (Request.Form.AllKeys.Where(x => x.ToLower().Equals("oa_social_login_token") ).FirstOrDefault() != null  && !String.IsNullOrWhiteSpace(Request.Form["oa_social_login_token"])))
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
                        var desearlizeObj = JsonConvert.DeserializeObject<Areas.Users.Models.SocialLoginModel.RootObject>(content);

                        var userToken = desearlizeObj.response.result.data.user.user_token;
                        var provider = desearlizeObj.response.result.data.user.identity.provider;
                        EFMVCDataContex db = new EFMVCDataContex();
                        var userTokenData = db.UserTokenLink.Where(s => s.UserToken == userToken).FirstOrDefault();
                        if (userTokenData == null)
                        {
                            string email = null;
                            var emailData = desearlizeObj.response.result.data.user.identity.emails;
                            if (emailData != null)
                            {
                                email = emailData.FirstOrDefault().value;
                                var userData = _userRepository.GetMany(top => top.Email.ToLower() == email.ToLower()).FirstOrDefault();
                                if (userData != null)
                                {
                                    //Comment 18-04-2019
                                    //ViewBag.Error = "The email is already exists.";

                                    ViewBag.Error = email + " Email Address is already exists in database so please choose another one.";
                                    LogOnFormModel loginModel = new LogOnFormModel();
                                    return View(loginModel);
                                }
                            }

                            AdvertiserSocialLoginModel model = new AdvertiserSocialLoginModel();

                            Session["AdvertUserToken"] = userToken;
                            Session["AdvertProvider"] = provider;

                            FillOrganisationType(db);
                            FillCountry(db);
                            //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                            model.Email = email;
                            return View("SocialLogin", model);
                        }
                        else
                        {
                            var userId = userTokenData.UserId;
                            User user = _userRepository.Get(u => u.UserId == userId && u.RoleId == 3);
                            if (user != null)
                            {
                                if (user.Activated != 1)
                                {
                                    var errorMsg = user.Activated == 0 ? "Your account is not approved by adtones administrator so please contact adtones admin."
                                             : user.Activated == 2 ? "Your account is  suspended by adtones administrator so please contact adtones admin."
                                             : user.Activated == 3 ? "Your account is  deleted by adtones administrator so please contact adtones admin."
                                             : "Something went wrong! please try again";
                                    ViewBag.Error = errorMsg;
                                    LogOnFormModel model = new LogOnFormModel();
                                    return View(model);
                                }
                                else
                                {
                                    formAuthentication.SetAuthCookie(HttpContext,
                                       UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                           user));
                                    var urls = formAuthentication.getRedirectionURL(user.Email, true).Remove(0, 1);
                                    var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                                    return Redirect(siteaddress + urls);
                                }
                            }
                            else
                            {
                                ViewBag.Error = "The email is already exists for different user role.";
                                LogOnFormModel loginModel = new LogOnFormModel();
                                return View(loginModel);
                            }


                        }
                    }

                }
            }
            LogOnFormModel logOnFormModel = new LogOnFormModel();

            if (Request.Cookies["UserName"] != null)
            {
                logOnFormModel.UserName = Request.Cookies["UserName"].Value;
                logOnFormModel.Password = Request.Cookies["Password"].Value;
            }
            else
            {
                logOnFormModel.UserName = "";
                logOnFormModel.Password = "";
            }


            return View(logOnFormModel);
        }





        private void FillOrganisationType(EFMVCDataContex db)
        {
            var typeList = db.OrganisationType.Select(top => new
            {
                Name = top.Type,
                Id = top.OrganisationTypeId
            }).ToList();
            ViewBag.OrgType = new MultiSelectList(typeList, "Id", "Name");

        }

        private void FillCountry(EFMVCDataContex db)
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name + " " + top.CountryCode,
                Id = top.Id
            }).OrderBy(s => s.Name).ToList();
            ViewBag.Country = new MultiSelectList(countrydetails, "Id", "Name");

        }



        public ActionResult Index2()
        {
            return View();
        }


        /// <param name="returnUrl">The return URL.</param>
        [ValidateInput(false)]
        public JsonResult JsonLogin(LogOnFormModel form, string returnUrl)
        {
            try
            {


                int usererror = 0;
                if (ModelState.IsValid)
                {
                    User user = _userRepository.Get(u => u.Email.ToLower() == form.UserName.ToLower() && u.Activated != 3);
                    if (user != null)
                    {
                        if (user.VerificationStatus == false)
                        {
                            ModelState.AddModelError("", "Please verify your email account.");
                            usererror = -1;
                        }
                        else if (user.Activated == 0)
                        {
                            ModelState.AddModelError("", "Your account is not approved by adtones administrator so please contact adtones admin.");
                            usererror = -1;
                        }
                        else if (user.Activated == 2)
                        {
                            ModelState.AddModelError("", "Your account is  suspended by adtones administrator so please contact adtones admin.");
                            usererror = -1;
                        }
                    }
                    else
                    {
                        User userdeleted = _userRepository.Get(u => u.Email == form.UserName && u.Activated == 3);
                        if (userdeleted != null)
                        {
                            ModelState.AddModelError("", "Your account is  deleted by adtones administrator so please contact adtones admin.");
                            usererror = -1;
                        }
                        if (user == null)
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            usererror = -1;
                        }
                    }
                    if (usererror == 0)
                    {
                        if (Request.Cookies["UserId"] != null)
                        {
                            if (Request.Cookies["UserName"].Value.ToString() != null)
                            {
                                if (Request.Cookies["UserName"].Value.ToString() == form.UserName)
                                {
                                    var username = Request.Cookies["UserName"].Value.ToString();
                                    user = _userRepository.Get(u => u.Email == username && u.Activated != 3);
                                }
                                else
                                {
                                    user = _userRepository.Get(u => u.Email == form.UserName && u.Activated != 3);
                                }
                            }
                            else
                            {
                                user = _userRepository.Get(u => u.Email == form.UserName && u.Activated != 3);
                            }

                            if (Request.Cookies["Password"].Value.ToString() == form.Password)
                            {
                                if (ValidatePassword(user, form.Password))
                                {
                                    formAuthentication.SetAuthCookie(HttpContext,
                                    UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                        user));
                                    if (Url.IsLocalUrl(returnUrl))
                                    {
                                        return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + returnUrl }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        var urls = string.Empty;
                                        if (user.RoleId == 3)
                                        {
                                            //urls = formAuthentication.getRedirectionURL(Request.Cookies["UserName"].Value, true).Remove(0, 1);
                                            //return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls }, JsonRequestBehavior.AllowGet);
                                            urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                                            var redirecturl = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls;
                                            return Json(new { success = true, redirect = redirecturl }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                        }
                                    }
                                }
                            }
                            //else if (ValidatePassword(user, Request.Cookies["Password"].Value))
                            //{
                            //    formAuthentication.SetAuthCookie(HttpContext,
                            //    UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                            //        user));
                            //    if (Url.IsLocalUrl(returnUrl))
                            //    {
                            //        return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + returnUrl }, JsonRequestBehavior.AllowGet);
                            //    }
                            //    else
                            //    {
                            //        var urls = string.Empty;
                            //        if (user.RoleId == 3)
                            //        {
                            //            //urls = formAuthentication.getRedirectionURL(Request.Cookies["UserName"].Value, true).Remove(0, 1);
                            //            //return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls }, JsonRequestBehavior.AllowGet);
                            //            urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                            //            var redirecturl = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls;
                            //            return Json(new { success = true, redirect = redirecturl }, JsonRequestBehavior.AllowGet);
                            //        }
                            //        else
                            //        {
                            //            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            //        }
                            //    }
                            //}
                            else if (ValidatePassword(user, form.Password))
                            {
                                UserInfo userInfo = UserAuthenticationTicketBuilder.CreateUserContextFromUser(user);

                                var authTicket = new FormsAuthenticationTicket(
                                                                                1,
                                                                                user.FirstName,  //user id
                                                                                DateTime.Now,
                                                                                DateTime.Now.AddMinutes(20),  // expiry
                                                                                form.RememberMe,  //true to remember
                                                                                                  //false, //roles 
                                                                                userInfo.ToString()
                                                                               );
                                System.Web.HttpCookie cookie;
                                cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                                //encrypt the ticket and add it to a cookie
                                if (form.RememberMe == true)
                                {
                                    Response.Cookies["UserId"].Value = user.UserId.ToString();
                                    Response.Cookies["UserName"].Value = user.Email;
                                    Response.Cookies["Password"].Value = form.Password;
                                    cookie.Expires = authTicket.Expiration;
                                    cookie.Path = FormsAuthentication.FormsCookiePath;
                                    Response.Cookies.Add(cookie);
                                }
                                else
                                {
                                    formAuthentication.SetAuthCookie(HttpContext,
                                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                            user));
                                }

                                if (Url.IsLocalUrl(returnUrl))
                                {
                                    return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + returnUrl }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    var urls = string.Empty;
                                    if (user.RoleId == 3)
                                    {
                                        //urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                                        //return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls }, JsonRequestBehavior.AllowGet);
                                        urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                                        var redirecturl = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls;
                                        return Json(new { success = true, redirect = redirecturl }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            }
                        }
                        else if (ValidatePassword(user, form.Password))
                        {
                            UserInfo userInfo = UserAuthenticationTicketBuilder.CreateUserContextFromUser(user);

                            var authTicket = new FormsAuthenticationTicket(
                                                                            1,
                                                                            user.FirstName,  //user id
                                                                            DateTime.Now,
                                                                            DateTime.Now.AddMinutes(20),  // expiry
                                                                            form.RememberMe,  //true to remember
                                                                                              //false, //roles 
                                                                            userInfo.ToString()
                                                                           );
                            System.Web.HttpCookie cookie;
                            cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                            //encrypt the ticket and add it to a cookie
                            if (form.RememberMe == true)
                            {
                                Response.Cookies["UserId"].Value = user.UserId.ToString();
                                Response.Cookies["UserName"].Value = user.Email;
                                Response.Cookies["Password"].Value = form.Password;
                                cookie.Expires = authTicket.Expiration;
                                cookie.Path = FormsAuthentication.FormsCookiePath;
                                Response.Cookies.Add(cookie);
                            }
                            else
                            {
                                formAuthentication.SetAuthCookie(HttpContext,
                                        UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                            user));
                            }

                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + returnUrl }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var urls = string.Empty;
                                if (user.RoleId == 3 || user.RoleId == 9)
                                {
                                    //urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                                    //return Json(new { success = true, redirect = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls }, JsonRequestBehavior.AllowGet);
                                    urls = formAuthentication.getRedirectionURL(form.UserName, true).Remove(0, 1);
                                    var redirecturl = ConfigurationManager.AppSettings["siteAddress"].ToString() + urls;
                                    return Json(new { success = true, redirect = redirecturl }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                    }
                }

                // If we got this far, something failed
                return Json(new { success = false, errors = GetErrorsFromModelState() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //
        // GET: /Account/LogOff

        /// <summary>
        /// Logs the off.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("LogOff")]
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Landing");
        }

        /// <summary>
        /// Gets the state of the errors from model.
        /// </summary>
        /// <returns>IEnumerable{System.String}.</returns>
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidatePassword(User user, string password)
        {
            string encoded = Md5Encrypt.Md5EncryptPassword(password);
            return user.PasswordHash.Equals(encoded);
        }
        [HttpPost]
        public JsonResult Checkinternetspeed()
        {
            double[] speeds = new double[5];
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    int jQueryFileSize = 202; //Size of File in KB.
                    WebClient client = new WebClient();
                    DateTime startTime = DateTime.Now;
                    var url = ConfigurationManager.AppSettings["downloadtestfile"].ToString();
                    client.DownloadFile(url, Server.MapPath("~/downloadtest/upload/img-01.jpg"));
                    DateTime endTime = DateTime.Now;
                    var checkvalue = (endTime - startTime).TotalSeconds;
                    //  var checkvalue = 0;
                    if (checkvalue != 0)
                        speeds[i] = Math.Round((jQueryFileSize / (endTime - startTime).TotalSeconds));
                    else
                        speeds[i] = Math.Round((jQueryFileSize / 0.82936239999999994));
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
                //return Json(28811);
            }
            return Json(speeds.Average());
        }

        // Social Login Registartion Page - First Call
        public ActionResult SocialLogin()
        {
            EFMVCDataContex db = new EFMVCDataContex();
            FillOrganisationType(db);
            AdvertiserSocialLoginModel model = new AdvertiserSocialLoginModel();
            FillCountry(db);
            //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
            return View(model);
        }

        // Social Login EmailVerification Page - Second Call
        public ActionResult EmailVerification(AdvertiserSocialLoginModel model)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();

                //Add 18-04-2019
                var isMobileExist = _contactsRepository.GetMany(s => s.MobileNumber == model.PhoneNumber).Any();
                if (isMobileExist)
                {
                    TempData["Error"] = model.PhoneNumber + " Phone Number already exist in database.Please choose another one.";
                    FillOrganisationType(db);
                    FillCountry(db);
                    //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                    return View("SocialLogin");
                }

                //var isEmailExist = _userRepository.GetMany(s => s.Email.ToLower() == model.Email.ToLower() && s.RoleId == 3 && s.Activated == 1).Any();
                var isEmailExist = _userRepository.GetMany(s => s.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                if (isEmailExist != null)
                {
                    //Add 18-04-2019
                    TempData["Error"] = model.Email + " Email address already exist in database.Please choose another one.";
                    FillOrganisationType(db);
                    FillCountry(db);
                    //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                    return View("SocialLogin");
                }
                if (isEmailExist != null)
                {
                    if (isEmailExist.Activated == 2)
                    {
                        TempData["Error"] = "Your account is suspended please contact adtones admin.";
                        FillOrganisationType(db);
                        FillCountry(db);
                        //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                        return View("SocialLogin");
                    }
                    else if (isEmailExist.Activated == 3)
                    {
                        TempData["Error"] = "Your account is deleted please contact adtones admin.";
                        FillOrganisationType(db);
                        FillCountry(db);
                        //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                        return View("SocialLogin");
                    }
                }


                if (!string.IsNullOrEmpty(model.CountryCode))
                {
                    var countryID = int.Parse(model.CountryCode);
                    var CountryCode = db.Country.Where(c => c.Id == countryID).Select(c => c.CountryCode).FirstOrDefault();
                    var firstCode = CountryCode.StartsWith("+");
                    if (firstCode == true)
                    {
                        Session["AdvertCountryCode"] = CountryCode.Remove(0, 1);
                    }
                }

                Session["AdvertEmail"] = model.Email;
                Session["AdvertFirstName"] = model.FirstName;
                Session["AdvertLastName"] = model.LastName;
                Session["AdvertOrganisation"] = model.Organisation;
                Session["AdvertOrganisationTypeId"] = model.OrganisationTypeId;
                // Session["AdvertCountryCode"] = model.CountryCode;
                Session["AdvertPhoneNumber"] = model.PhoneNumber;
                Session["OperatorId"] = model.opratorAdminId;
                ViewBag.email = model.Email;

                SendEmailVerificationCode(model.Email);
                return View();
            }
            catch(Exception ex)
            {
                //model.opratorAdminList = new List<SelectListItem> { new SelectListItem { Text = "-- Select Operator --", Value = "" } };
                return Redirect("SocialLogin");
            }

        }

        // Social Login Verification Code and User Registartion - Third Call
        public ActionResult VeifyEmailCode(string confirmCode)
        {

            string Email = null;
            string FirstName = null;
            string LastName = null;
            string Organisation = null;
            string CountryCode = null;
            int? OrganisationTypeId = 0;
            string PhoneNumber = null;
            string UserToken = null;
            string Provider = null;
            int OperatorId = 0;
            var countryId = 0;
            var currencyId = 0;

            try
            {
                var code = confirmCode.Split('-');

                //var code1 = strCode1;
                //var code2 = strCode2;
                //var code3 = strCode3;

                string code1 = null;
                string code2 = null;
                string code3 = null;


                if (Session["AdvertCode1"] != null && Session["AdvertCode2"] != null && Session["AdvertCode3"] != null)
                {
                    code1 = Session["AdvertCode1"].ToString();
                    code2 = Session["AdvertCode2"].ToString();
                    code3 = Session["AdvertCode3"].ToString();
                }

                if (Session["AdvertEmail"] != null)
                {
                    Email = Session["AdvertEmail"].ToString();
                }

                if (Session["AdvertFirstName"] != null)
                {
                    FirstName = Session["AdvertFirstName"].ToString();
                }

                if (Session["AdvertLastName"] != null)
                {
                    LastName = Session["AdvertLastName"].ToString();
                }

                if (Session["AdvertOrganisation"] != null)
                {
                    Organisation = Session["AdvertOrganisation"].ToString();
                }

                if (Session["AdvertOrganisationTypeId"] != null)
                {
                    OrganisationTypeId = (int)Session["AdvertOrganisationTypeId"];
                }

                if (Session["AdvertCountryCode"] != null)
                {
                    CountryCode = Session["AdvertCountryCode"].ToString();
                }

                if (Session["AdvertPhoneNumber"] != null)
                {
                    PhoneNumber = Session["AdvertPhoneNumber"].ToString();
                }

                if (Session["AdvertUserToken"] != null)
                {
                    UserToken = Session["AdvertUserToken"].ToString();
                }

                if (Session["AdvertProvider"] != null)
                {
                    Provider = Session["AdvertProvider"].ToString();
                }

                if (Session["OperatorId"] != null)
                {
                    OperatorId = Convert.ToInt32(Session["OperatorId"].ToString());
                }

                var receivedCode1 = code[0];
                var receivedCode2 = code[1];
                var receivedCode3 = code[2];

                if (code1 != null && code1 != null && code1 != null)
                {
                    if (code1.ToString() == receivedCode1 && code2.ToString() == receivedCode2 && code3.ToString() == receivedCode3)
                    {

                        EFMVCDataContex db = new EFMVCDataContex();
                        User userData = new User();
                        userData.Email = Email;
                        userData.FirstName = FirstName;
                        userData.LastName = LastName;
                        userData.PasswordHash = null;
                        userData.DateCreated = DateTime.Now;
                        userData.Organisation = Organisation;
                        userData.LastLoginTime = DateTime.Now;
                        userData.Outstandingdays = 0;
                        userData.Activated = 1;
                        userData.RoleId = 3;
                        userData.VerificationStatus = true;
                        userData.IsEmailVerfication = true;
                        //if (CountryCode == "44")
                        //    userData.OperatorId = 1;
                        //else if (CountryCode == "254")
                        //    userData.OperatorId = 2;
                        //else if (CountryCode == "221")
                        //    userData.OperatorId = 3;
                        //else if (CountryCode == "91")
                        //    userData.OperatorId = 4;
                        //else if (CountryCode == "39")
                        //    userData.OperatorId = 5;
                        //else if (CountryCode == "33")
                        //    userData.OperatorId = 6;
                        //else if (CountryCode == "45")
                        //    userData.OperatorId = 7;
                        //else
                        //    userData.OperatorId = 1;

                        userData.OperatorId = OperatorId;
                        if (Organisation != null)
                            userData.OrganisationTypeId = OrganisationTypeId == 0 ? null : OrganisationTypeId;
                        else
                            userData.OrganisationTypeId = null;

                        //if (!string.IsNullOrEmpty(PhoneNumber))
                        //    userData.AdvertiserPhoneNumber = CountryCode + PhoneNumber;
                        //else
                        //    userData.AdvertiserPhoneNumber = null;
                        userData.IsSessionFlag = false;
                        userData.LockOutTime = null;
                        userData.LastPasswordChangedDate = DateTime.Now;
                        db.Users.Add(userData);
                        db.SaveChanges();

                        if (!string.IsNullOrEmpty(userData.Organisation))
                        {
                            AddCompanyDetail(userData.Organisation, userData.UserId);
                        }

                        if (!string.IsNullOrEmpty(PhoneNumber))
                        {
                            //Commented 28-02-2019
                            //Contacts objContact = new Contacts();
                            //objContact.UserId = userData.UserId;
                            //objContact.Email = userData.Email;
                            //objContact.MobileNumber = CountryCode + PhoneNumber;
                            //var dbCountryCode = "+" + CountryCode;
                            //if (!string.IsNullOrEmpty(CountryCode))
                            //{
                            //    countryId = _countryRepository.GetAll().Where(top => top.CountryCode == dbCountryCode).Select(top => top.Id).FirstOrDefault();
                            //    objContact.CountryId = countryId;
                            //    currencyId = _currencyRepository.GetAll().Where(top => top.CountryId == countryId).Select(top => top.CurrencyId).FirstOrDefault();
                            //    if (currencyId != 0)
                            //    {
                            //        if (currencyId == 13)
                            //        {
                            //            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                            //            objContact.CurrencyId = currency.CurrencyId;
                            //        }
                            //        else
                            //        {
                            //            objContact.CurrencyId = currencyId;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                            //        objContact.CurrencyId = currency.CurrencyId;
                            //    }
                            //}
                            //else
                            //{
                            //    var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                            //    objContact.CurrencyId = currency.CurrencyId;
                            //}
                            //db.Contacts.Add(objContact);
                            //db.SaveChanges();

                            //Add 28-02-2019
                            Contacts objContact = new Contacts();
                            objContact.UserId = userData.UserId;
                            objContact.Email = userData.Email;
                            if (!string.IsNullOrEmpty(CountryCode))
                            {
                                objContact.MobileNumber = CountryCode + PhoneNumber;
                                var dbCountryCode = "+" + CountryCode;
                                countryId = _countryRepository.GetAll().Where(top => top.CountryCode == dbCountryCode).Select(top => top.Id).FirstOrDefault();
                                objContact.CountryId = countryId;
                                currencyId = _currencyRepository.GetAll().Where(top => top.CountryId == countryId).Select(top => top.CurrencyId).FirstOrDefault();
                                if (currencyId != 0)
                                {
                                    if (currencyId == 13)
                                    {
                                        var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                        objContact.CurrencyId = currency.CurrencyId;
                                    }
                                    else
                                    {
                                        objContact.CurrencyId = currencyId;
                                    }
                                }
                                else
                                {
                                    var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                                    objContact.CurrencyId = currency.CurrencyId;
                                }
                            }
                            else
                            {
                                var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                                objContact.CurrencyId = currency.CurrencyId;
                                objContact.CountryId = currency.CountryId;
                                objContact.MobileNumber = "1" + PhoneNumber;
                            }
                            db.Contacts.Add(objContact);
                            db.SaveChanges();

                        }
                        //else
                        //{
                        //    userData.AdvertiserPhoneNumber = null;
                        //}


                        UserTokenLink tokenLink = new UserTokenLink();
                        tokenLink.UserId = userData.UserId;
                        tokenLink.UserToken = UserToken;
                        tokenLink.Provider = Provider;
                        db.UserTokenLink.Add(tokenLink);
                        db.SaveChanges();

                        formAuthentication.SetAuthCookie(HttpContext,
                                UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                    userData));

                        Session["AdvertEmail"] = null;
                        Session["AdvertFirstName"] = null;
                        Session["AdvertLastName"] = null;
                        Session["AdvertOrganisation"] = null;
                        Session["AdvertOrganisationTypeId"] = null;
                        Session["AdvertCountryCode"] = null;
                        Session["AdvertPhoneNumber"] = null;
                        Session["AdvertUserToken"] = null;
                        Session["AdvertProvider"] = null;
                        Session["AdvertCode1"] = null;
                        Session["AdvertCode2"] = null;
                        Session["AdvertCode3"] = null;

                        return Json("Success", JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                // LiveAgent.CreateTicket("Advert registration error", ex.Message.ToString(), Session["AdvertEmail"].ToString());
                GenerateTicket(ex.Message.ToString(), Email, FirstName + " " + LastName);
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            // LiveAgent.CreateTicket("Advert registration error", "Something went wrong for advert registration", Session["AdvertEmail"].ToString());
            GenerateTicket("Something went wrong for advert registration", Email, FirstName + " " + LastName);
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }

        public void GenerateTicket(string message, string email, string userName)
        {

            string ticketCode = LiveAgent.CreateTicket("Advert registration error", message, Session["AdvertEmail"].ToString());
            QuestionFormModel _model = new QuestionFormModel();

            _model.UserId = null;
            _model.QNumber = ticketCode;
            _model.SubjectId = (int)QuestionSubjectStatus.Other;
            _model.ClientId = null;
            _model.CampaignProfileId = null;
            _model.PaymentMethodId = null;
            _model.Title = "Advert registration error";
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



        private void AddCompanyDetail(string organisation, int userId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            CompanyDetails comp = new CompanyDetails();
            comp.UserId = userId;
            comp.CompanyName = organisation;
            db.CompanyDetails.Add(comp);
            db.SaveChanges();
        }
        public ActionResult ResendEmailVerificationCode()
        {
            try
            {
                string email = null;
                if (Session["AdvertEmail"] != null)
                {
                    email = Session["AdvertEmail"].ToString();
                    SendEmailVerificationCode(email);
                }

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch
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

            Session["AdvertCode1"] = emailcode1;
            Session["AdvertCode2"] = emailcode2;
            Session["AdvertCode3"] = emailcode3;

            string verifyCode = emailcode1 + " " + emailcode2 + " " + emailcode3 + " ";

            emailContent = string.Format(emailContent, verifyCode);

            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
            mail.Subject = "Email Verification";
            mail.Body = emailContent.Replace("\n", "<br/>");
            //mail.Body = emailContent;

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

        [Route("GetCopyRight")]
        [HttpGet]
        public ActionResult GetCopyRight()
        {
           CopyRight copyright =  _copyRightRepository.GetAll().LastOrDefault();
           String copyRightText = String.Empty;
           
            if (copyright != null)
                copyRightText =_copyRightRepository.GetAll().LastOrDefault().CopyRightText;

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

        //[HttpPost]
        //public ActionResult FillOperatorAdmin(int countryID)
        //{
        //    EFMVCDataContex db = new EFMVCDataContex();
        //    var lst = db.Operator.Where(a => a.CountryId == countryID).Select(s => new SelectListItem { Text = s.OperatorName, Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
        //    return Json(lst, JsonRequestBehavior.AllowGet);

        //}

        public JsonResult MpesaDemo()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            var client = new RestClient("https://213.136.72.180:9877/ussd/stkpush");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\n  \"BusinessShortCode\": \"288520\",\n  \"Msisdn\": \"254710413687\",\n  \"Amount\": \"10\",\n  \"AccountReference\": \"Test\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return Json(response.StatusCode.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}
