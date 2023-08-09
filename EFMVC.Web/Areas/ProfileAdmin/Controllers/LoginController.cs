﻿using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.ProfileAdmin.Controllers
{
    [CompressResponse]
    [RouteArea("ProfileAdmin")]
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        /// <summary>
        /// The _copyRight Repository
        /// </summary>
        private readonly ICopyRightRepository _copyRightRepository;

        public LoginController(ICommandBus commandBus, IUserRepository userRepository, IFormsAuthentication formAuthentication, ICopyRightRepository copyRightRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            this.formAuthentication = formAuthentication;
            _copyRightRepository = copyRightRepository;
        }
        // GET: ProfileAdmin/Login
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("CheckLogin")]
        [HttpPost]
        public ActionResult CheckLogin(LogOnFormModel form)
        {
            try
            {

                int usererror = 0;
                if (ModelState.IsValid)
                {
                    User user = _userRepository.Get(u => (u.Email == form.UserName) && u.RoleId == (int)UserRole.ProfileAdmin && u.Activated != 3);
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
                                ModelState.AddModelError("", "Your account is  deleted by adtones administrator so please contact adtones admin.");
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
                            if (user.RoleId == (int)UserRole.ProfileAdmin)
                            {
                                urls = ConfigurationManager.AppSettings["profileAdmin"].ToString();
                                var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
                                return Redirect(siteaddress + urls);
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
                }

                // If we got this far, something failed
                return View("Index", form);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool ValidatePassword(User user, string password)
        {
            string encoded = Md5Encrypt.Md5EncryptPassword(password);
            return user.PasswordHash.Equals(encoded);
        }

        [Route("LogOff")]
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
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
    }
}