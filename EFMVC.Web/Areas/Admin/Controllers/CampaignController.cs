using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.SearchClass;
using EFMVC.Model;
using EFMVC.Web.ViewModels;
using AutoMapper;
using EFMVC.Web.Common;
using EFMVC.Domain.Commands;
using EFMVC.CommandProcessor.Command;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using System.Globalization;
using EFMVC.Web.Models;

namespace EFMVC.Web.Areas.Admin.Controllers
{

    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Campaign")]
    public class CampaignController : Controller
    {
        //
        // GET: /Admin/Campaign/

        //
        // GET: /Admin/Advert/

        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IQuestionRepository _questionRepository;

        private readonly IBillingRepository _billingRepository;

        private readonly ICountryRepository _countryRepository;

        private readonly IOperatorRepository _operatorRepository;
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _campaign advert repository
        /// </summary>
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;
        public CampaignController(ICommandBus commandBus, ICampaignAuditRepository campaignAuditRepository, IAdvertRepository advertRepository, IUserRepository userRepository, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, ICampaignAdvertRepository campaignAdvertRepository, IQuestionRepository questionRepository, IBillingRepository billingRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository, IContactsRepository contactsRepository)
        {
            _campaignAuditRepository = campaignAuditRepository;
            _commandBus = commandBus;
            _advertRepository = advertRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _profileRepository = profileRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
            _questionRepository = questionRepository;
            _billingRepository = billingRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _contactsRepository = contactsRepository;
        }

        [Route("Index")]
        [Route("{userId}/{campaignId}")]
        public ActionResult Index(int? userId, int? campaignId)
        {
            //Session["UserID"] = userId;
            //Session["CampaignID"] = campaignId;
            TempData["UserID"] = userId;
            TempData["CampaignID"] = campaignId;
            //FillUserDropdown(userId);
            FillClientDropdown();
            FillAdvertDropdown();
            FillCampaignStatus(userId);
            FillCountry();
            ViewBag.SearchResult = false;
            List<CampaignAdminResult> _result = new List<CampaignAdminResult>();
            var campaigndropdown = _profileRepository.GetAll().Select(top => new
            {
                CampaignName = top.CampaignName,
                CampaignProfileId = top.CampaignProfileId
            }).ToList();
            ViewBag.campaigns = new MultiSelectList(campaigndropdown, "CampaignProfileId", "CampaignName");
            CampaignAdminFilter _filterCritearea = new CampaignAdminFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {

                List<CampaignAdminResult> _result = new List<CampaignAdminResult>();

                IEnumerable<CampaignProfileFormModel> campaignProfileFormModels = null;
                IEnumerable<CampaignProfile> campaignProfiles = null;

                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }

                //if (param.Columns.First().Search.Value == "0")
                //{
                //    campaignProfiles = _profileRepository.GetAll().OrderByDescending(top => top.CreatedDateTime).ToList();
                //    cnt = campaignProfiles.Count();
                //    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                //}

                int?[] CountryId = new int?[cnt];
                int?[] OperatorId = new int?[cnt];
                if (TempData["CountryId"] != null)
                {
                    var test = TempData["CountryId"];
                    int?[] id = (int?[])TempData["CountryId"];
                    CountryId = id.Select(a => (int?)Convert.ToInt32(a)).ToArray();
                }
                else
                {
                    CountryId = null;
                }
                if (TempData["OperatorId"] != null)
                {
                    int?[] id = (int?[])TempData["OperatorId"];
                    OperatorId = id.Select(a => (int?)Convert.ToInt32(a)).ToArray();
                }
                else
                {
                    OperatorId = null;
                }

                if (searchValue == true)
                {
                    #region Search Functionality

                    int[] UserId = new int[cnt];
                    int?[] ClientId = new int?[cnt];
                    int[] CampaignId = new int[cnt];
                    int[] AdvertId = new int[cnt];
                    int[] CampaignStatusId = new int[cnt];
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

                            ClientId = columnSearch[1].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                            //ClientId = columnSearch[1].Split(',').Select(int.Parse).ToArray();
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

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            AdvertId = columnSearch[3].Split(',').Select(int.Parse).ToArray();
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
                            CampaignStatusId = columnSearch[4].Split(',').Select(int.Parse).ToArray();
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
                            var data = columnSearch[5].Split(',').ToArray();
                            string strTodate = data[1];
                            DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string strFromdate = data[0];
                            DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            fromdate = Fromdate;
                            todate = Todate;
                        }
                        else
                        {
                            columnSearch[5] = null;

                        }
                    }

                    // campaignProfiles = _profileRepository.GetMany(top => (UserId.Contains(top.UserId)) || (ClientId.Contains(top.ClientId)) ||
                    //(AdvertId.Contains(top.CampaignAdverts.FirstOrDefault().AdvertId)) || (CampaignId.Contains(top.CampaignProfileId)) || (CampaignStatusId.Contains(top.Status))
                    //|| ((top.CreatedDateTime >= fromdate && top.CreatedDateTime <= todate))
                    //);

