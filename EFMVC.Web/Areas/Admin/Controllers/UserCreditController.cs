using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("AdvertiserCredit")]
    public class UserCreditController : Controller
    {
        //
        // GET: /Admin/UserCredit/

        // GET: /AdminQuestion/

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _usercredit repository
        /// </summary>
        private readonly IUsersCreditRepository _usercreditRepository;

        private readonly IBillingRepository _billingRepository;
        /// <summary>
        /// The _usercreditpayment repository
        /// </summary>
        private readonly IUsersCreditPaymentRepository _usercreditpaymentRepository;

        private readonly IContactsRepository _contactsRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public UserCreditController(ICommandBus commandBus, IUserRepository userRepository, IUsersCreditRepository usercreditRepository, IUsersCreditPaymentRepository usercreditpaymentRepository, IBillingRepository billingRepository, IContactsRepository contactsRepository, ICurrencyRepository currencyRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _usercreditRepository = usercreditRepository;
            _usercreditpaymentRepository = usercreditpaymentRepository;
            _billingRepository = billingRepository;
            _contactsRepository = contactsRepository;
            _currencyRepository = currencyRepository;
            _countryRepository = countryRepository;
        }

        [Route("AddCredit")]
        public ActionResult AddCredit()
        {
            UserCreditFormModel _usercredit = new UserCreditFormModel();
            var result = _usercreditRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            var usercreditResult = (from item in result
                                    select new
                                    {
                                        Id = item.UserId,
                                        Name = item.User.FirstName + " " + item.User.LastName + "(" + item.User.Email + ")"
                                    }).ToList();
            var userlist = _userRepository.GetAll().Where(top => top.VerificationStatus == true && top.Activated == 1 && top.RoleId == 3).ToList();
            var userlistResult = (from item in userlist
                                  select new
                                  {
                                      Id = item.UserId,
                                      Name = item.FirstName + " " + item.LastName + "(" + item.Email + ")"
                                  }).ToList();
            var finalList = userlistResult.Except(usercreditResult).ToList();
            ViewBag.userdetails = (from item in finalList
                                   select new SelectListItem
                                   {
                                       Value = item.Id.ToString(),
                                       Text = item.Name
                                   }).ToList();
            var countryList = _countryRepository.GetAll().OrderBy(top => top.Name);
            ViewBag.countrydetails = (from item in countryList
                                      select new SelectListItem
                                      {
                                          Value = item.Id.ToString(),
                                          Text = item.Name
                                      }).ToList();
            FillCurrencyList(0);
            return View(_usercredit);
        }
        [Route("AddCredit")]
        [HttpPost]
        public ActionResult AddCredit(UserCreditFormModel _creditmodel)
        {
            if (ModelState.IsValid)
            {
                UsersCreditFormModel _usercredit = new UsersCreditFormModel();
                var userName = _userRepository.GetById(_creditmodel.UserId);
                var userCredits = _usercreditRepository.GetMany(top => top.UserId == _creditmodel.UserId).ToList();
                if (userCredits.Count() > 0)
                {
                    _usercredit.Id = userCredits.FirstOrDefault().Id;
                    _usercredit.AssignCredit = userCredits.FirstOrDefault().AssignCredit + _creditmodel.AssignCredit;
                    _usercredit.AvailableCredit = userCredits.FirstOrDefault().AvailableCredit + _creditmodel.AssignCredit;
                }
                else
                {
                    _usercredit.Id = 0;
                    _usercredit.AssignCredit = _creditmodel.AssignCredit;
                    _usercredit.AvailableCredit = _creditmodel.AssignCredit;
                }
                _usercredit.UserId = _creditmodel.UserId;
                _usercredit.CreatedDate = DateTime.Now;
                _usercredit.UpdatedDate = DateTime.Now;
                _usercredit.CurrencyId = _creditmodel.CurrencyId;
                CreateOrUpdateUsersCreditCommand command =
                Mapper.Map<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>(_usercredit);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Assign credit to the users successfully.";
                    TempData["status"] = "Assigned credit to " + userName.FirstName + " " + userName.LastName + " successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(_creditmodel);
        }
        [HttpGet]
        [Route("CreditDetails")]
        [Route("{id}/{userId}")]
        public ActionResult CreditDetails(int id, int? userId)
        {

            UsersCredit usercredit = new UsersCredit();
            if (userId != null)
            {
                usercredit = _usercreditRepository.Get(top => top.UserId == userId);
                ViewBag.username = usercredit.User.FirstName + " " + usercredit.User.LastName;
            }
            else
            {
                usercredit = _usercreditRepository.GetById(id);
                ViewBag.username = usercredit.User.FirstName + " " + usercredit.User.LastName;
            }
            UserCreditFormModel _usercredit = new UserCreditFormModel();
            FillCurrencyList(usercredit.CurrencyId);
            if (usercredit != null)
            {

                _usercredit.Id = usercredit.Id;
                _usercredit.AssignCredit = usercredit.AssignCredit;
                _usercredit.UserId = usercredit.UserId;
                _usercredit.CurrencyId = usercredit.CurrencyId;
                var usercreditpayment = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == usercredit.UserId).ToList();
                if (usercreditpayment.Count > 0)
                {

                    ViewBag.usercreditPayment = usercreditpayment;
                }
                else
                {
                    ViewBag.usercreditPayment = null;

                }
            }
            var result = _usercreditRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            ViewBag.userdetails = (from item in result
                                   select new SelectListItem
                                   {
                                       Value = item.UserId.ToString(),
                                       Text = item.User.FirstName + " " + item.User.LastName
                                   }).ToList();

            var resultCurrency = _usercreditRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            ViewBag.userdetails = (from item in result
                                   select new SelectListItem
                                   {
                                       Value = item.UserId.ToString(),
                                       Text = item.User.FirstName + " " + item.User.LastName
                                   }).ToList();

            var countryList = _countryRepository.GetAll().OrderBy(top => top.Name);
            ViewBag.countrydetails = (from item in countryList
                                      select new SelectListItem
                                      {
                                          Value = item.Id.ToString(),
                                          Text = item.Name
                                      }).ToList();
            if (userId != null)
            {
                _usercredit.CountryId = _contactsRepository.GetMany(top => top.UserId == userId).Select(top => top.CountryId.Value).FirstOrDefault();
            }
            else
            {
                _usercredit.CountryId = _contactsRepository.GetMany(top => top.UserId == usercredit.UserId).Select(top => top.CountryId.Value).FirstOrDefault();
            }
            return View(_usercredit);
        }
        [Route("UpdateCredit")]
        public ActionResult UpdateCredit(UserCreditFormModel _creditmodel)
        {
            if (ModelState.IsValid)
            {

                UsersCreditFormModel _usercredit = new UsersCreditFormModel();
                _usercredit.Id = _creditmodel.Id;
                _usercredit.UserId = _creditmodel.UserId;
                _usercredit.AssignCredit = _creditmodel.AssignCredit;
                var usercreditpayment = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == _creditmodel.UserId).Sum(top => top.Amount);
                var userbilling = _billingRepository.GetAll().Where(top => top.UserId == _creditmodel.UserId && top.PaymentMethodId == 1).Sum(top => top.FundAmount);
                _usercredit.AvailableCredit = (_creditmodel.AssignCredit + usercreditpayment) - (userbilling);
                _usercredit.CreatedDate = DateTime.Now;
                _usercredit.UpdatedDate = DateTime.Now;
                _usercredit.CurrencyId = _creditmodel.CurrencyId;
                CreateOrUpdateUsersCreditCommand command =
                Mapper.Map<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>(_usercredit);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Update user credit successfully.";
                    var userName = _userRepository.GetById(_creditmodel.UserId);
                    TempData["status"] = "Advertiser " + userName.FirstName + " " + userName.LastName + " have credit " + _creditmodel.AssignCredit + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(_creditmodel);
        }
        [Route("Index")]
        public ActionResult Index()
        {
            //List<UserCreditResult> _result = FillUserCreditResult();
            List<UserCreditResult> _result = new List<UserCreditResult>();
            SearchClass.UserCreditFilter _filterCritearea = new SearchClass.UserCreditFilter();

            var result = _usercreditRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            var userdropdown = (from item in result
                                select new
                                {
                                    Id = item.UserId,
                                    Name = item.User.DisplayName
                                }).ToList();
            ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");

            var countryResult = _countryRepository.GetAll().OrderBy(top => top.Name);
            var countrydropdown = (from item in countryResult
                                   select new
                                   {
                                       Id = item.Id,
                                       Name = item.Name
                                   }).ToList();
            ViewBag.countrydetails = new MultiSelectList(countrydropdown, "Id", "Name");

            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 28-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<UserCreditResult> _result = new List<UserCreditResult>();
                IEnumerable<UsersCredit> userCredit = null;
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
                    string Email = string.Empty;
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();
                    decimal creditFrom = 0.00M;
                    decimal creditTo = 0.00M;

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

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            var data = columnSearch[3].Split(',').ToArray();
                            creditFrom = Convert.ToDecimal(data[0]);
                            creditTo = Convert.ToDecimal(data[1]);
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[8]))
                    {
                        if (columnSearch[8] != "null")
                        {
                            var data = columnSearch[8].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[8] = null;
                        }
                    }

                    userCredit = _usercreditRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    foreach (var item in userCredit)
                    {
                        var totalused = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == item.UserId).Sum(top => top.FundAmount);
                        var totalplayed = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == item.UserId).Sum(top => top.Amount);
                        _result.Add(new UserCreditResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Credit = item.AssignCredit, AvailableCredit = item.AvailableCredit, TotalUsed = totalused, TotalPayed = totalplayed, RemainingAmount = totalused - totalplayed, Organisation = item.User.Organisation });
                    }
                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.Email == Email).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => (UserId.Contains(Convert.ToInt32(top.UserId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        _result = _result.Where(top => (top.Credit >= creditFrom && top.Credit <= creditTo)).ToList();
                    }
                    if (columnSearch[8] != null)
                    {
                        _result = _result.Where(top => (top.CreatedDateSort >= CreatedDatefromdate && top.CreatedDateSort <= CreatedDatetodate)).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    userCredit = _usercreditRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    foreach (var item in userCredit)
                    {
                        var totalused = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == item.UserId).Sum(top => top.FundAmount);
                        var totalplayed = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == item.UserId).Sum(top => top.Amount);
                        _result.Add(new UserCreditResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Credit = item.AssignCredit, AvailableCredit = item.AvailableCredit, TotalUsed = totalused, TotalPayed = totalplayed, RemainingAmount = totalused - totalplayed, Organisation = item.User.Organisation });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserCreditResult> result = new DTResult<UserCreditResult>
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
        // Function For Filter/Sorting UserCredit Data
        private static List<UserCreditResult> ApplySorting(DTParameters param, List<UserCreditResult> result)
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
                        result = result.OrderBy(top => top.Organisation).ToList();
                    else
                        result = result.OrderByDescending(top => top.Organisation).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Credit).ToList();
                    else
                        result = result.OrderByDescending(top => top.Credit).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.AvailableCredit).ToList();
                    else
                        result = result.OrderByDescending(top => top.AvailableCredit).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalUsed).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalUsed).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalPayed).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalPayed).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.RemainingAmount).ToList();
                    else
                        result = result.OrderByDescending(top => top.RemainingAmount).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        public List<UserCreditResult> FillUserCreditResult()
        {
            List<UserCreditResult> _usercreditResult = new List<UserCreditResult>();

            var result = _usercreditRepository.GetAll().OrderByDescending(top => top.Id);
            foreach (var item in result)
            {
                var totalused = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == item.UserId).Sum(top => top.FundAmount);
                var totalplayed = _usercreditpaymentRepository.GetAll().Where(top => top.UserId == item.UserId).Sum(top => top.Amount);
                _usercreditResult.Add(new UserCreditResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Credit = item.AssignCredit, AvailableCredit = item.AvailableCredit, TotalUsed = totalused, TotalPayed = totalplayed, RemainingAmount = totalused - totalplayed, Organisation = item.User.Organisation });

            }
            var userdropdown = (from item in _usercreditResult
                                select new
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                }).ToList();
            ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");

            return _usercreditResult;
        }
        [Route("SearchUsersCredit")]
        public ActionResult SearchUsersCredit([Bind(Prefix = "Item2")]SearchClass.UserCreditFilter _filterCritearea, int?[] CountryId, int[] UserId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserCreditResult> _result = new List<UserCreditResult>();
                var finalresult = new List<UserCreditResult>();
                if (_filterCritearea != null)
                {
                    _result = FillUserCreditResult();
                    finalresult = getusercreditResult(_result, _filterCritearea, CountryId, UserId);
                }
                else
                {
                    _result = FillUserCreditResult();
                    finalresult = getusercreditResult(_result, _filterCritearea, CountryId, UserId);
                }

                return PartialView("_UserCreditDetails", finalresult);
            }
            else
            {
                return PartialView("_UserCreditDetails", "notauthorise");
            }
        }
        public List<UserCreditResult> getusercreditResult(List<UserCreditResult> usercreditresult, SearchClass.UserCreditFilter _filterCritearea, int?[] CountryId, int[] UserId)
        {
            if (usercreditresult != null && _filterCritearea != null)
            {
                if (CountryId != null)
                {
                    List<int> userId = _contactsRepository.GetMany(top => CountryId.Contains(top.CountryId)).Select(top => top.UserId).ToList();
                    usercreditresult = usercreditresult.Where(top => userId.Contains(top.UserId)).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.Email))
                {
                    usercreditresult = usercreditresult.Where(top => top.Email.Contains(_filterCritearea.Email)).ToList();

                }
                if (UserId != null)
                {
                    usercreditresult = usercreditresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //usercreditresult = usercreditresult.Where(top => top.CreatedDate.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();

                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    usercreditresult = usercreditresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();


                }
                if ((!String.IsNullOrEmpty(_filterCritearea.FromCredit) && !String.IsNullOrEmpty(_filterCritearea.ToCredit)))
                {
                    decimal fromcredit = Convert.ToDecimal(_filterCritearea.FromCredit);
                    decimal tocredit = Convert.ToDecimal(_filterCritearea.ToCredit);
                    usercreditresult = usercreditresult.Where(top => top.Credit >= fromcredit && top.Credit <= tocredit).ToList();
                }
            }
            else
            {
                if (CountryId != null)
                {
                    List<int> userId = _contactsRepository.GetMany(top => CountryId.Contains(top.CountryId)).Select(top => top.UserId).ToList();
                    usercreditresult = usercreditresult.Where(top => userId.Contains(top.UserId)).ToList();
                }
                if (UserId != null)
                {
                    usercreditresult = usercreditresult.Where(top => UserId.Contains(top.Id)).ToList();
                }
            }
            return usercreditresult;
        }

        //[HttpPost]
        //[Route("GetCountry")]
        //public ActionResult GetCountry(string userId)
        //{
        //    if (userId != "0" || userId != "")
        //    {
        //        int UserId = Convert.ToInt32(userId);
        //        var userExists = _contactsRepository.Get(c => c.UserId == UserId);
        //        if (userExists != null)
        //        {
        //            var currencyId = _contactsRepository.Get(c => c.UserId == UserId).CurrencyId;
        //            int CurrencyId = Convert.ToInt32(currencyId);
        //            var countryCode = _currencyRepository.GetById(Convert.ToInt32(CurrencyId)).CurrencyCode;
        //            countryCode = "Credit" + " " + "(" + countryCode + ")";
        //            return Json(countryCode);
        //        }
        //        else
        //        {
        //            return Json("");
        //        }
        //    }
        //    return Json("");
        //}

        private void FillCurrencyList(int CurrencyId)
        {
            var currency = (from action in _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1) select new SelectListItem { Text = action.CurrencyCode, Value = action.CurrencyId.ToString(), Selected = action.CurrencyId == CurrencyId ? true : false }).ToList();
            ViewBag.currencyList = currency;
        }

        //Fill Operator
        [HttpPost]
        [Route("GetUserByCountry")]
        public ActionResult GetUserByCountry(int countryId)
        {
            if (countryId == 0)
            {
                List<SelectListItem> userdetails = new List<SelectListItem>();
                userdetails.Insert(0, (new SelectListItem { Text = "--Select Advertiser--", Value = "" }));
                ViewBag.userdetails = userdetails;
            }
            else
            {
                List<SelectListItem> userdetails = new List<SelectListItem>();
                List<int> userId = _contactsRepository.GetMany(top => top.CountryId == countryId).Select(top => top.UserId).ToList();
                var result = _userRepository.GetMany(top => userId.Contains(top.UserId) && top.RoleId == (int)UserRoles.Advertiser).OrderBy(top => top.FirstName);
                var userdata = (from item in result
                                select new
                                {
                                    Id = item.UserId,
                                    Name = item.FirstName + " " + item.LastName + "(" + item.Email + ")"
                                }).ToList();

                userdetails = (from item in userdata
                               select new SelectListItem
                               {
                                   Value = item.Id.ToString(),
                                   Text = item.Name
                               }).ToList();
                userdetails.Insert(0, new SelectListItem { Text = "--Select Advertiser--", Value = "" });
                ViewBag.userdetails = userdetails;
            }
            return Json(ViewBag.userdetails);
        }

        //public ActionResult FillCurrencyListBasedOnUser(int countryID)
        //{
        //    EFMVCDataContex db = new EFMVCDataContex();
        //    //var lst = db.Operator.Where(a => a.CountryId == countryID).Select(s => new SelectListItem { Text = s.OperatorName, Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
        //    var currency = _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1).ToList(); //select new SelectListItem { Text = action.CurrencyCode, Value = action.CurrencyId.ToString() }).ToList();
        //    var items = new List<SelectListItem>();

        //    foreach (var currencyFinal in currency)
        //    {
        //        items.Add(new SelectListItem()
        //        {
        //            Text = currencyFinal.CurrencyCode,
        //            Value = currencyFinal.CurrencyId.ToString(),
        //            // Put all sorts of business logic in here
        //            Selected = currencyFinal.CurrencyId == 13 ? true : false
        //        });
        //    }
        //    return Json(items, JsonRequestBehavior.AllowGet);

        //}
    }
}
