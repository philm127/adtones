using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Models;
using EFMVC.Domain.Commands.OperatorAdmin;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.Web.Common;
using EFMVC.Core.Common;
using EFMVC.Domain.Commands.Contacts;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Core.Extensions;
using EFMVC.Model;
using EFMVC.Domain.Commands.Security;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("OperatorRegistration")]
    public class OperatorRegistrationController : Controller
    {
        // GET: Admin/OperatorRegistration

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _contacts repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _currency repository
        /// </summary>
        private readonly ICurrencyRepository _currencyRepository;

        /// <summary>
        /// The _userPasswordHistory repository
        /// </summary>
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public OperatorRegistrationController(ICommandBus commandBus, IUserRepository userRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository, IContactsRepository contactsRepository, ICurrencyRepository currencyRepository, IUserPasswordHistoryRepository userPasswordHistoryRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _contactsRepository = contactsRepository;
            _currencyRepository = currencyRepository;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
        }

        private const int PASSWORD_HISTORY_LIMIT = 8;

        //Index Page of Operator Admin
        [Route("Index")]
        public ActionResult Index()
        {
            List<OperatorRegistrationResult> _result = FillOperatorAdminResult();
            SearchClass.OperatorRegistrationFilter _filterCritearea = new SearchClass.OperatorRegistrationFilter();
            FillCountry();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 01-07-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<OperatorRegistrationResult> _result = new List<OperatorRegistrationResult>();
                IEnumerable<User> user = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                            searchValue = true;
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
                        if (columnSearch[0] != "null")
                        {
                            FirstName = columnSearch[0].ToString();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            LastName = columnSearch[1].ToString();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            Email = columnSearch[2].ToString();
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            Organisation = columnSearch[3].ToString();
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[4]))
                    {
                        if (columnSearch[4] != "null")
                        {
                            CountryId = columnSearch[4].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[4] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[5]))
                    {
                        if (columnSearch[5] != "null")
                        {
                            OperatorId = columnSearch[5].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[5] = null;
                        }
                    }

                    user = _userRepository.GetMany(top => top.RoleId == 6).OrderByDescending(top => top.DateCreated).ToList();
                    foreach (var item in user)
                    {
                        _result.Add(new OperatorRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, Organisation = item.Organisation == null ? "-" : item.Organisation, CountryId = (int)item.Operator.CountryId, CountryName = item.Operator.Country.Name, OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
                    }

                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.FirstName == FirstName).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => top.LastName == LastName).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        _result = _result.Where(top => top.Email == Email).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        _result = _result.Where(top => top.Organisation == Organisation).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        _result = _result.Where(top => (CountryId.Contains((int)top.CountryId))).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        _result = _result.Where(top => (OperatorId.Contains((int)top.OperatorId))).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    user = _userRepository.GetMany(top => top.RoleId == 6).OrderByDescending(top => top.DateCreated).ToList();
                    foreach (var item in user)
                    {
                        _result.Add(new OperatorRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, Organisation = item.Organisation == null ? "-" : item.Organisation, CountryId = (int)item.Operator.CountryId, CountryName = item.Operator.Country.Name, OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<OperatorRegistrationResult> result = new DTResult<OperatorRegistrationResult>
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

        //Add 01-07-2019
        // Function For Filter/Sorting OperatorRegistration Data
        private static List<OperatorRegistrationResult> ApplySorting(DTParameters param, List<OperatorRegistrationResult> result)
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
                        result = result.OrderBy(top => top.Organisation).ToList();
                    else
                        result = result.OrderByDescending(top => top.Organisation).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CountryName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CountryName).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.OperatorName).ToList();
                    else
                        result = result.OrderByDescending(top => top.OperatorName).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.IsActive).ToList();
                    else
                        result = result.OrderByDescending(top => top.IsActive).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        // Listing Operator Admin
        public List<OperatorRegistrationResult> FillOperatorAdminResult()
        {
            List<OperatorRegistrationResult> _result = new List<OperatorRegistrationResult>();
            var operatorRegistrationResult = _userRepository.GetMany(top => top.RoleId == 6).OrderByDescending(top => top.DateCreated).ToList();
            foreach (var item in operatorRegistrationResult)
            {
                _result.Add(new OperatorRegistrationResult { Id = item.UserId, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, Organisation = item.Organisation == null ? "-" : item.Organisation, CountryId = (int)item.Operator.CountryId, CountryName = item.Operator.Country.Name, OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, IsActive = item.Activated == 1 ? "True" : "False", CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), CreatedDateSort = item.DateCreated });
            }
            return _result;
        }

        //Add Operator Admin
        [Route("AddOperatorRegistration")]
        public ActionResult AddOperatorRegistration()
        {
            OperatorAdminFormModel operatorAdminFormModel = new OperatorAdminFormModel();
            FillCountry();
            FillOperatorForAdd(0);
            operatorAdminFormModel.Activated = 1;
            return View(operatorAdminFormModel);
        }

        //Save Operator Admin
        [Route("AddOperatorRegistration")]
        [HttpPost]
        public ActionResult AddOperatorRegistration(OperatorAdminFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userOperatorIdExist = _userRepository.GetMany(top => top.OperatorId == model.OperatorId && top.RoleId == (int)UserRole.OperatorAdmin).ToList();
                    var userExist = _userRepository.Get(top => top.Email.ToLower().Equals(model.Email.ToLower()));
                    var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(model.MobileNumber));
                    if (userOperatorIdExist.Count() > 0)
                    {
                        if (model.OperatorId != (int)OperatorTableId.Safaricom)
                        {
                            string operatorName = _operatorRepository.GetById(model.OperatorId).OperatorName;
                            TempData["Error"] = "Operator admin for " + operatorName + " already exists.";
                            FillCountry();
                            FillOperatorForAdd(model.CountryId);
                            return View("AddOperatorRegistration", model);
                        }
                    }
                    if (userExist != null)
                    {
                        TempData["Error"] = model.Email + " Email Exist.";
                        FillCountry();
                        FillOperatorForAdd(model.CountryId);
                        return View("AddOperatorRegistration", model);
                    }
                    if (contactExist != null)
                    {
                        TempData["Error"] = model.MobileNumber + " Mobile Number Exist.";
                        FillCountry();
                        FillOperatorForAdd(model.CountryId);
                        return View("AddOperatorRegistration", model);
                    }

                    string passwordHash = Md5Encrypt.Md5EncryptPassword(model.Password);

                    CreateOrUpdateOperatorAdminRegistrationCommand command =
                   Mapper.Map<OperatorAdminFormModel, CreateOrUpdateOperatorAdminRegistrationCommand>(model);

                    command.Email = model.Email;
                    command.FirstName = model.FirstName;
                    command.LastName = model.LastName;
                    command.Password = passwordHash;
                    command.DateCreated = DateTime.Now;
                    command.Organisation = model.Organisation;
                    command.LastLoginTime = DateTime.Now;
                    command.RoleId = (int)UserRole.OperatorAdmin;
                    command.Activated = model.Activated;
                    command.VerificationStatus = true;
                    command.Outstandingdays = 0;
                    command.OperatorId = model.OperatorId;
                    command.IsMsisdnMatch = true;
                    command.IsEmailVerfication = true;
                    command.PhoneticAlphabet = null;
                    command.IsMobileVerfication = true;
                    command.OrganisationTypeId = null;
                    command.UserMatchTableName = null;
                    command.LastPasswordChangedDate = DateTime.Now;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        CreateOrUpdateContactsCommand command1 =
                   Mapper.Map<OperatorAdminFormModel, CreateOrUpdateContactsCommand>(model);

                        var currencyId = _currencyRepository.Get(top => top.CountryId == model.CountryId).CurrencyId;
                        if (currencyId != 0)
                        {
                            if (currencyId == 13)
                            {
                                var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                command1.CurrencyId = currency.CurrencyId;
                            }
                            else
                            {
                                command1.CurrencyId = currencyId;
                            }
                        }
                        else
                        {
                            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                            command1.CurrencyId = currency.CurrencyId;
                        }

                        command1.UserId = result.Id;
                        command1.MobileNumber = model.MobileNumber;
                        command1.FixedLine = null;
                        command1.Email = model.Email;
                        command1.PhoneNumber = model.PhoneNumber;
                        command1.Address = model.Address;
                        command1.CountryId = model.CountryId;
                        ICommandResult result1 = _commandBus.Submit(command1);
                        if (result1.Success)
                        {
                            //SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.Password);
                            ////TempData["status"] = "Record added successfully.";
                            //TempData["status"] = "Operator Admin registered for Operator " + model.FirstName + " " + model.LastName;
                            //return RedirectToAction("Index");
                        }

                        UserPasswordHistoryFormModel formModel = new UserPasswordHistoryFormModel();
                        UserPasswordHistoryCommand command2 =
                   Mapper.Map<UserPasswordHistoryFormModel, UserPasswordHistoryCommand>(formModel);
                        command2.UserId = result.Id;
                        command2.PasswordHash = passwordHash;
                        command2.DateCreated = DateTime.Now;
                        ICommandResult result2 = _commandBus.Submit(command2);
                        if (result2.Success)
                        {
                            SendEmailVerificationCode(model.OperatorId, model.FirstName, model.LastName, model.Email, model.Password);
                            TempData["status"] = "Operator Admin registered for Operator " + model.FirstName + " " + model.LastName;
                            return RedirectToAction("Index");
                        }
                    }
                }
                FillCountry();
                FillOperatorForAdd(model.CountryId);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                FillCountry();
                FillOperatorForAdd(model.CountryId);
                return View("AddOperatorRegistration", model);
            }
        }

        //Edit Operator Admin
        [Route("UpdateOperatorRegistration")]
        public ActionResult UpdateOperatorRegistration(int id)
        {
            try
            {
                int countryId = 0;
                OperatorAdminFormModel model = new OperatorAdminFormModel();
                var userData = _userRepository.Get(top => top.UserId == id);
                var userContactData = _contactsRepository.Get(top => top.UserId == id);
                if (userContactData.CountryId != null)
                {
                    countryId = (int)userContactData.CountryId;
                }
                FillCountry();
                FillOperatorForAdd(countryId);
                model.UserId = userData.UserId;
                model.Email = userData.Email;
                model.FirstName = userData.FirstName;
                model.LastName = userData.LastName;
                model.Organisation = userData.Organisation;
                model.CountryId = countryId;
                model.OperatorId = userData.OperatorId;
                model.MobileNumber = userContactData.MobileNumber;
                model.PhoneNumber = userContactData.PhoneNumber;
                model.Address = userContactData.Address;
                model.Activated = userData.Activated;
                model.OldEmail = userData.Email;
                model.Password = userData.PasswordHash;
                model.LastPasswordChangedDate = userData.LastPasswordChangedDate;
                return View(model);
            }
            catch (Exception ex)
            {
                OperatorAdminFormModel model = new OperatorAdminFormModel();
                TempData["Error"] = ex.Message;
                FillCountry();
                FillOperatorForAdd(0);
                return View(model);
            }
        }

        //Update Operator Admin
        [Route("UpdateOperatorRegistration")]
        [HttpPost]
        public ActionResult UpdateOperatorRegistration(OperatorAdminFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Password != null && model.Password != "")
                    {
                        string passwordHash = Md5Encrypt.Md5EncryptPassword(model.Password);
                        if (IsPreviousPassword(model.UserId, passwordHash))
                        {
                            TempData["Error"] = "Cannot reuse old password.";
                            FillCountry();
                            FillOperatorForAdd(model.CountryId);
                            return View("UpdateOperatorRegistration", model);
                        }
                    }

                    var userOperatorIdExist = _userRepository.GetMany(top => top.OperatorId == model.OperatorId && top.RoleId == (int)UserRole.OperatorAdmin && top.UserId != model.UserId).ToList();
                    var userExist = _userRepository.Get(top => top.Email.ToLower().Equals(model.Email.ToLower()) && top.UserId != model.UserId);
                    var contactExist = _contactsRepository.Get(top => top.MobileNumber.Equals(model.MobileNumber) && top.UserId != model.UserId);
                    if (userOperatorIdExist.Count() > 0)
                    {
                        if (model.OperatorId != (int)OperatorTableId.Safaricom)
                        {
                            string operatorName = _operatorRepository.GetById(model.OperatorId).OperatorName;
                            TempData["Error"] = "Operator admin for " + operatorName + " already exists.";
                            FillCountry();
                            FillOperatorForAdd(model.CountryId);
                            return View("UpdateOperatorRegistration", model);
                        }
                    }
                    if (userExist != null)
                    {
                        TempData["Error"] = model.Email + " Email Exist.";
                        FillCountry();
                        FillOperatorForAdd(model.CountryId);
                        return View("UpdateOperatorRegistration", model);
                    }
                    if (contactExist != null)
                    {
                        TempData["Error"] = model.MobileNumber + " Mobile Number Exist.";
                        FillCountry();
                        FillOperatorForAdd(model.CountryId);
                        return View("UpdateOperatorRegistration", model);
                    }

                    var userData = _userRepository.GetById(model.UserId);

                    CreateOrUpdateOperatorAdminRegistrationCommand command =
                   Mapper.Map<OperatorAdminFormModel, CreateOrUpdateOperatorAdminRegistrationCommand>(model);

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
                    command.LastPasswordChangedDate = model.Password == null ? userData.LastPasswordChangedDate : DateTime.Now;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        var userContactData = _contactsRepository.Get(top => top.UserId == model.UserId);

                        CreateOrUpdateContactsCommand command1 =
                   Mapper.Map<OperatorAdminFormModel, CreateOrUpdateContactsCommand>(model);

                        var currencyId = _currencyRepository.Get(top => top.CountryId == model.CountryId).CurrencyId;
                        if (currencyId != 0)
                        {
                            if (currencyId == 13)
                            {
                                var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR"));
                                command1.CurrencyId = currency.CurrencyId;
                            }
                            else
                            {
                                command1.CurrencyId = currencyId;
                            }
                        }
                        else
                        {
                            var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                            command1.CurrencyId = currency.CurrencyId;
                        }

                        command1.Id = userContactData.Id;
                        command1.UserId = userContactData.UserId;
                        command1.MobileNumber = model.MobileNumber;
                        command1.FixedLine = userContactData.FixedLine;
                        command1.Email = model.Email;
                        command1.PhoneNumber = model.PhoneNumber == "" ? userContactData.PhoneNumber : model.PhoneNumber;
                        command1.Address = model.Address == "" ? userContactData.Address : model.Address;
                        command1.CountryId = model.CountryId;
                        ICommandResult result1 = _commandBus.Submit(command1);
                        if (result1.Success)
                        {
                            if (model.Password != null && model.Password != "")
                            {
                                var password = Md5Encrypt.Md5EncryptPassword(model.Password);

                                UserPasswordHistoryFormModel formModel = new UserPasswordHistoryFormModel();
                                UserPasswordHistoryCommand command2 =
                           Mapper.Map<UserPasswordHistoryFormModel, UserPasswordHistoryCommand>(formModel);
                                command2.UserId = userContactData.UserId;
                                command2.PasswordHash = password;
                                command2.DateCreated = DateTime.Now;
                                ICommandResult result2 = _commandBus.Submit(command2);
                                if (result2.Success)
                                {
                                    if (model.OldEmail != model.Email || model.OldPassword != password)
                                    {
                                        SendEmailVerificationCode(model.OperatorId, model.FirstName, model.LastName, model.Email, model.Password);
                                    }
                                }
                            }
                            //TempData["status"] = "Record updated successfully.";
                            TempData["status"] = "Operator Admin details updated for Operator " + model.FirstName + " " + model.LastName;
                            return RedirectToAction("Index");
                        }
                    }
                }
                FillCountry();
                FillOperatorForAdd(model.CountryId);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                FillCountry();
                FillOperatorForAdd(model.CountryId);
                return View("UpdateOperatorRegistration", model);
            }
        }

        //Search  Operator Admin
        [Route("SearchOperatorRegistration")]
        public ActionResult SearchOperatorRegistration([Bind(Prefix = "Item2")]SearchClass.OperatorRegistrationFilter _filterCritearea, int[] CountryId, int[] OperatorId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<OperatorRegistrationResult> _result = new List<OperatorRegistrationResult>();
                var finalresult = new List<OperatorRegistrationResult>();
                if (_filterCritearea != null)
                {
                    _result = FillOperatorAdminResult();
                    finalresult = getOperatorAdminRegistrationInformationResult(_result, _filterCritearea, CountryId, OperatorId);
                }
                else
                {
                    _result = FillOperatorAdminResult();
                    finalresult = getOperatorAdminRegistrationInformationResult(_result, _filterCritearea, CountryId, OperatorId);
                }

                return PartialView("_OperatorRegistrationDetails", finalresult);
            }
            else
            {
                return PartialView("_OperatorRegistrationDetails", "notauthorise");
            }
        }

        //Search  Operator Admin
        public List<OperatorRegistrationResult> getOperatorAdminRegistrationInformationResult(List<OperatorRegistrationResult> operatorAdminRegistrationInformationresult, SearchClass.OperatorRegistrationFilter _filterCritearea, int[] CountryId, int[] OperatorId)
        {
            if (operatorAdminRegistrationInformationresult != null && _filterCritearea != null)
            {
                if (CountryId != null)
                {
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if (OperatorId != null)
                {
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
                if ((_filterCritearea.FirstName != null))
                {
                    //operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.FirstName == _filterCritearea.FirstName).ToList();
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.FirstName.ToLower().Contains(_filterCritearea.FirstName.ToLower())).ToList();
                }
                if ((_filterCritearea.LastName != null))
                {
                    //operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.LastName == _filterCritearea.LastName).ToList();
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.LastName.ToLower().Contains(_filterCritearea.LastName.ToLower())).ToList();
                }
                if ((_filterCritearea.Email != null))
                {
                    //operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.Email == _filterCritearea.Email).ToList();
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.Email.ToLower().Contains(_filterCritearea.Email)).ToList();
                }
                if ((_filterCritearea.Organisation != null))
                {
                    //operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.Organisation == _filterCritearea.Organisation).ToList();
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => top.Organisation.ToLower().Contains(_filterCritearea.Organisation.ToLower())).ToList();
                }

            }
            else
            {
                if (CountryId != null)
                {
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if (OperatorId != null)
                {
                    operatorAdminRegistrationInformationresult = operatorAdminRegistrationInformationresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
            }
            return operatorAdminRegistrationInformationresult;
        }

        //Send Email to Operator Admin
        private void SendEmailVerificationCode(int OperatorId, string firstName, string LastName, string email, string password)
        {
            string safaricomOperatorAdminSiteAddress = "";
            string subject = "Operator Registration";
            string siteAddress = ConfigurationManager.AppSettings["siteAddress"];
            string url = "";

            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            var reader = new StreamReader(
                                   Server.MapPath(ConfigurationManager.AppSettings["OperatorAdminRegistrationEmailTemplete"]));
            string emailContent = reader.ReadToEnd();

            if (OperatorId == (int)OperatorTableId.Safaricom)
            {
                safaricomOperatorAdminSiteAddress = ConfigurationManager.AppSettings["SafaricomOperatorAdminSiteAddress"].ToString();
                url = safaricomOperatorAdminSiteAddress + "OperatorAdmin/Login/Index";
                url = "<a href='" + url + "'>" + url + " </a>";

                mail.To.Add(email);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SafaricomSiteEmailAddress"]);

                emailContent = string.Format(emailContent, firstName, LastName, url, email, password);

                mail.Subject = subject;
                mail.Body = emailContent.Replace("\n", "<br/>");
                mail.IsBodyHtml = true;

                smtp.Host = ConfigurationManager.AppSettings["SafaricomSmtpServerAddress"]; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (ConfigurationManager.AppSettings["SafaricomSMTPEmail"].ToString(), ConfigurationManager.AppSettings["SafaricomSMTPPassword"].ToString()); // ***use valid credentials***
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SafaricomSmtpServerPort"]);

                //Or your Smtp Email ID and Password
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SafaricomEnableEmailSending"].ToString());
            }
            else
            {
                url = siteAddress + "Admin/Login/Index";
                url = "<a href='" + url + "'>" + url + " </a>";

                mail.To.Add(email);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);

                emailContent = string.Format(emailContent, firstName, LastName, url, email, password);

                mail.Subject = subject;
                mail.Body = emailContent.Replace("\n", "<br/>");
                mail.IsBodyHtml = true;

                smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                //Or your Smtp Email ID and Password
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
            }
            
            smtp.Send(mail);
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

        //Fill Operator
        [HttpPost]
        [Route("FillOperatorForAdd")]
        public ActionResult FillOperatorForAdd(int countryId)
        {
            if (countryId == 0)
            {
                List<SelectListItem> operatordetails = new List<SelectListItem>();
                operatordetails.Insert(0, (new SelectListItem { Text = "--Select Operator--", Value = "0" }));
                ViewBag.operatordetails = operatordetails;
            }
            else
            {
                List<SelectListItem> operatordetails = new List<SelectListItem>();
                operatordetails = _operatorRepository.GetMany(top => top.CountryId == countryId && top.IsActive == true).Select(top => new SelectListItem
                {
                    Text = top.OperatorName,
                    Value = top.OperatorId.ToString()
                }).ToList();
                operatordetails.Insert(0, new SelectListItem { Text = "--Select Operator--", Value = "0" });
                ViewBag.operatordetails = operatordetails;
            }
            return Json(ViewBag.operatordetails);
        }

        private bool IsPreviousPassword(int userId, string newPassword)
        {
            var userPasswordHistory = _userPasswordHistoryRepository.GetMany(top => top.UserId == userId).OrderByDescending(top=>top.UserPasswordHistoryId).Select(top => top.PasswordHash).Take(PASSWORD_HISTORY_LIMIT);

            if (userPasswordHistory.Contains(newPassword)) return true;
            else return false;

            //if (user.PreviousUserPasswords.OrderByDescending(x => x.CreateDate).
            // Select(x => x.PasswordHash).Take(PASSWORD_HISTORY_LIMIT).Where(x => PasswordHasher.VerifyHashedPassword(x, newPassword) != PasswordVerificationResult.Failed).Any())
            //{
            //    return true;
            //}
            //return false;
        }
    }
}