// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-15-2013
// ***********************************************************************
// <copyright file="ProfileController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Profile;
using EFMVC.Model;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using PagedList;

/// <summary>
/// The Controllers namespace.
/// </summary>

namespace EFMVC.Web.Controllers
{
    /// <summary>
    /// Class ProfileController.
    /// </summary>
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class ProfileController : Controller
    {
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public ProfileController(ICommandBus commandBus, IProfileRepository profileRepository,
                                 IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);

            IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<UserProfileFormModel> userProfileFormModels =
                Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileFormModel>>(userProfiles);
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

            Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();

            foreach (UserProfileFormModel userProfileFormModel in userProfileFormModels)
            {
                userProfileFormModel.Location =
                    selectLists["locationList"].FirstOrDefault(x => x.Value == userProfileFormModel.Location) == null
                        ? "Not Set"
                        : selectLists["locationList"].FirstOrDefault(x => x.Value == userProfileFormModel.Location).Text;
                userProfileFormModel.Gender =
                    selectLists["genderList"].FirstOrDefault(x => x.Value == userProfileFormModel.Gender) == null
                        ? "Not Set"
                        : selectLists["genderList"].FirstOrDefault(x => x.Value == userProfileFormModel.Gender).Text;
                userProfileFormModel.IncomeBracket =
                    selectLists["incomeBracketList"].FirstOrDefault(x => x.Value == userProfileFormModel.IncomeBracket) ==
                    null
                        ? "Not Set"
                        : selectLists["incomeBracketList"].FirstOrDefault(
                            x => x.Value == userProfileFormModel.IncomeBracket).Text;
                userProfileFormModel.WorkingStatus =
                    selectLists["workingStatusList"].FirstOrDefault(x => x.Value == userProfileFormModel.WorkingStatus) ==
                    null
                        ? "Not Set"
                        : selectLists["workingStatusList"].FirstOrDefault(
                            x => x.Value == userProfileFormModel.WorkingStatus).Text;
                userProfileFormModel.RelationshipStatus =
                    selectLists["relationshipStatusList"].FirstOrDefault(
                        x => x.Value == userProfileFormModel.RelationshipStatus) == null
                        ? "Not Set"
                        : selectLists["relationshipStatusList"].FirstOrDefault(
                            x => x.Value == userProfileFormModel.RelationshipStatus).Text;
                userProfileFormModel.Education =
                    selectLists["educationList"].FirstOrDefault(x => x.Value == userProfileFormModel.Education) == null
                        ? "Not Set"
                        : selectLists["educationList"].FirstOrDefault(x => x.Value == userProfileFormModel.Education).
                              Text;
                userProfileFormModel.HouseholdStatus =
                    selectLists["householdStatusList"].FirstOrDefault(
                        x => x.Value == userProfileFormModel.HouseholdStatus) == null
                        ? "Not Set"
                        : selectLists["householdStatusList"].FirstOrDefault(
                            x => x.Value == userProfileFormModel.HouseholdStatus).Text;
            }

            ViewData["User"] = userFormModel;

            return View(userProfileFormModels);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();

            var viewModel = new UserProfileFormModel
                                {
                                    LocationList = selectLists["locationList"],
                                    GenderList = selectLists["genderList"],
                                    IncomeBracketList = selectLists["incomeBracketList"],
                                    WorkingStatusList = selectLists["workingStatusList"],
                                    RelationshipStatusList = selectLists["relationshipStatusList"],
                                    EducationList = selectLists["educationList"],
                                    HouseholdStatusList = selectLists["householdStatusList"]
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();

            UserProfile userProfile = _profileRepository.GetById(id);
            UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);

            model.LocationList = selectLists["locationList"];
            model.GenderList = selectLists["genderList"];
            model.IncomeBracketList = selectLists["incomeBracketList"];
            model.WorkingStatusList = selectLists["workingStatusList"];
            model.RelationshipStatusList = selectLists["relationshipStatusList"];
            model.EducationList = selectLists["educationList"];
            model.HouseholdStatusList = selectLists["householdStatusList"];

            return View(model);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            var command = new DeleteProfileCommand {Id = id};
            _commandBus.Submit(command);

            IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);

            IEnumerable<UserProfileFormModel> userProfileFormModels =
                Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileFormModel>>(userProfiles);

