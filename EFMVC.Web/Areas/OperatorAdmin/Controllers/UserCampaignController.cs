using AutoMapper;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.EF;
using EFMVC.Web.Areas.OperatorAdmin.Models;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using EFMVC.Web.Models;

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("UserCampaign")]
    public class UserCampaignController : Controller
    {
        // GET: OperatorAdmin/UserCampaign

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companyDetailsRepository;

        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _contact repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _campaign repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;

        /// <summary>
        /// The _currency repository
        /// </summary>
        private readonly ICurrencyRepository _currencyRepository;

        /// <summary>
        /// The _currencyRate repository
        /// </summary>
        private readonly ICurrencyRateRepository _currencyRateRepository;

        private readonly CurrencyConversion _currencyConversion;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserCampaignController(ICommandBus commandBus, IUserRepository userRepository, ICompanyDetailsRepository companyDetailsRepository, IOperatorRepository operatorRepository, IContactsRepository contactsRepository, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, IAdvertRepository advertRepository, IBillingRepository billingRepository, ICurrencyRepository currencyRepository, ICurrencyRateRepository currencyRateRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _companyDetailsRepository = companyDetailsRepository;
            _operatorRepository = operatorRepository;
            _contactsRepository = contactsRepository;
            _clientRepository = clientRepository;
            _profileRepository = profileRepository;
            _advertRepository = advertRepository;
            _billingRepository = billingRepository;
            _currencyRepository = currencyRepository;
            _currencyRateRepository = currencyRateRepository;
            _currencyConversion = Common.CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        [Route("Index")]
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var operatorAdminId = efmvcUser.UserId;
            var operatorId = _userRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault().OperatorId;
            var countryId = _operatorRepository.GetById(operatorId).CountryId;

            List<UserCampaignResult> _result = new List<UserCampaignResult>();
            UserCampaignResult campaignResult = new UserCampaignResult();
            UserCampaignFilter _filterCritearea = new UserCampaignFilter();
            CurrencySymbol currencySymbol = new CurrencySymbol();

            FillUserDropdown(countryId, operatorId);
            FillClientDropdown(0);
            FillCampaignDropdown(0);
            TempData["CountryId"] = countryId;

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
            campaignResult.CurrencyCode = currencyCode;
            _result.Add(campaignResult);
            return View(Tuple.Create(_result, _filterCritearea));
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault().OperatorId;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                var cnt = 10;
                int countryId = 0;

                if (TempData["CountryId"] == null)
                {
                    countryId = 0;
                }
                else
                {
                    countryId = Convert.ToInt32(TempData["CountryId"].ToString());
                }

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }

                List<Model.CampaignProfile> campaignProfiles = null;
                List<UserCampaignResult> _result = new List<UserCampaignResult>();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();

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

                if (searchValue == true)
                {
                    int[] UserId = new int[cnt];
                    int?[] ClientId = new int?[cnt];
                    int[] CampaignId = new int[cnt];
                    DateTime fromdate = new DateTime();
                    DateTime todate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            UserId = columnSearch[0].Split(',').Select(int.Parse).ToArray();
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
                            ClientId = columnSearch[1].Split(',').Select(top => (int?)Convert.ToInt32(top)).ToArray();
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
                            CampaignId = columnSearch[2].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }
                    if (!String.IsNullOrEmpty(columnSearch[6]))
                    {
                        if (columnSearch[6] != "null")
                        {
                            var data = columnSearch[6].Split(',').ToArray();
                            string strTodate = data[1];
                            DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string strFromdate = data[0];
                            DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            fromdate = Fromdate;
                            todate = Todate;
                        }
                        else
                        {
                            columnSearch[6] = null;

                        }
                    }

                    if (UserId.Count() > 0)
                    {
                        campaignProfiles = _profileRepository.GetMany(top => UserId.Contains(top.UserId) && (top.Status != 5)).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }
                    else
                    {
                        campaignProfiles = null;
                    }

                    if (columnSearch[1] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => ClientId.Contains(top.ClientId)).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => CampaignId.Contains(top.CampaignProfileId)).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (columnSearch[6] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => top.CreatedDateTime.Date >= fromdate && top.CreatedDateTime.Date <= todate).ToList();
                    }

                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length).ToList();
                }

                if (campaignProfiles != null)
                {
                    foreach (var item in campaignProfiles)
                    {
                        //calculate average bid that has status Played.
                        IEnumerable<CampaignAuditFormModel> CampaignAuditData = null;
                        var play = CampaignAuditStatusExtensions.PlayedAsLowerCase;

                        EFMVCDataContex db = new EFMVCDataContex();
                        CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == play && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                                            .Select(s => new CampaignAuditFormModel { CampaignAuditId = s.CampaignAuditId, CampaignProfileId = s.CampaignProfileId, UserProfileId = s.UserProfileId, BidValue = s.BidValue, TotalCost = s.TotalCost });

                        var totalbidval = 0.00M;
                        double totalspend = 0;

                        var userProfileAdvertCount = CampaignAuditData.Select(s => s.UserProfileId).Distinct().Count();

                        var totalPlayCnt = db.CampaignAudits.Where(top => top.Status.ToLower() == play && top.CampaignProfileId == item.CampaignProfileId).Count();
                        var finaltotalplays = totalPlayCnt;
                        var bidresult = CampaignAuditData;
                        totalbidval = Convert.ToDecimal(bidresult.Sum(top => top.BidValue));
                        var playCount = CampaignAuditData.Count();
                        if (totalbidval > 0)
                        {
                            totalbidval = totalbidval / playCount;
                        }

                        totalspend = CampaignAuditData.Sum(s => s.TotalCost);

                        decimal totalspendamount = 0.00M;
                        decimal totalaveragebidval = 0.00M;
                        decimal totalbudget = 0.00M;
                        decimal currencyRate = 0.00M;
                        string fromCurrencyCode = item.CurrencyCode;
                        string toCurrencyCode = currencyCode;

                        if (toCurrencyCode == fromCurrencyCode)
                        {
                            totalspendamount = Convert.ToDecimal(totalspend.ToString("F2"));
                            totalaveragebidval = Convert.ToDecimal(totalbidval.ToString("F2"));
                            totalbudget = Convert.ToDecimal(item.TotalBudget.ToString("F2"));

                            totalspendamount = decimal.Round((totalspendamount), 2, MidpointRounding.AwayFromZero);
                            totalaveragebidval = decimal.Round((totalaveragebidval), 2, MidpointRounding.AwayFromZero);
                            totalbudget = decimal.Round((totalbudget), 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            totalspendamount = Convert.ToDecimal(Convert.ToDecimal(totalspend).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
                            totalaveragebidval = Convert.ToDecimal(totalbidval).ConvertToDisplay(_currencyConversion, fromCurrencyCode);
                            totalbudget = Convert.ToDecimal(item.TotalBudget).ConvertToDisplay(_currencyConversion, fromCurrencyCode);

                            totalspendamount = decimal.Round((totalspendamount), 2, MidpointRounding.AwayFromZero);
                            totalaveragebidval = decimal.Round((totalaveragebidval), 2, MidpointRounding.AwayFromZero);
                            totalbudget = decimal.Round((totalbudget), 2, MidpointRounding.AwayFromZero);
                        }

                        string status = String.Empty;
                        if (item.Status == 2)
                        {
                            var billingDetails = _billingRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId).ToList();
                            if (billingDetails.Count() == 0)
                            {
                                CampaignStatus campaignStatus = (CampaignStatus)8;
                                status = campaignStatus.ToString();
                            }
                            else
                            {
                                CampaignStatus campaignStatus = (CampaignStatus)item.Status;
                                status = campaignStatus.ToString();
                            }
                        }
                        else
                        {
                            CampaignStatus campaignStatus = (CampaignStatus)item.Status;
                            status = campaignStatus.ToString();
                        }

                        if (item.IsAdminApproval == false)
                        {
                            status = "Waiting for admin approval";
                        }
                        if(item.IsAdminApproval == false && item.Status == 7)
                        {
                            CampaignStatus campaignStatus = (CampaignStatus)item.Status;
                            status = campaignStatus.ToString();
                        }

                        string companyName = String.Empty;
                        string mobileNumber = String.Empty;
                        if (item.User != null)
                        {
                            var contactDetails = db.Contacts.Where(top => top.UserId == item.UserId).FirstOrDefault();
                            if (contactDetails != null)
                            {
                                mobileNumber = contactDetails.MobileNumber;
                            }
                            else
                            {
                                mobileNumber = "-";
                            }
                        }
                        else
                        {
                            mobileNumber = "-";
                        }
                        if (item.User != null)
                        {
                            var companyDetails = _companyDetailsRepository.Get(top => top.UserId == item.UserId);
                            if(companyDetails != null)
                            {
                                companyName = companyDetails.CompanyName;
                            }
                            else
                            {
                                companyName = "-";
                            }
                        }
                        else
                        {
                            companyName = "-";
                        }

                        _result.Add(new UserCampaignResult
                        {
                            CampaignProfileId = item.CampaignProfileId,
                            CampaignName = item.CampaignName,
                            ClientId = item.ClientId,
                            ClientName = item.ClientId == 0 ? "-" : item.ClientId == null ? "-" : item.Client.Name,
                            UserId = item.UserId,
                            AdvertiserCompanyName = companyName,
                            AdvertiserMobileNumber = mobileNumber,
                            Status = status == "Waitingforadapproval" ? "Waiting for ad approval" : status == "InProgress" ? "In Progress" : status == "CampaignPausedDueToInsufficientFunds" ? "Campaign Paused Due To Insufficient Funds" : status,
                            TotalBudget = totalbudget,
                            TotalAverageBid = totalaveragebidval,
                            TotalSpend = totalspendamount,
                            CurrencyCode = currencysymbol,
                            CountryId = userCountryId,
                            CurrencyId = userCurrencyId,
                            CreatedDateTime = item.CreatedDateTime,
                            DisplayCreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy")
                        });
                    }
                }

                if (!String.IsNullOrEmpty(columnSearch[3]))
                {
                    if (columnSearch[3] != "null")
                    {
                        var data = columnSearch[3].Split(',').ToArray();
                        var FromTotalBudget = decimal.Parse(data[0]);
                        var ToTotalBudget = decimal.Parse(data[1]);

                        _result = _result.Where(top => top.TotalBudget >= FromTotalBudget && top.TotalBudget <= ToTotalBudget).ToList();
                        cnt = _result.Count();
                        _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(columnSearch[4]))
                {
                    if (columnSearch[4] != "null")
                    {
                        var data = columnSearch[4].Split(',').ToArray();
                        var FromTotalAvgBid = int.Parse(data[0]);
                        var ToTotalAvgBid = int.Parse(data[1]);

                        _result = _result.Where(top => top.TotalAverageBid >= FromTotalAvgBid && top.TotalAverageBid <= ToTotalAvgBid).ToList();
                        cnt = _result.Count();
                        _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    }
                }

                if (!String.IsNullOrEmpty(columnSearch[5]))
                {
                    if (columnSearch[5] != "null")
                    {
                        var data = columnSearch[5].Split(',').ToArray();
                        var FromTotalSpend = decimal.Parse(data[0]);
                        var ToTotalSpend = decimal.Parse(data[1]);

                        _result = _result.Where(top => top.TotalSpend >= FromTotalSpend && top.TotalSpend <= ToTotalSpend).ToList();
                        cnt = _result.Count();
                        _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    }
                }

                _result = ApplySorting(param, _result);
                //dtsource = dtsource.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserCampaignResult> result = new DTResult<UserCampaignResult>
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

        // Function For Filter/Sorting Campaign Profile Data
        private static List<UserCampaignResult> ApplySorting(DTParameters param, List<UserCampaignResult> dtsource)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.AdvertiserCompanyName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.AdvertiserCompanyName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.AdvertiserMobileNumber).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.AdvertiserMobileNumber).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.ClientName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.CampaignName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.TotalBudget).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.TotalBudget).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.TotalAverageBid).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.TotalAverageBid).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.TotalSpend).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.TotalSpend).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.CreatedDateTime).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.CreatedDateTime).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.Status).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.Status).ToList();
                }
            }
            return dtsource;
        }

        public double CurrencyConversion(decimal amount, string currency)
        {
            var exchangeRate = _currencyRateRepository.Get(top => top.CurrencyCode == currency);
            double rate = Convert.ToDouble(amount) * Convert.ToDouble(exchangeRate.CurrencyRateAmount);
            return rate;
        }

        public void FillUserDropdown(int? countryId, int operatorId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (countryId != 0 || countryId != null)
            {
                var advertUserId = _advertRepository.GetMany(top => top.OperatorId == operatorId).Select(top => top.UserId).ToList();
                //var userId = _contactsRepository.GetMany(top => top.CountryId == countryId.Value).Select(top => top.UserId);
                //var userdetails = _userRepository.GetAll().Where(top => userId.Contains(top.UserId) && top.RoleId == 3 && top.Activated == 1).Select(top => new
                //var userdetails = _userRepository.GetAll().Where(top => top.OperatorId == operatorId && top.RoleId == 3 && top.Activated == 1).Select(top => new
                var userdetails = _userRepository.GetAll().Where(top => advertUserId.Contains(top.UserId) && top.RoleId == 3 && top.Activated == 1).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    Id = top.UserId.ToString(),
                }).ToList();
                ViewBag.users = new MultiSelectList(userdetails, "Id", "Name");
            }
            else
            {
                ViewBag.users = null;
            }
        }

        public void FillClientDropdown(int? countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (countryId != 0)
            {
                var clientdetails = _clientRepository.GetMany(top => top.CountryId == countryId && (top.Status == 1 || top.Status == 2)).Select(top => new
                {
                    Name = top.Name,
                    Id = top.Id.ToString(),
                }).ToList();
                ViewBag.clients = new MultiSelectList(clientdetails, "Id", "Name");
            }
            else
            {
                List<Client> clientList = new List<Client>();
                var clientdetails = clientList.Select(top => new
                {
                    Name = top.Name,
                    Id = top.Id.ToString(),
                }).ToList();
                ViewBag.clients = new MultiSelectList(clientdetails, "Id", "Name");
            }
        }

        public void FillCampaignDropdown(int? countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (countryId != 0)
            {
                var campaigndetails = _profileRepository.GetMany(top => top.CountryId == countryId).Select(top => new
                {
                    Name = top.CampaignName,
                    Id = top.CampaignProfileId.ToString(),
                }).ToList();
                ViewBag.campaigns = new MultiSelectList(campaigndetails, "Id", "Name");
            }
            else
            {
                List<CampaignProfile> campaignList = new List<CampaignProfile>();
                var campaigndetails = campaignList.Select(top => new
                {
                    Name = top.CampaignName,
                    Id = top.CampaignProfileId.ToString(),
                }).ToList();
                ViewBag.campaigns = new MultiSelectList(campaigndetails, "Id", "Name");
            }
        }

        [Route("GetClientsByUserId")]
        [HttpPost]
        public ActionResult GetClientsByUserId(int[] userId)
        {
            try
            {
                if (userId != null)
                {
                    var clientdetails = _clientRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(clientdetails);
                }
                else
                {
                    var clientdetails = _clientRepository.GetAll().Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(null);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("GetCampaignsByUserId")]
        [HttpPost]
        public ActionResult GetCampaignsByUserId(int[] userId)
        {
            try
            {
                if (userId != null)
                {
                    var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId)) && top.Status != 5).Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(campaigndetails);
                }
                else
                {
                    var campaigndetails = _profileRepository.GetAll().Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(null);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("GetCampaignByClientId")]
        [HttpPost]
        public ActionResult GetCampaignByClientId(int[] clientId, int[] userId)
        {
            try
            {
                if (clientId != null)
                {
                    if (clientId != null)
                    {
                        var campaignList = _profileRepository.GetMany(top => top.ClientId != 0).ToList();
                        var campaigndetails = campaignList.Where(top => clientId.Contains((int)(top.ClientId.Value))).Select(top => new
                        {
                            Name = top.CampaignName,
                            Id = top.CampaignProfileId
                        }).ToList();
                        return Json(campaigndetails);
                    }
                    else
                    {
                        var campaigndetails = _profileRepository.GetAll().Select(top => new
                        {
                            Name = top.CampaignName,
                            Id = top.CampaignProfileId
                        }).ToList();
                        return Json(null);
                    }
                }
                else
                {
                    if (userId != null)
                    {
                        var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId)) && top.Status != 5).Select(top => new
                        {
                            Name = top.CampaignName,
                            Id = top.CampaignProfileId
                        }).ToList();
                        return Json(campaigndetails);
                    }
                    else
                    {
                        var campaigndetails = _profileRepository.GetAll().Select(top => new
                        {
                            Name = top.CampaignName,
                            Id = top.CampaignProfileId
                        }).ToList();
                        return Json(null);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }
    }
}