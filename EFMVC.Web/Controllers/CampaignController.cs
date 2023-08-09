// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-30-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 02-10-2014
// ***********************************************************************
// <copyright file="CampaignController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;

/// <summary>
/// The Controllers namespace.
/// </summary>

namespace EFMVC.Web.Controllers
{
    /// <summary>
    /// Class CampaignController.
    /// </summary>
    [CompressResponse]
    [Authorize]
    public class CampaignController : Controller
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="advertRepository">The advert repository.</param>
        public CampaignController(ICommandBus commandBus, ICampaignProfileRepository profileRepository,
                                  IUserRepository userRepository, IAdvertRepository advertRepository, 
                                  ICampaignAuditRepository campaignAuditRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
            _campaignAuditRepository = campaignAuditRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);

            IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

            IEnumerable<Advert> adverts = _advertRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<AdvertFormModel> advertFormModels =
                Mapper.Map<IEnumerable<Advert>, IEnumerable<AdvertFormModel>>(adverts);

            ViewData["Adverts"] = advertFormModels;
            ViewData["User"] = userFormModel;

            return View(campaignProfileFormModels);
        } 
        /// <summary>
          /// Indexes this instance.
          /// </summary>
          /// <returns>ActionResult.</returns>
     
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();

            var viewModel = new CampaignProfileFormModel
                                {
                                    CreatedDateTime = DateTime.Now,
                                    UpdatedDateTime = DateTime.Now
                                };

            return View(viewModel);
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Initialise()
        {
            var viewModel = new CampaignProfileFormModel
                                {
                                    CreatedDateTime = DateTime.Now,
                                    UpdatedDateTime = DateTime.Now
                                };

            return View(viewModel);
        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");


            if (id != null)
            {
                var model = GetEditData(id, efmvcUser.UserId);

                if (model != null)
                    return View(model);
            }

            return View("Index");
        }

        private CampaignProfileFormModel GetEditData(int id, int userId)
        {
            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == userId) == 0)
                return null;

            CampaignProfile campaignProfile = _profileRepository.GetById(id);
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
                previousMonthEnd = new DateTime(DateTime.Now.Year -1, 12,
                                                DateTime.DaysInMonth(DateTime.Now.Year-1, 12), 23, 59,
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
            

//            _campaignAuditRepository.Get(x => x.CampaignProfileId == id);

            return model;
        }

        