            return PartialView("_ProfileList", userProfileFormModels);
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Save(UserProfileFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateProfileCommand command =
                    Mapper.Map<UserProfileFormModel, CreateOrUpdateProfileCommand>(model);
                command.UserProfileAdverts =
                    Mapper.Map
                        <ICollection<UserProfileAdvertFormModel>, ICollection<CreateOrUpdateUserProfileAdvertCommand>>(
                            model.UserProfileAdverts);
                command.UserProfileAttitudes =
                    Mapper.Map
                        <ICollection<UserProfileAttitudeFormModel>,
                            ICollection<CreateOrUpdateUserProfileAttitudeCommand>>(model.UserProfileAttitudes);
                command.UserProfileCinemas =
                    Mapper.Map
                        <ICollection<UserProfileCinemaFormModel>, ICollection<CreateOrUpdateUserProfileCinemaCommand>>(
                            model.UserProfileCinemas);
                command.UserProfileInternets =
                    Mapper.Map
                        <ICollection<UserProfileInternetFormModel>,
                            ICollection<CreateOrUpdateUserProfileInternetCommand>>(model.UserProfileInternets);
                command.UserProfileMobiles =
                    Mapper.Map
                        <ICollection<UserProfileMobileFormModel>, ICollection<CreateOrUpdateUserProfileMobileCommand>>(
                            model.UserProfileMobiles);
                command.UserProfilePresses =
                    Mapper.Map
                        <ICollection<UserProfilePressFormModel>, ICollection<CreateOrUpdateUserProfilePressCommand>>(
                            model.UserProfilePresses);
                command.UserProfileProductsServices =
                    Mapper.Map
                        <ICollection<UserProfileProductsServiceFormModel>,
                            ICollection<CreateOrUpdateUserProfileProductsServiceCommand>>(
                                model.UserProfileProductsServices);
                command.UserProfileRadios =
                    Mapper.Map
                        <ICollection<UserProfileRadioFormModel>, ICollection<CreateOrUpdateUserProfileRadioCommand>>(
                            model.UserProfileRadios);
                command.UserProfileTimeSettings =
                    Mapper.Map
                        <ICollection<UserProfileTimeSettingFormModel>,
                            ICollection<CreateOrUpdateUserProfileTimeSettingCommand>>(model.UserProfileTimeSettings);
                command.UserProfileTvs =
                    Mapper.Map<ICollection<UserProfileTvFormModel>, ICollection<CreateOrUpdateUserProfileTvCommand>>(
                        model.UserProfileTvs);

                command.UserId = efmvcUser.UserId;

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }
            //if fail
            if (model.UserProfileId == 0)
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
            genderList.Add(new SelectListItem {Text = "Prefer not to answer", Value = "2"});

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
            lists.Add("locationList", locationList);

