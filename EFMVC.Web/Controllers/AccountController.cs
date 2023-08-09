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
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class AccountController : Controller
    {
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

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;



        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="contactsRepository">The contacts repository.</param>
        /// <param name="companydetailsRepository">The companydetails repository.</param>
        public AccountController(ICommandBus commandBus, IContactsRepository contactsRepository, ICompanyDetailsRepository companydetailsRepository, IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _contactsRepository = contactsRepository;
            _companydetailsRepository = companydetailsRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
        }
        
        public ActionResult Index()
        {
            FillCountryList();
            AccountInfo _accountInfo = new AccountInfo();
            UserProfileInfo _profileinfo = new UserProfileInfo();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var userinfo = _userRepository.GetById(efmvcUser.UserId);
            if (userinfo != null)
            {
                ViewBag.emailAddress = userinfo.Email;
                _profileinfo.Email = userinfo.Email;
                _profileinfo.FirstName = userinfo.FirstName;
                _profileinfo.LastName = userinfo.LastName;
            }
            ContactsFormModel contactmodel = new ContactsFormModel();
            CompanyDetailsFormModel companymodel = new CompanyDetailsFormModel();
            var _contactinfo = _contactsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            if (_contactinfo != null)
            {
                contactmodel = Mapper.Map<Contacts, ContactsFormModel>(_contactinfo);
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
            return View(_accountInfo);

        }
        private void FillCountryList()
        {
       
            var country = (from action in _countryRepository.GetAll()
                           select new SelectListItem
                           {
                               Text = action.Name.ToString(),
                               Value = action.Id.ToString()
                           }).ToList();
            
            ViewBag.country = country;
        }
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
        [HttpPost]
        public ActionResult UpdateUserInfo(string email,string oldemail, string firstname, string lastname)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var _existsemail = _userRepository.GetMany(top => top.Email.Trim().ToLower() == email.Trim().ToLower() && top.UserId != efmvcUser.UserId).FirstOrDefault();
                    if (_existsemail != null)
                    {
                        if (_existsemail.Activated == 3)
                        {
                            //use deleted email address
                        }
                        else
                        {
                            return Json("The Email Address is already exists in database so please choose another one.");
                        }
                    }
                    var command = new ChangeUserProfileInfoCommand
                    {
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname,
                        UserId = efmvcUser.UserId
                    };

                    ICommandResult commandResult = _commandBus.Submit(command);

                    if (commandResult.Success)
                    {
                        if(email!= oldemail)
                        {
                            sendemailtonewuser(email, oldemail);
                            sendemailtoolduser(email, oldemail);
                           
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
        public void sendemailtonewuser(string newemail,string oldemail)
        {

            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["ChangeEmailAddressTemplate"]));
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, oldemail , newemail);
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