                    //add for and search 23-01-2019
                    campaignProfiles = _profileRepository.GetAll().OrderByDescending(top => top.CreatedDateTime).ToList();
                    foreach (var item in campaignProfiles)
                    {
                        int status = item.Status;
                        if (item.Status == 2)
                        {
                            var billingDetails = _billingRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId).ToList();
                            if (billingDetails.Count() == 0)
                            {
                                item.Status = 8;
                            }
                            else
                            {
                                item.Status = item.Status;
                            }
                        }

                        // 06-11-20199
                        //if(item.Status == 7)
                        //{
                        //    item.Status = 5;
                        //}
                    }

                    if (columnSearch[0] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => (UserId.Contains(top.UserId))).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => (ClientId.Contains(top.ClientId))).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => (CampaignId.Contains(top.CampaignProfileId))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => top.CampaignAdverts != null && top.CampaignAdverts.Count() > 0 && (AdvertId.Contains(top.CampaignAdverts.FirstOrDefault().AdvertId))).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => (CampaignStatusId.Contains(top.Status))).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => (top.CreatedDateTime.Date >= fromdate && top.CreatedDateTime.Date <= todate)).ToList();
                    }

                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                    #endregion

                }
                else
                {
                    //campaignProfiles = _profileRepository.GetAll().Skip(param.Start).Take(param.Length);
                    //if (campaignProfiles.Count() >= 10)
                    //    cnt = _profileRepository.GetAll().Count(); 

                    campaignProfiles = _profileRepository.GetAll().OrderByDescending(top => top.CreatedDateTime).ToList();
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                }

                if (TempData["UserID"] != null)
                {
                    int[] campaignStatus = new int[4] { 1, 2, 3, 4 };
                    int uId = (int)TempData["UserID"];
                    campaignProfiles = _profileRepository.GetMany(top => top.UserId == uId && campaignStatus.Contains(top.Status)).OrderByDescending(top => top.CreatedDateTime).ToList();
                    //if (campaignProfiles.Count() > 10)
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                }
                if (TempData["CampaignID"] != null)
                {
                    int campId = (int)TempData["CampaignID"];
                    campaignProfiles = _profileRepository.GetMany(top => top.CampaignProfileId == campId).OrderByDescending(top => top.CreatedDateTime).ToList();
                    //if (campaignProfiles.Count() > 10)
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                }
                if (CountryId != null)
                {
                    campaignProfiles = _profileRepository.GetMany(top => (CountryId.Contains(top.CountryId.Value))).OrderByDescending(top => top.CreatedDateTime).ToList();
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                }
                if (OperatorId != null)
                {
                    campaignProfiles = _profileRepository.GetMany(top => (OperatorId.Contains(top.User.OperatorId))).OrderByDescending(top => top.CreatedDateTime).ToList();
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length);
                }
                foreach (var item in campaignProfiles)
                {
                    //calculate average bid that has status Played.

                    IEnumerable<CampaignAuditFormModel> CampaignAuditData = null;
                    var CountryID = _profileRepository.Get(x => x.CampaignProfileId == item.CampaignProfileId).CountryId;
                    var ConnString = ConnectionString.GetConnectionString(CountryID);


                    double totalbidval = 0;
                    double totalspend = 0;
                    double SMSCost = 0;
                    double EmailCost = 0;

                    //if (!string.IsNullOrEmpty(ConnString))
                    //{
                    //    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    //    CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == play && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                    //                        .Select(s => new CampaignAuditFormModel { BidValue = s.BidValue, SMSCost = s.SMSCost, EmailCost = s.EmailCost });
                    //}
                    //else
                    //{
                    EFMVCDataContex db = new EFMVCDataContex();
                    CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                                       .Select(s => new CampaignAuditFormModel { BidValue = s.BidValue, SMSCost = s.SMSCost, EmailCost = s.EmailCost });
                    //}

                    var finaltotalplays = CampaignAuditData.Count();

                    if (finaltotalplays > 0)
                    {
                        totalbidval = totalbidval + Convert.ToDouble(CampaignAuditData.Average(d1 => d1.BidValue));
                        totalspend = totalspend + CampaignAuditData.Sum(top => top.BidValue);
                        SMSCost = SMSCost + CampaignAuditData.Sum(d1 => d1.SMSCost);
                        EmailCost = EmailCost + CampaignAuditData.Sum(d1 => d1.EmailCost);
                        totalspend = totalspend + SMSCost + EmailCost;
                    }
                    var advertdetails = item.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId).FirstOrDefault();
                    string advertname = string.Empty;
                    var advertId = 0;
                    if (advertdetails != null)
                    {

                        advertname = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertName;
                        advertId = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertId;
                    }
                    else
                    {
                        advertname = "-";
                        advertId = 0;
                    }

                    int status = item.Status;
                    if (item.Status == 2)
                    {
                        var billingDetails = _billingRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId).ToList();
                        if (billingDetails.Count() == 0)
                        {
                            status = 8;
                        }
                        else
                        {
                            status = item.Status;
                        }
                    }

                    _result.Add(new CampaignAdminResult
                    {
                        CampaignProfileId = item.CampaignProfileId,
                        Status = status,
                        UserId = item.UserId,
                        Email = item.User.Email,
                        UserName = item.User.FirstName + " " + item.User.LastName,
                        ClientId = item.ClientId,
                        ClientName = item.ClientId == 0 ? "-" : item.ClientId == null ? "-" : item.Client.Name,
                        CampaignName = item.CampaignName,
                        AdvertId = advertId,
                        AdvertName = advertname,
                        finaltotalplays = finaltotalplays,
                        TotalBudget = item.TotalBudget,
                        totalspend = Convert.ToDecimal(totalspend),
                        FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend),
                        totalaveragebid = Convert.ToDecimal(totalbidval),
                        CreatedDateTime = item.CreatedDateTime,
                        DisplayCreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy"),
                        TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId),
                        IsAdminApproval = item.IsAdminApproval
                    });

                }
                //Session["UserID"] = null;
                //Session["CampaignID"] = null;

                TempData.Keep("UserID");
                TempData.Keep("CampaignID");
                TempData.Keep("CountryId");
                TempData.Keep("OperatorId");

                _result = ApplySorting(param, _result);

                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<CampaignAdminResult> result = new DTResult<CampaignAdminResult>
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
        private static List<CampaignAdminResult> ApplySorting(DTParameters param, List<CampaignAdminResult> result)
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
                        result = result.OrderBy(top => top.UserName).ToList();
                    else
                        result = result.OrderByDescending(top => top.UserName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.AdvertName).ToList();
                    else
                        result = result.OrderByDescending(top => top.AdvertName).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.finaltotalplays).ToList();
                    else
                        result = result.OrderByDescending(top => top.finaltotalplays).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalBudget).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalBudget).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.totalspend).ToList();
                    else
                        result = result.OrderByDescending(top => top.totalspend).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.FundsAvailable).ToList();
                    else
                        result = result.OrderByDescending(top => top.FundsAvailable).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.totalaveragebid).ToList();
                    else
                        result = result.OrderByDescending(top => top.totalaveragebid).ToList();
                }
                else if (paramOrderDetails.Column == 10)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateTime).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateTime).ToList();
                }
                else if (paramOrderDetails.Column == 12)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TicketCount).ToList();
                    else
                        result = result.OrderByDescending(top => top.TicketCount).ToList();
                }
            }
            return result;
        }

        private List<CampaignAdminResult> GetCampaignResult(int? userId, int? campaignId)
        {
            List<CampaignAdminResult> _result = new List<CampaignAdminResult>();
            IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetAll();
            IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);
            campaignProfileFormModels = campaignProfileFormModels.OrderByDescending(top => top.CreatedDateTime);
            FillCampaignDropdown(campaignProfileFormModels);
            if (userId != null)
            {
                campaignProfiles = campaignProfiles.Where(top => top.UserId == userId).OrderByDescending(top => top.CreatedDateTime).ToList();
            }
            if (campaignId != null)
            {
                campaignProfiles = campaignProfiles.Where(top => top.CampaignProfileId == campaignId).OrderByDescending(top => top.CreatedDateTime).ToList();
            }

            foreach (var item in campaignProfiles)
            {
                //calculate average bid that has status Played.

                IEnumerable<CampaignAuditFormModel> CampaignAuditData = null;
                var CountryID = _profileRepository.Get(x => x.CampaignProfileId == item.CampaignProfileId).CountryId;
                var ConnString = ConnectionString.GetConnectionString(CountryID);


                double totalbidval = 0;
                double totalspend = 0;
                double SMSCost = 0;
                double EmailCost = 0;

                //if (!string.IsNullOrEmpty(ConnString))
                //{
                //    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                //    CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == play && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                //                        .Select(s => new CampaignAuditFormModel { BidValue = s.BidValue, SMSCost = s.SMSCost, EmailCost = s.EmailCost });
                //}
                //else
                //{
                EFMVCDataContex db = new EFMVCDataContex();
                CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                                   .Select(s => new CampaignAuditFormModel { BidValue = s.BidValue, SMSCost = s.SMSCost, EmailCost = s.EmailCost });
                //}

                var finaltotalplays = CampaignAuditData.Count();

                if (finaltotalplays > 0)
                {
                    totalbidval = totalbidval + Convert.ToDouble(CampaignAuditData.Average(d1 => d1.BidValue));
                    totalspend = totalspend + CampaignAuditData.Sum(top => top.BidValue);
                    SMSCost = SMSCost + CampaignAuditData.Sum(d1 => d1.SMSCost);
                    EmailCost = EmailCost + CampaignAuditData.Sum(d1 => d1.EmailCost);
                    totalspend = totalspend + SMSCost + EmailCost;
                }
                var advertdetails = item.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId).FirstOrDefault();
                string advertname = string.Empty;
                var advertId = 0;
                if (advertdetails != null)
                {

                    advertname = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertName;
                    advertId = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertId;
                }
                else
                {
                    advertname = "-";
                    advertId = 0;
                }

                int status = item.Status;
                if (item.Status == 2)
                {
                    var billingDetails = _billingRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId).ToList();
                    if (billingDetails.Count() == 0)
                    {
                        status = 8;
                    }
                    else
                    {
                        status = item.Status;
                    }
                }
                if (item.Status == 7)
                {
                    item.Status = 5;
                }

                _result.Add(new CampaignAdminResult
                {
                    CampaignProfileId = item.CampaignProfileId,
                    Status = status,
                    UserId = item.UserId,
                    Email = item.User.Email,
                    UserName = item.User.FirstName + " " + item.User.LastName,
                    ClientId = item.ClientId,
                    ClientName = item.ClientId == 0 ? "-" : item.ClientId == null ? "-" : item.Client.Name,
                    CampaignName = item.CampaignName,
                    AdvertId = advertId,
                    AdvertName = advertname,
                    finaltotalplays = finaltotalplays,
                    TotalBudget = item.TotalBudget,
                    totalspend = Convert.ToDecimal(totalspend),
                    FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend),
                    totalaveragebid = Convert.ToDecimal(totalbidval),
                    CreatedDateTime = item.CreatedDateTime,
                    DisplayCreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy"),
                    TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId),
                    IsAdminApproval = item.IsAdminApproval
                });

            }

            //foreach (var item in campaignProfiles)
            //{
            //    //calculate average bid that has status Played.
            //    var ticketCount = 0;
            //    double totalbidval = 0;
            //    double totalspend = 0;
            //    double SMSCost = 0;
            //    double EmailCost = 0;
            //    var finaltotalplays = item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count();

            //    //caculate the AverageBid field
            //    if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
            //    {
            //        totalbidval = totalbidval + Convert.ToDouble(item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Average(d1 => d1.BidValue));
            //    }
            //    //cauculate total  spend that has status Played.
            //    if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
            //    {
            //        totalspend = totalspend + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(top => top.BidValue);
            //        SMSCost = SMSCost + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(d1 => d1.SMSCost);
            //        EmailCost = EmailCost + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(d1 => d1.EmailCost);
            //        totalspend = totalspend + SMSCost + EmailCost;
            //    }
            //    var advertdetails = item.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId).FirstOrDefault();
            //    string advertname = string.Empty;
            //    var advertId = 0;
            //    if (advertdetails != null)
            //    {

            //        advertname = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertName;
            //        advertId = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault().AdvertId;
            //    }
            //    else
            //    {
            //        advertname = "-";
            //        advertId = 0;
            //    }
            //    int status = item.Status;
            //    if (item.Status == 2)
            //    {
            //        var billingDetails = _billingRepository.GetMany(top => top.CampaignProfileId == item.CampaignProfileId).ToList();
            //        if (billingDetails.Count() == 0)
            //        {
            //            item.Status = 8;
            //        }
            //        else
            //        {
            //            item.Status = item.Status;
            //        }
            //    }
            //    //_result.Add(new CampaignAdminResult { CampaignProfileId = item.CampaignProfileId, CampaignName = item.CampaignName, AdvertId = advertId, AdvertName = advertname, ClientId = item.ClientId, ClientName = item.Client.Name, CreatedDateTime = item.CreatedDateTime, Email = item.User.Email, UserId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, finaltotalplays = finaltotalplays, FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend), Status = item.Status, totalaveragebid = Convert.ToDecimal(totalbidval), TotalBudget = item.TotalBudget, totalspend = Convert.ToDecimal(totalspend), TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId) });
            //    _result.Add(new CampaignAdminResult { CampaignProfileId = item.CampaignProfileId, CampaignName = item.CampaignName, AdvertId = advertId, AdvertName = advertname, ClientId = item.ClientId.Value, ClientName = item.ClientId == 0 ? "-" : item.Client.Name, CreatedDateTime = item.CreatedDateTime, Email = item.User.Email, UserId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, finaltotalplays = finaltotalplays, FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend), Status = item.Status, totalaveragebid = Convert.ToDecimal(totalbidval), TotalBudget = item.TotalBudget, totalspend = Convert.ToDecimal(totalspend), TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId), IsAdminApproval = item.IsAdminApproval });

            //}

            return _result;
        }
        [Route("UpdateStatus")]
        [Route("{id}/{status}")]
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {

            if (User.Identity.IsAuthenticated)
            {
                var campaigndetails = _profileRepository.GetById(id);

                CampaignProfileFormModel CampaignProfileFormModel = new CampaignProfileFormModel();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                if (status == 2)
                {
                    var campaignBilling = _billingRepository.Get(top => top.CampaignProfileId == campaigndetails.CampaignProfileId);
                    if (campaignBilling != null)
                    {
                        command.Status = status;
                    }
                    else
                    {
                        command.Status = (int)CampaignStatus.CampaignPausedDueToInsufficientFunds;
                    }
                }
                else
                {
                    command.Status = status;
                }

                if (campaigndetails != null)
                {
                    command.StartDate = campaigndetails.StartDate;
                    command.EndDate = campaigndetails.EndDate;
                    command.CampaignProfileId = id;
                    command.UserId = campaigndetails.UserId;
                    command.MaxBid = campaigndetails.MaxBid;
                    command.MaxMonthBudget = campaigndetails.MaxWeeklyBudget;
                    command.MaxWeeklyBudget = campaigndetails.MaxWeeklyBudget;
                    command.MaxHourlyBudget = campaigndetails.MaxHourlyBudget;
                    command.MaxDailyBudget = campaigndetails.MaxDailyBudget;
                    command.CampaignDescription = campaigndetails.CampaignDescription;
                    command.CampaignName = campaigndetails.CampaignName;
                    command.ClientId = campaigndetails.ClientId;
                    command.Active = campaigndetails.Active;
                    command.AvailableCredit = campaigndetails.AvailableCredit;
                    command.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                    command.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                    command.CancelledToDate = campaigndetails.CancelledToDate;
                    command.CreatedDateTime = campaigndetails.CreatedDateTime;
                    command.EmailToDate = campaigndetails.EmailToDate;
                    command.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                    command.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                    command.TotalCredit = campaigndetails.TotalCredit;
                    command.SpendToDate = campaigndetails.SpendToDate;
                    command.MaxDailyBudget = campaigndetails.MaxDailyBudget;
                    command.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                    command.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                    command.PlaysToDate = campaigndetails.PlaysToDate;
                    command.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                    command.SmsLastMonth = campaigndetails.SmsLastMonth;
                    command.SmsToDate = campaigndetails.SmsToDate;
                    command.TotalBudget = campaigndetails.TotalBudget;
                    command.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                    command.UserId = campaigndetails.UserId;
                    command.EmailBody = campaigndetails.EmailBody;
                    command.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                    command.EmailSubject = campaigndetails.EmailSubject;
                    command.SmsBody = campaigndetails.SmsBody;
                    command.SmsOriginator = campaigndetails.SmsOriginator;
                    command.CampaignProfileId = id;
                    command.CountryId = (int)campaigndetails.CountryId;
                    command.NextStatus = campaigndetails.NextStatus;
                    command.IsAdminApproval = true;
                    command.CurrencyCode = campaigndetails.CurrencyCode;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        // UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.AddCampaignData(campaigndetails, SQLServerEntities);
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, conn);

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);

                        //if (command.Status == 8)
                        //{
                        //    EFMVCDataContex eFMVCData = new EFMVCDataContex();
                        //    var campaignAdvertDetails = _campaignAdvertRepository.Get(top => top.CampaignProfileId == campaigndetails.CampaignProfileId);
                        //    var advertDetails = eFMVCData.Adverts.Where(top => top.AdvertId == campaignAdvertDetails.AdvertId).FirstOrDefault();
                        //    advertDetails.UpdatedDateTime = DateTime.Now;
                        //    advertDetails.Status = (int)AdvertStatus.CampaignPausedDueToInsufficientFunds;
                        //    eFMVCData.SaveChanges();
                        //    //var advertDetails = _advertRepository.Get(top => top.AdvertId == campaignAdvertDetails.AdvertId);
                        //    if (ConnString != null && ConnString.Count() > 0)
                        //    {
                        //        foreach (var itemConnString in ConnString)
                        //        {
                        //            EFMVCDataContex db1 = new EFMVCDataContex(itemConnString);
                        //            var advertDetails1 = db1.Adverts.Where(s => s.AdtoneServerAdvertId == advertDetails.AdvertId).FirstOrDefault();
                        //            if (advertDetails1 != null)
                        //            {
                        //                advertDetails1.UpdatedDateTime = DateTime.Now;
                        //                advertDetails1.Status = (int)AdvertStatus.CampaignPausedDueToInsufficientFunds;
                        //                advertDetails1.IsAdminApproval = true;
                        //                advertDetails1.NextStatus = false;
                        //                db1.SaveChanges();
                        //            }
                        //        }
                        //    }
                        //}

                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);

                                var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailFromOP != null)
                                {
                                    obj.UpdateCampaignStatus(campaigndetailFromOP.CampaignProfileId, status, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                }

                                //obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                //PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);

                            }
                        }

                        //using (SQLServerEntities)
                        //{
                        //    var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == id).FirstOrDefault();
                        //    if(campaignmatch != null)
                        //    {
                        //        campaignmatch.Status = status;
                        //        SQLServerEntities.SaveChanges();
                        //    }
                        //}
                        //return Json("success");
                        var statusvalue = (CampaignStatus)command.Status;
                        var campaignName = command.CampaignName.ToString();
                        return Json(new { success = "success", value = statusvalue.ToString(), value1 = campaignName });
                    }
                }

                //return Json("fail");
                return Json(new { success = "fail", value = "Internal Server Error." });
            }
            else
            {
                return Json("notauthorise");
            }
        }

        //Add 26-02-2019
        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName, int?[] countryId, int?[] operatorId)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName) && countryId != null && operatorId != null && operatorId[0] != null)
                {
                    List<int> advertUserId = _advertRepository.GetMany(top => operatorId.Contains(top.OperatorId)).Select(top => top.UserId).Distinct().ToList();
                    List<int> userId = _contactsRepository.GetMany(top => countryId.Contains(top.CountryId)).Select(top => top.UserId).ToList();
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3 && userId.Contains(top.UserId) && advertUserId.Contains(top.UserId)).Select(top => new
                    //var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3 && countryId.Contains(top.Operator.CountryId) && operatorId.Contains(top.OperatorId)).Select(top => new
                    // var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else if (!string.IsNullOrEmpty(UserName) && countryId != null && operatorId == null)
                {
                    List<int> userId = _contactsRepository.GetMany(top => countryId.Contains(top.CountryId)).Select(top => top.UserId).ToList();
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3 && userId.Contains(top.UserId)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else if (!string.IsNullOrEmpty(UserName) && countryId != null && operatorId[0] == null)
                {
                    List<int> userId = _contactsRepository.GetMany(top => countryId.Contains(top.CountryId)).Select(top => top.UserId).ToList();
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3 && userId.Contains(top.UserId)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else if (!string.IsNullOrEmpty(UserName) && countryId == null && operatorId == null)
                {
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else if (!string.IsNullOrEmpty(UserName) && countryId == null && operatorId[0] == null)
                {
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
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

        private void FillCampaignDropdown(IEnumerable<CampaignProfileFormModel> campaignProfileFormModels)
        {
            var campaigndropdown = campaignProfileFormModels.Select(top => new
            {
                CampaignName = top.CampaignName,
                CampaignProfileId = top.CampaignProfileId
            }).ToList();
            ViewBag.campaigns = new MultiSelectList(campaigndropdown, "CampaignProfileId", "CampaignName");
        }

        //Add 07-08-2019
        [Route("FillUserDropdown")]
        [HttpPost]
        public ActionResult FillUserDropdown(int? userId)
        {
            if (userId != null)
            {
                //var userdetails = _userRepository.GetAll().Where(top => top.UserId == userId).Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName,
                //    UserId = top.UserId,
                //}).Take(1000).ToList();

                //Comment 07-08-2019
                //var userdetails = _userRepository.GetAll().Where(top => top.UserId == userId).Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName,
                //    UserId = top.UserId,
                //}).Take(1000).ToList();
                //ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");

                var userdetails = _userRepository.GetMany(top => top.UserId == userId.Value).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                return Json(data);
            }
            else
            {
                //var userdetails = _userRepository.GetAll().Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName,
                //    UserId = top.UserId,
                //}).Take(1000).ToList();

                //Comment 07-08-2019
                //var userdetails = _userRepository.GetAll().Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName,
                //    UserId = top.UserId,
                //}).Take(1000).ToList();
                //ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");

                return Json("");
            }

        }
        public void FillClientDropdown()
        {
            var clientdetails = _clientRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                // Id = top.Id
                ClientId = top.Id
            }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "ClientId", "Name");
        }
        public void FillAdvertDropdown()
        {

            var advertdetails = _advertRepository.GetAll().Select(top => new
            {
                AdvertName = top.AdvertName,
                AdvertId = top.AdvertId
            }).ToList();
            ViewBag.adverts = new MultiSelectList(advertdetails, "AdvertId", "AdvertName");
        }
        public void FillCampaignStatus(int? userId)
        {
            IEnumerable<Common.CampaignStatus> campaignTypes = Enum.GetValues(typeof(Common.CampaignStatus))
                                                     .Cast<Common.CampaignStatus>();
            var campaignstatus = (from action in campaignTypes
                                  select new
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            if (userId != null)
            {
                ViewBag.CampaignStatusId = new MultiSelectList(campaignstatus, "Value", "Text", new int[] { 4 });
            }
            else
            {
                ViewBag.CampaignStatusId = new MultiSelectList(campaignstatus, "Value", "Text");
            }
        }
        [Route("GetClientsUser")]
        [HttpPost]
        public ActionResult GetClientsUser(int[] userId)
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
                    return Json(clientdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        [Route("GetClientsAdvert")]
        [HttpPost]
        public ActionResult GetClientsAdvert(int?[] clientId)
        {
            try
            {
                if (clientId != null)
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => clientId.Contains((int?)(top.ClientId))).Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
                else
                {
                    var advertdetails = _advertRepository.GetAll().Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        [Route("GetCampaignAdvert")]
        [HttpPost]
        public ActionResult GetCampaignAdvert(int[] campaignId)
        {
            try
            {
                if (campaignId != null && campaignId.FirstOrDefault() != 0)
                {
                    //var advertId = _campaignAdvertRepository.Get(top => campaignId.Contains(top.CampaignProfileId));
                    List<int> advertId = _campaignAdvertRepository.GetMany(top => campaignId.Contains(top.CampaignProfileId)).Select(top => top.AdvertId).ToList();
                    if (advertId.Count() > 0)
                    {
                        //var advertdetails = _advertRepository.GetAll().Where(top => top.AdvertId == advertId.AdvertId).Select(top => new
                        var advertdetails = _advertRepository.GetAll().Where(top => advertId.Contains(top.AdvertId)).Select(top => new
                        {
                            Name = top.AdvertName,
                            Id = top.AdvertId
                        }).ToList();
                        return Json(advertdetails);
                    }
                    return Json("nodata");
                }
                else
                {
                    var advertdetails = _advertRepository.GetAll().Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        [Route("GetClientsCampaign")]
        [HttpPost]
        public ActionResult GetClientsCampaign(int?[] clientId, int[] userId)
        {
            try
            {

                if (clientId != null)
                {
                    if (clientId != null)
                    {

                        var campaigndetails = _profileRepository.GetAll().Where(top => clientId.Contains((int?)(top.ClientId))).Select(top => new
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
                        return Json(campaigndetails);
                    }
                }
                else
                {
                    if (userId != null)
                    {

                        var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
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
                        return Json(campaigndetails);
                    }
                }
            }
            catch (Exception ex)
            {

                return Json("error");
            }
        }

        [Route("GetUsersCampaign")]
        [HttpPost]
        public ActionResult GetUsersCampaign(int[] userId)
        {
            try
            {


                if (userId != null)
                {

                    var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
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
                    return Json(campaigndetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("SearchCampaigns")]
        public ActionResult SearchCampaigns([Bind(Prefix = "Item2")]SearchClass.CampaignAdminFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] CampaignId, int[] CampaignStatusId)
        {
            ViewBag.SearchResult = true;
            if (User.Identity.IsAuthenticated)
            {
                List<CampaignAdminResult> _result = new List<CampaignAdminResult>();
                var finalresult = new List<CampaignAdminResult>();
                if (_filterCritearea != null)
                {
                    _result = GetCampaignResult(null, null);
                    finalresult = getcampaignResult(_result, _filterCritearea, UserId, ClientId, AdvertId, CampaignId, CampaignStatusId);
                }
                else
                {
                    CampaignStatusId = new int[] { 1 };
                    _result = GetCampaignResult(null, null);
                    finalresult = getcampaignResult(_result, _filterCritearea, UserId, ClientId, AdvertId, CampaignId, CampaignStatusId);
                }

                return PartialView("_UserCampaignDetails", finalresult);
            }
            else
            {
                return PartialView("_UserCampaignDetails", "notauthorise");
            }
        }

        public List<CampaignAdminResult> getcampaignResult(List<CampaignAdminResult> campaignresult, SearchClass.CampaignAdminFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] CampaignId, int[] CampaignStatusId)
        {
            if (campaignresult != null && _filterCritearea != null)
            {

                if (UserId != null)
                {
                    campaignresult = campaignresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }
                if (ClientId != null)
                {
                    campaignresult = campaignresult.Where(top => ClientId.Contains(top.ClientId)).ToList();
                }
                if (AdvertId != null)
                {
                    campaignresult = campaignresult.Where(top => AdvertId.Contains(top.AdvertId)).ToList();
                }
                if (CampaignId != null)
                {
                    campaignresult = campaignresult.Where(top => CampaignId.Contains(top.CampaignProfileId)).ToList();
                }
                if (CampaignStatusId != null)
                {
                    campaignresult = campaignresult.Where(top => CampaignStatusId.Contains(top.Status)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    // campaignresult = campaignresult.Where(top => top.CreatedDateTime.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDateTime.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    campaignresult = campaignresult.Where(top => top.CreatedDateTime.Date >= Fromdate && top.CreatedDateTime.Date <= Todate).ToList();
                }
            }
            else
            {
                if (UserId != null)
                {
                    campaignresult = campaignresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }
                if (CampaignStatusId != null)
                {
                    //campaignresult = campaignresult.Where(top => CampaignStatusId.Contains(top.Status)).ToList();
                    campaignresult = campaignresult.ToList();
                }
            }
            return campaignresult;
        }

        [Route("ResetSearchCampaigns")]
        public ActionResult ResetSearchCampaigns()
        {
            ViewBag.SearchResult = true;
            if (User.Identity.IsAuthenticated)
            {
                List<CampaignAdminResult> _result = new List<CampaignAdminResult>();
                IEnumerable<CampaignProfile> campaignProfiles = null;

                var finalresult = new List<CampaignAdminResult>();
                int userId = 0;

                campaignProfiles = _profileRepository.GetAll().OrderByDescending(top => top.CreatedDateTime).ToList();

                IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);
                campaignProfileFormModels = campaignProfileFormModels.OrderByDescending(top => top.CreatedDateTime);

                FillCampaignDropdown(campaignProfileFormModels);
                foreach (var item in campaignProfileFormModels)
                {
                    //calculate average bid that has status Played.
                    var ticketCount = 0;
                    double totalbidval = 0;
                    double totalspend = 0;
                    double SMSCost = 0;
                    double EmailCost = 0;
                    var relatedAudit = item.GetDomainCampaignAudits(_campaignAuditRepository);
                    var finaltotalplays = relatedAudit.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase && top.PlayLengthTicks > 6000);
                    var finaltotalplaysCount = finaltotalplays.Count();

                    //caculate the AverageBid field
                    if (finaltotalplaysCount > 0)
                    {
                        totalbidval = totalbidval + Convert.ToDouble(finaltotalplays.Average(d1 => d1.BidValue));
                    }
                    //cauculate total  spend that has status Played.
                    if (finaltotalplaysCount > 0)
                    {
                        totalspend = totalspend + finaltotalplays.Sum(top => top.BidValue);
                        SMSCost = SMSCost + finaltotalplays.Sum(d1 => d1.SMSCost);
                        EmailCost = EmailCost + finaltotalplays.Sum(d1 => d1.EmailCost);
                        totalspend = totalspend + SMSCost + EmailCost;
                    }
                    var advertdetails = item.CampaignAdverts.FirstOrDefault(top => top.CampaignProfileId == item.CampaignProfileId);
                    string advertname = string.Empty;
                    var advertId = 0;
                    if (advertdetails != null)
                    {
                        var ad = _advertRepository.AsQueryable().FirstOrDefault(top => top.AdvertId == advertdetails.AdvertId);
                        advertname = ad.AdvertName;
                        advertId = ad.AdvertId;
                    }
                    else
                    {
                        advertname = "-";
                        advertId = 0;
                    }
                    //finalresult.Add(new CampaignAdminResult { CampaignProfileId = item.CampaignProfileId, CampaignName = item.CampaignName, AdvertId = advertId, AdvertName = advertname, ClientId = item.ClientId, ClientName = item.Client.Name, CreatedDateTime = item.CreatedDateTime, Email = item.User.Email, UserId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, finaltotalplays = finaltotalplays, FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend), Status = item.Status, totalaveragebid = Convert.ToDecimal(totalbidval), TotalBudget = item.TotalBudget, totalspend = Convert.ToDecimal(totalspend), TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId) });
                    finalresult.Add(new CampaignAdminResult { CampaignProfileId = item.CampaignProfileId, CampaignName = item.CampaignName, AdvertId = advertId, AdvertName = advertname, ClientId = item.ClientId.Value, ClientName = item.ClientId == 0 ? "-" : item.Client.Name, CreatedDateTime = item.CreatedDateTime, Email = item.User.Email, UserId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, finaltotalplays = finaltotalplaysCount, FundsAvailable = Convert.ToDecimal(item.TotalBudget) - Convert.ToDecimal(totalspend), Status = item.Status, totalaveragebid = Convert.ToDecimal(totalbidval), TotalBudget = item.TotalBudget, totalspend = Convert.ToDecimal(totalspend), TicketCount = _questionRepository.Count(top => top.UserId == userId && top.CampaignProfileId == item.CampaignProfileId) });

                }
                return PartialView("_UserCampaignDetails", finalresult);
            }
            else
            {
                return PartialView("_UserCampaignDetails", "notauthorise");
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
            TempData["CountryId"] = countryId;
            return Json(ViewBag.operatordetails);
        }

        [Route("GetClientByCountryId")]
        [HttpPost]
        public ActionResult GetClientByCountryId(int?[] countryId)
        {
            try
            {
                if (countryId != null)
                {
                    var clientdetails = _clientRepository.GetMany(top => countryId.Contains((int?)(top.CountryId))).Select(top => new
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
                    return Json(clientdetails);
                }
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [Route("GetCampaignByCountryId")]
        [HttpPost]
        public ActionResult GetCampaignByCountryId(int?[] countryId)
        {
            try
            {
                if (countryId != null)
                {
                    var campaignprofiledetails = _profileRepository.GetMany(top => countryId.Contains((int?)(top.CountryId))).Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(campaignprofiledetails);
                }
                else
                {
                    var campaignprofiledetails = _profileRepository.GetAll().Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(campaignprofiledetails);
                }
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [Route("GetAdvertByCountryId")]
        [HttpPost]
        public ActionResult GetAdvertByCountryId(int?[] countryId, int?[] operatorId)
        {
            try
            {
                var advertdetails = _advertRepository.GetAll().ToList();
                if (countryId != null)
                {
                    if (countryId[0] != null)
                        advertdetails = advertdetails.Where(top => countryId.Contains((int?)(top.CountryId))).ToList();
                }
                if (operatorId != null)
                {
                    if (operatorId[0] != null)
                    {
                        advertdetails = advertdetails.Where(top => operatorId.Contains(top.OperatorId)).ToList();
                        TempData["OperatorId"] = operatorId;
                    }
                }
                var advertdata = advertdetails.Select(top => new
                {
                    Name = top.AdvertName,
                    Id = top.AdvertId
                }).ToList();
                return Json(advertdata);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}
