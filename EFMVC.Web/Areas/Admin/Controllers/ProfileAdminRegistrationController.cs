using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFMVC.Web.Models;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Core.Common;
using EFMVC.CommandProcessor.Command;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using EFMVC.Domain.Commands.ProfileAdmin;
using EFMVC.Domain.Commands.Contacts;
using AutoMapper;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("ProfileAdminRegistration")]
    public class ProfileAdminRegistrationController : Controller
    {
        // GET: Admin/ProfileAdminRegistration

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _contacts repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public ProfileAdminRegistrationController(ICommandBus commandBus, IUserRepository userRepository, IContactsRepository contactsRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _contactsRepository = contactsRepository;
        }

        [Route("Index")]
        public ActionResult Index()
        {
            List<ProfileAdminRegistrationResult> _result = FillProfileAdminResult();
            SearchClass.ProfileAdminRegistrationFilter _filterCritearea = new SearchClass.ProfileAdminRegistrationFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                List<ProfileAdminRegistrationResult> _result = new List<ProfileAdminRegistrationResult>();
                IEnumerable<User> user = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null") searchValue = true;
                    }
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    string FirstName = string.Empty;
                    string LastName = string.Empty;
                    string Email = string.Empty;
                    string Organisation = string.Empty;
                    int[] OperatorId = new int[cnt];
                    int[] CountryId = new int[cnt];
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null") FirstName = columnSearch[0].ToString();
                        else columnSearch[0] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null") LastName = columnSearch[1].ToString();
                        else columnSearch[1] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null") Email = columnSearch[2].ToString();
                        else columnSearch[2] = null;
                    }
                    user = _userRepository.GetMany(top => top.RoleId == (int)UserRole.ProfileAdmin).OrderByDescending(top => top.DateCreated).ToList();
                    foreach (var item in user)
                    {
                        _result.Add(new ProfileAdminRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
                    }
                    if (columnSearch[0] != null) _result = _result.Where(top => top.FirstName == FirstName).ToList();
                    if (columnSearch[1] != null) _result = _result.Where(top => top.LastName == LastName).ToList();
                    if (columnSearch[2] != null) _result = _result.Where(top => top.Email == Email).ToList();
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    #endregion
                }
                else
                {
                    user = _userRepository.GetMany(top => top.RoleId == (int)UserRole.ProfileAdmin).OrderByDescending(top => top.DateCreated).ToList();
                    foreach (var item in user)
                    {
                        _result.Add(new ProfileAdminRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }
                _result = ApplySorting(param, _result);
                DTResult<ProfileAdminRegistrationResult> result = new DTResult<ProfileAdminRegistrationResult>
                {
                    draw = param.Draw,
                    data = _result,
                    recordsFiltered = cnt,
                    recordsTotal = cnt
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Function For Filter/Sorting Profile Admin Registration Data
        private static List<ProfileAdminRegistrationResult> ApplySorting(DTParameters param, List<ProfileAdminRegistrationResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.FirstName).ToList();
                    else
                        result = result.OrderByDescending(top => top.FirstName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.LastName).ToList();
                    else
                        result = result.OrderByDescending(top => top.LastName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Email).ToList();
                    else
                        result = result.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.IsActive).ToList();
                    else
                        result = result.OrderByDescending(top => top.IsActive).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        // Listing Profile Admin
        public List<ProfileAdminRegistrationResult> FillProfileAdminResult()
        {
            List<ProfileAdminRegistrationResult> _result = new List<ProfileAdminRegistrationResult>();
            var operatorRegistrationResult = _userRepository.GetMany(top => top.RoleId == (int)UserRole.ProfileAdmin).OrderByDescending(top => top.DateCreated).ToList();
            foreach (var item in operatorRegistrationResult)
            {
                _result.Add(new ProfileAdminRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
            }
            return _result;
        }

        //Add Profile Admin Registration
        [Route("AddProfileAdminRegistration")]
        public ActionResult AddProfileAdminRegistration()
        {
            ProfileAdminRegistrationFormModel profileAdminRegistrationFormModel = new ProfileAdminRegistrationFormModel();
            profileAdminRegistrationFormModel.Activated = 1;
            return View(profileAdminRegistrationFormModel);
        }

        //Save Profile Admin Registration
        [Route("AddProfileAdminRegistration")]
        [HttpPost]
        public ActionResult AddProfileAdminRegistration(ProfileAdminRegistrationFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExist = _userRepository.Get(top => top.Email.ToLower().Equals(model.Email.ToLower()));
                    var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(model.MobileNumber));
                    if (userExist != null)
                    {
                        TempData["Error"] = model.Email + " Email Exist.";
                        return View("AddProfileAdminRegistration", model);
                    }
                    if (contactExist != null)
                    {
                        TempData["Error"] = model.MobileNumber + " Mobile Number Exist.";
                        return View("AddProfileAdminRegistration", model);
                    }
                    CreateOrUpdateProfileAdminRegistrationCommand command =
                        Mapper.Map<ProfileAdminRegistrationFormModel, CreateOrUpdateProfileAdminRegistrationCommand>(model);
                    command.Email = model.Email;
                    command.FirstName = model.FirstName;
                    command.LastName = model.LastName;
                    command.Password = Md5Encrypt.Md5EncryptPassword(model.Password);
                    command.DateCreated = DateTime.Now;
                    command.LastLoginTime = DateTime.Now;
                    command.RoleId = (int)UserRole.ProfileAdmin;
                    command.Activated = model.Activated;
                    command.VerificationStatus = true;
                    command.Outstandingdays = 0;
                    command.IsMsisdnMatch = true;
                    command.IsEmailVerfication = true;
                    command.PhoneticAlphabet = null;
                    command.IsMobileVerfication = true;
                    command.OrganisationTypeId = null;
                    command.UserMatchTableName = null;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        CreateOrUpdateContactsCommand command1 =
                            Mapper.Map<ProfileAdminRegistrationFormModel, CreateOrUpdateContactsCommand>(model);
                        command1.UserId = result.Id;
                        command1.MobileNumber = model.MobileNumber;
                        command1.FixedLine = null;
                        command1.Email = model.Email;
                        command1.PhoneNumber = model.PhoneNumber;
                        command1.Address = model.Address;
                        command1.CountryId = null;
                        command1.CurrencyId = null;
                        ICommandResult result1 = _commandBus.Submit(command1);
                        if (result1.Success)
                        {
                            SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.Password);
                            TempData["status"] = "Profile Admin registered " + model.FirstName + " " + model.LastName;
                            return RedirectToAction("Index");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View("AddProfileAdminRegistration", model);
            }
        }

        //Edit Profile Admin Registration
        [Route("UpdateProfileAdminRegistration")]
        public ActionResult UpdateProfileAdminRegistration(int id)
        {
            try
            {
                ProfileAdminRegistrationFormModel model = new ProfileAdminRegistrationFormModel();
                var userData = _userRepository.Get(top => top.UserId == id);
                var userContactData = _contactsRepository.Get(top => top.UserId == id);
                model.UserId = userData.UserId;
                model.Email = userData.Email;
                model.FirstName = userData.FirstName;
                model.LastName = userData.LastName;
                model.MobileNumber = userContactData.MobileNumber;
                model.PhoneNumber = userContactData.PhoneNumber;
                model.Address = userContactData.Address;
                model.Activated = userData.Activated;
                model.OldEmail = userData.Email;
                model.Password = userData.PasswordHash;
                return View(model);
            }
            catch (Exception ex)
            {
                ProfileAdminRegistrationFormModel model = new ProfileAdminRegistrationFormModel();
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        //Update Profile Admin Registration
        [Route("UpdateProfileAdminRegistration")]
        [HttpPost]
        public ActionResult UpdateProfileAdminRegistration(ProfileAdminRegistrationFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExist = _userRepository.Get(top => top.Email.ToLower().Equals(model.Email.ToLower()) && top.UserId != model.UserId);
                    var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(model.MobileNumber) && top.UserId != model.UserId);
                    if (userExist != null)
                    {
                        TempData["Error"] = model.Email + " Email Exist.";
                        return View("UpdateProfileAdminRegistration", model);
                    }
                    if (contactExist != null)
                    {
                        TempData["Error"] = model.MobileNumber + " Mobile Number Exist.";
                        return View("UpdateProfileAdminRegistration", model);
                    }
                    var userData = _userRepository.GetById(model.UserId);
                    CreateOrUpdateProfileAdminRegistrationCommand command =
                        Mapper.Map<ProfileAdminRegistrationFormModel, CreateOrUpdateProfileAdminRegistrationCommand>(model);
                    command.UserId = userData.UserId;
                    command.Email = model.Email;
                    command.FirstName = model.FirstName;
                    command.LastName = model.LastName;
                    command.Password = model.Password == null ? userData.PasswordHash : Md5Encrypt.Md5EncryptPassword(model.Password);
                    command.DateCreated = userData.DateCreated;
                    command.Organisation = model.Organisation;
                    command.LastLoginTime = (DateTime)userData.LastLoginTime;
                    command.RoleId = userData.RoleId;
                    command.Activated = model.Activated;
                    command.VerificationStatus = userData.VerificationStatus;
                    command.Outstandingdays = userData.Outstandingdays;
                    command.OperatorId = model.OperatorId;
                    command.IsMsisdnMatch = userData.IsMsisdnMatch;
                    command.IsEmailVerfication = userData.IsEmailVerfication;
                    command.PhoneticAlphabet = userData.PhoneticAlphabet;
                    command.IsMobileVerfication = userData.IsMobileVerfication;
                    command.OrganisationTypeId = userData.OrganisationTypeId;
                    command.UserMatchTableName = userData.UserMatchTableName;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        var userContactData = _contactsRepository.Get(top => top.UserId == model.UserId);
                        CreateOrUpdateContactsCommand command1 =
                            Mapper.Map<ProfileAdminRegistrationFormModel, CreateOrUpdateContactsCommand>(model);
                        command1.Id = userContactData.Id;
                        command1.UserId = userContactData.UserId;
                        command1.MobileNumber = model.MobileNumber;
                        command1.FixedLine = userContactData.FixedLine;
                        command1.Email = model.Email;
                        command1.PhoneNumber = model.PhoneNumber == "" ? userContactData.PhoneNumber : model.PhoneNumber;
                        command1.Address = model.Address == "" ? userContactData.Address : model.Address;
                        command1.CountryId = null;
                        command1.CurrencyId = null;
                        ICommandResult result1 = _commandBus.Submit(command1);
                        if (result1.Success)
                        {
                            if (model.Password != null && model.Password != "")
                            {
                                var password = Md5Encrypt.Md5EncryptPassword(model.Password);
                                if (model.OldEmail != model.Email || model.OldPassword != password)
                                {
                                    SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.Password);
                                }
                            }
                            TempData["status"] = "Profile Admin details updated " + model.FirstName + " " + model.LastName;
                            return RedirectToAction("Index");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View("UpdateProfileAdminRegistration", model);
            }
        }

        //Search Profile Admin Registration
        [Route("SearchProfileAdminRegistration")]
        public ActionResult SearchProfileAdminRegistration([Bind(Prefix = "Item2")]SearchClass.ProfileAdminRegistrationFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ProfileAdminRegistrationResult> _result = new List<ProfileAdminRegistrationResult>();
                var finalresult = new List<ProfileAdminRegistrationResult>();
                if (_filterCritearea != null)
                {
                    _result = FillProfileAdminResult();
                    finalresult = getProfileAdminRegistrationInformationResult(_result, _filterCritearea);
                }
                else
                {
                    _result = FillProfileAdminResult();
                    finalresult = getProfileAdminRegistrationInformationResult(_result, _filterCritearea);
                }
                return PartialView("_ProfileAdminRegistrationDetails", finalresult);
            }
            else return PartialView("_ProfileAdminRegistrationDetails", "notauthorise");
        }

        //Search Profile Admin Registration
        public List<ProfileAdminRegistrationResult> getProfileAdminRegistrationInformationResult(List<ProfileAdminRegistrationResult> profileAdminRegistrationInformationresult, SearchClass.ProfileAdminRegistrationFilter _filterCritearea)
        {
            if (profileAdminRegistrationInformationresult != null && _filterCritearea != null)
            {
                if (_filterCritearea.FirstName != null)
                {
                    profileAdminRegistrationInformationresult = profileAdminRegistrationInformationresult.Where(top => top.FirstName.ToLower().Contains(_filterCritearea.FirstName.ToLower())).ToList();
                }
                if (_filterCritearea.LastName != null)
                {
                    profileAdminRegistrationInformationresult = profileAdminRegistrationInformationresult.Where(top => top.LastName.ToLower().Contains(_filterCritearea.LastName.ToLower())).ToList();
                }
                if (_filterCritearea.Email != null)
                {
                    profileAdminRegistrationInformationresult = profileAdminRegistrationInformationresult.Where(top => top.Email.ToLower().Contains(_filterCritearea.Email)).ToList();
                }
            }
            return profileAdminRegistrationInformationresult;
        }

        //Send Email to Profile Admin
        private void SendEmailVerificationCode(string firstName, string LastName, string email, string password)
        {
            var reader = new StreamReader(Server.MapPath(ConfigurationManager.AppSettings["OperatorAdminRegistrationEmailTemplete"]));
            var url = ConfigurationManager.AppSettings["OperatorAdminUrl"];
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, firstName, LastName, url, email, password);
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
            mail.Subject = "Profile Admin Registration";
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
}