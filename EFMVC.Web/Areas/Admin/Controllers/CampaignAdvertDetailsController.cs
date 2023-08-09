using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Web.Areas.UsersAdmin.Models;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("CampaignAdvertDetails")]
    public class CampaignAdvertDetailsController : Controller
    {
        // GET: Admin/Country


        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICampaignAuditRepository _campaignAuditRepository;
        private readonly ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository;
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAdvertRepository _advertRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public CampaignAdvertDetailsController(ICommandBus commandBus, ICampaignAuditRepository campaignAuditRepository, IUserRepository userRepository, ICountryRepository countryRepository, ICampaignProfileRepository profileRepository, IClientRepository clientRepository, IAdvertRepository advertRepository, ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository, IProfileMatchInformationRepository profileMatchInformationRepository)
        {
            _campaignAuditRepository = campaignAuditRepository;
            _commandBus = commandBus;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _profileRepository = profileRepository;
            _clientRepository = clientRepository;
            _advertRepository = advertRepository;
            _campaignProfilePreferenceRepository = campaignProfilePreferenceRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
        }
       

        [Route("Index")]
        public ActionResult Index(int id)
        {
            FillCountryList();
            ViewBag.CampaignId = id;
            Session["CampId"] = id;
            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();
            CampaignProfileMapping _mapping = new CampaignProfileMapping();

            //var countryId = _advertRepository.GetById(id).CountryId;
            var countryId = _profileRepository.GetById(id).CountryId;
            var profileMatchId = _profileMatchInformationRepository.Get(top => top.CountryId == countryId && top.ProfileName == "Location" && top.IsActive == true).Id;
            int CountryId = Convert.ToInt32(countryId);
            //CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel();
            //CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel();
            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(CountryId);
            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId);
            //CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel();
            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel(CountryId);

            //CampaignProfileSkizaFormModel CampaignProfileSkizaDetail = new CampaignProfileSkizaFormModel();
            CampaignProfileSkizaFormModel CampaignProfileSkizaDetail = new CampaignProfileSkizaFormModel(CountryId);

            //CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel();
            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel(CountryId);
            CampaignProfileTimeSettingFormModel CampaignProfileTimeSetting = new CampaignProfileTimeSettingFormModel();

            EFMVC.Web.SearchClass.CampaignAuditFilter CampaignAuditFilter = new EFMVC.Web.SearchClass.CampaignAuditFilter();
            CampaignDashboardChartResult CampaignDashboardChartResult = new CampaignDashboardChartResult();
            CampaignAuditFilter.CampaignProfileId = id;
            //EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            //User user = _userRepository.GetById(efmvcUser.UserId);
            //var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
            var _clientdetails = _clientRepository.GetMany(x => (x.Status == 1 || x.Status == 2));
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            var campaignProfileResult = _profileRepository.GetMany(x => x.CampaignProfileId == id && (x.Status != 5));
            if (campaignProfileResult.Count() == 0)
                return RedirectToAction("Index", "Campaign");

            //ViewBag.Country = _countryRepository.Get(s => s.Name.ToLower() == "kenya").Name;//ConfigurationManager.AppSettings["Country"];
            ViewBag.Country = _countryRepository.GetById(CountryId).Name;
            if (id != 0)
            {

                CampaignProfileGeographicModel = CampaignProfileGeographic(id, CampaignProfileGeographicModel);
                //CampaignProfileGeographicModel.AreaQuestion = areaName;
                _mapping.CampaignProfileGeographicModel = CampaignProfileGeographicModel;

                CampaignProfileDemographicsmodel = CampaignProfileDemographic(id, CampaignProfileDemographicsmodel);
                _mapping.CampaignProfileDemographicsmodel = CampaignProfileDemographicsmodel;

                CampaignProfileAd = CampaignProfileAdvert(id, CampaignProfileAd);
                _mapping.CampaignProfileAd = CampaignProfileAd;

                CampaignProfileSkizaDetail = CampaignProfileSkizaInormation(id, CampaignProfileSkizaDetail);
                _mapping.CampaignProfileSkizaFormModel = CampaignProfileSkizaDetail;

                Mobilepro = Mobile(id, Mobilepro);
                _mapping.CampaignProfileMobileFormModel = Mobilepro;

                CampaignProfileTimeSetting = Timing(id, CampaignProfileTimeSetting);
                _mapping.CampaignProfileTimeSettingFormModel = CampaignProfileTimeSetting;

                var model = GetEditData(id);

                ViewBag.AdvertClientId = model.ClientId;
                _mapping.AdvertFormModel = advertFormModels();

                _mapping.CampaignAudit = new List<CampaignAuditResult>();
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                ViewBag.ClientId = model.ClientId;
                ViewBag.CampaignProfileId = model.CampaignProfileId;
                if (model != null)
                    CampaignDashboardChartResult = FillChartDataByCampaignId(model); // Remove this Comment
                                                                                     //CampaignDashboardChartResult = new CampaignDashboardChartResult();
                _mapping.CampaignDashboardChartResult = CampaignDashboardChartResult;

                ProfileGeographicOptions(_mapping.CampaignProfileGeographicModel, model.CountryId);
                ProfileDemographicsOptions(_mapping.CampaignProfileDemographicsmodel, model.CountryId);
                ProfileAdvertOptions(_mapping.CampaignProfileAd, model.CountryId);
                ProfileMobileFormOptions(_mapping.CampaignProfileMobileFormModel, model.CountryId);
                
                return View(Tuple.Create(model, _mapping));
            }
            else
            {
                ViewBag.ClientId = null;
                ViewBag.CampaignProfileId = null;
            }
            return View("Index");


        }

        private CampaignProfileTimeSettingFormModel Timing(int id, CampaignProfileTimeSettingFormModel Timing)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileTimeSettings != null &&
                   campaignProfile.CampaignProfileTimeSettings.Count != 0)
                {
                    CampaignProfileTimeSetting CampaignProfileTimeSettings =
                        campaignProfile.CampaignProfileTimeSettings.FirstOrDefault();

                    Timing = new CampaignProfileTimeSettingFormModel
                    {
                        CampaignProfileId =
                                          CampaignProfileTimeSettings.CampaignProfileId,
                        CampaignProfileTimeSettingsId =
                                          CampaignProfileTimeSettings.
                                          CampaignProfileTimeSettingsId
                    };
                    if (CampaignProfileTimeSettings.Monday != null)
                        Timing.MondaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Monday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Tuesday != null)
                        Timing.TuesdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Tuesday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Wednesday != null)
                        Timing.WednesdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Wednesday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Thursday != null)
                        Timing.ThursdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Thursday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Friday != null)
                        Timing.FridaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Friday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Saturday != null)
                        Timing.SaturdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Saturday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Sunday != null)
                        Timing.SundaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Sunday.Split(",".ToCharArray()));

                    Timing.AvailableTimes = GetTimes();

                }
            }
            else
            {
                Timing = new CampaignProfileTimeSettingFormModel { CampaignProfileId = id };
                Timing.AvailableTimes = GetTimes();
            }

            return Timing;
        }

        public IList<TimeOfDay> GetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay { Id = "01:00", Name = "01:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "02:00", Name = "02:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "03:00", Name = "03:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "04:00", Name = "04:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "05:00", Name = "05:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "06:00", Name = "06:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "07:00", Name = "07:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "08:00", Name = "08:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "09:00", Name = "09:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "10:00", Name = "10:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "11:00", Name = "11:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "12:00", Name = "12:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "13:00", Name = "13:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "14:00", Name = "14:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "15:00", Name = "15:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "16:00", Name = "16:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "17:00", Name = "17:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "18:00", Name = "18:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "19:00", Name = "19:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "20:00", Name = "20:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "21:00", Name = "21:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "22:00", Name = "22:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "23:00", Name = "23:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "24:00", Name = "24:00", IsSelected = false });

            return times;
        }

        public IList<TimeOfDay> ConvertTimesArrayToList(string[] selectedTimes)
        {
            return selectedTimes.Select(time => new TimeOfDay { Id = time, Name = time, IsSelected = true }).ToList();
        }

        [Route("LoadCampaign")]
        [HttpPost]
        public JsonResult LoadCampaign(DTParameters param)
        {
            try
            {               
                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }

                int cnt = 50;

                int id = (int)Session["CampId"];
                double totalcredit = 0;
                double spendtodate = 0;
                double playcost = 0;
                double emailcost = 0;
                double smscost = 0;
                if (TempData["commusuccess"] != null)
                {
                    ViewBag.commusuccess = TempData["commusuccess"];
                }
                if (TempData["commuerror"] != null)
                {
                    ViewBag.commuerror = TempData["commuerror"];

                }
                List<CampaignAuditResult> _audit = new List<CampaignAuditResult>();
                var advertname = string.Empty;
                var advertid = 0;
                var emailstatus = string.Empty;
                var smsstatus = string.Empty;


                if (_profileRepository.Count(x => x.CampaignProfileId == id && (x.Status != 5)) > 0)
                {

                    var CountryID = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id).CountryId;
                    var ConnString = ConnectionString.GetConnectionString(CountryID);

                    CampaignProfile profile = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id);

                    totalcredit = (double)profile.TotalCredit;

                    IEnumerable<CampaignAuditFormModel> model = null;

                    #region Search And Load
                    if (searchValue == true)
                    {
                        #region Searching Functionality
                        //cnt = _profileRepository.GetAll().Count();
                        //int[] playId = new int[cnt];
                        //int[] userId = new int[cnt];
                        //int[] CampaignAdvertId = new int[cnt];
                        //int[] CampaignStatusId = new int[cnt];

                        int playId = 0, userId = 0, status = 0, sms = 0, fromLengthOfPlay = 0, toLengthOfPlay = 0, fromPlayCost = 0, toPlayCost = 0, fromTotalCost = 0, toTotalCost = 0;
                        string disStatus = null, disSms = null;
                        DateTime startTime = new DateTime();
                        DateTime endTime = new DateTime();
                        if (!String.IsNullOrEmpty(columnSearch[0]))
                        {
                            if (columnSearch[0] != "null")
                            {
                                playId = Convert.ToInt32(columnSearch[0]);
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
                                userId = Convert.ToInt32(columnSearch[1]);
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
                                startTime = Convert.ToDateTime(columnSearch[2]).Date;
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
                                endTime = Convert.ToDateTime(columnSearch[3]).Date;
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
                                var data = columnSearch[4].Split(',').ToArray();
                                fromLengthOfPlay = Convert.ToInt32(data[0]);
                                toLengthOfPlay = Convert.ToInt32(data[1]);
                            }
                            else
                            {
                                columnSearch[4] = null;
                            }
                        }
                        if (!String.IsNullOrEmpty(columnSearch[6]))
                        {
                            if (columnSearch[6] != "null")
                            {
                                status = Convert.ToInt32(columnSearch[6]);
                                disStatus = status == 1 ? "Played" : status == 2 ? "Cancelled" : status == 3 ? "Short" : null;
                            }
                            else
                            {
                                columnSearch[6] = null;
                            }
                        }
                        if (!String.IsNullOrEmpty(columnSearch[7]))
                        {
                            if (columnSearch[7] != "null")
                            {
                                var data = columnSearch[7].Split(',').ToArray();
                                fromPlayCost = Convert.ToInt32(data[0]);
                                toPlayCost = Convert.ToInt32(data[1]);
                            }
                            else
                            {
                                columnSearch[7] = null;
                            }
                        }

                        if (!String.IsNullOrEmpty(columnSearch[8]))
                        {
                            if (columnSearch[8] != "null")
                            {
                                sms = Convert.ToInt32(columnSearch[8]);
                                disSms = sms == 1 ? "SMS" : sms == 2 ? "No" : sms == 3 ? "SMS" : null;
                            }
                            else
                            {
                                columnSearch[8] = null;
                            }
                        }

                        if (!String.IsNullOrEmpty(columnSearch[12]))
                        {
                            if (columnSearch[12] != "null")
                            {
                                var data = columnSearch[12].Split(',').ToArray();
                                fromTotalCost = Convert.ToInt32(data[0]);
                                toTotalCost = Convert.ToInt32(data[1]);
                            }
                            else
                            {
                                columnSearch[12] = null;

                            }
                        }

                        if (!string.IsNullOrEmpty(ConnString))
                        {
                            EFMVCDataContex db = new EFMVCDataContex(ConnString);
                            model = db.CampaignAudits.Where(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                                 || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                                 || (s.Status == disStatus)
                                                                 || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                                 || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                                 || (s.SMS == disSms)
                                                                 || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                                 || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                                 ))
                                                                .OrderByDescending(s => s.StartTime).Skip(param.Start).Take(param.Length)
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
                            cnt = db.CampaignAudits.Where(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                                 || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                                 || (s.Status == disStatus)
                                                                 || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                                 || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                                 || (s.SMS == disSms)
                                                                 || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                                 || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                                 )).Count();


                        }
                        else
                        {
                            EFMVCDataContex db = new EFMVCDataContex();
                            model = db.CampaignAudits.Where(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                                 || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                                 || (s.Status == disStatus)
                                                                 || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                                 || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                                 || (s.SMS == disSms)
                                                                 || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                                 || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                                 ))
                                                                .OrderByDescending(s => s.StartTime).Skip(param.Start).Take(param.Length)
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
                            cnt = db.CampaignAudits.Where(s => s.CampaignProfileId == id && (s.CampaignAuditId == playId || s.UserProfileId == userId
                                                                 || ((s.PlayLengthTicks / 1000) >= fromLengthOfPlay && (s.PlayLengthTicks / 1000) <= toLengthOfPlay)
                                                                 || (s.Status == disStatus)
                                                                 || (s.BidValue >= fromPlayCost && s.BidValue <= toPlayCost)
                                                                 || (s.TotalCost >= fromTotalCost && s.TotalCost <= toTotalCost)
                                                                 || (s.SMS == disSms)
                                                                 || (s.StartTime.Year == startTime.Year && s.StartTime.Month == startTime.Month && s.StartTime.Day == startTime.Day)
                                                                 || (s.EndTime.Year == endTime.Year && s.EndTime.Month == endTime.Month && s.EndTime.Day == endTime.Day)
                                                                 )).Count();
                        }

                        #endregion
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ConnString))
                        {
                            EFMVCDataContex db = new EFMVCDataContex(ConnString);
                            cnt = db.CampaignAudits.Where(s => s.CampaignProfileId == id).Count();
                            //if (param.Length == -1)
                            //    param.Length = cnt;

                            model = db.CampaignAudits.Where(s => s.CampaignProfileId == id).OrderByDescending(s => s.StartTime).Skip(param.Start).Take(param.Length)
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




                        }
                        else
                        {
                            cnt = profile.CampaignAudits.Count();
                            model = Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits).Skip(param.Start).Take(param.Length).ToList();

                        }
                    }

                    #endregion

                    foreach (var item in model)
                    {

                        playcost = playcost + item.BidValue;
                        emailcost = emailcost + item.EmailCost;
                        smscost = smscost + item.SMSCost;
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
                            if (item.Email.Trim().ToLower() == "none")
                            {
                                emailstatus = "No";
                            }
                            else if (item.Email.Trim().ToLower() == "requested")
                            {
                                emailstatus = "Yes";
                            }
                            else
                            {
                                emailstatus = "Both";
                            }
                        }
                        if (!String.IsNullOrEmpty(item.SMS))
                        {
                            if (item.SMS.Trim().ToLower() == "none")
                            {
                                smsstatus = "No";
                            }
                            else if (item.SMS.Trim().ToLower() == "requested")
                            {
                                smsstatus = "Yes";
                            }
                            else
                            {
                                smsstatus = "Both";
                            }
                        }
                        _audit.Add(new CampaignAuditResult { PlayID = item.CampaignAuditId, UserID = item.UserProfileId, StartDate = item.StartTime.ToString("MM/dd/yyyy hh:mm:ss"), EndDate = item.EndTime.ToString("MM/dd/yyyy hh:mm:ss"), LengthOfPlay = (int)item.PlayLengthTicks /*item.PlayLength.Seconds*/, AdvertName = advertname, AdvertId = advertid, Status = item.Status, PlayCost = RoundUp(item.BidValue, 2), SMS = smsstatus, SMSCost = item.SMSCost, Email = emailstatus, EmailCost = item.EmailCost, TotalCost = RoundUp(item.TotalCost, 2), DisplayStartDate = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss"), DisplayEndDate = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss") });
                    }
                    spendtodate = playcost + emailcost + smscost;
                    ViewData["maxspendtodate"] = RoundUp(spendtodate, 2);
                    ViewData["maxremainding"] = RoundUp((totalcredit - spendtodate), 0);
                }
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


        private void FillCountryList()
        {
            var country = (from action in _countryRepository.GetAll()
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();
            ViewBag.countryList = country;
        }

        public void FillCampaignAuditStatus()
        {
            IEnumerable<Common.CampaignAuditStatus> audTypes = Enum.GetValues(typeof(Common.CampaignAuditStatus))
                                                     .Cast<Common.CampaignAuditStatus>();
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


            IEnumerable<Common.CampaignAuditSMSStatus> audsmsTypes = Enum.GetValues(typeof(Common.CampaignAuditSMSStatus))
                                                     .Cast<Common.CampaignAuditSMSStatus>();
            var auditsmsstatus = (from action in audsmsTypes
                                  select new SelectListItem
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            auditsmsstatus.Insert(0, new SelectListItem { Text = "--Select Status--", Value = "0" });
            ViewBag.campaignauditSMSStatus = auditsmsstatus;
        }

        private void FillAddClient(IEnumerable<ClientModel> clientModels)
        {
            var clientdetails = clientModels.Select(top => new SelectListItem
            {
                Text = top.Name,
                Value = top.Id.ToString(),
            });
            var client = clientdetails.ToList();
            ViewBag.client = client;
        }

        private CampaignProfileGeographicFormModel ProfileGeographicOptions(CampaignProfileGeographicFormModel model, int countryId)
        {
            model.Location = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Location);
            return model;
        }

        private CampaignProfileDemographicsFormModel ProfileDemographicsOptions(CampaignProfileDemographicsFormModel model, int countryId)
        {
            model.Age = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Age);
            model.Gender = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Gender);
            model.HouseholdStatus = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.HouseholdStatus);
            model.WorkingStatus = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.WorkingStatus);
            model.RelationshipStatus = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.RelationshipStatus);
            model.Education = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Education);
            return model;
        }

        private CampaignProfileAdvertFormModel ProfileAdvertOptions(CampaignProfileAdvertFormModel model, int countryId)
        {
            model.Food = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Food);
            model.SweetsSnacks = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.SweetsSnacks);
            model.AlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.AlcoholicDrinks);
            model.NonAlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.NonAlcoholicDrinks);
            model.HouseholdAppliancesProducts = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.HouseholdAppliancesProducts);
            model.ToiletriesCosmetics = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.ToiletriesCosmetics);
            model.PharmaceuticalChemistsProducts = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.PharmaceuticalChemistsProducts);
            model.TobaccoProducts = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.TobaccoProducts);
            model.Pets = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Pets);
            model.ClothingFashion = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.ClothingFashion);
            model.DIYGardening = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.DIYGardening);
            model.ElectronicsOtherPersonalItems = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.ElectronicsOtherPersonalItems);
            model.CommunicationsInternetTelecom = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.CommunicationsInternetTelecom);
            model.FinancialServices = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.FinancialServices);
            model.HolidaysTravelTourism = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.HolidaysTravelTourism);
            model.SportsLeisure = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.SportsLeisure);
            model.MotoringAutomotive = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.MotoringAutomotive);
            model.NewspapersMagazines = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.NewspapersMagazines);
            model.TvVideoRadio = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.TvVideoRadio);
            model.Cinema = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Cinema);
            model.SocialNetworking = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.SocialNetworking);
            model.Shopping = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Shopping);
            model.Fitness = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Fitness);
            model.Environment = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Environment);
            model.GoingOutEntertainment = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.GoingOutEntertainment);
            model.Religion = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Religion);
            model.Music = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Music);
            model.BusinessOpportunities = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.BusinessOpportunities);
            model.Over18Gambling = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Over18Gambling);
            model.Restaurants = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Restaurants);
            model.Insurance = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Insurance);
            model.Furniture = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Furniture);
            model.Informationtechnology = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Informationtechnology);
            model.Energy = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Energy);
            model.Supermarkets = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Supermarkets);
            model.Healthcare = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Healthcare);
            model.JobsandEducation = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.JobsandEducation);
            model.Gifts = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Gifts);
            model.AdvocacyLegal = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.AdvocacyLegal);
            model.DatingPersonal = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.DatingPersonal);
            model.RealEstateProperty = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.RealEstateProperty);
            model.Games = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Games);
            model.SkizaProfile = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.SkizaProfile);

            return model;

        }

        private CampaignProfileMobileFormModel ProfileMobileFormOptions(CampaignProfileMobileFormModel model, int countryId)
        {
            model.ContractType = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.Mobileplan);
            model.AverageMonthlySpend = GetProfileMatchOption.IsActiveProfileInfo(countryId, (int)ProfileMatchInfo.AverageMonthlySpend);
            return model;
        }

        private CampaignProfileFormModel GetEditData(int id)
        {
            if (_profileRepository.Count(x => x.CampaignProfileId == id && (x.Status != 5)) == 0)
                return null;

            CampaignProfile campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == id);
            ViewBag.campaignName = campaignProfile.CampaignName;
            if (campaignProfile.NumberInBatch == 0)
                campaignProfile.NumberInBatch = 1;
            CampaignProfileFormModel model = Mapper.Map<CampaignProfile, CampaignProfileFormModel>(campaignProfile);

            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59,
                                                    59);

            DateTime previousMonthStart;
            DateTime previousMonthEnd;

            if (DateTime.Now.Month == 1)
            {
                previousMonthStart = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
                previousMonthEnd = new DateTime(DateTime.Now.Year - 1, 12,
                                                DateTime.DaysInMonth(DateTime.Now.Year - 1, 12), 23, 59,
                                                59);
            }
            else
            {
                previousMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0);
                previousMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1,
                                                DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1), 23, 59,
                                                59);
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

            ViewData.Add("previousMonthPlayCount", previousMonthPlayCount);
            ViewData.Add("previousMonthSMSCount", previousMonthSMSCount);
            ViewData.Add("previousMonthEmailCount", previousMonthEmailCount);
            ViewData.Add("currentMonthSMSCount", currentMonthSMSCount);
            ViewData.Add("currentMonthEmailCount", currentMonthEmailCount);
            ViewData.Add("totalPlayCount", totalPlayCount);
            ViewData.Add("currentMonthPlayCount", currentMonthPlayCount);
            ViewData.Add("totalSMSCount", totalSMSCount);
            ViewData.Add("totalEmailCount", totalEmailCount);
            CampaignStatus campaignstatus = (CampaignStatus)campaignProfile.Status;
            string status = campaignstatus.ToString();
            ViewData.Add("status", status);
            ViewData.Add("maxremainding", campaignProfile.TotalCredit - (decimal)campaignProfile.SpendToDate);
            ViewData.Add("maxspendtodate", campaignProfile.SpendToDate);
            if (!String.IsNullOrEmpty(campaignProfile.EmailFileLocation))
            {

                ViewData.Add("emailfilelocation", ConfigurationManager.AppSettings["siteAddress"].ToString() + campaignProfile.EmailFileLocation);
            }
            else
            {
                ViewData.Add("emailfilelocation", String.Empty);

            }
            if (!String.IsNullOrEmpty(campaignProfile.SMSFileLocation))
            {
                ViewData.Add("smsfilelocation", ConfigurationManager.AppSettings["siteAddress"].ToString() + campaignProfile.SMSFileLocation);
            }
            else
            {
                ViewData.Add("smsfilelocation", String.Empty);
            }
            ViewBag.campaignName = campaignProfile.CampaignName;

            return model;
        }

        private IEnumerable<AdvertFormModel> advertFormModels()
        {
            IEnumerable<Advert> adverts = _advertRepository.GetMany(x => x.Status != 4);
            IEnumerable<AdvertFormModel> advertFormModels =
                Mapper.Map<IEnumerable<Advert>, IEnumerable<AdvertFormModel>>(adverts);
            return advertFormModels;
        }
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        public CampaignDashboardChartResult FillChartDataByCampaignId(CampaignProfileFormModel _CampaignProfileFormModel)
        {
            List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
            List<CampaignNoOfPlay> _campaignNoofplay = new List<CampaignNoOfPlay>();
            List<Campaignchartresult> _campaignbidavgResult = new List<Campaignchartresult>();
            List<CampaignAvgbid> _campaignAvgbid = new List<CampaignAvgbid>();
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            float PlaystoDate = 0;
               decimal TotalBudget = 0;
            double AverageBid = 0, SpendToDate = 0, MaxBid = 0, AveragePlayTime = 0, SMSCost = 0, EmailCost = 0, Cancelled = 0;
            List<long> _maxplaylength = new List<long>();
            int FreePlays = 0;
            DateTime? maxdate = new DateTime(), mindate = new DateTime();
            if (_CampaignProfileFormModel != null)
            {
                TotalBudget = _CampaignProfileFormModel.TotalBudget;
                var campaignMaxBid = _CampaignProfileFormModel.MaxBid;


                List<CampaignAudit> campaignAudit = null;
                var ConnString = ConnectionString.GetConnectionString(_CampaignProfileFormModel.CountryId);
                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    campaignAudit = db.CampaignAudits.Where(s => s.CampaignProfileId == _CampaignProfileFormModel.CampaignProfileId).ToList();
                }
                else
                {
                    EFMVCDataContex db = new EFMVCDataContex();
                    campaignAudit = db.CampaignAudits.Where(s => s.CampaignProfileId == _CampaignProfileFormModel.CampaignProfileId).ToList();
                }


                if (campaignAudit.Count > 0)
                {
                    //caculate the PlaystoDate field
                    var playedCampaignAuditData = campaignAudit.Where(top => top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase && top.PlayLengthTicks > 6000);
                    if (playedCampaignAuditData.Count() > 0)
                    {
                        PlaystoDate = PlaystoDate + playedCampaignAuditData.Count();
                        SpendToDate = SpendToDate + playedCampaignAuditData.Sum(top => top.BidValue);
                        SpendToDate = SpendToDate + playedCampaignAuditData.Sum(top => top.SMSCost);
                        SpendToDate = SpendToDate + playedCampaignAuditData.Sum(top => top.EmailCost);
                        AverageBid = AverageBid + playedCampaignAuditData.Average(top => top.BidValue);
                        AveragePlayTime = AveragePlayTime + playedCampaignAuditData.Average(top => top.PlayLengthTicks);
                        _maxplaylength.Add(playedCampaignAuditData.Max(top => top.PlayLengthTicks));
                        MaxBid = MaxBid + playedCampaignAuditData.Max(top => top.BidValue);

                        _campaignNoofplay = playedCampaignAuditData.Select(s => new CampaignNoOfPlay { startdate = s.StartTime, playcount = 1, startdatecompare = s.StartTime }).ToList();
                        _campaignAvgbid = playedCampaignAuditData.Select(s => new CampaignAvgbid { startdate = s.StartTime, bidvalue = s.BidValue, startdatecompare = s.StartTime }).ToList();
                        _playgroup = playedCampaignAuditData.Select(s => new MaxLengthGroup { second = (s.PlayLengthTicks / 1000) }).ToList();
                    }

                    //caculate the FreePlays field
                    var freePlaysCount = _CampaignProfileFormModel.GetDomainCampaignAudits(_campaignAuditRepository).Count(top => top.PlayLengthTicks <= 6000 && top.Status.ToLower() == CampaignAuditStatusExtensions.PlayedAsLowerCase);
                    if (freePlaysCount > 0)
                    {
                        FreePlays = FreePlays + freePlaysCount;
                    }

                    SMSCost = SMSCost + campaignAudit.Count(d1 => !string.IsNullOrWhiteSpace(d1.SMS));
                    EmailCost = EmailCost + campaignAudit.Count(d1 => !string.IsNullOrWhiteSpace(d1.Email));
                    Cancelled = Cancelled + campaignAudit.Where(top => top.Status.Trim().ToLower() == CampaignAuditStatusExtensions.CancelledAsLowerCase).Sum(d1 => d1.TotalCost);

                }
                //Divide average play time by 1000.
                if (AveragePlayTime != 0)
                {
                    AveragePlayTime = AveragePlayTime / 1000;
                }
            }
            if (_campaignNoofplay.Count > 0)
            {


                maxdate = _campaignNoofplay.Max(top => top.startdate.AddDays(1));
                mindate = maxdate.Value.Date.AddYears(-1);
            }
            GetCampaignbidplaydata(_campaignNoofplay, _campaignAvgbid, _campaignbidavgResult, maxdate, mindate);
            int[] _getbarChartdata = _getbarChartData(_playgroup);
            if (_getbarChartdata == null)
            {
                ViewBag.getbarChartdata = _getbarChartdata;
            }
            else
            {
                ViewBag.getbarChartdata = _getbarChartdata.ToArray();
            }
            _CampaignDashboardChartResult.PlaystoDate = PlaystoDate;
            _CampaignDashboardChartResult.SpendToDate = RoundUp(SpendToDate, 0);
            _CampaignDashboardChartResult.AverageBid = RoundUp(AverageBid, 2);
            _CampaignDashboardChartResult.AveragePlayTime = RoundUp(AveragePlayTime, 2);
            _CampaignDashboardChartResult.FreePlays = FreePlays;
            _CampaignDashboardChartResult.TotalBudget = TotalBudget;
            _CampaignDashboardChartResult.TotalSpend = RoundUp(SpendToDate, 0);
            _CampaignDashboardChartResult.MaxBid = RoundUp(MaxBid, 2);
            _CampaignDashboardChartResult.AvgMaxBid = RoundUp(AverageBid, 2);
            if (_maxplaylength.Count > 0)
            {
                var maxPlayLength = (_maxplaylength.Max()) / 1000;
                _CampaignDashboardChartResult.MaxPlayLength = maxPlayLength;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = RoundUp(((maxPlayLength / AveragePlayTime)) * 100, 0);
            }
            else
            {
                _CampaignDashboardChartResult.MaxPlayLength = 0;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = 0;
            }
            //caculate the FreePlays Percantage field
            if (PlaystoDate > 0 && FreePlays > 0)
                _CampaignDashboardChartResult.FreePlaysPercantage = RoundUp(((FreePlays / (PlaystoDate + FreePlays)) * 100), 0);
            else
                _CampaignDashboardChartResult.FreePlaysPercantage = 0;

            //caculate the TotalBudget Percantage field
            if (SpendToDate > 0 && TotalBudget > 0)
                _CampaignDashboardChartResult.TotalBudgetPercantage = RoundUp(((SpendToDate / (double)TotalBudget) * 100), 0);

            else
                _CampaignDashboardChartResult.TotalBudgetPercantage = 0;

            //caculate the MaxBid Percantage field
            if (AverageBid > 0 && MaxBid > 0)
                _CampaignDashboardChartResult.MaxBidPercantage = RoundUp(((AverageBid / MaxBid) * 100), 0);
            else
                _CampaignDashboardChartResult.MaxBidPercantage = 0;

            ViewBag.FreePlays = FreePlays;
            ViewBag.TotalPlayed = PlaystoDate;
            ViewBag.TotalBudget = TotalBudget;
            ViewBag.TotalSpend = RoundUp(SpendToDate, 0);
            ViewBag.MaxBid = RoundUp(MaxBid, 2);
            ViewBag.AvgMaxBid = RoundUp(AverageBid, 2);
            _CampaignDashboardChartResult.EmailCost = EmailCost;
            _CampaignDashboardChartResult.SMSCost = SMSCost;
            _CampaignDashboardChartResult.Cancelled = Cancelled;
            return _CampaignDashboardChartResult;
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
                    ViewBag.AvgbidMaxCount = 0;
                    ViewBag.NoofplayMaxCount = 0;
                    ViewBag.Campaignavgplayresult = null;
                }
            }
            else
            {
                ViewBag.AvgbidMaxCount = 0;
                ViewBag.NoofplayMaxCount = 0;
                ViewBag.Campaignavgplayresult = null;
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

        //Old Function
        //private CampaignProfileGeographicFormModel CampaignProfileGeographic(int id, CampaignProfileGeographicFormModel CampaignProfileGeo)
        //{
        //    CampaignProfile campaignProfile = _profileRepository.GetById(id);

        //    //if (campaignProfile != null)
        //    //{
        //    //    if (campaignProfile.CampaignProfilePreferences != null &&
        //    //       campaignProfile.CampaignProfilePreferences.Count != 0)
        //    //    {
        //    //        CampaignProfilePreference CampaignProfileGeograph =
        //    //           campaignProfile.CampaignProfilePreferences.FirstOrDefault();
        //    //        bool status = true; // checkCampaignProfileGeographic(CampaignProfileGeograph);
        //    //        if (status == false)
        //    //        {
        //    //            CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id, CampaignProfileGeographicId = CampaignProfileGeograph.Id };
        //    //        }
        //    //        else
        //    //        {
        //    //            CampaignProfileGeo = Mapper.Map<CampaignProfilePreference, CampaignProfileGeographicFormModel>(CampaignProfileGeograph);
        //    //            CampaignProfileGeo.CampaignProfileGeographicId = CampaignProfileGeograph.Id;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id };

        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id };
        //    //}

        //    return CampaignProfileGeo;
        //}

        //New Function
        private CampaignProfileGeographicFormModel CampaignProfileGeographic(int id, CampaignProfileGeographicFormModel CampaignProfileGeo)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            var Id = _profileRepository.GetById(id).CountryId;
            int CountryId = Convert.ToInt32(Id);
            var profileLabel = _profileMatchInformationRepository.GetMany(top => top.CountryId == CountryId);
            if (profileLabel == null || profileLabel.Count() == 0)
            {
                CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }
            else
            {
                var cId = profileLabel.Where(top => top.CountryId == Convert.ToInt32(Id)).Select(top => top.CountryId).FirstOrDefault();
                CountryId = cId.Value;
            }
            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                   campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference CampaignProfileGeograph =
                       campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = true; // checkCampaignProfileGeographic(CampaignProfileGeograph);
                    if (status == false)
                    {
                        CampaignProfileGeo = new CampaignProfileGeographicFormModel(CountryId) { CampaignProfileId = id, CampaignProfileGeographicId = CampaignProfileGeograph.Id };
                    }
                    else
                    {
                        CampaignProfileGeo = Mapper.Map<CampaignProfilePreference, CampaignProfileGeographicFormModel>(CampaignProfileGeograph);
                        CampaignProfileGeo.CampaignProfileGeographicId = CampaignProfileGeograph.Id;
                    }
                }
                else
                {
                    CampaignProfileGeo = new CampaignProfileGeographicFormModel(CountryId) { CampaignProfileId = id };

                }
            }
            else
            {
                CampaignProfileGeo = new CampaignProfileGeographicFormModel(CountryId) { CampaignProfileId = id };
            }

            return CampaignProfileGeo;
        }

        public bool checkcampaignProfileDemographics(CampaignProfilePreference campaignProfileDemographics)
        {
            if (String.IsNullOrEmpty(campaignProfileDemographics.Age_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.Gender_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.IncomeBracket_Demographics) &&
                String.IsNullOrEmpty(campaignProfileDemographics.WorkingStatus_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.RelationshipStatus_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.Education_Demographics)
                    && String.IsNullOrEmpty(campaignProfileDemographics.HouseholdStatus_Demographics))// code commented on 29-03-2017 && String.IsNullOrEmpty(campaignProfileDemographics.Location_Demographics))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Old Function
        //private CampaignProfileDemographicsFormModel CampaignProfileDemographic(int id, CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel)
        //{
        //    CampaignProfile campaignProfile = _profileRepository.GetById(id);

        //    if (campaignProfile != null)
        //    {
        //        if (campaignProfile.CampaignProfilePreferences != null &&
        //            campaignProfile.CampaignProfilePreferences.Count != 0)
        //        {
        //            CampaignProfilePreference campaignProfileDemographics =
        //                campaignProfile.CampaignProfilePreferences.FirstOrDefault();
        //            bool status = checkcampaignProfileDemographics(campaignProfileDemographics);
        //            if (status == false)
        //            {
        //                CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = campaignProfileDemographics.Id };
        //            }
        //            else
        //            {
        //                CampaignProfileDemographicsmodel =
        //                    Mapper.Map<CampaignProfilePreference, CampaignProfileDemographicsFormModel>(
        //                        campaignProfileDemographics);
        //                CampaignProfileDemographicsmodel.CampaignProfileDemographicsId = campaignProfileDemographics.Id;
        //                CampaignProfileDemographicsmodel.CampaignProfileId = id;
        //            }

        //        }
        //        else
        //        {

        //            CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
        //        }
        //    }
        //    else
        //    {
        //        CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
        //    }

        //    return CampaignProfileDemographicsmodel;
        //}

        // New Function
        private CampaignProfileDemographicsFormModel CampaignProfileDemographic(int id, CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            var Id = _profileRepository.GetById(id).CountryId;
            int CountryId = Convert.ToInt32(Id);
            var profileLabel = _profileMatchInformationRepository.GetMany(top => top.CountryId == CountryId);
            if (profileLabel == null || profileLabel.Count() == 0)
            {
                CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }
            else
            {
                var cId = profileLabel.Where(top => top.CountryId == Convert.ToInt32(Id)).Select(top => top.CountryId).FirstOrDefault();
                CountryId = cId.Value;
            }
            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                    campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileDemographics =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileDemographics(campaignProfileDemographics);
                    if (status == false)
                    {
                        CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId) { CampaignProfileId = id, CampaignProfileDemographicsId = campaignProfileDemographics.Id };
                    }
                    else
                    {
                        CampaignProfileDemographicsmodel =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileDemographicsFormModel>(
                                campaignProfileDemographics);
                        CampaignProfileDemographicsmodel.CampaignProfileDemographicsId = campaignProfileDemographics.Id;
                        CampaignProfileDemographicsmodel.CampaignProfileId = id;
                    }

                }
                else
                {

                    CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId) { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
                }
            }
            else
            {
                CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId) { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
            }

            return CampaignProfileDemographicsmodel;
        }

        private CampaignProfileAdvertFormModel CampaignProfileAdvert(int id, CampaignProfileAdvertFormModel Campaignad)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);
            var Id = _profileRepository.GetById(id).CountryId;
            int CountryId = Convert.ToInt32(Id);
            var profileLabel = _profileMatchInformationRepository.GetMany(top => top.CountryId == CountryId);
            if (profileLabel == null || profileLabel.Count() == 0)
            {
                CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }
            else
            {
                var cId = profileLabel.Where(top => top.CountryId == Convert.ToInt32(Id)).Select(top => top.CountryId).FirstOrDefault();
                CountryId = cId.Value;
            }
            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileAdvert =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileAdvert(campaignProfileAdvert);
                    if (status == false)
                    {
                        //Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = campaignProfileAdvert.Id };
                        Campaignad = new CampaignProfileAdvertFormModel(CountryId) { CampaignProfileId = id, CampaignProfileAdvertsId = campaignProfileAdvert.Id };
                    }
                    else
                    {
                        Campaignad =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileAdvertFormModel>(campaignProfileAdvert);
                        Campaignad.CampaignProfileAdvertsId = campaignProfileAdvert.Id;
                        Campaignad.CampaignProfileId = id;
                    }

                }
                else
                {
                    //Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };
                    Campaignad = new CampaignProfileAdvertFormModel(CountryId) { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };

                }
            }
            else
            {
                //Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };
                Campaignad = new CampaignProfileAdvertFormModel(CountryId) { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };
            }
            return Campaignad;
        }

        public bool checkcampaignProfileAdvert(CampaignProfilePreference campaignProfileAdvert)
        {
            if (string.IsNullOrEmpty(campaignProfileAdvert.AlcoholicDrinks_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.AppliancesOtherHouseholdDurables_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Cinema_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.CommunicationsInternet_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.DIYGardening_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.ElectronicsOtherPersonalItems_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.Environment_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Fashion_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.FinancialProducts_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.FinancialServices_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Fitness_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Food_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.GeneralUse_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.GoingOut_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Holidays_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.HolidaysTravel_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Householdproducts_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Magazines_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.Motoring_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Music_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Newspapers_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.NonAlcoholicDrinks_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.PetsPetFood_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.PharmaceuticalChemistsProducts_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.Radio_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Religion_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.Shopping_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.ShoppingRetailClothing_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.SocialNetworking_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.SportsLeisure_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.SweetSaltySnacks_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.TV_Advert) && string.IsNullOrEmpty(campaignProfileAdvert.TobaccoProducts_Advert)
                && string.IsNullOrEmpty(campaignProfileAdvert.ToiletriesCosmetics_Advert)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private CampaignProfileSkizaFormModel CampaignProfileSkizaInormation(int id, CampaignProfileSkizaFormModel CampaignSkiza)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);
            var Id = _profileRepository.GetById(id).CountryId;
            int CountryId = Convert.ToInt32(Id);
            var profileLabel = _profileMatchInformationRepository.GetMany(top => top.CountryId == CountryId);
            if (profileLabel == null || profileLabel.Count() == 0)
            {
                CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }
            else
            {
                var cId = profileLabel.Where(top => top.CountryId == Convert.ToInt32(Id)).Select(top => top.CountryId).FirstOrDefault();
                CountryId = cId.Value;
            }
            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileskiza =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileSkiza(campaignProfileskiza);
                    if (status == false)
                    {
                        //CampaignSkiza = new CampaignProfileSkizaFormModel { CampaignProfileId = id, CampaignProfileSKizaId = campaignProfileskiza.Id };
                        CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = campaignProfileskiza.Id, CountryId = (int)Id };
                    }
                    else
                    {
                        campaignProfileskiza.CountryId = (int)Id;
                        CampaignSkiza =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileSkizaFormModel>(campaignProfileskiza);
                        CampaignSkiza.CampaignProfileSKizaId = campaignProfileskiza.Id;
                        CampaignSkiza.CampaignProfileId = id;
                        CampaignSkiza.CountryId = CountryId;
                    }

                }
                else
                {
                    //CampaignSkiza = new CampaignProfileSkizaFormModel { CampaignProfileId = id, CampaignProfileSKizaId = 0 };
                    CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = 0, CountryId = (int)Id };

                }
            }
            else
            {
                //CampaignSkiza = new CampaignProfileSkizaFormModel { CampaignProfileId = id, CampaignProfileSKizaId = 0 };
                CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = 0, CountryId = (int)Id };
            }
            return CampaignSkiza;
        }

        public bool checkUserProfileSkiza(CampaignProfilePreference campaignProfileSkiza)
        {
            if (string.IsNullOrEmpty(campaignProfileSkiza.Hustlers_AdType) && string.IsNullOrEmpty(campaignProfileSkiza.Youth_AdType) && string.IsNullOrEmpty(campaignProfileSkiza.DiscerningProfessionals_AdType)
                && string.IsNullOrEmpty(campaignProfileSkiza.Mass_AdType)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private CampaignProfileMobileFormModel Mobile(int id, CampaignProfileMobileFormModel Mobile)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);
            var Id = _profileRepository.GetById(id).CountryId;
            int CountryId = Convert.ToInt32(Id);
            var profileLabel = _profileMatchInformationRepository.GetMany(top => top.CountryId == CountryId);
            if (profileLabel == null || profileLabel.Count() == 0)
            {
                CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }
            else
            {
                var cId = profileLabel.Where(top => top.CountryId == Convert.ToInt32(Id)).Select(top => top.CountryId).FirstOrDefault();
                CountryId = cId.Value;
            }
            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileMobile =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(campaignProfileMobile.ContractType_Mobile) && String.IsNullOrEmpty(campaignProfileMobile.Spend_Mobile))
                    {
                        //Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id, CampaignProfileMobileId = campaignProfileMobile.Id };
                        Mobile = new CampaignProfileMobileFormModel(CountryId) { CampaignProfileId = id, CampaignProfileMobileId = campaignProfileMobile.Id };
                    }
                    else
                    {
                        Mobile = Mapper.Map<CampaignProfilePreference, CampaignProfileMobileFormModel>(campaignProfileMobile);
                        Mobile.CampaignProfileMobileId = campaignProfileMobile.Id;
                        Mobile.CampaignProfileId = id;
                    }

                }
                else
                {
                    //Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id };
                    Mobile = new CampaignProfileMobileFormModel(CountryId) { CampaignProfileId = id };

                }
            }
            else
            {
                //Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id };
                Mobile = new CampaignProfileMobileFormModel(CountryId) { CampaignProfileId = id };
            }

            return Mobile;
        }

       
        [Route("UpdateCampaignInfo")]
        [HttpPost]
        public ActionResult UpdateCampaignInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileCommand command =
                      Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                    command.CampaignDescription = CampaignProfileFormModel.CampaignDescription;
                    command.CampaignName = CampaignProfileFormModel.CampaignName;
                    //command.ClientId = CampaignProfileFormModel.ClientId;
                    command.StartDate = CampaignProfileFormModel.StartDate;
                    command.EndDate = CampaignProfileFormModel.EndDate;
                    command.CountryId = CampaignProfileFormModel.CountryId;
                    
                    var campaigndetails = _profileRepository.GetById(CampaignProfileFormModel.CampaignProfileId);
                    if (campaigndetails != null)
                    {
                        if(CampaignProfileFormModel.ClientId == null)
                        {
                            command.ClientId = 0;
                        }
                        else
                        {
                            command.ClientId = CampaignProfileFormModel.ClientId.Value;
                        }

                        command.Status = campaigndetails.Status;
                        command.Active = campaigndetails.Active;
                        command.AvailableCredit = campaigndetails.AvailableCredit;
                        command.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                        command.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                        command.CancelledToDate = campaigndetails.CancelledToDate;
                        command.CreatedDateTime = campaigndetails.CreatedDateTime;
                        command.EmailToDate = campaigndetails.EmailToDate;
                        command.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                        command.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                        command.MaxBid = campaigndetails.MaxBid;
                        command.MaxMonthBudget = campaigndetails.MaxWeeklyBudget;
                        command.MaxWeeklyBudget = campaigndetails.MaxWeeklyBudget;
                        command.MaxHourlyBudget = campaigndetails.MaxHourlyBudget;
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
                        command.IsAdminApproval = campaigndetails.IsAdminApproval;

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetailsFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailsFromOP != null)
                                {
                                    obj.UpdateCampaignInfo(CampaignProfileFormModel, campaigndetailsFromOP, campaigndetails.UserId, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetailsFromOP.CampaignProfileId, item);
                                }
                            }
                        }

                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                   
                }
                return Json("fail");

            }
            catch (Exception ex)
            {

                return Json("fail");
            }
        }

        [Route("UpdateBudgetInfo")]
        public ActionResult UpdateBudgetInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {

                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                command.MaxBid = CampaignProfileFormModel.MaxBid;
                command.MaxMonthBudget = CampaignProfileFormModel.MaxMonthBudget;
                command.MaxWeeklyBudget = CampaignProfileFormModel.MaxWeeklyBudget;
                command.MaxHourlyBudget = CampaignProfileFormModel.MaxHourlyBudget;
                command.MaxDailyBudget = CampaignProfileFormModel.MaxDailyBudget;
                
                var campaigndetails = _profileRepository.GetById(CampaignProfileFormModel.CampaignProfileId);
                if (campaigndetails != null)
                {
                    command.StartDate = campaigndetails.StartDate;
                    command.EndDate = campaigndetails.EndDate;
                    command.CampaignDescription = campaigndetails.CampaignDescription;
                    command.CampaignName = campaigndetails.CampaignName;
                    command.ClientId = campaigndetails.ClientId;
                    command.Status = campaigndetails.Status;

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
                    command.CountryId = (int)campaigndetails.CountryId;
                    command.IsAdminApproval = campaigndetails.IsAdminApproval;

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();

                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                            var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
                            if (campaigndetailFromOP != null)
                            {
                                obj.UpdateCampaignBudgetInfo(CampaignProfileFormModel, campaigndetailFromOP, campaigndetails.UserId, SQLServerEntities);
                                PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                            }
                        }
                    }

                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {

                        return Json("success");
                    }
                }



                return Json("fail");
            }
            catch (Exception)
            {

                return Json("fail");
            }
        }

        [Route("UpdateCommunicationInfo")]
        public ActionResult UpdateCommunicationInfo(CampaignProfileFormModel CampaignProfileFormModel, HttpPostedFileBase emailfile, HttpPostedFileBase smsfile)
        {
            try
            {
                if (CampaignProfileFormModel.EmailBody == null && CampaignProfileFormModel.EmailSenderAddress == null && CampaignProfileFormModel.EmailSubject == null && CampaignProfileFormModel.SmsBody == null && CampaignProfileFormModel.SmsOriginator == null && emailfile == null && smsfile == null)
                {
                    return Json("Atleast One Field is Required.");
                }

                else
                {

                    CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                    command.EmailBody = CampaignProfileFormModel.EmailBody;
                    command.EmailSenderAddress = CampaignProfileFormModel.EmailSenderAddress;
                    command.EmailSubject = CampaignProfileFormModel.EmailSubject;
                    command.SmsBody = CampaignProfileFormModel.SmsBody;
                    command.SmsOriginator = CampaignProfileFormModel.SmsOriginator;

                    var campaigndetails = _profileRepository.GetById(CampaignProfileFormModel.CampaignProfileId);
                    if (campaigndetails != null)
                    {

                        if (emailfile != null)
                        {
                            if (emailfile.ContentLength != 0)
                            {
                                string fileName = Guid.NewGuid().ToString();
                                string extension = Path.GetExtension(emailfile.FileName);

                                string directoryName = Server.MapPath("~/Email/");
                                directoryName = Path.Combine(directoryName, campaigndetails.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, fileName + extension);
                                emailfile.SaveAs(path);

                                string archiveDirectoryName = Server.MapPath("~/Email/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                emailfile.SaveAs(archivePath);

                                CampaignProfileFormModel.EmailFileLocation = string.Format("/Email/{0}/{1}", campaigndetails.UserId.ToString(),
                                                                        fileName + extension);
                                command.EmailFileLocation = CampaignProfileFormModel.EmailFileLocation;
                            }
                        }
                        else
                        {
                            command.EmailFileLocation = campaigndetails.EmailFileLocation;
                        }
                        if (smsfile != null)
                        {
                            if (smsfile.ContentLength != 0)
                            {
                                string fileName = Guid.NewGuid().ToString();
                                string extension = Path.GetExtension(smsfile.FileName);

                                string directoryName = Server.MapPath("~/SMS/");
                                directoryName = Path.Combine(directoryName, campaigndetails.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, fileName + extension);
                                smsfile.SaveAs(path);

                                string archiveDirectoryName = Server.MapPath("~/SMS/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                smsfile.SaveAs(archivePath);

                                CampaignProfileFormModel.SMSFileLocation = string.Format("/SMS/{0}/{1}", campaigndetails.UserId.ToString(),
                                                                        fileName + extension);
                                command.SMSFileLocation = CampaignProfileFormModel.SMSFileLocation;
                            }
                        }
                        else
                        {
                            command.SMSFileLocation = campaigndetails.SMSFileLocation;
                        }
                        command.StartDate = campaigndetails.StartDate;
                        command.EndDate = campaigndetails.EndDate;
                        command.MaxBid = campaigndetails.MaxBid;
                        command.MaxMonthBudget = campaigndetails.MaxWeeklyBudget;
                        command.MaxWeeklyBudget = campaigndetails.MaxWeeklyBudget;
                        command.MaxHourlyBudget = campaigndetails.MaxHourlyBudget;
                        command.MaxDailyBudget = campaigndetails.MaxDailyBudget;
                        command.CampaignDescription = campaigndetails.CampaignDescription;
                        command.CampaignName = campaigndetails.CampaignName;
                        command.ClientId = campaigndetails.ClientId;
                        command.Status = campaigndetails.Status;

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
                        command.NumberInBatch = campaigndetails.NumberInBatch;
                        command.CountryId = (int)campaigndetails.CountryId;
                        command.IsAdminApproval = campaigndetails.IsAdminApproval;


                        var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();

                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailFromOP != null)
                                {
                                    obj.UpdateCampaignSmsAndEmail(CampaignProfileFormModel, campaigndetailFromOP, command.EmailFileLocation, command.SMSFileLocation, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                }

                            }
                        }


                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                    return Json("fail");
                }
            }
            catch (Exception ex)
            {
                return Json("fail");
               
            }
        }

        [Route("SaveGeographic")]
        [HttpPost]
        public ActionResult SaveGeographic(CampaignProfileGeographicFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileGeographicCommand command =
                        Mapper.Map<CampaignProfileGeographicFormModel, CreateOrUpdateCampaignProfileGeographicCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileGeographicId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileGeographicId = _campaignProfileId.Id;
                        }
                    }
                    var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                    if (campaignProfile != null)
                    {
                        command.CountryId = (int)campaignProfile.CountryId;
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailFromOP != null)
                                {
                                    obj.UpdateCampaignGeographic(model, campaigndetailFromOP, SQLServerEntities);
                                    if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                    {
                                        PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                    }
                                }

                            }
                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileGeographicId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [Route("SaveDemographics")]
        [HttpPost]
        public ActionResult SaveDemographics(CampaignProfileDemographicsFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    int? id = model.CampaignProfileId;
                    CreateOrUpdateCampaignProfileDemographicsCommand command =

                       Mapper.Map<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>(
                           model);
                   
                    if (model.CampaignProfileDemographicsId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            model.CampaignProfileDemographicsId = _campaignProfileId.Id;
                            command.Id = model.CampaignProfileDemographicsId;
                        }
                    }
                    else
                    {
                        command.Id = model.CampaignProfileDemographicsId;
                    }
                    ICommandResult result = _commandBus.Submit(command);

                    int countryId = 0;
                    var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                    if (campaignProfile != null)
                    {
                        countryId = (int)campaignProfile.CountryId;
                    }

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                            var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                            if (campaigndetailFromOP != null)
                            {
                                obj.UpdateCampaignDemographics(model, campaigndetailFromOP, SQLServerEntities);
                                if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                {
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                }
                            }

                        }
                    }


                    if (result.Success)
                    {
                        return Json("success");
                    }
                }

                return Json("success");

            }
            else
            {
                return Json("notauthorise");
            }
        }

        [Route("SaveAdverts")]
        [HttpPost]
        public ActionResult SaveAdverts(CampaignProfileAdvertFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                int? id = model.CampaignProfileId;
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                    CreateOrUpdateCampaignProfileAdvertCommand command =
                        Mapper.Map<CampaignProfileAdvertFormModel, CreateOrUpdateCampaignProfileAdvertCommand>(model);

                    //check campaignprofile exists.

                    if (model.CampaignProfileAdvertsId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileAdvertsId = _campaignProfileId.Id;
                        }
                    }



                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        int countryId = 0;
                        var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                        if (campaignProfile != null)
                        {
                            countryId = (int)campaignProfile.CountryId;
                        }

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailFromOP != null)
                                {
                                    obj.UpdateCampaignAdtypes(model, campaigndetailFromOP, SQLServerEntities);
                                    if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                    {
                                        PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                    }
                                }

                            }
                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileAdvertsId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [Route("SaveSkizaProfile")]
        [HttpPost]
        public ActionResult SaveSkizaProfile(CampaignProfileSkizaFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                int? id = model.CampaignProfileId;
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    
                    if (ModelState.IsValid)
                    {

                        EFMVCDataContex db = new EFMVCDataContex();
                        var campaignProfileData = db.CampaignProfilePreference.Where(s => s.CampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                        if (campaignProfileData != null)
                        {
                            campaignProfileData.Hustlers_AdType = model.Hustlers_AdType;
                            campaignProfileData.Youth_AdType = model.Youth_AdType;
                            campaignProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                            campaignProfileData.Mass_AdType = model.Mass_AdType;
                            campaignProfileData.CountryId = model.CountryId;
                            db.SaveChanges();
                        }
                        else
                        {
                            CampaignProfilePreference profilePreference = new CampaignProfilePreference();
                            profilePreference.Hustlers_AdType = model.Hustlers_AdType;
                            profilePreference.Youth_AdType = model.Youth_AdType;
                            profilePreference.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                            profilePreference.Mass_AdType = model.Mass_AdType;
                            profilePreference.CountryId = model.CountryId;
                            db.CampaignProfilePreference.Add(profilePreference);
                            db.SaveChanges();
                            model.CampaignProfileId = profilePreference.CampaignProfileId;
                        }


                        int countryId = 0;
                        var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                        if (campaignProfile != null)
                        {
                            countryId = (int)campaignProfile.CountryId;
                        }

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            #region External Server
                            foreach (var item in ConnString)
                            {
                                UserMatchTableProcess obj = new UserMatchTableProcess();
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                using (SQLServerEntities)
                                {
                                    var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(SQLServerEntities, model.CountryId);
                                    var externalCampaignProfileData = SQLServerEntities.CampaignProfilePreference.Where(s => s.CampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                                    if (externalCampaignProfileData != null)
                                    {
                                        externalCampaignProfileData.Hustlers_AdType = model.Hustlers_AdType;
                                        externalCampaignProfileData.Youth_AdType = model.Youth_AdType;
                                        externalCampaignProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                        externalCampaignProfileData.Mass_AdType = model.Mass_AdType;
                                        externalCampaignProfileData.CountryId = externalServerCountryId;
                                        SQLServerEntities.SaveChanges();
                                    }
                                    else
                                    {
                                        CampaignProfilePreference profilePreference = new CampaignProfilePreference();
                                        profilePreference.Hustlers_AdType = model.Hustlers_AdType;
                                        profilePreference.Youth_AdType = model.Youth_AdType;
                                        profilePreference.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                        profilePreference.Mass_AdType = model.Mass_AdType;
                                        externalCampaignProfileData.CountryId = externalServerCountryId;
                                        SQLServerEntities.CampaignProfilePreference.Add(profilePreference);
                                        SQLServerEntities.SaveChanges();
                                        model.CampaignProfileId = profilePreference.CampaignProfileId;
                                    }
                                    var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                    if (campaigndetailFromOP != null)
                                    {
                                        obj.UpdateSkizaProfile(model, campaigndetailFromOP, SQLServerEntities);
                                        if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                        {
                                            PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                        }
                                    }

                                }
                            }

                            #endregion
                        }

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

        [Route("SaveMobile")]
        [HttpPost]
        public ActionResult SaveMobile(CampaignProfileMobileFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileMobileCommand command =
                        Mapper.Map<CampaignProfileMobileFormModel, CreateOrUpdateCampaignProfileMobileCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileMobileId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileMobileId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        int countryId = 0;
                        var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                        if (campaignProfile != null)
                        {
                            countryId = (int)campaignProfile.CountryId;
                        }

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetailFromOP != null)
                                {
                                    obj.UpdateUsage(model, campaigndetailFromOP, SQLServerEntities);
                                    if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                    {
                                        PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                                    }
                                }

                            }
                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileMobileId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [Route("UpdateMedia")]
        public ActionResult UpdateMedia(int AdvertId, int CampaignProfileId, int CampaignAdvertID)
        {
            CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
            _campaignAdvert.CampaignAdvertId = CampaignAdvertID;
            _campaignAdvert.AdvertId = AdvertId;
            _campaignAdvert.CampaignProfileId = CampaignProfileId;
            var CountryID = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == CampaignProfileId).CountryId;

            CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
            Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

            ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);
            //var ConnString = ConnectionString.GetConnectionString(CountryID);

            //if (!string.IsNullOrEmpty(ConnString))
            //{
            //    EFMVCDataContex db = new EFMVCDataContex(ConnString);
            //    var CampaignAuditData = db.CampaignAudits.Where(s => s.CampaignProfileId == CampaignProfileId).ToList();
            //    if (CampaignAuditData.Count() > 0)
            //    {
            //        db.CampaignAudits.RemoveRange(CampaignAuditData);
            //        db.SaveChanges();
            //    }

            //}

            if (campaignAdvertcommandResult.Success)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Route("UpdateStatus")]
        public ActionResult UpdateStatus(int id, int status, int NumberInBatch)
        {

            if (User.Identity.IsAuthenticated)
            {
                CampaignProfileFormModel CampaignProfileFormModel = new CampaignProfileFormModel();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);


                command.Status = status;
                command.NumberInBatch = NumberInBatch;
               
                var campaigndetails = _profileRepository.GetById(id);
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
                    command.NumberInBatch = campaigndetails.NumberInBatch;
                    command.CountryId = (int)campaigndetails.CountryId;
                    command.IsAdminApproval = true;
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
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

                        }
                    }

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

        [HttpPost]
        [Route("SaveTimeSettings")]
        public ActionResult SaveTimeSettings(CampaignProfileTimeSettingFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileTimeSettingCommand command =
                        Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                            model);

                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        using (var SQLServerEntities = new EFMVCDataContex())
                        {

                            // var GetCampaignMatchProfileID = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault().CampaignProfileId;
                            var GetCampaignProfileForTimeSettings = SQLServerEntities.CampaignProfileTimeSettings.Where(s => s.CampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileForTimeSettings != null)
                            {
                                //var GetCampaignProfileForTimeSettings = mySQLEntities.campaignprofiletimesettings.Where(s => s.CampaignProfileId == GetCampaignProfileTimeByID.CampaignProfileId).FirstOrDefault();


                                GetCampaignProfileForTimeSettings.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;

                                //GetCampaignProfileForTimeSettings.MSCampaignProfileId = GetCampaignMatchProfileID;
                                SQLServerEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignprofiletimesetting = new CampaignProfileTimeSetting();

                                campaignprofiletimesetting.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.CampaignProfileId = model.CampaignProfileId;
                                SQLServerEntities.CampaignProfileTimeSettings.Add(campaignprofiletimesetting);
                                SaveChanges(SQLServerEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileTimeSettingsId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                // Log.Error(ex.Message + " " + sb, ex);
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException

            }
        }
    }
}