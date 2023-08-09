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
using EFMVC.Model;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Mailer;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;
using EFMVC.Domain.Commands.Security;
using EFMVC.Data;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Common;
using EFMVC.Domain.CountryConnectionString;

namespace EFMVC.Web.Controllers
{
    public class AdvertRegisterController : Controller
    { /// <summary>
      /// The _command bus
      /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _send email mailer
        /// </summary>
        private ISendEmailMailer _sendEmailMailer = new SendEmailMailer();
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly IFormsAuthentication _formAuthentication;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The form authentication
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="userRepository">The user repository.</param>
        //
        public ISendEmailMailer sendEmailMailer
        {
            get { return _sendEmailMailer; }
            set { _sendEmailMailer = value; }
        }
        public AdvertRegisterController(ICommandBus commandBus, IUserRepository userRepository, IFormsAuthentication formAuthentication, ICurrencyRepository currencyRepository, IContactsRepository contactsRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _formAuthentication = formAuthentication;
            _currencyRepository = currencyRepository;
            _contactsRepository = contactsRepository;
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ForgotPassword()
        {
            ForgotPassword _forgotPassword = new ViewModels.ForgotPassword();
            return View(_forgotPassword);
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword _model)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.Get(u => u.Email == _model.Email && u.RoleId == 3);
                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed";
                    return View("ForgotPassword", _model);
                }
                else if (user.VerificationStatus == false)
                {
                    TempData["error"] = "Please verify your email account.";
                    return View("ForgotPassword", _model);
                }
                else if (user.Activated == 0)
                {
                    TempData["error"] = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return View("ForgotPassword", _model);
                }
                else if (user.Activated == 2)
                {
                    TempData["error"] = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return View("ForgotPassword", _model);
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                string email = EncryptionHelper.EncryptSingleValue(user.Email);
                //string url = string.Format("{0}?activationCode={1}",
                //                           ConfigurationManager.AppSettings["ResetPassword"], email);
                string url = string.Format("<a href='" + ConfigurationManager.AppSettings["ResetPassword"] + "?activationCode=" + email + "'>{0}?activationCode={1}</a>",
                                           ConfigurationManager.AppSettings["ResetPassword"], email);
                var reader =
                          new StreamReader(
                              Server.MapPath(ConfigurationManager.AppSettings["ResetPasswordEmailTemplate"]));
                string emailContent = reader.ReadToEnd();
                emailContent = string.Format(emailContent, url);

                //MailHelper.SendMail(ConfigurationManager.AppSettings["SiteEmailAddress"], user.Email, null, null,
                //                    "Reset Password", emailContent,
                //                    ConfigurationManager.AppSettings["SmtpServerAddress"],
                //                    int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]));

                MailMessage mail = new MailMessage();
                mail.To.Add(user.Email);
                //mail.To.Add("xxx@gmail.com");
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                mail.Subject = "Reset Password";

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
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            try
            {
                TempData["ResetPassword"] = null;
                string email = Request.QueryString["activationCode"];
                email = EncryptionHelper.DecryptSingleValue(email);

                User user = _userRepository.Get(x => x.Email == email);

                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed";
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
        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordConfirmation _model)
        {
            try
            {
                User user = _userRepository.Get(x => x.Email == _model.Email);

                if (user == null)
                {
                    TempData["error"] = "Don't reveal that the user does not exist or is not confirmed";
                    return View("ResetPassword", _model);
                }
                else if (user.VerificationStatus == false)
                {
                    TempData["error"] = "Please verify your email account.";
                    return View("ResetPassword", _model);
                }
                else if (user.Activated == 0)
                {
                    TempData["error"] = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword", _model);
                }
                else if (user.Activated == 2)
                {
                    TempData["error"] = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return View("ResetPassword", _model);
                }
                var command = new ResetPasswordCommand
                {
                    UserId = user.UserId,
                    NewPassword = _model.NewPassword
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
        /// <summary>
        /// Registers the advertiser.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RegisterAdvertiser()
        {


            //Code commented on 28-08-2018 to check adverise register design on server
            //if (!SiteHelper.Instance.IsAdvertiserUrl)
            //    return View("Register");
            EFMVCDataContex db = new EFMVCDataContex();
            UserFormModel model = new UserFormModel();
            FillOrganisationType(db);
            //model.opratorAdminList = new List<SelectListItem> {
            //   new SelectListItem { Text = "-- Select Operator --", Value = "" }
            //};           
            //Code For Fill Country List on 12-11-2018 by Krunal.
            FillCountryList(model);

            return View(model);
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
        /// Registers the advertiser.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterAdvertiser(UserFormModel form)
        {
            ModelState.Remove("MSISDN");
            
            //Commented 26-02-2019
            //form.FirstName = null;
            try
            {
                if (ModelState.IsValid)
                {
                    //Add 18-04-2019
                    if (!string.IsNullOrEmpty(form.CountryCode))
                    {
                        EFMVCDataContex db = new EFMVCDataContex();
                        int countryID = int.Parse(form.CountryCode);
                        var countryCode = db.Country.Where(c => c.Id == countryID).Select(c => c.CountryCode).FirstOrDefault();
                        var firstCode = countryCode.StartsWith("+");
                        if (firstCode == true)
                        {
                            countryCode = countryCode.Remove(0, 1);
                        }
                        var mobileNo = countryCode + form.MSISDN;
                        var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(mobileNo));
                        if (contactExist != null)
                        {
                            TempData["Error"] = form.MSISDN + " Mobile Number is already exists in database so please choose another one.";
                            FillOrganisationType(db);
                            FillCountryList(form);
                            //FillOperatorAdminList(form);
                            return View(form);
                        }
                    }
                    

                    // var isEmailExist = _userRepository.GetMany(s => s.Email.ToLower() == form.Email.ToLower() && s.RoleId == 3 && (s.Activated == 1)).Any();
                    var isEmailExist = _userRepository.GetMany(s => s.Email.ToLower() == form.Email.ToLower()).FirstOrDefault();
                    if (isEmailExist != null)
                    {
                        //Add 18-04-2019
                        TempData["Error"] = form.Email + " Email Address is already exists in database so please choose another one.";
                        EFMVCDataContex db = new EFMVCDataContex();
                        FillOrganisationType(db);
                        FillCountryList(form);
                        //FillOperatorAdminList(form);
                        return View(form);
                    }
                    if (isEmailExist != null)
                    {
                        if (isEmailExist.Activated == 2)
                        {
                            TempData["Error"] = "Your account is suspended so please contact adtones admin.";
                            EFMVCDataContex db = new EFMVCDataContex();
                            FillOrganisationType(db);
                            FillCountryList(form);
                            //FillOperatorAdminList(form);
                            return View(form);
                        }
                        else if (isEmailExist.Activated == 3)
                        {
                            TempData["Error"] = "Your account is deleted so please contact adtones admin.";
                            EFMVCDataContex db = new EFMVCDataContex();
                            FillOrganisationType(db);
                            FillCountryList(form);
                            //FillOperatorAdminList(form);
                            return View(form);
                        }
                    }

                    UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
                    command.Activated = 0;
                    command.RoleId = (Int32)UserRoles.Advertiser;
                    command.Outstandingdays = 0;
                    command.IsSessionFlag = false;
                    command.LockOutTime = null;
                    command.LastPasswordChangedDate = DateTime.Now;
                    IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
                    ModelState.AddModelErrors(errors);
                    if (ModelState.IsValid)
                    {
                        //if (form.CountryCode == "44")
                        //    command.OperatorId = 1;
                        //else if (form.CountryCode == "254")
                        //    command.OperatorId = 2;
                        //else if (form.CountryCode == "221")
                        //    command.OperatorId = 3;
                        //else if (form.CountryCode == "91")
                        //    command.OperatorId = 4;
                        //else if (form.CountryCode == "39")
                        //    command.OperatorId = 5;
                        //else if (form.CountryCode == "33")
                        //    command.OperatorId = 6;
                        //else if (form.CountryCode == "45")
                        //    command.OperatorId = 7;
                        //else
                        //    command.OperatorId = 1;

                        //if (form.CountryCode == "7")
                        //    command.OperatorId = 1;
                        //else if (form.CountryCode == "9")
                        //    command.OperatorId = 2;
                        //else if (form.CountryCode == "10")
                        //    command.OperatorId = 3;
                        //else if (form.CountryCode == "11")
                        //    command.OperatorId = 4;
                        //else if (form.CountryCode == "12")
                        //    command.OperatorId = 5;
                        //else if (form.CountryCode == "13")
                        //    command.OperatorId = 6;
                        //else if (form.CountryCode == "14")
                        //    command.OperatorId = 7;
                        //else
                        //    command.OperatorId = 1;

                        command.OperatorId = Convert.ToInt32(0);

                        EFMVCDataContex db = new EFMVCDataContex();
                        var countryCode = "";
                        var countryID = 0;
                        var currencyID = 0;
                        if (!string.IsNullOrEmpty(form.CountryCode))
                        {
                            countryID = int.Parse(form.CountryCode);
                            countryCode = db.Country.Where(c => c.Id == countryID).Select(c => c.CountryCode).FirstOrDefault();
                            var firstCode = countryCode.StartsWith("+");
                            if (firstCode == true)
                            {
                                countryCode = countryCode.Remove(0, 1);
                            }
                        }

                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            User user = _userRepository.Get(u => u.Email == form.Email);
                            int compDetailId = 0;
                            int contactId = 0;
                            if (!string.IsNullOrEmpty(form.Organisation))
                            {
                                compDetailId = AddCompanyDetail(form.Organisation, user.UserId);
                            }

                            if (user.RoleId == 3)
                            {
                                //Commented 28-02-2019
                                //if (!string.IsNullOrEmpty(form.MSISDN))
                                //{
                                //    Contacts objContact = new Contacts();
                                //    var firstNumber = form.MSISDN.StartsWith("0");
                                //    if (firstNumber == true)
                                //    {
                                //        form.MSISDN = form.MSISDN.Remove(0, 1);
                                //    }
                                //    objContact.UserId = user.UserId;
                                //    objContact.Email = user.Email;
                                //    objContact.MobileNumber = countryCode + form.MSISDN;
                                //    if (!string.IsNullOrEmpty(form.CountryCode))
                                //    {
                                //        objContact.CountryId = countryID;
                                //        if (countryID == 13)
                                //        {
                                //            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                //            objContact.CurrencyId = currency.CurrencyId;
                                //        }
                                //        else
                                //        {
                                //            currencyID = _currencyRepository.GetAll().Where(top => top.CountryId == countryID).Select(top => top.CurrencyId).FirstOrDefault();
                                //        }
                                //        if (currencyID != 0)
                                //        {
                                //            objContact.CurrencyId = currencyID;
                                //        }
                                //        else
                                //        {
                                //            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                                //            objContact.CurrencyId = currency.CurrencyId;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                                //        objContact.CurrencyId = currency.CurrencyId;
                                //    }
                                //    db.Contacts.Add(objContact);
                                //    db.SaveChanges();
                                //}

                                
                                //Add 28-02-2019
                                if (!string.IsNullOrEmpty(form.MSISDN))
                                {
                                    Contacts objContact = new Contacts();
                                    
                                    if (!string.IsNullOrEmpty(form.CountryCode))
                                    {
                                        var firstNumber = form.MSISDN.StartsWith("0");
                                        if (firstNumber == true)
                                        {
                                            form.MSISDN = form.MSISDN.Remove(0, 1);
                                        }
                                        objContact.UserId = user.UserId;
                                        objContact.Email = user.Email;
                                        objContact.MobileNumber = countryCode + form.MSISDN;
                                        objContact.CountryId = countryID;
                                        if (countryID == 13)
                                        {
                                            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                            objContact.CurrencyId = currency.CurrencyId;
                                        }
                                        else
                                        {
                                            currencyID = _currencyRepository.GetAll().Where(top => top.CountryId == countryID).Select(top => top.CurrencyId).FirstOrDefault();
                                        }
                                        if (currencyID != 0)
                                        {
                                            objContact.CurrencyId = currencyID;
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
                                    }
                                    
                                    db.Contacts.Add(objContact);
                                    db.SaveChanges();
                                    contactId = objContact.Id;
                                }
                            }

                            #region Add records on operator server db of that country
                            var ConnString = ConnectionString.GetConnectionStringByCountryId(countryID);
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    db = new EFMVCDataContex(item);
                                    var userData = new User
                                    {
                                        FirstName = command.FirstName,
                                        LastName = command.LastName,
                                        Email = command.Email,
                                        Organisation = command.Organisation,
                                        Password = command.Password,
                                        RoleId = command.RoleId,
                                        DateCreated = DateTime.Now,
                                        LastLoginTime = DateTime.Now,
                                        Activated = command.Activated,
                                        Outstandingdays = command.Outstandingdays,
                                        VerificationStatus = command.VerificationStatus,
                                        OperatorId = command.OperatorId,
                                        IsMobileVerfication = command.IsMobileVerfication,
                                        OrganisationTypeId = command.OrganisationTypeId == 0 ? null : command.OrganisationTypeId,
                                        UserMatchTableName = command.UserMatchTableName,
                                        IsMsisdnMatch = command.IsMsisdnMatch,
                                        AdtoneServerUserId = user.UserId,
                                        IsSessionFlag = false,
                                        LockOutTime = null,
                                        LastPasswordChangedDate = DateTime.Now
                                    };

                                    db.Users.Add(userData);
                                    db.SaveChanges();

                                   // var userDetails = db.Users.Where(u => u.Email == form.Email).FirstOrDefault();

                                    if (!string.IsNullOrEmpty(form.Organisation))
                                    {
                                        CompanyDetails comp = new CompanyDetails();
                                        comp.UserId = userData.UserId;
                                        comp.CompanyName = form.Organisation;
                                        comp.AdtoneServerCompanyDetailId = compDetailId;
                                        db.CompanyDetails.Add(comp);
                                        db.SaveChanges();
                                    }

                                    if (userData.RoleId == 3)
                                    {
                                        if (!string.IsNullOrEmpty(form.MSISDN))
                                        {
                                            Contacts objContact = new Contacts();

                                            if (!string.IsNullOrEmpty(form.CountryCode))
                                            {
                                                var firstNumber = form.MSISDN.StartsWith("0");
                                                if (firstNumber == true)
                                                {
                                                    form.MSISDN = form.MSISDN.Remove(0, 1);
                                                }
                                                objContact.UserId = userData.UserId;
                                                objContact.Email = userData.Email;
                                                objContact.MobileNumber = countryCode + form.MSISDN;
                                                objContact.CountryId = countryID;
                                                if (countryID == 13)
                                                {
                                                    var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                                    objContact.CurrencyId = currency.CurrencyId;
                                                }
                                                else
                                                {
                                                    currencyID = _currencyRepository.GetAll().Where(top => top.CountryId == countryID).Select(top => top.CurrencyId).FirstOrDefault();
                                                }
                                                if (currencyID != 0)
                                                {
                                                    objContact.CurrencyId = currencyID;
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
                                            }
                                            objContact.AdtoneServerContactId = contactId;

                                            db.Contacts.Add(objContact);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                            #endregion

                            string email = EncryptionHelper.EncryptSingleValue(user.Email);
                            string url = string.Format("{0}?activationCode={1}",
                                                       ConfigurationManager.AppSettings["AdvertiserVerificationUrl"], email);

                            var reader =
                                new StreamReader(
                                    Server.MapPath(ConfigurationManager.AppSettings["AdvertiserVerificationEmailTemplate"]));
                            string emailContent = reader.ReadToEnd();
                            emailContent = string.Format(emailContent, url);

                            //MailHelper.SendMail(ConfigurationManager.AppSettings["SiteEmailAddress"], user.Email, null, null,
                            //                    "Email Verification", emailContent,
                            //                    ConfigurationManager.AppSettings["SmtpServerAddress"],
                            //                    int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]));

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

                            return RedirectToAction("RegisterAdvertiserSuccess", "AdvertRegister");
                        }

                        ModelState.AddModelError("", "An unknown error occurred.");
                    }
                    // If we got this far, something failed, redisplay form
                    EFMVCDataContex db1 = new EFMVCDataContex();
                    FillOrganisationType(db1);
                    FillCountryList(form);
                    //FillOperatorAdminList(form);
                    return View(form);
                }
            }
            catch (Exception ex)
            {
                GenerateTicket(ex.Message.ToString(), form.Email, form.FirstName + " " + form.LastName);
                return Json(new { errors = GetErrorsFromModelState() });
            }
            GenerateTicket("Something went wrong for advert registration", form.Email, form.FirstName + " " + form.LastName);
            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
        public void GenerateTicket(string message, string email, string userName)
        {

            string ticketCode = LiveAgent.CreateTicket("Advert registration error", message, email);
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


        private int AddCompanyDetail(string organisation, int userId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            CompanyDetails comp = new CompanyDetails();
            comp.UserId = userId;
            comp.CompanyName = organisation;
            db.CompanyDetails.Add(comp);
            db.SaveChanges();
            return comp.Id;
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
                    EFMVCDataContex db = new EFMVCDataContex();

                    var command = new ChangeVerificationStatusCommand { UserId = user.UserId, VerificationStatus = true };

                    ICommandResult commandResult = _commandBus.Submit(command);

                    var isGravityUser = db.GravityFormsTrack.Where(s => s.Email == email).Any();
                    if (isGravityUser)
                    {
                        sendemailtoadmin(user.Email, user.FirstName, user.LastName, user.Organisation);
                        return RedirectToAction("Index", "Landing");
                    }

                    ViewData.Add("VerifyAdvertiserResult", "success");

                }
                else
                {
                    ViewData.Add("VerifyAdvertiserResult", "failed");
                }
                //sendemailtoadmin
                sendemailtoadmin(user.Email, user.FirstName, user.LastName, user.Organisation);
            }
            catch (Exception)
            {
                ViewData.Add("VerifyAdvertiserResult", "failed");
            }

            return View();
        }
        public void sendemailtoadmin(string email, string fname, string lname, string Organisation)
        {

            //send email to admin
            var adminaddress = ConfigurationManager.AppSettings["adminemailaddress"].ToString();
            DateTimeOffset uktimezone = TimeZoneInfo.ConvertTime(
            DateTime.Now, TimeZoneInfo.Local,
            TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            //DateTime utc = uktimezone.UtcDateTime;
            //string current_time = utc.ToString("HH:mm dd/MM/yyyy");

            string current_time = uktimezone.DateTime.ToString("HH:mm dd-MM-yyyy");
            string subject = "Campaign Manager Registration-" + current_time + "-" + fname + " " + lname;
            string[] mailto = new string[1];
            mailto[0] = adminaddress;
            sendEmail(adminaddress, fname, lname, Organisation, subject, "1", mailto, null, null, null, true, current_time, email);
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
        /// <summary>
        /// Registers the advertiser success.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult RegisterAdvertiserSuccess()
        {

            return View();
        }
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

        private void FillOrganisationType(EFMVCDataContex db)
        {
            var typeList = db.OrganisationType.Select(top => new
            {
                Name = top.Type,
                Id = top.OrganisationTypeId
            }).ToList();
            ViewBag.OrgType = new MultiSelectList(typeList, "Id", "Name");

        }

        private void FillCountryList(UserFormModel model)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var countryList = db.Country.Select(s => new SelectListItem { Text = s.Name + " " + s.CountryCode, Value = s.Id.ToString() }).OrderBy(s => s.Text).ToList();
            countryList.Insert(0, new SelectListItem { Text = "--Select Country--", Value = "" });
            model.CountryList = countryList;
        }

        /// <summary>
        /// Gets the state of the errors from model.
        /// </summary>
        /// <returns>IEnumerable{System.String}.</returns>
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        public void sendEmail(string to, string fname, string lname, string Organisation, string subject, string formatId, string[] mailTo, string[] mailCC, string[] mailBcc, string[] attachment, bool isBodyHTML, string completedDatetime, string UserEmail)
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
            EmailContent.Organisation = Organisation;
            EmailContent.Subject = subject;
            EmailContent.FormatId = 1;
            EmailContent.CompletedDatetime = completedDatetime;
            if (EmailContent.FormatId != 1)
            {
                sendEmailMailer.SendEmail(EmailContent).SendAsync();
            }
            else
            {

                var reader =
                         new StreamReader(
                             Server.MapPath(ConfigurationManager.AppSettings["AdminTempleteVerification"]));
                string emailContent = reader.ReadToEnd();
                //var GetLogo = Server.MapPath("~/Images/adtoneslogo.png");
                //var GetLogo = "/Images/adtoneslogo.png";
                var GetLogo = ConfigurationManager.AppSettings["adtonelogo"].ToString();
                emailContent = string.Format(emailContent, GetLogo, EmailContent.Fname, EmailContent.Lname, EmailContent.Organisation, UserEmail, EmailContent.CompletedDatetime, EmailContent.Link);


                MailMessage mail = new MailMessage();


                mail.To.Add(to);
                //mail.To.Add("xxx@gmail.com");
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                mail.Subject = subject;

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
            }
        }
        //[HttpPost]
        //public ActionResult FillOperatorAdmin(int countryID)
        //{
        //    EFMVCDataContex db = new EFMVCDataContex();
        //    var lst = db.Operator.Where(a => a.CountryId == countryID).Select(s => new SelectListItem { Text = s.OperatorName , Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
        //    return Json(lst, JsonRequestBehavior.AllowGet);

        //}

        private void FillOperatorAdminList(UserFormModel model)
        {
            int countryID = int.Parse(model.CountryCode);
            EFMVCDataContex db = new EFMVCDataContex();
            var operatorAdminList = db.Operator.Where(a => a.CountryId == countryID).Select(s => new SelectListItem { Text = s.OperatorName, Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
            operatorAdminList.Insert(0, new SelectListItem { Text = "--Select Operator--", Value = "" });
            model.opratorAdminList = operatorAdminList;
        }
    }
}
