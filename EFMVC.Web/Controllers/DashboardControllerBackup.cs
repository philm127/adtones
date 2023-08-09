using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.SearchClass;
using EFMVC.Web.ViewModels;
using EFMVC.ProvisioningModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using EFMVC.Data.Infrastructure;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class DashboardController : Controller
    {

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The _campaign advert repository
        /// </summary>
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="advertRepository">The advert repository.</param>
        public DashboardController(ICommandBus commandBus, ICampaignProfileRepository profileRepository,
                                  IUserRepository userRepository, IAdvertRepository advertRepository, ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository,
                                  ICampaignAuditRepository campaignAuditRepository, IClientRepository clientRepository, ICampaignAdvertRepository campaignAdvertRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _clientRepository = clientRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
            _campaignProfilePreferenceRepository = campaignProfilePreferenceRepository;
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
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
        private void FillAddCampaign(int UserId)
        {
            var CampaignDetails = _profileRepository.GetAll().Where(s => s.UserId == UserId).Select(top => new SelectListItem { Text = top.CampaignName, Value = top.CampaignProfileId.ToString() }).ToList();

            var AllCampaign = CampaignDetails.ToList();
            ViewBag.allCampaignList = AllCampaign;
        }
        public ActionResult GetCampaignData(int id)
        {
           
            FillCountryList();
            ViewBag.CampaignId = id;
            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();
            CampaignProfileMapping _mapping = new CampaignProfileMapping();
            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel();
            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel();
            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel();
            CampaignProfileAttitudeFormModel CampaignProfileAtt = new CampaignProfileAttitudeFormModel();
            CampaignProfileCinemaFormModel Cinemapro = new CampaignProfileCinemaFormModel();
            CampaignProfileInternetFormModel Internetpro = new CampaignProfileInternetFormModel();
            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel();
            CampaignProfilePressFormModel Presspro = new CampaignProfilePressFormModel();
            CampaignProfileProductsServiceFormModel ProductServicepro = new CampaignProfileProductsServiceFormModel();
            CampaignProfileRadioFormModel Radiopro = new CampaignProfileRadioFormModel();
            CampaignProfileTvFormModel Tvpro = new CampaignProfileTvFormModel();
            CampaignProfileTimeSettingFormModel Timingpro = new CampaignProfileTimeSettingFormModel();
            CampaignAuditFilter CampaignAuditFilter = new CampaignAuditFilter();
            CampaignDashboardChartResult CampaignDashboardChartResult = new CampaignDashboardChartResult();
            CampaignAuditFilter.CampaignProfileId = id;
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId && (x.Status != 5)) == 0)
                return RedirectToAction("Index");


            if (id != 0)
            {
                CampaignProfileGeographicModel = CampaignProfileGeographic(id, CampaignProfileGeographicModel);
                _mapping.CampaignProfileGeographicModel = CampaignProfileGeographicModel;

                CampaignProfileDemographicsmodel = CampaignProfileDemographic(id, CampaignProfileDemographicsmodel);
                _mapping.CampaignProfileDemographicsmodel = CampaignProfileDemographicsmodel;

                CampaignProfileAd = CampaignProfileAdvert(id, CampaignProfileAd);
                _mapping.CampaignProfileAd = CampaignProfileAd;
                CampaignProfileAtt = CampaignProfileAttitude(id, CampaignProfileAtt);
                _mapping.campaignProfileAttitude = CampaignProfileAtt;
                Cinemapro = Cinema(id, Cinemapro);
                _mapping.CampaignProfileCinemaFormModel = Cinemapro;
                Internetpro = Internet(id, Internetpro);
                _mapping.CampaignProfileInternetFormModel = Internetpro;
                Mobilepro = Mobile(id, Mobilepro);
                _mapping.CampaignProfileMobileFormModel = Mobilepro;
                Presspro = Press(id, Presspro);
                _mapping.CampaignProfilePressFormModel = Presspro;
                ProductServicepro = ProductsServices(id, ProductServicepro);
                _mapping.CampaignProfileProductsServiceFormModel = ProductServicepro;
                Radiopro = Radio(id, Radiopro);
                _mapping.CampaignProfileRadioFormModel = Radiopro;
                Tvpro = Tv(id, Tvpro);
                _mapping.CampaignProfileTvFormModel = Tvpro;
                Timingpro = Timing(id, Timingpro);
                _mapping.CampaignProfileTimeSettingFormModel = Timingpro;
                var model = GetEditData(id, efmvcUser.UserId);
                ViewBag.AdvertClientId = model.ClientId;
                _mapping.AdvertFormModel = advertFormModels();
                _mapping.CampaignAudit = campaignAudit(id);
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                ViewBag.ClientId = model.ClientId;
                ViewBag.CampaignProfileId = model.CampaignProfileId;
                if (model != null)
                    //fill chart data
                    CampaignDashboardChartResult = FillChartDataByCampaignId(model);
                _mapping.CampaignDashboardChartResult = CampaignDashboardChartResult;
                //return View(Tuple.Create(model, _mapping));

                return PartialView(Tuple.Create(model, _mapping));
            }
            else
            {
                ViewBag.ClientId = null;
                ViewBag.CampaignProfileId = null;
            }
            return View("Index");

        }
        public ActionResult Initialise()
        {
            Session["NewCampaignId"] = null;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            FillAddCampaign(efmvcUser.UserId);
            var viewModel = new CampaignProfileFormModel
            {
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            };
            viewModel.NumberInBatch = 1;
            return View(viewModel);


        }
        public ActionResult Save(CampaignProfileFormModel model, string hdnstatus)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                model.Status = (int)CampaignStatus.Waitingforadapproval;
                if (model.StartDate == null && model.EndDate != null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels =
                        Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign start date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate == null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels =
                        Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign end date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate != null)
                {
                    if (model.EndDate.Value.Date < model.StartDate.Value.Date)
                    {
                        var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                        IEnumerable<ClientModel> clientModels =
                            Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                        FillAddClient(clientModels);
                        TempData["Error"] = "EndDate must be greater than StartDate.";
                        return View("Initialise", model);

                    }
                }
                model.Active = true;
                model.TotalBudget = 0;






                model.CampaignProfileAttitudes = new Collection<CampaignProfileAttitudeFormModel>
                                                     {new CampaignProfileAttitudeFormModel()};

                CreateOrUpdateCampaignProfileCommand command =
                    Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(model);





                command.CampaignProfileAdverts =
                    Mapper.Map
                        <ICollection<CampaignProfileAdvertFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAdvertCommand>>(model.CampaignProfileAdverts ??
                                                                                     new Collection
                                                                                         <CampaignProfileAdvertFormModel
                                                                                         >
                                                                                         {
                                                                                             new CampaignProfileAdvertFormModel
                                                                                                 ()
                                                                                         });
                command.CampaignProfileAttitudes =
                    Mapper.Map
                        <ICollection<CampaignProfileAttitudeFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAttitudeCommand>>(model.CampaignProfileAttitudes);
                command.CampaignProfileCinemas =
                    Mapper.Map
                        <ICollection<CampaignProfileCinemaFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileCinemaCommand>>(model.CampaignProfileCinemas);
                command.CampaignProfileInternets =
                    Mapper.Map
                        <ICollection<CampaignProfileInternetFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileInternetCommand>>(model.CampaignProfileInternets);
                command.CampaignProfileMobiles =
                    Mapper.Map
                        <ICollection<CampaignProfileMobileFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileMobileCommand>>(model.CampaignProfileMobiles);
                command.CampaignProfilePresses =
                    Mapper.Map
                        <ICollection<CampaignProfilePressFormModel>,
                            ICollection<CreateOrUpdateCampaignProfilePressCommand>>(model.CampaignProfilePresses);
                command.CampaignProfileProductsServices =
                    Mapper.Map
                        <ICollection<CampaignProfileProductsServiceFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileProductsServiceCommand>>(
                                model.CampaignProfileProductsServices);
                command.CampaignProfileRadios =
                    Mapper.Map
                        <ICollection<CampaignProfileRadioFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileRadioCommand>>(model.CampaignProfileRadios);
                command.CampaignProfileTimeSettings =
                    Mapper.Map
                        <ICollection<CampaignProfileTimeSettingFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileTimeSettingCommand>>(
                                model.CampaignProfileTimeSettings);
                command.CampaignProfileTvs =
                    Mapper.Map
                        <ICollection<CampaignProfileTvFormModel>, ICollection<CreateOrUpdateCampaignProfileTvCommand>>(
                            model.CampaignProfileTvs);
                command.CampaignProfileDemographics =
                    Mapper.Map
                        <ICollection<CampaignProfileDemographicsFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileDemographicsCommand>>(
                                model.CampaignProfileDemographicsFormModels ??
                                new Collection<CampaignProfileDemographicsFormModel>
                                    {new CampaignProfileDemographicsFormModel()});

                command.UserId = efmvcUser.UserId;
                command.NumberInBatch = model.NumberInBatch;

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var campaignmatch = new campaignmatch();

                            campaignmatch.CampaignName = model.CampaignName;
                            campaignmatch.CampaignDescription = model.CampaignDescription;
                            campaignmatch.ClientId = model.ClientId;
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(model.MaxDailyBudget);
                            campaignmatch.TotalBudget = Convert.ToInt64(model.TotalBudget);
                            campaignmatch.MaxBid = Convert.ToInt32(model.MaxBid);
                            campaignmatch.StartDate = model.StartDate;
                            campaignmatch.EndDate = model.EndDate;
                            campaignmatch.Active = model.Active;
                            campaignmatch.UserId = efmvcUser.UserId;
                            campaignmatch.Status = model.Status;
                            campaignmatch.MSCampaignProfileId = result.Id;
                            campaignmatch.NumberInBatch = model.NumberInBatch;

                            mySQLEntities.campaignmatches.Add(campaignmatch);
                            SaveChanges(mySQLEntities);

                        }

                        TempData["campaignId"] = result.Id;
                        return RedirectToAction("Campaign", new { id = result.Id });
                    }
                }
            }
            //if fail
            if (model.CampaignProfileId == 0)
                return View("Initialise", model);

            return View("Initialise", model);
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

        public ActionResult Campaign(int id)
        {
            FillCountryList();
            Session["CampaignId"] = id;
            ViewBag.CampaignId = id;
            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();
            CampaignProfileMapping _mapping = new CampaignProfileMapping();
            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel();
            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel();
            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel();
            CampaignProfileAttitudeFormModel CampaignProfileAtt = new CampaignProfileAttitudeFormModel();
            CampaignProfileCinemaFormModel Cinemapro = new CampaignProfileCinemaFormModel();
            CampaignProfileInternetFormModel Internetpro = new CampaignProfileInternetFormModel();
            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel();
            CampaignProfilePressFormModel Presspro = new CampaignProfilePressFormModel();
            CampaignProfileProductsServiceFormModel ProductServicepro = new CampaignProfileProductsServiceFormModel();
            CampaignProfileRadioFormModel Radiopro = new CampaignProfileRadioFormModel();
            CampaignProfileTvFormModel Tvpro = new CampaignProfileTvFormModel();
            CampaignProfileTimeSettingFormModel Timingpro = new CampaignProfileTimeSettingFormModel();
            CampaignAuditFilter CampaignAuditFilter = new CampaignAuditFilter();
            CampaignDashboardChartResult CampaignDashboardChartResult = new CampaignDashboardChartResult();
            CampaignAuditFilter.CampaignProfileId = id;
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId && (x.Status != 5)) == 0)
                return RedirectToAction("Index");


            if (id != 0)
            {
                CampaignProfileGeographicModel = CampaignProfileGeographic(id, CampaignProfileGeographicModel);
                _mapping.CampaignProfileGeographicModel = CampaignProfileGeographicModel;

                CampaignProfileDemographicsmodel = CampaignProfileDemographic(id, CampaignProfileDemographicsmodel);
                _mapping.CampaignProfileDemographicsmodel = CampaignProfileDemographicsmodel;

                CampaignProfileAd = CampaignProfileAdvert(id, CampaignProfileAd);
                _mapping.CampaignProfileAd = CampaignProfileAd;
                CampaignProfileAtt = CampaignProfileAttitude(id, CampaignProfileAtt);
                _mapping.campaignProfileAttitude = CampaignProfileAtt;
                Cinemapro = Cinema(id, Cinemapro);
                _mapping.CampaignProfileCinemaFormModel = Cinemapro;
                Internetpro = Internet(id, Internetpro);
                _mapping.CampaignProfileInternetFormModel = Internetpro;
                Mobilepro = Mobile(id, Mobilepro);
                _mapping.CampaignProfileMobileFormModel = Mobilepro;
                Presspro = Press(id, Presspro);
                _mapping.CampaignProfilePressFormModel = Presspro;
                ProductServicepro = ProductsServices(id, ProductServicepro);
                _mapping.CampaignProfileProductsServiceFormModel = ProductServicepro;
                Radiopro = Radio(id, Radiopro);
                _mapping.CampaignProfileRadioFormModel = Radiopro;
                Tvpro = Tv(id, Tvpro);
                _mapping.CampaignProfileTvFormModel = Tvpro;
                Timingpro = Timing(id, Timingpro);
                _mapping.CampaignProfileTimeSettingFormModel = Timingpro;
                var model = GetEditData(id, efmvcUser.UserId);
                ViewBag.AdvertClientId = model.ClientId;
                _mapping.AdvertFormModel = advertFormModels();
                _mapping.CampaignAudit = campaignAudit(id);
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                ViewBag.ClientId = model.ClientId;
                ViewBag.CampaignProfileId = model.CampaignProfileId;
                if (model != null)
                    //fill chart data
                    CampaignDashboardChartResult = FillChartDataByCampaignId(model);
                _mapping.CampaignDashboardChartResult = CampaignDashboardChartResult;
                return View(Tuple.Create(model, _mapping));
            }
            else
            {
                ViewBag.ClientId = null;
                ViewBag.CampaignProfileId = null;
            }
            return View("Index");

        }
        private List<CampaignAuditResult> campaignAudit(int id)
        {

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
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            var advertname = string.Empty;
            var advertid = 0;
            var emailstatus = string.Empty;
            var smsstatus = string.Empty;
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId && (x.Status != 5)) > 0)
            {
                CampaignProfile profile = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id);
                totalcredit = profile.TotalCredit;
                IEnumerable<CampaignAuditFormModel> model =
                    Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits);
                foreach (var item in model.OrderByDescending(s => s.StartTime))
                {

                    playcost = playcost + item.BidValue;
                    emailcost = emailcost + item.EmailCost;
                    smscost = smscost + item.SMSCost;
                    var advertdetails = profile.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId);
                    if (advertdetails != null)
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
                    _audit.Add(new CampaignAuditResult { PlayID = item.CampaignAuditId, UserID = item.UserProfileId, StartDate = item.StartTime.ToString("MM/dd/yyyy hh:mm:ss"), EndDate = item.EndTime.ToString("MM/dd/yyyy hh:mm:ss"), LengthOfPlay = item.PlayLength.Seconds, AdvertName = advertname, AdvertId = advertid, Status = item.Status, PlayCost = RoundUp(item.BidValue, 2), SMS = smsstatus, SMSCost = item.SMSCost, Email = emailstatus, EmailCost = item.EmailCost, TotalCost = RoundUp(item.TotalCost, 2), DisplayStartDate = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss"), DisplayEndDate = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss") });
                }
                spendtodate = playcost + emailcost + smscost;
                ViewData["maxspendtodate"] = RoundUp(spendtodate, 2);
                ViewData["maxremainding"] = RoundUp((totalcredit - spendtodate), 0);
            }

            return _audit;
        }
        private IEnumerable<AdvertFormModel> advertFormModels()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            IEnumerable<Advert> adverts = _advertRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<AdvertFormModel> advertFormModels =
                Mapper.Map<IEnumerable<Advert>, IEnumerable<AdvertFormModel>>(adverts);
            return advertFormModels;
        }

        //[Route("UpdateMedia")]
        public ActionResult UpdateMedia(int AdvertId, int CampaignProfileId, int CampaignAdvertID)
        {
            CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
            _campaignAdvert.CampaignAdvertId = CampaignAdvertID;
            _campaignAdvert.AdvertId = AdvertId;
            _campaignAdvert.CampaignProfileId = CampaignProfileId;
            CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
            Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

            ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);
            if (campaignAdvertcommandResult.Success)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddMedia(int AdvertId, int CampaignProfileId)
        {
            if(CampaignProfileId == 0)
            {
                var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                CampaignProfileId = CampProfileId;
            }
            var GetCampaignAdvert = _campaignAdvertRepository.GetAll().Where(s => s.CampaignProfileId == CampaignProfileId).FirstOrDefault();

            if (GetCampaignAdvert == null) // Insert
            {
                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                _campaignAdvert.AdvertId = AdvertId;
                _campaignAdvert.CampaignProfileId = CampaignProfileId;
                CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);


                ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);
                if (campaignAdvertcommandResult.Success)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            else // Update
            {
                GetCampaignAdvert.AdvertId = AdvertId;
                GetCampaignAdvert.CampaignProfileId = CampaignProfileId;
                _campaignAdvertRepository.Update(GetCampaignAdvert);

                _unitOfWork.Commit();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

        }
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
                        Common.CampaignAuditStatus cstatus = (Common.CampaignAuditStatus)Convert.ToInt32(_filterCritearea.Status);
                        string auditstatus = cstatus.ToString();
                        campaignaudit = campaignaudit.Where(top => top.Status.ToLower() == auditstatus.ToLower()).ToList();
                    }
                }

                if (!String.IsNullOrEmpty(_filterCritearea.FromPlayCost) && !String.IsNullOrEmpty(_filterCritearea.ToPlayCost))
                {

                    campaignaudit = campaignaudit.Where(top => top.PlayCost >= Convert.ToInt32(_filterCritearea.FromPlayCost) && top.PlayCost <= Convert.ToInt32(_filterCritearea.ToPlayCost)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromTotalCost) && !String.IsNullOrEmpty(_filterCritearea.ToTotalCost))
                {

                    campaignaudit = campaignaudit.Where(top => top.TotalCost >= Convert.ToInt32(_filterCritearea.FromTotalCost) && top.TotalCost <= Convert.ToInt32(_filterCritearea.ToTotalCost)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.SMSStatus))
                {
                    if (_filterCritearea.SMSStatus != "0")
                    {
                        Common.CampaignAuditSMSStatus cstatus = (Common.CampaignAuditSMSStatus)Convert.ToInt32(_filterCritearea.SMSStatus);
                        string auditsmsstatus = cstatus.ToString();
                        campaignaudit = campaignaudit.Where(top => top.SMS.ToLower() == auditsmsstatus.ToLower()).ToList();
                    }
                }

                if (_filterCritearea.StartTime != null)
                {
                    var StartTime = Convert.ToDateTime(_filterCritearea.StartTime);
                    campaignaudit = campaignaudit.Where(top => top.StartDate.Contains(StartTime.ToString("MM/dd/yyyy"))).ToList();
                }

                if (_filterCritearea.EndTime != null)
                {
                    var EndTime = Convert.ToDateTime(_filterCritearea.EndTime);
                    campaignaudit = campaignaudit.Where(top => top.EndDate.Contains(EndTime.ToString("MM/dd/yyyy"))).ToList();
                }

            }
            return campaignaudit;
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

        private CampaignProfileAdvertFormModel CampaignProfileAdvert(int id, CampaignProfileAdvertFormModel Campaignad)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileAdvert =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileAdvert(campaignProfileAdvert);
                    if (status == false)
                    {
                        Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = campaignProfileAdvert.Id };
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
                    Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };

                }
            }
            else
            {
                Campaignad = new CampaignProfileAdvertFormModel { CampaignProfileId = id, CampaignProfileAdvertsId = 0 };
            }
            return Campaignad;
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
        private CampaignProfileDemographicsFormModel CampaignProfileDemographic(int id, CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

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
                        CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = campaignProfileDemographics.Id };
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

                    CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
                }
            }
            else
            {
                CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel { CampaignProfileId = id, CampaignProfileDemographicsId = 0 };
            }

            return CampaignProfileDemographicsmodel;
        }
        private CampaignProfileCinemaFormModel Cinema(int id, CampaignProfileCinemaFormModel Cinema)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileCinema =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(campaignProfileCinema.Cinema_Cinema))
                    {
                        Cinema = new CampaignProfileCinemaFormModel { CampaignProfileId = id, CampaignProfileCinemaId = campaignProfileCinema.Id };
                    }
                    else
                    {
                        Cinema = Mapper.Map<CampaignProfilePreference, CampaignProfileCinemaFormModel>(campaignProfileCinema);
                        Cinema.CampaignProfileCinemaId = campaignProfileCinema.Id;
                        Cinema.CampaignProfileId = id;
                    }
                }
                else
                {
                    Cinema = new CampaignProfileCinemaFormModel { CampaignProfileId = id };

                }
            }
            else
            {
                Cinema = new CampaignProfileCinemaFormModel { CampaignProfileId = id };
            }

            return Cinema;
        }
        public bool checkcampaignProfileInternet(CampaignProfilePreference campaignProfileInternet)
        {
            if (string.IsNullOrEmpty(campaignProfileInternet.Auctions_Internet) && string.IsNullOrEmpty(campaignProfileInternet.Shopping_Internet) && string.IsNullOrEmpty(campaignProfileInternet.SocialNetworking_Internet)
                && string.IsNullOrEmpty(campaignProfileInternet.Research_Internet) && string.IsNullOrEmpty(campaignProfileInternet.Video_Internet)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private CampaignProfileInternetFormModel Internet(int id, CampaignProfileInternetFormModel Internet)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                  campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileInternet =
                         campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileInternet(campaignProfileInternet);
                    if (status == false)
                    {
                        Internet = new CampaignProfileInternetFormModel { CampaignProfileId = id, CampaignProfileInternetId = campaignProfileInternet.Id };

                    }
                    else
                    {
                        Internet = Mapper.Map<CampaignProfilePreference, CampaignProfileInternetFormModel>(campaignProfileInternet);
                        Internet.CampaignProfileInternetId = campaignProfileInternet.Id;
                        Internet.CampaignProfileId = id;
                    }
                }
                else
                {
                    Internet = new CampaignProfileInternetFormModel { CampaignProfileId = id };
                }
            }
            else
            {
                Internet = new CampaignProfileInternetFormModel { CampaignProfileId = id };
            }

            return Internet;
        }
        private CampaignProfileMobileFormModel Mobile(int id, CampaignProfileMobileFormModel Mobile)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileMobile =
                        campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(campaignProfileMobile.ContractType_Mobile) && String.IsNullOrEmpty(campaignProfileMobile.Spend_Mobile))
                    {
                        Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id, CampaignProfileMobileId = campaignProfileMobile.Id };
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
                    Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id };

                }
            }
            else
            {
                Mobile = new CampaignProfileMobileFormModel { CampaignProfileId = id };
            }

            return Mobile;
        }
        public bool checkcampaignProfilePress(CampaignProfilePreference campaignProfilePress)
        {
            if (string.IsNullOrEmpty(campaignProfilePress.FreeNewpapers_Press) && string.IsNullOrEmpty(campaignProfilePress.Local_Press) && string.IsNullOrEmpty(campaignProfilePress.Magazines_Press)
                && string.IsNullOrEmpty(campaignProfilePress.National_Press)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private CampaignProfilePressFormModel Press(int id, CampaignProfilePressFormModel Press)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {

                    CampaignProfilePreference campaignProfilePress = campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfilePress(campaignProfilePress);
                    if (status == false)
                    {
                        Press = new CampaignProfilePressFormModel { CampaignProfileId = id, CampaignProfilePressId = campaignProfilePress.Id };
                    }
                    else
                    {
                        Press = Mapper.Map<CampaignProfilePreference, CampaignProfilePressFormModel>(campaignProfilePress);
                        Press.CampaignProfilePressId = campaignProfilePress.Id;
                        Press.CampaignProfileId = id;
                    }
                }
                else
                {
                    Press = new CampaignProfilePressFormModel { CampaignProfileId = id };
                }
            }
            else
            {
                Press = new CampaignProfilePressFormModel { CampaignProfileId = id };
            }

            return Press;
        }
        public bool checkcampaignProfileProductsServices(CampaignProfilePreference campaignProfileProductsServices)
        {
            if (string.IsNullOrEmpty(campaignProfileProductsServices.AlcoholicDrinks_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.AppliancesOtherHouseholdDurables_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.CommunicationsInternet_ProductsService)
                && string.IsNullOrEmpty(campaignProfileProductsServices.DIYGardening_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.ElectronicsOtherPersonalItems_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.FinancialServices_ProductsService)
                && string.IsNullOrEmpty(campaignProfileProductsServices.Food_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.HolidaysTravel_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.Householdproducts_ProductsService)
                && string.IsNullOrEmpty(campaignProfileProductsServices.Motoring_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.NonAlcoholicDrinks_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.PetsPetFood_ProductsService)
                && string.IsNullOrEmpty(campaignProfileProductsServices.PharmaceuticalChemistsProducts_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.ShoppingRetailClothing_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.SportsLeisure_ProductsService)
                && string.IsNullOrEmpty(campaignProfileProductsServices.SweetSaltySnacks_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.TobaccoProducts_ProductsService) && string.IsNullOrEmpty(campaignProfileProductsServices.ToiletriesCosmetics_ProductsService)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private CampaignProfileProductsServiceFormModel ProductsServices(int id, CampaignProfileProductsServiceFormModel ProductsServices)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                   campaignProfile.CampaignProfilePreferences.Count != 0)
                {

                    CampaignProfilePreference campaignProfileProductsServices =
                         campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileProductsServices(campaignProfileProductsServices);
                    if (status == false)
                    {
                        ProductsServices = new CampaignProfileProductsServiceFormModel { CampaignProfileId = id, CampaignProfileProductsServicesId = campaignProfileProductsServices.Id };
                    }
                    else
                    {
                        ProductsServices =
                              Mapper.Map<CampaignProfilePreference, CampaignProfileProductsServiceFormModel>(
                                  campaignProfileProductsServices);
                        ProductsServices.CampaignProfileProductsServicesId = campaignProfileProductsServices.Id;
                        ProductsServices.CampaignProfileId = id;
                    }
                }
                else
                {
                    ProductsServices = new CampaignProfileProductsServiceFormModel { CampaignProfileId = id };
                }
            }
            else
            {
                ProductsServices = new CampaignProfileProductsServiceFormModel { CampaignProfileId = id };
            }

            return ProductsServices;
        }
        public bool checkcampaignProfileRadio(CampaignProfilePreference campaignProfileRadio)
        {
            if (string.IsNullOrEmpty(campaignProfileRadio.Local_Radio) && string.IsNullOrEmpty(campaignProfileRadio.Music_Radio) && string.IsNullOrEmpty(campaignProfileRadio.National_Radio)
                && string.IsNullOrEmpty(campaignProfileRadio.Sport_Radio) && string.IsNullOrEmpty(campaignProfileRadio.Talk_Radio)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private CampaignProfileRadioFormModel Radio(int id, CampaignProfileRadioFormModel Radio)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {

                    CampaignProfilePreference campaignProfileRadio = campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileRadio(campaignProfileRadio);
                    if (status == false)
                    {
                        Radio = new CampaignProfileRadioFormModel { CampaignProfileId = id, CampaignProfileRadioId = campaignProfileRadio.Id };
                    }
                    else
                    {
                        Radio =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileRadioFormModel>(campaignProfileRadio);
                        Radio.CampaignProfileRadioId = campaignProfileRadio.Id;
                        Radio.CampaignProfileId = id;
                    }
                }
                else
                {
                    Radio = new CampaignProfileRadioFormModel { CampaignProfileId = id };
                }
            }
            else
            {
                Radio = new CampaignProfileRadioFormModel { CampaignProfileId = id };
            }

            return Radio;
        }
        public bool checkcampaignProfileTv(CampaignProfilePreference campaignProfileTv)
        {
            if (string.IsNullOrEmpty(campaignProfileTv.Cable_TV) && string.IsNullOrEmpty(campaignProfileTv.Internet_TV) && string.IsNullOrEmpty(campaignProfileTv.Satallite_TV)
                && string.IsNullOrEmpty(campaignProfileTv.Terrestrial_TV)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private CampaignProfileTvFormModel Tv(int id, CampaignProfileTvFormModel Tv)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null && campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference campaignProfileTv = campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkcampaignProfileTv(campaignProfileTv);
                    if (status == false)
                    {
                        Tv = new CampaignProfileTvFormModel { CampaignProfileId = id, CampaignProfileTvId = campaignProfileTv.Id };
                    }
                    else
                    {
                        Tv =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileTvFormModel>(campaignProfileTv);
                        Tv.CampaignProfileTvId = campaignProfileTv.Id;
                        Tv.CampaignProfileId = id;
                    }
                }
                else
                {
                    Tv = new CampaignProfileTvFormModel { CampaignProfileId = id };
                }
            }
            else
            {
                Tv = new CampaignProfileTvFormModel { CampaignProfileId = id };
            }

            return Tv;
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
        public IList<TimeOfDay> ConvertTimesArrayToList(string[] selectedTimes)
        {
            return selectedTimes.Select(time => new TimeOfDay { Id = time, Name = time, IsSelected = true }).ToList();
        }
        public bool checkCampaignProfileAttitude(CampaignProfilePreference CampaignProfileAttitude)
        {
            if (string.IsNullOrEmpty(CampaignProfileAttitude.Environment_Attitude) && string.IsNullOrEmpty(CampaignProfileAttitude.Fashion_Attitude) && string.IsNullOrEmpty(CampaignProfileAttitude.Fitness_Attitude)
                && string.IsNullOrEmpty(CampaignProfileAttitude.FinancialStabiity_Attitude) && string.IsNullOrEmpty(CampaignProfileAttitude.GoingOut_Attitude) && string.IsNullOrEmpty(CampaignProfileAttitude.Holidays_Attitude)
                && string.IsNullOrEmpty(CampaignProfileAttitude.Music_Attitude) && string.IsNullOrEmpty(CampaignProfileAttitude.Religion_Attitude)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool checkCampaignProfileGeographic(CampaignProfilePreference CampaignProfileGeograph)
        {
            if (string.IsNullOrEmpty(CampaignProfileGeograph.Postcode) && CampaignProfileGeograph.CountryId != 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private CampaignProfileGeographicFormModel CampaignProfileGeographic(int id, CampaignProfileGeographicFormModel CampaignProfileGeo)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                   campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference CampaignProfileGeograph =
                       campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkCampaignProfileGeographic(CampaignProfileGeograph);
                    if (status == false)
                    {
                        CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id, CampaignProfileGeographicId = CampaignProfileGeograph.Id };
                    }
                    else
                    {
                        CampaignProfileGeo = Mapper.Map<CampaignProfilePreference, CampaignProfileGeographicFormModel>(CampaignProfileGeograph);
                        CampaignProfileGeo.CampaignProfileGeographicId = CampaignProfileGeograph.Id;
                    }
                }
                else
                {
                    CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id };

                }
            }
            else
            {
                CampaignProfileGeo = new CampaignProfileGeographicFormModel { CampaignProfileId = id };
            }

            return CampaignProfileGeo;
        }

        private CampaignProfileAttitudeFormModel CampaignProfileAttitude(int id, CampaignProfileAttitudeFormModel CampaignProfileAt)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePreferences != null &&
                   campaignProfile.CampaignProfilePreferences.Count != 0)
                {
                    CampaignProfilePreference CampaignProfileAttitude =
                       campaignProfile.CampaignProfilePreferences.FirstOrDefault();
                    bool status = checkCampaignProfileAttitude(CampaignProfileAttitude);
                    if (status == false)
                    {
                        CampaignProfileAt = new CampaignProfileAttitudeFormModel { CampaignProfileId = id, CampaignProfileAttitudeId = CampaignProfileAttitude.Id };
                    }
                    else
                    {
                        CampaignProfileAt = Mapper.Map<CampaignProfilePreference, CampaignProfileAttitudeFormModel>(CampaignProfileAttitude);
                        CampaignProfileAt.CampaignProfileAttitudeId = CampaignProfileAttitude.Id;
                    }
                }
                else
                {
                    CampaignProfileAt = new CampaignProfileAttitudeFormModel { CampaignProfileId = id };

                }
            }
            else
            {
                CampaignProfileAt = new CampaignProfileAttitudeFormModel { CampaignProfileId = id };
            }

            return CampaignProfileAt;
        }
        private CampaignProfileFormModel GetEditData(int id, int userId)
        {
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == userId && (x.Status != 5)) == 0)
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


            int currentMonthPlayCount =
                model.CampaignAudits.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd);

            int previousMonthPlayCount =
                model.CampaignAudits.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd);

            int previousMonthSMSCount =
                model.CampaignAudits.Count(
                    x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.SMS == "1");

            int previousMonthEmailCount =
                model.CampaignAudits.Count(
                    x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.Email == "1");

            int currentMonthSMSCount =
                model.CampaignAudits.Count(
                    x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.SMS == "1");

            int currentMonthEmailCount =
                model.CampaignAudits.Count(
                    x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.Email == "1");

            int totalPlayCount = model.CampaignAudits.Count();
            int totalSMSCount = model.CampaignAudits.Count(x => x.SMS == "1");
            int totalEmailCount = model.CampaignAudits.Count(x => x.Email == "1");

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
            ViewData.Add("maxremainding", campaignProfile.TotalCredit - campaignProfile.SpendToDate);
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

        //
        // GET: /Dashboard/
        //Pass Multiple Model to view

        public void FillClientDropdown(int? clientId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var clientdetails = _clientRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && (top.Status == 1 || top.Status == 2)).Select(top => new
            {
                Name = top.Name,
                Id = top.Id.ToString(),

            }).ToList();
            if (clientId != null)
            {
                ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name", new[] { clientId });
            }
            else
            {
                ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
            }
            if (TempData["DashboardClientId"] != null)
            {
                var chartClientId = (int[])(TempData["DashboardClientId"]);
                ViewBag.chartclient = new MultiSelectList(clientdetails, "Id", "Name", chartClientId);
            }
            else
            {
                ViewBag.chartclient = new MultiSelectList(clientdetails, "Id", "Name");
            }

            //ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name", new[] {1});

            IEnumerable<Common.CampaignStatus> actionTypes = Enum.GetValues(typeof(Common.CampaignStatus))
                                                     .Cast<Common.CampaignStatus>();
            var compaignStatus = (from action in actionTypes
                                  select new
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            ViewBag.compaignStatus = new MultiSelectList(compaignStatus, "Value", "Text", new[] { 1, 2, 3, 4 });

        }
        public void FillAdvert()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var advert = (from action in _advertRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && (top.Status != 4 && top.Status != 5))
                          select new
                          {
                              AdvertName = action.AdvertName,
                              AdvertId = action.AdvertId
                          }).ToList();


            ViewBag.advert = new MultiSelectList(advert, "AdvertId", "AdvertName");


        }
        public void FillCampaignDropdown(IEnumerable<CampaignProfileFormModel> campaigndetails)
        {
            if (campaigndetails.Count() > 0)
            {


                var campaign = (from action in campaigndetails
                                select new
                                {
                                    CampaignName = action.CampaignName,
                                    CampaignProfileId = action.CampaignProfileId
                                }).ToList();
                TempData["campaignDetails"] = campaign;
                ViewBag.compaign = new MultiSelectList(campaign, "CampaignProfileId", "CampaignName");


            }
        }
        public ActionResult Index(int? clientid, int? advertid)
        {
            Session["NewCampaignId"] = null;
            List<CampaignProfileResult> _result = new List<CampaignProfileResult>();
            FillClientDropdown(clientid);
            FillAdvert();
            FilterCritearea _filterCritearea = new FilterCritearea();
            if (clientid != null)
            {
                _filterCritearea.ClientId = Convert.ToString(clientid);
            }
            if (advertid != null)
            {
                _filterCritearea.AdvertId = Convert.ToString(advertid);
            }
            _filterCritearea.Fromdate = DateTime.Now;
            _filterCritearea.Todate = DateTime.Now;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);
            IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status != 5));
            IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);
            //Fill ChartData
            CampaignDashboardChartResult _CampaignDashboardChartResult = FillChartData(campaignProfiles, null);
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);
            FillCampaignDropdown(campaignProfileFormModels);
            if (clientid != null)
            {
                campaignProfileFormModels = campaignProfileFormModels.Where(top => top.ClientId == clientid);

            }

            if (campaignProfileFormModels.Count() > 0)
            {
                int[] CampaingStatusId = new int[] { 1, 2, 3, 4 };
                campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaingStatusId.Contains(top.Status));
                CampainResult(_result, campaignProfileFormModels, null, null, null, null);
                if (advertid != null)
                {
                    _result = _result.Where(top => top.AdvertId == advertid).ToList();
                }
                TempData["compainresult"] = _result;
            }

            ViewData["User"] = userFormModel;
            ViewBag.campaignProfileFormModels = campaignProfileFormModels;
            return View(Tuple.Create(_result, _filterCritearea, _CampaignDashboardChartResult));

        }

        private void CampainResult(List<CampaignProfileResult> _result, IEnumerable<CampaignProfileFormModel> campaignProfileFormModels, int[] CampaignClientId, int[] CampaignAdvertId, int[] CampaignProfileId, int[] CampaingStatusId)
        {

            foreach (var item in campaignProfileFormModels)
            {
                //calculate average bid that has status Played.
                var totalbidval = 0;
                var totalspend = 0;
                var finaltotalplays = item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count();
                var bidresult = item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000);
                totalbidval = Convert.ToInt32(bidresult.Sum(top => top.BidValue));
                if (totalbidval > 0)
                    totalbidval = totalbidval / bidresult.Count();
                //cauculate total  spend that has status Played.
                var spendresult = item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000);
                totalspend = Convert.ToInt32(bidresult.Sum(top => top.TotalCost));
                if (totalspend > 0)
                    totalspend = totalspend / spendresult.Count();
                totalspend = totalspend * spendresult.Count();
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
                _result.Add(new CampaignProfileResult { Active = item.Active, AvailableCredit = item.AvailableCredit, Adverts = item.Adverts, AverageDailyPlays = item.AverageDailyPlays, CampaignAdverts = item.CampaignAdverts, CampaignAudits = item.CampaignAudits, CampaignDescription = item.CampaignDescription, CampaignName = item.CampaignName, CampaignProfileAdverts = item.CampaignProfileAdverts, CampaignProfileAttitudes = item.CampaignProfileAttitudes, CampaignProfileCinemas = item.CampaignProfileCinemas, CampaignProfileDateSettings = item.CampaignProfileDateSettings, CampaignProfileDemographicsFormModels = item.CampaignProfileDemographicsFormModels, CampaignProfileId = item.CampaignProfileId, CampaignProfileInternets = item.CampaignProfileInternets, CampaignProfileMobiles = item.CampaignProfileMobiles, CampaignProfilePresses = item.CampaignProfilePresses, CampaignProfileProductsServices = item.CampaignProfileProductsServices, CampaignProfileRadios = item.CampaignProfileRadios, CampaignProfileTimeSettings = item.CampaignProfileTimeSettings, CampaignProfileTvs = item.CampaignProfileTvs, CancelledCurrentMonth = item.CancelledCurrentMonth, CancelledLastMonth = item.CancelledLastMonth, CancelledToDate = item.CancelledToDate, Client = item.Client, ClientId = item.ClientId, CreatedDateTime = item.CreatedDateTime, EmailBody = item.EmailBody, EmailsCurrentMonth = item.EmailsCurrentMonth, EmailsDelievered = item.EmailsDelievered, EmailSenderAddress = item.EmailSenderAddress, EmailsLastMonth = item.EmailsLastMonth, EmailSubject = item.EmailSubject, EmailToDate = item.EmailToDate, MaxBid = item.MaxBid, MaxDailyBudget = item.MaxDailyBudget, NumberOfPlays = item.NumberOfPlays, PlaysCurrentMonth = item.PlaysCurrentMonth, PlaysLastMonth = item.PlaysLastMonth, PlaysToDate = item.PlaysToDate, SmsBody = item.SmsBody, SmsCurrentMonth = item.SmsCurrentMonth, SmsLastMonth = item.SmsLastMonth, SmsOriginator = item.SmsOriginator, SmsRequests = item.SmsRequests, SmsToDate = item.SmsToDate, Status = item.Status, TotalBudget = item.TotalBudget, UpdatedDateTime = item.UpdatedDateTime, UserId = item.UserId, totalaveragebid = totalbidval, totalspend = totalspend, finaltotalplays = finaltotalplays, advertname = advertname, AdvertId = advertId, FundsAvailable = Convert.ToDecimal(item.TotalBudget) - totalspend });

            }

        }

        public List<CampaignProfileResult> getcampaignResult(List<CampaignProfileResult> campaignProfileFormModels, FilterCritearea _filterCritearea, int[] CampaignClientId, int[] CampaignAdvertId, int[] CampaignProfileId, int[] CampaingStatusId)
        {
            if (campaignProfileFormModels != null && _filterCritearea != null)
            {
                if (CampaignClientId != null)
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaignClientId.Contains(top.ClientId)).ToList();

                }
                if (CampaignAdvertId != null)
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaignAdvertId.Contains(top.AdvertId)).ToList();

                }
                if (CampaignProfileId != null)
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaignProfileId.Contains(top.CampaignProfileId)).ToList();

                }

                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => top.CreatedDateTime.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDateTime.Date <= _filterCritearea.Todate.Value.Date).ToList();
                }
                if (CampaingStatusId != null)
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaingStatusId.Contains(top.Status)).ToList();

                }

                if (!String.IsNullOrEmpty(_filterCritearea.FromPlays) && !String.IsNullOrEmpty(_filterCritearea.ToPlays))
                {

                    campaignProfileFormModels = campaignProfileFormModels.Where(top => top.finaltotalplays >= Convert.ToInt32(_filterCritearea.FromPlays) && top.finaltotalplays <= Convert.ToInt32(_filterCritearea.ToPlays)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromSpend) && !String.IsNullOrEmpty(_filterCritearea.ToSpend))
                {

                    campaignProfileFormModels = campaignProfileFormModels.Where(top => top.totalspend >= Convert.ToInt32(_filterCritearea.FromSpend) && top.totalspend <= Convert.ToInt32(_filterCritearea.ToSpend)).ToList();


                }
                if (!String.IsNullOrEmpty(_filterCritearea.FromAvgbid) && !String.IsNullOrEmpty(_filterCritearea.ToAvgbid))
                {
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => top.totalaveragebid >= Convert.ToInt32(_filterCritearea.FromAvgbid) && top.totalaveragebid <= Convert.ToInt32(_filterCritearea.ToAvgbid)).ToList();


                }

            }
            return campaignProfileFormModels;
        }
        [HttpPost]
        public ActionResult SearchCampain([Bind(Prefix = "Item2")]FilterCritearea _filterCritearea, int[] CampaignClientId, int[] CampaignAdvertId, int[] CampaignProfileId, int[] CampaingStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (_filterCritearea == null)
                {
                    CampaingStatusId = new int[] { 2 };
                    List<CampaignProfileResult> _result = new List<CampaignProfileResult>();
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                    User user = _userRepository.GetById(efmvcUser.UserId);
                    IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId && x.Status != 5);
                    IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                        Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaingStatusId.Contains(top.Status));
                    if (campaignProfileFormModels.Count() > 0)
                    {
                        campaignProfileFormModels = campaignProfileFormModels.OrderByDescending(top => top.CreatedDateTime);
                        CampainResult(_result, campaignProfileFormModels, CampaignClientId, CampaignAdvertId, CampaignProfileId, CampaingStatusId);
                    }
                    return PartialView("_CampaignList", _result);
                }
                else
                {
                    List<CampaignProfileResult> _result = new List<CampaignProfileResult>();
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                    User user = _userRepository.GetById(efmvcUser.UserId);
                    IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                        Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);

                    if (campaignProfileFormModels.Count() > 0)
                    {
                        campaignProfileFormModels = campaignProfileFormModels.OrderByDescending(top => top.CreatedDateTime);
                        CampainResult(_result, campaignProfileFormModels, CampaignClientId, CampaignAdvertId, CampaignProfileId, CampaingStatusId);
                    }
                    var result = getcampaignResult(_result, _filterCritearea, CampaignClientId, CampaignAdvertId, CampaignProfileId, CampaingStatusId);
                    return PartialView("_CampaignList", result);
                }

            }
            else
            {
                return PartialView("_CampaignList", "notauthorise");
            }
        }

        [HttpPost]
        public ActionResult UpdateCampaignInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    CreateOrUpdateCampaignProfileCommand command =
                      Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                    command.UserId = efmvcUser.UserId;

                    command.CampaignDescription = CampaignProfileFormModel.CampaignDescription;
                    command.CampaignName = CampaignProfileFormModel.CampaignName;
                    command.ClientId = CampaignProfileFormModel.ClientId;
                    command.StartDate = CampaignProfileFormModel.StartDate;
                    command.EndDate = CampaignProfileFormModel.EndDate;



                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();
                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            campaignmatch.CampaignName = CampaignProfileFormModel.CampaignName;
                            campaignmatch.CampaignDescription = CampaignProfileFormModel.CampaignDescription;
                            campaignmatch.ClientId = CampaignProfileFormModel.ClientId;
                            campaignmatch.StartDate = CampaignProfileFormModel.StartDate;
                            campaignmatch.EndDate = CampaignProfileFormModel.EndDate;
                            campaignmatch.UserId = efmvcUser.UserId;
                            campaignmatch.NumberInBatch = CampaignProfileFormModel.NumberInBatch;
                            campaignmatch.MSCampaignProfileId = CampaignProfileFormModel.CampaignProfileId;
                            mySQLEntities.SaveChanges();
                        }
                    }


                    var campaigndetails = _profileRepository.GetById(CampaignProfileFormModel.CampaignProfileId);
                    if (campaigndetails != null)
                    {

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


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();

                            if (GetCampaignProfileById != null)
                            {
                                var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                                campaignmatch.Status = campaigndetails.Status;
                                campaignmatch.Active = campaigndetails.Active;
                                campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                                campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                                campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                                campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                                campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                                campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                                campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                                campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                                campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                                campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                                campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                                campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                                campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                                campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                                campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                                campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                                campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                                campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                                campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                                campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                                campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                                campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                                campaignmatch.UserId = campaigndetails.UserId;
                                campaignmatch.EmailBody = campaigndetails.EmailBody;
                                campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                                campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                                campaignmatch.SmsBody = campaigndetails.SmsBody;
                                campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;

                                campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                                campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                                campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;

                                mySQLEntities.SaveChanges();
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
            catch (Exception)
            {

                return Json("fail");
            }
        }
        public ActionResult UpdateBudgetInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {

                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                command.UserId = efmvcUser.UserId;

                command.MaxBid = CampaignProfileFormModel.MaxBid;
                command.MaxMonthBudget = CampaignProfileFormModel.MaxMonthBudget;
                command.MaxWeeklyBudget = CampaignProfileFormModel.MaxWeeklyBudget;
                command.MaxHourlyBudget = CampaignProfileFormModel.MaxHourlyBudget;
                command.MaxDailyBudget = CampaignProfileFormModel.MaxDailyBudget;


                using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                {
                    var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();
                    if (GetCampaignProfileById != null)
                    {
                        var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                        campaignmatch.MaxBid = Convert.ToInt32(CampaignProfileFormModel.MaxBid);
                        campaignmatch.MaxMonthBudget = Convert.ToInt64(CampaignProfileFormModel.MaxMonthBudget);
                        campaignmatch.MaxWeeklyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxWeeklyBudget);
                        campaignmatch.MaxHourlyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxHourlyBudget);
                        campaignmatch.MaxDailyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxDailyBudget);
                        campaignmatch.UserId = efmvcUser.UserId;
                        campaignmatch.MSCampaignProfileId = CampaignProfileFormModel.CampaignProfileId;
                        mySQLEntities.SaveChanges();
                    }
                }

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


                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();

                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            campaignmatch.StartDate = campaigndetails.StartDate;
                            campaignmatch.EndDate = campaigndetails.EndDate;
                            campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                            campaignmatch.CampaignName = campaigndetails.CampaignName;
                            campaignmatch.ClientId = campaigndetails.ClientId;
                            campaignmatch.Status = campaigndetails.Status;

                            campaignmatch.Active = campaigndetails.Active;
                            campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                            campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                            campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                            campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                            campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                            campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                            campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                            campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;

                            campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                            campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                            campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                            campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                            campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                            campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                            campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                            campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                            campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.EmailBody = campaigndetails.EmailBody;
                            campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                            campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                            campaignmatch.SmsBody = campaigndetails.SmsBody;
                            campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;

                            campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                            campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                            campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;
                            campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;

                            mySQLEntities.SaveChanges();
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

        public ActionResult UpdateCommunicationInfo(CampaignProfileFormModel CampaignProfileFormModel, HttpPostedFileBase emailfile, HttpPostedFileBase smsfile)
        {
            try
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

                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();

                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                            campaignmatch.EMAIL_MESSAGE = CampaignProfileFormModel.EmailBody;
                            campaignmatch.SMS_MESSAGE = CampaignProfileFormModel.SmsBody;
                            campaignmatch.ORIGINATOR = CampaignProfileFormModel.SmsOriginator;

                            campaignmatch.EmailBody = CampaignProfileFormModel.EmailBody;
                            campaignmatch.EmailSenderAddress = CampaignProfileFormModel.EmailSenderAddress;
                            campaignmatch.EmailSubject = CampaignProfileFormModel.EmailSubject;
                            campaignmatch.SmsBody = CampaignProfileFormModel.SmsBody;
                            campaignmatch.SmsOriginator = CampaignProfileFormModel.SmsOriginator;

                            campaignmatch.EmailFileLocation = command.EmailFileLocation;
                            campaignmatch.SMSFileLocation = command.SMSFileLocation;

                            campaignmatch.StartDate = campaigndetails.StartDate;
                            campaignmatch.EndDate = campaigndetails.EndDate;
                            campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                            campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                            campaignmatch.CampaignName = campaigndetails.CampaignName;
                            campaignmatch.ClientId = campaigndetails.ClientId;
                            campaignmatch.Status = campaigndetails.Status;

                            campaignmatch.Active = campaigndetails.Active;
                            campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                            campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                            campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                            campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                            campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                            campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                            campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                            campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;

                            campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                            campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                            campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                            campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                            campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                            campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                            campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                            campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                            campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;
                            mySQLEntities.SaveChanges();
                        }
                    }


                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        TempData["commusuccess"] = "Record updated successfully;";
                        return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
                    }
                }



                return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });

            }
            catch (Exception ex)
            {
                TempData["commuerror"] = ex.InnerException.Message;
                return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
            }
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status, int NumberInBatch)
        {

            if (User.Identity.IsAuthenticated)
            {
                CampaignProfileFormModel CampaignProfileFormModel = new CampaignProfileFormModel();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);


                command.Status = status;
                command.NumberInBatch = NumberInBatch;
                using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                {
                    var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == id).FirstOrDefault();
                    if (GetCampaignProfileById != null)
                    {
                        var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                        campaignmatch.Status = status;
                        mySQLEntities.SaveChanges();
                    }
                }

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

                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == id).FirstOrDefault();

                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            campaignmatch.StartDate = campaigndetails.StartDate;
                            campaignmatch.EndDate = campaigndetails.EndDate;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                            campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                            campaignmatch.CampaignName = campaigndetails.CampaignName;
                            campaignmatch.ClientId = campaigndetails.ClientId;
                            campaignmatch.Active = campaigndetails.Active;
                            campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                            campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                            campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                            campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                            campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                            campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                            campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                            campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                            campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                            campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                            campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                            campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                            campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                            campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                            campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                            campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                            campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.EmailBody = campaigndetails.EmailBody;
                            campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                            campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                            campaignmatch.SmsBody = campaigndetails.SmsBody;
                            campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;

                            campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                            campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                            campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;
                            campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;

                            mySQLEntities.SaveChanges();
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

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {



                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForMobile = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                                GetCampaignProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();
                                campaignmatch.ContractType_Mobile = model.ContractType_Mobile;
                                campaignmatch.Spend_Mobile = model.Spend_Mobile;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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
        [HttpPost]
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
                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {

                            var GetCampaignMatchProfileID = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault().CampaignProfileId;
                            var GetCampaignProfileTimeByID = mySQLEntities.campaignprofiletimesettings.Where(s => s.MSCampaignProfileId == GetCampaignMatchProfileID).FirstOrDefault();
                            if (GetCampaignProfileTimeByID != null)
                            {
                                var GetCampaignProfileForTimeSettings = mySQLEntities.campaignprofiletimesettings.Where(s => s.CampaignProfileId == GetCampaignProfileTimeByID.CampaignProfileId).FirstOrDefault();


                                GetCampaignProfileForTimeSettings.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                                // GetCampaignProfileForTimeSettings.MSCampaignProfileId = model.CampaignProfileId;
                                GetCampaignProfileForTimeSettings.MSCampaignProfileId = GetCampaignMatchProfileID;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignprofiletimesetting = new campaignprofiletimesetting();

                                campaignprofiletimesetting.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.MSCampaignProfileId = GetCampaignMatchProfileID;
                                mySQLEntities.campaignprofiletimesettings.Add(campaignprofiletimesetting);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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
        [HttpPost]
        public ActionResult SaveTv(CampaignProfileTvFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileTvCommand command =
                        Mapper.Map<CampaignProfileTvFormModel, CreateOrUpdateCampaignProfileTvCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileTvId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileTvId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForTv = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForTv.Satallite_TV = model.Satallite_TV;
                                GetCampaignProfileForTv.Cable_TV = model.Cable_TV;
                                GetCampaignProfileForTv.Terrestrial_TV = model.Terrestrial_TV;
                                GetCampaignProfileForTv.Internet_TV = model.Internet_TV;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Satallite_TV = model.Satallite_TV;
                                campaignmatch.Cable_TV = model.Cable_TV;
                                campaignmatch.Terrestrial_TV = model.Terrestrial_TV;
                                campaignmatch.Internet_TV = model.Internet_TV;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileTvId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SaveRadio(CampaignProfileRadioFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileRadioCommand command =
                        Mapper.Map<CampaignProfileRadioFormModel, CreateOrUpdateCampaignProfileRadioCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileRadioId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileRadioId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForRadio = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForRadio.National_Radio = model.National_Radio;
                                GetCampaignProfileForRadio.Local_Radio = model.Local_Radio;
                                GetCampaignProfileForRadio.Music_Radio = model.Music_Radio;
                                GetCampaignProfileForRadio.Sport_Radio = model.Sport_Radio;
                                GetCampaignProfileForRadio.Talk_Radio = model.Talk_Radio;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.National_Radio = model.National_Radio;
                                campaignmatch.Local_Radio = model.Local_Radio;
                                campaignmatch.Music_Radio = model.Music_Radio;
                                campaignmatch.Sport_Radio = model.Sport_Radio;
                                campaignmatch.Talk_Radio = model.Talk_Radio;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileRadioId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SaveProductsServices(CampaignProfileProductsServiceFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileProductsServiceCommand command =
                        Mapper.Map
                            <CampaignProfileProductsServiceFormModel, CreateOrUpdateCampaignProfileProductsServiceCommand>(
                                model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileProductsServicesId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileProductsServicesId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForProductsServices = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForProductsServices.Food_ProductsService = model.Food_ProductsService;
                                GetCampaignProfileForProductsServices.SweetSaltySnacks_ProductsService = model.SweetSaltySnacks_ProductsService;
                                GetCampaignProfileForProductsServices.AlcoholicDrinks_ProductsService = model.AlcoholicDrinks_ProductsService;
                                GetCampaignProfileForProductsServices.NonAlcoholicDrinks_ProductsService = model.NonAlcoholicDrinks_ProductsService;
                                GetCampaignProfileForProductsServices.Householdproducts_ProductsService = model.Householdproducts_ProductsService;
                                GetCampaignProfileForProductsServices.ToiletriesCosmetics_ProductsService = model.ToiletriesCosmetics_ProductsService;
                                GetCampaignProfileForProductsServices.PharmaceuticalChemistsProducts_ProductsService = model.PharmaceuticalChemistsProducts_ProductsService;
                                GetCampaignProfileForProductsServices.TobaccoProducts_ProductsService = model.TobaccoProducts_ProductsService;
                                GetCampaignProfileForProductsServices.PetsPetFood_ProductsService = model.PetsPetFood_ProductsService;
                                GetCampaignProfileForProductsServices.ShoppingRetailClothing_ProductsService = model.ShoppingRetailClothing_ProductsService;
                                GetCampaignProfileForProductsServices.DIYGardening_ProductsService = model.DIYGardening_ProductsService;
                                GetCampaignProfileForProductsServices.AppliancesOtherHouseholdDurables_ProductsService = model.AppliancesOtherHouseholdDurables_ProductsService;
                                GetCampaignProfileForProductsServices.ElectronicsOtherPersonalItems_ProductsService = model.ElectronicsOtherPersonalItems_ProductsService;
                                GetCampaignProfileForProductsServices.CommunicationsInternet_ProductsService = model.CommunicationsInternet_ProductsService;
                                GetCampaignProfileForProductsServices.FinancialServices_ProductsService = model.FinancialServices_ProductsService;
                                GetCampaignProfileForProductsServices.HolidaysTravel_ProductsService = model.HolidaysTravel_ProductsService;
                                GetCampaignProfileForProductsServices.SportsLeisure_ProductsService = model.SportsLeisure_ProductsService;
                                GetCampaignProfileForProductsServices.Motoring_ProductsService = model.Motoring_ProductsService;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Food_ProductsService = model.Food_ProductsService;
                                campaignmatch.SweetSaltySnacks_ProductsService = model.SweetSaltySnacks_ProductsService;
                                campaignmatch.AlcoholicDrinks_ProductsService = model.AlcoholicDrinks_ProductsService;
                                campaignmatch.NonAlcoholicDrinks_ProductsService = model.NonAlcoholicDrinks_ProductsService;
                                campaignmatch.Householdproducts_ProductsService = model.Householdproducts_ProductsService;
                                campaignmatch.ToiletriesCosmetics_ProductsService = model.ToiletriesCosmetics_ProductsService;
                                campaignmatch.PharmaceuticalChemistsProducts_ProductsService = model.PharmaceuticalChemistsProducts_ProductsService;
                                campaignmatch.TobaccoProducts_ProductsService = model.TobaccoProducts_ProductsService;
                                campaignmatch.PetsPetFood_ProductsService = model.PetsPetFood_ProductsService;
                                campaignmatch.ShoppingRetailClothing_ProductsService = model.ShoppingRetailClothing_ProductsService;
                                campaignmatch.DIYGardening_ProductsService = model.DIYGardening_ProductsService;
                                campaignmatch.AppliancesOtherHouseholdDurables_ProductsService = model.AppliancesOtherHouseholdDurables_ProductsService;
                                campaignmatch.ElectronicsOtherPersonalItems_ProductsService = model.ElectronicsOtherPersonalItems_ProductsService;
                                campaignmatch.CommunicationsInternet_ProductsService = model.CommunicationsInternet_ProductsService;
                                campaignmatch.FinancialServices_ProductsService = model.FinancialServices_ProductsService;
                                campaignmatch.HolidaysTravel_ProductsService = model.HolidaysTravel_ProductsService;
                                campaignmatch.SportsLeisure_ProductsService = model.SportsLeisure_ProductsService;
                                campaignmatch.Motoring_ProductsService = model.Motoring_ProductsService;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }


                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileProductsServicesId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SavePress(CampaignProfilePressFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfilePressCommand command =
                        Mapper.Map<CampaignProfilePressFormModel, CreateOrUpdateCampaignProfilePressCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfilePressId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfilePressId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForPress = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForPress.Local_Press = model.Local_Press;
                                GetCampaignProfileForPress.National_Press = model.National_Press;
                                GetCampaignProfileForPress.FreeNewpapers_Press = model.FreeNewpapers_Press;
                                GetCampaignProfileForPress.Magazines_Press = model.Magazines_Press;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Local_Press = model.Local_Press;
                                campaignmatch.National_Press = model.National_Press;
                                campaignmatch.FreeNewpapers_Press = model.FreeNewpapers_Press;
                                campaignmatch.Magazines_Press = model.Magazines_Press;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfilePressId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SaveInternet(CampaignProfileInternetFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileInternetCommand command =
                        Mapper.Map<CampaignProfileInternetFormModel, CreateOrUpdateCampaignProfileInternetCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileInternetId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileInternetId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForInternet = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForInternet.SocialNetworking_Internet = model.SocialNetworking_Internet;
                                GetCampaignProfileForInternet.Video_Internet = model.Video_Internet;
                                GetCampaignProfileForInternet.Research_Internet = model.Research_Internet;
                                GetCampaignProfileForInternet.Auctions_Internet = model.Auctions_Internet;
                                GetCampaignProfileForInternet.Shopping_Internet = model.Shopping_Internet;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.SocialNetworking_Internet = model.SocialNetworking_Internet;
                                campaignmatch.Video_Internet = model.Video_Internet;
                                campaignmatch.Research_Internet = model.Research_Internet;
                                campaignmatch.Auctions_Internet = model.Auctions_Internet;
                                campaignmatch.Shopping_Internet = model.Shopping_Internet;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileInternetId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SaveCinema(CampaignProfileCinemaFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileCinemaCommand command =
                        Mapper.Map<CampaignProfileCinemaFormModel, CreateOrUpdateCampaignProfileCinemaCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileCinemaId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileCinemaId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForCinema = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                                GetCampaignProfileForCinema.Cinema_Cinema = model.Cinema_Cinema;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Cinema_Cinema = model.Cinema_Cinema;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileCinemaId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
        [HttpPost]
        public ActionResult SaveAttitude(CampaignProfileAttitudeFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    CreateOrUpdateCampaignProfileAttitudeCommand command =
                        Mapper.Map<CampaignProfileAttitudeFormModel, CreateOrUpdateCampaignProfileAttitudeCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileAttitudeId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileAttitudeId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForAttitude = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForAttitude.Fitness_Attitude = model.Fitness_Attitude;
                                GetCampaignProfileForAttitude.Holidays_Attitude = model.Holidays_Attitude;
                                GetCampaignProfileForAttitude.Environment_Attitude = model.Environment_Attitude;
                                GetCampaignProfileForAttitude.GoingOut_Attitude = model.GoingOut_Attitude;
                                GetCampaignProfileForAttitude.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                                GetCampaignProfileForAttitude.Religion_Attitude = model.Religion_Attitude;
                                GetCampaignProfileForAttitude.Fashion_Attitude = model.Fashion_Attitude;
                                GetCampaignProfileForAttitude.Music_Attitude = model.Music_Attitude;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Fitness_Attitude = model.Fitness_Attitude;
                                campaignmatch.Holidays_Attitude = model.Holidays_Attitude;
                                campaignmatch.Environment_Attitude = model.Environment_Attitude;
                                campaignmatch.GoingOut_Attitude = model.GoingOut_Attitude;
                                campaignmatch.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                                campaignmatch.Religion_Attitude = model.Religion_Attitude;
                                campaignmatch.Fashion_Attitude = model.Fashion_Attitude;
                                campaignmatch.Music_Attitude = model.Music_Attitude;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileAttitudeId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }
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


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForAdverts = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForAdverts.Food_Advert = model.Food_Advert;
                                GetCampaignProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                                GetCampaignProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                                GetCampaignProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                                GetCampaignProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                                GetCampaignProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                                GetCampaignProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                                GetCampaignProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                                GetCampaignProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                                GetCampaignProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                                GetCampaignProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                                GetCampaignProfileForAdverts.AppliancesOtherHouseholdDurables_Advert = model.AppliancesOtherHouseholdDurables_Advert;
                                GetCampaignProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                                GetCampaignProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                                GetCampaignProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                                GetCampaignProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                                GetCampaignProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                                GetCampaignProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                                GetCampaignProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                                GetCampaignProfileForAdverts.Magazines_Advert = model.Magazines_Advert;
                                GetCampaignProfileForAdverts.TV_Advert = model.TV_Advert;
                                GetCampaignProfileForAdverts.Radio_Advert = model.Radio_Advert;
                                GetCampaignProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                                GetCampaignProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                                GetCampaignProfileForAdverts.GeneralUse_Advert = model.GeneralUse_Advert;
                                GetCampaignProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                                GetCampaignProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                                GetCampaignProfileForAdverts.Holidays_Advert = model.Holidays_Advert;
                                GetCampaignProfileForAdverts.Environment_Advert = model.Environment_Advert;
                                GetCampaignProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                                GetCampaignProfileForAdverts.FinancialProducts_Advert = model.FinancialProducts_Advert;
                                GetCampaignProfileForAdverts.Religion_Advert = model.Religion_Advert;
                                GetCampaignProfileForAdverts.Fashion_Advert = model.Fashion_Advert;
                                GetCampaignProfileForAdverts.Music_Advert = model.Music_Advert;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Food_Advert = model.Food_Advert;
                                campaignmatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                                campaignmatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                                campaignmatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                                campaignmatch.Householdproducts_Advert = model.Householdproducts_Advert;
                                campaignmatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                                campaignmatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                                campaignmatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                                campaignmatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                                campaignmatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                                campaignmatch.DIYGardening_Advert = model.DIYGardening_Advert;
                                campaignmatch.AppliancesOtherHouseholdDurables_Advert = model.AppliancesOtherHouseholdDurables_Advert;
                                campaignmatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                                campaignmatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                                campaignmatch.FinancialServices_Advert = model.FinancialServices_Advert;
                                campaignmatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                                campaignmatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                                campaignmatch.Motoring_Advert = model.Motoring_Advert;
                                campaignmatch.Newspapers_Advert = model.Newspapers_Advert;
                                campaignmatch.Magazines_Advert = model.Magazines_Advert;
                                campaignmatch.TV_Advert = model.TV_Advert;
                                campaignmatch.Radio_Advert = model.Radio_Advert;
                                campaignmatch.Cinema_Advert = model.Cinema_Advert;
                                campaignmatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                                campaignmatch.GeneralUse_Advert = model.GeneralUse_Advert;
                                campaignmatch.Shopping_Advert = model.Shopping_Advert;
                                campaignmatch.Fitness_Advert = model.Fitness_Advert;
                                campaignmatch.Holidays_Advert = model.Holidays_Advert;
                                campaignmatch.Environment_Advert = model.Environment_Advert;
                                campaignmatch.GoingOut_Advert = model.GoingOut_Advert;
                                campaignmatch.FinancialProducts_Advert = model.FinancialProducts_Advert;
                                campaignmatch.Religion_Advert = model.Religion_Advert;
                                campaignmatch.Fashion_Advert = model.Fashion_Advert;
                                campaignmatch.Music_Advert = model.Music_Advert;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;
                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            //var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            //if (GetCampaignProfileById != null)
                            //{
                            //    var GetCampaignProfileForAttitude = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                            //    GetCampaignProfileForAttitude.Fitness_Attitude = model.Fitness_Attitude;
                            //    GetCampaignProfileForAttitude.Holidays_Attitude = model.Holidays_Attitude;
                            //    GetCampaignProfileForAttitude.Environment_Attitude = model.Environment_Attitude;
                            //    GetCampaignProfileForAttitude.GoingOut_Attitude = model.GoingOut_Attitude;
                            //    GetCampaignProfileForAttitude.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                            //    GetCampaignProfileForAttitude.Religion_Attitude = model.Religion_Attitude;
                            //    GetCampaignProfileForAttitude.Fashion_Attitude = model.Fashion_Attitude;
                            //    GetCampaignProfileForAttitude.Music_Attitude = model.Music_Attitude;

                            //    mySQLEntities.SaveChanges();
                            //}
                            //else
                            //{
                            //    var campaignmatch = new campaignmatch();

                            //    campaignmatch.Fitness_Attitude = model.Fitness_Attitude;
                            //    campaignmatch.Holidays_Attitude = model.Holidays_Attitude;
                            //    campaignmatch.Environment_Attitude = model.Environment_Attitude;
                            //    campaignmatch.GoingOut_Attitude = model.GoingOut_Attitude;
                            //    campaignmatch.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                            //    campaignmatch.Religion_Attitude = model.Religion_Attitude;
                            //    campaignmatch.Fashion_Attitude = model.Fashion_Attitude;
                            //    campaignmatch.Music_Attitude = model.Music_Attitude;
                            //    campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                            //    mySQLEntities.campaignmatches.Add(campaignmatch);
                            //    // mySQLEntities.SaveChanges();
                            //    SaveChanges(mySQLEntities);
                            //}

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

        [HttpPost]
        public ActionResult SaveDemographics(CampaignProfileDemographicsFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    int? id = model.CampaignProfileId;
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    CreateOrUpdateCampaignProfileDemographicsCommand command =

                       Mapper.Map<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>(
                           model);
                    //check campaignprofile exists.

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

                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {

                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                        if (GetCampaignProfileById != null)
                        {
                            var GetCampaignProfileForDemographics = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            GetCampaignProfileForDemographics.Age_Demographics = model.Age_Demographics;
                            GetCampaignProfileForDemographics.Gender_Demographics = model.Gender_Demographics;
                            GetCampaignProfileForDemographics.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
                            GetCampaignProfileForDemographics.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
                            GetCampaignProfileForDemographics.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
                            GetCampaignProfileForDemographics.Education_Demographics = model.Education_Demographics;
                            GetCampaignProfileForDemographics.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
                            // code commented on  29-03-2017
                            // GetCampaignProfileForDemographics.Location_Demographics = model.Location_Demographics;

                            mySQLEntities.SaveChanges();
                        }
                        else
                        {
                            var campaignmatch = new campaignmatch();

                            campaignmatch.Age_Demographics = model.Age_Demographics;
                            campaignmatch.Gender_Demographics = model.Gender_Demographics;
                            campaignmatch.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
                            campaignmatch.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
                            campaignmatch.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
                            campaignmatch.Education_Demographics = model.Education_Demographics;
                            campaignmatch.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
                            // code commented on  29-03-2017
                            // campaignmatch.Location_Demographics = model.Location_Demographics;
                            campaignmatch.MSCampaignProfileId = model.CampaignProfileId;


                            mySQLEntities.campaignmatches.Add(campaignmatch);
                            // mySQLEntities.SaveChanges();
                            SaveChanges(mySQLEntities);
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
        [HttpPost]
        public ActionResult GetClientAdvert(int[] clientId)
        {
            try
            {


                if (clientId != null)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var advertdetails = _advertRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && clientId.Contains(top.ClientId)).Select(top => new
                    {
                        AdvertName = top.AdvertName,
                        AdvertId = top.AdvertId.ToString()
                    }).ToList();
                    return Json(advertdetails);

                }
                else
                {
                    return Json("nodata");
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        public ActionResult GetCampaignAdvert(int[] campaignId)
        {
            try
            {


                if (campaignId != null)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var advertId = _campaignAdvertRepository.Get(top => campaignId.Contains(top.CampaignProfileId));
                    if (advertId != null)
                    {
                        var advertdetails = _advertRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && top.AdvertId == advertId.AdvertId).Select(top => new
                        {
                            AdvertName = top.AdvertName,
                            AdvertId = top.AdvertId.ToString()
                        }).ToList();
                        return Json(advertdetails);
                    }
                    return Json("nodata");
                }
                else
                {
                    return Json("nodata");
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        public ActionResult GetClientCampaign(int[] clientId)
        {
            try
            {
                if (clientId != null)
                {
                    List<CampaignProfileResult> _result = (List<CampaignProfileResult>)TempData["compainresult"];
                    if (TempData["compainresult"] != null)
                    {


                        var _resultDetails = _result.Where(top => clientId.Contains(top.ClientId)).Select(top => new
                        {
                            CampaignName = top.CampaignName,
                            CampaignProfileId = top.CampaignProfileId
                        }).ToList();
                        TempData.Keep("compainresult");
                        return Json(_resultDetails);

                    }
                }
                else
                {
                    List<CampaignProfileResult> _result = (List<CampaignProfileResult>)TempData["compainresult"];
                    if (TempData["compainresult"] != null)
                    {


                        var _resultDetails = _result.Select(top => new
                        {
                            CampaignName = top.CampaignName,
                            CampaignProfileId = top.CampaignProfileId
                        }).ToList();
                        TempData.Keep("compainresult");
                        return Json(_resultDetails);

                    }
                }
                return Json("error");
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        public CampaignDashboardChartResult FillChartData(IEnumerable<CampaignProfile> _CampaignProfileFormModel, int[] clientId)
        {
            List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
            List<CampaignNoOfPlay> _campaignNoofplay = new List<CampaignNoOfPlay>();
            List<Campaignchartresult> _campaignbidavgResult = new List<Campaignchartresult>();
            List<CampaignAvgbid> _campaignAvgbid = new List<CampaignAvgbid>();
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            float PlaystoDate = 0, TotalBudget = 0;
            double SpendToDate = 0;
            double EmailCampaignCost = 0, SMSCampaignCost = 0, CancelledCampaignCost = 0;
            double AverageBid = 0, MaxBid = 0, AvgMaxBid = 0, AveragePlayTime = 0, SMSCost = 0, EmailCost = 0, Cancelled = 0;
            List<long> _maxplaylength = new List<long>();
            int FreePlays = 0, TotalPlayed = 0;
            DateTime? maxdate = new DateTime(), mindate = new DateTime();
            _CampaignProfileFormModel = _CampaignProfileFormModel.Where(top => top.Status != 5);
            if (_CampaignProfileFormModel != null)
            {
                if (TempData["DashboardClientId"] != null)
                {
                    clientId = (int[])TempData["DashboardClientId"];
                }
                if (clientId != null)
                {
                    _CampaignProfileFormModel = _CampaignProfileFormModel.Where(top => clientId.Contains(top.ClientId));
                }
                TotalBudget = TotalBudget + _CampaignProfileFormModel.Sum(d => d.TotalBudget);

                foreach (var item in _CampaignProfileFormModel)
                {
                    if (item.CampaignAudits.Count > 0)
                    {

                        _maxplaylength.Add(item.CampaignAudits.Max(top => top.PlayLengthTicks));
                        //caculate the PlaystoDate field
                        if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                        {
                            PlaystoDate = PlaystoDate + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count();
                        }
                        //caculate the SpendToDate field
                        if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                        {
                            SpendToDate = SpendToDate + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(top => top.BidValue);
                            SMSCost = SMSCost + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(d1 => d1.SMSCost);
                            EmailCost = EmailCost + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(d1 => d1.EmailCost);
                            SpendToDate = SpendToDate + SMSCost + EmailCost;
                        }
                        //caculate the AverageBid field
                        if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                        {
                            AverageBid = AverageBid + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Average(d1 => d1.BidValue);
                        }
                        //caculate the AveragePlayTime  field
                        // Code commented on 30-05-2017
                        //if (item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                        //{                           
                        //    AveragePlayTime = AveragePlayTime + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Average(d1 => d1.PlayLengthTicks);
                        //}
                        //caculate the FreePlays field
                        if (item.CampaignAudits.Where(top => top.PlayLengthTicks <= 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count() > 0)
                        {
                            FreePlays = FreePlays + item.CampaignAudits.Where(top => top.PlayLengthTicks < 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count();
                        }
                        //caculate the Max Bid Field
                        if (item.CampaignAudits.Where(top => top.PlayLengthTicks > 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count() > 0)
                        {
                            MaxBid = MaxBid + item.CampaignAudits.Where(top => top.PlayLengthTicks > 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Max(top => top.BidValue);
                        }
                        SMSCampaignCost = SMSCampaignCost + item.CampaignAudits.Sum(d1 => d1.SMSCost);
                        EmailCampaignCost = EmailCampaignCost + item.CampaignAudits.Sum(d1 => d1.EmailCost);
                        Cancelled = Cancelled + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Cancelled).ToLower() && top.PlayLengthTicks > 6000).Sum(d1 => d1.TotalCost);
                        CancelledCampaignCost = CancelledCampaignCost + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Cancelled).ToLower()).Sum(d1 => d1.TotalCost);


                        TotalPlayed = TotalPlayed + item.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count();

                        foreach (var camAudit in item.CampaignAudits)
                        {
                            if (camAudit.PlayLengthTicks > 6000 && camAudit.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower())
                            {
                                _campaignNoofplay.Add(new CampaignNoOfPlay { startdate = camAudit.StartTime, playcount = 1, startdatecompare = camAudit.StartTime });
                                _campaignAvgbid.Add(new CampaignAvgbid { startdate = camAudit.StartTime, bidvalue = camAudit.BidValue, startdatecompare = camAudit.StartTime });
                                _playgroup.Add(new MaxLengthGroup { second = (camAudit.PlayLengthTicks / 1000) });
                            }
                        }
                    }
                }
                var GetCampaignAuditData = _campaignAuditRepository.GetAll().Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).ToList();
                if(GetCampaignAuditData.Count() > 0)
                {
                    AveragePlayTime = GetCampaignAuditData.Average(s => s.PlayLengthTicks);
                }

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
            //_CampaignDashboardChartResult.AveragePlayTime = RoundUp(AveragePlayTime, 2); 
            _CampaignDashboardChartResult.AveragePlayTime = RoundUp(AveragePlayTime, 1);
            _CampaignDashboardChartResult.FreePlays = FreePlays;
            _CampaignDashboardChartResult.TotalPlayed = Convert.ToInt32(PlaystoDate);
            _CampaignDashboardChartResult.TotalBudget = TotalBudget;
            _CampaignDashboardChartResult.TotalSpend = RoundUp(SpendToDate, 2);
            _CampaignDashboardChartResult.MaxBid = RoundUp(MaxBid, 2);
            _CampaignDashboardChartResult.AvgMaxBid = RoundUp(AverageBid, 2);
            if (_maxplaylength.Count > 0)
            {
                _CampaignDashboardChartResult.MaxPlayLength = (_maxplaylength.Max()) / 1000;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = RoundUp((AveragePlayTime / (_maxplaylength.Max() / 1000)), 2);
            }
            else
            {
                _CampaignDashboardChartResult.MaxPlayLength = 0;
                _CampaignDashboardChartResult.MaxPlayLengthPercantage = 0;
            }
            //get max bid percantage
            if (AvgMaxBid > 0 && MaxBid > 0)
                _CampaignDashboardChartResult.MaxBidPercantage = RoundUp(((AverageBid / MaxBid) * 100), 2);
            else
                _CampaignDashboardChartResult.MaxBidPercantage = 0;
            //get total budget percantage
            if (SpendToDate > 0 && TotalBudget > 0)
                _CampaignDashboardChartResult.TotalBudgetPercantage = RoundUp(((SpendToDate / TotalBudget) * 100), 2);

            else
                _CampaignDashboardChartResult.TotalBudgetPercantage = 0;
            //free play percantage
            if (PlaystoDate > 0 && FreePlays > 0)
                _CampaignDashboardChartResult.FreePlaysPercantage = RoundUp(((FreePlays / (PlaystoDate + FreePlays)) * 100), 0);
            else
                _CampaignDashboardChartResult.FreePlaysPercantage = 0;
            ViewBag.FreePlays = FreePlays;
            ViewBag.TotalPlayed = TotalPlayed;
            ViewBag.TotalBudget = TotalBudget;
            ViewBag.TotalSpend = SpendToDate;
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
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        public ActionResult GetChartDetailsbyClient(int[] CampaignChartIndexClientId)
        {
            TempData["DashboardClientId"] = CampaignChartIndexClientId;
            return Json("success");
        }

        public CampaignDashboardChartResult FillChartDataByCampaignId(CampaignProfileFormModel _CampaignProfileFormModel)
        {
            List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
            List<CampaignNoOfPlay> _campaignNoofplay = new List<CampaignNoOfPlay>();
            List<Campaignchartresult> _campaignbidavgResult = new List<Campaignchartresult>();
            List<CampaignAvgbid> _campaignAvgbid = new List<CampaignAvgbid>();
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            float PlaystoDate = 0, TotalBudget = 0;
            double AverageBid = 0, SpendToDate = 0, MaxBid = 0, AveragePlayTime = 0, SMSCost = 0, EmailCost = 0, Cancelled = 0;
            List<long> _maxplaylength = new List<long>();
            int FreePlays = 0;
            DateTime? maxdate = new DateTime(), mindate = new DateTime();
            if (_CampaignProfileFormModel != null)
            {



                TotalBudget = _CampaignProfileFormModel.TotalBudget;
                var campaignMaxBid = _CampaignProfileFormModel.MaxBid;
                if (_CampaignProfileFormModel.CampaignAudits.Count > 0)
                {
                    //caculate the PlaystoDate field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                    {
                        PlaystoDate = PlaystoDate + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count();
                    }
                    //caculate the SpendToDate field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                    {
                        SpendToDate = SpendToDate + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(top => top.BidValue);
                        SpendToDate = SpendToDate + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(top => top.SMSCost);
                        SpendToDate = SpendToDate + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Sum(top => top.EmailCost);
                    }

                    //caculate the AverageBid field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                    {
                        AverageBid = AverageBid + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Average(top => top.BidValue);
                    }
                    //caculate the AveragePlayTime field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                    {
                        AveragePlayTime = AveragePlayTime + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Average(top => top.PlayLengthTicks);
                    }
                    //caculate the FreePlays field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.PlayLengthTicks <= 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count() > 0)
                    {
                        FreePlays = FreePlays + _CampaignProfileFormModel.CampaignAudits.Where(top => top.PlayLengthTicks <= 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count();
                    }
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Count() > 0)
                    {
                        _maxplaylength.Add(_CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower() && top.PlayLengthTicks > 6000).Max(top => top.PlayLengthTicks));
                    }
                    //caculate the Max Bid Field
                    if (_CampaignProfileFormModel.CampaignAudits.Where(top => top.PlayLengthTicks > 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Count() > 0)
                    {
                        MaxBid = MaxBid + _CampaignProfileFormModel.CampaignAudits.Where(top => top.PlayLengthTicks > 6000 && top.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower()).Max(top => top.BidValue);
                    }
                    SMSCost = SMSCost + _CampaignProfileFormModel.CampaignAudits.Sum(d1 => d1.SMSCost);
                    EmailCost = EmailCost + _CampaignProfileFormModel.CampaignAudits.Sum(d1 => d1.EmailCost);
                    Cancelled = Cancelled + _CampaignProfileFormModel.CampaignAudits.Where(top => top.Status.Trim().ToLower() == Convert.ToString(CampaignAuditStatus.Cancelled).ToLower()).Sum(d1 => d1.TotalCost);
                    foreach (var camAudit in _CampaignProfileFormModel.CampaignAudits)
                    {
                        if (camAudit.PlayLengthTicks > 6000 && camAudit.Status.ToLower() == Convert.ToString(CampaignAuditStatus.Played).ToLower())
                        {
                            _campaignNoofplay.Add(new CampaignNoOfPlay { startdate = camAudit.StartTime, playcount = 1, startdatecompare = camAudit.StartTime });
                            _campaignAvgbid.Add(new CampaignAvgbid { startdate = camAudit.StartTime, bidvalue = camAudit.BidValue, startdatecompare = camAudit.StartTime });
                            _playgroup.Add(new MaxLengthGroup { second = (camAudit.PlayLengthTicks / 1000) });
                        }
                    }
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
                _CampaignDashboardChartResult.TotalBudgetPercantage = RoundUp(((SpendToDate / TotalBudget) * 100), 0);

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




        #region AddCampaign

      

        public ActionResult SaveCampaign(CampaignProfileFormModel model, string hdnstatus)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                model.Status = (int)CampaignStatus.Play;
                if (model.StartDate == null && model.EndDate != null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels =
                        Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign start date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate == null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels =
                        Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign end date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate != null)
                {
                    if (model.EndDate.Value.Date < model.StartDate.Value.Date)
                    {
                        var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                        IEnumerable<ClientModel> clientModels =
                            Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                        FillAddClient(clientModels);
                        TempData["Error"] = "EndDate must be greater than StartDate.";
                        return View("Initialise", model);

                    }
                }
                model.Active = true;
                model.TotalBudget = 0;


                model.CampaignProfileAttitudes = new Collection<CampaignProfileAttitudeFormModel>
                                                     {new CampaignProfileAttitudeFormModel()};

                CreateOrUpdateCampaignProfileCommand command =
                    Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(model);





                command.CampaignProfileAdverts =
                    Mapper.Map
                        <ICollection<CampaignProfileAdvertFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAdvertCommand>>(model.CampaignProfileAdverts ??
                                                                                     new Collection
                                                                                         <CampaignProfileAdvertFormModel
                                                                                         >
                                                                                         {
                                                                                             new CampaignProfileAdvertFormModel
                                                                                                 ()
                                                                                         });
                command.CampaignProfileAttitudes =
                    Mapper.Map
                        <ICollection<CampaignProfileAttitudeFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAttitudeCommand>>(model.CampaignProfileAttitudes);
                command.CampaignProfileCinemas =
                    Mapper.Map
                        <ICollection<CampaignProfileCinemaFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileCinemaCommand>>(model.CampaignProfileCinemas);
                command.CampaignProfileInternets =
                    Mapper.Map
                        <ICollection<CampaignProfileInternetFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileInternetCommand>>(model.CampaignProfileInternets);
                command.CampaignProfileMobiles =
                    Mapper.Map
                        <ICollection<CampaignProfileMobileFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileMobileCommand>>(model.CampaignProfileMobiles);
                command.CampaignProfilePresses =
                    Mapper.Map
                        <ICollection<CampaignProfilePressFormModel>,
                            ICollection<CreateOrUpdateCampaignProfilePressCommand>>(model.CampaignProfilePresses);
                command.CampaignProfileProductsServices =
                    Mapper.Map
                        <ICollection<CampaignProfileProductsServiceFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileProductsServiceCommand>>(
                                model.CampaignProfileProductsServices);
                command.CampaignProfileRadios =
                    Mapper.Map
                        <ICollection<CampaignProfileRadioFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileRadioCommand>>(model.CampaignProfileRadios);
                command.CampaignProfileTimeSettings =
                    Mapper.Map
                        <ICollection<CampaignProfileTimeSettingFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileTimeSettingCommand>>(
                                model.CampaignProfileTimeSettings);
                command.CampaignProfileTvs =
                    Mapper.Map
                        <ICollection<CampaignProfileTvFormModel>, ICollection<CreateOrUpdateCampaignProfileTvCommand>>(
                            model.CampaignProfileTvs);
                command.CampaignProfileDemographics =
                    Mapper.Map
                        <ICollection<CampaignProfileDemographicsFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileDemographicsCommand>>(
                                model.CampaignProfileDemographicsFormModels ??
                                new Collection<CampaignProfileDemographicsFormModel>
                                    {new CampaignProfileDemographicsFormModel()});


                command.UserId = efmvcUser.UserId;
                command.NumberInBatch = model.NumberInBatch;
                command.CreatedDateTime = DateTime.Now;
                command.UpdatedDateTime = DateTime.Now;

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var campaignmatch = new campaignmatch();

                            campaignmatch.CampaignName = model.CampaignName;
                            campaignmatch.CampaignDescription = model.CampaignDescription;
                            campaignmatch.ClientId = model.ClientId;
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(model.MaxDailyBudget);
                            campaignmatch.TotalBudget = Convert.ToInt64(model.TotalBudget);
                            campaignmatch.MaxBid = Convert.ToInt32(model.MaxBid);
                            campaignmatch.StartDate = model.StartDate;
                            campaignmatch.EndDate = model.EndDate;
                            campaignmatch.Active = model.Active;
                            campaignmatch.UserId = efmvcUser.UserId;
                            campaignmatch.Status = model.Status;
                            campaignmatch.MSCampaignProfileId = result.Id;
                            campaignmatch.NumberInBatch = model.NumberInBatch;

                            mySQLEntities.campaignmatches.Add(campaignmatch);
                            SaveChanges(mySQLEntities);

                        }

                        Session["NewCampaignId"] = result.Id;
                        return Json("success");
                        //return Json(new { data = "success", NewCampaignsId = result.Id, ClientId =model.ClientId }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            //if fail
            if (model.CampaignProfileId == 0)
                return Json("fail");

            return Json("fail");
        }


        public ActionResult SaveBudgetInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {
                var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                CampaignProfileFormModel.CampaignProfileId = CampProfileId;

                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                command.UserId = efmvcUser.UserId;

                command.MaxBid = CampaignProfileFormModel.MaxBid;
                command.MaxMonthBudget = CampaignProfileFormModel.MaxMonthBudget;
                command.MaxWeeklyBudget = CampaignProfileFormModel.MaxWeeklyBudget;
                command.MaxHourlyBudget = CampaignProfileFormModel.MaxHourlyBudget;
                command.MaxDailyBudget = CampaignProfileFormModel.MaxDailyBudget;


                using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                {
                    var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();
                    if (GetCampaignProfileById != null)
                    {
                        var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                        campaignmatch.MaxBid = Convert.ToInt32(CampaignProfileFormModel.MaxBid);
                        campaignmatch.MaxMonthBudget = Convert.ToInt64(CampaignProfileFormModel.MaxMonthBudget);
                        campaignmatch.MaxWeeklyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxWeeklyBudget);
                        campaignmatch.MaxHourlyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxHourlyBudget);
                        campaignmatch.MaxDailyBudget = Convert.ToInt64(CampaignProfileFormModel.MaxDailyBudget);
                        campaignmatch.UserId = efmvcUser.UserId;
                        campaignmatch.MSCampaignProfileId = CampaignProfileFormModel.CampaignProfileId;
                        mySQLEntities.SaveChanges();
                    }
                }

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


                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();

                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            campaignmatch.StartDate = campaigndetails.StartDate;
                            campaignmatch.EndDate = campaigndetails.EndDate;
                            campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                            campaignmatch.CampaignName = campaigndetails.CampaignName;
                            campaignmatch.ClientId = campaigndetails.ClientId;
                            campaignmatch.Status = campaigndetails.Status;

                            campaignmatch.Active = campaigndetails.Active;
                            campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                            campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                            campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                            campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                            campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                            campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                            campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                            campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;

                            campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                            campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                            campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                            campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                            campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                            campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                            campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                            campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                            campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.EmailBody = campaigndetails.EmailBody;
                            campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                            campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                            campaignmatch.SmsBody = campaigndetails.SmsBody;
                            campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;

                            campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                            campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                            campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;
                            campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;

                            mySQLEntities.SaveChanges();
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


        public ActionResult SaveCommunicationInfo(CampaignProfileFormModel CampaignProfileFormModel, HttpPostedFileBase emailfile, HttpPostedFileBase smsfile)
        {

            try
            {
                var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                CampaignProfileFormModel.CampaignProfileId = CampProfileId;
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
                    command.NumberInBatch = campaigndetails.NumberInBatch;
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

                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {
                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == CampaignProfileFormModel.CampaignProfileId).FirstOrDefault();

                        if (GetCampaignProfileById != null)
                        {
                            var campaignmatch = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                            campaignmatch.EMAIL_MESSAGE = CampaignProfileFormModel.EmailBody;
                            campaignmatch.SMS_MESSAGE = CampaignProfileFormModel.SmsBody;
                            campaignmatch.ORIGINATOR = CampaignProfileFormModel.SmsOriginator;

                            campaignmatch.EmailBody = CampaignProfileFormModel.EmailBody;
                            campaignmatch.EmailSenderAddress = CampaignProfileFormModel.EmailSenderAddress;
                            campaignmatch.EmailSubject = CampaignProfileFormModel.EmailSubject;
                            campaignmatch.SmsBody = CampaignProfileFormModel.SmsBody;
                            campaignmatch.SmsOriginator = CampaignProfileFormModel.SmsOriginator;

                            campaignmatch.EmailFileLocation = command.EmailFileLocation;
                            campaignmatch.SMSFileLocation = command.SMSFileLocation;

                            campaignmatch.StartDate = campaigndetails.StartDate;
                            campaignmatch.EndDate = campaigndetails.EndDate;
                            campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                            campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                            campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                            campaignmatch.CampaignName = campaigndetails.CampaignName;
                            campaignmatch.ClientId = campaigndetails.ClientId;
                            campaignmatch.Status = campaigndetails.Status;

                            campaignmatch.Active = campaigndetails.Active;
                            campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                            campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                            campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                            campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                            campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                            campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                            campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                            campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;

                            campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                            campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                            campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                            campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                            campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                            campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                            campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                            campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                            campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                            campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                            campaignmatch.UserId = campaigndetails.UserId;
                            campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;
                            mySQLEntities.SaveChanges();
                        }
                    }


                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {                        
                        return Json("success");
                    }
                }

                return Json("success");

            }
            catch (Exception ex)
            {
                return Json("fail");
            }
        }

        [HttpPost]
        public ActionResult SaveDemographicsInfo(CampaignProfileDemographicsFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    int? id = CampProfileId;
                    model.CampaignProfileId = CampProfileId;
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    CreateOrUpdateCampaignProfileDemographicsCommand command =

                       Mapper.Map<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>(
                           model);
                    //check campaignprofile exists.

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

                    using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    {

                        var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                        if (GetCampaignProfileById != null)
                        {
                            var GetCampaignProfileForDemographics = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                            GetCampaignProfileForDemographics.Age_Demographics = model.Age_Demographics;
                            GetCampaignProfileForDemographics.Gender_Demographics = model.Gender_Demographics;
                            GetCampaignProfileForDemographics.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
                            GetCampaignProfileForDemographics.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
                            GetCampaignProfileForDemographics.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
                            GetCampaignProfileForDemographics.Education_Demographics = model.Education_Demographics;
                            GetCampaignProfileForDemographics.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
                            // code commented on  29-03-2017
                            // GetCampaignProfileForDemographics.Location_Demographics = model.Location_Demographics;

                            mySQLEntities.SaveChanges();
                        }
                        else
                        {
                            var campaignmatch = new campaignmatch();

                            campaignmatch.Age_Demographics = model.Age_Demographics;
                            campaignmatch.Gender_Demographics = model.Gender_Demographics;
                            campaignmatch.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
                            campaignmatch.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
                            campaignmatch.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
                            campaignmatch.Education_Demographics = model.Education_Demographics;
                            campaignmatch.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
                            // code commented on  29-03-2017
                            // campaignmatch.Location_Demographics = model.Location_Demographics;
                            campaignmatch.MSCampaignProfileId = model.CampaignProfileId;


                            mySQLEntities.campaignmatches.Add(campaignmatch);
                            // mySQLEntities.SaveChanges();
                            SaveChanges(mySQLEntities);
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

        [HttpPost]
        public ActionResult SaveGeographicInfo(CampaignProfileGeographicFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

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
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            //var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            //if (GetCampaignProfileById != null)
                            //{
                            //    var GetCampaignProfileForAttitude = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                            //    GetCampaignProfileForAttitude.Fitness_Attitude = model.Fitness_Attitude;
                            //    GetCampaignProfileForAttitude.Holidays_Attitude = model.Holidays_Attitude;
                            //    GetCampaignProfileForAttitude.Environment_Attitude = model.Environment_Attitude;
                            //    GetCampaignProfileForAttitude.GoingOut_Attitude = model.GoingOut_Attitude;
                            //    GetCampaignProfileForAttitude.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                            //    GetCampaignProfileForAttitude.Religion_Attitude = model.Religion_Attitude;
                            //    GetCampaignProfileForAttitude.Fashion_Attitude = model.Fashion_Attitude;
                            //    GetCampaignProfileForAttitude.Music_Attitude = model.Music_Attitude;

                            //    mySQLEntities.SaveChanges();
                            //}
                            //else
                            //{
                            //    var campaignmatch = new campaignmatch();

                            //    campaignmatch.Fitness_Attitude = model.Fitness_Attitude;
                            //    campaignmatch.Holidays_Attitude = model.Holidays_Attitude;
                            //    campaignmatch.Environment_Attitude = model.Environment_Attitude;
                            //    campaignmatch.GoingOut_Attitude = model.GoingOut_Attitude;
                            //    campaignmatch.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                            //    campaignmatch.Religion_Attitude = model.Religion_Attitude;
                            //    campaignmatch.Fashion_Attitude = model.Fashion_Attitude;
                            //    campaignmatch.Music_Attitude = model.Music_Attitude;
                            //    campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                            //    mySQLEntities.campaignmatches.Add(campaignmatch);
                            //    // mySQLEntities.SaveChanges();
                            //    SaveChanges(mySQLEntities);
                            //}

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

        [HttpPost]
        public ActionResult SaveAdvertsInfo(CampaignProfileAdvertFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                int? id = CampProfileId;
                model.CampaignProfileId = CampProfileId;
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


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForAdverts = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForAdverts.Food_Advert = model.Food_Advert;
                                GetCampaignProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                                GetCampaignProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                                GetCampaignProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                                GetCampaignProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                                GetCampaignProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                                GetCampaignProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                                GetCampaignProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                                GetCampaignProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                                GetCampaignProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                                GetCampaignProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                                GetCampaignProfileForAdverts.AppliancesOtherHouseholdDurables_Advert = model.AppliancesOtherHouseholdDurables_Advert;
                                GetCampaignProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                                GetCampaignProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                                GetCampaignProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                                GetCampaignProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                                GetCampaignProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                                GetCampaignProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                                GetCampaignProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                                GetCampaignProfileForAdverts.Magazines_Advert = model.Magazines_Advert;
                                GetCampaignProfileForAdverts.TV_Advert = model.TV_Advert;
                                GetCampaignProfileForAdverts.Radio_Advert = model.Radio_Advert;
                                GetCampaignProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                                GetCampaignProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                                GetCampaignProfileForAdverts.GeneralUse_Advert = model.GeneralUse_Advert;
                                GetCampaignProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                                GetCampaignProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                                GetCampaignProfileForAdverts.Holidays_Advert = model.Holidays_Advert;
                                GetCampaignProfileForAdverts.Environment_Advert = model.Environment_Advert;
                                GetCampaignProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                                GetCampaignProfileForAdverts.FinancialProducts_Advert = model.FinancialProducts_Advert;
                                GetCampaignProfileForAdverts.Religion_Advert = model.Religion_Advert;
                                GetCampaignProfileForAdverts.Fashion_Advert = model.Fashion_Advert;
                                GetCampaignProfileForAdverts.Music_Advert = model.Music_Advert;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Food_Advert = model.Food_Advert;
                                campaignmatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                                campaignmatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                                campaignmatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                                campaignmatch.Householdproducts_Advert = model.Householdproducts_Advert;
                                campaignmatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                                campaignmatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                                campaignmatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                                campaignmatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                                campaignmatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                                campaignmatch.DIYGardening_Advert = model.DIYGardening_Advert;
                                campaignmatch.AppliancesOtherHouseholdDurables_Advert = model.AppliancesOtherHouseholdDurables_Advert;
                                campaignmatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                                campaignmatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                                campaignmatch.FinancialServices_Advert = model.FinancialServices_Advert;
                                campaignmatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                                campaignmatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                                campaignmatch.Motoring_Advert = model.Motoring_Advert;
                                campaignmatch.Newspapers_Advert = model.Newspapers_Advert;
                                campaignmatch.Magazines_Advert = model.Magazines_Advert;
                                campaignmatch.TV_Advert = model.TV_Advert;
                                campaignmatch.Radio_Advert = model.Radio_Advert;
                                campaignmatch.Cinema_Advert = model.Cinema_Advert;
                                campaignmatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                                campaignmatch.GeneralUse_Advert = model.GeneralUse_Advert;
                                campaignmatch.Shopping_Advert = model.Shopping_Advert;
                                campaignmatch.Fitness_Advert = model.Fitness_Advert;
                                campaignmatch.Holidays_Advert = model.Holidays_Advert;
                                campaignmatch.Environment_Advert = model.Environment_Advert;
                                campaignmatch.GoingOut_Advert = model.GoingOut_Advert;
                                campaignmatch.FinancialProducts_Advert = model.FinancialProducts_Advert;
                                campaignmatch.Religion_Advert = model.Religion_Advert;
                                campaignmatch.Fashion_Advert = model.Fashion_Advert;
                                campaignmatch.Music_Advert = model.Music_Advert;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;
                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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

        [HttpPost]
        public ActionResult SaveAttitudeInfo(CampaignProfileAttitudeFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileAttitudeCommand command =
                        Mapper.Map<CampaignProfileAttitudeFormModel, CreateOrUpdateCampaignProfileAttitudeCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileAttitudeId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileAttitudeId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForAttitude = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForAttitude.Fitness_Attitude = model.Fitness_Attitude;
                                GetCampaignProfileForAttitude.Holidays_Attitude = model.Holidays_Attitude;
                                GetCampaignProfileForAttitude.Environment_Attitude = model.Environment_Attitude;
                                GetCampaignProfileForAttitude.GoingOut_Attitude = model.GoingOut_Attitude;
                                GetCampaignProfileForAttitude.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                                GetCampaignProfileForAttitude.Religion_Attitude = model.Religion_Attitude;
                                GetCampaignProfileForAttitude.Fashion_Attitude = model.Fashion_Attitude;
                                GetCampaignProfileForAttitude.Music_Attitude = model.Music_Attitude;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Fitness_Attitude = model.Fitness_Attitude;
                                campaignmatch.Holidays_Attitude = model.Holidays_Attitude;
                                campaignmatch.Environment_Attitude = model.Environment_Attitude;
                                campaignmatch.GoingOut_Attitude = model.GoingOut_Attitude;
                                campaignmatch.FinancialStabiity_Attitude = model.FinancialStabiity_Attitude;
                                campaignmatch.Religion_Attitude = model.Religion_Attitude;
                                campaignmatch.Fashion_Attitude = model.Fashion_Attitude;
                                campaignmatch.Music_Attitude = model.Music_Attitude;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileAttitudeId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveCinemaInfo(CampaignProfileCinemaFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileCinemaCommand command =
                        Mapper.Map<CampaignProfileCinemaFormModel, CreateOrUpdateCampaignProfileCinemaCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileCinemaId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileCinemaId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForCinema = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();
                                GetCampaignProfileForCinema.Cinema_Cinema = model.Cinema_Cinema;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Cinema_Cinema = model.Cinema_Cinema;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileCinemaId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveInternetInfo(CampaignProfileInternetFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileInternetCommand command =
                        Mapper.Map<CampaignProfileInternetFormModel, CreateOrUpdateCampaignProfileInternetCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileInternetId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileInternetId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForInternet = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForInternet.SocialNetworking_Internet = model.SocialNetworking_Internet;
                                GetCampaignProfileForInternet.Video_Internet = model.Video_Internet;
                                GetCampaignProfileForInternet.Research_Internet = model.Research_Internet;
                                GetCampaignProfileForInternet.Auctions_Internet = model.Auctions_Internet;
                                GetCampaignProfileForInternet.Shopping_Internet = model.Shopping_Internet;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.SocialNetworking_Internet = model.SocialNetworking_Internet;
                                campaignmatch.Video_Internet = model.Video_Internet;
                                campaignmatch.Research_Internet = model.Research_Internet;
                                campaignmatch.Auctions_Internet = model.Auctions_Internet;
                                campaignmatch.Shopping_Internet = model.Shopping_Internet;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileInternetId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SavePressInfo(CampaignProfilePressFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfilePressCommand command =
                        Mapper.Map<CampaignProfilePressFormModel, CreateOrUpdateCampaignProfilePressCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfilePressId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfilePressId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);


                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForPress = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForPress.Local_Press = model.Local_Press;
                                GetCampaignProfileForPress.National_Press = model.National_Press;
                                GetCampaignProfileForPress.FreeNewpapers_Press = model.FreeNewpapers_Press;
                                GetCampaignProfileForPress.Magazines_Press = model.Magazines_Press;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Local_Press = model.Local_Press;
                                campaignmatch.National_Press = model.National_Press;
                                campaignmatch.FreeNewpapers_Press = model.FreeNewpapers_Press;
                                campaignmatch.Magazines_Press = model.Magazines_Press;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfilePressId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveProductsServicesInfo(CampaignProfileProductsServiceFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileProductsServiceCommand command =
                        Mapper.Map
                            <CampaignProfileProductsServiceFormModel, CreateOrUpdateCampaignProfileProductsServiceCommand>(
                                model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileProductsServicesId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileProductsServicesId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {


                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForProductsServices = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForProductsServices.Food_ProductsService = model.Food_ProductsService;
                                GetCampaignProfileForProductsServices.SweetSaltySnacks_ProductsService = model.SweetSaltySnacks_ProductsService;
                                GetCampaignProfileForProductsServices.AlcoholicDrinks_ProductsService = model.AlcoholicDrinks_ProductsService;
                                GetCampaignProfileForProductsServices.NonAlcoholicDrinks_ProductsService = model.NonAlcoholicDrinks_ProductsService;
                                GetCampaignProfileForProductsServices.Householdproducts_ProductsService = model.Householdproducts_ProductsService;
                                GetCampaignProfileForProductsServices.ToiletriesCosmetics_ProductsService = model.ToiletriesCosmetics_ProductsService;
                                GetCampaignProfileForProductsServices.PharmaceuticalChemistsProducts_ProductsService = model.PharmaceuticalChemistsProducts_ProductsService;
                                GetCampaignProfileForProductsServices.TobaccoProducts_ProductsService = model.TobaccoProducts_ProductsService;
                                GetCampaignProfileForProductsServices.PetsPetFood_ProductsService = model.PetsPetFood_ProductsService;
                                GetCampaignProfileForProductsServices.ShoppingRetailClothing_ProductsService = model.ShoppingRetailClothing_ProductsService;
                                GetCampaignProfileForProductsServices.DIYGardening_ProductsService = model.DIYGardening_ProductsService;
                                GetCampaignProfileForProductsServices.AppliancesOtherHouseholdDurables_ProductsService = model.AppliancesOtherHouseholdDurables_ProductsService;
                                GetCampaignProfileForProductsServices.ElectronicsOtherPersonalItems_ProductsService = model.ElectronicsOtherPersonalItems_ProductsService;
                                GetCampaignProfileForProductsServices.CommunicationsInternet_ProductsService = model.CommunicationsInternet_ProductsService;
                                GetCampaignProfileForProductsServices.FinancialServices_ProductsService = model.FinancialServices_ProductsService;
                                GetCampaignProfileForProductsServices.HolidaysTravel_ProductsService = model.HolidaysTravel_ProductsService;
                                GetCampaignProfileForProductsServices.SportsLeisure_ProductsService = model.SportsLeisure_ProductsService;
                                GetCampaignProfileForProductsServices.Motoring_ProductsService = model.Motoring_ProductsService;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Food_ProductsService = model.Food_ProductsService;
                                campaignmatch.SweetSaltySnacks_ProductsService = model.SweetSaltySnacks_ProductsService;
                                campaignmatch.AlcoholicDrinks_ProductsService = model.AlcoholicDrinks_ProductsService;
                                campaignmatch.NonAlcoholicDrinks_ProductsService = model.NonAlcoholicDrinks_ProductsService;
                                campaignmatch.Householdproducts_ProductsService = model.Householdproducts_ProductsService;
                                campaignmatch.ToiletriesCosmetics_ProductsService = model.ToiletriesCosmetics_ProductsService;
                                campaignmatch.PharmaceuticalChemistsProducts_ProductsService = model.PharmaceuticalChemistsProducts_ProductsService;
                                campaignmatch.TobaccoProducts_ProductsService = model.TobaccoProducts_ProductsService;
                                campaignmatch.PetsPetFood_ProductsService = model.PetsPetFood_ProductsService;
                                campaignmatch.ShoppingRetailClothing_ProductsService = model.ShoppingRetailClothing_ProductsService;
                                campaignmatch.DIYGardening_ProductsService = model.DIYGardening_ProductsService;
                                campaignmatch.AppliancesOtherHouseholdDurables_ProductsService = model.AppliancesOtherHouseholdDurables_ProductsService;
                                campaignmatch.ElectronicsOtherPersonalItems_ProductsService = model.ElectronicsOtherPersonalItems_ProductsService;
                                campaignmatch.CommunicationsInternet_ProductsService = model.CommunicationsInternet_ProductsService;
                                campaignmatch.FinancialServices_ProductsService = model.FinancialServices_ProductsService;
                                campaignmatch.HolidaysTravel_ProductsService = model.HolidaysTravel_ProductsService;
                                campaignmatch.SportsLeisure_ProductsService = model.SportsLeisure_ProductsService;
                                campaignmatch.Motoring_ProductsService = model.Motoring_ProductsService;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }


                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileProductsServicesId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveRadioInfo(CampaignProfileRadioFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileRadioCommand command =
                        Mapper.Map<CampaignProfileRadioFormModel, CreateOrUpdateCampaignProfileRadioCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileRadioId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileRadioId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForRadio = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForRadio.National_Radio = model.National_Radio;
                                GetCampaignProfileForRadio.Local_Radio = model.Local_Radio;
                                GetCampaignProfileForRadio.Music_Radio = model.Music_Radio;
                                GetCampaignProfileForRadio.Sport_Radio = model.Sport_Radio;
                                GetCampaignProfileForRadio.Talk_Radio = model.Talk_Radio;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.National_Radio = model.National_Radio;
                                campaignmatch.Local_Radio = model.Local_Radio;
                                campaignmatch.Music_Radio = model.Music_Radio;
                                campaignmatch.Sport_Radio = model.Sport_Radio;
                                campaignmatch.Talk_Radio = model.Talk_Radio;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileRadioId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveTvInfo(CampaignProfileTvFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileTvCommand command =
                        Mapper.Map<CampaignProfileTvFormModel, CreateOrUpdateCampaignProfileTvCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileTvId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            command.CampaignProfileTvId = _campaignProfileId.Id;
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {
                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForTv = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForTv.Satallite_TV = model.Satallite_TV;
                                GetCampaignProfileForTv.Cable_TV = model.Cable_TV;
                                GetCampaignProfileForTv.Terrestrial_TV = model.Terrestrial_TV;
                                GetCampaignProfileForTv.Internet_TV = model.Internet_TV;

                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();

                                campaignmatch.Satallite_TV = model.Satallite_TV;
                                campaignmatch.Cable_TV = model.Cable_TV;
                                campaignmatch.Terrestrial_TV = model.Terrestrial_TV;
                                campaignmatch.Internet_TV = model.Internet_TV;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
                            }

                        }

                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                }

                //if fail
                if (model.CampaignProfileTvId == 0)
                    return Json("fail");

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        [HttpPost]
        public ActionResult SaveMobileInfo(CampaignProfileMobileFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

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

                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {



                            var GetCampaignProfileById = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
                            if (GetCampaignProfileById != null)
                            {
                                var GetCampaignProfileForMobile = mySQLEntities.campaignmatches.Where(s => s.CampaignProfileId == GetCampaignProfileById.CampaignProfileId).FirstOrDefault();

                                GetCampaignProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                                GetCampaignProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignmatch = new campaignmatch();
                                campaignmatch.ContractType_Mobile = model.ContractType_Mobile;
                                campaignmatch.Spend_Mobile = model.Spend_Mobile;
                                campaignmatch.MSCampaignProfileId = model.CampaignProfileId;

                                mySQLEntities.campaignmatches.Add(campaignmatch);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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

        [HttpPost]
        public ActionResult SaveTimeSettingsInfo(CampaignProfileTimeSettingFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var CampProfileId = Convert.ToInt32(Session["NewCampaignId"]);
                    model.CampaignProfileId = CampProfileId;

                    CreateOrUpdateCampaignProfileTimeSettingCommand command =
                        Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                            model);

                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                        {

                            var GetCampaignMatchProfileID = mySQLEntities.campaignmatches.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault().CampaignProfileId;
                            var GetCampaignProfileTimeByID = mySQLEntities.campaignprofiletimesettings.Where(s => s.MSCampaignProfileId == GetCampaignMatchProfileID).FirstOrDefault();
                            if (GetCampaignProfileTimeByID != null)
                            {
                                var GetCampaignProfileForTimeSettings = mySQLEntities.campaignprofiletimesettings.Where(s => s.CampaignProfileId == GetCampaignProfileTimeByID.CampaignProfileId).FirstOrDefault();


                                GetCampaignProfileForTimeSettings.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                GetCampaignProfileForTimeSettings.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                                // GetCampaignProfileForTimeSettings.MSCampaignProfileId = model.CampaignProfileId;
                                GetCampaignProfileForTimeSettings.MSCampaignProfileId = GetCampaignMatchProfileID;
                                mySQLEntities.SaveChanges();
                            }
                            else
                            {
                                var campaignprofiletimesetting = new campaignprofiletimesetting();

                                campaignprofiletimesetting.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                                campaignprofiletimesetting.MSCampaignProfileId = GetCampaignMatchProfileID;
                                mySQLEntities.campaignprofiletimesettings.Add(campaignprofiletimesetting);
                                // mySQLEntities.SaveChanges();
                                SaveChanges(mySQLEntities);
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

        #endregion
    }
}
