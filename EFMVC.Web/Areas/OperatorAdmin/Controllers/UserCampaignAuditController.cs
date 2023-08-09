using EFMVC.Web.Common;
using EFMVC.Web.SearchClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFMVC.Web.Models;
using AutoMapper;
using System.Data;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Model;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.ViewModels;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("UserCampaignAudit")]
    public class UserCampaignAuditController : Controller
    {
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _campaignProfile repository
        /// </summary>
        private readonly ICampaignProfileRepository _campaignProfileRepository;

        /// <summary>
        /// The _currency repository
        /// </summary>
        private readonly ICurrencyRepository _currencyRepository;

        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _campaignAudit repository
        /// </summary>
        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The _contact repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _userProfile repository
        /// </summary>
        private readonly IUserProfileRepository _userProfileRepository;

        private readonly CurrencyConversion _currencyConversion;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserCampaignAuditController(ICommandBus commandBus, IUserRepository userRepository, ICampaignProfileRepository campaignProfileRepository, ICurrencyRepository currencyRepository, IOperatorRepository operatorRepository, ICountryRepository countryRepository, ICampaignAuditRepository campaignAuditRepository, IContactsRepository contactsRepository, IUserProfileRepository userProfileRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _campaignProfileRepository = campaignProfileRepository;
            _currencyRepository = currencyRepository;
            _operatorRepository = operatorRepository;
            _countryRepository = countryRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _contactsRepository = contactsRepository;
            _userProfileRepository = userProfileRepository;
            _currencyConversion = CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        // GET: OperatorAdmin/UserCampaignAudit
        [Route("Index")]
        public ActionResult Index(int CampaignProfileId)
        {
            FillCountryList();
            Session["CampaignId"] = CampaignProfileId;
            ViewBag.CampaignId = CampaignProfileId;

            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();
            var campaignProfile = _campaignProfileRepository.GetById(CampaignProfileId);
            TempData["UserId"] = campaignProfile.UserId;
            ViewBag.campaignName = campaignProfile.CampaignName;
            CampaignProfileMapping _mapping = new CampaignProfileMapping();
            CampaignAuditFilter CampaignAuditFilter = new CampaignAuditFilter();
            CampaignDashboardChartResult campaignDashboardChart = new CampaignDashboardChartResult();
            var Id = campaignProfile.CountryId;
            int CountryId = Convert.ToInt32(Id);
            string currencyCode;
            currencyCode = _currencyRepository.Get(top => top.CountryId == CountryId).CurrencyCode;
            //FillCurrencyList();
            Session["CountryId"] = CountryId;
            Session["CurrencyId"] = 0;
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            if (Id != 0)
            {
                CampaignAuditFilter.CampaignProfileId = CampaignProfileId;
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                _mapping.CampaignAudit = new List<CampaignAuditResult>();
                _mapping.CampaignAudit.Add(new CampaignAuditResult { CurrencyCode = currencyCode });
                var model = GetEditData(CampaignProfileId, efmvcUser.UserId);
                if (model != null) campaignDashboardChart = FillChartDataByCampaignId(model);
                _mapping.CampaignDashboardChartResult = campaignDashboardChart;

                return View(_mapping);
            }
            else
            {
                ViewBag.ClientId = null;
                ViewBag.CampaignProfileId = null;
            }
            return View("Index");
        }
		
		private void FillCountryList()
        {
            var countryId = _operatorRepository.GetMany(s => s.IsActive).Select(c => c.CountryId).ToList();
            var country = (from action in _countryRepository.GetMany(c => countryId.Contains(c.Id)).OrderBy(c => c.Name)
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();
            ViewBag.countryList = country;
        }		
		public void FillCampaignAuditStatus()
        {
            IEnumerable<CampaignAuditStatus> audTypes = Enum.GetValues(typeof(CampaignAuditStatus))
                                                     .Cast<CampaignAuditStatus>();
            var auditstatus = (from action in audTypes
                               select new SelectListItem
                               {
                                   Text = action.ToString(),
                                   Value = ((int)action).ToString()
                               }).ToList();
            auditstatus.Insert(0, new SelectListItem { Text = "--Select Status--", Value = "0" });
            ViewBag.campaignauditStatus = auditstatus;
        }
        public void FillCampaignAuditSMSStatus()
        {
            IEnumerable<CampaignAuditSMSStatus> audsmsTypes = Enum.GetValues(typeof(CampaignAuditSMSStatus))
                                                     .Cast<CampaignAuditSMSStatus>();
            var auditsmsstatus = (from action in audsmsTypes
                                  select new SelectListItem
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            auditsmsstatus.Insert(0, new SelectListItem { Text = "--Select Status--", Value = "0" });
            ViewBag.campaignauditSMSStatus = auditsmsstatus;
        }
        //private void FillCurrencyList()
        //{
        //    var currency = (from action in db.Currencies.OrderBy(c => c.CurrencyCode).Skip(1)
        //                    select new SelectListItem
        //                    {
        //                        Text = action.CurrencyCode,
        //                        Value = action.CurrencyId.ToString()
        //                    }).ToList();
        //    ViewBag.currencyList = currency;
        //}

        [HttpPost]
        public JsonResult LoadCampaign(DTParameters param, string CampaignId)
        {
            try
            {
                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();
                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }
                int cnt = 10;
                int id = 0;
                if (!string.IsNullOrEmpty(CampaignId)) id = Convert.ToInt32(CampaignId);
                double totalcredit = 0;
                double spendtodate = 0;
                double playcost = 0;
                double emailcost = 0;
                double smscost = 0;
                if (TempData["commusuccess"] != null) ViewBag.commusuccess = TempData["commusuccess"];
                if (TempData["commuerror"] != null) ViewBag.commuerror = TempData["commuerror"];
                List<CampaignAuditResult> _audit = new List<CampaignAuditResult>();
                EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
                var advertname = string.Empty;
                var advertid = 0;
                var emailstatus = string.Empty;
                var smsstatus = string.Empty;
                if (_campaignProfileRepository.Count(x => x.CampaignProfileId == id && (x.Status != 5)) > 0)
                {
                    CampaignProfile profile = _campaignProfileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id);
                    var CountryID = profile.CountryId;
                    totalcredit = (double)profile.TotalCredit;
                    IEnumerable<CampaignAuditFormModel> model = null;
                    #region Search And Load
                    if (searchValue == true)
                    {
                        #region Searching Functionality
                        int playId = 0, userId = 0, status = 0, sms = 0, fromLengthOfPlay = 0, toLengthOfPlay = 0, fromPlayCost = 0, toPlayCost = 0, fromTotalCost = 0, toTotalCost = 0;
                        string disStatus = null, disSms = null;
                        DateTime startTime = new DateTime();
                        DateTime endTime = new DateTime();
                        if (!String.IsNullOrEmpty(columnSearch[0]))
                        {
                            if (columnSearch[0] != "null") playId = Convert.ToInt32(columnSearch[0]);
                            else columnSearch[0] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[1]))
                        {
                            if (columnSearch[1] != "null") userId = Convert.ToInt32(columnSearch[1]);
                            else columnSearch[1] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[2]))
                        {
                            if (columnSearch[2] != "null") startTime = Convert.ToDateTime(columnSearch[2]).Date;
                            else columnSearch[2] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[3]))
                        {
                            if (columnSearch[3] != "null") endTime = Convert.ToDateTime(columnSearch[3]).Date;
                            else columnSearch[3] = null;
                        }
                        if (!String.IsNullOrEmpty(columnSearch[4]))
                        {
                            if (columnSearch[4] != "null")
                            {
                                var data = columnSearch[4].Split(',').ToArray();
                                fromLengthOfPlay = Convert.ToInt32(data[0]);
                                toLengthOfPlay = Convert.ToInt32(data[1]);
                            }
                            else columnSearch[4] = null;
                        }
                        if (!String.IsNullOrEmpty(columnSearch[6]))
                        {
                            if (columnSearch[6] != "null")
                            {
                                status = Convert.ToInt32(columnSearch[6]);
                                disStatus = status == 1 ? "Played" : status == 2 ? "Cancelled" : status == 3 ? "Short" : null;
                            }
                            else columnSearch[6] = null;
                        }
                        if (!String.IsNullOrEmpty(columnSearch[7]))
                        {
                            if (columnSearch[7] != "null")
                            {
                                var data = columnSearch[7].Split(',').ToArray();
                                fromPlayCost = Convert.ToInt32(data[0]);
                                toPlayCost = Convert.ToInt32(data[1]);
                            }
                            else columnSearch[7] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[8]))
                        {
                            if (columnSearch[8] != "null")
                            {
                                sms = Convert.ToInt32(columnSearch[8]);
                                disSms = sms == 1 ? "SMS" : sms == 2 ? "No" : sms == 3 ? "SMS" : null;
                            }
                            else columnSearch[8] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[12]))
                        {
                            if (columnSearch[12] != "null")
                            {
                                var data = columnSearch[12].Split(',').ToArray();
                                fromTotalCost = Convert.ToInt32(data[0]);
                                toTotalCost = Convert.ToInt32(data[1]);
                            }
                            else columnSearch[12] = null;
                        }
                        model = _campaignAuditRepository.GetMany(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                             || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                             || (s.Status == disStatus)
                                                             || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                             || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                             || (s.SMS == disSms)
                                                             || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                             || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                             ))
                                                            .OrderByDescending(s => s.CampaignAuditId)
                                                            .Select(s => new CampaignAuditFormModel
                                                            {
                                                                BidValue = s.BidValue,
                                                                EmailCost = s.EmailCost,
                                                                SMSCost = s.SMSCost,
                                                                CampaignProfileId = s.CampaignProfileId,
                                                                Email = s.Email,
                                                                SMS = s.SMS,
                                                                PlayLengthTicks = s.PlayLengthTicks / 1000,
                                                                CampaignAuditId = s.CampaignAuditId,
                                                                UserProfileId = s.UserProfileId,
                                                                StartTime = s.StartTime,
                                                                EndTime = s.EndTime,
                                                                Status = s.Status,
                                                                TotalCost = s.TotalCost
                                                            });
                        cnt = _campaignAuditRepository.GetMany(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                             || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                             || (s.Status == disStatus)
                                                             || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                             || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                             || (s.SMS == disSms)
                                                             || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                             || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                             )).Count();
                        cnt = profile.CampaignAudits.Count();
                        model = model.Skip(param.Start).Take(param.Length).ToList();
                        #endregion
                    }
                    else
                    {
                        cnt = profile.CampaignAudits.Count();
                        model = Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits.OrderByDescending(top => top.CampaignAuditId).ToList());
                        model = model.Skip(param.Start).Take(param.Length).ToList();
                    }
                    #endregion
                    var userCountryId = _contactsRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                    string currencysymbol = "";
                    string currencyCode = "";
                    var userCurrencyId = _contactsRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                    if (userCurrencyId != null || userCurrencyId != 0)
                    {
                        currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode);
                        currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
                    }
                    else
                    {
                        currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyRepository.GetCurrencyUsingCountryId(userCountryId).CurrencyCode);
                        currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
                    }

                    foreach (var item in model)
                    {
                        var advertdetails = profile.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId);
                        if (advertdetails != null && advertdetails.Count() > 0)
                        {
                            advertname = advertdetails.FirstOrDefault().Advert.AdvertName;
                            advertid = advertdetails.FirstOrDefault().Advert.AdvertId;
                        }
                        else
                        {
                            advertname = string.Empty;
                            advertid = 0;
                        }
                        if (!String.IsNullOrEmpty(item.Email))
                        {
                            if (item.Email.Trim().ToLower() == "none") emailstatus = "No";
                            else if (item.Email.Trim().ToLower() == "requested") emailstatus = "Yes";
                            else emailstatus = "Both";
                        }
                        if (!String.IsNullOrEmpty(item.SMS))
                        {
                            if (item.SMS.Trim().ToLower() == "none") smsstatus = "No";
                            else if (item.SMS.Trim().ToLower() == "requested") smsstatus = "Yes";
                            else smsstatus = "Both";
                        }

                        var userDetails = _userProfileRepository.GetById(item.UserProfileId);
                        if (userDetails != null)
                        {
                            double playcostdata = 0.00;
                            double emailcostdata = 0.00;
                            double smscostdata = 0.00;
                            double totalcost = 0.00;
                            decimal currencyRate = 0.00M;
                            var fromCurrencyCode = _campaignProfileRepository.GetById(id).CurrencyCode;
                            string toCurrencyCode = currencyCode;
                            if (currencyCode == fromCurrencyCode)
                            {
                                playcostdata = Convert.ToDouble(item.BidValue.ToString());
                                emailcostdata = Convert.ToDouble(item.EmailCost.ToString());
                                smscostdata = Convert.ToDouble(item.SMSCost.ToString());
                                totalcost = Convert.ToDouble(item.TotalCost.ToString());
                                playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
                                emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
                                smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
                            }
                            else
                            {
                                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                                currencyRate = currencyModel.Amount;
                                if (currencyModel.Code == "OK")
                                {
                                    playcostdata = Convert.ToDouble(Convert.ToDecimal(item.BidValue) * currencyRate);
                                    emailcostdata = Convert.ToDouble(Convert.ToDecimal(item.EmailCost) * currencyRate);
                                    smscostdata = Convert.ToDouble(Convert.ToDecimal(item.SMSCost) * currencyRate);
                                    totalcost = Convert.ToDouble(Convert.ToDecimal(item.TotalCost) * currencyRate);
                                    playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
                                    emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
                                    smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
                                }
                            }
                            var lengthOfPlay = Convert.ToDouble(item.PlayLengthTicks) / 1000;
                            _audit.Add(new CampaignAuditResult { PlayID = item.CampaignAuditId, UserID = userDetails.UserId, StartDate = item.StartTime.ToString("MM/dd/yyyy hh:mm:ss"), EndDate = item.EndTime.ToString("MM/dd/yyyy hh:mm:ss"), LengthOfPlay = RoundUp(lengthOfPlay, 2), AdvertName = advertname, AdvertId = advertid, Status = item.Status, PlayCost = RoundUp(Convert.ToDouble(playcostdata.ToString("F2")), 2), SMS = item.SMS == null ? "-" : smsstatus, SMSCost = Convert.ToDouble(smscostdata.ToString("F2")), Email = item.Email == null ? "-" : emailstatus, EmailCost = Convert.ToDouble(emailcostdata.ToString("F2")), TotalCost = RoundUp(Convert.ToDouble(totalcost.ToString("F2")), 2), DisplayStartDate = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayStartDateSort = item.StartTime, DisplayEndDate = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayEndDateSort = item.EndTime, CurrencyCode = currencysymbol, CountryId = userCountryId, CurrencyId = userCurrencyId });
                        }
                    }
                }
                else
                {
                    cnt = 0;
                }
                _audit = ApplySortingCampaigm(param, _audit);

               
                DTResult<CampaignAuditResult> result = new DTResult<CampaignAuditResult>
                {
                    draw = param.Draw,
                    data = _audit,
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

        private static List<CampaignAuditResult> ApplySortingCampaigm(DTParameters param, List<CampaignAuditResult> _audit)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.PlayID).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.PlayID).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.UserID).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.UserID).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.DisplayStartDateSort).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.DisplayStartDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.DisplayEndDateSort).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.DisplayEndDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.LengthOfPlay).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.LengthOfPlay).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.AdvertName).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.AdvertName).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.Status).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.Status).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.PlayCost).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.PlayCost).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.SMS).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.SMS).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.SMSCost).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.SMSCost).ToList();
                }
                else if (paramOrderDetails.Column == 10)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.Email).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 11)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.EmailCost).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.EmailCost).ToList();
                }
                else if (paramOrderDetails.Column == 12)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        _audit = _audit.OrderBy(top => top.TotalCost).ToList();
                    else
                        _audit = _audit.OrderByDescending(top => top.TotalCost).ToList();
                }
            }
            return _audit;
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        [HttpPost]
        [Route("SearchAudit")]
        public ActionResult SearchAudit([Bind(Prefix = "Item2")]CampaignAuditFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<CampaignAuditResult> _result = new List<CampaignAuditResult>();
                var finalresult = new List<CampaignAuditResult>();
                if (_filterCritearea == null)
                {

                    _filterCritearea = new CampaignAuditFilter();
                    _filterCritearea.CampaignProfileId = Convert.ToInt32(Session["CampaignId"]);
                    _result = campaignAudit(_filterCritearea.CampaignProfileId);
                    finalresult = getcampaignauditResult(_result, _filterCritearea);
                }
                else
                {
                    _result = campaignAudit(_filterCritearea.CampaignProfileId);
                    finalresult = getcampaignauditResult(_result, _filterCritearea);

                }
                return PartialView("_CampaignAuditList", finalresult);
            }
            else
            {
                return PartialView("_CampaignAuditList", "notauthorise");
            }
        }

        private List<CampaignAuditResult> campaignAudit(int id)
        {
            double totalcredit = 0;
            double playcost = 0;
            double emailcost = 0;
            double smscost = 0;
            if (TempData["commusuccess"] != null) ViewBag.commusuccess = TempData["commusuccess"];
            if (TempData["commuerror"] != null) ViewBag.commuerror = TempData["commuerror"];
            List<CampaignAuditResult> _audit = new List<CampaignAuditResult>();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            CurrencyModel currencyModel = new CurrencyModel();
            var advertname = string.Empty;
            var advertid = 0;
            var emailstatus = string.Empty;
            var smsstatus = string.Empty;
            if (_campaignProfileRepository.Count(x => x.CampaignProfileId == id && (x.Status != 5)) > 0)
            {
                CampaignProfile profile = _campaignProfileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id);
                totalcredit = (double)profile.TotalCredit;
                IEnumerable<CampaignAuditFormModel> model = Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits.OrderByDescending(top => top.CampaignAuditId).ToList());
                var userCountryId = _contactsRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                string currencysymbol = "";
                string currencyCode = "";
                var userCurrencyId = _contactsRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                if (userCurrencyId != null && userCurrencyId != 0)
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode);
                    currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
                }
                else
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyRepository.GetCurrencyUsingCountryId(userCountryId).CurrencyCode);
                    currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
                }
                double playcostamount = 0.00;
                double emailcostamount = 0.00;
                double smscostamount = 0.00;
                double totalcostamount = 0.00;
                foreach (var item in model.OrderByDescending(s => s.CampaignAuditId).ToList())
                {
                    playcost = item.BidValue;
                    emailcost = item.EmailCost;
                    smscost = item.SMSCost;
                    var advertdetails = profile.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId);
                    if (advertdetails != null && advertdetails.Count() > 0)
                    {
                        advertname = advertdetails.FirstOrDefault().Advert.AdvertName;
                        advertid = advertdetails.FirstOrDefault().Advert.AdvertId;
                    }
                    else
                    {
                        advertname = string.Empty;
                        advertid = 0;
                    }
                    if (!String.IsNullOrEmpty(item.Email))
                    {
                        if (item.Email.Trim().ToLower() == "none") emailstatus = "No";
                        else if (item.Email.Trim().ToLower() == "requested") emailstatus = "Yes";
                        else emailstatus = "Both";
                    }
                    if (!String.IsNullOrEmpty(item.SMS))
                    {
                        if (item.SMS.Trim().ToLower() == "none") smsstatus = "No";
                        else if (item.SMS.Trim().ToLower() == "requested") smsstatus = "Yes";
                        else smsstatus = "Both";
                    }
                    var userDetails = _userProfileRepository.GetById(item.UserProfileId);
                    if (userDetails != null)
                    {
                        decimal currencyRate = 0.00M;
                        string fromCurrencyCode = _campaignProfileRepository.GetById(item.CampaignProfileId).CurrencyCode;
                        string toCurrencyCode = currencyCode;
                        if (toCurrencyCode == fromCurrencyCode)
                        {
                            playcostamount = Convert.ToDouble(playcost.ToString());
                            emailcostamount = Convert.ToDouble(emailcost.ToString());
                            smscostamount = Convert.ToDouble(smscost.ToString());
                            totalcostamount = Convert.ToDouble(item.TotalCost.ToString());
                            playcostamount = Convert.ToDouble(playcostamount.ToString("F2"));
                            emailcostamount = Convert.ToDouble(emailcostamount.ToString("F2"));
                            smscostamount = Convert.ToDouble(smscostamount.ToString("F2"));
                            totalcostamount = Convert.ToDouble(totalcostamount.ToString("F2"));
                        }
                        else
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK")
                            {
                                playcostamount = Convert.ToDouble(Convert.ToDecimal(playcost) * currencyRate);
                                emailcostamount = Convert.ToDouble(Convert.ToDecimal(emailcost) * currencyRate);
                                smscostamount = Convert.ToDouble(Convert.ToDecimal(smscost) * currencyRate);
                                totalcostamount = Convert.ToDouble(Convert.ToDecimal(item.TotalCost) * currencyRate);
                                playcostamount = Math.Round(playcostamount, 2, MidpointRounding.AwayFromZero);
                                emailcostamount = Math.Round(emailcostamount, 2, MidpointRounding.AwayFromZero);
                                smscostamount = Math.Round(smscostamount, 2, MidpointRounding.AwayFromZero);
                                totalcostamount = Math.Round(totalcostamount, 2, MidpointRounding.AwayFromZero);
                            }
                        }
                        var lengthOfPlay = Convert.ToDouble(item.PlayLengthTicks) / 1000;
                        _audit.Add(new CampaignAuditResult { PlayID = item.CampaignAuditId, UserID = userDetails.UserId, StartDate = item.StartTime.ToString("MM/dd/yyyy hh:mm:ss"), EndDate = item.EndTime.ToString("MM/dd/yyyy hh:mm:ss"), LengthOfPlay = RoundUp(lengthOfPlay, 2), AdvertName = advertname, AdvertId = advertid, Status = item.Status, PlayCost = RoundUp(playcostamount, 2), SMS = item.SMS == null ? "-" : smsstatus, SMSCost = smscostamount, Email = item.Email == null ? "-" : emailstatus, EmailCost = emailcostamount, TotalCost = RoundUp(totalcostamount, 2), DisplayStartDate = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayStartDateSort = item.StartTime, DisplayEndDate = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayEndDateSort = item.EndTime, CurrencyCode = currencysymbol, CountryId = userCountryId, CurrencyId = userCurrencyId });
                    }
                }
            }
            return _audit;
        }

        public List<CampaignAuditResult> getcampaignauditResult(List<CampaignAuditResult> campaignaudit, CampaignAuditFilter _filterCritearea)
        {
            if (campaignaudit != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.PlayID))
                {
                    if (_filterCritearea.PlayID != "0")
                    {
                        int PlayID = Convert.ToInt32(_filterCritearea.PlayID);
                        campaignaudit = campaignaudit.Where(top => top.PlayID == PlayID).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(_filterCritearea.UserID))
                {
                    if (_filterCritearea.UserID != "0")
                    {
                        int UserID = Convert.ToInt32(_filterCritearea.UserID);
                        campaignaudit = campaignaudit.Where(top => top.UserID == UserID).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromPlayLength) && !String.IsNullOrEmpty(_filterCritearea.FromPlayLength))
                {

                    campaignaudit = campaignaudit.Where(top => top.LengthOfPlay >= Convert.ToInt32(_filterCritearea.FromPlayLength) && top.LengthOfPlay <= Convert.ToInt32(_filterCritearea.ToPlayLength)).ToList();

                }

                if (!String.IsNullOrEmpty(_filterCritearea.Status))
                {
                    if (_filterCritearea.Status != "0")
                    {
                        CampaignAuditStatus cstatus = (CampaignAuditStatus)Convert.ToInt32(_filterCritearea.Status);
                        string auditstatus = cstatus.ToString();
                        campaignaudit = campaignaudit.Where(top => top.Status.ToLower() == auditstatus.ToLower()).ToList();
                    }
                }

                if (!String.IsNullOrEmpty(_filterCritearea.FromPlayCost) && !String.IsNullOrEmpty(_filterCritearea.ToPlayCost))
                {

                    campaignaudit = campaignaudit.Where(top => top.PlayCost >= Convert.ToDouble(_filterCritearea.FromPlayCost) && top.PlayCost <= Convert.ToDouble(_filterCritearea.ToPlayCost)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromTotalCost) && !String.IsNullOrEmpty(_filterCritearea.ToTotalCost))
                {

                    campaignaudit = campaignaudit.Where(top => top.TotalCost >= Convert.ToDouble(_filterCritearea.FromTotalCost) && top.TotalCost <= Convert.ToDouble(_filterCritearea.ToTotalCost)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.SMSStatus))
                {
                    if (_filterCritearea.SMSStatus != "0")
                    {
                        CampaignAuditSMSStatus cstatus = (CampaignAuditSMSStatus)Convert.ToInt32(_filterCritearea.SMSStatus);
                        string auditsmsstatus = cstatus.ToString();
                        campaignaudit = campaignaudit.Where(top => top.SMS.ToLower() == auditsmsstatus.ToLower()).ToList();
                    }
                }

                if ((_filterCritearea.StartFromtime != null && _filterCritearea.StartTotime != null))
                {
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;                    
                    DateTime to = DateTime.Parse(_filterCritearea.StartTotime.Substring(11, 8));
                    string todatetime = _filterCritearea.StartTotime.Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                    Todate = DateTime.ParseExact(todatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);                   
                    DateTime from = DateTime.Parse(_filterCritearea.StartFromtime.Substring(11, 8));
                    string fromdatetime = _filterCritearea.StartFromtime.Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                    Fromdate = DateTime.ParseExact(fromdatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);                    
                    campaignaudit = campaignaudit.Where(top => top.DisplayStartDateSort >= Fromdate.Value && top.DisplayStartDateSort <= Todate.Value).ToList();
                }
                if ((_filterCritearea.EndFromtime != null && _filterCritearea.EndTotime != null))
                {
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;
                   
                    DateTime from = DateTime.Parse(_filterCritearea.EndTotime.Substring(11, 8));
                    string fromdatetime = _filterCritearea.EndTotime.Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                    Todate = DateTime.ParseExact(fromdatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    DateTime to = DateTime.Parse(_filterCritearea.EndFromtime.Substring(11, 8));
                    string todatetime = _filterCritearea.EndFromtime.Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                    Fromdate = DateTime.ParseExact(todatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    campaignaudit = campaignaudit.Where(top => top.DisplayEndDateSort >= Fromdate.Value && top.DisplayEndDateSort <= Todate.Value).ToList();
                }

            }
            return campaignaudit;
        }

        public CampaignDashboardChartResult FillChartDataByCampaignId(CampaignProfileFormModel _CampaignProfileFormModel)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
            List<CampaignNoOfPlay> _campaignNoofplay = new List<CampaignNoOfPlay>();
            List<Campaignchartresult> _campaignbidavgResult = new List<Campaignchartresult>();
            List<CampaignAvgbid> _campaignAvgbid = new List<CampaignAvgbid>();
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            CurrencyModel currencyModel = new CurrencyModel();
            float PlaystoDate = 0; decimal TotalBudget = 0;
            double AverageBid = 0, SpendToDate = 0, MaxBid = 0, AveragePlayTime = 0, SMSCost = 0, EmailCost = 0, Cancelled = 0;
            List<long> _maxplaylength = new List<long>();
            int FreePlays = 0, TotalReach = 0;
            DateTime? maxdate = new DateTime(), mindate = new DateTime();
            var UserId = efmvcUser.UserId;
            Contacts contacts = _contactsRepository.Get(c => c.UserId == UserId);
            var userCountryId = contacts.CountryId;
            string currencysymbol = "";
            string currencyCode = "";
            var userCurrencyId = contacts.CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            {
                currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode);
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencysymbol = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyRepository.GetCurrencyUsingCountryId(userCountryId).CurrencyCode);
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            if (_CampaignProfileFormModel != null)
            {
                TotalBudget = _CampaignProfileFormModel.TotalBudget;
                var campaignMaxBid = _CampaignProfileFormModel.MaxBid;
                List<CampaignAudit> campaignAudit = null;
                campaignAudit = _campaignAuditRepository.GetMany(s => s.CampaignProfileId == _CampaignProfileFormModel.CampaignProfileId).ToList();
                if (campaignAudit.Count > 0)
                {
                    var allPlayData = campaignAudit.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase);
                    PlaystoDate = PlaystoDate + allPlayData.Count();
                    //caculate the PlaystoDate field
                    var playedCampaignAuditData = campaignAudit.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase && top.PlayLengthTicks >= 6000);
                    if (playedCampaignAuditData.Count() > 0)
                    {
                        var sumOfBidValue = playedCampaignAuditData.Sum(top => top.BidValue);
                        var totalCount = playedCampaignAuditData.Count();
                        SpendToDate = SpendToDate + sumOfBidValue;
                        SpendToDate = SpendToDate + playedCampaignAuditData.Sum(top => top.SMSCost);
                        SpendToDate = SpendToDate + playedCampaignAuditData.Sum(top => top.EmailCost);
                        AverageBid = (sumOfBidValue / totalCount);
                        var playLengthTicksSum = allPlayData.Select(s => s.PlayLengthTicks).Sum();
                        var sumOfSecond = Convert.ToDouble(playLengthTicksSum) / 1000;
                        AveragePlayTime = (sumOfSecond / PlaystoDate);
                        _maxplaylength.Add(playedCampaignAuditData.Max(top => top.PlayLengthTicks));
                        MaxBid = MaxBid + playedCampaignAuditData.Max(top => top.BidValue);
                        _campaignNoofplay = playedCampaignAuditData.Select(s => new CampaignNoOfPlay { startdate = s.StartTime, playcount = 1, startdatecompare = s.StartTime }).ToList();
                        _campaignAvgbid = playedCampaignAuditData.Select(s => new CampaignAvgbid { startdate = s.StartTime, bidvalue = s.BidValue, startdatecompare = s.StartTime }).ToList();
                        _playgroup = playedCampaignAuditData.Select(s => new MaxLengthGroup { second = (s.PlayLengthTicks / 1000) }).ToList();
                        TotalReach = playedCampaignAuditData.DistinctBy(top => top.UserProfileId).Count();
                    }
                    //caculate the FreePlays field
                    var freePlaysCount = _CampaignProfileFormModel.GetDomainCampaignAudits(_campaignAuditRepository).Count(top => top.PlayLengthTicks < 6000 && top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase);
                    if (freePlaysCount > 0)
                    {
                        FreePlays = FreePlays + freePlaysCount;
                    }
                    SMSCost = SMSCost + campaignAudit.Count(d1 => !string.IsNullOrWhiteSpace(d1.SMS));
                    EmailCost = EmailCost + campaignAudit.Count(d1 => !string.IsNullOrWhiteSpace(d1.Email));
                    Cancelled = Cancelled + campaignAudit.Where(top => top.Status.Trim().ToLower() == Convert.ToString(CampaignAuditStatus.Cancelled).ToLower()).Sum(d1 => d1.TotalCost);
                }
            }
            if (_campaignNoofplay.Count > 0)
            {
                maxdate = _campaignNoofplay.Max(top => top.startdate.AddDays(1));
                mindate = maxdate.Value.Date.AddYears(-1);
            }
            GetCampaignbidplaydata(_campaignNoofplay, _campaignAvgbid, _campaignbidavgResult, maxdate, mindate);
            if (_playgroup.Count() > 0)
            {
                int[] _getbarChartdata = _getbarChartData(_playgroup);
                if (_getbarChartdata == null) ViewBag.getbarChartdata = _getbarChartdata;
                else ViewBag.getbarChartdata = _getbarChartdata.ToArray();
            }
            else
            {
                _playgroup.Add(new MaxLengthGroup { second = 0 });
                int[] _getbarChartdata1 = _getbarChartData(_playgroup);
                ViewBag.getbarChartdata = _getbarChartdata1.ToArray();
            }
            double campaignSpendToDate = 0.00;
            double campaignTotalBudget = 0.00;
            double campaignAverageBid = 0.00;
            decimal currencyRate = 0.00M;
            string fromCurrencyCode = _campaignProfileRepository.GetById(_CampaignProfileFormModel.CampaignProfileId).CurrencyCode;
            string toCurrencyCode = currencyCode;
            if (toCurrencyCode == fromCurrencyCode)
            {
                campaignSpendToDate = Convert.ToDouble(SpendToDate);
                campaignTotalBudget = Convert.ToDouble(TotalBudget);
                campaignAverageBid = Convert.ToDouble(AverageBid);
            }
            else
            {
                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                currencyRate = currencyModel.Amount;
                if (currencyModel.Code == "OK")
                {
                    campaignSpendToDate = Convert.ToDouble(Convert.ToDecimal(SpendToDate) * currencyRate);
                    campaignTotalBudget = Convert.ToDouble(Convert.ToDecimal(TotalBudget) * currencyRate);
                    campaignAverageBid = Convert.ToDouble(Convert.ToDecimal(AverageBid) * currencyRate);
                }
            }
            _CampaignDashboardChartResult.PlaystoDate = PlaystoDate;
            _CampaignDashboardChartResult.SpendToDate = RoundUp(campaignSpendToDate, 2);
            _CampaignDashboardChartResult.AverageBid = RoundUp(AverageBid, 2);
            _CampaignDashboardChartResult.AveragePlayTime = RoundUp(AveragePlayTime, 2);
            _CampaignDashboardChartResult.FreePlays = FreePlays;
            _CampaignDashboardChartResult.TotalBudget = (decimal)RoundUp(campaignTotalBudget, 2);
            _CampaignDashboardChartResult.TotalSpend = RoundUp(campaignSpendToDate, 2);
            _CampaignDashboardChartResult.MaxBid = RoundUp(MaxBid, 2);
            _CampaignDashboardChartResult.AvgMaxBid = RoundUp(campaignAverageBid, 2);
            _CampaignDashboardChartResult.CurrencyCode = toCurrencyCode;
            _CampaignDashboardChartResult.TotalReach = TotalReach;
            if (_maxplaylength.Count > 0)
            {
                var maxPlayLength = (_maxplaylength.Max()) / 1000;
                _CampaignDashboardChartResult.MaxPlayLength = maxPlayLength;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = RoundUp(((maxPlayLength / AveragePlayTime)) * 100, 2);
                var playtimeDecimal = Convert.ToDecimal(_CampaignDashboardChartResult.AveragePlayTime);
                _CampaignDashboardChartResult.MaxPlayLength = playtimeDecimal;
            }
            else
            {
                _CampaignDashboardChartResult.MaxPlayLength = 0;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = 0;
            }
            //caculate the FreePlays Percantage field
            if (PlaystoDate > 0 && FreePlays > 0) _CampaignDashboardChartResult.FreePlaysPercantage = RoundUp(((FreePlays / PlaystoDate) * 100), 2);
            else _CampaignDashboardChartResult.FreePlaysPercantage = 0;
            //caculate the TotalBudget Percantage field
            if (SpendToDate > 0 && TotalBudget > 0) _CampaignDashboardChartResult.TotalBudgetPercantage = RoundUp(((SpendToDate / Convert.ToDouble(TotalBudget)) * 100), 2);
            else _CampaignDashboardChartResult.TotalBudgetPercantage = 0;
            //caculate the MaxBid Percantage field
            if (AverageBid > 0 && MaxBid > 0) _CampaignDashboardChartResult.MaxBidPercantage = RoundUp(((AverageBid / MaxBid) * 100), 2);
            else _CampaignDashboardChartResult.MaxBidPercantage = 0;
            var spendToDate = _CampaignDashboardChartResult.SpendToDate;
            _CampaignDashboardChartResult.SpendToDate = spendToDate;
            _CampaignDashboardChartResult.AverageBid = campaignAverageBid;
            var totalBudgetData = _CampaignDashboardChartResult.TotalBudget;
            _CampaignDashboardChartResult.TotalBudget = decimal.Parse(totalBudgetData.ToString());
            ViewBag.FreePlays = FreePlays == 0 ? 1 : FreePlays;
            ViewBag.TotalPlayed = PlaystoDate == 0 ? 0 : PlaystoDate;
            ViewBag.TotalBudget = _CampaignDashboardChartResult.TotalBudget == 0 ? 1 : _CampaignDashboardChartResult.TotalBudget;
            ViewBag.TotalSpend = _CampaignDashboardChartResult.SpendToDate == 0 ? 1 : _CampaignDashboardChartResult.SpendToDate;
            ViewBag.MaxBid = RoundUp(TotalReach == 0 ? 0 : TotalReach, 2);
            ViewBag.AvgMaxBid = RoundUp(campaignAverageBid == 0 ? 1 : campaignAverageBid, 2);
            _CampaignDashboardChartResult.EmailCost = EmailCost;
            _CampaignDashboardChartResult.SMSCost = SMSCost;
            _CampaignDashboardChartResult.Cancelled = Cancelled;
            return _CampaignDashboardChartResult;
        }

        private CampaignProfileFormModel GetEditData(int id, int userId)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            CurrencyModel currencyModel = new CurrencyModel();
            var UserId = userId;
            if(_campaignProfileRepository.Count(x => x.CampaignProfileId == id && (x.Status != 5)) == 0) return null; //&& x.UserId == UserId
            CampaignProfile campaignProfile = _campaignProfileRepository.GetById(id);
            ViewBag.campaignName = campaignProfile.CampaignName;
            if (campaignProfile.NumberInBatch == 0) campaignProfile.NumberInBatch = 1;
            CampaignProfileFormModel model = Mapper.Map<CampaignProfile, CampaignProfileFormModel>(campaignProfile);
            if (campaignProfile.CurrencyCode != null)
            {
                var CurrencyId = _currencyRepository.Get(top => top.CurrencyCode == campaignProfile.CurrencyCode).CurrencyId;
                model.CurrencyId = CurrencyId;
            }
            else
            {
                var CurrencyId = _currencyRepository.Get(top => top.CurrencyCode == "KES").CurrencyId;
                model.CurrencyId = CurrencyId;
            }
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59);
            DateTime previousMonthStart;
            DateTime previousMonthEnd;
            if (DateTime.Now.Month == 1)
            {
                previousMonthStart = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
                previousMonthEnd = new DateTime(DateTime.Now.Year - 1, 12, DateTime.DaysInMonth(DateTime.Now.Year - 1, 12), 23, 59, 59);
            }
            else
            {
                previousMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0);
                previousMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1), 23, 59, 59);
            }

            var relatedAudit = model.GetDomainCampaignAudits(_campaignAuditRepository);
            int currentMonthPlayCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd);
            int previousMonthPlayCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd);
            int previousMonthSMSCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.SMS == "1");
            int previousMonthEmailCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.Email == "1");
            int currentMonthSMSCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.SMS == "1");
            int currentMonthEmailCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.Email == "1");
            int totalPlayCount = relatedAudit.Count();
            int totalSMSCount = relatedAudit.Count(x => x.SMS == "1");
            int totalEmailCount = relatedAudit.Count(x => x.Email == "1");

            Contacts contacts = _contactsRepository.Get(c => c.UserId == UserId);
            var userCountryId = contacts.CountryId;
            string currencyCode = "";
            var userCurrencyId = contacts.CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            {
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            double playcostdata = 0.00;
            double emailcostdata = 0.00;
            double smscostdata = 0.00;
            double totalcost = 0.00;
            double totalcredit = 0;
            double spendtodate = 0;
            double playcost = 0;
            double emailcost = 0;
            double smscost = 0;
            decimal currencyRate = 0.00M;
            string fromCurrencyCode = _campaignProfileRepository.GetById(id).CurrencyCode;
            string toCurrencyCode = currencyCode;
            foreach (var item in campaignProfile.CampaignAudits)
            {
                if (toCurrencyCode == fromCurrencyCode)
                {
                    playcostdata = Convert.ToDouble(item.BidValue.ToString());
                    emailcostdata = Convert.ToDouble(item.EmailCost.ToString());
                    smscostdata = Convert.ToDouble(item.SMSCost.ToString());
                    totalcost = Convert.ToDouble(item.TotalCost.ToString());
                    playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
                    emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
                    smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        playcostdata = Convert.ToDouble(Convert.ToDecimal(item.BidValue) * currencyRate);
                        emailcostdata = Convert.ToDouble(Convert.ToDecimal(item.EmailCost) * currencyRate);
                        smscostdata = Convert.ToDouble(Convert.ToDecimal(item.SMSCost) * currencyRate);
                        totalcost = Convert.ToDouble(Convert.ToDecimal(item.TotalCost) * currencyRate);
                        playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
                        emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
                        smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
                    }
                }
            }
            ViewData.Add("previousMonthPlayCount", previousMonthPlayCount);
            ViewData.Add("previousMonthSMSCount", previousMonthSMSCount);
            ViewData.Add("previousMonthEmailCount", previousMonthEmailCount);
            ViewData.Add("currentMonthSMSCount", currentMonthSMSCount);
            ViewData.Add("currentMonthEmailCount", currentMonthEmailCount);
            ViewData.Add("totalPlayCount", totalPlayCount);
            ViewData.Add("currentMonthPlayCount", currentMonthPlayCount);
            ViewData.Add("totalSMSCount", totalSMSCount);
            ViewData.Add("totalEmailCount", totalEmailCount);
            string status = "";
            if (campaignProfile.Status == 8)
            {
                string campaignstatus = "Campaign Paused Due To Insufficient Funds";
                status = campaignstatus.ToString();
            }
            else if (campaignProfile.Status == 7) status = "InProgress";
            else
            {
                CampaignStatus campaignstatus = (CampaignStatus)campaignProfile.Status;
                status = campaignstatus.ToString();
            }
            ViewData.Add("status", status);
            totalcredit = (double)campaignProfile.TotalCredit;
            spendtodate = playcost + emailcost + smscost;
            ViewData["maxspendtodate"] = RoundUp(spendtodate, 2);
            ViewData["maxremainding"] = RoundUp((totalcredit - spendtodate), 2);
            ViewData.Add("statuscheck", campaignProfile.Status);
            return model;
        }

        private void GetCampaignbidplaydata(List<CampaignNoOfPlay> _campaignNoofplay, List<CampaignAvgbid> _campaignAvgbid, List<Campaignchartresult> _result, DateTime? maxdate, DateTime? mindate)
        {
            //get data for campaignbar+line chart

            if (_campaignNoofplay.Count > 0)
            {
                _campaignNoofplay = _campaignNoofplay.Where(top => top.startdate.Year == DateTime.Now.Year).ToList();
                _campaignAvgbid = _campaignAvgbid.Where(top => top.startdate.Year == DateTime.Now.Year).ToList();
                if (_campaignNoofplay.Count > 0)
                {


                    var maxcampaignplaydate = _campaignNoofplay.Max(top => top.startdatecompare);
                    var mincampaignplaydate = _campaignNoofplay.Min(top => top.startdatecompare);

                    var totaldayscampaigncount = maxcampaignplaydate - mincampaignplaydate;

                    if (totaldayscampaigncount.Days >= 50)
                    {
                        //get noof playdata
                        var xNoofplay = (from F in _campaignNoofplay
                                         where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                         group F by new { Week = Math.Floor((decimal)F.startdate.DayOfYear / 7) } into FGroup
                                         orderby FGroup.Key.Week

                                         select new NoOfPlayCampaign
                                         {
                                             name = (int)(FGroup.Key.Week),
                                             value = FGroup.Count()
                                         }).ToList();
                        //get noof avgbidValue
                        var xAvgbid = (from F in _campaignAvgbid
                                       where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                       group F by new { Week = Math.Floor((decimal)F.startdate.DayOfYear / 7) } into FGroup
                                       orderby FGroup.Key.Week
                                       select new NoOfAvgbidCampaign
                                       {
                                           name = (int)(FGroup.Key.Week),
                                           value = FGroup.Average(t => t.bidvalue)
                                       }).ToList();
                        if (xNoofplay.Count() > 0)
                        {
                            ViewBag.NoofplayMaxCount = xNoofplay.Max(top => top.value) + 1;
                        }
                        else
                        {
                            ViewBag.NoofplayMaxCount = 0;
                        }
                        if (xAvgbid.Count() > 0)
                        {
                            ViewBag.AvgbidMaxCount = xAvgbid.Max(top => top.value) + 5;
                        }
                        else
                        {
                            ViewBag.AvgbidMaxCount = 0;
                        }

                        _result.Add(new Campaignchartresult { status = 1, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });
                    }
                    else if (totaldayscampaigncount.Days >= 3 && totaldayscampaigncount.Days <= 49)
                    {
                        //get noof playdata
                        var xNoofplay = (from F in _campaignNoofplay
                                         where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                         group F by new { DayName = F.startdate.DayOfWeek } into FGroup
                                         orderby FGroup.Key.DayName
                                         select new NoOfPlayCampaign
                                         {
                                             name = Convert.ToInt32(FGroup.Key.DayName),
                                             value = FGroup.Count()
                                         }).ToList();

                        //get noof avgbidValue
                        var xAvgbid = (from F in _campaignAvgbid
                                       where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                       group F by new { DayName = F.startdate.DayOfWeek } into FGroup
                                       orderby FGroup.Key.DayName
                                       select new NoOfAvgbidCampaign
                                       {
                                           name = Convert.ToInt32(FGroup.Key.DayName),
                                           value = FGroup.Average(t => t.bidvalue)
                                       }).ToList();
                        if (xNoofplay.Count() > 0)
                        {
                            ViewBag.NoofplayMaxCount = xNoofplay.Max(top => top.value) + 1;
                        }
                        else
                        {
                            ViewBag.NoofplayMaxCount = 0;
                        }
                        if (xAvgbid.Count() > 0)
                        {
                            ViewBag.AvgbidMaxCount = xAvgbid.Max(top => top.value) + 5;
                        }
                        else
                        {
                            ViewBag.AvgbidMaxCount = 0;
                        }
                        _result.Add(new Campaignchartresult { status = 2, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });
                    }
                    else
                    {
                        //get noof playdata
                        var xNoofplay = (from F in _campaignNoofplay
                                         where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                         group F by new { Hours = F.startdate.Hour } into FGroup
                                         orderby FGroup.Key.Hours
                                         select new NoOfPlayCampaign
                                         {
                                             name = Convert.ToInt32(FGroup.Key.Hours),
                                             value = FGroup.Count()
                                         }).ToList();
                        //get noof avgbidValue
                        var xAvgbid = (from F in _campaignAvgbid
                                       where F.startdate.Date >= mindate.Value.Date && F.startdate <= maxdate.Value.Date
                                       group F by new { Hours = F.startdate.Hour } into FGroup
                                       orderby FGroup.Key.Hours
                                       select new NoOfAvgbidCampaign
                                       {
                                           name = Convert.ToInt32(FGroup.Key.Hours),
                                           value = FGroup.Average(t => t.bidvalue)
                                       }).ToList();
                        if (xNoofplay.Count() > 0)
                        {
                            ViewBag.NoofplayMaxCount = xNoofplay.Max(top => top.value) + 1;
                        }
                        else
                        {
                            ViewBag.NoofplayMaxCount = 0;
                        }
                        if (xAvgbid.Count() > 0)
                        {
                            ViewBag.AvgbidMaxCount = xAvgbid.Max(top => top.value) + 5;
                        }
                        else
                        {
                            ViewBag.AvgbidMaxCount = 0;
                        }
                        _result.Add(new Campaignchartresult { status = 3, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });
                    }
                    ViewBag.Campaignavgplayresult = _result;
                }
                else
                {
                    List<NoOfPlayCampaign> xNoofplay = new List<NoOfPlayCampaign>();
                    xNoofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                    List<NoOfAvgbidCampaign> xAvgbid = new List<NoOfAvgbidCampaign>();
                    xAvgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                    _result.Add(new Campaignchartresult { status = 3, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });

                    ViewBag.AvgbidMaxCount = 0;
                    ViewBag.NoofplayMaxCount = 0;
                    ViewBag.Campaignavgplayresult = _result;
                }
            }
            else
            {
                List<NoOfPlayCampaign> xNoofplay = new List<NoOfPlayCampaign>();
                xNoofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                List<NoOfAvgbidCampaign> xAvgbid = new List<NoOfAvgbidCampaign>();
                xAvgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                _result.Add(new Campaignchartresult { status = 3, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });

                ViewBag.AvgbidMaxCount = 0;
                ViewBag.NoofplayMaxCount = 0;
                ViewBag.Campaignavgplayresult = _result;
            }
        }
        public int[] _getbarChartData(List<MaxLengthGroup> _data)
        {
            int[] _barData = new int[9];
            if (_data.Count > 0)
            {
                _barData[0] = _data.Where(top => top.second >= 6 && top.second <= 9).Count();
                _barData[1] = _data.Where(top => top.second >= 9 && top.second <= 12).Count();
                _barData[2] = _data.Where(top => top.second >= 12 && top.second <= 15).Count();
                _barData[3] = _data.Where(top => top.second >= 15 && top.second <= 18).Count();
                _barData[4] = _data.Where(top => top.second >= 18 && top.second <= 21).Count();
                _barData[5] = _data.Where(top => top.second >= 21 && top.second <= 24).Count();
                _barData[6] = _data.Where(top => top.second >= 24 && top.second <= 27).Count();
                _barData[7] = _data.Where(top => top.second >= 27 && top.second <= 30).Count();
                _barData[8] = _data.Where(top => top.second >= 30 && top.second <= 999).Count();
            }
            else
            {
                return null;
            }
            return _barData;
        }
    }
}