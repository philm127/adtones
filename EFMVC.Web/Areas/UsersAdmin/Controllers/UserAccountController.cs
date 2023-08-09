using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Web.Areas.UsersAdmin.ViewModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using DocumentFormat.OpenXml.Wordprocessing;
using EFMVC.Web.Models;
using Newtonsoft.Json.Linq;

namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("UserAccount")]
    public class UserAccountController : Controller
    {
        // GET: UsersAdmin/UserAccount
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;
        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserAccountController(ICommandBus commandBus, IUserRepository userRepository, IProfileRepository profileRepository, IUserProfileRepository userProfileRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
        }
        public void FillUserRole()
        {
            IEnumerable<Common.UserRole> userroleTypes = Enum.GetValues(typeof(Common.UserRole))
                                                     .Cast<Common.UserRole>();
            var userrole = (from action in userroleTypes
                            select new SelectListItem
                            {
                                Text = action.ToString(),
                                Value = ((int)action).ToString()
                            }).ToList();
            ViewBag.userrole = userrole;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            List<UserResult> _result = new List<UserResult>();
            FillUserStatus();
            FillCountry();
            SearchClass.UserFilter _filterCritearea = new SearchClass.UserFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }
        public void FillUserStatus()
        {
            IEnumerable<Common.UserStatus> userTypes = Enum.GetValues(typeof(Common.UserStatus))
                                                     .Cast<Common.UserStatus>();
            var userstatus = (from action in userTypes
                              select new
                              {
                                  Text = action.ToString(),
                                  Value = ((int)action).ToString()
                              }).ToList();
            ViewBag.userStatus = new MultiSelectList(userstatus, "Value", "Text");
        }

        public class ExtendedParameters
        {
            public string[] Countries { get; set; }
            public string[] Statuses { get; set; }
            public string[] Operators { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string MSISDN { get; set; }
            public string Email { get; set; }
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters<ExtendedParameters> param)
        {
            try
            {
                IQueryable<UserResult> userResult = null;

                userResult = _userRepository.AsQueryable().Where(u=>u.VerificationStatus && u.RoleId == 2 && u.Operator != null)
                    .Join(_userProfileRepository.AsQueryable(), u => u.UserId, p => p.UserId, (u, up) =>
                        new UserResult()
                        {
                            Activated = u.Activated,
                            DateCreated = u.DateCreated,
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            RoleId = u.RoleId,
                            MSISDN = up.MSISDN,
                            UserId = u.UserId,
                            OperatorId = u.OperatorId,
                            CountryId = u.Operator.CountryId ?? 0,
                            RoleName = ((UserRole)u.RoleId).ToString(),
                        });
                var total = userResult.Count();

                userResult = SetFilter(param.ExtendedParameters, userResult);

                if (param.Order != null && param.Order.Length > 0)
                {
                    foreach (DTOrder order in param.Order)
                    {
                        var col = param.Columns[order.Column];
                        string fieldName = col.Data;
                        switch (fieldName)
                        {
                            case nameof(UserResult.Email):
                                userResult = order.Dir == DTOrderDir.DESC ? userResult.OrderByDescending(u => u.Email) : userResult.OrderBy(u => u.Email);
                                break;
                            case nameof(UserResult.Name):
                                userResult = order.Dir == DTOrderDir.DESC ? userResult.OrderByDescending(u => u.FirstName).ThenByDescending(u=>u.LastName) : userResult.OrderBy(u => u.FirstName).ThenBy(u=>u.LastName);
                                break;
                            case nameof(UserResult.MSISDN):
                                userResult = order.Dir == DTOrderDir.DESC ? userResult.OrderByDescending(u => u.MSISDN) : userResult.OrderBy(u => u.MSISDN);
                                break;
                            case nameof(UserResult.CreatedDate):
                                userResult = order.Dir == DTOrderDir.DESC ? userResult.OrderByDescending(u => u.DateCreated) : userResult.OrderBy(u => u.DateCreated);
                                break;
                            case nameof(UserResult.Status):
                                userResult = order.Dir == DTOrderDir.DESC ? userResult.OrderByDescending(u => u.Status) : userResult.OrderBy(u => u.Status);
                                break;
                        }
                        
                        break;
                    }
                }
                else
                    userResult = userResult.OrderBy(u => u.UserId);
                
                
                int totalFiltered = userResult.Count();
                var returnedData = userResult.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserResult> result = new DTResult<UserResult>
                {
                    draw = param.Draw,
                    data = returnedData,
                    recordsFiltered = totalFiltered,
                    recordsTotal = total
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private IQueryable<UserResult> SetFilter(ExtendedParameters p, IQueryable<UserResult> source)
        {
            if (p == null)
                return source;
            if (p.Countries != null && p.Countries.Length > 0)
            {
                var trimmedAndParsed = p.Countries.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => int.Parse(c.Trim()));
                HashSet<int> countryIds = new HashSet<int>(trimmedAndParsed);
                if(countryIds.Count > 0)
                    source = source.Where(u=>countryIds.Contains(u.CountryId));
            }

            if (p.Operators != null && p.Operators.Length > 0)
            {
                var trimmedAndParsed = p.Operators.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => int.Parse(o.Trim()));
                HashSet<int> operatorIds = new HashSet<int>(trimmedAndParsed);
                if (operatorIds.Count > 0)
                    source = source.Where(u => operatorIds.Contains(u.OperatorId));
            }

            if (p.Statuses != null && p.Statuses.Length > 0)
            {
                var trimmedAndParsed = p.Statuses.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => int.Parse(o.Trim()));
                HashSet<int> statusIds = new HashSet<int>(trimmedAndParsed);
                if (statusIds.Count > 0)
                    source = source.Where(u => statusIds.Contains(u.Activated));
            }

            if (!string.IsNullOrWhiteSpace(p.MSISDN))
            {
                string trimmed = p.MSISDN.Trim();
                source = source.Where(u => u.MSISDN.Contains(trimmed));
            }

            if (!string.IsNullOrWhiteSpace(p.DateFrom))
            {
                var trimmed = p.DateFrom.Trim();
                DateTime dateTime;
                if (DateTime.TryParseExact(trimmed, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                    source = source.Where(u => u.DateCreated >= dateTime);
            }

            if (!string.IsNullOrWhiteSpace(p.DateTo))
            {
                var trimmed = p.DateTo.Trim();
                DateTime dateTime;
                if (DateTime.TryParseExact(trimmed, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                    source = source.Where(u => u.DateCreated <= dateTime);
            }

            if (!string.IsNullOrWhiteSpace(p.Email))
            {
                var trimmed = p.Email.Trim();
                source = source.Where(u => u.Email == trimmed);
            }

            return source;
        }

        [Route("UpdateUserInfo")]
        public ActionResult UserDetails(int id)
        {
            Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();
            UserProfileFormModel model = new UserProfileFormModel();

            ViewBag.fullname = null;
            var details = _userRepository.Get(top => top.UserId == id);
            ViewBag.UserId = details.UserId;
            ViewBag.fullname = details.FirstName + " " + details.LastName;
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(details);
            if (userFormModel.UserProfile != null)
            {
                Model.UserProfile userProfile = _profileRepository.GetById(userFormModel.UserProfile.UserProfileId);
                model = Mapper.Map<Model.UserProfile, UserProfileFormModel>(userProfile);
            }
            //code commented on 30-03-2017
            // model.LocationList = selectLists["locationList"];
            model.GenderList = selectLists["genderList"];
            model.IncomeBracketList = selectLists["incomeBracketList"];
            model.WorkingStatusList = selectLists["workingStatusList"];
            model.RelationshipStatusList = selectLists["relationshipStatusList"];
            model.EducationList = selectLists["educationList"];
            model.HouseholdStatusList = selectLists["householdStatusList"];
            return View(model);

        }
        [Route("ChangePassword")]
        public ActionResult ChangePassword(int id)
        {
            var details = _userRepository.Get(top => top.UserId == id);
            ViewBag.UserId = details.UserId;
            ViewBag.fullname = details.FirstName + " " + details.LastName;
            ChangePasswordFormModel _form = new ChangePasswordFormModel();
            return View(_form);
        }
        [Route("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(string userid, ChangePasswordFormModel form)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {

                    var command = new ChangePasswordCommand
                    {
                        UserId = Convert.ToInt32(userid),
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
                            var userinfo = _userRepository.GetById(Convert.ToInt32(userid));
                            if (userinfo != null)
                            {
                                changepassowrd(userinfo.Email);
                            }
                            TempData["success"] = "User Password changed successfully.";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Model", "The current password is incorrect or the new password is invalid.");
                        int Userid = Convert.ToInt32(userid);
                        var details = _userRepository.Get(top => top.UserId == Userid);
                        ViewBag.UserId = details.UserId;
                        ViewBag.fullname = details.FirstName + " " + details.LastName;
                        return View("ChangePassword");
                    }
                }

            }
            return RedirectToAction("ChangePassword", new { @id = Convert.ToInt32(userid) });
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
        private Dictionary<string, IList<SelectListItem>> GetSelectLists()
        {
            var lists = new Dictionary<string, IList<SelectListItem>>();

            IList<SelectListItem> ageBracketList = new List<SelectListItem>();
            ageBracketList.Add(new SelectListItem { Text = "Under 18", Value = "0" });
            ageBracketList.Add(new SelectListItem { Text = "18-24", Value = "1" });
            ageBracketList.Add(new SelectListItem { Text = "25-34", Value = "2" });
            ageBracketList.Add(new SelectListItem { Text = "35-44", Value = "3" });
            ageBracketList.Add(new SelectListItem { Text = "45-54", Value = "4" });
            ageBracketList.Add(new SelectListItem { Text = "55-64", Value = "5" });
            ageBracketList.Add(new SelectListItem { Text = "65+", Value = "6" });
            ageBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "7" });

            IList<SelectListItem> genderList = new List<SelectListItem>();
            genderList.Add(new SelectListItem { Text = "Male", Value = "0" });
            genderList.Add(new SelectListItem { Text = "Female", Value = "1" });
            genderList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "2" });

            IList<SelectListItem> incomeBracketList = new List<SelectListItem>();
            incomeBracketList.Add(new SelectListItem { Text = "£0 to £14,999", Value = "0" });
            incomeBracketList.Add(new SelectListItem { Text = "£15,000 to £24,999", Value = "1" });
            incomeBracketList.Add(new SelectListItem { Text = "£25,000 to £39,999", Value = "2" });
            incomeBracketList.Add(new SelectListItem { Text = "£40,000 to £74,999", Value = "3" });
            incomeBracketList.Add(new SelectListItem { Text = "£75,000 to £99,999", Value = "4" });
            incomeBracketList.Add(new SelectListItem { Text = "£100,000+", Value = "5" });
            incomeBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "6" });

            IList<SelectListItem> workingStatusList = new List<SelectListItem>();
            workingStatusList.Add(new SelectListItem { Text = "Employed", Value = "0" });
            workingStatusList.Add(new SelectListItem { Text = "Self-Employed", Value = "1" });
            workingStatusList.Add(new SelectListItem { Text = "Housewife/Househusband", Value = "2" });
            workingStatusList.Add(new SelectListItem { Text = "Retired", Value = "3" });
            workingStatusList.Add(new SelectListItem { Text = "Unpaid Carer", Value = "4" });
            workingStatusList.Add(new SelectListItem { Text = "Full or Part-time Education", Value = "5" });
            workingStatusList.Add(new SelectListItem { Text = "Not Working", Value = "6" });
            workingStatusList.Add(new SelectListItem { Text = "None of these", Value = "7" });
            workingStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "8" });

            IList<SelectListItem> relationshipStatusList = new List<SelectListItem>();
            relationshipStatusList.Add(new SelectListItem { Text = "Divorced", Value = "0" });
            relationshipStatusList.Add(new SelectListItem { Text = "Living with another", Value = "1" });
            relationshipStatusList.Add(new SelectListItem { Text = "Married", Value = "2" });
            relationshipStatusList.Add(new SelectListItem { Text = "Separated", Value = "3" });
            relationshipStatusList.Add(new SelectListItem { Text = "Single", Value = "4" });
            relationshipStatusList.Add(new SelectListItem { Text = "Widowed", Value = "5" });
            relationshipStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "6" });

            IList<SelectListItem> educationList = new List<SelectListItem>();
            educationList.Add(new SelectListItem { Text = "Secondary", Value = "0" });
            educationList.Add(new SelectListItem { Text = "College", Value = "1" });
            educationList.Add(new SelectListItem { Text = "University", Value = "2" });
            educationList.Add(new SelectListItem { Text = "Post Graduate", Value = "3" });
            educationList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "4" });

            IList<SelectListItem> householdStatusList = new List<SelectListItem>();
            householdStatusList.Add(new SelectListItem { Text = "Rent", Value = "0" });
            householdStatusList.Add(new SelectListItem { Text = "Owner", Value = "1" });
            householdStatusList.Add(new SelectListItem { Text = "Live with someone", Value = "2" });
            householdStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "3" });

            //code commented on 30-03-2017
            //IList<SelectListItem> locationList = new List<SelectListItem>();
            //locationList.Add(new SelectListItem { Text = "London", Value = "0" });
            //locationList.Add(new SelectListItem { Text = "South East (excl. London)", Value = "1" });
            //locationList.Add(new SelectListItem { Text = "South West", Value = "2" });
            //locationList.Add(new SelectListItem { Text = "East Anglia", Value = "3" });
            //locationList.Add(new SelectListItem { Text = "Midlands", Value = "4" });
            //locationList.Add(new SelectListItem { Text = "Wales", Value = "5" });
            //locationList.Add(new SelectListItem { Text = "North West", Value = "6" });
            //locationList.Add(new SelectListItem { Text = "North East", Value = "7" });
            //locationList.Add(new SelectListItem { Text = "Scotland", Value = "8" });
            //locationList.Add(new SelectListItem { Text = "Northern Ireland", Value = "9" });

            lists.Add("ageBracketList", ageBracketList);
            lists.Add("genderList", genderList);
            lists.Add("incomeBracketList", incomeBracketList);
            lists.Add("workingStatusList", workingStatusList);
            lists.Add("relationshipStatusList", relationshipStatusList);
            lists.Add("educationList", educationList);
            lists.Add("householdStatusList", householdStatusList);
            //code commented on 30-03-2017
            //lists.Add("locationList", locationList);

            return lists;
        }

        [Route("UpdateUserInfo")]
        [HttpPost]
        public ActionResult UpdateUserInfo(UserProfileFormModel model, string userid)
        {
            try
            {


                if (ModelState.IsValid)
                {


                    CreateOrUpdateProfileCommand command =
                        Mapper.Map<UserProfileFormModel, CreateOrUpdateProfileCommand>(model);
                    command.UserProfileAdverts =
                        Mapper.Map
                            <ICollection<UserProfileAdvertFormModel>, ICollection<CreateOrUpdateUserProfileAdvertCommand>>(
                                model.UserProfileAdverts);
                    command.UserProfileAttitudes =
                        Mapper.Map
                            <ICollection<UserProfileAttitudeFormModel>,
                                ICollection<CreateOrUpdateUserProfileAttitudeCommand>>(model.UserProfileAttitudes);
                    command.UserProfileCinemas =
                        Mapper.Map
                            <ICollection<UserProfileCinemaFormModel>, ICollection<CreateOrUpdateUserProfileCinemaCommand>>(
                                model.UserProfileCinemas);
                    command.UserProfileInternets =
                        Mapper.Map
                            <ICollection<UserProfileInternetFormModel>,
                                ICollection<CreateOrUpdateUserProfileInternetCommand>>(model.UserProfileInternets);
                    command.UserProfileMobiles =
                        Mapper.Map
                            <ICollection<UserProfileMobileFormModel>, ICollection<CreateOrUpdateUserProfileMobileCommand>>(
                                model.UserProfileMobiles);
                    command.UserProfilePresses =
                        Mapper.Map
                            <ICollection<UserProfilePressFormModel>, ICollection<CreateOrUpdateUserProfilePressCommand>>(
                                model.UserProfilePresses);
                    command.UserProfileProductsServices =
                        Mapper.Map
                            <ICollection<UserProfileProductsServiceFormModel>,
                                ICollection<CreateOrUpdateUserProfileProductsServiceCommand>>(
                                    model.UserProfileProductsServices);
                    command.UserProfileRadios =
                        Mapper.Map
                            <ICollection<UserProfileRadioFormModel>, ICollection<CreateOrUpdateUserProfileRadioCommand>>(
                                model.UserProfileRadios);
                    command.UserProfileTimeSettings =
                        Mapper.Map
                            <ICollection<UserProfileTimeSettingFormModel>,
                                ICollection<CreateOrUpdateUserProfileTimeSettingCommand>>(model.UserProfileTimeSettings);
                    command.UserProfileTvs =
                        Mapper.Map<ICollection<UserProfileTvFormModel>, ICollection<CreateOrUpdateUserProfileTvCommand>>(
                            model.UserProfileTvs);

                    command.UserId = Convert.ToInt32(userid);

                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            TempData["success"] = "User profile updated successfully.";
                            return RedirectToAction("Index");
                        }
                    }
                }
                Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();
                // //code commented on 30-03-2017
                //model.LocationList = selectLists["locationList"];
                model.GenderList = selectLists["genderList"];
                model.IncomeBracketList = selectLists["incomeBracketList"];
                model.WorkingStatusList = selectLists["workingStatusList"];
                model.RelationshipStatusList = selectLists["relationshipStatusList"];
                model.EducationList = selectLists["educationList"];
                model.HouseholdStatusList = selectLists["householdStatusList"];
                return View("UserDetails", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Model", ex);
                return View("UserDetails", model);
            }
        }

        [Route("ApproveORSuspendUser")]
        public ActionResult ApproveORSuspendUser(int id, int status)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = status;
            ICommandResult result = _commandBus.Submit(command);
            var user = _userRepository.GetById(id);
            if (status == 1)
            {
                //TempData["status"] = "User is approved successfully.";
                //271191
                // CheckUserExistSoapApi(user);

                //Add 15-02-2019
                TempData["success"] = "User is approved successfully.";
            }
            else
            {
                TempData["status"] = "User is suspended successfully.";
                //271191
                //DeleteUserExistFromSoapApi(user);
            }
            return Json("success");
        }

        [Route("DeleteUser")]
        public ActionResult DeleteUser(int id)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = 3;
            ICommandResult result = _commandBus.Submit(command);
            TempData["status"] = "User is deleted successfully.";
            return RedirectToAction("Index");

        }

        [Route("ActivateUserSection")]
        public ActionResult ActivateUserSection(int id)
        {
            Session["userId"] = id;
            return Json("success");
        }

        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName, int?[] countryId, int?[] operatorId)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    var userdetails = _userRepository.AsQueryable().Where(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 2 && operatorId.Contains(top.OperatorId) && countryId.Contains(top.Operator.CountryId)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).OrderBy(r=>r.Name).Take(15).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        //Fill Country
        public void FillCountry()
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.countrydetails = new MultiSelectList(countrydetails, "Id", "Name");
            FillOperator(null);
        }

        //Fill Operator
        [HttpPost]
        [Route("FillOperator")]
        public ActionResult FillOperator(int?[] countryId)
        {
            if (countryId == null)
            {
                var operatordetails = _operatorRepository.GetMany(top => top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            else
            {
                var operatordetails = _operatorRepository.GetMany(top => countryId.Contains(top.CountryId) && top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            return Json(ViewBag.operatordetails);
        }

    }
}