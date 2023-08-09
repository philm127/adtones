// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-15-2013
// ***********************************************************************
// <copyright file="AccountController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;

/// <summary>
/// The Controllers namespace.
/// </summary>

namespace EFMVC.Web.Controllers
{
    /// <summary>
    /// Class AccountController.
    /// </summary>
    public class Account1Controller : Controller
    {
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The command bus
        /// </summary>
        private readonly ICommandBus commandBus;

        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        /// <summary>
        /// Initializes a new instance of the <see cref="Account1Controller"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="formAuthentication">The form authentication.</param>
        public Account1Controller(ICommandBus commandBus, IUserRepository userRepository,
                                 IProfileRepository profileRepository, IFormsAuthentication formAuthentication)
        {
            this.commandBus = commandBus;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            this.formAuthentication = formAuthentication;
        }

        //
        // GET: /Account/LogOff

        /// <summary>
        /// Logs the off.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Registers the advertiser.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RegisterAdvertiser()
        {
            if (!SiteHelper.Instance.IsAdvertiserUrl)
                return View("Register");

            return View(new UserFormModel());
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Register()
        {
            if (SiteHelper.Instance.IsAdvertiserUrl)
                return View("RegisterAdvertiser");

            return ContextDependentView();
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

        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return ContextDependentView();
        }

        /// <summary>
        /// Logins the specified form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Login(LogOnFormModel form, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.Get(u => u.Email == form.UserName);

                if (user != null)
                {
                    if (SiteHelper.Instance.IsAdvertiserUrl)
                    {
                        if (user.RoleId != 1 && user.RoleId != 3)
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            return View("Login", form);
                        }
                    }
                    else if (!SiteHelper.Instance.IsAdvertiserUrl)
                    {
                        if (!user.Activated)
                        {
                            ModelState.AddModelError("", "This account has not been activated. Please reply to the text message sent to the registered mobile phone number during signup.");
                            return View("Login", form);
                        }

                        if (user.RoleId != 1 && user.RoleId != 2)
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            return View("Login", form);
                        }
                    }
                }

                if (user != null)
                {
                    if (ValidatePassword(user, form.Password))
                    {
                        formAuthentication.SetAuthCookie(HttpContext,
                            UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                user));

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed
            return View("Login", form);
        }

        /// <summary>
        /// Jsons the login.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult JsonLogin(LogOnFormModel form, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.Get(u => u.Email == form.UserName && u.Activated);