            return lists;
        }


        /// <summary>
        /// Edits the adverts.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditAdverts(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileAdverts != null && userProfile.UserProfileAdverts.Count != 0)
                {
                    UserProfileAdvert userProfileAdvert = userProfile.UserProfileAdverts.FirstOrDefault();

                    UserProfileAdvertFormModel model =
                        Mapper.Map<UserProfileAdvert, UserProfileAdvertFormModel>(userProfileAdvert);

                    return View("CreateAdvert", model);
                }
            }

            return View("CreateAdvert",
                        new UserProfileAdvertFormModel
                            {
                                UserProfileId = id,
                                AlcoholicDrinks = 1,
                                AppliancesOtherHouseholdDurables = 1,
                                Cinema = 1,
                                CommunicationsInternet = 1,
                                DIYGardening = 1,
                                ElectronicsOtherPersonalItems = 1,
                                Environment = 1,
                                Fashion = 1,
                                FinancialProducts = 1,
                                FinancialServices = 1,
                                Fitness = 1,
                                Food = 1,
                                GeneralUse = 1,
                                GoingOut = 1,
                                Holidays = 1,
                                HolidaysTravel = 1,
                                Householdproducts = 1,
                                Magazines = 1,
                                Motoring = 1,
                                Music = 1,
                                Newspapers = 1,
                                NonAlcoholicDrinks = 1,
                                PetsPetFood = 1,
                                PharmaceuticalChemistsProducts = 1,
                                Radio = 1,
                                Religion = 1,
                                Shopping = 1,
                                ShoppingRetailClothing = 1,
                                SocialNetworking = 1,
                                SportsLeisure = 1,
                                SweetSaltySnacks = 1,
                                TV = 1,
                                TobaccoProducts = 1,
                                ToiletriesCosmetics = 1
                            });
        }

        /// <summary>
        /// Saves the adverts.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveAdverts(UserProfileAdvertFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateUserProfileAdvertCommand command =
                    Mapper.Map<UserProfileAdvertFormModel, CreateOrUpdateUserProfileAdvertCommand>(model);


                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileAdvertsId == 0)
                return View("CreateAdvert", model);

            return View("Index");
        }

        /// <summary>
        /// Edits the attitude.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EditAttitude(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileAttitudes != null && userProfile.UserProfileAttitudes.Count != 0)
                {
                    UserProfileAttitude userProfileAttitude = userProfile.UserProfileAttitudes.FirstOrDefault();

                    UserProfileAttitudeFormModel model =
                        Mapper.Map<UserProfileAttitude, UserProfileAttitudeFormModel>(userProfileAttitude);

                    return View("CreateAttitude", model);
                }
            }

            return View("CreateAttitude",
                        new UserProfileAttitudeFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                Environment = 1,
                                Fashion = 1,
                                Fitness = 1,
                                FinancialStabiity = 1,
                                GoingOut = 1,
                                Holidays = 1,
                                Music = 1,
                                Religion = 1
                            });
        }

        /// <summary>
        /// Saves the attitude.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveAttitude(UserProfileAttitudeFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileAttitudeCommand command =
                    Mapper.Map<UserProfileAttitudeFormModel, CreateOrUpdateUserProfileAttitudeCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileAttitudeId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileCinemas != null && userProfile.UserProfileCinemas.Count != 0)
                {
                    UserProfileCinema userProfileCinema = userProfile.UserProfileCinemas.FirstOrDefault();

                    UserProfileCinemaFormModel model =
                        Mapper.Map<UserProfileCinema, UserProfileCinemaFormModel>(userProfileCinema);

                    return View("CreateCinema", model);
                }
            }

            return View("CreateCinema",
                        new UserProfileCinemaFormModel {UserProfileId = userProfile.UserProfileId, Cinema = 0});
        }

        /// <summary>
        /// Saves the cinema.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveCinema(UserProfileCinemaFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileCinemaCommand command =
                    Mapper.Map<UserProfileCinemaFormModel, CreateOrUpdateUserProfileCinemaCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileCinemaId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileInternets != null && userProfile.UserProfileInternets.Count != 0)
                {
                    UserProfileInternet userProfileInternet = userProfile.UserProfileInternets.FirstOrDefault();

                    UserProfileInternetFormModel model =
                        Mapper.Map<UserProfileInternet, UserProfileInternetFormModel>(userProfileInternet);

                    return View("CreateInternet", model);
                }
            }

            return View("CreateInternet",
                        new UserProfileInternetFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                Auctions = 0,
                                Shopping = 0,
                                SocialNetworking = 0,
                                Research = 0,
                                Video = 0
                            });
        }

        /// <summary>
        /// Saves the internet.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveInternet(UserProfileInternetFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileInternetCommand command =
                    Mapper.Map<UserProfileInternetFormModel, CreateOrUpdateUserProfileInternetCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileInternetId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            IList<SelectListItem> contractTypeList = new List<SelectListItem>();
            contractTypeList.Add(new SelectListItem {Value = "0", Text = "Don't Know"});
            contractTypeList.Add(new SelectListItem {Value = "1", Text = "PAYG"});
            contractTypeList.Add(new SelectListItem {Value = "2", Text = "Contract"});

            IList<SelectListItem> spendList = new List<SelectListItem>();
            spendList.Add(new SelectListItem {Value = "0", Text = "Don't Know"});
            spendList.Add(new SelectListItem {Value = "1", Text = "0-9"});
            spendList.Add(new SelectListItem {Value = "2", Text = "10-19"});
            spendList.Add(new SelectListItem {Value = "3", Text = "20-29"});
            spendList.Add(new SelectListItem {Value = "4", Text = "30-39"});
            spendList.Add(new SelectListItem {Value = "5", Text = "40-49"});
            spendList.Add(new SelectListItem {Value = "6", Text = "50+"});

            if (userProfile != null)
            {
                if (userProfile.UserProfileMobiles != null && userProfile.UserProfileMobiles.Count != 0)
                {
                    UserProfileMobile userProfileMobile = userProfile.UserProfileMobiles.FirstOrDefault();

                    UserProfileMobileFormModel model =
                        Mapper.Map<UserProfileMobile, UserProfileMobileFormModel>(userProfileMobile);
                    model.ContractTypeList = contractTypeList;
                    model.SpendList = spendList;

                    return View("CreateMobile", model);
                }
            }

            return View("CreateMobile",
                        new UserProfileMobileFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                ContractTypeList = contractTypeList,
                                SpendList = spendList
                            });
        }

        /// <summary>
        /// Saves the mobile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveMobile(UserProfileMobileFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileMobileCommand command =
                    Mapper.Map<UserProfileMobileFormModel, CreateOrUpdateUserProfileMobileCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileMobileId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfilePresses != null && userProfile.UserProfilePresses.Count != 0)
                {
                    UserProfilePress userProfilePress = userProfile.UserProfilePresses.FirstOrDefault();

                    UserProfilePressFormModel model =
                        Mapper.Map<UserProfilePress, UserProfilePressFormModel>(userProfilePress);

                    return View("CreatePress", model);
                }
            }

            return View("CreatePress",
                        new UserProfilePressFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                FreeNewpapers = 0,
                                Local = 0,
                                Magazines = 0,
                                National = 0
                            });
        }

        /// <summary>
        /// Saves the press.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SavePress(UserProfilePressFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfilePressCommand command =
                    Mapper.Map<UserProfilePressFormModel, CreateOrUpdateUserProfilePressCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfilePressId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileProductsServices != null &&
                    userProfile.UserProfileProductsServices.Count != 0)
                {
                    UserProfileProductsService userProfileProductsServices =
                        userProfile.UserProfileProductsServices.FirstOrDefault();

                    UserProfileProductsServiceFormModel model =
                        Mapper.Map<UserProfileProductsService, UserProfileProductsServiceFormModel>(
                            userProfileProductsServices);

                    return View("CreateProductsServices", model);
                }
            }

            return View("CreateProductsServices",
                        new UserProfileProductsServiceFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                AlcoholicDrinks = 0,
                                AppliancesOtherHouseholdDurables = 0,
                                CommunicationsInternet = 0,
                                DIYGardening = 0,
                                ElectronicsOtherPersonalItems = 0,
                                FinancialServices = 0,
                                Food = 0,
                                HolidaysTravel = 0,
                                Householdproducts = 0,
                                Motoring = 0,
                                NonAlcoholicDrinks = 0,
                                PetsPetFood = 0,
                                PharmaceuticalChemistsProducts = 0,
                                ShoppingRetailClothing = 0,
                                SportsLeisure = 0,
                                SweetSaltySnacks = 0,
                                TobaccoProducts = 0,
                                ToiletriesCosmetics = 0
                            });
        }

        /// <summary>
        /// Saves the products services.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveProductsServices(UserProfileProductsServiceFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileProductsServiceCommand command =
                    Mapper.Map<UserProfileProductsServiceFormModel, CreateOrUpdateUserProfileProductsServiceCommand>(
                        model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileProductsServicesId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileRadios != null && userProfile.UserProfileRadios.Count != 0)
                {
                    UserProfileRadio userProfileRadio = userProfile.UserProfileRadios.FirstOrDefault();

                    UserProfileRadioFormModel model =
                        Mapper.Map<UserProfileRadio, UserProfileRadioFormModel>(userProfileRadio);

                    return View("CreateRadio", model);
                }
            }

            return View("CreateRadio",
                        new UserProfileRadioFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                Local = 0,
                                Music = 0,
                                National = 0,
                                Sport = 0,
                                Talk = 0
                            });
        }

        /// <summary>
        /// Saves the radio.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveRadio(UserProfileRadioFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileRadioCommand command =
                    Mapper.Map<UserProfileRadioFormModel, CreateOrUpdateUserProfileRadioCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileRadioId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileTimeSettings != null && userProfile.UserProfileTimeSettings.Count != 0)
                {
                    UserProfileTimeSetting userProfileTimeSettings =
                        userProfile.UserProfileTimeSettings.FirstOrDefault();

                    var model = new UserProfileTimeSettingFormModel
                                    {
                                        UserProfileId =
                                            userProfileTimeSettings.UserProfileId,
                                        UserProfileTimeSettingsId =
                                            userProfileTimeSettings.
                                            UserProfileTimeSettingsId
                                    };

                    if (userProfileTimeSettings.Monday != null)
                        model.MondaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Monday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Tuesday != null)
                        model.TuesdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Tuesday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Wednesday != null)
                        model.WednesdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Wednesday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Thursday != null)
                        model.ThursdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Thursday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Friday != null)
                        model.FridaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Friday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Saturday != null)
                        model.SaturdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Saturday.Split(",".ToCharArray()));
                    
                    if (userProfileTimeSettings.Sunday != null)
                        model.SundaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Sunday.Split(",".ToCharArray()));

                    model.AvailableTimes = GetTimes();

                    return View("CreateTimeSettings", model);
                }
            }

            return View("CreateTimeSettings",
                        new UserProfileTimeSettingFormModel
                            {UserProfileId = userProfile.UserProfileId, AvailableTimes = GetTimes()});
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
        public ActionResult SaveTimeSettings(UserProfileTimeSettingFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileTimeSettingCommand command =
                    Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileTimeSettingsId == 0)
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

            if (_profileRepository.Count(x => x.UserProfileId == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileTvs != null && userProfile.UserProfileTvs.Count != 0)
                {
                    UserProfileTv userProfileTv = userProfile.UserProfileTvs.FirstOrDefault();

                    UserProfileTvFormModel model = Mapper.Map<UserProfileTv, UserProfileTvFormModel>(userProfileTv);

                    return View("CreateTv", model);
                }
            }

            return View("CreateTv",
                        new UserProfileTvFormModel
                            {
                                UserProfileId = userProfile.UserProfileId,
                                Cable = 0,
                                Internet = 0,
                                Satallite = 0,
                                Terrestrial = 0
                            });
        }

        /// <summary>
        /// Saves the tv.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SaveTv(UserProfileTvFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileTvCommand command =
                    Mapper.Map<UserProfileTvFormModel, CreateOrUpdateUserProfileTvCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            //if fail
            if (model.UserProfileTvId == 0)
                return View("CreateTv", model);

            return View("Index");
        }

        /// <summary>
        /// Advertses the received.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AdvertsReceived(int? page)
        {
            IPagedList<UserProfileAdvertsReceivedFromModel> listPaged = GetPagedAdverts(page);
                // GetPagedNames is found in BaseController
            if (listPaged == null)
                return HttpNotFound();

            ViewData["ListPaged"] = listPaged;
            return Request.IsAjaxRequest()
                       ? (ActionResult) PartialView("_AdvertsReceivedPartial")
                       : View();
        }

        /// <summary>
        /// Gets the paged adverts.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>IPagedList{UserProfileAdvertsReceivedFromModel}.</returns>
        protected IPagedList<UserProfileAdvertsReceivedFromModel> GetPagedAdverts(int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand
            IEnumerable<UserProfileAdvertsReceivedFromModel> listUnpaged = GetReceivedAdvertsFromDatabase();

            // page the list
            const int pageSize = 20;
            IPagedList<UserProfileAdvertsReceivedFromModel> listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        // in this case we return IEnumerable<string>, but in most
        // - DB situations you'll want to return IQueryable<string>
        /// <summary>
        /// Gets the received adverts from database.
        /// </summary>
        /// <returns>IEnumerable{UserProfileAdvertsReceivedFromModel}.</returns>
        protected IEnumerable<UserProfileAdvertsReceivedFromModel> GetReceivedAdvertsFromDatabase()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            User user = _userRepository.GetById(efmvcUser.UserId);
            UserProfile userProfile = user.UserProfiles.FirstOrDefault();

            IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
                userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.DateTimePlayed);

            IEnumerable<UserProfileAdvertsReceivedFromModel> models =
                Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
                    userProfileAdvertsReceiveds);

            return models;
        }

        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            return View();
        }

        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangeEmail(ChangeEmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var command = new ChangeEmailCommand
                                  {
                                      Email = model.Email,
                                      UserId = efmvcUser.UserId
                                  };

                ICommandResult commandResult = _commandBus.Submit(command);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Changes the msisdn.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ChangeMSISDN()
        {
            return View("ChangeMobileNumber");
        }

        /// <summary>
        /// Changes the msisdn.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangeMSISDN(ChangeMSISDNFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                User user = _userRepository.GetById(efmvcUser.UserId);
                UserProfile userProfile = user.UserProfiles.FirstOrDefault();

                var command = new UpdateMSISDNCommand {UserProfileId = userProfile.UserProfileId, MSISDN = model.MSISDN};

                ICommandResult commandResult = _commandBus.Submit(command);
            }

            return RedirectToAction("Index");
        }
    }
}