//        [HttpPost]
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Delete(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            var command = new DeleteCamapignProfileCommand {Id = id};
            _commandBus.Submit(command);

            IEnumerable<CampaignProfile> campaignProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);

            IEnumerable<CampaignProfileFormModel> campaignProfileFormModels =
                Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(campaignProfiles);

            return PartialView("_CampaignList", campaignProfileFormModels);
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Save(CampaignProfileFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

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


                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        return RedirectToAction("Edit", new {id = result.Id});
                    }
                }
            }
            //if fail
            if (model.CampaignProfileId == 0)
                return View("Create", model);

            return View("Edit", model);
        }

        /// <summary>
        /// Gets the select lists.
        /// </summary>
        /// <returns>Dictionary{System.StringIList{SelectListItem}}.</returns>
        private Dictionary<string, IList<SelectListItem>> GetSelectLists()
        {
            var lists = new Dictionary<string, IList<SelectListItem>>();

            IList<SelectListItem> ageBracketList = new List<SelectListItem>();
            ageBracketList.Add(new SelectListItem {Text = "Under 18", Value = "0"});
            ageBracketList.Add(new SelectListItem {Text = "18-24", Value = "1"});
            ageBracketList.Add(new SelectListItem {Text = "25-34", Value = "2"});
            ageBracketList.Add(new SelectListItem {Text = "35-44", Value = "3"});
            ageBracketList.Add(new SelectListItem {Text = "45-54", Value = "4"});
            ageBracketList.Add(new SelectListItem {Text = "55-64", Value = "5"});
            ageBracketList.Add(new SelectListItem {Text = "65+", Value = "6"});
            ageBracketList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "7"});

            IList<SelectListItem> genderList = new List<SelectListItem>();
            genderList.Add(new SelectListItem {Text = "Male", Value = "0"});
            genderList.Add(new SelectListItem {Text = "Female", Value = "1"});
            genderList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "3"});

            IList<SelectListItem> incomeBracketList = new List<SelectListItem>();
            incomeBracketList.Add(new SelectListItem {Text = "£0 to £14,999", Value = "0"});
            incomeBracketList.Add(new SelectListItem {Text = "£15,000 to £24,999", Value = "1"});
            incomeBracketList.Add(new SelectListItem {Text = "£25,000 to £39,999", Value = "2"});
            incomeBracketList.Add(new SelectListItem {Text = "£40,000 to £74,999", Value = "3"});
            incomeBracketList.Add(new SelectListItem {Text = "£75,000 to £99,999", Value = "4"});
            incomeBracketList.Add(new SelectListItem {Text = "£100,000+", Value = "5"});
            incomeBracketList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "6"});

            IList<SelectListItem> workingStatusList = new List<SelectListItem>();
            workingStatusList.Add(new SelectListItem {Text = "Employed", Value = "0"});
            workingStatusList.Add(new SelectListItem {Text = "Self-Employed", Value = "1"});
            workingStatusList.Add(new SelectListItem {Text = "Housewife/Househusband", Value = "2"});
            workingStatusList.Add(new SelectListItem {Text = "Retired", Value = "3"});
            workingStatusList.Add(new SelectListItem {Text = "Unpaid Carer", Value = "4"});
            workingStatusList.Add(new SelectListItem {Text = "Full or Part-time Education", Value = "5"});
            workingStatusList.Add(new SelectListItem {Text = "Not Working", Value = "6"});
            workingStatusList.Add(new SelectListItem {Text = "None of these", Value = "7"});
            workingStatusList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "8"});

            IList<SelectListItem> relationshipStatusList = new List<SelectListItem>();
            relationshipStatusList.Add(new SelectListItem {Text = "Divorced", Value = "0"});
            relationshipStatusList.Add(new SelectListItem {Text = "Living with another", Value = "1"});
            relationshipStatusList.Add(new SelectListItem {Text = "Married", Value = "2"});
            relationshipStatusList.Add(new SelectListItem {Text = "Separated", Value = "3"});
            relationshipStatusList.Add(new SelectListItem {Text = "Single", Value = "4"});
            relationshipStatusList.Add(new SelectListItem {Text = "Widowed", Value = "5"});
            relationshipStatusList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "6"});

            IList<SelectListItem> educationList = new List<SelectListItem>();
            educationList.Add(new SelectListItem {Text = "Secondary", Value = "0"});
            educationList.Add(new SelectListItem {Text = "College", Value = "1"});
            educationList.Add(new SelectListItem {Text = "University", Value = "2"});
            educationList.Add(new SelectListItem {Text = "Post Graduate", Value = "3"});
            educationList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "4"});

            IList<SelectListItem> householdStatusList = new List<SelectListItem>();
            householdStatusList.Add(new SelectListItem {Text = "Rent", Value = "0"});
            householdStatusList.Add(new SelectListItem {Text = "Owner", Value = "1"});
            householdStatusList.Add(new SelectListItem {Text = "Live with someone", Value = "2"});
            householdStatusList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "3"});

            IList<SelectListItem> locationList = new List<SelectListItem>();
            locationList.Add(new SelectListItem {Text = "London", Value = "0"});
            locationList.Add(new SelectListItem {Text = "South East (excl. London)", Value = "1"});
            locationList.Add(new SelectListItem {Text = "South West", Value = "2"});
            locationList.Add(new SelectListItem {Text = "East Anglia", Value = "3"});
            locationList.Add(new SelectListItem {Text = "Midlands", Value = "4"});
            locationList.Add(new SelectListItem {Text = "Wales", Value = "5"});
            locationList.Add(new SelectListItem {Text = "North West", Value = "6"});
            locationList.Add(new SelectListItem {Text = "North East", Value = "7"});
            locationList.Add(new SelectListItem {Text = "Scotland", Value = "8"});
            locationList.Add(new SelectListItem {Text = "Northern Ireland", Value = "9"});

            lists.Add("ageBracketList", ageBracketList);
            lists.Add("genderList", genderList);
            lists.Add("incomeBracketList", incomeBracketList);
            lists.Add("workingStatusList", workingStatusList);
            lists.Add("relationshipStatusList", relationshipStatusList);
            lists.Add("educationList", educationList);
            lists.Add("householdStatusList", householdStatusList);
            lists.Add("locationList", householdStatusList);

            return lists;
        }

        /// <summary>
        /// Edit the demographics
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>ActionResult</returns>
        public ActionResult EditDemographics(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileDemographics != null &&
                    campaignProfile.CampaignProfileDemographics.Count != 0)
                {
                    CampaignProfileDemographics campaignProfileDemographics =
                        campaignProfile.CampaignProfileDemographics.FirstOrDefault();

                    CampaignProfileDemographicsFormModel model =
                        Mapper.Map<CampaignProfileDemographics, CampaignProfileDemographicsFormModel>(
                            campaignProfileDemographics);

                    return View("CreateDemographics", model);
                }
            }

            return View("CreateDemographics", new CampaignProfileDemographicsFormModel {CampaignProfileId = id});
        }

        /// <summary>
        /// Save the demographics
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveDemographics(CampaignProfileDemographicsFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateCampaignProfileDemographicsCommand command =
                    Mapper.Map<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>(
                        model);


                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileDemographicsId == 0)
                return View("CreateDemographics", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the adverts.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditAdverts(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileAdverts != null && campaignProfile.CampaignProfileAdverts.Count != 0)
                {
                    CampaignProfileAdvert campaignProfileAdvert =
                        campaignProfile.CampaignProfileAdverts.FirstOrDefault();

                    CampaignProfileAdvertFormModel model =
                        Mapper.Map<CampaignProfileAdvert, CampaignProfileAdvertFormModel>(campaignProfileAdvert);

                    return View("CreateAdvert", model);
                }
            }

            return View("CreateAdvert", new CampaignProfileAdvertFormModel {CampaignProfileId = id});
        }

        /// <summary>
        /// Saves the adverts.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveAdverts(CampaignProfileAdvertFormModel model)
        {
            int? id = model.CampaignProfileId;
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateCampaignProfileAdvertCommand command =
                    Mapper.Map<CampaignProfileAdvertFormModel, CreateOrUpdateCampaignProfileAdvertCommand>(model);


                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileAdvertsId == 0)
                return View("CreateAdvert", model);

            return View("Edit", id);
        }

        /// <summary>
        /// Edits the attitude.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditAttitude(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileAttitudes != null &&
                    campaignProfile.CampaignProfileAttitudes.Count != 0)
                {
                    CampaignProfileAttitude campaignProfileAttitude =
                        campaignProfile.CampaignProfileAttitudes.FirstOrDefault();

                    CampaignProfileAttitudeFormModel model =
                        Mapper.Map<CampaignProfileAttitude, CampaignProfileAttitudeFormModel>(campaignProfileAttitude);

                    return View("CreateAttitude", model);
                }
            }

            return View("CreateAttitude",
                        new CampaignProfileAttitudeFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the attitude.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveAttitude(CampaignProfileAttitudeFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileAttitudeCommand command =
                    Mapper.Map<CampaignProfileAttitudeFormModel, CreateOrUpdateCampaignProfileAttitudeCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileAttitudeId == 0)
                return View("CreateAttitude", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the cinema.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditCinema(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileCinemas != null && campaignProfile.CampaignProfileCinemas.Count != 0)
                {
                    CampaignProfileCinema campaignProfileCinema =
                        campaignProfile.CampaignProfileCinemas.FirstOrDefault();

                    CampaignProfileCinemaFormModel model =
                        Mapper.Map<CampaignProfileCinema, CampaignProfileCinemaFormModel>(campaignProfileCinema);

                    return View("CreateCinema", model);
                }
            }

            return View("CreateCinema",
                        new CampaignProfileCinemaFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the cinema.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveCinema(CampaignProfileCinemaFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileCinemaCommand command =
                    Mapper.Map<CampaignProfileCinemaFormModel, CreateOrUpdateCampaignProfileCinemaCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileCinemaId == 0)
                return View("CreateCinema", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the internet.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditInternet(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileInternets != null &&
                    campaignProfile.CampaignProfileInternets.Count != 0)
                {
                    CampaignProfileInternet campaignProfileInternet =
                        campaignProfile.CampaignProfileInternets.FirstOrDefault();

                    CampaignProfileInternetFormModel model =
                        Mapper.Map<CampaignProfileInternet, CampaignProfileInternetFormModel>(campaignProfileInternet);

                    return View("CreateInternet", model);
                }
            }

            return View("CreateInternet",
                        new CampaignProfileInternetFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the internet.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveInternet(CampaignProfileInternetFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileInternetCommand command =
                    Mapper.Map<CampaignProfileInternetFormModel, CreateOrUpdateCampaignProfileInternetCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileInternetId == 0)
                return View("CreateInternet", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the mobile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditMobile(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            IList<SelectListItem> contractTypeList = new List<SelectListItem>();
            contractTypeList.Add(new SelectListItem {Text = "PAYG", Value = "PAYG"});
            contractTypeList.Add(new SelectListItem {Text = "Contract", Value = "Contract"});

            IList<SelectListItem> spendList = new List<SelectListItem>();
            spendList.Add(new SelectListItem {Text = "0-10", Value = "0-10"});
            spendList.Add(new SelectListItem {Text = "10-20", Value = "10-20"});
            spendList.Add(new SelectListItem {Text = "30+", Value = "30+"});

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileMobiles != null && campaignProfile.CampaignProfileMobiles.Count != 0)
                {
                    CampaignProfileMobile campaignProfileMobile =
                        campaignProfile.CampaignProfileMobiles.FirstOrDefault();

                    CampaignProfileMobileFormModel model =
                        Mapper.Map<CampaignProfileMobile, CampaignProfileMobileFormModel>(campaignProfileMobile);

                    return View("CreateMobile", model);
                }
            }

            return View("CreateMobile",
                        new CampaignProfileMobileFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the mobile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveMobile(CampaignProfileMobileFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileMobileCommand command =
                    Mapper.Map<CampaignProfileMobileFormModel, CreateOrUpdateCampaignProfileMobileCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileMobileId == 0)
                return View("CreateMobile", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the press.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditPress(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfilePresses != null && campaignProfile.CampaignProfilePresses.Count != 0)
                {
                    CampaignProfilePress campaignProfilePress = campaignProfile.CampaignProfilePresses.FirstOrDefault();

                    CampaignProfilePressFormModel model =
                        Mapper.Map<CampaignProfilePress, CampaignProfilePressFormModel>(campaignProfilePress);

                    return View("CreatePress", model);
                }
            }

            return View("CreatePress",
                        new CampaignProfilePressFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the press.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SavePress(CampaignProfilePressFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfilePressCommand command =
                    Mapper.Map<CampaignProfilePressFormModel, CreateOrUpdateCampaignProfilePressCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfilePressId == 0)
                return View("CreatePress", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the products services.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditProductsServices(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileProductsServices != null &&
                    campaignProfile.CampaignProfileProductsServices.Count != 0)
                {
                    CampaignProfileProductsService campaignProfileProductsServices =
                        campaignProfile.CampaignProfileProductsServices.FirstOrDefault();

                    CampaignProfileProductsServiceFormModel model =
                        Mapper.Map<CampaignProfileProductsService, CampaignProfileProductsServiceFormModel>(
                            campaignProfileProductsServices);

                    return View("CreateProductsServices", model);
                }
            }

            return View("CreateProductsServices",
                        new CampaignProfileProductsServiceFormModel
                            {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the products services.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveProductsServices(CampaignProfileProductsServiceFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileProductsServiceCommand command =
                    Mapper.Map
                        <CampaignProfileProductsServiceFormModel, CreateOrUpdateCampaignProfileProductsServiceCommand>(
                            model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileProductsServicesId == 0)
                return View("CreateProductsServices", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the radio.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditRadio(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileRadios != null && campaignProfile.CampaignProfileRadios.Count != 0)
                {
                    CampaignProfileRadio campaignProfileRadio = campaignProfile.CampaignProfileRadios.FirstOrDefault();

                    CampaignProfileRadioFormModel model =
                        Mapper.Map<CampaignProfileRadio, CampaignProfileRadioFormModel>(campaignProfileRadio);

                    return View("CreateRadio", model);
                }
            }

            return View("CreateRadio",
                        new CampaignProfileRadioFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the radio.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveRadio(CampaignProfileRadioFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileRadioCommand command =
                    Mapper.Map<CampaignProfileRadioFormModel, CreateOrUpdateCampaignProfileRadioCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileRadioId == 0)
                return View("CreateRadio", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the time settings.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditTimeSettings(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.CampaignProfileTimeSettings != null &&
                    userProfile.CampaignProfileTimeSettings.Count != 0)
                {
                    CampaignProfileTimeSetting CampaignProfileTimeSettings =
                        userProfile.CampaignProfileTimeSettings.FirstOrDefault();

                    var model = new CampaignProfileTimeSettingFormModel
                                    {
                                        CampaignProfileId =
                                            CampaignProfileTimeSettings.CampaignProfileId,
                                        CampaignProfileTimeSettingsId =
                                            CampaignProfileTimeSettings.
                                            CampaignProfileTimeSettingsId
                                    };

                    if (CampaignProfileTimeSettings.Monday != null)
                        model.MondaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Monday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Tuesday != null)
                        model.TuesdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Tuesday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Wednesday != null)
                        model.WednesdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Wednesday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Thursday != null)
                        model.ThursdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Thursday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Friday != null)
                        model.FridaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Friday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Saturday != null)
                        model.SaturdaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Saturday.Split(",".ToCharArray()));
                    if (CampaignProfileTimeSettings.Sunday != null)
                        model.SundaySelectedTimes =
                            ConvertTimesArrayToList(CampaignProfileTimeSettings.Sunday.Split(",".ToCharArray()));

                    model.AvailableTimes = GetTimes();

                    return View("CreateTimeSettings", model);
                }
            }

            return View("CreateTimeSettings",
                        new CampaignProfileTimeSettingFormModel
                            {CampaignProfileId = userProfile.CampaignProfileId, AvailableTimes = GetTimes()});
        }

        /// <summary>
        /// Converts the times array to list.
        /// </summary>
        /// <param name="selectedTimes">The selected times.</param>
        /// <returns>IList{TimeOfDay}.</returns>
        public IList<TimeOfDay> ConvertTimesArrayToList(string[] selectedTimes)
        {
            return selectedTimes.Select(time => new TimeOfDay {Id = time, Name = time, IsSelected = true}).ToList();
        }

        /// <summary>
        /// Gets the times.
        /// </summary>
        /// <returns>IList{TimeOfDay}.</returns>
        public IList<TimeOfDay> GetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay {Id = "01:00", Name = "01:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "02:00", Name = "02:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "03:00", Name = "03:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "04:00", Name = "04:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "05:00", Name = "05:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "06:00", Name = "06:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "07:00", Name = "07:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "08:00", Name = "08:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "09:00", Name = "09:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "10:00", Name = "10:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "11:00", Name = "11:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "12:00", Name = "12:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "13:00", Name = "13:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "14:00", Name = "14:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "15:00", Name = "15:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "16:00", Name = "16:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "17:00", Name = "17:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "18:00", Name = "18:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "19:00", Name = "19:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "20:00", Name = "20:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "21:00", Name = "21:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "22:00", Name = "22:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "23:00", Name = "23:00", IsSelected = false});
            times.Add(new TimeOfDay {Id = "24:00", Name = "24:00", IsSelected = false});

            return times;
        }

        /// <summary>
        /// Saves the time settings.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveTimeSettings(CampaignProfileTimeSettingFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileTimeSettingCommand command =
                    Mapper.Map<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>(
                        model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileTimeSettingsId == 0)
                return View("CreateTimeSettings", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the tv.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditTv(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile campaignProfile = _profileRepository.GetById(id);

            if (campaignProfile != null)
            {
                if (campaignProfile.CampaignProfileTvs != null && campaignProfile.CampaignProfileTvs.Count != 0)
                {
                    CampaignProfileTv campaignProfileTv = campaignProfile.CampaignProfileTvs.FirstOrDefault();

                    CampaignProfileTvFormModel model =
                        Mapper.Map<CampaignProfileTv, CampaignProfileTvFormModel>(campaignProfileTv);

                    return View("CreateTv", model);
                }
            }

            return View("CreateTv",
                        new CampaignProfileTvFormModel {CampaignProfileId = campaignProfile.CampaignProfileId});
        }

        /// <summary>
        /// Saves the tv.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveTv(CampaignProfileTvFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateCampaignProfileTvCommand command =
                    Mapper.Map<CampaignProfileTvFormModel, CreateOrUpdateCampaignProfileTvCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Edit", new { id = model.CampaignProfileId });
                }
            }

            //if fail
            if (model.CampaignProfileTvId == 0)
                return View("CreateTv", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the advert media.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult EditAdvertMedia(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_advertRepository.Count(x => x.AdvertId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            Advert advert = _advertRepository.GetById(id);

            if (advert != null)
            {
                AdvertFormModel model = Mapper.Map<Advert, AdvertFormModel>(advert);

                return View("CreateAdvertMedia", model);
            }

            return View("CreateAdvertMedia", new AdvertFormModel {UserId = efmvcUser.UserId});
        }

        /// <summary>
        /// Creates the media advert.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult CreateMediaAdvert()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            return View("CreateAdvertMedia", new AdvertFormModel {UserId = efmvcUser.UserId});
        }

        /// <summary>
        /// Saves the advert media.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveAdvertMedia(AdvertFormModel model)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            if (Request.Files.Count != 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file.ContentLength != 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);

                    string directoryName = Server.MapPath("~/Media/");
                    directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                    if (!Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);

                    string path = Path.Combine(directoryName, fileName + extension);
                    file.SaveAs(path);

                    string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                    if (!Directory.Exists(archiveDirectoryName))
                        Directory.CreateDirectory(archiveDirectoryName);

                    string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                    file.SaveAs(archivePath);

                    model.MediaFileLocation = string.Format("/Media/{0}/{1}", efmvcUser.UserId.ToString(),
                                                            fileName + extension);
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AdvertId == 0)
                    model.CreatedDateTime = DateTime.Now;

                model.UpdatedDateTime = DateTime.Now;
                model.UserId = efmvcUser.UserId;

                CreateOrUpdateAdvertCommand command = Mapper.Map<AdvertFormModel, CreateOrUpdateAdvertCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            if (model.AdvertId == 0)
                return View("CreateAdvertMedia", model);

            return View("Index");
        }

        /// <summary>
        /// Deletes the advert media.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult DeleteAdvertMedia(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_advertRepository.Count(x => x.AdvertId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            var command = new DeleteAdvertCommand {Id = id};
            ICommandResult commandResult = _commandBus.Submit(command);

            if (commandResult.Success)
                return RedirectToAction("Index");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates the campaign advert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult CreateCampaignAdvert(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile profile = _profileRepository.GetById(id);
            IEnumerable<Advert> adverts = profile.User.Adverts.AsEnumerable();
            CampaignAdvert profileAdvert = profile.CampaignAdverts.FirstOrDefault();

            if (profileAdvert == null || profileAdvert.CampaignAdvertId == 0)
            {
                profileAdvert = new CampaignAdvert();
                profileAdvert.CampaignProfileId = id;
            }

            IEnumerable<AdvertFormModel> advertModels =
                Mapper.Map<IEnumerable<Advert>, IEnumerable<AdvertFormModel>>(adverts);
            CampaignAdvertFormModel model = Mapper.Map<CampaignAdvert, CampaignAdvertFormModel>(profileAdvert);

            ViewData.Add("Adverts", advertModels);

            return View("CreateCampaignAdvert", model);
        }

        /// <summary>
        /// Campaigns the audit list.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult CampaignAuditList(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile profile = _profileRepository.GetById(id);
            IEnumerable<CampaignAuditFormModel> model =
                Mapper.Map<IEnumerable<CampaignAudit>, IEnumerable<CampaignAuditFormModel>>(profile.CampaignAudits);

            ViewData["CampaignProfileId"] = id;

            return View(model);
        }

        /// <summary>
        /// Campaigns the audit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auditId">The audit identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult CampaignAudit(int id, int auditId)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.CampaignProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            CampaignProfile profile = _profileRepository.GetById(id);
            CampaignAudit audit = profile.CampaignAudits.FirstOrDefault(x => x.CampaignAuditId == auditId);

            if (audit != null && audit.CampaignAuditId != 0)
            {
                CampaignAuditFormModel model = Mapper.Map<CampaignAudit, CampaignAuditFormModel>(audit);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Saves the campaign advert.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveCampaignAdvert(CampaignAdvertFormModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.AdvertId == 0)
                {
                    ModelState.AddModelError("", "Please assign an advert.");
                    return View("CreateCampaignAdvert", model);
                }

                CreateOrUpdateCampaignAdvertCommand command =
                    Mapper.Map<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>(model);

                ICommandResult commandResult = _commandBus.Submit(command);

                if (commandResult.Success)
                    return RedirectToAction("Edit", new {id = model.CampaignProfileId});
            }

            return RedirectToAction("Edit", new {id = model.CampaignProfileId});
        }

//        [HttpGet]
//        public ActionResult CreateTest()
//        {
//            TestModel model = new TestModel
//                                  {
//                                      TestQuestion = new List<QuestionOptionModel> {
//                                                  new QuestionOptionModel { QuestionName = "Neutral", QuestionValue = "1", Selected = false },
//                                                  new QuestionOptionModel { QuestionName = "Do Want", QuestionValue = "2", Selected = false },
//                                                  new QuestionOptionModel { QuestionName = "Don't Want", QuestionValue = "0", Selected = false }
//                                      }
//                                  };
//
//            return View(model);
//        }
//
//        [HttpPost]
//        public ActionResult SaveTest(TestModel model)
//        {
//            TestEntityModel entityModel = new TestEntityModel();
//            entityModel.TestQuestion = model.TestQuestionValue;
//
//            return RedirectToAction("Index");
//        }
    }
}