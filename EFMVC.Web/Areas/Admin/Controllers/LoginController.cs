using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [RouteArea("Admin")]
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
        // GET: Admin/Login

        [Route("Index")]
        public ActionResult Index()
        {
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
        public ActionResult CheckLogin(LogOnFormModel form)
        {
            try
            {                         
                int usererror = 0;
                if (ModelState.IsValid)
                {
                    User user = _userRepository.Get(u => (u.Email.ToLower() == form.UserName.ToLower()) && u.Activated != 3);
                    if (user != null)
                    {
                        if (user.VerificationStatus == false)
                        {
                            ModelState.AddModelError("", "Please verify your email account.");
                            usererror = -1;
                        }
                        else if (user.Activated == 0)
                        {
                            if (user.RoleId == 6)
                            {
                                ModelState.AddModelError("", "Your account has been InActive by adtones administrator.so, Please contact adtones admin.");
                                usererror = -1;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your account is not approved by adtones administrator so please contact adtones admin.");
                                usererror = -1;
                            }
                        }
                        else if (user.Activated == 1)
                        {
                            //if (user.OperatorId == (int)OperatorTableId.Safaricom)
                            //{
                            //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            //    usererror = -1;
                            //}
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
                            var siteaddress = ConfigurationManager.AppSettings["siteAddress"].ToString();

                            if (user.RoleId == (int)UserRole.Admin)
                            {
                                urls = ConfigurationManager.AppSettings["adminURL"].ToString();
                                return Redirect(siteaddress+urls);
                            }
                            else if (user.RoleId == (int)UserRole.UserAdmin)
                            {
                                urls = ConfigurationManager.AppSettings["userAdminURL"].ToString();
                                return Redirect(siteaddress + urls);
                            }
                            else if (user.RoleId == (int)UserRole.OperatorAdmin)
                            {
                                urls = ConfigurationManager.AppSettings["operatorAdmin"].ToString();
                                return Redirect(siteaddress + urls);
                            }
                            else if (user.RoleId == (int)UserRole.AdvertAdmin)
                            {
                                urls = ConfigurationManager.AppSettings["advertAdminURL"].ToString();
                                return Redirect(siteaddress + urls);
                            }
                            else if (user.RoleId == (int)UserRole.ProfileAdmin)
                            {
                                urls = ConfigurationManager.AppSettings["profileAdmin"].ToString();
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

        //Add 10-06-2019
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
                User user = _userRepository.Get(u => u.Email == model.Email && (u.RoleId == 1 || u.RoleId == 4 || u.RoleId == 5 || u.RoleId == 6));
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
                else if (user.Activated == 3)
                {
                    TempData["error"] = "Your account is  deleted by adtones administrator so please contact adtones admin.";
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

                User user = _userRepository.Get(x => x.Email == email && (x.RoleId == 1 || x.RoleId == 4 || x.RoleId == 5 || x.RoleId == 6));

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
                User user = _userRepository.Get(x => x.Email == model.Email  && (x.RoleId == 1 || x.RoleId == 4 || x.RoleId == 5 || x.RoleId == 6));

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

        [Route("GetCopyRight")]
        [HttpGet]
        public ActionResult GetCopyRight()
        {
            CopyRight copyright = _copyRightRepository.GetAll().LastOrDefault();
            String copyRightText = String.Empty;

            if (copyright != null)
                copyRightText = _copyRightRepository.GetAll().LastOrDefault().CopyRightText;

            return Json(copyRightText, JsonRequestBehavior.AllowGet);
        }
    }
}