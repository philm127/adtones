using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Domain.Commands.Security;
using EFMVC.Web.ViewModels;
using AutoMapper;
using EFMVC.Model;
using EFMVC.Domain.Commands.CompanyDetails;
using EFMVC.Domain.Commands.Contacts;
using System.Globalization;
using EFMVC.Web.Models;
using EFMVC.Web.Common;
using Microsoft.Ajax.Utilities;

namespace EFMVC.Web.Areas.Admin.Controllers
{

    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("UserManagement")]
    public class UserManagementController : Controller
    {
        //
        // GET: /Admin/UserManagement/
        //
        // GET: /AdminQuestion/

        /// <summary>
        /// The _client repository
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

        private readonly IBillingRepository _billingRepository;

        private readonly IUsersCreditPaymentRepository _usercreditpaymentRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserManagementController(ICommandBus commandBus, IUserRepository userRepository, IContactsRepository contactsRepository, ICompanyDetailsRepository companydetailsRepository, ICountryRepository countryRepository, IBillingRepository billingRepository, IUsersCreditPaymentRepository usercreditpaymentRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _contactsRepository = contactsRepository;
            _companydetailsRepository = companydetailsRepository;
            _countryRepository = countryRepository;
            _billingRepository = billingRepository;
            _usercreditpaymentRepository = usercreditpaymentRepository;
        }
        private void FillCountryList()
        {

            var country = (from action in _countryRepository.GetAll()
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();
            ViewBag.country = country;
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
            //List<UserResult> _result = FillUserResult();
            List<UserResult> _result = new List<UserResult>();
            FillUserStatus();
            SearchClass.UserFilter _filterCritearea = new SearchClass.UserFilter();
            //var result = _userRepository.GetAll().Where(top => top.VerificationStatus == true && (top.RoleId == 1 || top.RoleId == 3)).OrderByDescending(top => top.DateCreated);
            //var userdropdown = (from item in result
            //                    select new
            //                    {
            //                        Id = item.UserId,
            //                        Name = item.DisplayName
            //                    }).ToList();
            //ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");
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

        //Add 28-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                List<UserResult> _result = new List<UserResult>();
                IEnumerable<User> user = null;
                //DateTimeFormat dateTimeConvert = new DateTimeFormat();
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
                    int?[] UserId = new int?[cnt];
                    int[] StatusId = new int[cnt];
                    string Email = string.Empty;
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();
                    
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            Email = columnSearch[0].ToString();
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
                            UserId = columnSearch[1].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[8]))
                    {
                        if (columnSearch[8] != "null")
                        {
                            var data = columnSearch[2].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[8] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[9]))
                    {
                        if (columnSearch[9] != "null")
                        {
                            StatusId = columnSearch[4].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[9] = null;
                        }
                    }

                    user = _userRepository.GetAll().Where(top => top.VerificationStatus == true && (top.RoleId == 1 || top.RoleId == 3)).OrderByDescending(top => top.UserId);
                    foreach (var item in user)
                    {
                        double creditlimit = 0;
                        //double outstandinginvoice = item.UsersCreditPayment.Count();

                        List<UserCreditPaymentResult> _usercreditResult = new List<UserCreditPaymentResult>();
                        var result1 = _billingRepository.GetMany(s => s.PaymentMethodId == 1).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                        foreach (var item1 in result1)
                        {
                            var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == item1.CampaignProfileId).Sum(top => top.FundAmount);
                            var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).Sum(s => s.Amount);
                            var outStandingAmount = totalAmount - paidAmount;
                            var clientId = item1.ClientId == null ? 0 : item1.ClientId;
                            var description = "-";
                            var userCreditPaymentData = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).OrderByDescending(s => s.Id).FirstOrDefault();
                            if (userCreditPaymentData != null)
                            {
                                description = userCreditPaymentData.Description;
                            }
                            if (outStandingAmount > 0)
                            {
                                _usercreditResult.Add(new UserCreditPaymentResult
                                {
                                    Id = item1.Id,
                                    UserId = (int)item1.UserId,
                                });
                            }
                        }

                        double outstandinginvoice = _usercreditResult.Where(top => top.UserId == item.UserId).Count();

                        if (item.UsersCredit.Count() > 0)
                        {
                            creditlimit = Convert.ToDouble(item.UsersCredit.FirstOrDefault().AssignCredit);
                        }
                        var role = (UserRole)item.RoleId;
                        var fstatus = (UserStatus)item.Activated;
                        int[] campaignStatus = new int[4] { 1, 2, 3, 4 };
                        _result.Add(new UserResult { Id = item.UserId, RoleId = item.RoleId, Role = role.ToString(), Email = item.Email, NoOfactivecampaign = item.CampaignProfiles.Where(top => campaignStatus.Contains(top.Status)).Count(), NoOfunapprovedadverts = item.Adverts.Where(top => top.Status == 4).Count(), Creditlimit = creditlimit, Outstandinginvoice = outstandinginvoice, CreatedDateSort = item.DateCreated, CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), FirstName = item.FirstName, LastName = item.LastName, status = item.Activated, fstatus = fstatus.ToString(), Name = item.FirstName + " " + item.LastName, TicketCount = item.Questions.Where(top => top.Status == 1 || top.Status == 2).Count() });
                    }
                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.Email == Email).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => (UserId.Contains(Convert.ToInt32(top.Id)))).ToList();
                    }
                    if (columnSearch[8] != null)
                    {
                        _result = _result.Where(top => (top.CreatedDateSort >= CreatedDatefromdate && top.CreatedDateSort <= CreatedDatetodate)).ToList();
                    }
                    if (columnSearch[9] != null)
                    {
                        _result = _result.Where(top => (StatusId.Contains((int)top.status))).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    user = _userRepository.GetAll().Where(top => top.VerificationStatus == true && (top.RoleId == 1 || top.RoleId == 3)).OrderByDescending(top => top.UserId);
                    foreach (var item in user)
                    {
                        double creditlimit = 0;
                        //double outstandinginvoice = item.UsersCreditPayment.Count();
                        List<UserCreditPaymentResult> _usercreditResult = new List<UserCreditPaymentResult>();
                        var result1 = _billingRepository.GetMany(s => s.PaymentMethodId == 1).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                        foreach (var item1 in result1)
                        {
                            var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == item1.CampaignProfileId).Sum(top => top.FundAmount);
                            var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).Sum(s => s.Amount);
                            var outStandingAmount = totalAmount - paidAmount;
                            var clientId = item1.ClientId == null ? 0 : item1.ClientId;
                            var description = "-";
                            var userCreditPaymentData = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).OrderByDescending(s => s.Id).FirstOrDefault();
                            if (userCreditPaymentData != null)
                            {
                                description = userCreditPaymentData.Description;
                            }
                            if (outStandingAmount > 0)
                            {
                                _usercreditResult.Add(new UserCreditPaymentResult
                                {
                                    Id = item1.Id,
                                    UserId = (int)item1.UserId,
                                });
                            }
                        }

                        double outstandinginvoice = _usercreditResult.Where(top => top.UserId == item.UserId).Count();

                        if (item.UsersCredit.Count() > 0)
                        {
                            creditlimit = Convert.ToDouble(item.UsersCredit.FirstOrDefault().AssignCredit);
                        }
                        var role = (UserRole)item.RoleId;
                        var fstatus = (UserStatus)item.Activated;
                        int[] campaignStatus = new int[4] { 1, 2, 3, 4 };
                        _result.Add(new UserResult { Id = item.UserId, RoleId = item.RoleId, Role = role.ToString(), Email = item.Email, NoOfactivecampaign = item.CampaignProfiles.Where(top => campaignStatus.Contains(top.Status)).Count(), NoOfunapprovedadverts = item.Adverts.Where(top => top.Status == 4).Count(), Creditlimit = creditlimit, Outstandinginvoice = outstandinginvoice, CreatedDateSort = item.DateCreated, CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), FirstName = item.FirstName, LastName = item.LastName, status = item.Activated, fstatus = fstatus.ToString(), Name = item.FirstName + " " + item.LastName, TicketCount = item.Questions.Where(top => top.Status == 1 || top.Status == 2).Count() });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserResult> result = new DTResult<UserResult>
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

        //Add 28-06-2019
        // Function For Filter/Sorting User Data
        private static List<UserResult> ApplySorting(DTParameters param, List<UserResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Email).ToList();
                    else
                        result = result.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Role).ToList();
                    else
                        result = result.OrderByDescending(top => top.Role).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.NoOfactivecampaign).ToList();
                    else
                        result = result.OrderByDescending(top => top.NoOfactivecampaign).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.NoOfunapprovedadverts).ToList();
                    else
                        result = result.OrderByDescending(top => top.NoOfunapprovedadverts).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TicketCount).ToList();
                    else
                        result = result.OrderByDescending(top => top.TicketCount).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Creditlimit).ToList();
                    else
                        result = result.OrderByDescending(top => top.Creditlimit).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Outstandinginvoice).ToList();
                    else
                        result = result.OrderByDescending(top => top.Outstandinginvoice).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fstatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fstatus).ToList();
                }
            }
            return result;
        }

        public List<UserResult> FillUserResult()
        {
            List<UserResult> _userResult = new List<UserResult>();

            var result = _userRepository.GetAll().Where(top => top.VerificationStatus == true && (top.RoleId == 1 || top.RoleId == 3)).OrderByDescending(top => top.UserId);
            foreach (var item in result)
            {
                double creditlimit = 0;
                //double outstandinginvoice = item.UsersCreditPayment.Count();
                List<UserCreditPaymentResult> _usercreditResult = new List<UserCreditPaymentResult>();
                var result1 = _billingRepository.GetMany(s => s.PaymentMethodId == 1).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                foreach (var item1 in result1)
                {
                    var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == item1.CampaignProfileId).Sum(top => top.FundAmount);
                    var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).Sum(s => s.Amount);
                    var outStandingAmount = totalAmount - paidAmount;
                    var clientId = item1.ClientId == null ? 0 : item1.ClientId;
                    var description = "-";
                    var userCreditPaymentData = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item1.CampaignProfileId).OrderByDescending(s => s.Id).FirstOrDefault();
                    if (userCreditPaymentData != null)
                    {
                        description = userCreditPaymentData.Description;
                    }
                    if (outStandingAmount > 0)
                    {
                        _usercreditResult.Add(new UserCreditPaymentResult
                        {
                            Id = item1.Id,
                            UserId = (int)item1.UserId,
                        });
                    }
                }
                double outstandinginvoice = _usercreditResult.Where(top => top.UserId == item.UserId).Count();

                if (item.UsersCredit.Count() > 0)
                {
                    creditlimit = Convert.ToDouble(item.UsersCredit.FirstOrDefault().AssignCredit);
                }
                var role = (UserRole)item.RoleId;
                var fstatus = (UserStatus)item.Activated;
                int[] campaignStatus = new int[4] { 1, 2, 3, 4 };
                //_userResult.Add(new UserResult { Id = item.UserId, RoleId = item.RoleId, Role = role.ToString(), Email = item.Email, NoOfactivecampaign = item.CampaignProfiles.Where(top => top.Status != 5).Count(), NoOfunapprovedadverts = item.Adverts.Where(top => top.Status == 4).Count(), Creditlimit = creditlimit, Outstandinginvoice = outstandinginvoice, CreatedDateSort = item.DateCreated, CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), FirstName = item.FirstName, LastName = item.LastName, status = item.Activated, fstatus = fstatus.ToString(), Name = item.FirstName + " " + item.LastName, TicketCount = item.Questions.Where(top => top.Status == 1 || top.Status == 2).Count() });
                _userResult.Add(new UserResult { Id = item.UserId, RoleId = item.RoleId, Role = role.ToString(), Email = item.Email, NoOfactivecampaign = item.CampaignProfiles.Where(top => campaignStatus.Contains(top.Status)).Count(), NoOfunapprovedadverts = item.Adverts.Where(top => top.Status == 4).Count(), Creditlimit = creditlimit, Outstandinginvoice = outstandinginvoice, CreatedDateSort = item.DateCreated, CreatedDate = item.DateCreated.ToString("dd/MM/yyyy"), FirstName = item.FirstName, LastName = item.LastName, status = item.Activated, fstatus = fstatus.ToString(), Name = item.FirstName + " " + item.LastName, TicketCount = item.Questions.Where(top => top.Status == 1 || top.Status == 2).Count() });
            }
            var userdropdown = (from item in _userResult
                                select new
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                }).ToList();
            ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");
            return _userResult;
        }
        [Route("UserDetails")]
        public ActionResult UserDetails(int id)
        {
            FillCountryList();
            AdminAccountInfo _accountInfo = new AdminAccountInfo();

            UsersAdmin.ViewModel.UserProfile _profile = new UsersAdmin.ViewModel.UserProfile();
            ContactsFormModel contactmodel = new ContactsFormModel();
            CompanyDetailsFormModel companymodel = new CompanyDetailsFormModel();
            var details = _userRepository.Get(top => top.UserId == id);
            ViewBag.fullname = null;
            if (details != null)
            {
                FillUserRole();
                _profile.Id = details.UserId;
                contactmodel.UserId = details.UserId;
                companymodel.UserId = details.UserId;
                _profile.RoleId = details.RoleId;
                _profile.Email = details.Email;
                _profile.Fname = details.FirstName;
                _profile.Lname = details.LastName;
                _profile.Outstandingdays = details.Outstandingdays;
                ViewBag.fullname = details.FirstName + " " + details.LastName;
                _profile.Organisation = details.Organisation;
                ViewBag.userinfo = _profile;

            }
          


            
            var _contactinfo = _contactsRepository.GetMany(top => top.UserId == id).FirstOrDefault();
            if (_contactinfo != null)
            {
                contactmodel = Mapper.Map<Contacts, ContactsFormModel>(_contactinfo);
                ViewBag.contactId = contactmodel.Id;
                _accountInfo.ContactsFormModel = contactmodel;
            }
            else
            {
                ViewBag.contactId = 0;
                _accountInfo.ContactsFormModel = contactmodel;
            }
            var _companyInfo = _companydetailsRepository.GetMany(top => top.UserId == id).FirstOrDefault();
            if (_companyInfo != null)
            {
                companymodel = Mapper.Map<CompanyDetails, CompanyDetailsFormModel>(_companyInfo);
                ViewBag.companyId = companymodel.Id;
                _accountInfo.CompanyDetailsFormModel = companymodel;

                _accountInfo.CompanyDetailsFormModel.CountryId = companymodel.CountryId;
            }
            else
            {
                ViewBag.companyId = 0;
                _accountInfo.CompanyDetailsFormModel = companymodel;
            }
          
            _accountInfo.UserProfileInfo = _profile;
            return View(_accountInfo);
        }
        [Route("SaveContactInfo")]
        [HttpPost]
        public ActionResult SaveContactInfo(ContactsFormModel ContactsFormModel, string contactId)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        ContactsFormModel.Id = Convert.ToInt32(contactId);
                        CreateOrUpdateContactsCommand command =
                            Mapper.Map<ContactsFormModel, CreateOrUpdateContactsCommand>(ContactsFormModel);
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
        [Route("SaveCompanyInfo")]
        [HttpPost]
        public ActionResult SaveCompanyInfo(CompanyDetailsFormModel CompanyFormModel,string companyId)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        CompanyFormModel.Id = Convert.ToInt32(companyId);
                        CreateOrUpdateCompanyDetailsCommand command =
                            Mapper.Map<CompanyDetailsFormModel, CreateOrUpdateCompanyDetailsCommand>(CompanyFormModel);
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
        [Route("UpdateUserInfo")]
        [HttpPost]
        public ActionResult UpdateUserInfo(UsersAdmin.ViewModel.UserProfile _model)
        {
            if (ModelState.IsValid)
            {
                ChangeUserRoleCommand command = new ChangeUserRoleCommand();
                command.Fname = _model.Fname;
                command.Lname = _model.Lname;
                command.Organisation = _model.Organisation;
                command.UserId = _model.Id;
                command.RoleId = _model.RoleId;
                command.Outstandingdays = _model.Outstandingdays;
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["success"] = "User Role updated successfully.";
                }

            }

            return RedirectToAction("UserDetails", new { @id = _model.Id });
        }

        [Route("ApproveORSuspendUser")]
        public ActionResult ApproveORSuspendUser(int id, int status)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = status;
            ICommandResult result = _commandBus.Submit(command);
            var userName = _userRepository.GetById(id);
            if (status == 1)
            {
                //TempData["status"] = "User is approved successfully.";
                TempData["status"] = userName.FirstName + " " + userName.LastName + " is approved successfully.";
            }
            else
            {
                //TempData["status"] = "User is suspended successfully.";
                TempData["status"] = userName.FirstName + " " + userName.LastName + " is suspended successfully.";
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

        [Route("SearchUsers")]
        public ActionResult SearchUsers([Bind(Prefix = "Item2")]SearchClass.UserFilter _filterCritearea, int[] UserId, int[] UserStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserResult> _result = new List<UserResult>();
                var finalresult = new List<UserResult>();
                if (_filterCritearea != null)
                {
                    _result = FillUserResult();
                    finalresult = getuserResult(_result, _filterCritearea, UserId, UserStatusId);
                }
                else
                {
                    UserStatusId = new int[] { 1 };
                    _result = FillUserResult();
                    finalresult = getuserResult(_result, _filterCritearea, UserId, UserStatusId);
                }

                return PartialView("_UserDetails", finalresult);
            }
            else
            {
                return PartialView("_UserDetails", "notauthorise");
            }
        }
        public List<UserResult> getuserResult(List<UserResult> userresult, SearchClass.UserFilter _filterCritearea, int[] UserId, int[] UserStatusId)
        {
            if (userresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.Email))
                {
                    userresult = userresult.Where(top => top.Email.Contains(_filterCritearea.Email)).ToList();

                }
              
                if (UserId != null)
                {
                    userresult = userresult.Where(top => UserId.Contains(top.Id)).ToList();
                }
                if (UserStatusId != null)
                {
                    userresult = userresult.Where(top => UserStatusId.Contains(top.status)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {

                    //userresult = userresult.Where(top => top.CreatedDate.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    userresult = userresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();
                }
            }
            else
            {
                if (UserId != null)
                {
                    userresult = userresult.Where(top => UserId.Contains(top.Id)).ToList();
                }
                if (UserStatusId != null)
                {
                    //userresult = userresult.Where(top => UserStatusId.Contains(top.status)).ToList();
                    userresult = userresult.ToList();
                }
            }
            return userresult;
        }
    }
}
