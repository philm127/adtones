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
using EFMVC.Data;
using Newtonsoft.Json;
using EFMVC.Domain.CountryConnectionString;
using System.Net.Mail;
using Minuco.MPLS.Common.Encryption;
using System.Net;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using EFMVC.Model.Entities;
using EFMVC.Domain.Commands.Campaign;
using EFMVC.Domain.Commands.Clients;
using MadScripterWrappers;
using System.Web.Script.Serialization;
using EFMVC.Domain.Commands.Advert;
using Aussie.ActionFilter;
using EFMVC.Domain.OperatorServerData;
using System.Reflection;
using System.Threading.Tasks;
using Adtones.Rollups.Data.DataObjects;
using Adtones.Rollups.Data.Services;
using EFMVC.Web.Areas.Admin.Controllers;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Web.Core;
using Microsoft.Ajax.Utilities;

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
        private readonly IAreaRepository _areaRepository;
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
        private readonly IFlotChartMetricsRepository _flotChartMetricsRepository;
        private readonly ISparkLineMetricsRepository _sparkLineMetricsRepository;
        private readonly ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository;

        private readonly ICacheService _cacheService;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        private readonly IContactsRepository _contactsRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ICampaignMetricsRepository _campaignMetricsRepository;
        private readonly IAdvertCategoryRepository _advertCategoryRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IProfileMatchLabelRepository _profileMatchLabelRepository;

        private readonly IAdvertRejectionRepository _advertRejectionRepository;

        private readonly IBillingRepository _billingRepository;

        private readonly IContactsRepository _contactRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly CurrencyConversion _currencyConversion;
        private readonly CampaignDashboardSummariesProvider _summariesProvider;

        private readonly StatsProvider _statsProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="advertRepository">The advert repository.</param>
        public DashboardController(ICacheService cacheService, ICommandBus commandBus, ICampaignProfileRepository profileRepository, IContactsRepository contactsRepository, IAreaRepository areaRepository,
                                  IUserRepository userRepository, IAdvertRepository advertRepository, ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository,
                                  ICampaignAuditRepository campaignAuditRepository, IClientRepository clientRepository, ICampaignAdvertRepository campaignAdvertRepository,
                                  IFlotChartMetricsRepository flotChartMetricsRepository, ISparkLineMetricsRepository sparkLineMetricsRepository, CampaignDashboardSummariesProvider summariesProvider,
                                  ICampaignMetricsRepository campaignMetricsRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository,
                                  ICurrencyRateRepository currencyRateRepository, IAdvertCategoryRepository advertCategoryRepository, IOperatorRepository operatorRepository,
                                  IProfileMatchInformationRepository profileMatchInformationRepository, IProfileMatchLabelRepository profileMatchLabelRepository, StatsProvider statsProvider,
                                  IAdvertRejectionRepository advertRejectionRepository, IBillingRepository billingRepository, IUnitOfWork unitOfWork, IContactsRepository contactRepository)
        {
            _statsProvider = statsProvider;
            _cacheService = cacheService;
            _commandBus = commandBus;
            _summariesProvider = summariesProvider;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _clientRepository = clientRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
            _campaignProfilePreferenceRepository = campaignProfilePreferenceRepository;
            _countryRepository = countryRepository;
            _contactsRepository = contactsRepository;
            _areaRepository = areaRepository;
            _currencyRepository = currencyRepository;
            _currencyRateRepository = currencyRateRepository;
            _campaignMetricsRepository = campaignMetricsRepository;
            _flotChartMetricsRepository = flotChartMetricsRepository;
            _sparkLineMetricsRepository = sparkLineMetricsRepository;
            _advertCategoryRepository = advertCategoryRepository;
            _operatorRepository = operatorRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _profileMatchLabelRepository = profileMatchLabelRepository;
            _advertRejectionRepository = advertRejectionRepository;
            _billingRepository = billingRepository;
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            _currencyConversion = CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        static int CampaignProfileID;
        static int ClientID;

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
        public async Task<ActionResult> GetCampaignData(int id)
        {

            FillCountryList();
            ViewBag.CampaignId = id;
            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();

            var Id = _profileRepository.GetById(id).CountryId;
            //var profileMatchId = _profileMatchInformationRepository.Get(top => top.CountryId == Id && top.ProfileName == "Location").Id;
            int CountryId = Convert.ToInt32(Id);
            CampaignProfileMapping _mapping = new CampaignProfileMapping();
            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(CountryId);
            //CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(profileMatchId);
            //CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel();
            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId);
            //CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel();
            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel(CountryId);
            CampaignProfileAttitudeFormModel CampaignProfileAtt = new CampaignProfileAttitudeFormModel();
            CampaignProfileCinemaFormModel Cinemapro = new CampaignProfileCinemaFormModel();
            CampaignProfileInternetFormModel Internetpro = new CampaignProfileInternetFormModel();
            //CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel();
            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel(CountryId);
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
            string currencyId;
            string currencyCode;
            currencyId = _contactsRepository.GetAll().Where(top => top.UserId == user.UserId).Select(top => top.CurrencyId).FirstOrDefault().ToString();
            currencyCode = _currencyRepository.GetAll().Where(top => top.CurrencyId == Convert.ToInt32(currencyId)).Select(top => top.CurrencyCode).FirstOrDefault();
            //ViewBag.Country = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Name; //ConfigurationManager.AppSettings["Country"];
            ViewBag.Country = _countryRepository.GetById(CountryId).Name; //ConfigurationManager.AppSettings["Country"];
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
                var model = await GetEditData(id, efmvcUser.UserId);
                ViewBag.AdvertClientId = model.ClientId;
                _mapping.AdvertFormModel = advertFormModels();
                _mapping.CampaignAudit = campaignAudit(id);
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                ViewBag.ClientId = model.ClientId;
                ViewBag.CampaignProfileId = model.CampaignProfileId;
                if (model != null)
                    //fill chart data
                    CampaignDashboardChartResult = await FillChartDataByCampaignId(model);
                CampaignDashboardChartResult.CurrencyCode = currencyCode;
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
            FillCountryList();
            FillCurrencyList();
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
                CurrencyModel currencyModel = new CurrencyModel();
                model.Status = (int)CampaignStatus.waitingforapproval;
                if (model.StartDate == null && model.EndDate != null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign start date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate == null)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillAddClient(clientModels);
                    TempData["Error"] = "Please provide campaign end date.";
                    return View("Initialise", model);
                }
                if (model.StartDate != null && model.EndDate != null)
                {
                    if (model.EndDate.Value.Date < model.StartDate.Value.Date)
                    {
                        var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                        IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                        FillAddClient(clientModels);
                        TempData["Error"] = "EndDate must be greater than StartDate.";
                        return View("Initialise", model);
                    }
                }
                var CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == model.CampaignName && c.UserId == efmvcUser.UserId).ToList();
                if (CampaignNameexists.Count() > 0)
                {
                    var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                    IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                    FillCountryList();
                    FillAddClient(clientModels);
                    FillAddCampaign(efmvcUser.UserId);
                    TempData["Error"] = model.CampaignName + " already Exists.";
                    return View("Initialise", model);
                }
                model.Active = true;
                model.TotalBudget = 0;
                model.CampaignProfileAttitudes = new Collection<CampaignProfileAttitudeFormModel> { new CampaignProfileAttitudeFormModel() };
                CreateOrUpdateCampaignProfileCommand command = Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(model);
                command.CampaignProfileAdverts = Mapper.Map<ICollection<CampaignProfileAdvertFormModel>, ICollection<CreateOrUpdateCampaignProfileAdvertCommand>>(model.CampaignProfileAdverts ?? new Collection<CampaignProfileAdvertFormModel> { new CampaignProfileAdvertFormModel(model.CountryId) });
                command.CampaignProfileAttitudes = Mapper.Map<ICollection<CampaignProfileAttitudeFormModel>, ICollection<CreateOrUpdateCampaignProfileAttitudeCommand>>(model.CampaignProfileAttitudes);
                command.CampaignProfileCinemas = Mapper.Map<ICollection<CampaignProfileCinemaFormModel>, ICollection<CreateOrUpdateCampaignProfileCinemaCommand>>(model.CampaignProfileCinemas);
                command.CampaignProfileInternets = Mapper.Map<ICollection<CampaignProfileInternetFormModel>, ICollection<CreateOrUpdateCampaignProfileInternetCommand>>(model.CampaignProfileInternets);
                command.CampaignProfileMobiles = Mapper.Map<ICollection<CampaignProfileMobileFormModel>, ICollection<CreateOrUpdateCampaignProfileMobileCommand>>(model.CampaignProfileMobiles);
                command.CampaignProfilePresses = Mapper.Map<ICollection<CampaignProfilePressFormModel>, ICollection<CreateOrUpdateCampaignProfilePressCommand>>(model.CampaignProfilePresses);
                command.CampaignProfileProductsServices = Mapper.Map<ICollection<CampaignProfileProductsServiceFormModel>, ICollection<CreateOrUpdateCampaignProfileProductsServiceCommand>>(model.CampaignProfileProductsServices);
                command.CampaignProfileRadios = Mapper.Map<ICollection<CampaignProfileRadioFormModel>, ICollection<CreateOrUpdateCampaignProfileRadioCommand>>(model.CampaignProfileRadios);
                command.CampaignProfileTimeSettings = Mapper.Map<ICollection<CampaignProfileTimeSettingFormModel>, ICollection<CreateOrUpdateCampaignProfileTimeSettingCommand>>(model.CampaignProfileTimeSettings);
                command.CampaignProfileTvs = Mapper.Map<ICollection<CampaignProfileTvFormModel>, ICollection<CreateOrUpdateCampaignProfileTvCommand>>(model.CampaignProfileTvs);
                command.CampaignProfileDemographics = Mapper.Map<ICollection<CampaignProfileDemographicsFormModel>, ICollection<CreateOrUpdateCampaignProfileDemographicsCommand>>(model.CampaignProfileDemographicsFormModels ?? new Collection<CampaignProfileDemographicsFormModel> { new CampaignProfileDemographicsFormModel(model.CountryId) });
                command.UserId = efmvcUser.UserId;
                command.NumberInBatch = model.NumberInBatch;
                command.CountryId = model.CountryId;
                decimal currencyRate = 0.00M;
                var currencyCode = _currencyRepository.Get(c => c.CountryId == model.CountryId).CurrencyCode;
                var currencyData = _currencyRepository.Get(c => c.CurrencyId == model.CurrencyId);
                var currencyCountryId = currencyData.Country.Id;
                var fromCurrencyCode = currencyData.CurrencyCode;
                var toCurrencyCode = _currencyRepository.Get(c => c.CountryId == model.CountryId).CurrencyCode;
                if (currencyCountryId == model.CountryId)
                {
                    command.MaxDailyBudget = model.MaxDailyBudget;
                    command.TotalBudget = model.TotalBudget;
                    command.MaxBid = model.MaxBid;
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        double MaxDailyBudget = Convert.ToDouble(Convert.ToDecimal(model.MaxDailyBudget) * currencyRate);
                        double TotalBudget = Convert.ToDouble(Convert.ToDecimal(model.TotalBudget) * currencyRate);
                        double MaxBid = Convert.ToDouble(Convert.ToDecimal(model.MaxBid) * currencyRate);
                        command.MaxDailyBudget = float.Parse(MaxDailyBudget.ToString());
                        command.TotalBudget = decimal.Parse(TotalBudget.ToString());
                        command.MaxBid = float.Parse(MaxBid.ToString());
                    }
                }
                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == result.Id).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                }
                            }
                        }
                        var userDetails = _userRepository.GetById(efmvcUser.UserId);
                        //Email Code
                        var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
                        if (adminDetails != null)
                        {
                            var reader = new StreamReader(Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
                            var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
                            string emailContent = reader.ReadToEnd();
                            emailContent = string.Format(emailContent, model.CampaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);
                            MailMessage mail = new MailMessage();
                            mail.To.Add(adminDetails.Email);
                            mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                            mail.Subject = "Campaign Verification";
                            mail.Body = emailContent.Replace("\n", "<br/>");
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                            smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);
                            //Or your Smtp Email ID and Password
                            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                            smtp.Send(mail);
                        }
                        TempData["campaignId"] = result.Id;
                        return RedirectToAction("Campaign", new { id = result.Id });
                    }
                }
            }
            //if fail
            if (model.CampaignProfileId == 0) return View("Initialise", model);
            return View("Initialise", model);
        }

        private void FillCountryList()
        {
            //Add 26-04-2019
            var countryId = _operatorRepository.GetMany(s => s.IsActive).Select(c => c.CountryId).ToList();
            var country = (from action in _countryRepository.GetMany(c => countryId.Contains(c.Id)).OrderBy(c => c.Name)
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();

            //Comment 26-04-2019
            //var country = (from action in _countryRepository.GetAll().OrderBy(c => c.Name)
            //               select new SelectListItem
            //               {
            //                   Text = action.Name,
            //                   Value = action.Id.ToString()
            //               }).ToList();

            ViewBag.countryList = country;
        }

        public async Task<ActionResult> Campaign(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            FillCountryList();
            Session["CampaignId"] = id;
            ViewBag.CampaignId = id;
            FillCampaignAuditStatus();
            FillCampaignAuditSMSStatus();
            CampaignProfileMapping _mapping = new CampaignProfileMapping();
            var campaignProfileResult = await _profileRepository.AsQueryable().FirstOrDefaultAsync(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId && (x.Status != 5));
            if(campaignProfileResult == null) 
                return RedirectToAction("Index");
            int CountryId = campaignProfileResult.CountryId ?? -1;
            FillCurrencyList();
            Session["CountryId"] = CountryId;
            Session["CurrencyId"] = 0;
            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(CountryId);
            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId);
            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel(CountryId);
            CampaignProfileSkizaFormModel CampaignProfileSkizaDetail = new CampaignProfileSkizaFormModel(CountryId);
            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel(CountryId);
            CampaignProfileTimeSettingFormModel CampaignProfileTimeSetting = new CampaignProfileTimeSettingFormModel();
            CampaignAuditFilter CampaignAuditFilter = new CampaignAuditFilter();
            CampaignDashboardChartResult CampaignDashboardChartResult = new CampaignDashboardChartResult();
            CampaignAuditFilter.CampaignProfileId = id;
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
            IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            
            ViewBag.Country = _countryRepository.GetById(CountryId).Name;
            if (id != 0)
            {
                CampaignProfileGeographicModel = CampaignProfileGeographic(id, CampaignProfileGeographicModel);
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
                var model = await GetEditData(id, efmvcUser.UserId);
                _mapping.AdvertFormModel = advertFormModels();
                _mapping.CampaignAuditFilter = CampaignAuditFilter;
                if (model != null)
                {
                    ViewBag.AdvertClientId = model.ClientId;
                    ViewBag.ClientId = model.ClientId;
                    ViewBag.CampaignProfileId = model.CampaignProfileId;
                    CampaignDashboardChartResult = await FillChartDataByCampaignId(model);
                    ProfileGeographicOptions(_mapping.CampaignProfileGeographicModel, model.CountryId);
                    ProfileDemographicsOptions(_mapping.CampaignProfileDemographicsmodel, model.CountryId);
                    ProfileAdvertOptions(_mapping.CampaignProfileAd, model.CountryId);
                    ProfileMobileFormOptions(_mapping.CampaignProfileMobileFormModel, model.CountryId);
                }
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

        private List<CampaignAuditResult> campaignAudit(int id)
        {
            double totalcredit = 0;
            double spendtodate = 0;
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
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId && (x.Status != 5)) > 0)
            {
                CampaignProfile profile = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == id);
                totalcredit = (double)profile.TotalCredit;
                IEnumerable<CampaignAuditFormModel> model = Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits.OrderByDescending(top=>top.CampaignAuditId).ToList());
                var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                string currencysymbol = "";
                string currencyCode = "";
                var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
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
                    EFMVCDataContex db = new EFMVCDataContex();
                    var userDetails = db.Userprofiles.Where(top => top.UserProfileId == item.UserProfileId).ToList();
                    if (userDetails.Count() > 0)
                    {
                        decimal currencyRate = 0.00M;
                        string fromCurrencyCode = _profileRepository.GetById(item.CampaignProfileId).CurrencyCode;
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
                            playcostamount = Convert.ToDouble(Convert.ToDecimal(playcost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
                            emailcostamount = Convert.ToDouble(Convert.ToDecimal(emailcost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
                            smscostamount = Convert.ToDouble(Convert.ToDecimal(smscost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
                            totalcostamount = Convert.ToDouble(Convert.ToDecimal(item.TotalCost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
                            playcostamount = Math.Round(playcostamount, 2, MidpointRounding.AwayFromZero);
                            emailcostamount = Math.Round(emailcostamount, 2, MidpointRounding.AwayFromZero);
                            smscostamount = Math.Round(smscostamount, 2, MidpointRounding.AwayFromZero);
                            totalcostamount = Math.Round(totalcostamount, 2, MidpointRounding.AwayFromZero);
                        }
                        var lengthOfPlay = Convert.ToDouble(item.PlayLengthTicks) / 1000;
                        _audit.Add(new CampaignAuditResult { PlayID = item.CampaignAuditId, UserID = userDetails.FirstOrDefault().UserId, StartDate = item.StartTime.ToString("MM/dd/yyyy hh:mm:ss"), EndDate = item.EndTime.ToString("MM/dd/yyyy hh:mm:ss"), LengthOfPlay = RoundUp(lengthOfPlay, 2), AdvertName = advertname, AdvertId = advertid, Status = item.Status, PlayCost = RoundUp(playcostamount, 2), SMS = item.SMS == null ? "-" : smsstatus, SMSCost = smscostamount, Email = item.Email == null ? "-" : emailstatus, EmailCost = emailcostamount, TotalCost = RoundUp(totalcostamount, 2), DisplayStartDate = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayStartDateSort = item.StartTime, DisplayEndDate = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss tt"), DisplayEndDateSort = item.EndTime, CurrencyCode = currencysymbol, CountryId = userCountryId, CurrencyId = userCurrencyId });
                    }
                }
            }
            return _audit;
        }

        private IEnumerable<AdvertFormModel> advertFormModels()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            IEnumerable<Advert> adverts = _advertRepository.GetMany(x => x.UserId == efmvcUser.UserId && x.Status != 4);
            IEnumerable<AdvertFormModel> advertFormModels =
                Mapper.Map<IEnumerable<Advert>, IEnumerable<AdvertFormModel>>(adverts);
            return advertFormModels;
        }

        // Update Ads 
        public ActionResult UpdateMedia(int AdvertId, int CampaignProfileId, int CampaignAdvertID)
        {
            CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
            _campaignAdvert.CampaignAdvertId = CampaignAdvertID;
            _campaignAdvert.AdvertId = AdvertId;
            _campaignAdvert.CampaignProfileId = CampaignProfileId;
            CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
            Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

            ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);
            EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
            string adName = null;
            adName = SQLServerEntities.Adverts.Where(s => s.AdvertId == AdvertId).FirstOrDefault().MediaFileLocation;
            if (!string.IsNullOrEmpty(adName))
            {
                var operatorId = _advertRepository.GetById(AdvertId).OperatorId;
                var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)operatorId).FirstOrDefault();
                adName = operatorFTPDetails.FtpRoot + "/" + adName.Split('/')[3];
                //adName = "/usr/local/arthar/adds/" + adName.Split('/')[3];
            }
            //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
            //UserMatchTableProcess obj = new UserMatchTableProcess();
            //obj.UpdateCampaignAd(CampaignProfileId, adName, SQLServerEntities);
            //PreMatchProcess.PrematchProcessForCampaign(CampaignProfileId, conn);

            var CountryID = _profileRepository.Get(x => (x.Status != 5) && x.CampaignProfileId == CampaignProfileId).CountryId;
            var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryID);
            if (ConnString != null && ConnString.Count() > 0)
            {
                UserMatchTableProcess obj = new UserMatchTableProcess();
                foreach (var item in ConnString)
                {
                    SQLServerEntities = new EFMVCDataContex(item);
                    var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileId).FirstOrDefault();
                    if (campaigndetailFromOP != null)
                    {
                        obj.UpdateCampaignAd(campaigndetailFromOP.CampaignProfileId, adName, SQLServerEntities);
                        PreMatchProcess.PrematchProcessForCampaign(campaigndetailFromOP.CampaignProfileId, item);
                    }

                }
            }

            if (campaignAdvertcommandResult.Success)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddMedia(int AdvertId, int CampaignProfileId)
        {
            if (CampaignProfileId == 0)
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
                        if (campaignProfileAdvert.CountryId == 0)
                        {
                            campaignProfileAdvert.CountryId = CountryId;
                        }
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
                        CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = campaignProfileskiza.Id, CountryId = CountryId };
                    }
                    else
                    {
                        if (campaignProfileskiza.CountryId == 0)
                        {
                            campaignProfileskiza.CountryId = CountryId;
                        }
                        CampaignSkiza =
                            Mapper.Map<CampaignProfilePreference, CampaignProfileSkizaFormModel>(campaignProfileskiza);
                        CampaignSkiza.CampaignProfileSKizaId = campaignProfileskiza.Id;
                        CampaignSkiza.CampaignProfileId = id;
                        //CampaignSkiza.CountryId = CountryId;
                    }

                }
                else
                {
                    //CampaignSkiza = new CampaignProfileSkizaFormModel { CampaignProfileId = id, CampaignProfileSKizaId = 0 };
                    CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = 0, CountryId = CountryId };

                }
            }
            else
            {
                //CampaignSkiza = new CampaignProfileSkizaFormModel { CampaignProfileId = id, CampaignProfileSKizaId = 0 };
                CampaignSkiza = new CampaignProfileSkizaFormModel(CountryId) { CampaignProfileId = id, CampaignProfileSKizaId = 0, CountryId = CountryId };
            }
            return CampaignSkiza;
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
                        if (campaignProfileDemographics.CountryId == 0)
                        {
                            campaignProfileDemographics.CountryId = CountryId;
                        }
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
                        if (campaignProfileMobile.CountryId == 0)
                        {
                            campaignProfileMobile.CountryId = CountryId;
                        }
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
                //var countryId = _profileRepository.GetById(id).CountryId;
                //var profileMatchId = _profileMatchInformationRepository.Get(top => top.CountryId == countryId && top.ProfileName == "Location").Id;

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
                        if (CampaignProfileGeograph.CountryId == 0)
                        {
                            CampaignProfileGeograph.CountryId = CountryId;
                        }
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

        private async Task<CampaignProfileFormModel> GetEditData(int id, int userId)
        {
            //EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            //CurrencyModel currencyModel = new CurrencyModel();
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == userId && (x.Status != 5)) == 0) return null;
            CampaignProfile campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == id);
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
            //DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            //DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59);
            //DateTime previousMonthStart;
            //DateTime previousMonthEnd;
            //if (DateTime.Now.Month == 1)
            //{
            //    previousMonthStart = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
            //    previousMonthEnd = new DateTime(DateTime.Now.Year - 1, 12, DateTime.DaysInMonth(DateTime.Now.Year - 1, 12), 23, 59, 59);
            //}
            //else
            //{
            //    previousMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0);
            //    previousMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1), 23, 59, 59);
            //}

            //var relatedAudit = model.GetDomainCampaignAudits(_campaignAuditRepository);
            //int currentMonthPlayCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd);
            //int previousMonthPlayCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd);
            //int previousMonthSMSCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.SMS == "1");
            //int previousMonthEmailCount = relatedAudit.Count(x => x.StartTime >= previousMonthStart && x.StartTime <= previousMonthEnd && x.Email == "1");
            //int currentMonthSMSCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.SMS == "1");
            //int currentMonthEmailCount = relatedAudit.Count(x => x.StartTime >= currentMonthStart && x.StartTime <= currentMonthEnd && x.Email == "1");
            //int totalPlayCount = relatedAudit.Count();
            //int totalSMSCount = relatedAudit.Count(x => x.SMS == "1");
            //int totalEmailCount = relatedAudit.Count(x => x.Email == "1");
            //var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            //string currencyCode = "";
            //var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
            //if (userCurrencyId != null || userCurrencyId != 0)
            //{
            //    currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            //}
            //else
            //{
            //    currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            //}
            //double playcostdata = 0.00;
            //double emailcostdata = 0.00;
            //double smscostdata = 0.00;
            //double totalcost = 0.00;
            //double totalcredit = 0;
            //double spendtodate = 0;
            //double playcost = 0;
            //double emailcost = 0;
            //double smscost = 0;
            string fromCurrencyCode = _profileRepository.GetById(id).CurrencyCode;
            string toCurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode;
            //foreach (var item in campaignProfile.CampaignAudits)
            //{
            //    if (toCurrencyCode == fromCurrencyCode)
            //    {
            //        playcostdata = Convert.ToDouble(item.BidValue.ToString());
            //        emailcostdata = Convert.ToDouble(item.EmailCost.ToString());
            //        smscostdata = Convert.ToDouble(item.SMSCost.ToString());
            //        totalcost = Convert.ToDouble(item.TotalCost.ToString());
            //        playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
            //        emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
            //        smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
            //    }
            //    else
            //    {
            //        playcostdata = Convert.ToDouble(Convert.ToDecimal(item.BidValue).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
            //        emailcostdata = Convert.ToDouble(Convert.ToDecimal(item.EmailCost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
            //        smscostdata = Convert.ToDouble(Convert.ToDecimal(item.SMSCost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
            //        totalcost = Convert.ToDouble(Convert.ToDecimal(item.TotalCost).ConvertToDisplay(_currencyConversion, fromCurrencyCode));
            //        playcost = playcost + Convert.ToDouble(playcostdata.ToString("F2"));
            //        emailcost = emailcost + Convert.ToDouble(emailcostdata.ToString("F2"));
            //        smscost = smscost + Convert.ToDouble(smscostdata.ToString("F2"));
            //    }
            //}
            //ViewData.Add("previousMonthPlayCount", previousMonthPlayCount);
            //ViewData.Add("previousMonthSMSCount", previousMonthSMSCount);
            //ViewData.Add("previousMonthEmailCount", previousMonthEmailCount);
            //ViewData.Add("currentMonthSMSCount", currentMonthSMSCount);
            //ViewData.Add("currentMonthEmailCount", currentMonthEmailCount);
            //ViewData.Add("totalPlayCount", totalPlayCount);
            //ViewData.Add("currentMonthPlayCount", currentMonthPlayCount);
            //ViewData.Add("totalSMSCount", totalSMSCount);
            //ViewData.Add("totalEmailCount", totalEmailCount);
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
            //totalcredit = (double)campaignProfile.TotalCredit;
            //spendtodate = playcost + emailcost + smscost;
            //ViewData["maxspendtodate"] = RoundUp(spendtodate, 2);
            //ViewData["maxremainding"] = RoundUp((totalcredit - spendtodate), 2);
            ViewData.Add("statuscheck", campaignProfile.Status);
            if (!String.IsNullOrEmpty(campaignProfile.EmailFileLocation)) ViewData.Add("emailfilelocation", ConfigurationManager.AppSettings["siteAddress"].ToString() + campaignProfile.EmailFileLocation);
            else ViewData.Add("emailfilelocation", String.Empty);
            if (!String.IsNullOrEmpty(campaignProfile.SMSFileLocation)) ViewData.Add("smsfilelocation", ConfigurationManager.AppSettings["siteAddress"].ToString() + campaignProfile.SMSFileLocation);
            else ViewData.Add("smsfilelocation", String.Empty);
            ViewBag.campaignName = campaignProfile.CampaignName;
            return model;
        }

        public async Task FillClientDropdown(int? clientId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var clientdetails = await _clientRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && (top.Status == 1 || top.Status == 2)).Select(top => new
            {
                Name = top.Name,
                Id = top.Id.ToString(),

            }).ToListAsync();
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
            //ViewBag.compaignStatus = new MultiSelectList(compaignStatus, "Value", "Text", new[] { 1, 2, 3, 4, 6, 7 });
            ViewBag.compaignStatus = new MultiSelectList(compaignStatus, "Value", "Text");
        }

        public async Task FillAdvert()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var advert = await (from action in _advertRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && (top.Status != 4 && top.Status != 5))
                          select new
                          {
                              AdvertName = action.AdvertName,
                              AdvertId = action.AdvertId
                          }).ToListAsync();


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

        public async Task<ActionResult> Index(int? clientid, int? advertid, string campaignName)
        {
            Session["NewCampaignId"] = null;
            Session["mintotalspend"] = null;
            Session["minfinaltotalplays"] = null;
            Session["mintotalaveragebid"] = null;
            Session["maxtotalspend"] = null;
            Session["maxfinaltotalplays"] = null;
            Session["maxtotalaveragebid"] = null;

            List<CampaignProfileResult> _result = new List<CampaignProfileResult>();
            CampaignProfileResult campaignProfileResult = new CampaignProfileResult();
            await FillClientDropdown(clientid);
            await FillAdvert();
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

            User user = await _userRepository.AsQueryable().FirstOrDefaultAsync(u=>u.UserId == efmvcUser.UserId);

            IEnumerable<CampaignProfile> campaignProfiles = await _profileRepository.AsQueryable().Where(x => x.UserId == efmvcUser.UserId && (x.Status != 5)).ToListAsync();

            Session["UserId"] = efmvcUser.UserId;

            //IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
            //    Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);

            //Fill ChartData
            // CampaignDashboardChartResult _CampaignDashboardChartResult = FillChartData(campaignProfiles, null); 
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();

            List<CampaignProfileFormModel> campaignProfileFormModels = null;

            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

            //FillCampaignDropdown(campaignProfileFormModels);
            //if (campaignProfileFormModels.Count() > 0)
            //{
            //var campaign = (from action in campaignProfileFormModels
            //                select new
            //                {
            //                    CampaignName = action.CampaignName,
            //                    CampaignProfileId = action.CampaignProfileId
            //                }).ToList();

            var campaign = campaignProfiles.Select(top => new
            {
                CampaignName = top.CampaignName,
                CampaignProfileId = top.CampaignProfileId
            }).ToList();

            TempData["campaignDetails"] = campaign;
            ViewBag.compaign = new MultiSelectList(campaign, "CampaignProfileId", "CampaignName");
            //}

            if (clientid != null)
            {
                // IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status != 5));
                campaignProfileFormModels =
                   Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles).Where(top => top.ClientId == clientid).ToList();
            }

            if (campaignProfileFormModels != null)
            {
                if (campaignProfileFormModels.Count > 0)
                {
                    int[] CampaingStatusId = new int[] { 1, 2, 3, 4, 6 };
                    campaignProfileFormModels = campaignProfileFormModels.Where(top => CampaingStatusId.Contains(top.Status)).ToList();
                    //CampainResult(_result, campaignProfileFormModels, null, null, null, null); 
                    //if (advertid != null)
                    //{
                    //    // _result = _result.Where(top => top.AdvertId == advertid).ToList(); 
                    //}
                    TempData["compainresult"] = _result;
                }
            }
            ViewData["User"] = userFormModel;
            ViewBag.campaignProfileFormModels = campaignProfileFormModels;
            ViewBag.FreePlays = 0;
            ViewBag.TotalPlayed = 0;
            ViewBag.TotalBudget = 0;
            ViewBag.TotalSpend = 0;
            ViewBag.MaxBid = 0;
            ViewBag.AvgMaxBid = 0;
            ViewBag.NoofplayMaxCount = 0;
            ViewBag.AvgbidMaxCount = 0;
            _CampaignDashboardChartResult.CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode;
            campaignProfileResult.CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode;
            _result.Add(campaignProfileResult);

            if(campaignName != null && campaignName != "")
            {
                TempData["msgsuccess"] = "Campaign " + campaignName + " added successfully.";
            }
            else
            {
                TempData["msgsuccess"] = null;
            }

            return View(Tuple.Create(_result, _filterCritearea, _CampaignDashboardChartResult));
        }

        public async Task<JsonResult> LoadDataAll(DTParameters param)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();

            var consolidatedStats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Advertiser,
                efmvcUser.UserId, _currencyConversion);

            //List<CampaignDashboardSummaries> summaries = await _summariesProvider.GetCampaignDashboardSummariesForUser(efmvcUser.UserId, null);

            var campaigns = await _profileRepository.AsQueryable().Where(c => c.UserId == efmvcUser.UserId)
                .GroupJoin(_campaignAdvertRepository.AsQueryable(), c=>c.CampaignProfileId, cad=>cad.CampaignProfileId, (c,cad)=> new {Campaign=c, Cad = cad.DefaultIfEmpty()})
                .GroupJoin(_advertRepository.AsQueryable(), j=>j.Cad.FirstOrDefault().AdvertId, a=>a.AdvertId, (j, a)=>new {j.Campaign, Advert =a})            
                .ToListAsync();
            var joined = campaigns.GroupJoin(consolidatedStats.Dashboard, s => s.Campaign.CampaignProfileId, c => c.CampaignId,
                (c, s) => 
                    new
                    {
                        Campaign = c.Campaign, 
                        Summary = s.FirstOrDefault() ?? 
                                  new DashboardSummariesDao
                                  {
                                      Budget = c.Campaign.TotalBudget, 
                                      FundsAvailable = c.Campaign.TotalBudget, 
                                      AvgBid = Convert.ToDecimal(c.Campaign.MaxBid)
                                  },
                        Advert=c.Advert.FirstOrDefault(),

                    }).ToList();


            var resultUserMatches =  await _statsProvider.GetCampaignUserMatchCountAsync(joined.Select(x => x.Campaign.CampaignProfileId).ToArray());

            var data = joined.Select(item => new CampaignProfileResult
            {
                CampaignName = item.Campaign.CampaignName,
                CampaignProfileId = item.Campaign.CampaignProfileId,
                Status = item.Campaign.Status,
                TotalBudget = item.Summary.Budget,
                totalaveragebid = item.Summary.AvgBid,
                totalspend = item.Summary.Spend,
                finaltotalplays = (int)item.Summary.MoreSixSecPlays,
                advertname = item.Advert?.AdvertName ?? "-",
                AdvertId = item.Advert?.AdvertId ?? 0,
                FundsAvailable = item.Summary.FundsAvailable,
                ClientName = item.Campaign.ClientId == 0 ? "-" : item.Campaign.ClientId == null ? "-" : item.Campaign.Client.Name,
                ClientId = item.Campaign.ClientId,
                IsAdminApproval = item.Campaign.IsAdminApproval,
                CurrencyCode = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode),
                CountryId = _currencyConversion.DisplayCurrency.CountryId,
                Reach = (int)item.Summary.Reach,                
                CurrencyId = _currencyConversion.DisplayCurrency.CurrencyId,
                UserMatchedStatus = resultUserMatches.FirstOrDefault(x => x.CampaignProfileId == item.Campaign.CampaignProfileId)?.MatchedUsers > 0 ? 1 : 0,
                NumUsersMatched = resultUserMatches.FirstOrDefault(x => x.CampaignProfileId == item.Campaign.CampaignProfileId)?.MatchedUsers ?? 0,
            }).ToList();

            DTResult<CampaignProfileResult> result = new DTResult<CampaignProfileResult>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = data.Count,
                recordsTotal = data.Count
            };
            return Json(result);
        }

        public class CampaignAuditListParams
        {
            public int CampaignProfileId { get; set; }
            public string UserId { get; set; }
            public string FromPlayLength { get; set; }
            public string ToPlayLength { get; set; }
            public string FromPlayCost { get; set; }
            public string ToPlayCost { get; set; }
            public string FromTotalCost { get; set; }
            public string ToTotalCost { get; set; }
            public string FromStartTime { get; set; }
            public string ToStartTime { get; set; }
            public string FromEndTime { get; set; }
            public string ToEndTime { get; set; }
            public string SmsFilter { get; set; }
            public int TimeZoneOffsetMinutes { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> SearchAuditNew(DTParameters<CampaignAuditListParams> param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CurrencySymbol currencySymbol = new CurrencySymbol();

                if (param.ExtendedParameters?.CampaignProfileId == null)
                    return Json(new DTResult<CampaignAuditResultTR>
                    {
                        draw = param.Draw,
                        data = new List<CampaignAuditResultTR>(),
                        recordsFiltered = 0,
                        recordsTotal = 0,
                    });


                using (EFMVCDataContex db = new EFMVCDataContex())
                {
                    var campaignProfile = await db.CampaignProfiles.FirstAsync(c =>
                        c.CampaignProfileId == param.ExtendedParameters.CampaignProfileId);
                    var currencyId =
                        await _currencyRepository.GetCurrencyUsingCountryIdAsync(campaignProfile.CountryId);

                    var audit = db.CampaignAudits
                        .Where(ca => ca.CampaignProfileId == campaignProfile.CampaignProfileId)
                        .Join(db.CampaignProfiles, ca => ca.CampaignProfileId, cp => cp.CampaignProfileId,
                            (ca, cp) => new {Audit = ca, Campaign = cp})
                        .Join(db.Userprofiles.Include(nameof(UserProfile.User)), joined => joined.Audit.UserProfileId,
                            up => up.UserProfileId,
                            (joined, up) => new {Audit = joined.Audit, Campaign = joined.Campaign, User = up.User})
                        .Join(db.CampaignAdverts, joined => joined.Audit.CampaignProfileId,
                            cad => cad.CampaignProfileId,
                            (joined, cad) => new {joined.Audit, joined.User, joined.Campaign, Cad = cad})
                        .Join(db.Adverts, joined => joined.Cad.AdvertId, a => a.AdvertId,
                            (joined, a) => new {joined.Audit, joined.User, joined.Campaign, Advert = a})
                        .Select(joined => new CampaignAuditResultTR
                        {
                            CurrencyCode = joined.Campaign.CurrencyCode,
                            TotalCost = joined.Audit.TotalCost,
                            CurrencyId = currencyId.CurrencyId,
                            AdvertName = joined.Advert.AdvertName,
                            PlayID = joined.Audit.CampaignAuditId,
                            Email = joined.Audit.Email != null ? "yes" : "-",
                            EmailCost = joined.Audit.EmailCost,
                            SMS = joined.Audit.SMS != null ? "yes" : "-",
                            SMSCost = joined.Audit.SMSCost,
                            PlayCost = joined.Audit.BidValue,
                            UserID = joined.User.UserId,
                            LengthOfPlay = joined.Audit.PlayLengthTicks,
                            StartDate = joined.Audit.StartTime,
                            EndDate = joined.Audit.EndTime,
                        });

                    var totalCount = await audit.CountAsync();

                    audit = SetFilter(param.ExtendedParameters, audit, campaignProfile.CountryId);

                    audit = SetAuditSorting(param, audit);

                    var filteredCount = await audit.CountAsync();
                    var data = (await audit.Skip(param.Start).Take(param.Length).ToListAsync()).ToList();

                    if(_currencyConversion.DisplayCurrency.CurrencyCode != campaignProfile.CurrencyCode)
                    { 
                        data.ForEach(d=>{
                            d.CurrencyCode = currencySymbol.GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode);
                            d.CurrencyId = _currencyConversion.DisplayCurrency.CurrencyId;
                            d.TotalCost = Convert.ToDouble(_currencyConversion.ConvertToDisplayCurrency(Convert.ToDecimal(d.TotalCost), campaignProfile.CurrencyCode));
                            d.PlayCost = Convert.ToDouble(_currencyConversion.ConvertToDisplayCurrency(Convert.ToDecimal(d.PlayCost), campaignProfile.CurrencyCode));
                            d.SMSCost = Convert.ToDouble(_currencyConversion.ConvertToDisplayCurrency(Convert.ToDecimal(d.SMSCost), campaignProfile.CurrencyCode));
                            d.EmailCost = Convert.ToDouble(_currencyConversion.ConvertToDisplayCurrency(Convert.ToDecimal(d.EmailCost), campaignProfile.CurrencyCode));
                        });
                    }
                    DTResult<CampaignAuditResultTR> result = new DTResult<CampaignAuditResultTR>
                    {
                        draw = param.Draw,
                        data = data,
                        recordsFiltered = filteredCount,
                        recordsTotal = totalCount,
                    };
                    var json = Json(result);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
            }
            catch (Exception e)
            {
                return Json(new {error = e.Message});
            }
        }

        private IQueryable<CampaignAuditResultTR> SetAuditSorting(DTParameters<CampaignAuditListParams> param, IQueryable<CampaignAuditResultTR> audit)
        {
            if (param.Order != null && param.Order.Length > 0)
            {
                foreach (DTOrder order in param.Order)
                {
                    var col = param.Columns[order.Column];
                    string fieldName = col.Data;
                    switch (fieldName)
                    {
                        case nameof(CampaignAuditResultTR.LengthOfPlay):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.LengthOfPlay)
                                : audit.OrderBy(u => u.LengthOfPlay);
                            break;
                        case nameof(CampaignAuditResultTR.AdvertName):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.AdvertName)
                                : audit.OrderBy(u => u.AdvertName);
                            break;
                        case nameof(CampaignAuditResultTR.DisplayStartDate):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.StartDate)
                                : audit.OrderBy(u => u.StartDate);
                            break;
                        case nameof(CampaignAuditResultTR.DisplayEndDate):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.EndDate)
                                : audit.OrderBy(u => u.EndDate);
                            break;
                        case nameof(CampaignAuditResultTR.UserID):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.UserID)
                                : audit.OrderBy(u => u.UserID);
                            break;
                        case nameof(CampaignAuditResultTR.PlayCost):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.PlayCost)
                                : audit.OrderBy(u => u.PlayCost);
                            break;
                        case nameof(CampaignAuditResultTR.TotalCost):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.TotalCost)
                                : audit.OrderBy(u => u.TotalCost);
                            break;
                        case nameof(CampaignAuditResultTR.EmailCost):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.EmailCost)
                                : audit.OrderBy(u => u.EmailCost);
                            break;
                        case nameof(CampaignAuditResultTR.SMSCost):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.SMSCost)
                                : audit.OrderBy(u => u.SMSCost);
                            break;
                        case nameof(CampaignAuditResultTR.Email):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.Email)
                                : audit.OrderBy(u => u.Email);
                            break;
                        case nameof(CampaignAuditResultTR.SMS):
                            audit = order.Dir == DTOrderDir.DESC
                                ? audit.OrderByDescending(u => u.SMS)
                                : audit.OrderBy(u => u.SMS);
                            break;
                        default:
                            audit = audit.OrderByDescending(a => a.PlayID);
                            break;
                    }

                    break;
                }
            }
            else
                audit = audit.OrderByDescending(a => a.PlayID);

            return audit;
        }

        private IQueryable<CampaignAuditResultTR> SetFilter(CampaignAuditListParams p, IQueryable<CampaignAuditResultTR> source, int? itemCountryId)
        {
            if (p == null)
                return source;
            if (!string.IsNullOrWhiteSpace(p.UserId))
            {
                if (int.TryParse(p.UserId, out int userId))
                    source = source.Where(c => c.UserID == userId);
            }
            
            // TODO: WRITE SMS FILTER: 'YES', 'NO', 'BOTH'
            if (!string.IsNullOrWhiteSpace(p.SmsFilter) &&
                !p.SmsFilter.Trim().Equals("0", StringComparison.InvariantCultureIgnoreCase)
                && !p.SmsFilter.Trim().Equals("3", StringComparison.InvariantCultureIgnoreCase))
            {
                if (p.SmsFilter.Trim().Equals("1"))
                    source = source.Where(car => car.SMS == "yes");
                else if (p.SmsFilter.Trim().Equals("2"))
                    source = source.Where(car => car.SMS == "-");
            }

            if (!string.IsNullOrWhiteSpace(p.FromPlayLength) || !string.IsNullOrWhiteSpace(p.ToPlayLength))
            {
                Expression<Func<CampaignAuditResultTR, bool>> predicate = null;
                int fromLen, toLen;
                if (int.TryParse(p.FromPlayLength, out fromLen))
                {
                    if (int.TryParse(p.ToPlayLength, out toLen))
                        predicate = car => car.LengthOfPlay >= fromLen && car.LengthOfPlay <= toLen;
                    else
                        predicate = car => car.LengthOfPlay >= fromLen;
                    source = source.Where(predicate);
                }
                else if (int.TryParse(p.ToPlayLength, out toLen))
                {
                    predicate = car => car.LengthOfPlay <= toLen;
                    source = source.Where(predicate);
                }
            }

            if (!string.IsNullOrWhiteSpace(p.FromPlayCost) || !string.IsNullOrWhiteSpace(p.ToPlayCost))
            {
                Expression<Func<CampaignAuditResultTR, bool>> predicate = null;
                double @from, @to;
                if (TryParseDoubleAndConvertIfNeeded(p.FromPlayCost, out @from, itemCountryId))
                {
                    if (TryParseDoubleAndConvertIfNeeded(p.ToPlayCost, out @to, itemCountryId))
                        predicate = car => car.PlayCost >= @from && car.PlayCost <= @to;
                    else
                        predicate = car => car.PlayCost >= @from;
                    source = source.Where(predicate);
                }
                else if (TryParseDoubleAndConvertIfNeeded(p.ToPlayCost, out @to, itemCountryId))
                {
                    predicate = car => car.PlayCost <= @to;
                    source = source.Where(predicate);
                }
            }

            if (!string.IsNullOrWhiteSpace(p.FromTotalCost) || !string.IsNullOrWhiteSpace(p.ToTotalCost))
            {
                Expression<Func<CampaignAuditResultTR, bool>> predicate = null;
                double @from, @to;
                if (TryParseDoubleAndConvertIfNeeded(p.FromTotalCost, out @from, itemCountryId))
                {
                    if (TryParseDoubleAndConvertIfNeeded(p.ToTotalCost, out @to, itemCountryId))
                        predicate = car => car.TotalCost >= @from && car.TotalCost <= @to;
                    else
                        predicate = car => car.TotalCost >= @from;
                    source = source.Where(predicate);
                }
                else if (TryParseDoubleAndConvertIfNeeded(p.ToTotalCost, out @to, itemCountryId))
                {
                    predicate = car => car.TotalCost <= @to;
                    source = source.Where(predicate);
                }
            }

            if (!string.IsNullOrWhiteSpace(p.FromStartTime) || !string.IsNullOrWhiteSpace(p.ToStartTime)
                || !string.IsNullOrWhiteSpace(p.FromEndTime) || !string.IsNullOrWhiteSpace(p.ToEndTime))
            {
                Expression<Func<CampaignAuditResultTR, bool>> predicateS = null,predicateE = null;
                DateTime @fromS, @toS, @fromE, @toE;
                if (DateTime.TryParseExact(p.FromStartTime, CampaignAuditResultTR.DateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,  out @fromS))
                {
                    @fromS = @fromS.AddMinutes(p.TimeZoneOffsetMinutes);
                    if (DateTime.TryParseExact(p.ToStartTime, CampaignAuditResultTR.DateTimeFormat,
                        DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out @toS))
                    {
                        @toS = @toS.AddMinutes(p.TimeZoneOffsetMinutes);
                        predicateS = car => car.StartDate >= @fromS && car.StartDate <= @toS;
                    }
                    else
                        predicateS = car => car.StartDate >= @fromS;
                    source = source.Where(predicateS);
                }
                else if (DateTime.TryParseExact(p.ToStartTime, CampaignAuditResultTR.DateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out @toS))
                {
                    @toS = @toS.AddMinutes(p.TimeZoneOffsetMinutes);
                    predicateS = car => car.StartDate <= @toS;
                    source = source.Where(predicateS);
                }
                if (DateTime.TryParseExact(p.FromEndTime, CampaignAuditResultTR.DateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out @fromE))
                {
                    @fromE = @fromE.AddMinutes(p.TimeZoneOffsetMinutes);
                    if (DateTime.TryParseExact(p.ToEndTime, CampaignAuditResultTR.DateTimeFormat,
                        DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out @toE))
                    {
                        @toE = @toE.AddMinutes(p.TimeZoneOffsetMinutes);
                        predicateE = car => car.EndDate >= @fromE && car.EndDate <= @toE;
                    }
                    else
                        predicateE = car => car.EndDate >= @fromE;
                    source = source.Where(predicateE);
                }
                else if (DateTime.TryParseExact(p.ToEndTime, CampaignAuditResultTR.DateTimeFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out @toE))
                {
                    @toE = @toE.AddMinutes(p.TimeZoneOffsetMinutes);
                    predicateE = car => car.EndDate <= @toE;
                    source = source.Where(predicateE);
                }
            }

            return source;
        }

        private bool TryParseDoubleAndConvertIfNeeded(string val, out double dVal, int? itemCountryId)
        {
            bool result = double.TryParse(val, out double @raw);
            if (!result)
            {
                dVal = default(double);
                return false;
            }

            if (itemCountryId != _currencyConversion.DisplayCurrency.CountryId)
                dVal = Convert.ToDouble(_currencyConversion.ConvertFromDisplayCurrency(Convert.ToDecimal(@raw),
                    _currencyRepository.GetCurrencyUsingCountryId(itemCountryId).CurrencyCode));
            else
                dVal = @raw;
            return true;
        }

        #region Update Campaign Profile And Profile Mapping Data 
        // Update Campaign Information update at edit time
        [HttpPost]
        public ActionResult UpdateCampaignInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                    var CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == CampaignProfileFormModel.CampaignName && c.CampaignProfileId != CampaignProfileFormModel.CampaignProfileId && c.UserId == efmvcUser.UserId).ToList();
                    if (CampaignNameexists.Count() > 0)
                    {
                        return Json(CampaignProfileFormModel.CampaignName + " already Exists.");
                    }

                    CreateOrUpdateCampaignProfileCommand command =
                      Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                    command.UserId = efmvcUser.UserId;

                    command.CampaignDescription = CampaignProfileFormModel.CampaignDescription;
                    command.CampaignName = CampaignProfileFormModel.CampaignName;
                    //command.ClientId = CampaignProfileFormModel.ClientId;
                    command.StartDate = CampaignProfileFormModel.StartDate;
                    command.EndDate = CampaignProfileFormModel.EndDate;
                    command.CountryId = CampaignProfileFormModel.CountryId;

                    var campaigndetails = _profileRepository.GetById(CampaignProfileFormModel.CampaignProfileId);
                    if (campaigndetails != null)
                    {
                        if (CampaignProfileFormModel.ClientId == null)
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
                        int CurrencyId = Convert.ToInt32(CampaignProfileFormModel.CurrencyId);
                        var currencyCountryId = _currencyRepository.Get(c => c.CurrencyId == CurrencyId).Country.Id;
                        var currencyCode = _currencyRepository.Get(c => c.CountryId == currencyCountryId).CurrencyCode;
                        command.CurrencyCode = currencyCode;
                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        // obj.UpdateCampaignInfo(CampaignProfileFormModel, campaigndetails, efmvcUser.UserId, SQLServerEntities);
                        // PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, conn);

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
                                    obj.UpdateCampaignInfo(CampaignProfileFormModel, campaigndetailsFromOP, efmvcUser.UserId, SQLServerEntities);
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
                //return Json(ex.InnerException.Message.ToString());
                return Json("fail");
            }
        }

        // Update Budget Management at edit time
        public ActionResult UpdateBudgetInfo(CampaignProfileFormModel CampaignProfileFormModel)
        {
            try
            {
                CurrencyModel currencyModel = new CurrencyModel();
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);
                int CountryId = Convert.ToInt32(Session["CountryId"].ToString());
                int CurrencyId = Convert.ToInt32(Session["CurrencyId"].ToString());
                var countryCurrencyData = _currencyRepository.Get(c => c.CurrencyId == CurrencyId);
                int currencyCountryId = countryCurrencyData.Country.Id;
                var currencyData = _currencyRepository.Get(c => c.CountryId == CountryId);
                var fromCurrencyCode = countryCurrencyData.CurrencyCode;
                string toCurrencyCode = currencyData.CurrencyCode;
                decimal currencyRate = 0.00M;
                if (currencyCountryId == CountryId)
                {
                    command.UserId = efmvcUser.UserId;
                    command.MaxBid = CampaignProfileFormModel.MaxBid;
                    command.MaxMonthBudget = CampaignProfileFormModel.MaxMonthBudget;
                    command.MaxWeeklyBudget = CampaignProfileFormModel.MaxWeeklyBudget;
                    command.MaxHourlyBudget = CampaignProfileFormModel.MaxHourlyBudget;
                    command.MaxDailyBudget = CampaignProfileFormModel.MaxDailyBudget;
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        double MaxBid = Convert.ToDouble(Convert.ToDecimal(CampaignProfileFormModel.MaxBid) * currencyRate);
                        double MaxMonthBudget = Convert.ToDouble(Convert.ToDecimal(CampaignProfileFormModel.MaxMonthBudget) * currencyRate);
                        double MaxWeeklyBudget = Convert.ToDouble(Convert.ToDecimal(CampaignProfileFormModel.MaxWeeklyBudget) * currencyRate);
                        double MaxHourlyBudget = Convert.ToDouble(Convert.ToDecimal(CampaignProfileFormModel.MaxHourlyBudget) * currencyRate);
                        double MaxDailyBudget = Convert.ToDouble(Convert.ToDecimal(CampaignProfileFormModel.MaxDailyBudget) * currencyRate);

                        command.MaxBid = float.Parse(MaxBid.ToString());
                        command.MaxMonthBudget = float.Parse(MaxMonthBudget.ToString());
                        command.MaxWeeklyBudget = float.Parse(MaxWeeklyBudget.ToString());
                        command.MaxHourlyBudget = float.Parse(MaxHourlyBudget.ToString());
                        command.MaxDailyBudget = float.Parse(MaxDailyBudget.ToString());
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
                    command.CurrencyCode = campaigndetails.CurrencyCode;
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
                                obj.UpdateCampaignBudgetInfo(CampaignProfileFormModel, campaigndetailFromOP, efmvcUser.UserId, SQLServerEntities);
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

        // Update SMS and email at edit time
        public ActionResult UpdateCommunicationInfo(CampaignProfileFormModel CampaignProfileFormModel, HttpPostedFileBase emailfile, HttpPostedFileBase smsfile)
        {
            try
            {
                if (CampaignProfileFormModel.EmailBody == null && CampaignProfileFormModel.EmailSenderAddress == null && CampaignProfileFormModel.EmailSubject == null && CampaignProfileFormModel.SmsBody == null && CampaignProfileFormModel.SmsOriginator == null && emailfile == null && smsfile == null)
                {
                    TempData["commuerror"] = "Atleast one field is required.";
                    return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
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
                        command.CurrencyCode = campaigndetails.CurrencyCode;
                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        // obj.UpdateCampaignSmsAndEmail(CampaignProfileFormModel, campaigndetails, command.EmailFileLocation, command.SMSFileLocation, SQLServerEntities);
                        //PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, conn);

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
                            TempData["commusuccess"] = "Record updated successfully.";
                            return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
                        }
                    }

                    return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
                }

            }
            catch (Exception ex)
            {
                TempData["commuerror"] = ex.InnerException.Message;
                return RedirectToAction("Campaign", "Dashboard", new { @id = CampaignProfileFormModel.CampaignProfileId });
            }
        }

        // Update Status Play, Pause, Archieve from Campaign Information at edit time
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status, int NumberInBatch)
        {

            if (User.Identity.IsAuthenticated)
            {
                CampaignProfileFormModel CampaignProfileFormModel = new CampaignProfileFormModel();
                CreateOrUpdateCampaignProfileCommand command =
                  Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);

                //Add 28-02-2019
                var campaignCredited = _billingRepository.GetMany(top => top.CampaignProfileId == id).ToList();
                if (campaignCredited.Count() > 0)
                {
                    //TempData["msgsuccess"] = "Campaign status successfully changed to " + status;
                    command.Status = status;
                }
                else
                {
                    if (status == 2)
                    {
                        //TempData["msgsuccess"] = "Campaign status successfully changed to Campaign Paused Due To Insufficient Funds";
                        command.Status = (int)AdvertStatus.CampaignPausedDueToInsufficientFunds;
                    }
                    else
                    {
                        command.Status = status;
                    }
                }

                //command.Status = status;
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
                    //command.NumberInBatch = campaigndetails.NumberInBatch;
                    command.CountryId = (int)campaigndetails.CountryId;
                    command.IsAdminApproval = campaigndetails.IsAdminApproval;
                    command.CurrencyCode = campaigndetails.CurrencyCode;
                    if (campaigndetails.NextStatus == true)
                    {
                        command.NextStatus = false;
                    }
                    else
                    {
                        command.NextStatus = campaigndetails.NextStatus;
                    }
                    ICommandResult result = _commandBus.Submit(command);


                    // EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    // var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //obj.UpdateCampaignStatus(id, status, SQLServerEntities);
                    //PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, conn);

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

                    //EFMVCDataContex SQLServerEntities = null;
                    //var ConnString = ConnectionString.GetConnectionString(campaigndetails.CountryId);
                    //if (!string.IsNullOrEmpty(ConnString))
                    //    SQLServerEntities = new EFMVCDataContex(ConnString);
                    //else
                    //    SQLServerEntities = new EFMVCDataContex();

                    if (result.Success)
                    {
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

        [AuthorizeFilter]
        public ActionResult LaunchCampaignOnEdit(string campaignId, string campaignName, string clientId, string advertId, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    int? CountryId = null;
                    int CampaignId = Convert.ToInt32(campaignId);
                    int ClientId = Convert.ToInt32(clientId);
                    int AdvertId = Convert.ToInt32(advertId);
                    if (countryId != "")
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                    if (campaignName != "" || campaignName != null)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();

                        var campaignProfile = db.CampaignProfiles.Where(c => c.CampaignProfileId == CampaignId && c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (campaignProfile != null)
                        {
                            campaignProfile.Status = campaignProfile.Status;
                            campaignProfile.UpdatedDateTime = DateTime.Now;
                            campaignProfile.IsAdminApproval = true;
                            campaignProfile.NextStatus = false;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                    if (campaignProfileDetails != null)
                                    {
                                        campaignProfileDetails.Status = campaignProfileDetails.Status;
                                        campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                        campaignProfileDetails.IsAdminApproval = true;
                                        campaignProfileDetails.NextStatus = false;
                                        db1.SaveChanges();
                                    }
                                }
                            }

                            var client = db.Clients.Where(c => c.Id == ClientId && c.UserId == efmvcUser.UserId).FirstOrDefault();
                            if (client != null)
                            {
                                client.UpdatedDate = DateTime.Now;
                                client.Status = client.Status;
                                client.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db2 = new EFMVCDataContex(item);
                                        var clientDetails = db2.Clients.Where(s => s.AdtoneServerClientId == client.Id).FirstOrDefault();
                                        if (clientDetails != null)
                                        {
                                            clientDetails.UpdatedDate = DateTime.Now;
                                            clientDetails.Status = clientDetails.Status;
                                            clientDetails.NextStatus = false;
                                            db2.SaveChanges();
                                        }
                                    }
                                }
                            }
                            var advert = db.Adverts.Where(c => c.AdvertId == AdvertId && c.UserId == efmvcUser.UserId).FirstOrDefault();
                            if (advert != null)
                            {
                                advert.UpdatedDateTime = DateTime.Now;
                                advert.Status = advert.Status;
                                advert.IsAdminApproval = true;
                                advert.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db3 = new EFMVCDataContex(item);
                                        var advertDetails = db3.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                                        if (advertDetails != null)
                                        {
                                            advertDetails.UpdatedDateTime = DateTime.Now;
                                            advertDetails.Status = advertDetails.Status;
                                            advertDetails.IsAdminApproval = true;
                                            advertDetails.NextStatus = false;
                                            db3.SaveChanges();
                                        }
                                    }
                                }

                                var campaignAdvert = db.CampaignAdverts.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId && c.AdvertId == advert.AdvertId).FirstOrDefault();
                                if (campaignAdvert != null)
                                {
                                    campaignAdvert.NextStatus = false;
                                    db.SaveChanges();
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex db4 = new EFMVCDataContex(item);
                                            var campaignadvertDetails = db4.CampaignAdverts.Where(s => s.AdtoneServerCampaignAdvertId == campaignAdvert.CampaignAdvertId).FirstOrDefault();
                                            if (campaignadvertDetails != null)
                                            {
                                                campaignadvertDetails.NextStatus = false;
                                                db4.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                            var campaignProfilePreference = db.CampaignProfilePreference.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                            if (campaignProfilePreference != null)
                            {
                                campaignProfilePreference.CountryId = campaignProfile.CountryId.Value;
                                campaignProfilePreference.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db5 = new EFMVCDataContex(item);
                                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db5, campaignProfile.CountryId.Value);
                                        var campaignProfilePreferenceDetails = db5.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfilePreference.Id).FirstOrDefault();
                                        if (campaignProfilePreferenceDetails != null)
                                        {
                                            campaignProfilePreferenceDetails.CountryId = externalServerCountryId;
                                            campaignProfilePreferenceDetails.NextStatus = false;
                                            db5.SaveChanges();
                                        }
                                    }
                                }
                            }
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db6 = new EFMVCDataContex(item);
                                    var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db6, (int)campaignProfile.CampaignProfileId);
                                    var campaignMatchDetails = db6.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                                    if (campaignMatchDetails != null)
                                    {
                                        campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                        campaignMatchDetails.Status = campaignProfile.Status;
                                        campaignMatchDetails.NextStatus = false;
                                        db6.SaveChanges();

                                        PreMatchProcess.PrematchProcessForCampaign(externalServerCampaignProfileId, item);
                                    }
                                }
                            }

                            return Json("success");
                        }
                        return Json("success");
                    }
                    return Json("fail");
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        // Update Airtime Usage from Ads Profile Mapping at edit time
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

                        // EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        // var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //  UserMatchTableProcess obj = new UserMatchTableProcess();
                        // obj.UpdateUsage(model, SQLServerEntities);
                        //if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        //{
                        //    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        //}

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

        // Update Time bands at edit time
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

        // Update Adverts from Ads Profile Mapping
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

                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        // var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.UpdateCampaignAdtypes(model, SQLServerEntities);
                        //if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        //{
                        //    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        //}

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

        // Update Questionnarie at edit time
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

                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        // var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.UpdateSkizaProfile(model, SQLServerEntities);
                        //if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        //{
                        //    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        //}

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
                                    //var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(SQLServerEntities, model.CountryId);
                                    //var externalCampaignProfileData = SQLServerEntities.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfileData.Id).FirstOrDefault();
                                    //if (externalCampaignProfileData != null)
                                    //{
                                    //    externalCampaignProfileData.Hustlers_AdType = model.Hustlers_AdType;
                                    //    externalCampaignProfileData.Youth_AdType = model.Youth_AdType;
                                    //    externalCampaignProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                    //    externalCampaignProfileData.Mass_AdType = model.Mass_AdType;
                                    //    externalCampaignProfileData.CountryId = externalServerCountryId;
                                    //    SQLServerEntities.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    CampaignProfilePreference profilePreference = new CampaignProfilePreference();
                                    //    profilePreference.Hustlers_AdType = model.Hustlers_AdType;
                                    //    profilePreference.Youth_AdType = model.Youth_AdType;
                                    //    profilePreference.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                    //    profilePreference.Mass_AdType = model.Mass_AdType;
                                    //    profilePreference.CountryId = externalServerCountryId;
                                    //    SQLServerEntities.CampaignProfilePreference.Add(profilePreference);
                                    //    SQLServerEntities.SaveChanges();
                                    //    model.CampaignProfileId = profilePreference.CampaignProfileId;
                                    //}
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

        // Update Geographic from Ads Profile Mapping
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

                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.UpdateCampaignGeographic(model, SQLServerEntities);
                        //if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        //{
                        //    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        //}

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

        // Update Demographics from Ads Profile Mapping
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

                    int countryId = 0;
                    var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                    if (campaignProfile != null)
                    {
                        countryId = (int)campaignProfile.CountryId;
                    }

                    //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    //  var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //obj.UpdateCampaignDemographics(model, SQLServerEntities);
                    //if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                    //{
                    //    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                    //}

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

        #endregion
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
        public async Task<ActionResult> GetClientAdvert(int?[] clientId)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (clientId != null)
                {
                    var advertdetails = await _advertRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && clientId.Contains(top.ClientId)).Select(top => new
                    {
                        AdvertName = top.AdvertName,
                        AdvertId = top.AdvertId.ToString()
                    }).ToListAsync();
                    return Json(advertdetails);
                }
                else
                {
                    var _result = await _advertRepository.AsQueryable().Where(c => c.UserId == efmvcUser.UserId).ToListAsync();
                    if (_result != null)
                    {
                        var _resultDetails = _result.Select(top => new
                        {
                            AdvertName = top.AdvertName,
                            AdvertId = top.AdvertId.ToString()
                        }).ToList();
                        return Json(_resultDetails);
                    }
                    return Json("nodata");
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        public async Task<ActionResult> GetCampaignAdvert(int[] campaignId)
        {
            try
            {
                if (campaignId != null)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    //var advertId = _campaignAdvertRepository.Get(top => campaignId.Contains(top.CampaignProfileId));
                    List<int> advertId = await _campaignAdvertRepository.AsQueryable().Where(top => campaignId.Contains(top.CampaignProfileId)).Select(top => top.AdvertId).ToListAsync();
                    if (advertId.Count > 0)
                    {
                        var advertdetails = await _advertRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && advertId.Contains(top.AdvertId)).Select(top => new
                        {
                            AdvertName = top.AdvertName,
                            AdvertId = top.AdvertId.ToString()
                        }).ToListAsync();
                        return Json(advertdetails);
                    }
                    return Json("nodata");
                }
                else
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var _result = await _advertRepository.AsQueryable().Where(c => c.UserId == efmvcUser.UserId).ToListAsync();
                    if (_result != null)
                    {
                        var _resultDetails = _result.Select(top => new
                        {
                            AdvertName = top.AdvertName,
                            AdvertId = top.AdvertId.ToString()
                        }).ToList();
                        return Json(_resultDetails);
                    }
                    return Json("nodata");
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        public async Task<ActionResult> GetClientCampaign(int?[] clientId)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (clientId != null)
                {
                    var _result = await _profileRepository.AsQueryable().Where(c => clientId.Contains(c.ClientId) && c.UserId == efmvcUser.UserId).ToListAsync();
                    var _resultDetails = _result.Select(top => new
                    {
                        CampaignName = top.CampaignName,
                        CampaignProfileId = top.CampaignProfileId
                    }).ToList();
                    TempData.Keep("compainresult");
                    return Json(_resultDetails);
                }
                else
                {
                    var _result = await _profileRepository.AsQueryable().Where(c => c.UserId == efmvcUser.UserId).ToListAsync();

                    var _resultDetails = _result.Select(top => new
                    {
                        CampaignName = top.CampaignName,
                        CampaignProfileId = top.CampaignProfileId
                    }).ToList();
                    TempData.Keep("compainresult");
                    return Json(_resultDetails);
                }
                return Json("error");
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        //Advertiser Dashboard New Function For Optimization Using SP
        public async Task<ActionResult> FillChartDataAjax()
        {
            if (User.Identity.IsAuthenticated)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                IQueryable<CampaignProfile> _CampaignProfileFormModel = _profileRepository.AsQueryable().Where(x => x.UserId == efmvcUser.UserId && (x.Status != 5));
                int?[] clientId = null;
                List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
                List<CampaignNoOfPlay> _campaignNoofplay = new List<CampaignNoOfPlay>();
                List<Campaignchartresult> _campaignbidavgResult = new List<Campaignchartresult>();
                List<CampaignAvgbid> _campaignAvgbid = new List<CampaignAvgbid>();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                List<Campaignchartresult> _result = new List<Campaignchartresult>();
                CampaignDashboardChartResult _CampaignDashboardChartResult;
                var clientList = await _clientRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && (top.Status == 1 || top.Status == 2)).Select(top => new { Name = top.Name, Id = top.Id.ToString() }).ToListAsync();
                if (!_CampaignProfileFormModel.Any())
                {
                    ViewBag.FreePlays = 1;
                    ViewBag.TotalPlayed = 1;
                    ViewBag.TotalBudget = 1;
                    ViewBag.TotalSpend = 1;
                    ViewBag.MaxBid = 0;
                    ViewBag.AvgMaxBid = 1;
                    ViewBag.chartclient = new MultiSelectList(clientList, "Id", "Name");
                    List<NoOfPlayCampaign> Noofplay = new List<NoOfPlayCampaign>();
                    Noofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                    List<NoOfAvgbidCampaign> Avgbid = new List<NoOfAvgbidCampaign>();
                    Avgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                    _result.Add(new Campaignchartresult { status = 3, _playresult = Noofplay.ToArray(), _Avgresult = Avgbid.ToArray() });
                    ViewBag.NoofplayMaxCount = 0;
                    ViewBag.AvgbidMaxCount = 0;
                    ViewBag.Campaignavgplayresult = _result;
                    _playgroup.Add(new MaxLengthGroup { second = 0 });
                    ViewBag.getbarChartdata = _getbarChartData(_playgroup).ToArray();
                    
                    return PartialView("_DashboardStatisticsChart", new CampaignDashboardChartResult{CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode});
                }
                
                ViewBag.chartclient = new MultiSelectList(clientList, "Id", "Name");
                if (TempData["DashboardClientId"] != null) 
                    clientId = (int?[])TempData["DashboardClientId"];
                if (clientId != null)
                    _CampaignProfileFormModel = _CampaignProfileFormModel.Where(top => clientId.Contains(top.ClientId));

                var stats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Advertiser, efmvcUser.UserId, _currencyConversion);
                var summaries = stats.DashboardReduced;
                var totalReach = stats.Reach?.ReachValue;
                var totalBid = summaries.MaxBid;
                var avgBid = summaries.AvgBid;
                var totalPlays = summaries.TotalPlays;
                _CampaignDashboardChartResult = new CampaignDashboardChartResult
                {
                    PlaystoDate = summaries.TotalPlays,
                    FreePlays = (int)summaries.FreePlays,
                    SpendToDate = (double)summaries.Spend,
                    AverageBid = (double)summaries.AvgBid,
                    AveragePlayTime = (double)Math.Round(summaries.AvgPlayLength/1000, 2, MidpointRounding.ToEven),
                    FreePlaysPercantage = (double)summaries.FreePlaysPercentage(),
                    TotalBudget = summaries.Budget,
                    TotalBudgetPercantage = (double)summaries.BudgetPercentage(),
                    MaxBid = (double)summaries.MaxBid,
                    MaxBidPercantage =(double) Math.Round(totalBid == 0 ? 0 : avgBid / totalBid , 2),
                    MaxPlayLength = Math.Round(summaries.MaxPlayLength/1000M, 2, MidpointRounding.ToEven),
                    SMSCost = summaries.TotalSMS,
                    EmailCost = summaries.TotalEmail,
                    Cancelled = 0,
                    TotalPlayed = (int)summaries.TotalPlays,
                    TotalReach = (int)(totalReach ?? 0),
                    TotalSpend = (double)summaries.Spend,
                    AvgMaxBid = (double)summaries.AvgBid,
                    CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode,
                    MaxPlayLengthPercantage = totalPlays == 0 ? 0 : Math.Round((double)summaries.MoreSixSecPlays / totalPlays, 2),
                };
                
                if (stats.PlaysByPeriods.Values.Count > 0)
                {
                    int status = (int)stats.PlaysByPeriods.Values.Select(v => v.PeriodType).First();
                    var xNoofplay = stats.PlaysByPeriods.Values.OrderBy(v=>v.StatId).Select(v=>new NoOfPlayCampaign{name = v.PeriodName, value = v.Value}).ToList();
                    var xAvgbid = new [] { new NoOfAvgbidCampaign() { name = 0, value = 0 }};
                    _result.Add(new Campaignchartresult { status = status, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid });
                    ViewBag.NoofplayMaxCount = xNoofplay.Select(v=>v.value).Max();
                    ViewBag.AvgbidMaxCount = 0; //campaignNoofplayData.FirstOrDefault().AvgbidMaxCount;
                    ViewBag.Campaignavgplayresult = _result;
                }
                else
                {
                    List<NoOfPlayCampaign> xNoofplay = new List<NoOfPlayCampaign>();
                    xNoofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                    List<NoOfAvgbidCampaign> xAvgbid = new List<NoOfAvgbidCampaign>();
                    xAvgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                    _result.Add(new Campaignchartresult { status = 3, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() });
                    ViewBag.NoofplayMaxCount = 0;
                    ViewBag.AvgbidMaxCount = 0;
                    ViewBag.Campaignavgplayresult = _result;
                    _playgroup.Add(new MaxLengthGroup { second = 0 });
                    ViewBag.getbarChartdata = _getbarChartData(_playgroup).ToArray();
                }

                var sprkLineData = stats.SpikeLengths.Values.Select(v=>(int)v).ToList();
                    
                ViewBag.getbarChartdata = sprkLineData.ToArray();

                ViewBag.FreePlays = _CampaignDashboardChartResult.FreePlays;
                ViewBag.TotalPlayed = _CampaignDashboardChartResult.TotalPlayed;
                ViewBag.TotalBudget = _CampaignDashboardChartResult.TotalBudget;
                ViewBag.TotalSpend = _CampaignDashboardChartResult.SpendToDate;
                ViewBag.MaxBid = _CampaignDashboardChartResult.TotalReach;
                ViewBag.AvgMaxBid = _CampaignDashboardChartResult.AverageBid;
            
                return PartialView("_DashboardStatisticsChart", _CampaignDashboardChartResult);
            }
            else
            {
                return PartialView("_CampaignList", "notauthorise");
            }
        }

        public ActionResult GetAvgPlayTimeForChart()
        {
            double AveragePlayTime = 0;
            var GetCampaignAuditData = _campaignAuditRepository.GetMany(top => top.Status.ToLower() == "played" && top.PlayLengthTicks > 6000).ToList();
            if (GetCampaignAuditData.Count() > 0)
            {
                AveragePlayTime = (GetCampaignAuditData.Average(s => s.PlayLengthTicks)) / 1000;
            }

            return Json(AveragePlayTime, JsonRequestBehavior.AllowGet);
        }

        private async Task GetCampaignbidplaydata(int? campaignProfileId, int? campaignCountry, PeriodicIntegerData stats)
        {
            if(!campaignProfileId.HasValue)
            {
                ViewBag.AvgbidMaxCount = 0;
                ViewBag.NoofplayMaxCount = 0;
                ViewBag.Campaignavgplayresult = new List<Campaignchartresult>
                {
                    new Campaignchartresult
                    {
                        status = 3, 
                        _playresult = new [] {new NoOfPlayCampaign{name = 0, value = 0}}, 
                        _Avgresult = new [] {new NoOfAvgbidCampaign {name = 0, value = 0}}
                    }
                };
                return;
            }
            //SqlParameter reportTypeParam1 = new SqlParameter("@reportType", SqlDbType.NVarChar, 1) {Direction = ParameterDirection.InputOutput, Value = DBNull.Value};
            //SqlParameter reportTypeParam2 = new SqlParameter("@reportType", SqlDbType.NVarChar, 1) {Direction = ParameterDirection.InputOutput, Value = DBNull.Value};
            //SqlParameter startTimeParam = new SqlParameter("@startDate", SqlDbType.DateTime2) {Direction = ParameterDirection.Output,IsNullable = true, Value = DBNull.Value};
            //SqlParameter endTimeParam = new SqlParameter("@endDate", SqlDbType.DateTime2) {Direction = ParameterDirection.Output,IsNullable = true, Value = DBNull.Value};

            //List<NoOfPlayCampaign> playsData = new List<NoOfPlayCampaign>();
            //List<NoOfAvgbidCampaign> avgBidData = new List<NoOfAvgbidCampaign>();
            //string stateString = null;
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
            //{
            //    await conn.OpenAsync();
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "GetCampaignPlayAndBidStats";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@campaignId", campaignProfileId);
            //        cmd.Parameters.AddWithValue("@sourceType", "PLAYS");
            //        cmd.Parameters.Add(reportTypeParam1);
            //        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            //        {
            //            while (await dr.ReadAsync())
            //            {
            //                playsData.Add(new NoOfPlayCampaign {name = dr.GetInt32(0), value = Convert.ToDouble(dr.GetValue(1))});
            //            }
            //        }

            //        stateString = (string)reportTypeParam1.Value;
            //    }
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "GetCampaignPlayAndBidStats";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@campaignId", campaignProfileId);
            //        cmd.Parameters.AddWithValue("@sourceType", "AVGBID");
            //        cmd.Parameters.Add(reportTypeParam2);
            //        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            //        {
            //            while (await dr.ReadAsync())
            //            {
            //                avgBidData.Add(new NoOfAvgbidCampaign { name = dr.GetInt32(0), value = Convert.ToDouble(dr.GetDecimal(1)) });
            //            }
            //        }
            //    }
            //    conn.Close();
            //}

            //var campaignCurrency = await _currencyRepository.GetCurrencyUsingCountryIdAsync(countryId: campaignCountry);
            //if (_currencyConversion.DisplayCurrency.CurrencyCode != campaignCurrency.CurrencyCode)
            //{
            //    avgBidData.ForEach(d=>d.value = Convert.ToDouble(_currencyConversion.ConvertToDisplayCurrency(Convert.ToDecimal(d.value), campaignCurrency.CurrencyCode)));
            //}

            //int state = 0;
            //switch (stateString.FirstOrDefault())
            //{
            //    case 'W': state = 1;
            //        break;
            //    case 'D': state = 2;
            //        break;
            //    case 'H': state = 3;
            //        break;
            //}

            if (stats.Values.Count > 0)
            {
                ViewBag.AvgbidMaxCount = 0;//avgBidData.Max(p=>p.value);
                ViewBag.NoofplayMaxCount = stats.Values.Max(p=>p.Value);
                ViewBag.Campaignavgplayresult = new List<Campaignchartresult>
                {
                    new Campaignchartresult
                    {
                        status = (int)stats.Values.Select(v=>v.PeriodType).First(), 
                        _Avgresult = new [] { new NoOfAvgbidCampaign() }, //avgBidData.ToArray(), 
                        _playresult = stats.Values.OrderBy(v=>v.StatId).Select(v=>new NoOfPlayCampaign{value = v.Value, name = v.PeriodName}).ToArray(),
                    }
                };
            }
            else
            {
                List<NoOfPlayCampaign> xNoofplay = new List<NoOfPlayCampaign>();
                xNoofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                List<NoOfAvgbidCampaign> xAvgbid = new List<NoOfAvgbidCampaign>();
                xAvgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                
                ViewBag.AvgbidMaxCount = 0;
                ViewBag.NoofplayMaxCount = 0;
                ViewBag.Campaignavgplayresult = new List<Campaignchartresult> { new Campaignchartresult { status = 3, _playresult = xNoofplay.ToArray(), _Avgresult = xAvgbid.ToArray() } };
            }
        }

        public List<int> _getbarChartData(List<MaxLengthGroup> data)
        {
            List<int> barData = new List<int>();
            if (data.Count > 0)
            {
                barData.Add(data.Count(top => top.second >= 6 && top.second <= 9));
                barData.Add(data.Count(top => top.second >= 9 && top.second <= 12));
                barData.Add(data.Count(top => top.second >= 12 && top.second <= 15));
                barData.Add(data.Count(top => top.second >= 15 && top.second <= 18));
                barData.Add(data.Count(top => top.second >= 18 && top.second <= 21));
                barData.Add(data.Count(top => top.second >= 21 && top.second <= 24));
                barData.Add(data.Count(top => top.second >= 24 && top.second <= 27));
                barData.Add(data.Count(top => top.second >= 27 && top.second <= 30));
                barData.Add(data.Count(top => top.second >= 30 && top.second <= 999));
            }

            return barData;
        }

        public async Task<List<int>> _getbarChartData(IQueryable<MaxLengthGroup> data)
        {
            List<int> barData = new List<int>();
            if (data.Any())
            {
                barData.Add(await data.CountAsync(top => top.second >= 6 && top.second <= 9));
                barData.Add(await data.CountAsync(top => top.second >= 9 && top.second <= 12));
                barData.Add(await data.CountAsync(top => top.second >= 12 && top.second <= 15));
                barData.Add(await data.CountAsync(top => top.second >= 15 && top.second <= 18));
                barData.Add(await data.CountAsync(top => top.second >= 18 && top.second <= 21));
                barData.Add(await data.CountAsync(top => top.second >= 21 && top.second <= 24));
                barData.Add(await data.CountAsync(top => top.second >= 24 && top.second <= 27));
                barData.Add(await data.CountAsync(top => top.second >= 27 && top.second <= 30));
                barData.Add(await data.CountAsync(top => top.second >= 30 && top.second <= 999));
            }

            return barData;
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

        public async Task<CampaignDashboardChartResult> FillChartDataByCampaignId(CampaignProfileFormModel _CampaignProfileFormModel)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();

            CampaignDashboardChartResult _CampaignDashboardChartResult;
            var clientList = await _clientRepository.AsQueryable().Where(top => top.UserId == efmvcUser.UserId && (top.Status == 1 || top.Status == 2)).Select(top => new { Name = top.Name, Id = top.Id.ToString() }).ToListAsync();
            if (_CampaignProfileFormModel == null)
            {
                ViewBag.FreePlays = 1;
                ViewBag.TotalPlayed = 1;
                ViewBag.TotalBudget = 1;
                ViewBag.TotalSpend = 1;
                ViewBag.MaxBid = 0;
                ViewBag.AvgMaxBid = 1;
                ViewBag.chartclient = new MultiSelectList(clientList, "Id", "Name");
                List<NoOfPlayCampaign> Noofplay = new List<NoOfPlayCampaign>();
                Noofplay.Add(new NoOfPlayCampaign { name = 0, value = 0 });
                List<NoOfAvgbidCampaign> Avgbid = new List<NoOfAvgbidCampaign>();
                Avgbid.Add(new NoOfAvgbidCampaign { name = 0, value = 0 });
                ViewBag.NoofplayMaxCount = 0;
                ViewBag.AvgbidMaxCount = 0;
                ViewBag.Campaignavgplayresult = new List<Campaignchartresult> { new Campaignchartresult { status = 3, _playresult = Noofplay.ToArray(), _Avgresult = Avgbid.ToArray() } };
                ViewBag.getbarChartdata = _getbarChartData(new List<MaxLengthGroup> { new MaxLengthGroup { second = 0 } }).ToArray();

                return new CampaignDashboardChartResult { CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode };
            }

            var stats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Campaign, _CampaignProfileFormModel.CampaignProfileId, _currencyConversion);
            var summaries = stats.Dashboard.FirstOrDefault() ?? new DashboardSummariesDao
            {
                Budget = _CampaignProfileFormModel.TotalBudget, 
                AvgBid = Convert.ToDecimal(_CampaignProfileFormModel.MaxBid),
            };
            var totalReach = summaries.Reach;
            var totalBid = summaries.MaxBid;
            var avgBid = summaries.AvgBid;
            var totalPlays = summaries.TotalPlays;
            _CampaignDashboardChartResult = new CampaignDashboardChartResult
            {
                PlaystoDate = summaries.TotalPlays,
                FreePlays = (int)summaries.FreePlays,
                SpendToDate = (double)summaries.Spend,
                AverageBid = (double)summaries.AvgBid,
                AveragePlayTime = Convert.ToDouble(Math.Round(summaries.AvgPlayLength/1000M, 1, MidpointRounding.ToEven)),
                FreePlaysPercantage = (double) summaries.FreePlaysPercentage(),
                TotalBudget = summaries.Budget,
                TotalBudgetPercantage = (double) summaries.BudgetPercentage(),
                MaxBid = (double)summaries.MaxBid,
                MaxBidPercantage = (double)Math.Round(totalBid == 0 ? 0 : avgBid / totalBid, 2),
                MaxPlayLength = Math.Round(summaries.MaxPlayLength/1000M, 1, MidpointRounding.ToEven),
                SMSCost = summaries.TotalSMS,
                EmailCost = summaries.TotalEmail,
                Cancelled = 0,
                TotalPlayed = (int)summaries.TotalPlays,
                TotalReach = (int)(totalReach),
                TotalSpend = (double)summaries.Spend,
                AvgMaxBid = (double)summaries.AvgBid,
                CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode,
                MaxPlayLengthPercantage = totalPlays == 0 ? 0 : Math.Round((double)summaries.MoreSixSecPlays / totalPlays, 1),
            };

            ViewBag.getbarChartdata = stats.SpikeLengths.Values;
            
            ViewBag.FreePlays = _CampaignDashboardChartResult.FreePlays;
            ViewBag.TotalPlayed = _CampaignDashboardChartResult.TotalPlayed;
            ViewBag.TotalBudget = _CampaignDashboardChartResult.TotalBudget;
            ViewBag.TotalSpend = _CampaignDashboardChartResult.SpendToDate;
            ViewBag.MaxBid = _CampaignDashboardChartResult.TotalReach;
            ViewBag.AvgMaxBid = _CampaignDashboardChartResult.AverageBid;
            
            await GetCampaignbidplaydata(_CampaignProfileFormModel?.CampaignProfileId, _CampaignProfileFormModel?.CountryId, stats.PlaysByPeriods);
            return _CampaignDashboardChartResult;
        }

        #region Add New Campaign Wizard

        private void FillCampaign(int UserId)
        {
            var CampaignDetails = _profileRepository.GetAll().Where(s => s.UserId == UserId && s.NextStatus == false).Select(top => new SelectListItem { Text = top.CampaignName, Value = top.CampaignProfileId.ToString() }).ToList();

            var AllCampaign = CampaignDetails.ToList();
            ViewBag.allCampaignList = AllCampaign;
        }

        private void FillCountry()
        {
            var countryId = _operatorRepository.GetMany(s => s.IsActive).Select(c => c.CountryId).ToList();
            var country = (from action in _countryRepository.GetMany(c => countryId.Contains(c.Id)).OrderBy(c => c.Name)
                           select new SelectListItem
                           {
                               Text = action.Name,
                               Value = action.Id.ToString()
                           }).ToList();
            ViewBag.countryList = country;
            FillOperator(0);
        }

        public void FillClient(int UserId)
        {
            var ClientDetails = _clientRepository.GetAll().Where(s => s.UserId == UserId && s.NextStatus == false && s.Status == (int)ClientStatus.Live).Select(top => new SelectListItem { Text = top.Name, Value = top.Id.ToString() }).ToList();
            ClientDetails.Insert(0, new SelectListItem { Text = "-- Select Client --", Value = "" });
            var AllClient = ClientDetails.ToList();

            ViewBag.allClientList = AllClient;
        }

        private void FillAdvertList(int UserId)
        {
            var advert = (from action in _advertRepository.GetMany(c => c.UserId == UserId && c.NextStatus == false).OrderBy(c => c.AdvertName)
                          select new SelectListItem
                          {
                              Text = action.AdvertName,
                              Value = action.AdvertId.ToString()
                          }).ToList();
            ViewBag.advertList = advert;
        }

        public void FillAdvertCategory(int? userCountryId)
        {
            if (userCountryId != 0)
            {
                var advertCategoryDetails = _advertCategoryRepository.GetMany(top => top.CountryId == userCountryId).Select(top => new SelectListItem { Text = top.Name, Value = top.AdvertCategoryId.ToString() }).ToList();
                advertCategoryDetails.Insert(0, new SelectListItem { Text = "-- Select Advert Category --", Value = "" });
                var allAdvertCategory = advertCategoryDetails.ToList();
                ViewBag.allAdvertCategoryList = allAdvertCategory;
            }
            else
            {
                var advertCategoryDetails = _advertCategoryRepository.GetMany(top => top.CountryId == 9).Select(top => new SelectListItem { Text = top.Name, Value = top.AdvertCategoryId.ToString() }).ToList();
                advertCategoryDetails.Insert(0, new SelectListItem { Text = "-- Select Advert Category --", Value = "" });
                var allAdvertCategory = advertCategoryDetails.ToList();
                ViewBag.allAdvertCategoryList = allAdvertCategory;
            }
        }

        public void FillOperator(int? countryId)
        {
            if (countryId == null || countryId == 0)
            {
                var operatordetails = _operatorRepository.GetMany(s => s.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.OperatorList = new MultiSelectList(operatordetails, "Id", "Name");

            }
            else
            {
                var operatordetails = _operatorRepository.GetMany(s => s.IsActive == true && s.CountryId == countryId).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.OperatorList = new MultiSelectList(operatordetails, "Id", "Name");
            }

        }

        public ActionResult GetOperatorList(int countryId)
        {
            var operatorList = _operatorRepository.GetMany(s => s.CountryId == countryId && s.IsActive == true).Select(s => new SelectListItem { Text = s.OperatorName, Value = s.OperatorId.ToString() }).OrderBy(s => s.Text).ToList();
            if (operatorList.Count() > 0)
            {
                return Json(operatorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [AuthorizeFilter]
        public ActionResult AddNewCampaign()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                CampaignProfileID = 0;
                ClientID = 0;

                EFMVCDataContex db = new EFMVCDataContex();

                User user = _userRepository.GetById(efmvcUser.UserId);
                var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2));
                IEnumerable<ClientModel> clientModels =
                    Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);

                NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();

                var countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("Kenya".ToLower())).Id;

                CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(countryId);
                CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(countryId);
                CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel(countryId);
                CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel(countryId);
                CampaignProfileSkizaFormModel CampaignSkiza = new CampaignProfileSkizaFormModel(countryId);
                CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();

                //Campaign
                var str1 = RandomString(3);
                var str2 = RandomString(3);
                var str3 = RandomString(3);

                string phonetic = phonetic = str1.ToLower() + "-" + str2.ToLower() + "-" + str3.ToLower();
                var temp = ConvertPhoneticAlphabet(phonetic);
                model.PhoneticAlphabet = phonetic;
                ViewBag.Phonetic = temp;

                //Client
                var str4 = RandomString(3);
                var str5 = RandomString(3);
                var str6 = RandomString(3);

                string clientphonetic = phonetic = str4.ToLower() + "-" + str5.ToLower() + "-" + str6.ToLower();
                var clienttemp = ConvertPhoneticAlphabet(clientphonetic);
                ViewBag.ClientPhoneticAlphabet = clientphonetic;
                ViewBag.ClientPhonetic = clienttemp;

                //Advert
                var str7 = RandomString(3);
                var str8 = RandomString(3);
                var str9 = RandomString(3);

                string advertPhonetic = advertPhonetic = str7.ToLower() + "-" + str8.ToLower() + "-" + str9.ToLower();
                var advertTemp = ConvertPhoneticAlphabet(advertPhonetic);

                ViewBag.AdvertPhoneticAlphabet = advertPhonetic;
                ViewBag.AdvertPhonetic = advertTemp;

                FillCampaign(efmvcUser.UserId);
                FillClient(efmvcUser.UserId);
                FillCountry();
                FillAdvertList(efmvcUser.UserId);
                FillCurrencyList();

                int? userCountryId = null;
                var countryId1 = _contactsRepository.Get(top => top.UserId == efmvcUser.UserId).CountryId;
                if (countryId1 == null)
                {
                    var userData = _userRepository.GetById(efmvcUser.UserId);
                    if (userData.OperatorId != 0)
                    {
                        userCountryId = _operatorRepository.GetById(userData.OperatorId).CountryId.Value;
                        FillAdvertCategory(userCountryId);
                    }
                    else
                    {
                        FillAdvertCategory(0);
                    }
                }
                else
                {
                    FillAdvertCategory(countryId1);
                }

                ViewBag.Country = _countryRepository.Get(top => top.Name.ToLower().Equals("Kenya".ToLower())).Name;

                CampaignProfileGeographicModel = CampaignProfileGeographicMapping(countryId, CampaignProfileGeographicModel);
                model.newAdProfileMappingFormModel.CampaignProfileGeographicModel = CampaignProfileGeographicModel;

                CampaignProfileDemographicsmodel = CampaignProfileDemographicMapping(countryId, CampaignProfileDemographicsmodel);
                model.newAdProfileMappingFormModel.CampaignProfileDemographicsmodel = CampaignProfileDemographicsmodel;

                CampaignProfileAd = CampaignProfileAdvertMapping(countryId, CampaignProfileAd);
                model.newAdProfileMappingFormModel.CampaignProfileAd = CampaignProfileAd;

                CampaignSkiza = CampaignProfileSkizaInormationMapping(countryId, CampaignSkiza);
                model.newAdProfileMappingFormModel.CampaignProfileSkizaFormModel = CampaignSkiza;

                Mobilepro = UsageMapping(countryId, Mobilepro);
                model.newAdProfileMappingFormModel.CampaignProfileMobileFormModel = Mobilepro;

                CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(0);
                model.newAdProfileMappingFormModel.CampaignProfileTimeSettingModel = CampaignProfileTimeSettingModel;

                ProfileGeographicOptions(model.newAdProfileMappingFormModel.CampaignProfileGeographicModel, countryId);

                ProfileDemographicsOptions(model.newAdProfileMappingFormModel.CampaignProfileDemographicsmodel, countryId);
                ProfileAdvertOptions(model.newAdProfileMappingFormModel.CampaignProfileAd, countryId);
                ProfileMobileFormOptions(model.newAdProfileMappingFormModel.CampaignProfileMobileFormModel, countryId);

                return View(model);
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult GetCampaignDetails(string campaignId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (campaignId != null && campaignId != "0" && campaignId != "")
                {
                    NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
                    var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
                    model.CampaignName = campaignDetail.CampaignName;
                    model.CampaignDescription = campaignDetail.CampaignDescription;
                    model.CountryId = Convert.ToInt32(campaignDetail.CountryId);

                    FillCampaign(efmvcUser.UserId);

                    FillCountry();

                    return PartialView("_AddNewCampaignInfo", model);
                }
                return Json("Fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult GetClientDetails(string clientId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (clientId != null && clientId != "0" && clientId != "")
                {
                    NewClientFormModel model = new NewClientFormModel();
                    var clientDetail = _clientRepository.GetById(Convert.ToInt32(clientId));
                    model.ClientName = clientDetail.Name;
                    model.ClientDescription = clientDetail.Description;
                    model.ClientEmail = clientDetail.Email;
                    model.ClientContactInfo = clientDetail.ContactInfo;

                    FillClient(efmvcUser.UserId);

                    return PartialView("_AddNewClientInfo", model);
                }
                return Json("Fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult GetAdvertDetails(string advertId, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (advertId != null && advertId != "0" && advertId != "")
                {
                    int userCountryId = 0;
                    var userData = _userRepository.GetById(efmvcUser.UserId);
                    if (userData.OperatorId == 0)
                    {
                        userCountryId = _contactRepository.Get(top => top.UserId == userData.UserId).CountryId.Value;
                    }
                    else
                    {
                        userCountryId = _operatorRepository.GetById(userData.OperatorId).CountryId.Value;
                    }

                    NewAdvertFormModel model = new NewAdvertFormModel();
                    var advertDetail = _advertRepository.GetById(Convert.ToInt32(advertId));
                    model.AdvertName = advertDetail.AdvertName;
                    model.AdvertClientId = advertDetail.ClientId == null ? 0 : advertDetail.ClientId;
                    model.BrandName = advertDetail.Brand;
                    model.AdvertCategoryId = advertDetail.AdvertCategoryId == null ? 0 : advertDetail.AdvertCategoryId.Value;
                    model.Script = advertDetail.Script;
                    model.Numberofadsinabatch = "1";
                    model.OperatorId = advertDetail.OperatorId == null ? 0 : advertDetail.OperatorId.Value;

                    ViewBag.mediafile = advertDetail.MediaFileLocation;
                    ViewBag.scriptfile = advertDetail.ScriptFileLocation;

                    FillAdvertList(efmvcUser.UserId);
                    FillClient(efmvcUser.UserId);
                    FillAdvertCategory(userCountryId);
                    FillOperator(Convert.ToInt32(countryId));

                    return PartialView("_AddNewAdvertInfo", model);
                }
                return Json("Fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddNewCampaignInfo(string campaignId, string campaignName, string campaignDescription, string phoneticAlphabet, string countryId, string clientCheck)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
                    EFMVCDataContex db = new EFMVCDataContex();
                    PostedTimesModel postedTimesModel = new PostedTimesModel();
                    postedTimesModel.DayIds = new string[] { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };
                    int CountryId = 0;
                    if (countryId == "12" || countryId == "13" || countryId == "14")
                    {
                        CountryId = 12;
                    }
                    else if (countryId == "11")
                    {
                        CountryId = 8;
                    }
                    else
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;

                    if (campaignId != "")
                    {
                        IEnumerable<CampaignProfile> CampaignNameexists;
                        if (CampaignProfileID == 0)
                        {
                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                        }
                        else
                        {
                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                        }
                        if (CampaignNameexists.Count() > 0)
                        {
                            FillCampaign(efmvcUser.UserId);
                            FillCountry();

                            return Json("Exists");
                        }
                        else
                        {
                            var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
                            model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                            model.UserId = campaignDetail.UserId;
                            model.ClientId = campaignDetail.ClientId;
                            model.CampaignName = campaignName;
                            model.CampaignDescription = campaignDescription;
                            model.TotalBudget = campaignDetail.TotalBudget;
                            model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
                            model.MaxBid = campaignDetail.MaxBid;
                            model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
                            model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
                            model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
                            model.TotalCredit = campaignDetail.TotalCredit;
                            model.SpendToDate = campaignDetail.SpendToDate;
                            model.AvailableCredit = campaignDetail.AvailableCredit;
                            model.PlaysToDate = campaignDetail.PlaysToDate;
                            model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
                            model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
                            model.CancelledToDate = campaignDetail.CancelledToDate;
                            model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
                            model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
                            model.SmsToDate = campaignDetail.SmsToDate;
                            model.SmsLastMonth = campaignDetail.SmsLastMonth;
                            model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
                            model.EmailToDate = campaignDetail.EmailToDate;
                            model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
                            model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
                            model.EmailFileLocation = campaignDetail.EmailFileLocation;
                            model.Active = campaignDetail.Active;
                            model.NumberOfPlays = campaignDetail.NumberOfPlays;
                            model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
                            model.SmsRequests = campaignDetail.SmsRequests;
                            model.EmailsDelievered = campaignDetail.EmailsDelievered;
                            model.EmailSubject = campaignDetail.EmailSubject;
                            model.EmailBody = campaignDetail.EmailBody;
                            model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
                            model.SmsOriginator = campaignDetail.SmsOriginator;
                            model.SmsBody = campaignDetail.SmsBody;
                            model.SMSFileLocation = campaignDetail.SMSFileLocation;
                            model.CreatedDateTime = campaignDetail.CreatedDateTime;
                            model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
                            model.Status = (int)CampaignStatus.InProgress;
                            model.StartDate = campaignDetail.StartDate;
                            model.EndDate = campaignDetail.EndDate;
                            model.NumberInBatch = campaignDetail.NumberInBatch;
                            model.CountryId = int.Parse(countryId);
                            model.IsAdminApproval = false;
                            model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
                            model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
                            model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
                            model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
                            model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
                            model.BucketCount = campaignDetail.BucketCount;
                            model.PhoneticAlphabet = phoneticAlphabet;
                            model.NextStatus = false;
                            model.CurrencyCode = currencyCode;

                            CreateOrUpdateCopyCampaignProfileCommand command =
                                Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                if (CampaignData.Count() > 0 && CampaignData != null)
                                {
                                    CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                }

                                //Update Campaign Profile Time Setting
                                CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                    CampaignProfileTimeSettingModel);
                                ICommandResult result1 = _commandBus.Submit(command1);

                                var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    UserMatchTableProcess obj = new UserMatchTableProcess();
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                        var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                        if (campaigndetails != null)
                                        {
                                            obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                            PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                        }
                                    }
                                }

                                var userDetails = _userRepository.GetById(efmvcUser.UserId);

                                //Email Code
                                var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
                                if (adminDetails != null)
                                {
                                    var reader =
                                        new StreamReader(
                                            Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
                                    var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
                                    string emailContent = reader.ReadToEnd();

                                    emailContent = string.Format(emailContent, campaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);

                                    MailMessage mail = new MailMessage();
                                    mail.To.Add(adminDetails.Email);
                                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                                    mail.Subject = "Campaign Verification";

                                    mail.Body = emailContent.Replace("\n", "<br/>");

                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                                    smtp.Credentials = new System.Net.NetworkCredential
                                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                                    //Or your Smtp Email ID and Password
                                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                                    smtp.Send(mail);
                                }
                                return Json("success");
                            }
                        }
                    }
                    else
                    {
                        IEnumerable<CampaignProfile> CampaignNameexists;
                        if (CampaignProfileID == 0)
                        {
                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                        }
                        else
                        {
                            CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                        }
                        if (CampaignNameexists.Count() > 0)
                        {
                            FillCampaign(efmvcUser.UserId);
                            FillCountry();

                            return Json("Exists");
                        }
                        else
                        {
                            model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                            model.UserId = efmvcUser.UserId;
                            model.ClientId = null;
                            model.CampaignName = campaignName;
                            model.CampaignDescription = campaignDescription;
                            model.TotalBudget = decimal.Parse("0.00");
                            model.MaxDailyBudget = float.Parse("0.00");
                            model.MaxBid = float.Parse("0.00");
                            model.MaxMonthBudget = float.Parse("0.00");
                            model.MaxWeeklyBudget = float.Parse("0.00");
                            model.MaxHourlyBudget = float.Parse("0.00");
                            model.TotalCredit = decimal.Parse("0.00");
                            model.SpendToDate = float.Parse("0.00");
                            model.AvailableCredit = decimal.Parse("0.00");
                            model.PlaysToDate = int.Parse("0");
                            model.PlaysLastMonth = int.Parse("0");
                            model.PlaysCurrentMonth = int.Parse("0");
                            model.CancelledToDate = int.Parse("0");
                            model.CancelledLastMonth = int.Parse("0");
                            model.CancelledCurrentMonth = int.Parse("0");
                            model.SmsToDate = int.Parse("0");
                            model.SmsLastMonth = int.Parse("0");
                            model.SmsCurrentMonth = int.Parse("0");
                            model.EmailToDate = int.Parse("0");
                            model.EmailsLastMonth = int.Parse("0");
                            model.EmailsCurrentMonth = int.Parse("0");
                            model.EmailFileLocation = null;
                            model.Active = true;
                            model.NumberOfPlays = int.Parse("0");
                            model.AverageDailyPlays = int.Parse("0");
                            model.SmsRequests = int.Parse("0");
                            model.EmailsDelievered = int.Parse("0");
                            model.EmailSubject = null;
                            model.EmailBody = null;
                            model.EmailSenderAddress = null;
                            model.SmsOriginator = null;
                            model.SmsBody = null;
                            model.SMSFileLocation = null;
                            model.CreatedDateTime = DateTime.Now;
                            model.UpdatedDateTime = DateTime.Now;
                            model.Status = (int)CampaignStatus.InProgress;
                            model.StartDate = null;
                            model.EndDate = null;
                            model.NumberInBatch = 0;
                            model.CountryId = int.Parse(countryId);
                            model.IsAdminApproval = false;
                            model.RemainingMaxDailyBudget = float.Parse("0.00"); ;
                            model.RemainingMaxHourlyBudget = float.Parse("0.00"); ;
                            model.RemainingMaxWeeklyBudget = float.Parse("0.00");
                            model.RemainingMaxMonthBudget = float.Parse("0.00");
                            model.ProvidendSpendAmount = decimal.Parse("0.00");
                            model.BucketCount = int.Parse("0");
                            model.PhoneticAlphabet = phoneticAlphabet;
                            model.NextStatus = true;
                            model.AdtoneServerCampaignProfileId = null;
                            model.CurrencyCode = currencyCode;

                            CreateOrUpdateCopyCampaignProfileCommand command =
                                Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                if (CampaignData.Count() > 0 && CampaignData != null)
                                {
                                    CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                }

                                //Update Campaign Profile Time Setting
                                CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                    CampaignProfileTimeSettingModel);
                                ICommandResult result1 = _commandBus.Submit(command1);

                                var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    UserMatchTableProcess obj = new UserMatchTableProcess();
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                        var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                        if (campaigndetails != null)
                                        {
                                            obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                            PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                        }
                                    }
                                }

                                var userDetails = _userRepository.GetById(efmvcUser.UserId);

                                //Email Code
                                var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
                                if (adminDetails != null)
                                {
                                    var reader =
                                        new StreamReader(
                                            Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
                                    var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
                                    string emailContent = reader.ReadToEnd();

                                    emailContent = string.Format(emailContent, campaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);

                                    MailMessage mail = new MailMessage();
                                    mail.To.Add(adminDetails.Email);
                                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                                    mail.Subject = "Campaign Verification";

                                    mail.Body = emailContent.Replace("\n", "<br/>");

                                    mail.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                                    smtp.Credentials = new System.Net.NetworkCredential
                                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                                    //Or your Smtp Email ID and Password
                                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                                    smtp.Send(mail);
                                }
                                return Json("success");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException.Message;
                    return Json("fail");
                }
                return Json("");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter] // Get Data
        public ActionResult AddClientData(string campaignId, string campaignName, string campaignDescription, string phoneticAlphabet, string countryId, string clientCheck)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
                    EFMVCDataContex db = new EFMVCDataContex();
                    PostedTimesModel postedTimesModel = new PostedTimesModel();
                    postedTimesModel.DayIds = new string[] { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00" };
                    int CountryId = 0;
                    if (countryId == "12" || countryId == "13" || countryId == "14")
                    {
                        CountryId = 12;
                    }
                    else if (countryId == "11")
                    {
                        CountryId = 8;
                    }
                    else
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;

                    if (clientCheck == "true")
                    {
                        if (campaignId != "")
                        {
                            IEnumerable<CampaignProfile> CampaignNameexists;
                            if (CampaignProfileID == 0)
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                            }
                            else
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                            }
                            if (CampaignNameexists.Count() > 0)
                            {
                                FillCampaign(efmvcUser.UserId);
                                FillCountry();

                                return Json("Exists");
                            }
                            else
                            {
                                var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                                model.UserId = campaignDetail.UserId;
                                model.ClientId = campaignDetail.ClientId;
                                model.CampaignName = campaignName;
                                model.CampaignDescription = campaignDescription;
                                model.TotalBudget = campaignDetail.TotalBudget;
                                model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
                                model.MaxBid = campaignDetail.MaxBid;
                                model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
                                model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
                                model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
                                model.TotalCredit = campaignDetail.TotalCredit;
                                model.SpendToDate = campaignDetail.SpendToDate;
                                model.AvailableCredit = campaignDetail.AvailableCredit;
                                model.PlaysToDate = campaignDetail.PlaysToDate;
                                model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
                                model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
                                model.CancelledToDate = campaignDetail.CancelledToDate;
                                model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
                                model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
                                model.SmsToDate = campaignDetail.SmsToDate;
                                model.SmsLastMonth = campaignDetail.SmsLastMonth;
                                model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
                                model.EmailToDate = campaignDetail.EmailToDate;
                                model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
                                model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
                                model.EmailFileLocation = campaignDetail.EmailFileLocation;
                                model.Active = campaignDetail.Active;
                                model.NumberOfPlays = campaignDetail.NumberOfPlays;
                                model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
                                model.SmsRequests = campaignDetail.SmsRequests;
                                model.EmailsDelievered = campaignDetail.EmailsDelievered;
                                model.EmailSubject = campaignDetail.EmailSubject;
                                model.EmailBody = campaignDetail.EmailBody;
                                model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
                                model.SmsOriginator = campaignDetail.SmsOriginator;
                                model.SmsBody = campaignDetail.SmsBody;
                                model.SMSFileLocation = campaignDetail.SMSFileLocation;
                                model.CreatedDateTime = campaignDetail.CreatedDateTime;
                                model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
                                model.Status = (int)CampaignStatus.InProgress;
                                model.StartDate = campaignDetail.StartDate;
                                model.EndDate = campaignDetail.EndDate;
                                model.NumberInBatch = campaignDetail.NumberInBatch;
                                model.CountryId = int.Parse(countryId);
                                model.IsAdminApproval = false;
                                model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
                                model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
                                model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
                                model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
                                model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
                                model.BucketCount = campaignDetail.BucketCount;
                                model.PhoneticAlphabet = phoneticAlphabet;
                                model.NextStatus = true;
                                model.CurrencyCode = currencyCode;

                                CreateOrUpdateCopyCampaignProfileCommand command =
                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                                ICommandResult result = _commandBus.Submit(command);
                                if (result.Success)
                                {
                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                    if (CampaignData.Count() > 0 && CampaignData != null)
                                    {
                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                    }

                                    //Update Campaign Profile Time Setting
                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                        CampaignProfileTimeSettingModel);
                                    ICommandResult result1 = _commandBus.Submit(command1);

                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        UserMatchTableProcess obj = new UserMatchTableProcess();
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                            if (campaigndetails != null)
                                            {
                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                            }
                                        }
                                    }

                                    FillClient(efmvcUser.UserId);
                                    return Json(new { success = "successjson" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            IEnumerable<CampaignProfile> CampaignNameexists;
                            if (CampaignProfileID == 0)
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                            }
                            else
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                            }
                            if (CampaignNameexists.Count() > 0)
                            {
                                FillCampaign(efmvcUser.UserId);
                                FillCountry();

                                return Json("Exists");
                            }
                            else
                            {
                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                                model.UserId = efmvcUser.UserId;
                                model.ClientId = null;
                                model.CampaignName = campaignName;
                                model.CampaignDescription = campaignDescription;
                                model.TotalBudget = decimal.Parse("0.00");
                                model.MaxDailyBudget = float.Parse("0.00");
                                model.MaxBid = float.Parse("0.00");
                                model.MaxMonthBudget = float.Parse("0.00");
                                model.MaxWeeklyBudget = float.Parse("0.00");
                                model.MaxHourlyBudget = float.Parse("0.00");
                                model.TotalCredit = decimal.Parse("0.00");
                                model.SpendToDate = float.Parse("0.00");
                                model.AvailableCredit = decimal.Parse("0.00");
                                model.PlaysToDate = int.Parse("0");
                                model.PlaysLastMonth = int.Parse("0");
                                model.PlaysCurrentMonth = int.Parse("0");
                                model.CancelledToDate = int.Parse("0");
                                model.CancelledLastMonth = int.Parse("0");
                                model.CancelledCurrentMonth = int.Parse("0");
                                model.SmsToDate = int.Parse("0");
                                model.SmsLastMonth = int.Parse("0");
                                model.SmsCurrentMonth = int.Parse("0");
                                model.EmailToDate = int.Parse("0");
                                model.EmailsLastMonth = int.Parse("0");
                                model.EmailsCurrentMonth = int.Parse("0");
                                model.EmailFileLocation = null;
                                model.Active = true;
                                model.NumberOfPlays = int.Parse("0");
                                model.AverageDailyPlays = int.Parse("0");
                                model.SmsRequests = int.Parse("0");
                                model.EmailsDelievered = int.Parse("0");
                                model.EmailSubject = null;
                                model.EmailBody = null;
                                model.EmailSenderAddress = null;
                                model.SmsOriginator = null;
                                model.SmsBody = null;
                                model.SMSFileLocation = null;
                                model.CreatedDateTime = DateTime.Now;
                                model.UpdatedDateTime = DateTime.Now;
                                model.Status = (int)CampaignStatus.InProgress;
                                model.StartDate = null;
                                model.EndDate = null;
                                model.NumberInBatch = 0;
                                model.CountryId = int.Parse(countryId);
                                model.IsAdminApproval = false;
                                model.RemainingMaxDailyBudget = float.Parse("0.00");
                                model.RemainingMaxHourlyBudget = float.Parse("0.00");
                                model.RemainingMaxWeeklyBudget = float.Parse("0.00");
                                model.RemainingMaxMonthBudget = float.Parse("0.00");
                                model.ProvidendSpendAmount = decimal.Parse("0.00");
                                model.BucketCount = int.Parse("0");
                                model.PhoneticAlphabet = phoneticAlphabet;
                                model.NextStatus = true;
                                model.AdtoneServerCampaignProfileId = null;
                                model.CurrencyCode = currencyCode;

                                CreateOrUpdateCopyCampaignProfileCommand command =
                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                                ICommandResult result = _commandBus.Submit(command);
                                if (result.Success)
                                {
                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                    if (CampaignData.Count() > 0 && CampaignData != null)
                                    {
                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                    }

                                    //Update Campaign Profile Time Setting
                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                        CampaignProfileTimeSettingModel);
                                    ICommandResult result1 = _commandBus.Submit(command1);

                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        UserMatchTableProcess obj = new UserMatchTableProcess();
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                            if (campaigndetails != null)
                                            {
                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                            }
                                        }
                                    }

                                    FillClient(efmvcUser.UserId);
                                    return Json(new { success = "successjson" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (campaignId != "")
                        {
                            IEnumerable<CampaignProfile> CampaignNameexists;
                            if (CampaignProfileID == 0)
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                            }
                            else
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                            }
                            if (CampaignNameexists.Count() > 0)
                            {
                                FillCampaign(efmvcUser.UserId);
                                FillCountry();

                                return Json("Exists");
                            }
                            else
                            {
                                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

                                var campaignDetail = _profileRepository.GetById(Convert.ToInt32(campaignId));
                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                                model.UserId = campaignDetail.UserId;
                                model.ClientId = campaignDetail.ClientId;
                                model.CampaignName = campaignName;
                                model.CampaignDescription = campaignDescription;
                                model.TotalBudget = campaignDetail.TotalBudget;
                                model.MaxDailyBudget = campaignDetail.MaxDailyBudget;
                                model.MaxBid = campaignDetail.MaxBid;
                                model.MaxMonthBudget = campaignDetail.MaxMonthBudget;
                                model.MaxWeeklyBudget = campaignDetail.MaxWeeklyBudget;
                                model.MaxHourlyBudget = campaignDetail.MaxHourlyBudget;
                                model.TotalCredit = campaignDetail.TotalCredit;
                                model.SpendToDate = campaignDetail.SpendToDate;
                                model.AvailableCredit = campaignDetail.AvailableCredit;
                                model.PlaysToDate = campaignDetail.PlaysToDate;
                                model.PlaysLastMonth = campaignDetail.PlaysLastMonth;
                                model.PlaysCurrentMonth = campaignDetail.PlaysCurrentMonth;
                                model.CancelledToDate = campaignDetail.CancelledToDate;
                                model.CancelledLastMonth = campaignDetail.CancelledLastMonth;
                                model.CancelledCurrentMonth = campaignDetail.CancelledCurrentMonth;
                                model.SmsToDate = campaignDetail.SmsToDate;
                                model.SmsLastMonth = campaignDetail.SmsLastMonth;
                                model.SmsCurrentMonth = campaignDetail.SmsCurrentMonth;
                                model.EmailToDate = campaignDetail.EmailToDate;
                                model.EmailsLastMonth = campaignDetail.EmailsLastMonth;
                                model.EmailsCurrentMonth = campaignDetail.EmailsCurrentMonth;
                                model.EmailFileLocation = campaignDetail.EmailFileLocation;
                                model.Active = campaignDetail.Active;
                                model.NumberOfPlays = campaignDetail.NumberOfPlays;
                                model.AverageDailyPlays = campaignDetail.AverageDailyPlays;
                                model.SmsRequests = campaignDetail.SmsRequests;
                                model.EmailsDelievered = campaignDetail.EmailsDelievered;
                                model.EmailSubject = campaignDetail.EmailSubject;
                                model.EmailBody = campaignDetail.EmailBody;
                                model.EmailSenderAddress = campaignDetail.EmailSenderAddress;
                                model.SmsOriginator = campaignDetail.SmsOriginator;
                                model.SmsBody = campaignDetail.SmsBody;
                                model.SMSFileLocation = campaignDetail.SMSFileLocation;
                                model.CreatedDateTime = campaignDetail.CreatedDateTime;
                                model.UpdatedDateTime = campaignDetail.UpdatedDateTime;
                                model.Status = (int)CampaignStatus.InProgress;
                                model.StartDate = campaignDetail.StartDate;
                                model.EndDate = campaignDetail.EndDate;
                                model.NumberInBatch = campaignDetail.NumberInBatch;
                                model.CountryId = int.Parse(countryId);
                                model.IsAdminApproval = false;
                                model.RemainingMaxDailyBudget = campaignDetail.RemainingMaxDailyBudget;
                                model.RemainingMaxHourlyBudget = campaignDetail.RemainingMaxHourlyBudget;
                                model.RemainingMaxWeeklyBudget = campaignDetail.RemainingMaxWeeklyBudget;
                                model.RemainingMaxMonthBudget = campaignDetail.RemainingMaxMonthBudget;
                                model.ProvidendSpendAmount = campaignDetail.ProvidendSpendAmount;
                                model.BucketCount = campaignDetail.BucketCount;
                                model.PhoneticAlphabet = phoneticAlphabet;
                                model.NextStatus = true;
                                model.CurrencyCode = currencyCode;

                                CreateOrUpdateCopyCampaignProfileCommand command =
                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                                ICommandResult result = _commandBus.Submit(command);
                                if (result.Success)
                                {
                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                    if (CampaignData.Count() > 0 && CampaignData != null)
                                    {
                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                    }

                                    //Update Campaign Profile Time Setting
                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                        CampaignProfileTimeSettingModel);
                                    ICommandResult result1 = _commandBus.Submit(command1);

                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        UserMatchTableProcess obj = new UserMatchTableProcess();
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                            if (campaigndetails != null)
                                            {
                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                            }
                                        }
                                    }

                                    return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            IEnumerable<CampaignProfile> CampaignNameexists;
                            if (CampaignProfileID == 0)
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).ToList();
                            }
                            else
                            {
                                CampaignNameexists = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.CampaignProfileId != CampaignProfileID).ToList();
                            }
                            if (CampaignNameexists.Count() > 0)
                            {
                                FillCampaign(efmvcUser.UserId);
                                FillCountry();

                                return Json("Exists");
                            }
                            else
                            {
                                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

                                model.CampaignProfileId = CampaignProfileID == 0 ? 0 : CampaignProfileID;
                                model.UserId = efmvcUser.UserId;
                                model.ClientId = null;
                                model.CampaignName = campaignName;
                                model.CampaignDescription = campaignDescription;
                                model.TotalBudget = decimal.Parse("0.00");
                                model.MaxDailyBudget = float.Parse("0.00");
                                model.MaxBid = float.Parse("0.00");
                                model.MaxMonthBudget = float.Parse("0.00");
                                model.MaxWeeklyBudget = float.Parse("0.00");
                                model.MaxHourlyBudget = float.Parse("0.00");
                                model.TotalCredit = decimal.Parse("0.00");
                                model.SpendToDate = float.Parse("0.00");
                                model.AvailableCredit = decimal.Parse("0.00");
                                model.PlaysToDate = int.Parse("0");
                                model.PlaysLastMonth = int.Parse("0");
                                model.PlaysCurrentMonth = int.Parse("0");
                                model.CancelledToDate = int.Parse("0");
                                model.CancelledLastMonth = int.Parse("0");
                                model.CancelledCurrentMonth = int.Parse("0");
                                model.SmsToDate = int.Parse("0");
                                model.SmsLastMonth = int.Parse("0");
                                model.SmsCurrentMonth = int.Parse("0");
                                model.EmailToDate = int.Parse("0");
                                model.EmailsLastMonth = int.Parse("0");
                                model.EmailsCurrentMonth = int.Parse("0");
                                model.EmailFileLocation = null;
                                model.Active = true;
                                model.NumberOfPlays = int.Parse("0");
                                model.AverageDailyPlays = int.Parse("0");
                                model.SmsRequests = int.Parse("0");
                                model.EmailsDelievered = int.Parse("0");
                                model.EmailSubject = null;
                                model.EmailBody = null;
                                model.EmailSenderAddress = null;
                                model.SmsOriginator = null;
                                model.SmsBody = null;
                                model.SMSFileLocation = null;
                                model.CreatedDateTime = DateTime.Now;
                                model.UpdatedDateTime = DateTime.Now;
                                model.Status = (int)CampaignStatus.InProgress;
                                model.StartDate = null;
                                model.EndDate = null;
                                model.NumberInBatch = 0;
                                model.CountryId = int.Parse(countryId);
                                model.IsAdminApproval = false;
                                model.RemainingMaxDailyBudget = float.Parse("0.00");
                                model.RemainingMaxHourlyBudget = float.Parse("0.00");
                                model.RemainingMaxWeeklyBudget = float.Parse("0.00");
                                model.RemainingMaxMonthBudget = float.Parse("0.00");
                                model.ProvidendSpendAmount = decimal.Parse("0.00");
                                model.BucketCount = int.Parse("0");
                                model.PhoneticAlphabet = phoneticAlphabet;
                                model.NextStatus = true;
                                model.AdtoneServerCampaignProfileId = null;
                                model.CurrencyCode = currencyCode;

                                CreateOrUpdateCopyCampaignProfileCommand command =
                                    Mapper.Map<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>(model);

                                ICommandResult result = _commandBus.Submit(command);
                                if (result.Success)
                                {
                                    var CampaignData = _profileRepository.GetAll().Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                    if (CampaignData.Count() > 0 && CampaignData != null)
                                    {
                                        CampaignProfileID = CampaignData.FirstOrDefault().CampaignProfileId;
                                    }

                                    //Update Campaign Profile Time Setting
                                    CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();
                                    CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(CampaignProfileID);
                                    CampaignProfileTimeSettingModel.MondayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.TuesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.WednesdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.ThursdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.FridayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SaturdayPostedTimes = postedTimesModel;
                                    CampaignProfileTimeSettingModel.SundayPostedTimes = postedTimesModel;
                                    CreateOrUpdateCampaignProfileTimeSettingCommand command1 =
                                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                                        CampaignProfileTimeSettingModel);
                                    ICommandResult result1 = _commandBus.Submit(command1);

                                    var ConnString = ConnectionString.GetConnectionStringByCountryId(model.CountryId);
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        UserMatchTableProcess obj = new UserMatchTableProcess();
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == CampaignProfileID).FirstOrDefault();
                                            if (campaigndetails != null)
                                            {
                                                obj.AddCampaignData(campaigndetails, SQLServerEntities);
                                                PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                            }
                                        }
                                    }

                                    return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    return Json("");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter] // Save Data
        public ActionResult AddClientInfo(string campaignName, string clientId, string clientName, string clientDescription, string clientEmail, string clientContactPhone, string phoneticAlphabet, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            if (efmvcUser != null)
            {
                NewClientFormModel model = new NewClientFormModel();
                EFMVCDataContex db = new EFMVCDataContex();

                int ClientId = Convert.ToInt32(clientId);
                NewAdvertFormModel model1 = new NewAdvertFormModel();
                NewAdvertRejectionFormModel model2 = new NewAdvertRejectionFormModel();

                if (clientId != null && clientId != "0" && clientId != "")
                {
                    try
                    {
                        IEnumerable<Client> clientNameexists;
                        if (ClientID == 0)
                        {
                            clientNameexists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId).ToList();
                        }
                        else
                        {
                            clientNameexists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.Id == ClientID).ToList();
                        }
                        if (clientNameexists.Count() > 0)
                        {
                            FillClient(efmvcUser.UserId);

                            return Json("Exists");
                        }
                        else
                        {
                            var clientDetail = _clientRepository.GetById(Convert.ToInt32(clientId));
                            model.ClientId = ClientID == 0 ? 0 : ClientID;
                            model.UserId = efmvcUser.UserId;
                            model.ClientName = clientName;
                            model.ClientDescription = clientDescription;
                            model.ClientContactInfo = "";
                            model.ClientBudget = clientDetail.Budget;
                            model.CreatedDate = (DateTime)clientDetail.CreatedDate;
                            model.UpdatedDate = (DateTime)clientDetail.UpdatedDate;
                            model.ClientStatus = (int)ClientStatus.InProgress;
                            model.ClientEmail = clientEmail;
                            model.ClientPhoneticAlphabet = phoneticAlphabet;
                            model.ClientContactPhone = clientContactPhone;
                            model.NextStatus = false;
                            model.CountryId = Convert.ToInt32(countryId);
                            model.AdtoneServerClientId = null;

                            CreateOrUpdateCopyClientCommand command =
                                Mapper.Map<NewClientFormModel, CreateOrUpdateCopyClientCommand>(model);

                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                var ClientData = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == phoneticAlphabet).ToList();
                                if (ClientData.Count() > 0 && ClientData != null)
                                {
                                    ClientID = ClientData.FirstOrDefault().Id;
                                }

                                var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                                if (campaign != null)
                                {
                                    campaign.ClientId = result.Id;
                                    db.SaveChanges();
                                }

                                return Json("success");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.InnerException.Message;
                        return Json("fail");
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter] // Next Client Data
        public ActionResult AddBudgetData(string campaignName, string clientId, string clientName, string clientDescription, string clientEmail, string clientContactPhone, string countryId, string clientPhoneticAlphabet)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                EFMVCDataContex db = new EFMVCDataContex();
                NewClientFormModel model = new NewClientFormModel();
                int ClientId = Convert.ToInt32(clientId);
                NewAdvertFormModel model1 = new NewAdvertFormModel();
                NewAdvertRejectionFormModel model2 = new NewAdvertRejectionFormModel();

                if (clientId != "")
                {
                    try
                    {
                        int CountryId = 0;
                        if (countryId == "12" || countryId == "13" || countryId == "14")
                        {
                            CountryId = 12;
                        }
                        else if (countryId == "11")
                        {
                            CountryId = 8;
                        }
                        else
                        {
                            CountryId = Convert.ToInt32(countryId);
                        }
                        var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;
                        var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;

                        IEnumerable<Client> clientNameExists;
                        if (ClientID == 0)
                        {
                            clientNameExists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId).ToList();
                        }
                        else
                        {
                            clientNameExists = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.Id != ClientID).ToList();
                        }
                        if (clientNameExists.Count() > 0)
                        {
                            FillClient(efmvcUser.UserId);

                            return Json("Exists");
                        }
                        else
                        {
                            var clientDetail = _clientRepository.GetById(Convert.ToInt32(clientId));
                            model.ClientId = ClientID == 0 ? 0 : ClientID;
                            model.UserId = efmvcUser.UserId;
                            model.ClientName = clientName;
                            model.ClientDescription = clientDescription;
                            model.ClientContactInfo = "";
                            model.ClientBudget = clientDetail.Budget;
                            model.CreatedDate = (DateTime)clientDetail.CreatedDate;
                            model.UpdatedDate = (DateTime)clientDetail.UpdatedDate;
                            model.ClientStatus = (int)ClientStatus.InProgress;
                            model.ClientEmail = clientEmail;
                            model.ClientPhoneticAlphabet = clientPhoneticAlphabet;
                            model.ClientContactPhone = clientContactPhone;
                            model.NextStatus = true;
                            model.CountryId = Convert.ToInt32(countryId);
                            model.AdtoneServerClientId = null;

                            CreateOrUpdateCopyClientCommand command =
                                Mapper.Map<NewClientFormModel, CreateOrUpdateCopyClientCommand>(model);

                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                var ClientData = _clientRepository.GetAll().Where(c => c.Name == clientName && c.UserId == efmvcUser.UserId && c.PhoneticAlphabet == clientPhoneticAlphabet).ToList();
                                if (ClientData.Count() > 0 && ClientData != null)
                                {
                                    ClientID = ClientData.FirstOrDefault().Id;
                                }

                                var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                                if (campaign != null)
                                {
                                    campaign.ClientId = result.Id;
                                    db.SaveChanges();
                                }

                                return Json(new { success = "successbudget", value = currencyCode, value1 = currencyId }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.InnerException.Message;
                        return Json("fail");
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddBudgetInfo(string campaignName, string countryId, string currencyId, string monthlyBudget, string weeklyBudget, string dailyBudget, string hourlyBudget, string maximumBid, string clientName)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (campaignName != "" && maximumBid != "")
                {
                    try
                    {
                        int? CountryId = null;
                        if (countryId != "") CountryId = Convert.ToInt32(countryId);
                        var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
                        NewCampaignProfileFormModel model = new NewCampaignProfileFormModel();
                        EFMVCDataContex db = new EFMVCDataContex();
                        CurrencyModel currencyModel = new CurrencyModel();
                        var client = _clientRepository.Get(c => c.Name == clientName);
                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (campaign != null)
                        {
                            if (client != null) campaign.ClientId = client.Id;
                            //Code For Currency Conversion
                            decimal currencyRate = 0.00M;
                            int Countryid = Convert.ToInt32(countryId);
                            int CurrencyId = Convert.ToInt32(currencyId);
                            var currencyData = _currencyRepository.Get(c => c.CurrencyId == CurrencyId);
                            var currencyCountryId = currencyData.Country.Id;
                            var fromCurrencyCode = currencyData.CurrencyCode;
                            var toCurrencyCode = _currencyRepository.Get(c => c.CountryId == Countryid).CurrencyCode;
                            if (currencyCountryId == Countryid)
                            {
                                campaign.MaxDailyBudget = float.Parse(dailyBudget);
                                campaign.MaxBid = float.Parse(maximumBid);
                                campaign.MaxMonthBudget = float.Parse(monthlyBudget);
                                campaign.MaxWeeklyBudget = float.Parse(weeklyBudget);
                                campaign.MaxHourlyBudget = float.Parse(hourlyBudget);
                                campaign.RemainingMaxDailyBudget = float.Parse(dailyBudget);
                                campaign.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
                                campaign.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
                                campaign.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
                            }
                            else
                            {
                                //passed static 1 value to avoid multiple api call
                                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                                currencyRate = currencyModel.Amount;
                                if (currencyModel.Code == "OK")
                                {
                                    campaign.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                    campaign.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                    campaign.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                    campaign.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                    campaign.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                    campaign.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                    campaign.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                    campaign.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                    campaign.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                }
                                else
                                {
                                    return Json("fail");
                                }
                            }
                            campaign.CurrencyCode = toCurrencyCode;
                            campaign.UpdatedDateTime = System.DateTime.Now;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                UserMatchTableProcess obj = new UserMatchTableProcess();
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                    if (campaignProfileDetails != null)
                                    {
                                        CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
                                        if (client != null)
                                        {
                                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, client.Id);
                                            int? operatorClientId;
                                            if (externalServerClientId == 0) operatorClientId = null;
                                            else operatorClientId = externalServerClientId;
                                            campaignProfileDetails.ClientId = operatorClientId;
                                            campaignProfileFormModel.ClientId = operatorClientId;
                                        }
                                        if (currencyCountryId == Countryid)
                                        {
                                            campaignProfileDetails.MaxDailyBudget = float.Parse(dailyBudget);
                                            campaignProfileDetails.MaxBid = float.Parse(maximumBid);
                                            campaignProfileDetails.MaxMonthBudget = float.Parse(monthlyBudget);
                                            campaignProfileDetails.MaxWeeklyBudget = float.Parse(weeklyBudget);
                                            campaignProfileDetails.MaxHourlyBudget = float.Parse(hourlyBudget);
                                            campaignProfileDetails.RemainingMaxDailyBudget = float.Parse(dailyBudget);
                                            campaignProfileDetails.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
                                            campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
                                            campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
                                            campaignProfileFormModel.MaxBid = float.Parse(maximumBid);
                                            campaignProfileFormModel.MaxMonthBudget = float.Parse(monthlyBudget);
                                            campaignProfileFormModel.MaxWeeklyBudget = float.Parse(weeklyBudget);
                                            campaignProfileFormModel.MaxHourlyBudget = float.Parse(hourlyBudget);
                                            campaignProfileFormModel.MaxDailyBudget = float.Parse(dailyBudget);
                                        }
                                        else
                                        {
                                            if (currencyModel.Code == "OK")
                                            {
                                                campaignProfileDetails.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                                campaignProfileDetails.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                                campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                                campaignProfileFormModel.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                                campaignProfileFormModel.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                                campaignProfileFormModel.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                                campaignProfileFormModel.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                                campaignProfileFormModel.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                            }
                                        }
                                        campaignProfileDetails.CurrencyCode = toCurrencyCode;
                                        campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
                                        db1.SaveChanges();
                                        obj.UpdateCampaignBudgetInfo(campaignProfileFormModel, campaignProfileDetails, efmvcUser.UserId, db1);
                                        PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                    }
                                }
                            }
                            return Json("success");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.InnerException.Message;
                        return Json("fail");
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddAdvertData(string campaignName, string countryId, string currencyId, string monthlyBudget, string weeklyBudget, string dailyBudget, string hourlyBudget, string maximumBid, string clientName)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    int? CountryId = null;
                    if (countryId != "") CountryId = Convert.ToInt32(countryId);
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
                    EFMVCDataContex db = new EFMVCDataContex();
                    CurrencyModel currencyModel = new CurrencyModel();
                    var client = _clientRepository.Get(c => c.Name == clientName && c.UserId == efmvcUser.UserId);
                    var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                    if (campaign != null)
                    {
                        if (client != null) campaign.ClientId = client.Id;
                        //Code For Currency Conversion
                        decimal currencyRate = 0.00M;
                        int Countryid = Convert.ToInt32(countryId);
                        int CurrencyId = Convert.ToInt32(currencyId);
                        var currencyData = _currencyRepository.Get(c => c.CurrencyId == CurrencyId);
                        var currencyCountryId = currencyData.Country.Id;
                        var fromCurrencyCode = currencyData.CurrencyCode;
                        var toCurrencyCode = _currencyRepository.Get(c => c.CountryId == Countryid).CurrencyCode;
                        if (currencyCountryId == Countryid)
                        {
                            campaign.MaxDailyBudget = float.Parse(dailyBudget);
                            campaign.MaxBid = float.Parse(maximumBid);
                            campaign.MaxMonthBudget = float.Parse(monthlyBudget);
                            campaign.MaxWeeklyBudget = float.Parse(weeklyBudget);
                            campaign.MaxHourlyBudget = float.Parse(hourlyBudget);
                            campaign.RemainingMaxDailyBudget = float.Parse(dailyBudget);
                            campaign.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
                            campaign.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
                            campaign.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
                        }
                        else
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK")
                            {
                                campaign.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                campaign.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                campaign.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                campaign.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                campaign.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                campaign.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                campaign.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                campaign.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                campaign.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                            }
                            else
                            {
                                return Json("fail");
                            }
                        }
                        campaign.CurrencyCode = toCurrencyCode;
                        campaign.UpdatedDateTime = System.DateTime.Now;
                        db.SaveChanges();
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex db1 = new EFMVCDataContex(item);
                                var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                if (campaignProfileDetails != null)
                                {
                                    CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
                                    if (client != null)
                                    {
                                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, client.Id);
                                        int? operatorClientId;
                                        if (externalServerClientId == 0) operatorClientId = null;
                                        else operatorClientId = externalServerClientId;
                                        campaignProfileDetails.ClientId = operatorClientId;
                                        campaignProfileFormModel.ClientId = operatorClientId;
                                    }
                                    if (currencyCountryId == Countryid)
                                    {
                                        campaignProfileDetails.MaxDailyBudget = float.Parse(dailyBudget);
                                        campaignProfileDetails.MaxBid = float.Parse(maximumBid);
                                        campaignProfileDetails.MaxMonthBudget = float.Parse(monthlyBudget);
                                        campaignProfileDetails.MaxWeeklyBudget = float.Parse(weeklyBudget);
                                        campaignProfileDetails.MaxHourlyBudget = float.Parse(hourlyBudget);
                                        campaignProfileDetails.RemainingMaxDailyBudget = float.Parse(dailyBudget);
                                        campaignProfileDetails.RemainingMaxMonthBudget = float.Parse(monthlyBudget);
                                        campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse(weeklyBudget);
                                        campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse(hourlyBudget);
                                        campaignProfileFormModel.MaxBid = float.Parse(maximumBid);
                                        campaignProfileFormModel.MaxMonthBudget = float.Parse(monthlyBudget);
                                        campaignProfileFormModel.MaxWeeklyBudget = float.Parse(weeklyBudget);
                                        campaignProfileFormModel.MaxHourlyBudget = float.Parse(hourlyBudget);
                                        campaignProfileFormModel.MaxDailyBudget = float.Parse(dailyBudget);
                                    }
                                    else
                                    {
                                        if (currencyModel.Code == "OK")
                                        {
                                            campaignProfileDetails.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                            campaignProfileDetails.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.RemainingMaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.RemainingMaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.RemainingMaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                            campaignProfileDetails.RemainingMaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                            campaignProfileFormModel.MaxBid = float.Parse((Convert.ToDecimal(maximumBid) * currencyRate).ToString());
                                            campaignProfileFormModel.MaxMonthBudget = float.Parse((Convert.ToDecimal(monthlyBudget) * currencyRate).ToString());
                                            campaignProfileFormModel.MaxWeeklyBudget = float.Parse((Convert.ToDecimal(weeklyBudget) * currencyRate).ToString());
                                            campaignProfileFormModel.MaxHourlyBudget = float.Parse((Convert.ToDecimal(hourlyBudget) * currencyRate).ToString());
                                            campaignProfileFormModel.MaxDailyBudget = float.Parse((Convert.ToDecimal(dailyBudget) * currencyRate).ToString());
                                        }
                                    }
                                    campaignProfileDetails.CurrencyCode = toCurrencyCode;
                                    campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
                                    db1.SaveChanges();
                                    obj.UpdateCampaignBudgetInfo(campaignProfileFormModel, campaignProfileDetails, efmvcUser.UserId, db1);
                                    PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                }
                            }
                        }
                        var ClientDetails = _clientRepository.GetAll().Where(s => s.UserId == efmvcUser.UserId).Select(top => new SelectListItem { Text = top.Name, Value = top.Id.ToString() }).ToList();
                        ClientDetails.Insert(0, new SelectListItem { Text = "-- Select Client --", Value = "" });
                        var userData = _userRepository.GetById(efmvcUser.UserId);
                        if (userData.OperatorId != 0)
                        {
                            var userCountryId = _operatorRepository.GetById(userData.OperatorId).CountryId.Value;
                            FillAdvertCategory(userCountryId);
                        }
                        else
                        {
                            FillAdvertCategory(0);
                        }
                        var batch = 1;
                        var countryData = _countryRepository.GetById(Convert.ToInt32(countryId.ToString())).TermAndConditionFileName;
                        var tnc = "";
                        if (!string.IsNullOrEmpty(countryData))
                        {
                            tnc = countryData;
                        }
                        return Json(new { success = "success", value2 = batch, value3 = tnc, value4 = campaign.ClientId, value5 = ClientDetails }, JsonRequestBehavior.AllowGet);
                    }
                    return Json("fail");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddAdvertInfo(string campaignName, string advertPhoneticAlphabet, string advertId, string advertName, string advertClientId, string advertBrandName, string advertCategoryId, string script, string numberofadsinabatch, HttpPostedFileBase mediaFile, HttpPostedFileBase scriptFile, string countryId, string operatorId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                string countryName = _countryRepository.GetById(Convert.ToInt32(countryId)).Name;
                string operatorName = _operatorRepository.GetById(Convert.ToInt32(operatorId)).OperatorName;

                NewAdvertFormModel model = new NewAdvertFormModel();
                EFMVCDataContex db = new EFMVCDataContex();
                AdvertEmail advertEmail = new AdvertEmail(_commandBus, _userRepository);

                int? CountryId = null;
                if (countryId != "")
                {
                    CountryId = Convert.ToInt32(countryId);
                }
                var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                if (advertId != "")
                {
                    advertId = advertId == "" ? "0" : advertId;
                    int advertID = Convert.ToInt32(advertId);
                    IEnumerable<Advert> AdvertNameexists;
                    if (advertId == "0")
                    {
                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == advertID).ToList();
                    }
                    else
                    {
                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != advertID).ToList();
                    }
                    if (AdvertNameexists.Count() > 0)
                    {
                        FillAdvertList(efmvcUser.UserId);
                        FillClient(efmvcUser.UserId);
                        FillAdvertCategory(efmvcUser.UserId);
                        FillOperator(Convert.ToInt32(countryId));

                        return Json("Exists");
                    }
                    else
                    {
                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                        int campaignId = 0;

                        if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
                        {
                            campaignId = Convert.ToInt32(campaign.CampaignProfileId);
                        }

                        int? clientId = null;
                        if (advertClientId == "")
                        {
                            clientId = null;
                        }
                        else
                        {
                            clientId = Convert.ToInt32(advertClientId);
                        }

                        var advertDetail = _advertRepository.GetById(Convert.ToInt32(advertId));
                        model.AdvertId = 0;
                        model.UserId = efmvcUser.UserId;
                        model.AdvertClientId = clientId;
                        model.AdvertName = advertName;
                        model.BrandName = advertBrandName;
                        model.UploadedToMediaServer = false;
                        model.CreatedDateTime = DateTime.Now;
                        model.UpdatedDateTime = DateTime.Now;
                        model.Status = (int)AdvertStatus.Waitingforapproval;
                        model.Script = script;
                        model.IsAdminApproval = false;
                        model.AdvertCategoryId = int.Parse(advertCategoryId);
                        model.CountryId = int.Parse(countryId);
                        model.PhoneticAlphabet = advertPhoneticAlphabet;
                        model.NextStatus = false;
                        model.CampProfileId = campaignId;
                        model.AdtoneServerAdvertId = null;
                        model.OperatorId = Convert.ToInt32(operatorId);

                        #region Media
                        if (mediaFile != null)
                        {
                            if (mediaFile.ContentLength != 0)
                            {
                                var userData = _userRepository.GetById(efmvcUser.UserId);
                                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                string fileName = firstAudioName;

                                string fileName2 = null;
                                if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                                {
                                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                    fileName2 = secondAudioName.ToString();
                                }

                                string extension = Path.GetExtension(mediaFile.FileName);

                                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                string outputFormat = "wav";
                                var audioFormatExtension = "." + outputFormat;

                                if (extension != audioFormatExtension)
                                {
                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                    string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                    mediaFile.SaveAs(tempPath);

                                    SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName + "." + outputFormat);
                                }
                                else
                                {
                                    string directoryName = Server.MapPath("~/Media/");
                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                    if (!Directory.Exists(directoryName))
                                        Directory.CreateDirectory(directoryName);

                                    string path = Path.Combine(directoryName, fileName + extension);
                                    mediaFile.SaveAs(path);

                                    StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
                                    string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                                    if (!Directory.Exists(archiveDirectoryName))
                                        Directory.CreateDirectory(archiveDirectoryName);

                                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                    mediaFile.SaveAs(archivePath);

                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + extension);
                                }
                            }
                        }
                        else
                        {
                            var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                            string fileName = firstAudioName;

                            string fileName2 = null;
                            if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                            {
                                var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                fileName2 = secondAudioName.ToString();
                            }

                            string outputFormat = "wav";
                            var audioFormatExtension = "." + outputFormat;
                            var extension = advertDetail.MediaFileLocation == null ? "" : advertDetail.MediaFileLocation.Split('.').LastOrDefault();
                            if (extension != "")
                            {
                                extension = "." + extension;
                                if (extension != audioFormatExtension)
                                {
                                    var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
                                    string fileName1 = firstAudioName;
                                    string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
                                    string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

                                    string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
                                    string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

                                    if (!Directory.Exists(targetPath))
                                        Directory.CreateDirectory(targetPath);
                                    if (System.IO.File.Exists(sourceFile) == true)
                                    {
                                        System.IO.File.Copy(sourceFile, destFile, true);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName1 + "." + outputFormat);
                                    }

                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                    string tempPath = Path.Combine(tempDirectoryName, fileName1 + extension);

                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName1 + "." + outputFormat);
                                }
                                else
                                {
                                    var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
                                    string fileName1 = firstAudioName;
                                    string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
                                    string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

                                    string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
                                    string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

                                    if (!Directory.Exists(targetPath))
                                        Directory.CreateDirectory(targetPath);
                                    if (System.IO.File.Exists(sourceFile) == true)
                                    {
                                        System.IO.File.Copy(sourceFile, destFile, true);
                                    }

                                    string directoryName1 = Server.MapPath("~/Media/");
                                    directoryName1 = Path.Combine(directoryName1, efmvcUser.UserId.ToString());

                                    if (!Directory.Exists(directoryName1))
                                        Directory.CreateDirectory(directoryName1);

                                    string path1 = Path.Combine(directoryName1, fileName + extension);

                                    string fileName3 = firstAudioName;
                                    string sourcePath1 = Server.MapPath("~/Media/Archive/");
                                    string targetPath1 = Server.MapPath("~/Media/Archive/");

                                    string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertmediafilename);
                                    string destFile1 = System.IO.Path.Combine(targetPath1, fileName3 + extension);

                                    if (!Directory.Exists(targetPath1))
                                        Directory.CreateDirectory(targetPath1);

                                    if (System.IO.File.Exists(sourceFile1) == true)
                                    {
                                        System.IO.File.Copy(sourceFile1, destFile1, true);
                                    }

                                    StoreSecondAudioFile(directoryName1, fileName2, outputFormat, path1);
                                    string archiveDirectoryName1 = Server.MapPath("~/Media/Archive/");

                                    if (!Directory.Exists(archiveDirectoryName1))
                                        Directory.CreateDirectory(archiveDirectoryName1);

                                    string archivePath1 = Path.Combine(archiveDirectoryName1, fileName + extension);

                                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName + "." + outputFormat);
                                }
                            }
                            else
                            {
                                model.MediaFileLocation = advertDetail.MediaFileLocation;
                            }
                        }

                        #endregion

                        #region Script
                        if (scriptFile != null)
                        {
                            if (scriptFile.ContentLength != 0)
                            {
                                string fileName = Guid.NewGuid().ToString();
                                string extension = Path.GetExtension(scriptFile.FileName);

                                string directoryName = Server.MapPath("/Script/");
                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, fileName + extension);
                                scriptFile.SaveAs(path);

                                string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                scriptFile.SaveAs(archivePath);

                                model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName + extension);
                            }
                            else
                            {
                                model.ScriptFileLocation = "";
                            }
                        }
                        else
                        {
                            string scriptfileName = Guid.NewGuid().ToString();
                            string extension1 = advertDetail.ScriptFileLocation == null ? "" : advertDetail.ScriptFileLocation.Split('.').LastOrDefault();
                            if (extension1 != "")
                            {
                                var advertscriptfilename = advertDetail.ScriptFileLocation.Split('/').LastOrDefault();
                                string fileName1 = scriptfileName;
                                string sourcePath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());
                                string targetPath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());

                                string sourceFile = System.IO.Path.Combine(sourcePath, advertscriptfilename);
                                string destFile = System.IO.Path.Combine(targetPath, advertscriptfilename);

                                if (!Directory.Exists(targetPath))
                                    Directory.CreateDirectory(targetPath);

                                if (System.IO.File.Exists(sourceFile) == true)
                                {
                                    System.IO.File.Copy(sourceFile, destFile, true);

                                    System.IO.File.Delete(fileName1 + "." + extension1); // Delete the existing file if exists
                                    System.IO.File.Move(advertscriptfilename, fileName1 + "." + extension1);
                                }

                                string directoryName = Server.MapPath("/Script/");
                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, scriptfileName + extension1);

                                string fileName3 = scriptfileName;
                                string sourcePath1 = Server.MapPath("/Script/Archive/");
                                string targetPath1 = Server.MapPath("/Script/Archive/");

                                string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertscriptfilename);
                                string destFile1 = System.IO.Path.Combine(targetPath1, advertscriptfilename);

                                if (!Directory.Exists(targetPath1))
                                    Directory.CreateDirectory(targetPath1);

                                if (System.IO.File.Exists(sourceFile1) == true)
                                {
                                    System.IO.File.Copy(sourceFile1, destFile1, true);

                                    System.IO.File.Delete(fileName3 + "." + extension1); // Delete the existing file if exists
                                    System.IO.File.Move(advertscriptfilename, fileName3 + "." + extension1);
                                }

                                string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, scriptfileName + extension1);

                                model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        scriptfileName + extension1);
                            }
                            else
                            {
                                model.ScriptFileLocation = advertDetail.ScriptFileLocation;
                            }
                        }

                        #endregion

                        CreateOrUpdateCopyAdvertCommand command =
                            Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

                        ICommandResult result = _commandBus.Submit(command);

                        if (result.Success)
                        {
                            if (campaignId != 0)
                            {
                                CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                                _campaignAdvert.AdvertId = result.Id;
                                _campaignAdvert.CampaignProfileId = campaignId;
                                _campaignAdvert.NextStatus = true;
                                CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                                Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

                                ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

                                if (campaignAdvertcommandResult.Success)
                                {
                                    if (campaign != null)
                                    {
                                        campaign.NumberInBatch = int.Parse(numberofadsinabatch);
                                        campaign.UpdatedDateTime = DateTime.Now;
                                        db.SaveChanges();
                                        if (ConnString != null && ConnString.Count() > 0)
                                        {
                                            UserMatchTableProcess obj = new UserMatchTableProcess();
                                            string adName = "";
                                            if (command.MediaFileLocation == null || command.MediaFileLocation == "")
                                            {
                                                adName = "";
                                            }
                                            else
                                            {
                                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                                                var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
                                                var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                                adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
                                            }
                                            foreach (var item in ConnString)
                                            {
                                                EFMVCDataContex db1 = new EFMVCDataContex(item);
                                                var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                                if (campaignProfileDetails != null)
                                                {
                                                    campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
                                                    campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                                    db1.SaveChanges();

                                                    obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
                                                    PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                                }
                                            }
                                        }
                                        //Email Code
                                        //advertEmail.SendMail(advertName, model.OperatorId);
                                        advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
                                        return Json("success");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    advertId = advertId == "" ? "0" : advertId;
                    IEnumerable<Advert> AdvertNameexists;
                    if (advertId == "0")
                    {
                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == Convert.ToInt32(advertId)).ToList();
                    }
                    else
                    {
                        AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != Convert.ToInt32(advertId)).ToList();
                    }
                    if (AdvertNameexists.Count() > 0)
                    {
                        FillClient(efmvcUser.UserId);
                        FillAdvertCategory(efmvcUser.UserId);
                        FillOperator(Convert.ToInt32(countryId));

                        return Json("Exists");
                    }
                    else
                    {
                        try
                        {
                            #region Media
                            if (mediaFile != null)
                            {
                                if (mediaFile.ContentLength != 0)
                                {
                                    var userData = _userRepository.GetById(efmvcUser.UserId);

                                    var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                    string fileName = firstAudioName;

                                    string fileName2 = null;
                                    if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                                    {
                                        var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                        fileName2 = secondAudioName.ToString();
                                    }

                                    string extension = Path.GetExtension(mediaFile.FileName);

                                    var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                    string outputFormat = "wav";
                                    var audioFormatExtension = "." + outputFormat;

                                    if (extension != audioFormatExtension)
                                    {
                                        string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                        string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                        mediaFile.SaveAs(tempPath);

                                        SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + "." + outputFormat);
                                    }
                                    else
                                    {
                                        string directoryName = Server.MapPath("~/Media/");
                                        directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                        if (!Directory.Exists(directoryName))
                                            Directory.CreateDirectory(directoryName);

                                        string path = Path.Combine(directoryName, fileName + extension);
                                        mediaFile.SaveAs(path);

                                        StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
                                        string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                                        if (!Directory.Exists(archiveDirectoryName))
                                            Directory.CreateDirectory(archiveDirectoryName);

                                        string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                        mediaFile.SaveAs(archivePath);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                                fileName + extension);
                                    }
                                }
                            }
                            #endregion

                            #region Script

                            if (scriptFile != null)
                            {
                                if (scriptFile.ContentLength != 0)
                                {
                                    string fileName = Guid.NewGuid().ToString();
                                    string extension = Path.GetExtension(scriptFile.FileName);

                                    string directoryName = Server.MapPath("/Script/");
                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                    if (!Directory.Exists(directoryName))
                                        Directory.CreateDirectory(directoryName);

                                    string path = Path.Combine(directoryName, fileName + extension);
                                    scriptFile.SaveAs(path);

                                    string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                    if (!Directory.Exists(archiveDirectoryName))
                                        Directory.CreateDirectory(archiveDirectoryName);

                                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                    scriptFile.SaveAs(archivePath);

                                    model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + extension);
                                }
                                else
                                {
                                    model.ScriptFileLocation = "";
                                }
                            }
                            else
                            {
                                model.ScriptFileLocation = "";
                            }
                            #endregion

                            #region Add Records

                            var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                            int campaignId = 0;

                            if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
                            {
                                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
                            }

                            int? clientId = null;
                            if (advertClientId == "")
                            {
                                clientId = null;
                            }
                            else
                            {
                                clientId = Convert.ToInt32(advertClientId);
                            }

                            if (ModelState.IsValid)
                            {
                                model.AdvertId = 0;
                                model.UserId = efmvcUser.UserId;
                                model.AdvertClientId = clientId;
                                model.AdvertName = advertName;
                                model.BrandName = advertBrandName;
                                model.UploadedToMediaServer = false;
                                model.CreatedDateTime = DateTime.Now;
                                model.UpdatedDateTime = DateTime.Now;
                                model.Status = (int)AdvertStatus.Waitingforapproval;
                                model.Script = script;
                                model.IsAdminApproval = false;
                                model.AdvertCategoryId = int.Parse(advertCategoryId);
                                model.CountryId = int.Parse(countryId);
                                model.PhoneticAlphabet = advertPhoneticAlphabet;
                                model.NextStatus = false;
                                model.CampProfileId = campaignId;
                                model.AdtoneServerAdvertId = null;
                                model.OperatorId = Convert.ToInt32(operatorId);

                                CreateOrUpdateCopyAdvertCommand command = Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

                                ICommandResult result = _commandBus.Submit(command);

                                if (result.Success)
                                {
                                    if (campaignId != 0)
                                    {
                                        CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                                        _campaignAdvert.AdvertId = result.Id;
                                        _campaignAdvert.CampaignProfileId = campaignId;
                                        _campaignAdvert.NextStatus = true;
                                        CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                                        Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

                                        ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

                                        if (campaignAdvertcommandResult.Success)
                                        {
                                            if (campaign != null)
                                            {
                                                campaign.NumberInBatch = int.Parse(numberofadsinabatch);
                                                campaign.UpdatedDateTime = DateTime.Now;
                                                db.SaveChanges();
                                                if (ConnString != null && ConnString.Count() > 0)
                                                {
                                                    UserMatchTableProcess obj = new UserMatchTableProcess();

                                                    string adName = "";
                                                    if (command.MediaFileLocation == null || command.MediaFileLocation == "")
                                                    {
                                                        adName = "";
                                                    }
                                                    else
                                                    {
                                                        
                                                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                                                        var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
                                                        var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                                        adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
                                                    }

                                                    foreach (var item in ConnString)
                                                    {
                                                        EFMVCDataContex db1 = new EFMVCDataContex(item);
                                                        var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                                        if (campaignProfileDetails != null)
                                                        {
                                                            campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
                                                            campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                                            db1.SaveChanges();

                                                            obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
                                                            PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                                        }
                                                    }
                                                }
                                                //Email Code
                                                //advertEmail.SendMail(advertName, model.OperatorId);
                                                advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
                                                return Json("success");
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            TempData["Error"] = ex.InnerException.Message;
                            return Json("fail");
                        }
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddCampaignDateData(string campaignName, string advertPhoneticAlphabet, string advertId, string advertName, string advertClientId, string advertBrandName, string advertCategoryId, string script, string numberofadsinabatch, HttpPostedFileBase mediaFile, HttpPostedFileBase scriptFile, string countryId, string operatorId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                NewAdvertFormModel model = new NewAdvertFormModel();
                EFMVCDataContex db = new EFMVCDataContex();
                AdvertEmail advertEmail = new AdvertEmail(_commandBus, _userRepository);

                try
                {
                    string countryName = _countryRepository.GetById(Convert.ToInt32(countryId)).Name;
                    string operatorName = _operatorRepository.GetById(Convert.ToInt32(operatorId)).OperatorName;

                    int? CountryId = null;
                    if (countryId != "")
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                    if (advertId != "")
                    {
                        advertId = advertId == "" ? "0" : advertId;
                        int advertID = Convert.ToInt32(advertId);
                        IEnumerable<Advert> AdvertNameexists;
                        if (advertId == "0")
                        {
                            AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == advertID).ToList();
                        }
                        else
                        {
                            AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != advertID).ToList();
                        }
                        if (AdvertNameexists.Count() > 0)
                        {
                            FillAdvertList(efmvcUser.UserId);
                            FillClient(efmvcUser.UserId);
                            FillAdvertCategory(efmvcUser.UserId);
                            FillOperator(Convert.ToInt32(countryId));

                            return Json("Exists");
                        }
                        else
                        {
                            var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                            int campaignId = 0;

                            if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
                            {
                                campaignId = Convert.ToInt32(campaign.CampaignProfileId);
                            }

                            int? clientId = null;
                            if (advertClientId == "")
                            {
                                clientId = null;
                            }
                            else
                            {
                                clientId = Convert.ToInt32(advertClientId);
                            }

                            var advertDetail = _advertRepository.GetById(Convert.ToInt32(advertId));
                            model.AdvertId = 0;
                            model.UserId = efmvcUser.UserId;
                            model.AdvertClientId = clientId;
                            model.AdvertName = advertName;
                            model.BrandName = advertBrandName;
                            model.UploadedToMediaServer = false;
                            model.CreatedDateTime = DateTime.Now;
                            model.UpdatedDateTime = DateTime.Now;
                            model.Status = (int)AdvertStatus.Waitingforapproval;
                            model.Script = script;
                            model.IsAdminApproval = false;
                            model.AdvertCategoryId = int.Parse(advertCategoryId);
                            model.CountryId = int.Parse(countryId);
                            model.PhoneticAlphabet = advertPhoneticAlphabet;
                            model.NextStatus = false;
                            model.CampProfileId = campaignId;
                            model.AdtoneServerAdvertId = null;
                            model.OperatorId = Convert.ToInt32(operatorId);

                            #region Media
                            if (mediaFile != null)
                            {
                                if (mediaFile.ContentLength != 0)
                                {
                                    var userData = _userRepository.GetById(efmvcUser.UserId);
                                    var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                    string fileName = firstAudioName;

                                    string fileName2 = null;
                                    if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                                    {
                                        var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                        fileName2 = secondAudioName.ToString();
                                    }

                                    string extension = Path.GetExtension(mediaFile.FileName);

                                    var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                    string outputFormat = "wav";
                                    var audioFormatExtension = "." + outputFormat;

                                    if (extension != audioFormatExtension)
                                    {
                                        string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                        string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                        mediaFile.SaveAs(tempPath);

                                        SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + "." + outputFormat);
                                    }
                                    else
                                    {
                                        string directoryName = Server.MapPath("~/Media/");
                                        directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                        if (!Directory.Exists(directoryName))
                                            Directory.CreateDirectory(directoryName);

                                        string path = Path.Combine(directoryName, fileName + extension);
                                        mediaFile.SaveAs(path);

                                        StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
                                        string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                                        if (!Directory.Exists(archiveDirectoryName))
                                            Directory.CreateDirectory(archiveDirectoryName);

                                        string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                        mediaFile.SaveAs(archivePath);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                                fileName + extension);
                                    }
                                }
                            }
                            else
                            {
                                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                string fileName = firstAudioName;

                                string fileName2 = null;
                                if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                                {
                                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                    fileName2 = secondAudioName.ToString();
                                }

                                string outputFormat = "wav";
                                var audioFormatExtension = "." + outputFormat;
                                var extension = advertDetail.MediaFileLocation == null ? "" : advertDetail.MediaFileLocation.Split('.').LastOrDefault();
                                if (extension != "")
                                {
                                    extension = "." + extension;
                                    if (extension != audioFormatExtension)
                                    {
                                        var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
                                        string fileName1 = firstAudioName;
                                        string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
                                        string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

                                        string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
                                        string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

                                        if (!Directory.Exists(targetPath))
                                            Directory.CreateDirectory(targetPath);
                                        if (System.IO.File.Exists(sourceFile) == true)
                                        {
                                            System.IO.File.Copy(sourceFile, destFile, true);

                                            model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName1 + "." + outputFormat);
                                        }

                                        string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                        string tempPath = Path.Combine(tempDirectoryName, fileName1 + extension);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName1 + "." + outputFormat);
                                    }
                                    else
                                    {
                                        var advertmediafilename = advertDetail.MediaFileLocation.Split('/').LastOrDefault();
                                        string fileName1 = firstAudioName;
                                        string sourcePath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());
                                        string targetPath = Server.MapPath("~/Media/" + efmvcUser.UserId.ToString());

                                        string sourceFile = System.IO.Path.Combine(sourcePath, advertmediafilename);
                                        string destFile = System.IO.Path.Combine(targetPath, fileName1 + extension);

                                        if (!Directory.Exists(targetPath))
                                            Directory.CreateDirectory(targetPath);
                                        if (System.IO.File.Exists(sourceFile) == true)
                                        {
                                            System.IO.File.Copy(sourceFile, destFile, true);
                                        }

                                        string directoryName1 = Server.MapPath("~/Media/");
                                        directoryName1 = Path.Combine(directoryName1, efmvcUser.UserId.ToString());

                                        if (!Directory.Exists(directoryName1))
                                            Directory.CreateDirectory(directoryName1);

                                        string path1 = Path.Combine(directoryName1, fileName + extension);

                                        string fileName3 = firstAudioName;
                                        string sourcePath1 = Server.MapPath("~/Media/Archive/");
                                        string targetPath1 = Server.MapPath("~/Media/Archive/");

                                        string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertmediafilename);
                                        string destFile1 = System.IO.Path.Combine(targetPath1, fileName3 + extension);

                                        if (!Directory.Exists(targetPath1))
                                            Directory.CreateDirectory(targetPath1);

                                        if (System.IO.File.Exists(sourceFile1) == true)
                                        {
                                            System.IO.File.Copy(sourceFile1, destFile1, true);
                                        }

                                        StoreSecondAudioFile(directoryName1, fileName2, outputFormat, path1);
                                        string archiveDirectoryName1 = Server.MapPath("~/Media/Archive/");

                                        if (!Directory.Exists(archiveDirectoryName1))
                                            Directory.CreateDirectory(archiveDirectoryName1);

                                        string archivePath1 = Path.Combine(archiveDirectoryName1, fileName + extension);

                                        model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + "." + outputFormat);
                                    }
                                }
                                else
                                {
                                    model.MediaFileLocation = advertDetail.MediaFileLocation;
                                }
                            }

                            #endregion

                            #region Script
                            if (scriptFile != null)
                            {
                                if (scriptFile.ContentLength != 0)
                                {
                                    string fileName = Guid.NewGuid().ToString();
                                    string extension = Path.GetExtension(scriptFile.FileName);

                                    string directoryName = Server.MapPath("/Script/");
                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                    if (!Directory.Exists(directoryName))
                                        Directory.CreateDirectory(directoryName);

                                    string path = Path.Combine(directoryName, fileName + extension);
                                    scriptFile.SaveAs(path);

                                    string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                    if (!Directory.Exists(archiveDirectoryName))
                                        Directory.CreateDirectory(archiveDirectoryName);

                                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                    scriptFile.SaveAs(archivePath);

                                    model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            fileName + extension);
                                }
                                else
                                {
                                    model.ScriptFileLocation = "";
                                }
                            }
                            else
                            {
                                string scriptfileName = Guid.NewGuid().ToString();
                                string extension1 = advertDetail.ScriptFileLocation == null ? "" : advertDetail.ScriptFileLocation.Split('.').LastOrDefault();
                                if (extension1 != "")
                                {
                                    var advertscriptfilename = advertDetail.ScriptFileLocation.Split('/').LastOrDefault();
                                    string fileName1 = scriptfileName;
                                    string sourcePath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());
                                    string targetPath = Server.MapPath("/Script/" + efmvcUser.UserId.ToString());

                                    string sourceFile = System.IO.Path.Combine(sourcePath, advertscriptfilename);
                                    string destFile = System.IO.Path.Combine(targetPath, advertscriptfilename);

                                    if (!Directory.Exists(targetPath))
                                        Directory.CreateDirectory(targetPath);

                                    if (System.IO.File.Exists(sourceFile) == true)
                                    {
                                        System.IO.File.Copy(sourceFile, destFile, true);

                                        System.IO.File.Delete(fileName1 + "." + extension1); // Delete the existing file if exists
                                        System.IO.File.Move(advertscriptfilename, fileName1 + "." + extension1);
                                    }

                                    string directoryName = Server.MapPath("/Script/");
                                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                    if (!Directory.Exists(directoryName))
                                        Directory.CreateDirectory(directoryName);

                                    string path = Path.Combine(directoryName, scriptfileName + extension1);

                                    string fileName3 = scriptfileName;
                                    string sourcePath1 = Server.MapPath("/Script/Archive/");
                                    string targetPath1 = Server.MapPath("/Script/Archive/");

                                    string sourceFile1 = System.IO.Path.Combine(sourcePath1, advertscriptfilename);
                                    string destFile1 = System.IO.Path.Combine(targetPath1, advertscriptfilename);

                                    if (!Directory.Exists(targetPath1))
                                        Directory.CreateDirectory(targetPath1);

                                    if (System.IO.File.Exists(sourceFile1) == true)
                                    {
                                        System.IO.File.Copy(sourceFile1, destFile1, true);

                                        System.IO.File.Delete(fileName3 + "." + extension1); // Delete the existing file if exists
                                        System.IO.File.Move(advertscriptfilename, fileName3 + "." + extension1);
                                    }

                                    string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                    if (!Directory.Exists(archiveDirectoryName))
                                        Directory.CreateDirectory(archiveDirectoryName);

                                    string archivePath = Path.Combine(archiveDirectoryName, scriptfileName + extension1);

                                    model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                            scriptfileName + extension1);
                                }
                                else
                                {
                                    model.ScriptFileLocation = advertDetail.ScriptFileLocation;
                                }
                            }

                            #endregion

                            CreateOrUpdateCopyAdvertCommand command =
                                Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

                            ICommandResult result = _commandBus.Submit(command);

                            if (result.Success)
                            {
                                if (campaignId != 0)
                                {
                                    CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                                    _campaignAdvert.AdvertId = result.Id;
                                    _campaignAdvert.CampaignProfileId = campaignId;
                                    _campaignAdvert.NextStatus = true;
                                    CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                                    Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

                                    ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

                                    if (campaignAdvertcommandResult.Success)
                                    {
                                        if (campaign != null)
                                        {
                                            campaign.NumberInBatch = int.Parse(numberofadsinabatch);
                                            campaign.UpdatedDateTime = DateTime.Now;
                                            db.SaveChanges();
                                            if (ConnString != null && ConnString.Count() > 0)
                                            {
                                                UserMatchTableProcess obj = new UserMatchTableProcess();
                                                string adName = "";
                                                if (command.MediaFileLocation == null || command.MediaFileLocation == "")
                                                {
                                                    adName = "";
                                                }
                                                else
                                                {
                                                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                                                    var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
                                                    var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                                    adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
                                                }
                                                foreach (var item in ConnString)
                                                {
                                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                                    if (campaignProfileDetails != null)
                                                    {
                                                        campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
                                                        campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                                        db1.SaveChanges();

                                                        obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
                                                        PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                                    }
                                                }
                                            }
                                            //Email Code
                                            //advertEmail.SendMail(advertName, model.OperatorId);
                                            advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
                                            return Json("success");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        advertId = advertId == "" ? "0" : advertId;
                        IEnumerable<Advert> AdvertNameexists;
                        if (advertId == "0")
                        {
                            AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId == Convert.ToInt32(advertId)).ToList();
                        }
                        else
                        {
                            AdvertNameexists = _advertRepository.GetAll().Where(c => c.AdvertName == advertName && c.UserId == efmvcUser.UserId && c.AdvertId != Convert.ToInt32(advertId)).ToList();
                        }
                        if (AdvertNameexists.Count() > 0)
                        {
                            FillClient(efmvcUser.UserId);
                            FillAdvertCategory(efmvcUser.UserId);
                            FillOperator(Convert.ToInt32(countryId));

                            return Json("Exists");
                        }
                        else
                        {
                            try
                            {
                                #region Media
                                if (mediaFile != null)
                                {
                                    if (mediaFile.ContentLength != 0)
                                    {
                                        var userData = _userRepository.GetById(efmvcUser.UserId);

                                        var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                        string fileName = firstAudioName;

                                        string fileName2 = null;
                                        if (Convert.ToInt32(operatorId) == (int)OperatorTableId.Safaricom)
                                        {
                                            var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                            fileName2 = secondAudioName.ToString();
                                        }

                                        string extension = Path.GetExtension(mediaFile.FileName);

                                        var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                        string outputFormat = "wav";
                                        var audioFormatExtension = "." + outputFormat;

                                        if (extension != audioFormatExtension)
                                        {
                                            string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                            string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(tempPath);

                                            SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                            model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                                fileName + "." + outputFormat);
                                        }
                                        else
                                        {
                                            string directoryName = Server.MapPath("~/Media/");
                                            directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                            if (!Directory.Exists(directoryName))
                                                Directory.CreateDirectory(directoryName);

                                            string path = Path.Combine(directoryName, fileName + extension);
                                            mediaFile.SaveAs(path);

                                            StoreSecondAudioFile(directoryName, fileName2, outputFormat, path);
                                            string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                                            if (!Directory.Exists(archiveDirectoryName))
                                                Directory.CreateDirectory(archiveDirectoryName);

                                            string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(archivePath);

                                            model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                                    fileName + extension);
                                        }
                                    }
                                }
                                #endregion

                                #region Script

                                if (scriptFile != null)
                                {
                                    if (scriptFile.ContentLength != 0)
                                    {
                                        string fileName = Guid.NewGuid().ToString();
                                        string extension = Path.GetExtension(scriptFile.FileName);

                                        string directoryName = Server.MapPath("/Script/");
                                        directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                        if (!Directory.Exists(directoryName))
                                            Directory.CreateDirectory(directoryName);

                                        string path = Path.Combine(directoryName, fileName + extension);
                                        scriptFile.SaveAs(path);

                                        string archiveDirectoryName = Server.MapPath("/Script/Archive/");

                                        if (!Directory.Exists(archiveDirectoryName))
                                            Directory.CreateDirectory(archiveDirectoryName);

                                        string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                        scriptFile.SaveAs(archivePath);

                                        model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                                fileName + extension);
                                    }
                                    else
                                    {
                                        model.ScriptFileLocation = "";
                                    }
                                }
                                else
                                {
                                    model.ScriptFileLocation = "";
                                }
                                #endregion

                                #region Add Records

                                var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();
                                int campaignId = 0;

                                if (campaign.CampaignProfileId != 0 && campaign.CampaignProfileId != null)
                                {
                                    campaignId = Convert.ToInt32(campaign.CampaignProfileId);
                                }

                                int? clientId = null;
                                if (advertClientId == "")
                                {
                                    clientId = null;
                                }
                                else
                                {
                                    clientId = Convert.ToInt32(advertClientId);
                                }

                                if (ModelState.IsValid)
                                {
                                    model.AdvertId = 0;
                                    model.UserId = efmvcUser.UserId;
                                    model.AdvertClientId = clientId;
                                    model.AdvertName = advertName;
                                    model.BrandName = advertBrandName;
                                    model.UploadedToMediaServer = false;
                                    model.CreatedDateTime = DateTime.Now;
                                    model.UpdatedDateTime = DateTime.Now;
                                    model.Status = (int)AdvertStatus.Waitingforapproval;
                                    model.Script = script;
                                    model.IsAdminApproval = false;
                                    model.AdvertCategoryId = int.Parse(advertCategoryId);
                                    model.CountryId = int.Parse(countryId);
                                    model.PhoneticAlphabet = advertPhoneticAlphabet;
                                    model.NextStatus = false;
                                    model.CampProfileId = campaignId;
                                    model.AdtoneServerAdvertId = null;
                                    model.OperatorId = Convert.ToInt32(operatorId);

                                    CreateOrUpdateCopyAdvertCommand command = Mapper.Map<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>(model);

                                    ICommandResult result = _commandBus.Submit(command);

                                    if (result.Success)
                                    {
                                        if (campaignId != 0)
                                        {
                                            CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                                            _campaignAdvert.AdvertId = result.Id;
                                            _campaignAdvert.CampaignProfileId = campaignId;
                                            _campaignAdvert.NextStatus = true;
                                            CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                                            Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

                                            ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

                                            if (campaignAdvertcommandResult.Success)
                                            {
                                                if (campaign != null)
                                                {
                                                    campaign.NumberInBatch = int.Parse(numberofadsinabatch);
                                                    campaign.UpdatedDateTime = DateTime.Now;
                                                    db.SaveChanges();
                                                    if (ConnString != null && ConnString.Count() > 0)
                                                    {
                                                        UserMatchTableProcess obj = new UserMatchTableProcess();

                                                        string adName = "";
                                                        if (command.MediaFileLocation == null || command.MediaFileLocation == "")
                                                        {
                                                            adName = "";
                                                        }
                                                        else
                                                        {

                                                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                                                            var advertOperatorId = _advertRepository.GetById(result.Id).OperatorId;
                                                            var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                                            adName = operatorFTPDetails.FtpRoot + "/" + command.MediaFileLocation.Split('/')[3];
                                                        }

                                                        foreach (var item in ConnString)
                                                        {
                                                            EFMVCDataContex db1 = new EFMVCDataContex(item);
                                                            var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                                            if (campaignProfileDetails != null)
                                                            {
                                                                campaignProfileDetails.NumberInBatch = int.Parse(numberofadsinabatch);
                                                                campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                                                db1.SaveChanges();

                                                                obj.UpdateCampaignAd(campaignProfileDetails.CampaignProfileId, adName, db1);
                                                                PreMatchProcess.PrematchProcessForCampaign(campaignProfileDetails.CampaignProfileId, item);
                                                            }
                                                        }
                                                    }
                                                    //Email Code
                                                    //advertEmail.SendMail(advertName, model.OperatorId);
                                                    advertEmail.SendMail(advertName, model.OperatorId, efmvcUser.UserId, campaignName, countryName, operatorName, DateTime.Now);
                                                    return Json("success");
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                TempData["Error"] = ex.InnerException.Message;
                              
                                return Json("fail" + ex.InnerException.Message.ToString());
                            }
                        }
                    }
                    return Json("fail");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddCampaignDateInfo(string campaignName, string startDate, string endDate, string smsOriginator, string smsBody, string emailSubject, string emailBody, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (campaignName != "" || campaignName != null)
                {
                    try
                    {
                        int? CountryId = null;
                        if (countryId != "")
                        {
                            CountryId = Convert.ToInt32(countryId);
                        }
                        var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                        NewCampaignDateandInteraction model = new NewCampaignDateandInteraction();

                        EFMVCDataContex db = new EFMVCDataContex();

                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();

                        if (campaign != null)
                        {
                            campaign.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                            campaign.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                            campaign.SmsOriginator = !string.IsNullOrEmpty(smsOriginator) ? smsOriginator : null;
                            campaign.SmsBody = !string.IsNullOrEmpty(smsBody) ? smsBody : null;
                            campaign.EmailSubject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : null;
                            campaign.EmailBody = !string.IsNullOrEmpty(emailBody) ? emailBody : null;
                            campaign.UpdatedDateTime = System.DateTime.Now;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                    if (campaignProfileDetails != null)
                                    {
                                        campaignProfileDetails.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                                        campaignProfileDetails.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                                        campaignProfileDetails.SmsOriginator = !string.IsNullOrEmpty(smsOriginator) ? smsOriginator : null;
                                        campaignProfileDetails.SmsBody = !string.IsNullOrEmpty(smsBody) ? smsBody : null;
                                        campaignProfileDetails.EmailSubject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : null;
                                        campaignProfileDetails.EmailBody = !string.IsNullOrEmpty(emailBody) ? emailBody : null;
                                        campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
                                        db1.SaveChanges();
                                    }
                                }
                            }

                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                UserMatchTableProcess obj = new UserMatchTableProcess();
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                    var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                    if (campaigndetails != null)
                                    {
                                        CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
                                        campaignProfileFormModel.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                                        campaignProfileFormModel.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                                        campaignProfileFormModel.EmailBody = campaignProfileFormModel.EmailBody;
                                        campaignProfileFormModel.EmailSenderAddress = campaignProfileFormModel.EmailSenderAddress;
                                        campaignProfileFormModel.EmailSubject = campaignProfileFormModel.EmailSubject;
                                        campaignProfileFormModel.SmsBody = campaignProfileFormModel.SmsBody;
                                        campaignProfileFormModel.SmsOriginator = campaignProfileFormModel.SmsOriginator;
                                        obj.UpdateCampaignSmsAndEmailWizard(campaignProfileFormModel, campaigndetails, null, null, SQLServerEntities);
                                        PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                    }
                                }
                            }

                            return Json("success");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.InnerException.Message;
                        return Json("fail");
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult AddCampaignAdProfileData(string campaignName, string startDate, string endDate, string smsOriginator, string smsBody, string emailSubject, string emailBody, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                if (campaignName != "" || campaignName != null)
                {
                    try
                    {
                        int? CountryId = null;
                        if (countryId != "")
                        {
                            CountryId = Convert.ToInt32(countryId);
                        }
                        var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                        NewCampaignDateandInteraction model = new NewCampaignDateandInteraction();

                        EFMVCDataContex db = new EFMVCDataContex();

                        var campaign = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.UserId == efmvcUser.UserId).FirstOrDefault();

                        if (campaign != null)
                        {
                            campaign.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                            campaign.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                            campaign.SmsOriginator = !string.IsNullOrEmpty(smsOriginator) ? smsOriginator : null;
                            campaign.SmsBody = !string.IsNullOrEmpty(smsBody) ? smsBody : null;
                            campaign.EmailSubject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : null;
                            campaign.EmailBody = !string.IsNullOrEmpty(emailBody) ? emailBody : null;
                            campaign.UpdatedDateTime = System.DateTime.Now;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                    if (campaignProfileDetails != null)
                                    {
                                        campaignProfileDetails.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                                        campaignProfileDetails.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                                        campaignProfileDetails.SmsOriginator = !string.IsNullOrEmpty(smsOriginator) ? smsOriginator : null;
                                        campaignProfileDetails.SmsBody = !string.IsNullOrEmpty(smsBody) ? smsBody : null;
                                        campaignProfileDetails.EmailSubject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : null;
                                        campaignProfileDetails.EmailBody = !string.IsNullOrEmpty(emailBody) ? emailBody : null;
                                        campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
                                        db1.SaveChanges();
                                    }
                                }
                            }

                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                UserMatchTableProcess obj = new UserMatchTableProcess();
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                    var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaign.CampaignProfileId).FirstOrDefault();
                                    if (campaigndetails != null)
                                    {
                                        CampaignProfileFormModel campaignProfileFormModel = new CampaignProfileFormModel();
                                        campaignProfileFormModel.StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : (DateTime?)null;
                                        campaignProfileFormModel.EndDate = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null;
                                        campaignProfileFormModel.EmailBody = !string.IsNullOrEmpty(emailBody) ? emailBody : null;
                                        campaignProfileFormModel.EmailSenderAddress = null;
                                        campaignProfileFormModel.EmailSubject = !string.IsNullOrEmpty(emailSubject) ? emailSubject : null;
                                        campaignProfileFormModel.SmsBody = !string.IsNullOrEmpty(smsBody) ? smsBody : null;
                                        campaignProfileFormModel.SmsOriginator = !string.IsNullOrEmpty(smsOriginator) ? smsOriginator : null;
                                        obj.UpdateCampaignSmsAndEmailWizard(campaignProfileFormModel, campaigndetails, null, null, SQLServerEntities);
                                        PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                    }
                                }
                            }

                            NewCampaignProfileFormModel model1 = new NewCampaignProfileFormModel();

                            int CountryId1 = Convert.ToInt32(campaign.CountryId);

                            CampaignProfileGeographicFormModel CampaignProfileGeographicModel = new CampaignProfileGeographicFormModel(CountryId1);
                            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(CountryId1);
                            CampaignProfileAdvertFormModel CampaignProfileAd = new CampaignProfileAdvertFormModel(CountryId1);
                            CampaignProfileMobileFormModel Mobilepro = new CampaignProfileMobileFormModel(CountryId1);
                            CampaignProfileSkizaFormModel CampaignSkizaProfile = new CampaignProfileSkizaFormModel(CountryId1);
                            CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel = new CampaignProfileTimeSettingFormModel();

                            ViewBag.Country = _countryRepository.GetById(Convert.ToInt32(campaign.CountryId)).Name;

                            CampaignProfileGeographicModel = CampaignProfileGeographic(campaign.CampaignProfileId, CampaignProfileGeographicModel);
                            model1.newAdProfileMappingFormModel.CampaignProfileGeographicModel = CampaignProfileGeographicModel;

                            CampaignProfileDemographicsmodel = CampaignProfileDemographic(campaign.CampaignProfileId, CampaignProfileDemographicsmodel);
                            model1.newAdProfileMappingFormModel.CampaignProfileDemographicsmodel = CampaignProfileDemographicsmodel;

                            CampaignProfileAd = CampaignProfileAdvert(campaign.CampaignProfileId, CampaignProfileAd);
                            model1.newAdProfileMappingFormModel.CampaignProfileAd = CampaignProfileAd;

                            CampaignSkizaProfile = CampaignProfileSkizaInormation(campaign.CampaignProfileId, CampaignSkizaProfile);
                            model1.newAdProfileMappingFormModel.CampaignProfileSkizaFormModel = CampaignSkizaProfile;

                            Mobilepro = Mobile(campaign.CampaignProfileId, Mobilepro);
                            model1.newAdProfileMappingFormModel.CampaignProfileMobileFormModel = Mobilepro;

                            CampaignProfileTimeSettingModel = CampaignProfileTimeSettingMapping(campaign.CampaignProfileId);
                            model1.newAdProfileMappingFormModel.CampaignProfileTimeSettingModel = CampaignProfileTimeSettingModel;

                            ProfileGeographicOptions(model1.newAdProfileMappingFormModel.CampaignProfileGeographicModel, (int)campaign.CountryId);

                            ProfileDemographicsOptions(model1.newAdProfileMappingFormModel.CampaignProfileDemographicsmodel, (int)campaign.CountryId);
                            ProfileAdvertOptions(model1.newAdProfileMappingFormModel.CampaignProfileAd, (int)campaign.CountryId);
                            ProfileMobileFormOptions(model1.newAdProfileMappingFormModel.CampaignProfileMobileFormModel, (int)campaign.CountryId);

                            return PartialView("_AddNewAdProfileMappingInfo", model1.newAdProfileMappingFormModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.InnerException.Message;
                        return Json("fail");
                    }
                }
                return Json("fail");
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult UpdateCampaignWizard(string campaignId, string campaignName, string campaignPhoneticAlphabet, string clientId, string clientName, string clientPhoneticAlphabet, string advertName, string advertPhoneticAlphabet, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    int? CountryId = null;
                    if (countryId != "")
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                    if (campaignName != "" || campaignName != null)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();

                        var campaignProfile = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.PhoneticAlphabet == campaignPhoneticAlphabet && c.UserId == efmvcUser.UserId && c.NextStatus == true).FirstOrDefault();
                        if (campaignProfile != null)
                        {
                            campaignProfile.Status = (int)CampaignStatus.CampaignPausedDueToInsufficientFunds;
                            campaignProfile.UpdatedDateTime = DateTime.Now;
                            campaignProfile.IsAdminApproval = true;
                            campaignProfile.NextStatus = false;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(item);
                                    var campaignProfileDetails = db1.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                    if (campaignProfileDetails != null)
                                    {
                                        campaignProfileDetails.Status = (int)CampaignStatus.CampaignPausedDueToInsufficientFunds;
                                        campaignProfileDetails.UpdatedDateTime = DateTime.Now;
                                        campaignProfileDetails.IsAdminApproval = true;
                                        campaignProfileDetails.NextStatus = false;
                                        db1.SaveChanges();
                                    }
                                }
                            }

                            var client = db.Clients.Where(c => c.Name == clientName && c.PhoneticAlphabet == clientPhoneticAlphabet && c.UserId == efmvcUser.UserId && c.NextStatus == true).FirstOrDefault();
                            if (client != null)
                            {
                                client.UpdatedDate = DateTime.Now;
                                client.Status = (int)ClientStatus.Live;
                                client.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db2 = new EFMVCDataContex(item);
                                        var clientDetails = db2.Clients.Where(s => s.AdtoneServerClientId == client.Id).FirstOrDefault();
                                        if (clientDetails != null)
                                        {
                                            clientDetails.UpdatedDate = DateTime.Now;
                                            clientDetails.Status = (int)ClientStatus.Live;
                                            clientDetails.NextStatus = false;
                                            db2.SaveChanges();
                                        }
                                    }
                                }
                            }
                            var advert = db.Adverts.Where(c => c.AdvertName == advertName && c.PhoneticAlphabet == advertPhoneticAlphabet && c.UserId == efmvcUser.UserId || c.ClientId == campaignProfile.ClientId && c.NextStatus == true).FirstOrDefault();
                            if (advert != null)
                            {
                                advert.UpdatedDateTime = DateTime.Now;
                                advert.Status = (int)AdvertStatus.Waitingforapproval;
                                advert.IsAdminApproval = true;
                                advert.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db3 = new EFMVCDataContex(item);
                                        var advertDetails = db3.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                                        if (advertDetails != null)
                                        {
                                            advertDetails.UpdatedDateTime = DateTime.Now;
                                            advertDetails.Status = (int)AdvertStatus.Waitingforapproval;
                                            advertDetails.IsAdminApproval = true;
                                            advertDetails.NextStatus = false;
                                            db3.SaveChanges();
                                        }
                                    }
                                }

                                var campaignAdvert = db.CampaignAdverts.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId && c.AdvertId == advert.AdvertId && c.NextStatus == true).FirstOrDefault();
                                if (campaignAdvert != null)
                                {
                                    campaignAdvert.NextStatus = false;
                                    db.SaveChanges();
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex db4 = new EFMVCDataContex(item);
                                            var campaignadvertDetails = db4.CampaignAdverts.Where(s => s.AdtoneServerCampaignAdvertId == campaignAdvert.CampaignAdvertId).FirstOrDefault();
                                            if (campaignadvertDetails != null)
                                            {
                                                campaignadvertDetails.NextStatus = false;
                                                db4.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                            var campaignProfilePreference = db.CampaignProfilePreference.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId && c.NextStatus == true).FirstOrDefault();
                            if (campaignProfilePreference != null)
                            {
                                campaignProfilePreference.CountryId = campaignProfile.CountryId.Value;
                                campaignProfilePreference.NextStatus = false;
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db5 = new EFMVCDataContex(item);
                                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db5, campaignProfile.CountryId.Value);
                                        var campaignProfilePreferenceDetails = db5.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfilePreference.Id).FirstOrDefault();
                                        if (campaignProfilePreferenceDetails != null)
                                        {
                                            campaignProfilePreferenceDetails.CountryId = externalServerCountryId;
                                            campaignProfilePreferenceDetails.NextStatus = false;
                                            db5.SaveChanges();
                                        }
                                    }
                                }
                            }
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db6 = new EFMVCDataContex(item);
                                    var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db6, (int)campaignProfile.CampaignProfileId);
                                    var campaignMatchDetails = db6.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                                    if (campaignMatchDetails != null)
                                    {
                                        campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                        campaignMatchDetails.Status = (int)CampaignStatus.CampaignPausedDueToInsufficientFunds;
                                        campaignMatchDetails.NextStatus = false;
                                        db6.SaveChanges();
                                    }
                                }
                            }
                            
                            var userDetails = _userRepository.GetById(efmvcUser.UserId);

                            var adminDetails = _contactsRepository.Get(s => s.UserId == 19);
                            if (adminDetails != null)
                            {
                                var reader =
                                    new StreamReader(
                                        Server.MapPath(ConfigurationManager.AppSettings["CampaignEmailTemplate"]));
                                var url = ConfigurationManager.AppSettings["AdminUrlForCampaign"];
                                string emailContent = reader.ReadToEnd();

                                emailContent = string.Format(emailContent, campaignName, userDetails.FirstName, userDetails.LastName, userDetails.Organisation == null ? "-" : userDetails.Organisation, userDetails.Email, DateTime.Now.ToString("HH:mm dd-MM-yyyy"), url);

                                MailMessage mail = new MailMessage();
                                mail.To.Add(adminDetails.Email);
                                mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                                mail.Subject = "Campaign Verification";

                                mail.Body = emailContent.Replace("\n", "<br/>");

                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                                smtp.Credentials = new System.Net.NetworkCredential
                                     (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                                //Or your Smtp Email ID and Password
                                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                                smtp.Send(mail);
                            }
                            return Json("success");
                        }
                        return Json("success");
                    }
                    return Json("fail");
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        [AuthorizeFilter]
        public ActionResult DeleteCampaignWizard(string campaignId, string campaignName, string campaignPhoneticAlphabet, string clientId, string clientName, string clientPhoneticAlphabet, string advertName, string advertPhoneticAlphabet, string countryId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                try
                {
                    int? CountryId = null;
                    if (countryId != "")
                    {
                        CountryId = Convert.ToInt32(countryId);
                    }
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);

                    if (campaignName != "" || campaignName != null)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();

                        var campaignProfile = db.CampaignProfiles.Where(c => c.CampaignName == campaignName && c.PhoneticAlphabet == campaignPhoneticAlphabet && c.UserId == efmvcUser.UserId).FirstOrDefault();
                        if (campaignProfile != null)
                        {
                            var client = db.Clients.Where(c => c.Name == clientName && c.PhoneticAlphabet == clientPhoneticAlphabet && c.UserId == efmvcUser.UserId).FirstOrDefault();
                            var advert = db.Adverts.Where(c => c.AdvertName == advertName && c.PhoneticAlphabet == advertPhoneticAlphabet && c.UserId == efmvcUser.UserId || c.ClientId == campaignProfile.ClientId).FirstOrDefault();

                            if (advert != null)
                            {
                                var campaignAdvert = db.CampaignAdverts.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId && c.AdvertId == advert.AdvertId).FirstOrDefault();
                                if (campaignAdvert != null)
                                {
                                    db.CampaignAdverts.Remove(campaignAdvert);
                                    db.SaveChanges();
                                    if (ConnString != null && ConnString.Count() > 0)
                                    {
                                        foreach (var item in ConnString)
                                        {
                                            EFMVCDataContex db1 = new EFMVCDataContex(item);
                                            var campaignAdvertDetails = db1.CampaignAdverts.Where(s => s.AdtoneServerCampaignAdvertId == campaignAdvert.CampaignAdvertId).FirstOrDefault();
                                            if (campaignAdvertDetails != null)
                                            {
                                                db1.CampaignAdverts.Remove(campaignAdvertDetails);
                                                db1.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                db.Adverts.Remove(advert);
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db2 = new EFMVCDataContex(item);
                                        var advertDetails = db2.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                                        if (advertDetails != null)
                                        {
                                            db2.Adverts.Remove(advertDetails);
                                            db2.SaveChanges();
                                        }
                                    }
                                }
                            }

                            var campaignProfilePreference = db.CampaignProfilePreference.Where(c => c.CampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                            var campaignMatch = db.CampaignMatch.Where(c => c.MSCampaignProfileId == campaignProfile.CampaignProfileId && c.UserId == efmvcUser.UserId || c.ClientId == campaignProfile.ClientId).FirstOrDefault();

                            if (campaignProfilePreference != null)
                            {
                                db.CampaignProfilePreference.Remove(campaignProfilePreference);
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db3 = new EFMVCDataContex(item);
                                        var campaignProfilePreferenceDetails = db3.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfilePreference.Id).FirstOrDefault();
                                        if (campaignProfilePreferenceDetails != null)
                                        {
                                            db3.CampaignProfilePreference.Remove(campaignProfilePreferenceDetails);
                                            db3.SaveChanges();
                                        }
                                    }
                                }
                            }

                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var item in ConnString)
                                {
                                    EFMVCDataContex db4 = new EFMVCDataContex(item);
                                    var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db4, (int)campaignProfile.CampaignProfileId);
                                    var campaignMatchDetails = db4.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                                    if (campaignMatchDetails != null)
                                    {
                                        db4.CampaignMatch.Remove(campaignMatchDetails);
                                        db4.SaveChanges();
                                    }
                                }
                            }

                            if (campaignProfile != null)
                            {
                                db.CampaignProfiles.Remove(campaignProfile);
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db5 = new EFMVCDataContex(item);
                                        var campaignProfileDetails = db5.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                        if (campaignProfileDetails != null)
                                        {
                                            db5.CampaignProfiles.Remove(campaignProfileDetails);
                                            db5.SaveChanges();
                                        }
                                    }
                                }
                            }

                            if (client != null)
                            {
                                db.Clients.Remove(client);
                                db.SaveChanges();
                                if (ConnString != null && ConnString.Count() > 0)
                                {
                                    foreach (var item in ConnString)
                                    {
                                        EFMVCDataContex db6 = new EFMVCDataContex(item);
                                        var clientDetails = db6.Clients.Where(s => s.AdtoneServerClientId == client.Id).FirstOrDefault();
                                        if (clientDetails != null)
                                        {
                                            db6.Clients.Remove(clientDetails);
                                            db6.SaveChanges();
                                        }
                                    }
                                }
                            }

                            return Json("success");
                        }
                        return Json("success");
                    }
                    return Json("fail");
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException.Message;
                    return Json("fail");
                }
            }
            return RedirectToAction("Index", "Landing");
        }

        private string ConvertPhoneticAlphabet(string uniqueId)
        {
            var phoneticValue = "";
            var splitData = uniqueId.Split('-');
            var i = 0;
            foreach (var item in splitData)
            {
                char[] charArr = item.ToCharArray();
                foreach (var ch in charArr)
                {
                    switch (ch)
                    {
                        case 'a': phoneticValue += " ALPHA"; break;
                        case 'b': phoneticValue += " BRAVO"; break;
                        case 'c': phoneticValue += " CHARLIE"; break;
                        case 'd': phoneticValue += " DELTA"; break;
                        case 'e': phoneticValue += " ECHO"; break;
                        case 'f': phoneticValue += " FOXTROT"; break;
                        case 'g': phoneticValue += " GOLF"; break;
                        case 'h': phoneticValue += " HOTEL"; break;
                        case 'i': phoneticValue += " INDIA"; break;
                        case 'j': phoneticValue += " JULIET"; break;
                        case 'k': phoneticValue += " KILO"; break;
                        case 'l': phoneticValue += " LIMA"; break;
                        case 'm': phoneticValue += " MIKE"; break;
                        case 'n': phoneticValue += " NOVEMBER"; break;
                        case 'o': phoneticValue += " OSCAR"; break;
                        case 'p': phoneticValue += " PAPA"; break;
                        case 'q': phoneticValue += " QUEBEC"; break;
                        case 'r': phoneticValue += " ROMEO"; break;
                        case 's': phoneticValue += " SIERRA"; break;
                        case 't': phoneticValue += " TANGO"; break;
                        case 'u': phoneticValue += " UNIFORM"; break;
                        case 'v': phoneticValue += " VICTOR"; break;
                        case 'w': phoneticValue += " WHISKEY"; break;
                        case 'x': phoneticValue += " XRAY"; break;
                        case 'y': phoneticValue += " YANKEE"; break;
                        case 'z': phoneticValue += " ZULU"; break;
                        default: break;
                    }
                }
                i++;

                if (i < 3)
                {
                    phoneticValue += " - ";
                }
            }
            return phoneticValue.TrimStart();
        }

        private void SaveConvertedFile(string audioFilePath, string extension, string userId, string fileName, string outputFormat, string fileName2)
        {
            var id = Convert.ToInt32(userId);

            string inputFormat = extension.Replace(".", "");

            CloudConvert api = new CloudConvert("WNdHFlLrT9GdETzjTJC4BoUsjE6tXbRi8sZX5aokQbua3D2hbJITOTylPs7Nre1A");
            var url = api.GetProcessURL(inputFormat, outputFormat);

            var convertedFile = api.UploadFile(url, audioFilePath, outputFormat, null);
            var convertedMediaData = new JavaScriptSerializer().Deserialize<CloudConvertModel.RootObject>(convertedFile);

            using (WebClient webClient = new WebClient())
            {
                var downloadUrl = "http:" + convertedMediaData.output.url;
                string directoryName = Server.MapPath("~/Media/");
                directoryName = Path.Combine(directoryName, userId);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                string savePath = Path.Combine(directoryName, fileName + "." + outputFormat);

                webClient.DownloadFile(downloadUrl, savePath);

                StoreSecondAudioFile(directoryName, fileName2, outputFormat, savePath);

                string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                if (!Directory.Exists(archiveDirectoryName))
                    Directory.CreateDirectory(archiveDirectoryName);

                string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                System.IO.File.Copy(savePath, archivePath, true);

                System.IO.File.Delete(audioFilePath);
            }
        }

        public void StoreSecondAudioFile(string directoryName, string fileName2, string outputFormat, string savePath)
        {
            if (fileName2 != null)
            {
                var directoryName2 = Path.Combine(directoryName, "SecondAudioFile");
                if (!Directory.Exists(directoryName2))
                    Directory.CreateDirectory(directoryName2);

                string secondAudioPath = Path.Combine(directoryName2, fileName2 + "." + outputFormat);
                System.IO.File.Copy(savePath, secondAudioPath, true);
            }
        }

        private CampaignProfileGeographicFormModel CampaignProfileGeographicMapping(int Id, CampaignProfileGeographicFormModel CampaignProfileGeographicmodel)
        {
            int countryId = 0;
            if (Id == 0)
            {
                countryId = _profileRepository.GetById(Id).CountryId.Value;
            }
            else
            {
                countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("kenya".ToLower())).Id;
            }
            CampaignProfilePreference campaignProfileGeographics = _campaignProfilePreferenceRepository.GetAll().FirstOrDefault();
            if (campaignProfileGeographics == null)
            {
                CampaignProfileGeographicmodel = new CampaignProfileGeographicFormModel(Id);
            }
            else
            {
                bool status = checkcampaignProfileDemographics(campaignProfileGeographics);
                if (status == false)
                {
                    CampaignProfileGeographicmodel = new CampaignProfileGeographicFormModel(Id) { CampaignProfileGeographicId = campaignProfileGeographics.Id };
                }
                else
                {
                    if (campaignProfileGeographics.CountryId == 0)
                    {
                        campaignProfileGeographics.CountryId = countryId;
                    }
                    CampaignProfileGeographicmodel =
                        Mapper.Map<CampaignProfilePreference, CampaignProfileGeographicFormModel>(
                            campaignProfileGeographics);
                }
            }

            return CampaignProfileGeographicmodel;
        }

        private CampaignProfileDemographicsFormModel CampaignProfileDemographicMapping(int Id, CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel)
        {
            int countryId = 0;
            if (Id == 0)
            {
                countryId = _profileRepository.GetById(Id).CountryId.Value;
            }
            else
            {
                countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("kenya".ToLower())).Id;
            }
            CampaignProfilePreference campaignProfileDemographics = _campaignProfilePreferenceRepository.GetAll().FirstOrDefault();
            if (campaignProfileDemographics == null)
            {
                CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(Id);
            }
            else
            {
                bool status = checkcampaignProfileDemographics(campaignProfileDemographics);
                if (status == false)
                {
                    CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(Id) { CampaignProfileDemographicsId = campaignProfileDemographics.Id };
                }
                else
                {
                    if (campaignProfileDemographics.CountryId == 0)
                    {
                        campaignProfileDemographics.CountryId = countryId;
                    }
                    CampaignProfileDemographicsmodel =
                        Mapper.Map<CampaignProfilePreference, CampaignProfileDemographicsFormModel>(
                            campaignProfileDemographics);
                }
            }

            return CampaignProfileDemographicsmodel;
        }

        private CampaignProfileAdvertFormModel CampaignProfileAdvertMapping(int Id, CampaignProfileAdvertFormModel Campaignad)
        {
            int countryId = 0;
            if (Id == 0)
            {
                countryId = _profileRepository.GetById(Id).CountryId.Value;
            }
            else
            {
                countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("kenya".ToLower())).Id;
            }
            CampaignProfilePreference campaignProfileAdvert = _campaignProfilePreferenceRepository.GetAll().FirstOrDefault();
            if (campaignProfileAdvert == null)
            {
                Campaignad = new CampaignProfileAdvertFormModel(Id);
            }
            else
            {
                bool status = checkcampaignProfileAdvert(campaignProfileAdvert);
                if (status == false)
                {
                    Campaignad = new CampaignProfileAdvertFormModel(Id) { CampaignProfileAdvertsId = campaignProfileAdvert.Id };
                }
                else
                {
                    if (campaignProfileAdvert.CountryId == 0)
                    {
                        campaignProfileAdvert.CountryId = countryId;
                    }
                    Campaignad =
                        Mapper.Map<CampaignProfilePreference, CampaignProfileAdvertFormModel>(campaignProfileAdvert);
                }
            }

            return Campaignad;
        }

        private CampaignProfileSkizaFormModel CampaignProfileSkizaInormationMapping(int Id, CampaignProfileSkizaFormModel CampaignSkiza)
        {
            int countryId = 0;
            if (Id == 0)
            {
                countryId = _profileRepository.GetById(Id).CountryId.Value;
            }
            else
            {
                countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("kenya".ToLower())).Id;
            }
            CampaignProfilePreference campaignProfileSkiza = _campaignProfilePreferenceRepository.GetAll().FirstOrDefault();
            if (campaignProfileSkiza == null)
            {
                CampaignSkiza = new CampaignProfileSkizaFormModel(Id);
            }
            else
            {
                bool status = checkcampaignProfileDemographics(campaignProfileSkiza);
                if (status == false)
                {
                    CampaignSkiza = new CampaignProfileSkizaFormModel(Id) { CampaignProfileSKizaId = campaignProfileSkiza.Id };
                }
                else
                {
                    if (CampaignSkiza.CountryId == 0)
                    {
                        CampaignSkiza.CountryId = countryId;
                    }
                    CampaignSkiza =
                        Mapper.Map<CampaignProfilePreference, CampaignProfileSkizaFormModel>(
                            campaignProfileSkiza);
                }
            }
            return CampaignSkiza;
        }

        private CampaignProfileMobileFormModel UsageMapping(int Id, CampaignProfileMobileFormModel Mobile)
        {
            int countryId = 0;
            if (Id == 0)
            {
                countryId = _profileRepository.GetById(Id).CountryId.Value;
            }
            else
            {
                countryId = _countryRepository.Get(top => top.Name.ToLower().Equals("kenya".ToLower())).Id;
            }
            CampaignProfilePreference campaignProfileMobile = _campaignProfilePreferenceRepository.GetAll().FirstOrDefault();
            if (campaignProfileMobile == null)
            {
                Mobile = new CampaignProfileMobileFormModel(Id);
            }
            else
            {
                if (String.IsNullOrEmpty(campaignProfileMobile.ContractType_Mobile) && String.IsNullOrEmpty(campaignProfileMobile.Spend_Mobile))
                {
                    Mobile = new CampaignProfileMobileFormModel(Id) { CampaignProfileMobileId = campaignProfileMobile.Id };
                }
                else
                {
                    if (campaignProfileMobile.CountryId == 0)
                    {
                        campaignProfileMobile.CountryId = countryId;
                    }
                    Mobile = Mapper.Map<CampaignProfilePreference, CampaignProfileMobileFormModel>(campaignProfileMobile);
                }
            }

            return Mobile;
        }

        public CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingMapping(int id)
        {
            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileTimeSettings != null && campaignProfile.CampaignProfileTimeSettings.Count != 0)
                {
                    CampaignProfileTimeSetting campaignProfileTimeSettings =
                        campaignProfile.CampaignProfileTimeSettings.FirstOrDefault();

                    var model = new CampaignProfileTimeSettingFormModel
                    {
                        CampaignProfileId =
                                            campaignProfile.CampaignProfileId,
                        CampaignProfileTimeSettingsId =
                                            campaignProfileTimeSettings.
                                            CampaignProfileTimeSettingsId
                    };

                    if (campaignProfileTimeSettings.Monday == null)
                        model.MondaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Tuesday == null)
                        model.TuesdaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Wednesday == null)
                        model.WednesdaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Thursday == null)
                        model.ThursdaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Friday == null)
                        model.FridaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Saturday == null)
                        model.SaturdaySelectedTimes = WizardGetTimes();

                    if (campaignProfileTimeSettings.Sunday == null)
                        model.SundaySelectedTimes = WizardGetTimes();

                    model.AvailableTimes = WizardGetTimes();

                    return model;
                }
            }
            if (id == 0)
            {
                return new CampaignProfileTimeSettingFormModel
                { CampaignProfileId = id, AvailableTimes = WizardGetTimes() };
            }
            return new CampaignProfileTimeSettingFormModel
            { CampaignProfileId = id, AvailableTimes = WizardGetTimes() };
        }

        public IList<TimeOfDay> WizardGetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay { Id = "01:00", Name = "01:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "02:00", Name = "02:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "03:00", Name = "03:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "04:00", Name = "04:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "05:00", Name = "05:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "06:00", Name = "06:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "07:00", Name = "07:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "08:00", Name = "08:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "09:00", Name = "09:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "10:00", Name = "10:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "11:00", Name = "11:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "12:00", Name = "12:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "13:00", Name = "13:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "14:00", Name = "14:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "15:00", Name = "15:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "16:00", Name = "16:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "17:00", Name = "17:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "18:00", Name = "18:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "19:00", Name = "19:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "20:00", Name = "20:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "21:00", Name = "21:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "22:00", Name = "22:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "23:00", Name = "23:00", IsSelected = true });
            times.Add(new TimeOfDay { Id = "24:00", Name = "24:00", IsSelected = true });

            return times;
        }

        [HttpPost]
        public ActionResult SaveGeographicWizard(CampaignProfileGeographicFormModel model)
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

                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        UserMatchTableProcess obj = new UserMatchTableProcess();

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            foreach (var item in ConnString)
                            {
                                SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.UpdateCampaignGeographic(model, campaigndetails, SQLServerEntities);
                                }
                                if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                {
                                    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, item);
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

        [HttpPost]
        public ActionResult SaveDemographicsWizard(CampaignProfileDemographicsFormModel model)
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
                            command.NextStatus = true;
                        }
                        else
                        {
                            command.Id = model.CampaignProfileDemographicsId;
                            command.NextStatus = true;
                        }
                    }
                    else
                    {
                        command.Id = model.CampaignProfileDemographicsId;
                        command.NextStatus = true;
                    }
                    ICommandResult result = _commandBus.Submit(command);

                    int countryId = 0;
                    var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == model.CampaignProfileId);
                    if (campaignProfile != null)
                    {
                        countryId = (int)campaignProfile.CountryId;
                    }

                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                    UserMatchTableProcess obj = new UserMatchTableProcess();

                    if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                    {
                        PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                    }

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            SQLServerEntities = new EFMVCDataContex(item);
                            var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                            if (campaigndetails != null)
                            {
                                obj.UpdateCampaignDemographics(model, campaigndetails, SQLServerEntities);
                            }
                            if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                            {
                                PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, item);
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

        [HttpPost]
        public ActionResult SaveAdvertsWizard(CampaignProfileAdvertFormModel model)
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
                            model.CampaignProfileAdvertsId = _campaignProfileId.Id;
                            command.CampaignProfileAdvertsId = model.CampaignProfileAdvertsId;
                            command.NextStatus = true;
                        }
                        else
                        {
                            command.CampaignProfileAdvertsId = model.CampaignProfileAdvertsId;
                            command.NextStatus = true;
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

                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        UserMatchTableProcess obj = new UserMatchTableProcess();

                        if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        {
                            PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        }

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            foreach (var item in ConnString)
                            {
                                SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.UpdateCampaignAdtypes(model, campaigndetails, SQLServerEntities);
                                }
                                if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                {
                                    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, item);
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

        [HttpPost]
        public ActionResult SaveSkizaProfileWizard(CampaignProfileSkizaFormModel model)
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
                            profilePreference.CampaignProfileId = model.CampaignProfileId;
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
                                    var externalCampaignProfileData = SQLServerEntities.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfileData.Id).FirstOrDefault();
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
                                        profilePreference.CountryId = externalServerCountryId;
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

        [HttpPost]
        public ActionResult SaveMobileWizard(CampaignProfileMobileFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                    CreateOrUpdateCampaignProfileMobileCommand command =
                        Mapper.Map<CampaignProfileMobileFormModel, CreateOrUpdateCampaignProfileMobileCommand>(model);
                    //check campaignprofile exists.

                    if (model.CampaignProfileMobileId == 0)
                    {
                        var _campaignProfileId = _campaignProfilePreferenceRepository.Get(top => top.CampaignProfileId == model.CampaignProfileId);
                        if (_campaignProfileId != null)
                        {
                            model.CampaignProfileMobileId = _campaignProfileId.Id;
                            command.CampaignProfileMobileId = model.CampaignProfileMobileId;
                            command.NextStatus = true;
                        }
                        else
                        {
                            command.CampaignProfileMobileId = model.CampaignProfileMobileId;
                            command.NextStatus = true;
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

                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        UserMatchTableProcess obj = new UserMatchTableProcess();

                        if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                        {
                            PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, conn);
                        }

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            foreach (var item in ConnString)
                            {
                                SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfile.CampaignProfileId).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.UpdateUsage(model, campaigndetails, SQLServerEntities);
                                }
                                if (campaignProfile.Status == (int)CampaignStatus.Play && campaignProfile.IsAdminApproval == true)
                                {
                                    PreMatchProcess.PrematchProcessForCampaign(model.CampaignProfileId, item);
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

        [HttpPost]
        public ActionResult SaveTimeSettingsWizard(CampaignProfileTimeSettingFormModel model)
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

                        if (result.Success)
                        {
                            return Json("success");
                        }
                        else
                        {
                            return Json("fail");
                        }
                    }
                }

                return Json("fail");
            }
            else
            {
                return Json("notauthorise");
            }
        }

        private void UpdateCampaignMatch(int CampaignProfileId, int UserId, int campaignMatchId)
        {
            try
            {
                if (CampaignProfileId != 0)
                {
                    int countryId = 0;
                    var campaignProfile1 = _profileRepository.Get(x => x.CampaignProfileId == CampaignProfileId);
                    if (campaignProfile1 != null)
                    {
                        countryId = (int)campaignProfile1.CountryId;
                    }

                    var campaignProfile = _profileRepository.Get(c => c.CampaignProfileId == CampaignProfileId && c.UserId == UserId);

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db1 = new EFMVCDataContex(item);

                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db1, (int)campaignProfile.CampaignProfileId);
                            var campaignMatchDetails = db1.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db1, campaignProfile.UserId);
                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db1, (int)campaignProfile.ClientId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db1, campaignProfile.CountryId.Value);

                            bool isAdd = false;
                            if (campaignMatchDetails == null)
                            {
                                campaignMatchDetails = new CampaignMatch();
                                isAdd = true;
                            }

                            int? operatorClientId;
                            if (externalServerClientId == 0)
                            {
                                operatorClientId = null;
                            }
                            else
                            {
                                operatorClientId = externalServerClientId;
                            }

                            campaignMatchDetails.MaxBid = (int)campaignProfile.MaxBid;
                            campaignMatchDetails.SpentToDate = campaignProfile.SpendToDate.ToString();
                            campaignMatchDetails.AvailableCredit = campaignProfile.AvailableCredit.ToString();
                            campaignMatchDetails.UserId = externalServerUserId;
                            campaignMatchDetails.ClientId = operatorClientId;
                            campaignMatchDetails.CampaignName = campaignProfile.CampaignName;
                            campaignMatchDetails.CampaignDescription = campaignProfile.CampaignDescription;
                            campaignMatchDetails.TotalBudget = (decimal)campaignProfile.TotalBudget;
                            campaignMatchDetails.MaxDailyBudget = (decimal)campaignProfile.MaxDailyBudget;
                            campaignMatchDetails.MaxMonthBudget = (decimal)campaignProfile.MaxMonthBudget;
                            campaignMatchDetails.MaxWeeklyBudget = (decimal)campaignProfile.MaxWeeklyBudget;
                            campaignMatchDetails.MaxHourlyBudget = (decimal)campaignProfile.MaxHourlyBudget;
                            campaignMatchDetails.TotalCredit = (decimal)campaignProfile.TotalCredit;
                            campaignMatchDetails.PlaysToDate = campaignProfile.PlaysToDate;
                            campaignMatchDetails.PlaysLastMonth = campaignProfile.PlaysLastMonth;
                            campaignMatchDetails.PlaysCurrentMonth = campaignProfile.PlaysCurrentMonth;
                            campaignMatchDetails.CancelledToDate = campaignProfile.CancelledToDate;
                            campaignMatchDetails.CancelledLastMonth = campaignProfile.CancelledLastMonth;
                            campaignMatchDetails.CancelledCurrentMonth = campaignProfile.CancelledCurrentMonth;
                            campaignMatchDetails.SmsToDate = campaignProfile.SmsToDate;
                            campaignMatchDetails.SmsLastMonth = campaignProfile.SmsLastMonth;
                            campaignMatchDetails.SmsCurrentMonth = campaignProfile.SmsCurrentMonth;
                            campaignMatchDetails.EmailToDate = campaignProfile.EmailToDate;
                            campaignMatchDetails.EmailsLastMonth = campaignProfile.EmailsLastMonth;
                            campaignMatchDetails.EmailsCurrentMonth = campaignProfile.EmailsCurrentMonth;
                            campaignMatchDetails.EmailFileLocation = campaignProfile.EmailFileLocation;
                            campaignMatchDetails.Active = campaignProfile.Active;
                            campaignMatchDetails.NumberOfPlays = campaignProfile.NumberOfPlays;
                            campaignMatchDetails.AverageDailyPlays = campaignProfile.AverageDailyPlays;
                            campaignMatchDetails.SmsRequests = campaignProfile.SmsRequests;
                            campaignMatchDetails.EmailsDelievered = campaignProfile.EmailsDelievered;
                            campaignMatchDetails.EmailSubject = campaignProfile.EmailSubject;
                            campaignMatchDetails.EmailBody = campaignProfile.EmailBody;
                            campaignMatchDetails.EmailSenderAddress = campaignProfile.EmailSenderAddress;
                            campaignMatchDetails.SmsOriginator = campaignProfile.SmsOriginator;
                            campaignMatchDetails.SmsBody = campaignProfile.SmsBody;
                            campaignMatchDetails.SMSFileLocation = campaignProfile.SMSFileLocation;
                            campaignMatchDetails.CreatedDateTime = DateTime.Now;
                            campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                            campaignMatchDetails.Status = campaignProfile.Status;
                            campaignMatchDetails.StartDate = campaignProfile.StartDate;
                            campaignMatchDetails.EndDate = campaignProfile.EndDate;
                            campaignMatchDetails.NumberInBatch = campaignProfile.NumberInBatch;
                            campaignMatchDetails.CountryId = externalServerCountryId;
                            campaignMatchDetails.RemainingMaxMonthBudget = campaignProfile.RemainingMaxMonthBudget;
                            campaignMatchDetails.RemainingMaxWeeklyBudget = campaignProfile.RemainingMaxWeeklyBudget;
                            campaignMatchDetails.RemainingMaxHourlyBudget = campaignProfile.RemainingMaxHourlyBudget;
                            campaignMatchDetails.RemainingMaxDailyBudget = campaignProfile.RemainingMaxDailyBudget;

                            if (isAdd)
                            {
                                db1.CampaignMatch.Add(campaignMatchDetails);
                                db1.SaveChanges();
                            }
                            else
                            {
                                db1.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException.Message;
            }
        }

        #endregion

        public DataTable ExecuteLinkedSP(string spname, string conn, int UserID)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserID);
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }

        public DataTable ExecuteSP(string spname, string conn)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
            var currency = (from action in _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1)
                            select new SelectListItem
                            {
                                Text = action.CurrencyCode,
                                Value = action.CurrencyId.ToString()
                            }).ToList();
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
                if (id == 12 || id == 13 || id == 14)
                {
                    CountryId = 12;
                }
                else if (id == 11)
                {
                    CountryId = 8;
                }
                else
                {
                    CountryId = Convert.ToInt32(id);
                }
                var currencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;
                currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;
                Session["CountryId"] = CountryId;
                Session["CurrencyId"] = currencyId;
                return Json(new { data = "success", value = currencyCode, value1 = currencyId });
            }
            else if (label == "currency")
            {
                currencyCode = _currencyRepository.Get(c => c.CurrencyId == id).CurrencyCode;
                Session["CurrencyId"] = id;
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