                if (SiteHelper.Instance.IsAdvertiserUrl)
                {
                    if (user.RoleId != 3)
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

                if (user != null)
                {
                    if (ValidatePassword(user, form.Password))
                    {
                        formAuthentication.SetAuthCookie(HttpContext,
                                                         UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                             user));

                        return Json(new {success = true, redirect = returnUrl});
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed
            return Json(new {errors = GetErrorsFromModelState(), success = false});
        }

        //
        // POST: /Account/JsonRegister

        /// <summary>
        /// Jsons the register.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(UserFormModel form)
        {
            if (ModelState.IsValid)
            {
                UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
                command.Activated = true;
                command.RoleId = (Int32) UserRoles.User;
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    ICommandResult result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        User user = _userRepository.Get(u => u.Email == form.Email);
                        formAuthentication.SetAuthCookie(HttpContext,
                                                         UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                             user));
                        return Json(new {success = true});
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unknown error occurred.");
                    }
                }
                // If we got this far, something failed
                return Json(new {errors = GetErrorsFromModelState()});
            }

            // If we got this far, something failed
            return Json(new {errors = GetErrorsFromModelState()});
        }

        /// <summary>
        /// Registers the specified form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(UserFormModel form)
        {
            if (ModelState.IsValid)
            {
                UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
                command.Activated = false;
                command.RoleId = (Int32) UserRoles.User;
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    ICommandResult result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        User user = _userRepository.Get(u => u.Email == form.Email);
                        //                        formAuthentication.SetAuthCookie(this.HttpContext,
                        //                                                          UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                        //                                                              user));

                        SendVerificationSms(form.MSISDN);

                        return RedirectToAction("RegisterSuccess", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unknown error occurred.");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(form);
            }

            // If we got this far, something failed
            return Json(new {errors = GetErrorsFromModelState()});
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        private void SendVerificationSms(string mobileNumber)
        {
            if (mobileNumber.StartsWith("0"))
            {
                mobileNumber = mobileNumber.Substring(1);
                mobileNumber = string.Format("44{0}", mobileNumber);
            }

            string url = string.Format(ConfigurationManager.AppSettings["SmsVerificationUrl"],
                                       ConfigurationManager.AppSettings["SmsVerificationUsername"],
                                       ConfigurationManager.AppSettings["SmsVerificationPassword"], mobileNumber,
                                       ConfigurationManager.AppSettings["SmsVerificationOriginator"],
                                       ConfigurationManager.AppSettings["SmsVerificationMessage"]);

            var webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Method = "POST";

            var webResponse = (HttpWebResponse) webRequest.GetResponse();

            string responseData;
            using (var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                // dumps the HTML from the response into a string variable
                responseData = responseReader.ReadToEnd();
            }

            Console.WriteLine(responseData);
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
        /// Contexts the dependent view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordFormModel form)
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
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    ICommandResult result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(form);
        }

        /// <summary>
        /// Changes the password success.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
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
            client.Send(mail);
        }
        /// <summary>
        /// Registers the advertiser.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterAdvertiser(UserFormModel form)
        {
            if (ModelState.IsValid)
            {
                UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
                command.Activated = false;
                command.RoleId = (Int32) UserRoles.Advertiser;
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    ICommandResult result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        User user = _userRepository.Get(u => u.Email == form.Email);
                        string email = EncryptionHelper.EncryptSingleValue(user.Email);
                        string url = string.Format("{0}?activationCode={1}",
                                                   ConfigurationManager.AppSettings["AdvertiserVerificationUrl"], email);

                        var reader =
                            new StreamReader(
                                Server.MapPath(ConfigurationManager.AppSettings["AdvertiserVerificationEmailTemplate"]));
                        string emailContent = reader.ReadToEnd();
                        emailContent = string.Format(emailContent, url);
                        MailSending("testing.chaka@gmail.com", "this1admin", "Registration", user.Email, emailContent, "smtp.gmail.com", 587, true);
                        //MailHelper.SendMail(ConfigurationManager.AppSettings["SiteEmailAddress"], user.Email, null, null,
                        //                    "Email Verification", emailContent,
                        //                    ConfigurationManager.AppSettings["SmtpServerAddress"],
                        //                    int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]));

                        return RedirectToAction("RegisterAdvertiserSuccess", "Account");
                    }

                    ModelState.AddModelError("", "An unknown error occurred.");
                }
                // If we got this far, something failed, redisplay form
                return View(form);
            }

            // If we got this far, something failed
            return Json(new {errors = GetErrorsFromModelState()});
        }

        /// <summary>
        /// Registers the advertiser success.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RegisterAdvertiserSuccess()
        {
            return View();
        }

        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            return View();
        }

        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangeEmail(ChangeEmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var command = new ChangeEmailCommand
                                  {
                                      Email = model.Email,
                                      UserId = efmvcUser.UserId
                                  };

                ICommandResult commandResult = commandBus.Submit(command);

                if (commandResult.Success)
                {
                    return RedirectToAction("ChangeEmailSuccess");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Changes the email success.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangeEmailSuccess()
        {
            return View();
        }

        public ActionResult Verify(string mobileNumber)
        {
            try
            {
                using (
                    var writer = new StreamWriter(ConfigurationManager.AppSettings["SmsVerificationInboundLog"], true))
                {
                    if (mobileNumber.StartsWith("44"))
                        mobileNumber = mobileNumber.Substring(2);

                    if (!mobileNumber.StartsWith("0"))
                        mobileNumber = "0" + mobileNumber;

                    UserProfile userProfile = _profileRepository.Get(x => x.MSISDN == mobileNumber);

                    if (userProfile != null && userProfile.UserProfileId != 0)
                    {
                        var command = new ChangeActiveStatusCommand {UserId = userProfile.UserId, Activated = true};

                        ICommandResult commandResult = commandBus.Submit(command);
                        writer.WriteLine(commandResult.Success ? "Success: {0}" : "Failure: {0}", mobileNumber);
                    }
                    else
                    {
                        writer.WriteLine("Failure: {0}", mobileNumber);
                    }
                }
            }
            catch (Exception e)
            {
                using (
                    var writer = new StreamWriter(ConfigurationManager.AppSettings["SmsVerificationInboundLog"], true))
                {
                    writer.WriteLine("Failure with error: {0} - {1}", mobileNumber, e.Message);
                    writer.Close();
                }
            }

            return View();
        }

        public ActionResult VerifyAdvertiser()
        {
            try
            {
                string email = Request.QueryString["activationCode"];
                email = EncryptionHelper.DecryptSingleValue(email);

                User user = _userRepository.Get(x => x.Email == email);

                if (user != null && user.UserId != 0)
                {
                    var command = new ChangeVerificationStatusCommand {UserId = user.UserId, VerificationStatus = true};

                    ICommandResult commandResult = commandBus.Submit(command);

                    ViewData.Add("VerifyAdvertiserResult", "success");
                }
                else
                {
                    ViewData.Add("VerifyAdvertiserResult", "failed");
                }
            }
            catch (Exception)
            {
                ViewData.Add("VerifyAdvertiserResult", "failed");
            }

            return View();
        }
    }
}