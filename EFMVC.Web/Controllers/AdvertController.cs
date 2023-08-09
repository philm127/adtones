using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using EFMVC.Data;
using System.Data.Entity;
using MadScripterWrappers;
using System.Web.Script.Serialization;
using System.Net;
using System.Threading.Tasks;
using Adtones.Rollups.Data.Services;
using Adtones.Rollups.Data.DataObjects;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class AdvertController : Controller
    {
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        private readonly ICountryRepository _countryRepository;
        private readonly IAdvertCategoryRepository _advertCategoryRepository;
        private readonly IAdvertRejectionRepository _advertRejectionRepository;

        private readonly IBillingRepository _billingRepository;

        private readonly IOperatorRepository _operatorRepository;
        private readonly ICurrencyRepository _currencyRepository;

        private readonly StatsProvider _statsProvider;
        private readonly CurrencyConversion _currencyConversion;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="contactsRepository"></param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="advertRejectionRepository"></param>
        /// <param name="clientRepository"></param>
        /// <param name="countryRepository"></param>
        /// <param name="advertCategoryRepository"></param>
        /// <param name="billingRepository"></param>
        /// <param name="operatorRepository"></param>
        /// <param name="currencyRepository"></param>
        /// <param name="statsProvider"></param>
        public AdvertController(ICommandBus commandBus, ICampaignProfileRepository profileRepository, IContactsRepository contactsRepository,
                                IUserRepository userRepository, IAdvertRepository advertRepository, ICountryRepository countryRepository,
                                IAdvertRejectionRepository advertRejectionRepository, IClientRepository clientRepository, 
                                IAdvertCategoryRepository advertCategoryRepository,
                                IBillingRepository billingRepository, IOperatorRepository operatorRepository,
                                ICurrencyRepository currencyRepository, StatsProvider statsProvider)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
            _contactsRepository = contactsRepository;
            _clientRepository = clientRepository;
            _advertCategoryRepository = advertCategoryRepository;
            _countryRepository = countryRepository;
            _advertRejectionRepository = advertRejectionRepository;
            _billingRepository = billingRepository;
            _operatorRepository = operatorRepository;
            _currencyRepository = currencyRepository;
            _statsProvider = statsProvider;
            _currencyConversion = CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        public async Task<ActionResult> Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var stats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Advertiser, efmvcUser.UserId, _currencyConversion);
            
            var _clientdetails = await _clientRepository.AsQueryable().Where(x => x.UserId == efmvcUser.UserId && (x.Status == 1 || x.Status == 2)).ToListAsync();
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);

            FillClient(clientModels);
            FillStatus();

            //TempData["result"] = _result;
            return View(Tuple.Create(new List<AdvertResult>(0), new AdvertFilter(), 
                new KeyFeatureAdvert
                {
                    AvgPlayTime = Convert.ToDouble(stats.DashboardReduced.AvgPlayLength/1000), 
                    NoOfEmail = (int)stats.DashboardReduced.TotalEmail,
                    NoOfSMS = (int)stats.DashboardReduced.TotalSMS,
                    NoOfPlays = (int)stats.DashboardReduced.TotalPlays,
                }));
        }

        [HttpPost]
        public async Task<JsonResult> LoadDataGrid(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                int archivedStatus = (int)AdvertStatus.Archived;
                var curSymbol = new CurrencySymbol().GetCurrencySymbolByCurrencyCode(_currencyConversion.DisplayCurrency.CurrencyCode);
                var stats = await _statsProvider.GetConsolidatedStatsAsync(StatsDetailLevels.Advertiser, efmvcUser.UserId, _currencyConversion);
                var adverts = _advertRepository.AsQueryable().Where(a => a.Status != archivedStatus && a.UserId == efmvcUser.UserId).ToList();
                var joined = adverts
                        .GroupJoin(stats.Dashboard.Where(s => s.AdvertId != null), a => a.AdvertId, s => s.AdvertId, (a, st) => new { Advert = a, Stats = st == null ? new List<DashboardSummariesDao>(0) : st.ToList() })
                        .Select(j => new { Advert = j.Advert, CampaignCount = j.Stats.Count, Stats = j.Stats.Reduce(StatsDetailLevels.Advert, _currencyConversion) })
                        .GroupJoin(_clientRepository.AsQueryable(), j => j.Advert.ClientId, c => c.Id, (j, clients) => new { Advert = j.Advert, CampaignCount = j.CampaignCount, Stats = j.Stats, Client = clients.DefaultIfEmpty().FirstOrDefault() })
                        .Select(j => new
                        {
                            AdvertId = j.Advert.AdvertId,
                            CampaignCount = j.CampaignCount,
                            CampaignId = j.Stats.CampaignId ?? 0,
                            AdvertName = j.Advert.AdvertName,
                            ClientId = j.Client?.Id,
                            ClientName = j.Client?.Name,
                            CreatedDate = j.Advert.CreatedDateTime.ToString("yyyy-MM-dd"),
                            CreatedDateDisplay = j.Advert.CreatedDateTime.ToString("dd/MM/yyyy"),
                            MediaFileLocation = j.Advert.MediaFileLocation,
                            Status = j.Advert.Status,
                            StatusDisplayed = ((AdvertStatus)j.Advert.Status).ToString(),
                            TotalPlays = j.Stats.TotalPlays,
                            ValuePlays = j.Stats.MoreSixSecPlays,
                            AvgBid = j.Stats.AvgBid,
                            CurrencySymbol = curSymbol,
                            CurrencyCode = _currencyConversion.DisplayCurrency.CurrencyCode,
                        }).ToList();
                DTResult<object> result = new DTResult<object>
                {
                    draw = param.Draw,
                    data = joined.Cast<object>().ToList(),
                    recordsFiltered = joined.Count,
                    recordsTotal = joined.Count,
                };

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(new DTResult<object> {data=new List<object> {e.ToString()}, draw = param.Draw, recordsFiltered = 0, recordsTotal = 0});
            }
        }

        [HttpPost]
        public ActionResult DeleteAdvert(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_advertRepository.Count(x => x.AdvertId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            var currentMediaFile = _advertRepository.GetById(id).MediaFileLocation;
            if (currentMediaFile != null)
            {
                var adName = currentMediaFile.Split('/')[3];

                string deleteUserPath = Path.Combine(Server.MapPath("~/Media/"), efmvcUser.UserId.ToString(), adName);
                System.IO.File.Delete(deleteUserPath);

                string deletearchivePath = Path.Combine(Server.MapPath("~/Media/Archive/"), adName);
                System.IO.File.Delete(deletearchivePath);
            }

            var command = new DeleteAdvertCommand { Id = id };
            ICommandResult commandResult = _commandBus.Submit(command);

            if (commandResult.Success)
            {


                TempData["msgsuccess"] = "Record deleted successfully.";
                return Json("success");
            }

            return Json("error");
        }
        public ActionResult AddAdvert(int? clientId, int? campaignId)
        {
            AdvertFormModel _advert = new AdvertFormModel();
            FillAddStatus(null, null);
            _advert.Status = 4;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            FillAddCampaign(efmvcUser.UserId);
            FillOperator(0);
            //Comment 25-03-2019
            //FillAdvertCategory();

            //Add 25-03-2019
            int? userCountryId = null;
            var countryId = _contactsRepository.Get(top => top.UserId == efmvcUser.UserId).CountryId;
            if (countryId == null)
            {
                if (user.OperatorId != 0)
                {
                    userCountryId = _operatorRepository.GetById(user.OperatorId).CountryId.Value;
                    FillAdvertCategory(userCountryId);
                }
                else
                {
                    FillAdvertCategory(0);
                }
            }
            else
            {
                FillAdvertCategory(countryId);
            }

            FillCountry();
            if (clientId != null)
            {
                _advert.ClientId = (int)clientId;
            }
            if (campaignId != null)
            {
                TempData["campaignId"] = campaignId;

            }
            return View(_advert);
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

        public void FillCountry()
        {
            var clientdetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.country = new MultiSelectList(clientdetails, "Id", "Name");

        }

        public ActionResult AdvertDetails(int id)
        {

            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            FillAddCampaign(efmvcUser.UserId);

            //Comment 25-03-2019
            //FillAdvertCategory();

            //Add 25-03-2019
            var userCountryId = _contactsRepository.Get(top => top.UserId == efmvcUser.UserId).CountryId;
            if (userCountryId != 0 && userCountryId != null)
            {
                FillAdvertCategory(userCountryId);
            }
            else
            {
                FillAdvertCategory(0);
            }

            FillCountry();
            if (_advertRepository.Count(x => x.AdvertId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            Advert advert = _advertRepository.GetById(id);

            if (advert != null)
            {
                FillAddStatus(id, advert.Status);
                FillOperator(advert.CountryId);
                AdvertFormModel model = Mapper.Map<Advert, AdvertFormModel>(advert);
                ViewBag.advert = model.AdvertName;
                ViewBag.medialocation = @"~" + model.MediaFileLocation;
                var countryId = advert.CountryId;

                if (countryId != null)
                {
                    var termFileData = _countryRepository.GetById((int)countryId).TermAndConditionFileName;
                    ViewBag.TermAndConditionFile = termFileData;
                }
                else
                {
                    ViewBag.TermAndConditionFile = null;
                }

                if (!string.IsNullOrEmpty(model.ScriptFileLocation))
                {
                    ViewBag.scriptlocation = System.Configuration.ConfigurationManager.AppSettings["siteAddress"] + model.ScriptFileLocation;
                }
                else
                {
                    ViewBag.scriptlocation = String.Empty;
                }
                ViewBag.selectedadvertstatus = advert.Status;

                var campAdvertData = advert.CampaignAdverts.ToList();
                if (campAdvertData.Count() > 0)
                {
                    model.CampaignProfileId = campAdvertData.FirstOrDefault().CampaignProfileId;
                }
                model.IsTermChecked = true;
                return View("AdvertDetails", model);
            }
            return RedirectToAction("Index");

        }
        
        //Add 22-02-2019
        [HttpPost]
        public ActionResult UpdateAdvert(string advertId, string advertName, string advertClientId, string advertDescription, string advertBrand, string advertCampaignProfileId, string advertCategoryId, string script, HttpPostedFileBase mediaFile, HttpPostedFileBase scriptFile, string countryId, string operatorId, string btnCommand)
        {

            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            AdvertFormModel _model = new AdvertFormModel();
            AdvertEmail advertEmail = new AdvertEmail(_commandBus, _userRepository);
            try
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser user = HttpContext.User.GetEFMVCUser();
                    string campaignName = _profileRepository.GetById(Convert.ToInt32(advertCampaignProfileId)).CampaignName;
                    string countryName = _countryRepository.GetById(Convert.ToInt32(countryId)).Name;
                    string operatorName = _operatorRepository.GetById(Convert.ToInt32(operatorId)).OperatorName;

                    _model.AdvertId = Convert.ToInt32(advertId);
                    _model.AdvertName = advertName;
                    if (advertClientId == "")
                    {
                        _model.ClientId = null;
                    }
                    else
                    {
                        _model.ClientId = Convert.ToInt32(advertClientId);
                    }
                    _model.AdvertDescription = advertDescription;
                    _model.Brand = advertBrand;
                    _model.CampaignProfileId = Convert.ToInt32(advertCampaignProfileId);
                    if (!string.IsNullOrEmpty(advertCategoryId))
                    {
                        _model.AdvertCategoryId = Convert.ToInt32(advertCategoryId);
                    }
                    else
                    {
                        _model.AdvertCategoryId = null;
                    }
                    _model.Script = script;
                    _model.CountryId = Convert.ToInt32(countryId);
                    _model.OperatorId = Convert.ToInt32(operatorId);
                    var isExistName = _advertRepository.GetMany(s => s.AdvertName.ToLower().Trim() == _model.AdvertName.ToLower().Trim() && s.UserId == efmvcUser.UserId && s.AdvertId != _model.AdvertId).Any();
                    if (isExistName)
                    {
                        TempData["Error1"] = _model.AdvertName + " is already exists.";
                        var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                        IEnumerable<ClientModel> clientModels =
                            Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                        FillAddClient(clientModels);
                        FillAddCampaign(efmvcUser.UserId);

                        //Comment 25-03-2019
                        //FillAdvertCategory();

                        //Add 25-03-2019
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

                        FillCountry();
                        Advert advert = _advertRepository.GetById(_model.AdvertId);
                        AdvertFormModel model = Mapper.Map<Advert, AdvertFormModel>(advert);
                        ViewBag.advert = model.AdvertName;
                        ViewBag.medialocation = @"~" + model.MediaFileLocation;

                        var countryIdData = advert.CountryId;

                        if (countryIdData != null)
                        {
                            var termFileData = _countryRepository.GetById((int)countryIdData).TermAndConditionFileName;
                            ViewBag.TermAndConditionFile = termFileData;
                        }
                        else
                        {
                            ViewBag.TermAndConditionFile = null;
                        }

                        if (!string.IsNullOrEmpty(model.ScriptFileLocation))
                        {
                            ViewBag.scriptlocation = System.Configuration.ConfigurationManager.AppSettings["siteAddress"] + model.ScriptFileLocation;
                        }
                        else
                        {
                            ViewBag.scriptlocation = String.Empty;
                        }
                        ViewBag.selectedadvertstatus = advert.Status;
                        return Json("exists");
                    }

                    //Add 28-02-2019
                    var campaignCredited = _billingRepository.GetMany(top => top.CampaignProfileId == _model.CampaignProfileId).ToList();
                    if (campaignCredited.Count() > 0)
                    {
                        if (btnCommand == "Submit")
                            _model.Status = (int)AdvertStatus.Waitingforapproval;
                        else
                            _model.Status = (int)AdvertStatus.Draft;
                    }
                    else
                    {
                        //_model.Status = (int)AdvertStatus.CampaignPausedDueToInsufficientFunds;
                        _model.Status = (int)AdvertStatus.Waitingforapproval;
                    }

                    //Commented 28-02-2019
                    //if (btnCommand == "Submit")
                    //    _model.Status = (int)AdvertStatus.Waitingforapproval;
                    //else
                    //    _model.Status = (int)AdvertStatus.Draft;

                    if (Request.Files.Count != 0)
                    {
                        #region Media
                        if (mediaFile != null)
                        {
                            if (mediaFile.ContentLength != 0)
                            {
                                //string fileName = Guid.NewGuid().ToString();
                                //string fileName = _model.AdvertName;
                                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                string fileName = firstAudioName;

                                // FOr safaricom tarnsfer two ads on operator server
                                string fileName2 = null;
                                if (_model.OperatorId == (int)OperatorTableId.Safaricom)
                                {
                                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                    fileName2 = secondAudioName.ToString();
                                }


                                string extension = Path.GetExtension(mediaFile.FileName);

                                var currentMediaFile = _advertRepository.GetById(_model.AdvertId).MediaFileLocation;

                                if (currentMediaFile != null)
                                {
                                    var adName = currentMediaFile.Split('/')[3];

                                    string deleteUserPath = Path.Combine(Server.MapPath("~/Media/"), efmvcUser.UserId.ToString(), adName);
                                    System.IO.File.Delete(deleteUserPath);

                                    string deletearchivePath = Path.Combine(Server.MapPath("~/Media/Archive/"), adName);
                                    System.IO.File.Delete(deletearchivePath);

                                }


                                var userData = _userRepository.GetById(efmvcUser.UserId);
                                string outputFormat = userData.OperatorId == 1 ? "wav" : userData.OperatorId == 2 ? "mp3" : "wav";

                                var audioFormatExtension = "." + outputFormat;
                                if (extension != audioFormatExtension)
                                {
                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                    string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                    mediaFile.SaveAs(tempPath);

                                    SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                    _model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
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

                                    _model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
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

                                string directoryName = Server.MapPath("~/Script/");
                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, fileName + extension);
                                scriptFile.SaveAs(path);

                                string archiveDirectoryName = Server.MapPath("~/Script/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                scriptFile.SaveAs(archivePath);

                                _model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName + extension);
                            }
                        }
                        #endregion
                    }

                    #region Add Record
                    if (_model.AdvertId == 0)
                        _model.CreatedDateTime = DateTime.Now;

                    _model.UpdatedDateTime = DateTime.Now;
                    _model.UserId = efmvcUser.UserId;

                    CreateOrUpdateAdvertCommand command = Mapper.Map<AdvertFormModel, CreateOrUpdateAdvertCommand>(_model);

                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            if (btnCommand == "Submit")
                            {
                                GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                                string message = @"There's a " + _model.AdvertName + " advert in adtones. You'll be able to find new advert in your advert page. Please review the advert details and approve or reject advert.";
                                int subjectId = (int)QuestionSubjectStatus.Adreview;
                                objTicket.CreateAdTicket(efmvcUser.UserId, "Advert Verifcation", message, subjectId, _model.AdvertId);
                                // GenerateAdTicket(_model.AdvertName);

                                //Email Code
                                //advertEmail.SendMail(advertName, _model.OperatorId);
                                advertEmail.SendMail(advertName, _model.OperatorId, user.UserId, campaignName, countryName, operatorName, DateTime.Now);
                            }
                            EFMVCDataContex db = new EFMVCDataContex();
                            var campaignAdvert = db.CampaignAdverts.Where(s => s.AdvertId == _model.AdvertId).FirstOrDefault();

                            if (campaignAdvert != null)
                            {
                                if (_model.CampaignProfileId != 0 && _model.CampaignProfileId != null)
                                {
                                    campaignAdvert.CampaignProfileId = (int)_model.CampaignProfileId;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.CampaignAdverts.Remove(campaignAdvert);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (_model.CampaignProfileId != 0 && _model.CampaignProfileId != null)
                                {
                                    CampaignAdvert campAdvert = new CampaignAdvert();
                                    campAdvert.CampaignProfileId = (int)_model.CampaignProfileId;
                                    campAdvert.AdvertId = _model.AdvertId;
                                    db.CampaignAdverts.Add(campAdvert);
                                    db.SaveChanges();
                                }
                            }
                            //TempData["msgsuccess"] = "Record updated successfully.";
                            TempData["msgsuccess"] = "Advert " + advertName + " updated successfully.";
                            //return RedirectToAction("Index");
                            return Json("success");
                        }

                    }
                    Json("fail");
                    #endregion
                }
                return Json("fail");
            }
            catch (Exception ex)
            {
                var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                IEnumerable<ClientModel> clientModels =
                    Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                FillAddClient(clientModels);
                FillAddCampaign(efmvcUser.UserId);

                //Comment 25-03-2019
                //FillAdvertCategory();

                //Add 25-03-2019
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

                //TempData["Error"] = "Something went wrong, please try again.";
                return Json("fail");
                //TempData["Error"] = ex.Message.ToString();
                //return Json(ex.Message.ToString());
            }
        }
        
        //Add 22-02-2019
        [HttpPost]
        public ActionResult AddAdvert(string advertName, string advertClientId, string advertDescription, string advertBrand, string advertCampaignProfileId, string advertCategoryId, string script, HttpPostedFileBase mediaFile, HttpPostedFileBase scriptFile, string countryId, string btnCommand, string operatorId)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            AdvertFormModel _model = new AdvertFormModel();
            AdvertEmail advertEmail = new AdvertEmail(_commandBus, _userRepository);
            try
            {
                if (ModelState.IsValid)
                {
                    EFMVCUser user = HttpContext.User.GetEFMVCUser();
                    string campaignName = _profileRepository.GetById(Convert.ToInt32(advertCampaignProfileId)).CampaignName;
                    string countryName = _countryRepository.GetById(Convert.ToInt32(countryId)).Name;
                    string operatorName = _operatorRepository.GetById(Convert.ToInt32(operatorId)).OperatorName;

                    _model.AdvertName = advertName;
                    if (advertClientId == "")
                    {
                        _model.ClientId = null;
                    }
                    else
                    {
                        _model.ClientId = Convert.ToInt32(advertClientId);
                    }
                    _model.AdvertDescription = advertDescription;
                    _model.Brand = advertBrand;
                    _model.CampaignProfileId = Convert.ToInt32(advertCampaignProfileId);
                    if (!string.IsNullOrEmpty(advertCategoryId))
                    {
                        _model.AdvertCategoryId = Convert.ToInt32(advertCategoryId);
                    }
                    else
                    {
                        _model.AdvertCategoryId = null;
                    }
                    _model.Script = script;
                    _model.CountryId = Convert.ToInt32(countryId);
                    _model.OperatorId = Convert.ToInt32(operatorId);

                    var isExistName = _advertRepository.GetMany(s => s.AdvertName.ToLower().Trim() == advertName.ToLower().Trim() && s.UserId == efmvcUser.UserId).Any();
                    if (isExistName)
                    {
                        TempData["Error1"] = advertName + " is already exists.";
                        var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                        IEnumerable<ClientModel> clientModels =
                            Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                        FillAddClient(clientModels);
                        FillAddCampaign(efmvcUser.UserId);

                        //Comment 25-03-2019
                        //FillAdvertCategory();

                        //Add 25-03-2019
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

                        FillCountry();
                        return Json("exists");
                    }

                    //Add 28-02-2019
                    var campaignCredited = _billingRepository.GetMany(top => top.CampaignProfileId == _model.CampaignProfileId).ToList();
                    if (campaignCredited.Count() > 0)
                    {
                        if (btnCommand == "Submit")
                            _model.Status = (int)AdvertStatus.Waitingforapproval;
                        else
                            _model.Status = (int)AdvertStatus.Draft;
                    }
                    else
                    {
                        //_model.Status = (int)AdvertStatus.CampaignPausedDueToInsufficientFunds;
                        _model.Status = (int)AdvertStatus.Waitingforapproval;
                    }

                    //Commented 28-02-2019
                    //if (btnCommand == "Submit")
                    //    _model.Status = (int)AdvertStatus.Waitingforapproval;
                    //else
                    //    _model.Status = (int)AdvertStatus.Draft;


                    if (Request.Files.Count != 0)
                    {
                        #region Media
                        if (mediaFile != null)
                        {
                            if (mediaFile.ContentLength != 0)
                            {
                                var userData = _userRepository.GetById(efmvcUser.UserId);


                                //string fileName = Guid.NewGuid().ToString();
                                // string fileName = _model.AdvertName;
                                var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();

                                string fileName = firstAudioName;

                                string fileName2 = null;
                                if (_model.OperatorId == (int)OperatorTableId.Safaricom)
                                {
                                    var secondAudioName = Convert.ToInt64(firstAudioName) + 1;
                                    fileName2 = secondAudioName.ToString();
                                }

                                string extension = Path.GetExtension(mediaFile.FileName);

                                var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                string outputFormat = userData.OperatorId == 1 ? "wav" : userData.OperatorId == 2 ? "mp3" : "wav";
                               //string outputFormat = "wav";
                                var audioFormatExtension = "." + outputFormat;

                                if (extension != audioFormatExtension)
                                {
                                    string tempDirectoryName = Server.MapPath("~/Media/Temp/");
                                    string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                    mediaFile.SaveAs(tempPath);

                                    SaveConvertedFile(tempPath, extension, efmvcUser.UserId.ToString(), fileName, outputFormat, fileName2);

                                    _model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
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

                                    _model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
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

                                string directoryName = Server.MapPath("~/Script/");
                                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                                if (!Directory.Exists(directoryName))
                                    Directory.CreateDirectory(directoryName);

                                string path = Path.Combine(directoryName, fileName + extension);
                                scriptFile.SaveAs(path);

                                string archiveDirectoryName = Server.MapPath("~/Script/Archive/");

                                if (!Directory.Exists(archiveDirectoryName))
                                    Directory.CreateDirectory(archiveDirectoryName);

                                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                scriptFile.SaveAs(archivePath);

                                _model.ScriptFileLocation = string.Format("/Script/{0}/{1}", efmvcUser.UserId.ToString(),
                                                                        fileName + extension);
                            }
                            else
                            {
                                _model.ScriptFileLocation = "";
                            }
                        }
                        else
                        {
                            _model.ScriptFileLocation = "";
                        }
                        #endregion
                    }

                    #region Add Records
                    if (ModelState.IsValid)
                    {
                        if (_model.AdvertId == 0)
                            _model.CreatedDateTime = DateTime.Now;

                        _model.UpdatedDateTime = DateTime.Now;
                        _model.UserId = efmvcUser.UserId;

                        CreateOrUpdateAdvertCommand command = Mapper.Map<AdvertFormModel, CreateOrUpdateAdvertCommand>(_model);

                        if (ModelState.IsValid)
                        {
                            ICommandResult result = _commandBus.Submit(command);

                            if (result.Success)
                            {
                                if (btnCommand == "Submit")
                                {
                                    GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                                    string message = @"There's a " + _model.AdvertName + " advert in adtones. You'll be able to find new advert in your advert page. Please review the advert details and approve or reject advert.";
                                    int subjectId = (int)QuestionSubjectStatus.Adreview;
                                    objTicket.CreateAdTicket(efmvcUser.UserId, "Advert Verifcation", message, subjectId, result.Id);
                                    //GenerateAdTicket(_model.AdvertName);

                                    //Email Code
                                    //advertEmail.SendMail(advertName, _model.OperatorId);
                                    advertEmail.SendMail(advertName, _model.OperatorId, user.UserId, campaignName, countryName, operatorName, DateTime.Now);
                                }
                                int campaignId = 0;
                                if (TempData["campaignId"] != null)
                                {
                                    campaignId = Convert.ToInt32(TempData["campaignId"]);
                                }

                                if (_model.CampaignProfileId != 0 && _model.CampaignProfileId != null)
                                {
                                    campaignId = Convert.ToInt32(_model.CampaignProfileId);
                                }

                                if (campaignId != 0)
                                {
                                    CampaignAdvertFormModel _campaignAdvert = new CampaignAdvertFormModel();
                                    _campaignAdvert.AdvertId = result.Id;
                                    _campaignAdvert.CampaignProfileId = campaignId;
                                    CreateOrUpdateCampaignAdvertCommand campaignAdvertcommand =
                                    Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(_campaignAdvert);

                                    ICommandResult campaignAdvertcommandResult = _commandBus.Submit(campaignAdvertcommand);

                                    if (campaignAdvertcommandResult.Success)
                                    {
                                        //Commented 28-02-2019
                                        //update campaign status to ad approval
                                        //ChangeCampaignStatusCommand _campaignstatus = new ChangeCampaignStatusCommand();
                                        //_campaignstatus.CampaignProfileId = campaignId;
                                        //_campaignstatus.Status = 6;
                                        //ICommandResult campaignstatuscommandResult = _commandBus.Submit(_campaignstatus);
                                        //if (campaignstatuscommandResult.Success)
                                        //{

                                        //}
                                    }
                                }
                                //TempData["msgsuccess"] = "Record added successfully.";
                                // return RedirectToAction("AdvertDetails", "Advert", new { @id = result.Id });
                            }
                        }

                        //Commented 22-02-2019
                        //Add 15-02-2019
                        //TempData["msgsuccess"] = "Record added successfully.";
                        TempData["msgsuccess"] = "Advert " + advertName + " added successfully.";
                        //return RedirectToAction("Index");

                        //Add 22-02-2019
                        return Json("success");
                    }
                    #endregion
                }
                FillAddStatus(null, null);
                return Json("fail");
            }
            catch (Exception ex)
            {
                var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
                IEnumerable<ClientModel> clientModels =
                    Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
                FillAddClient(clientModels);
                FillAddCampaign(efmvcUser.UserId);

                //Comment 25-03-2019
                //FillAdvertCategory();

                //Add 25-03-2019
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

                TempData["Error"] = "Something went wrong, please try again.";
                return Json("fail");
            }
        }
        
        public ActionResult GetRejectedReasonList(int advertId)
        {
            var rejectionList = _advertRejectionRepository.GetMany(s => s.AdvertId == advertId).ToList();
            if (rejectionList.Count() > 0)
            {
                return PartialView("_AdvertRejectionList", rejectionList);
            }
            return Json("False");
        }


        public ActionResult ApproveAdvert(int advertId, string approveReason)
        {
            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            command.AdvertId = advertId;
            command.UpdatedBy = null;
            command.Status = 4; // Waiting For Approval Status
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateAdvertRejectionCommand command2 = new CreateOrUpdateAdvertRejectionCommand();
                command2.AdvertId = advertId;
                command2.UserId = efmvcUser.UserId;
                command2.RejectionReason = approveReason;
                command2.CreatedDate = DateTime.Now;
                ICommandResult result2 = _commandBus.Submit(command2);

                TempData["msgsuccess"] = "Ad approval request sent to admin.";
                return Json("success");
            }
            return Json("error");
        }
        private void SaveConvertedFile(string audioFilePath, string extension, string userId, string fileName, string outputFormat, string fileName2)
        {
            //https://github.com/MadScripter/CloudConvert-.NET-Wrapper/

            var id = Convert.ToInt32(userId);
            //var userData = _userRepository.GetById(id);            

            string inputFormat = extension.Replace(".", "");

            CloudConvert api = new CloudConvert("WNdHFlLrT9GdETzjTJC4BoUsjE6tXbRi8sZX5aokQbua3D2hbJITOTylPs7Nre1A");
            var url = api.GetProcessURL(inputFormat, outputFormat);

            //var audioFilePath = Server.MapPath("~/Audio/FirstAd.mp3");
            // var savePath = Server.MapPath("~/Audio/MyAd.wav");         
            Dictionary<string, object> options = new Dictionary<string, object>()
            {
                { "audio_codec", "PCM MU-LAW" },
                { "audio_bitrate", "64 kbps" },
                { "audio_frequency", "8000" },
                { "audio_channels", "1" }
                
            };

            var convertedFile = api.UploadFileTemp(url, audioFilePath, outputFormat, null, options);
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

                if (fileName2 != null)
                {
                    StoreSecondAudioFile(directoryName, fileName2, outputFormat, savePath);
                }

                string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                if (!Directory.Exists(archiveDirectoryName))
                    Directory.CreateDirectory(archiveDirectoryName);

                string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                System.IO.File.Copy(savePath, archivePath, true);

                System.IO.File.Delete(audioFilePath);
            }

            // var status =  api.GetStatus(url);
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
        
        private void FillClient(IEnumerable<ClientModel> clientModels)
        {
            var clientdetails = clientModels.Select(top => new
            {
                Name = top.Name,
                Id = top.Id.ToString(),
            }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
        }

        private void FillAddCampaign(int UserId)
        {
            // var campaignIdList = _campaignAdvertRepository.GetAll().Select(s => s.CampaignProfileId).ToList();
            // && !campaignIdList.Contains(s.CampaignProfileId)
            var CampaignDetails = _profileRepository.GetAll().Where(s => s.UserId == UserId).Select(top => new SelectListItem { Text = top.CampaignName, Value = top.CampaignProfileId.ToString() }).ToList();
            var AllCampaign = CampaignDetails.ToList();
            ViewBag.allCampaignList = AllCampaign;
        }
        private void FillAdvertCategory(int? userCountryId)
        {
            //Comment 25-03-2019
            //ViewBag.advertCategoryList = _advertCategoryRepository.GetAll().Select(top => new SelectListItem { Text = top.Name, Value = top.AdvertCategoryId.ToString() }).ToList(); 

            //Add 25-03-2019
            if (userCountryId != 0)
            {
                ViewBag.advertCategoryList = _advertCategoryRepository.GetMany(top => top.CountryId == userCountryId).Select(top => new SelectListItem { Text = top.Name, Value = top.AdvertCategoryId.ToString() }).ToList();
            }
            else
            {
                ViewBag.advertCategoryList = _advertCategoryRepository.GetMany(top => top.CountryId == 9).Select(top => new SelectListItem { Text = top.Name, Value = top.AdvertCategoryId.ToString() }).ToList();
            }
        }
        //_advertCategoryRepository
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

        public void FillStatus()
        {


            IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                     .Cast<Common.AdvertStatus>();
            var advertStatus = (from action in advertstatusTypes
                                select new SelectListItem
                                {
                                    Text = action.ToString(),
                                    Value = ((int)action).ToString()
                                }).ToList();

            //ViewBag.advertStatus = new MultiSelectList(advertStatus, "Value", "Text", new int[] { 1, 2, 4, 5, 6 });
            ViewBag.advertStatus = new MultiSelectList(advertStatus, "Value", "Text", new int[] { });
        }
        public void FillAddStatus(int? id, int? status)
        {

            if (id != null)
            {
                if (status != 4)
                {
                    IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                       .Cast<Common.AdvertStatus>();
                    var advertStatus = (from action in advertstatusTypes
                                        where (int)action != 4 && (int)action != 5
                                        select new SelectListItem
                                        {
                                            Text = action.ToString(),
                                            Value = ((int)action).ToString()
                                        }).ToList();
                    ViewBag.advertStatus = advertStatus;
                }
                else
                {
                    IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                       .Cast<Common.AdvertStatus>();
                    var advertStatus = (from action in advertstatusTypes
                                        select new SelectListItem
                                        {
                                            Text = action.ToString(),
                                            Value = ((int)action).ToString()
                                        }).ToList();
                    ViewBag.advertStatus = advertStatus;

                }

            }
            else
            {
                IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                        .Cast<Common.AdvertStatus>();
                var advertStatus = (from action in advertstatusTypes
                                    select new SelectListItem
                                    {
                                        Text = action.ToString(),
                                        Value = ((int)action).ToString()
                                    }).ToList();
                ViewBag.advertStatus = advertStatus;

            }


        }
        [HttpPost]
        public ActionResult GetClientAdvert(int?[] clientId)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                if (clientId != null)
                {

                    var advertdetails = _advertRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && clientId.Contains(top.ClientId)).Select(top => new
                    {
                        AdvertName = top.AdvertName,
                        AdvertId = top.AdvertId.ToString()
                    }).ToList();
                    return Json(advertdetails);

                }
                else
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId).Select(top => new
                    {
                        AdvertName = top.AdvertName,
                        AdvertId = top.AdvertId.ToString()
                    }).ToList();
                    return Json(advertdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        public ActionResult updateAdvertStatus(int advertId, int status)
        {
            Advert advert = _advertRepository.GetById(advertId);
            if (advert != null)
            {
                AdvertFormModel model = Mapper.Map<Advert, AdvertFormModel>(advert);
                model.Status = status;
                CreateOrUpdateAdvertCommand command = Mapper.Map<AdvertFormModel, CreateOrUpdateAdvertCommand>(model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //return Json("success");
                    var statusvalue = (AdvertStatus)command.Status;
                    var advertName = command.AdvertName.ToString();
                    return Json(new { success = "success", value = statusvalue.ToString(), value1 = advertName });
                }
            }
            //return Json("fail");
            return Json(new { success = "fail", value = "Internal Server Error." });
        }

        public ActionResult GetTermAndConditionFile(int countryId)
        {
            var countryData = _countryRepository.GetById(countryId).TermAndConditionFileName;
            if (!string.IsNullOrEmpty(countryData))
            {
                return Json(countryData, JsonRequestBehavior.AllowGet);
            }
            return Json("fail", JsonRequestBehavior.AllowGet);
        }

    }
}
