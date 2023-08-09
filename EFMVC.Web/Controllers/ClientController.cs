using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.SearchClass;
using EFMVC.Web.ViewModels;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class ClientController : Controller
    {

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        //
        // GET: /Client/

        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IContactsRepository _contactRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly CurrencyConversion _currencyConversion;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public ClientController(ICommandBus commandBus, IClientRepository clientRepository, IUserRepository userRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository, ICurrencyRateRepository currencyRateRepository, IContactsRepository contactRepository, IOperatorRepository operatorRepository, ICampaignProfileRepository profileRepository)
        {
            _userRepository = userRepository;
            _commandBus = commandBus;
            _clientRepository = clientRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _currencyRateRepository = currencyRateRepository;
            _contactRepository = contactRepository;
            _operatorRepository = operatorRepository;
            _profileRepository = profileRepository;
            _currencyConversion = Common.CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            int[] clienstatusId = new int[] { 1, 2, 3, 4 };
            List<ClientResult> _result = new List<ClientResult>();
            ClientFilter _filterCritearea = new ClientFilter();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            string currencyCode = "";
            var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            { 
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2 || x.Status == 3 || x.Status == 4)).OrderByDescending(top => top.CreatedDate).ToList();
            IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillClient(clientModels);
            if (clienstatusId != null) clientModels = clientModels.Where(top => clienstatusId.Contains(top.Status));
            FillClientStatus();
            TempData["result"] = _result;
            ViewBag.currencyCode = currencyCode;
            ViewBag.userCountryId = userCountryId;
            ViewBag.userCurrencyId = userCurrencyId;
            return View(Tuple.Create(_result, _filterCritearea));
        }
        
        private List<ClientResult> GetClientResult(int[] ClientId, int[] clientStatusId)
        {
            List<ClientResult> _result = new List<ClientResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            CurrencyModel currencyModel = new CurrencyModel();
            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2 || x.Status == 3 || x.Status == 4)).OrderByDescending(top => top.Id).ToList();
            IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillClient(clientModels);
            if (clientStatusId != null) clientModels = clientModels.Where(top => clientStatusId.Contains(top.Status));
            FillClientStatus();
            var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            string currencysymbol = "";
            string currencyCode = "";
            var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryId, _currencyRepository);
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            if (clientModels.Count() > 0)
            {
                foreach (var item in clientModels)
                {
                    decimal totalbudget = 0;
                    var totalspend = 0;
                    var totalplays = 0;
                    var totalbidval = 0;
                    var totalcampaign = 0;
                    string status = string.Empty;
                    ClientStatus clientStatus = (ClientStatus)item.Status;
                    status = clientStatus.ToString();
                    var createDate = Convert.ToDateTime(item.CreatedDate.ToString("MM/dd/yyyy"));
                    if (item.CampaignProfiles.Count() > 0)
                    {
                        var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        DataTable Test = ExecuteLinkedSP("ClientDashboard", connection, item.Id);
                        totalcampaign = int.Parse(Test.Rows[0].ItemArray[0].ToString());
                        totalbudget = decimal.Parse(Test.Rows[0].ItemArray[1].ToString());
                        totalspend = int.Parse(Test.Rows[0].ItemArray[2].ToString());
                        totalplays = int.Parse(Test.Rows[0].ItemArray[3].ToString());
                        totalbidval = int.Parse(Test.Rows[0].ItemArray[4].ToString());
                        double budget = 0.00;
                        double spend = 0.00;
                        double bidval = 0.00;
                        decimal currencyRate = 0.00M;
                        string fromCurrencyCode = _profileRepository.GetById(item.CampaignProfiles.FirstOrDefault().CampaignProfileId).CurrencyCode;
                        string toCurrencyCode = currencyCode;
                        if (currencyCode == fromCurrencyCode)
                        {
                            budget = Convert.ToDouble(totalbudget.ToString());
                            spend = Convert.ToDouble(totalspend.ToString());
                            bidval = Convert.ToDouble(totalbidval.ToString());
                        }
                        else
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK")
                            {
                                budget = Convert.ToDouble(totalbudget * currencyRate);
                                spend = Convert.ToDouble(Convert.ToDecimal(totalspend) * currencyRate);
                                bidval = Convert.ToDouble(Convert.ToDecimal(totalbidval) * currencyRate);
                            }
                        }
                        _result.Add(new ClientResult { Id = item.Id, NoOfCompaign = totalcampaign, Name = item.Name, CreatedDate = createDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, TotalBudget = Convert.ToDecimal(budget.ToString("F2")), TotalSpend = Convert.ToDecimal(spend.ToString("F2")), TotalPlays = totalplays, AvgBid = Convert.ToDecimal(bidval.ToString("F2")), Status = status, fStatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode });
                    }
                    else
                    {
                        _result.Add(new ClientResult { Id = item.Id, NoOfCompaign = totalcampaign, Name = item.Name, CreatedDate = createDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, TotalBudget = Convert.ToDecimal(totalbudget.ToString("F2")), TotalSpend = Convert.ToDecimal(totalspend.ToString("F2")), TotalPlays = totalplays, AvgBid = Convert.ToDecimal(totalbidval.ToString("F2")), Status = status, fStatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode, userCurrencyId = userCurrencyId });
                    }
                }
            }
            ViewBag.currencyCode = currencyCode;
            ViewBag.userCountryId = userCountryId;
            ViewBag.userCurrencyId = userCurrencyId;
            return _result;
        }

        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<ClientResult> _result = new List<ClientResult>();
                IEnumerable<Client> client = null;
                DateTimeFormat dateTimeConvert = new DateTimeFormat();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;
                var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                string currencysymbol = "";
                string currencyCode = "";
                var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                if (userCurrencyId != null || userCurrencyId != 0)
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                    currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
                }
                else
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryId, _currencyRepository);
                    currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
                }
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
                    int?[] ClientId = new int?[cnt];
                    int[] StatusId = new int[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();
                    decimal TotalBudgetFrom = 0.00M;
                    decimal TotalBudgetTo = 0.00M;
                    decimal TotalSpendFrom = 0.00M;
                    decimal TotalSpendTo = 0.00M;
                    decimal TotalAvgBidFrom = 0.00M;
                    decimal TotalAvgBidTo = 0.00M;
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null") ClientId = columnSearch[0].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        else columnSearch[0] = null;
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            var data = columnSearch[2].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else columnSearch[2] = null;
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            var data = columnSearch[3].Split(',').ToArray();
                            TotalBudgetFrom = Convert.ToDecimal(data[0]);
                            TotalBudgetTo = Convert.ToDecimal(data[1]);
                        }
                        else columnSearch[3] = null;
                    }

                    if (!String.IsNullOrEmpty(columnSearch[4]))
                    {
                        if (columnSearch[4] != "null")
                        {
                            var data = columnSearch[4].Split(',').ToArray();
                            TotalSpendFrom = Convert.ToDecimal(data[0]);
                            TotalSpendTo = Convert.ToDecimal(data[1]);
                        }
                        else columnSearch[4] = null;
                    }

                    if (!String.IsNullOrEmpty(columnSearch[6]))
                    {
                        if (columnSearch[6] != "null")
                        {
                            var data = columnSearch[6].Split(',').ToArray();
                            TotalAvgBidFrom = Convert.ToDecimal(data[0]);
                            TotalAvgBidTo = Convert.ToDecimal(data[1]);
                        }
                        else columnSearch[6] = null;
                    }

                    if (!String.IsNullOrEmpty(columnSearch[7]))
                    {
                        if (columnSearch[7] != "null") StatusId = columnSearch[7].Split(',').Select(int.Parse).ToArray();
                        else columnSearch[7] = null;
                    }
                    client = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2 || x.Status == 3 || x.Status == 4)).OrderByDescending(top => top.Id).ToList();
                    if (client.Count() > 0)
                    {
                        foreach (var item in client)
                        {
                            decimal totalbudget = 0;
                            var totalspend = 0;
                            var totalplays = 0;
                            var totalbidval = 0;
                            var totalcampaign = 0;
                            string fstatus = string.Empty;
                            double budget = 0.00;
                            double spend = 0.00;
                            double bidval = 0.00;
                            DateTime? createDate = new DateTime();
                            ClientStatus clientStatus = (ClientStatus)item.Status;
                            status = clientStatus.ToString();
                            if (item.CreatedDate != null) createDate = Convert.ToDateTime(item.CreatedDate.Value.ToString("MM/dd/yyyy"));
                            else createDate = null;
                            if (item.CampaignProfiles.Count() > 0)
                            {
                                var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                                DataTable Test = ExecuteLinkedSP("ClientDashboard", connection, item.Id);
                                totalcampaign = int.Parse(Test.Rows[0].ItemArray[0].ToString());
                                totalbudget = decimal.Parse(Test.Rows[0].ItemArray[1].ToString());
                                totalspend = int.Parse(Test.Rows[0].ItemArray[2].ToString());
                                totalplays = int.Parse(Test.Rows[0].ItemArray[3].ToString());
                                totalbidval = int.Parse(Test.Rows[0].ItemArray[4].ToString());
                                decimal currencyRate = 0.00M;
                                string fromCurrencyCode = _profileRepository.GetById(item.CampaignProfiles.FirstOrDefault().CampaignProfileId).CurrencyCode;
                                string toCurrencyCode = currencyCode;
                                if (currencyCode == fromCurrencyCode)
                                {
                                    budget = Convert.ToDouble(totalbudget.ToString());
                                    spend = Convert.ToDouble(totalspend.ToString());
                                    bidval = Convert.ToDouble(totalbidval.ToString());
                                }
                                else
                                {
                                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                                    currencyRate = currencyModel.Amount;
                                    if (currencyModel.Code == "OK")
                                    {
                                        budget = Convert.ToDouble(totalbudget * currencyRate);
                                        spend = Convert.ToDouble(Convert.ToDecimal(totalspend) * currencyRate);
                                        bidval = Convert.ToDouble(Convert.ToDecimal(totalbidval) * currencyRate);
                                    }
                                }
                            }
                            _result.Add(new ClientResult { Id = item.Id, NoOfCompaign = totalcampaign, Name = item.Name, CreatedDate = createDate == null ? null : createDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate == null ? null : item.CreatedDate, TotalBudget = Convert.ToDecimal(budget.ToString("F2")), TotalSpend = Convert.ToDecimal(spend.ToString("F2")), TotalPlays = totalplays, AvgBid = Convert.ToDecimal(bidval.ToString("F2")), Status = fstatus, fStatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode, userCurrencyId = userCurrencyId });
                            if (columnSearch[0] != null) _result = _result.Where(top => (ClientId.Contains(Convert.ToInt32(top.Id)))).ToList();
                            if (columnSearch[2] != null) _result = _result.Where(top => (top.CreatedDateSort >= CreatedDatefromdate && top.CreatedDateSort <= CreatedDatetodate)).ToList();
                            if (columnSearch[3] != null) _result = _result.Where(top => (top.TotalBudget >= TotalBudgetFrom && top.TotalBudget <= TotalBudgetTo)).ToList();
                            if (columnSearch[4] != null) _result = _result.Where(top => (top.AvgBid >= TotalAvgBidFrom && top.AvgBid <= TotalAvgBidFrom)).ToList();
                            if (columnSearch[7] != null) _result = _result.Where(top => (StatusId.Contains((int)top.fStatus))).ToList();
                        }
                        cnt = _result.Count();
                        _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    }
                    #endregion
                }
                else
                {
                    client = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2 || x.Status == 3 || x.Status == 4)).OrderByDescending(top => top.Id).ToList();
                    if (client.Count() > 0)
                    {
                        foreach (var item in client)
                        {
                            decimal totalbudget = 0;
                            var totalspend = 0;
                            var totalplays = 0;
                            var totalbidval = 0;
                            var totalcampaign = 0;
                            string fstatus = string.Empty;
                            double budget = 0.00;
                            double spend = 0.00;
                            double bidval = 0.00;
                            DateTime? createDate = new DateTime();
                            ClientStatus clientStatus = (ClientStatus)item.Status;
                            status = clientStatus.ToString();
                            if (item.CreatedDate != null) createDate = Convert.ToDateTime(item.CreatedDate.Value.ToString("MM/dd/yyyy"));
                            else createDate = null;
                            if (item.CampaignProfiles.Count() > 0)
                            {
                                var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                                DataTable Test = ExecuteLinkedSP("ClientDashboard", connection, item.Id);
                                totalcampaign = int.Parse(Test.Rows[0].ItemArray[0].ToString());
                                totalbudget = decimal.Parse(Test.Rows[0].ItemArray[1].ToString());
                                totalspend = int.Parse(Test.Rows[0].ItemArray[2].ToString());
                                totalplays = int.Parse(Test.Rows[0].ItemArray[3].ToString());
                                totalbidval = int.Parse(Test.Rows[0].ItemArray[4].ToString());
                                decimal currencyRate = 0.00M;
                                string fromCurrencyCode = _profileRepository.GetById(item.CampaignProfiles.FirstOrDefault().CampaignProfileId).CurrencyCode;
                                string toCurrencyCode = currencyCode;
                                if (currencyCode == fromCurrencyCode)
                                {
                                    budget = Convert.ToDouble(totalbudget.ToString());
                                    spend = Convert.ToDouble(totalspend.ToString());
                                    bidval = Convert.ToDouble(totalbidval.ToString());
                                }
                                else
                                {
                                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                                    currencyRate = currencyModel.Amount;
                                    if (currencyModel.Code == "OK")
                                    {
                                        budget = Convert.ToDouble(totalbudget * currencyRate);
                                        spend = Convert.ToDouble(Convert.ToDecimal(totalspend) * currencyRate);
                                        bidval = Convert.ToDouble(Convert.ToDecimal(totalbidval) * currencyRate);
                                    }
                                }
                            }
                            _result.Add(new ClientResult { Id = item.Id, NoOfCompaign = totalcampaign, Name = item.Name, CreatedDate = createDate == null ? null : createDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate == null ? null : item.CreatedDate, TotalBudget = Convert.ToDecimal(budget.ToString("F2")), TotalSpend = Convert.ToDecimal(spend.ToString("F2")), TotalPlays = totalplays, AvgBid = Convert.ToDecimal(bidval.ToString("F2")), Status = fstatus, fStatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode, userCurrencyId = userCurrencyId });
                        }
                        cnt = _result.Count();
                        _result = _result.Skip(param.Start).Take(param.Length).ToList();
                    }
                }
                _result = ApplySorting(param, _result);
                DTResult<ClientResult> result = new DTResult<ClientResult>
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
        
        // Function For Filter/Sorting Client Data
        private static List<ClientResult> ApplySorting(DTParameters param, List<ClientResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.NoOfCompaign).ToList();
                    else
                        result = result.OrderByDescending(top => top.NoOfCompaign).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalBudget).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalBudget).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalSpend).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalSpend).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalPlays).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalPlays).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.AvgBid).ToList();
                    else
                        result = result.OrderByDescending(top => top.AvgBid).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fStatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fStatus).ToList();
                }
            }
            return result;
        }

        public List<ClientResult> getclientResult(List<ClientResult> clientresult, ClientFilter _filterCritearea, int[] ClientId, int[] clientStatusId)
        {
            if (clientresult != null && _filterCritearea != null)
            {
                if (ClientId != null) clientresult = clientresult.Where(top => ClientId.Contains(top.Id)).ToList();
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    clientresult = clientresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();
                }
                if (clientStatusId != null) clientresult = clientresult.Where(top => clientStatusId.Contains(top.fStatus)).ToList();
                if (!String.IsNullOrEmpty(_filterCritearea.FromBudget) && !String.IsNullOrEmpty(_filterCritearea.ToBudget))
                {
                    clientresult = clientresult.Where(top => top.TotalBudget >= Convert.ToDecimal(_filterCritearea.FromBudget) && top.TotalBudget <= Convert.ToDecimal(_filterCritearea.ToBudget)).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromSpend) && !String.IsNullOrEmpty(_filterCritearea.ToSpend))
                {
                    clientresult = clientresult.Where(top => top.TotalSpend >= Convert.ToDecimal(_filterCritearea.FromSpend) && top.TotalSpend <= Convert.ToDecimal(_filterCritearea.ToSpend)).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.Frombid) && !String.IsNullOrEmpty(_filterCritearea.Tobid))
                {
                    clientresult = clientresult.Where(top => top.AvgBid >= Convert.ToDecimal(_filterCritearea.Frombid) && top.AvgBid <= Convert.ToDecimal(_filterCritearea.Tobid)).ToList();
                }
            }
            else
            {
                if (clientresult != null)
                {
                    if (clientStatusId != null) clientresult = clientresult.Where(top => clientStatusId.Contains(top.fStatus)).ToList();
                }
            }
            return clientresult;
        }

        private void FillClientStatus()
        {
            IEnumerable<Common.ClientStatus> cStatus = Enum.GetValues(typeof(Common.ClientStatus))
                                              .Cast<Common.ClientStatus>();
            var clientStatus = (from action in cStatus
                                select new { Text = action.ToString(), Value = ((int)action).ToString() }).ToList();
            ViewBag.clientStatus = new MultiSelectList(clientStatus, "Value", "Text", new int[] { });
        }

        public ActionResult SearchClient([Bind(Prefix = "Item2")]ClientFilter _filterCritearea, int[] ClientId, int[] clientStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ClientResult> _result = new List<ClientResult>();
                var finalresult = new List<ClientResult>();
                if (_filterCritearea != null)
                {
                    _result = GetClientResult(null, null);
                    finalresult = getclientResult(_result, _filterCritearea, ClientId, clientStatusId);
                }
                else
                {
                    _result = GetClientResult(null, null);
                    finalresult = getclientResult(_result, _filterCritearea, ClientId, clientStatusId);
                }
                TempData["result"] = finalresult;
                return PartialView("_ClientList", finalresult);
            }
            else
            {
                return PartialView("_ClientList", "notauthorise");
            }
        }

        private void FillClient(IEnumerable<ClientModel> clientModels)
        {
            var clientdetails = clientModels.Select(top => new { Name = top.Name, Id = top.Id }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
        }

        public void FillStatus()
        {
            IEnumerable<Common.ClientStatus> clientstatusTypes = Enum.GetValues(typeof(Common.ClientStatus)).Cast<Common.ClientStatus>();
            var clientStatus = (from action in clientstatusTypes select new SelectListItem { Text = action.ToString(), Value = ((int)action).ToString() }).ToList();
            ViewBag.clientStatus = clientStatus;
        }
        public void FillCountry()
        {
            var countryId = _operatorRepository.GetMany(s => s.IsActive).Select(c => c.CountryId).ToList();
            var country = (from action in _countryRepository.GetMany(c => countryId.Contains(c.Id)).OrderBy(c => c.Name) select new SelectListItem { Text = action.Name, Value = action.Id.ToString() }).ToList();
            ViewBag.country = country;
        }

        public double CurrencyConversion(decimal amount, string currency)
        {
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            var exchangeRate = _currencyRateRepository.Get(top => top.CurrencyCode == currency);
            double rate = Convert.ToDouble(amount) * Convert.ToDouble(exchangeRate.CurrencyRateAmount);
            return rate;
        }

        public ActionResult AddClient()
        {
            FillCountry();
            FillStatus();
            FillCurrencyList();
            return View();
        }

        public ActionResult EditClient(int? id)
        {
            FillStatus();
            FillCountry();
            FillCurrencyList();
            var _clientdetails = _clientRepository.Get(x => x.Id == id);
            if (_clientdetails != null)
            {
                ClientModel client = Mapper.Map<Client, ClientModel>(_clientdetails);
                ViewBag.clientname = client.Name;
                ViewBag.status = client.Status;
                return View(client);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddClient(ClientModel _client)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (_client.Id == 0) _client.CreatedDate = DateTime.Now;
                _client.UpdatedDate = DateTime.Now;
                _client.UserId = efmvcUser.UserId;
                _client.Name = _client.Name.Trim();
                var companyexists = _clientRepository.Get(top => top.Name.Trim().ToLower() == _client.Name.Trim().ToLower() && top.UserId == efmvcUser.UserId);
                if (companyexists != null)
                {
                    TempData["Error"] = "Client name already exists.";
                    FillStatus();
                    return View(_client);
                }
                CreateOrUpdateClientCommand command = Mapper.Map<ClientModel, CreateOrUpdateClientCommand>(_client);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["msgsuccess"] = "Client " + _client.Name + " added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(_client);
        }

        [HttpPost]
        public ActionResult UpdateClient(ClientModel _client)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (_client.Id == 0) _client.CreatedDate = DateTime.Now;
                _client.UpdatedDate = DateTime.Now;
                _client.UserId = efmvcUser.UserId;
                _client.Name = _client.Name.Trim();
                var companyexists = _clientRepository.Get(top => top.Name.Trim().ToLower() == _client.Name.Trim().ToLower() && top.UserId == efmvcUser.UserId && top.Id != _client.Id);
                if (companyexists != null)
                {
                    TempData["Error"] = "Client name already exists.";
                    FillStatus();
                    return View("EditClient", _client);
                }
                CreateOrUpdateClientCommand command = Mapper.Map<ClientModel, CreateOrUpdateClientCommand>(_client);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["msgsuccess"] = "Client " + _client.Name + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(_client);
        }

        [HttpPost]
        public ActionResult DeleteClient(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var command = new DeleteClientCommand { Id = Convert.ToInt32(id) };
                if (_commandBus != null) _commandBus.Submit(command);
                TempData["msgsuccess"] = "Record delete successfully.";
                return Json("success");
            }
            return Json("Please contact adtones adminstrator");
        }
        public ActionResult updateClientStatus(int clientId, int status)
        {
            var _clientdetails = _clientRepository.Get(x => x.Id == clientId);
            if (_clientdetails != null)
            {
                ClientModel client = Mapper.Map<Client, ClientModel>(_clientdetails);
                client.Status = status;
                CreateOrUpdateClientCommand command = Mapper.Map<ClientModel, CreateOrUpdateClientCommand>(client);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    var statusvalue = (ClientStatus)command.Status;
                    var clientName = command.Name.ToString();
                    return Json(new { success = "success", value = statusvalue.ToString(), value1 = clientName });
                }
            }
            return Json(new { success = "fail", value = "Internal Server Error." });
        }

        public DataTable ExecuteLinkedSP(string spname, string conn, int ClientID)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientId", ClientID);
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }

        #region Currency Code For Initalize

        //Get Currency
        private void FillCurrencyList()
        {
            var currency = (from action in _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1) select new SelectListItem { Text = action.CurrencyCode, Value = action.CurrencyId.ToString() }).ToList();
            ViewBag.currencyList = currency;
        }

        //Get Currency Code
        public ActionResult GetCurrencyCode(int id, string label)
        {
            CurrencySymbol currencySymbol = new CurrencySymbol();
            var currencyCode = "";
            if (label == "country")
            {
                int CountryId = 0;
                if (id == 12 || id == 13 || id == 14) CountryId = 12;
                else if (id == 11) CountryId = 8;
                else CountryId = Convert.ToInt32(id);
                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;
                currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;
                return Json(new { data = "success", value = currencyCode, value1 = currencyId });
            }
            else if (label == "currency")
            {
                currencyCode = _currencyRepository.Get(c => c.CurrencyId == id).CurrencyCode;
                return Json(new { data = "success", value = currencyCode });
            }
            else
            {
                return Json(new { data = "fail" });
            }
        }

        #endregion
    }
}
