using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("UserAdminProfile")]
    public class UserAdminProfileController : Controller
    {
        // GET: Users/Profile

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        private readonly ICountryRepository _countryRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IProfileMatchLabelRepository _profileMatchLabelRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        public UserAdminProfileController(ICommandBus commandBus, IProfileRepository profileRepository,
                                IUserRepository userRepository, ICountryRepository countryRepository,
                                IProfileMatchInformationRepository profileMatchInformationRepository,
                                IProfileMatchLabelRepository profileMatchLabelRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _profileMatchLabelRepository = profileMatchLabelRepository;
        }

        private readonly string[] _answerValues = new[]
                                                      {
                                                          "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                          "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                                      };

        [Route("Index")]
        public ActionResult Index()
        {
            UserProfileResult _userProfileResult = new UserProfileResult();
            if (Session["userId"] != null)
            {
                var userid = Convert.ToInt32(Session["userId"]);

                User user = _userRepository.GetById(userid);

                IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == userid);
                IEnumerable<UserProfileFormModel> userProfileFormModels =
                    Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileFormModel>>(userProfiles);
                UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

                Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();
                var countryId = user.Operator.CountryId;
                if (userFormModel.UserProfile != null)
                {
                    UserProfile userProfile = _profileRepository.GetById(userFormModel.UserProfile.UserProfileId);
                    UserProfileAdvertFormModel userProfileAdvert = UserProfileAdvertFormModel(userFormModel.UserProfile.UserProfileId);
                    ProfileMatchOption(userProfileAdvert, countryId);
                    _userProfileResult.UserProfileAdvertFormModel = userProfileAdvert;
                    UserProfileAttitudeFormModel userProfileAttitude = UserProfileAttitudeFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileAttitudeFormModel = userProfileAttitude;

                    SkizaProfileFormModel skizaProfile = UserProfileSkizaModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.SkizaProfileFormModel = skizaProfile;

                    UserProfileCinemaFormModel userProfileCinema = UserProfileCinemaFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileCinemaFormModel = userProfileCinema;
                    UserProfileInternetFormModel userProfileInternet = UserProfileInternetFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileInternetFormModel = userProfileInternet;
                    UserProfileMobileFormModel userProfileMobile = UserProfileMobileFormModel(userFormModel.UserProfile.UserProfileId);
                    MobileMatchOption(userProfileMobile, countryId);
                    _userProfileResult.UserProfileMobileFormModel = userProfileMobile;
                    UserProfilePressFormModel userProfilePress = UserProfilePressFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfilePressFormModel = userProfilePress;
                    UserProfileProductsServiceFormModel userProfileProduct = UserProfileProductsServiceFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileProductsServiceFormModel = userProfileProduct;
                    UserProfileRadioFormModel userProfileRadio = UserProfileRadioFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileRadioFormModel = userProfileRadio;
                    UserProfileTvFormModel userProfileTV = UserProfileTvFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileTvFormModel = userProfileTV;
                    UserProfileTimeSettingFormModel userProfileTimeSetting = UserProfileTimeSettingFormModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileTimeSettingFormModel = userProfileTimeSetting;
                    IEnumerable<UserProfileAdvertsReceivedFromModel> userProfileAdvertReceived = UserProfileAdvertsReceivedFromModel(userFormModel.UserProfile.UserProfileId);
                    _userProfileResult.UserProfileAdvertsReceivedFromModel = userProfileAdvertReceived;
                    UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);

                    // //code commented on 30-03-2017
                    //model.LocationList = selectLists["locationList"];
                    model.GenderList = selectLists["genderList"];
                    model.IncomeBracketList = selectLists["incomeBracketList"];
                    model.WorkingStatusList = selectLists["workingStatusList"];
                    model.RelationshipStatusList = selectLists["relationshipStatusList"];
                    model.EducationList = selectLists["educationList"];
                    model.HouseholdStatusList = selectLists["householdStatusList"];

                    _userProfileResult.userProfileFormModels = userProfileFormModels;
                    _userProfileResult.userProfileModel = model;
                    return View(_userProfileResult);
                }
                else
                {
                    UserProfile userProfile = new UserProfile();
                    UserProfileAdvertFormModel userProfileAdvert = new UserProfileAdvertFormModel();
                    ProfileMatchOption(userProfileAdvert, countryId);
                    _userProfileResult.UserProfileAdvertFormModel = UserProfileAdvertFormModel(0);
                    UserProfileAttitudeFormModel userProfileAttitude = UserProfileAttitudeFormModel(0);
                    _userProfileResult.UserProfileAttitudeFormModel = userProfileAttitude;
                    UserProfileCinemaFormModel userProfileCinema = UserProfileCinemaFormModel(0);
                    _userProfileResult.UserProfileCinemaFormModel = userProfileCinema;
                    UserProfileInternetFormModel userProfileInternet = UserProfileInternetFormModel(0);
                    _userProfileResult.UserProfileInternetFormModel = userProfileInternet;
                    UserProfileMobileFormModel userProfileMobile = UserProfileMobileFormModel(0);
                    MobileMatchOption(userProfileMobile, countryId);
                    _userProfileResult.UserProfileMobileFormModel = userProfileMobile;
                    UserProfilePressFormModel userProfilePress = UserProfilePressFormModel(0);
                    _userProfileResult.UserProfilePressFormModel = userProfilePress;
                    UserProfileProductsServiceFormModel userProfileProduct = UserProfileProductsServiceFormModel(0);
                    _userProfileResult.UserProfileProductsServiceFormModel = userProfileProduct;
                    UserProfileRadioFormModel userProfileRadio = UserProfileRadioFormModel(0);
                    _userProfileResult.UserProfileRadioFormModel = userProfileRadio;
                    UserProfileTvFormModel userProfileTV = UserProfileTvFormModel(0);
                    _userProfileResult.UserProfileTvFormModel = userProfileTV;
                    UserProfileTimeSettingFormModel userProfileTimeSetting = UserProfileTimeSettingFormModel(0);
                    _userProfileResult.UserProfileTimeSettingFormModel = userProfileTimeSetting;
                    IEnumerable<UserProfileAdvertsReceivedFromModel> userProfileAdvertReceived = UserProfileAdvertsReceivedFromModel(0);
                    _userProfileResult.UserProfileAdvertsReceivedFromModel = userProfileAdvertReceived;
                    UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);

                    // //code commented on 30-03-2017
                   // model.LocationList = selectLists["locationList"];
                    model.GenderList = selectLists["genderList"];
                    model.IncomeBracketList = selectLists["incomeBracketList"];
                    model.WorkingStatusList = selectLists["workingStatusList"];
                    model.RelationshipStatusList = selectLists["relationshipStatusList"];
                    model.EducationList = selectLists["educationList"];
                    model.HouseholdStatusList = selectLists["householdStatusList"];

                    _userProfileResult.userProfileFormModels = userProfileFormModels;
                    _userProfileResult.userProfileModel = model;
                    return View(_userProfileResult);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "UserAdmin" });
            }
        }

        private UserProfileMobileFormModel MobileMatchOption(UserProfileMobileFormModel model, int? countryId)
        {
            model.DisplayContractType = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Mobileplan);
            model.DisplaySpend = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.AverageMonthlySpend);

            return model;
        }

        private UserProfileAdvertFormModel ProfileMatchOption(UserProfileAdvertFormModel model, int? countryId)
        {
            model.Food = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Food");
            model.SweetsSnacks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Sweets/Snacks");
            model.AlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Alcoholic Drinks");
            model.NonAlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Non-Alcoholic Drinks");
            model.HouseholdAppliancesProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Household Appliances/Products");
            model.ToiletriesCosmetics = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Toiletries/Cosmetics");
            model.PharmaceuticalChemistsProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Pharmaceutical/Chemists Products");
            model.TobaccoProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Tobacco Products");
            model.Pets = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Pets");
            model.ClothingFashion = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Clothing/Fashion");
            model.DIYGardening = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "DIY/Gardening");
            model.ElectronicsOtherPersonalItems = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Electronics/Other Personal Items");
            model.CommunicationsInternetTelecom = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Communications/Internet Telecom");
            model.FinancialServices = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Financial Services");
            model.HolidaysTravelTourism = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Holidays/Travel Tourism");
            model.SportsLeisure = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Sports/Leisure");
            model.MotoringAutomotive = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Motoring/Automotive");
            model.NewspapersMagazines = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Newspapers/Magazines");
            model.TvVideoRadio = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "TV/Video/ Radio");
            model.Cinema = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Cinema");
            model.SocialNetworking = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Social Networking");
            model.Shopping = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Shopping(retail gen merc)");
            model.Fitness = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Fitness");
            model.Environment = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Environment");
            model.GoingOutEntertainment = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Going Out/Entertainment");
            model.Religion = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Religion");
            model.Music = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Music");
            model.BusinessOpportunities = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Business/opportunities");
            model.Over18Gambling = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Over 18/Gambling");
            model.Restaurants = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Restaurants");
            model.Insurance = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Insurance");
            model.Furniture = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Furniture");
            model.Informationtechnology = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Information technology");
            model.Energy = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Energy");
            model.Supermarkets = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Supermarkets");
            model.Healthcare = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Healthcare");
            model.JobsandEducation = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Jobs and Education");
            model.Gifts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Gifts");
            model.AdvocacyLegal = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Advocacy/Legal");
            model.DatingPersonal = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Dating & Personal");
            model.RealEstateProperty = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Real Estate/Property");
            model.Games = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Games");
            model.SkizaProfile = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Skiza Profile");

            //model.Food = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Food);
            //model.SweetsSnacks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.SweetsSnacks);
            //model.AlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.AlcoholicDrinks);
            //model.NonAlcoholicDrinks = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.NonAlcoholicDrinks);
            //model.HouseholdAppliancesProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.HouseholdAppliancesProducts);
            //model.ToiletriesCosmetics = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.ToiletriesCosmetics);
            //model.PharmaceuticalChemistsProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.PharmaceuticalChemistsProducts);
            //model.TobaccoProducts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.TobaccoProducts);
            //model.Pets = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Pets);
            //model.ClothingFashion = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.ClothingFashion);
            //model.DIYGardening = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.DIYGardening);
            //model.ElectronicsOtherPersonalItems = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.ElectronicsOtherPersonalItems);
            //model.CommunicationsInternetTelecom = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.CommunicationsInternetTelecom);
            //model.FinancialServices = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.FinancialServices);
            //model.HolidaysTravelTourism = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.HolidaysTravelTourism);
            //model.SportsLeisure = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.SportsLeisure);
            //model.MotoringAutomotive = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.MotoringAutomotive);
            //model.NewspapersMagazines = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.NewspapersMagazines);
            //model.TvVideoRadio = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.TvVideoRadio);
            //model.Cinema = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Cinema);
            //model.SocialNetworking = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.SocialNetworking);
            //model.Shopping = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Shopping);
            //model.Fitness = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Fitness);
            //model.Environment = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Environment);
            //model.GoingOutEntertainment = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.GoingOutEntertainment);
            //model.Religion = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Religion);
            //model.Music = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Music);
            //model.BusinessOpportunities = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.BusinessOpportunities);
            //model.Over18Gambling = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Over18Gambling);
            //model.Restaurants = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Restaurants);
            //model.Insurance = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Insurance);
            //model.Furniture = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Furniture);
            //model.Informationtechnology = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Informationtechnology);
            //model.Energy = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Energy);
            //model.Supermarkets = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Supermarkets);
            //model.Healthcare = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Healthcare);
            //model.JobsandEducation = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.JobsandEducation);
            //model.Gifts = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Gifts);
            //model.AdvocacyLegal = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.AdvocacyLegal);
            //model.DatingPersonal = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.DatingPersonal);
            //model.RealEstateProperty = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.RealEstateProperty);
            //model.Games = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.Games);
            //model.SkizaProfile = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.SkizaProfile);

            return model;

        }
        public bool checkUserProfileAdvert(UserProfilePreference userProfileAdvert)
        {
            if (string.IsNullOrEmpty(userProfileAdvert.AlcoholicDrinks_Advert) && string.IsNullOrEmpty(userProfileAdvert.AppliancesOtherHouseholdDurables_Advert) && string.IsNullOrEmpty(userProfileAdvert.Cinema_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.CommunicationsInternet_Advert) && string.IsNullOrEmpty(userProfileAdvert.DIYGardening_Advert) && string.IsNullOrEmpty(userProfileAdvert.ElectronicsOtherPersonalItems_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.Environment_Advert) && string.IsNullOrEmpty(userProfileAdvert.Fashion_Advert) && string.IsNullOrEmpty(userProfileAdvert.FinancialProducts_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.FinancialServices_Advert) && string.IsNullOrEmpty(userProfileAdvert.Fitness_Advert) && string.IsNullOrEmpty(userProfileAdvert.Food_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.GeneralUse_Advert) && string.IsNullOrEmpty(userProfileAdvert.GoingOut_Advert) && string.IsNullOrEmpty(userProfileAdvert.Holidays_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.HolidaysTravel_Advert) && string.IsNullOrEmpty(userProfileAdvert.Householdproducts_Advert) && string.IsNullOrEmpty(userProfileAdvert.Magazines_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.Motoring_Advert) && string.IsNullOrEmpty(userProfileAdvert.Music_Advert) && string.IsNullOrEmpty(userProfileAdvert.Newspapers_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.NonAlcoholicDrinks_Advert) && string.IsNullOrEmpty(userProfileAdvert.PetsPetFood_Advert) && string.IsNullOrEmpty(userProfileAdvert.PharmaceuticalChemistsProducts_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.Radio_Advert) && string.IsNullOrEmpty(userProfileAdvert.Religion_Advert) && string.IsNullOrEmpty(userProfileAdvert.Shopping_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.ShoppingRetailClothing_Advert) && string.IsNullOrEmpty(userProfileAdvert.SocialNetworking_Advert) && string.IsNullOrEmpty(userProfileAdvert.SportsLeisure_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.SweetSaltySnacks_Advert) && string.IsNullOrEmpty(userProfileAdvert.TV_Advert) && string.IsNullOrEmpty(userProfileAdvert.TobaccoProducts_Advert)
                && string.IsNullOrEmpty(userProfileAdvert.ToiletriesCosmetics_Advert)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileAdvertFormModel UserProfileAdvertFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileAdvertFormModel model = new ViewModels.UserProfileAdvertFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileAdvert = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileAdvert(userProfileAdvert);
                    if (status == false)
                    {
                        model = new UserProfileAdvertFormModel
                        {

                            Id = userProfileAdvert.Id,
                            UserProfileId = id,
                            AlcoholicDrinks_Advert = "B",
                            //AppliancesOtherHouseholdDurables_Advert = "B",
                            Cinema_Advert = "B",
                            CommunicationsInternet_Advert = "B",
                            DIYGardening_Advert = "B",
                            ElectronicsOtherPersonalItems_Advert = "B",
                            Environment_Advert = "B",
                            //Fashion_Advert = "B",
                           // FinancialProducts_Advert = "B",
                            FinancialServices_Advert = "B",
                            Fitness_Advert = "B",
                            Food_Advert = "B",
                           // GeneralUse_Advert = "B",
                            GoingOut_Advert = "B",
                          //  Holidays_Advert = "B",
                            HolidaysTravel_Advert = "B",
                            Householdproducts_Advert = "B",
                            //Magazines_Advert = "B",
                            Motoring_Advert = "B",
                            Music_Advert = "B",
                            Newspapers_Advert = "B",
                            NonAlcoholicDrinks_Advert = "B",
                            PetsPetFood_Advert = "B",
                            PharmaceuticalChemistsProducts_Advert = "B",
                           // Radio_Advert = "B",
                            Religion_Advert = "B",
                            Shopping_Advert = "B",
                            ShoppingRetailClothing_Advert = "B",
                            SocialNetworking_Advert = "B",
                            SportsLeisure_Advert = "B",
                            SweetSaltySnacks_Advert = "B",
                            TV_Advert = "B",
                            TobaccoProducts_Advert = "B",
                            ToiletriesCosmetics_Advert = "B"
                        };
                    }
                    else
                    {
                        model =
                       Mapper.Map<UserProfilePreference, UserProfileAdvertFormModel>(userProfileAdvert);

                    }
                    return model;

                }
                else
                {

                    model = new UserProfileAdvertFormModel
                    {

                        UserProfileId = id,
                        AlcoholicDrinks_Advert = "B",
                       // AppliancesOtherHouseholdDurables_Advert = "B",
                        Cinema_Advert = "B",
                        CommunicationsInternet_Advert = "B",
                        DIYGardening_Advert = "B",
                        ElectronicsOtherPersonalItems_Advert = "B",
                        Environment_Advert = "B",
                       // Fashion_Advert = "B",
                       // FinancialProducts_Advert = "B",
                        FinancialServices_Advert = "B",
                        Fitness_Advert = "B",
                        Food_Advert = "B",
                      //  GeneralUse_Advert = "B",
                        GoingOut_Advert = "B",
                       // Holidays_Advert = "B",
                        HolidaysTravel_Advert = "B",
                        Householdproducts_Advert = "B",
                       // Magazines_Advert = "B",
                        Motoring_Advert = "B",
                        Music_Advert = "B",
                        Newspapers_Advert = "B",
                        NonAlcoholicDrinks_Advert = "B",
                        PetsPetFood_Advert = "B",
                        PharmaceuticalChemistsProducts_Advert = "B",
                       // Radio_Advert = "B",
                        Religion_Advert = "B",
                        Shopping_Advert = "B",
                        ShoppingRetailClothing_Advert = "B",
                        SocialNetworking_Advert = "B",
                        SportsLeisure_Advert = "B",
                        SweetSaltySnacks_Advert = "B",
                        TV_Advert = "B",
                        TobaccoProducts_Advert = "B",
                        ToiletriesCosmetics_Advert = "B"
                    };
                    return model;
                }
            }
            else
            {
                model = new UserProfileAdvertFormModel
                {
                    UserProfileId = id,
                    AlcoholicDrinks_Advert = "B",
                  //  AppliancesOtherHouseholdDurables_Advert = "B",
                    Cinema_Advert = "B",
                    CommunicationsInternet_Advert = "B",
                    DIYGardening_Advert = "B",
                    ElectronicsOtherPersonalItems_Advert = "B",
                    Environment_Advert = "B",
                    //Fashion_Advert = "B",
                    //FinancialProducts_Advert = "B",
                    FinancialServices_Advert = "B",
                    Fitness_Advert = "B",
                    Food_Advert = "B",
                   // GeneralUse_Advert = "B",
                    GoingOut_Advert = "B",
                   // Holidays_Advert = "B",
                    HolidaysTravel_Advert = "B",
                    Householdproducts_Advert = "B",
                   // Magazines_Advert = "B",
                    Motoring_Advert = "B",
                    Music_Advert = "B",
                    Newspapers_Advert = "B",
                    NonAlcoholicDrinks_Advert = "B",
                    PetsPetFood_Advert = "B",
                    PharmaceuticalChemistsProducts_Advert = "B",
                   // Radio_Advert = "B",
                    Religion_Advert = "B",
                    Shopping_Advert = "B",
                    ShoppingRetailClothing_Advert = "B",
                    SocialNetworking_Advert = "B",
                    SportsLeisure_Advert = "B",
                    SweetSaltySnacks_Advert = "B",
                    TV_Advert = "B",
                    TobaccoProducts_Advert = "B",
                    ToiletriesCosmetics_Advert = "B"
                };
                return model;
            }

        }
        public bool checkUserProfileAttitude(UserProfilePreference userProfileAttitude)
        {
            if (string.IsNullOrEmpty(userProfileAttitude.Environment_Attitude) && string.IsNullOrEmpty(userProfileAttitude.Fashion_Attitude) && string.IsNullOrEmpty(userProfileAttitude.Fitness_Attitude)
                && string.IsNullOrEmpty(userProfileAttitude.FinancialStabiity_Attitude) && string.IsNullOrEmpty(userProfileAttitude.GoingOut_Attitude) && string.IsNullOrEmpty(userProfileAttitude.Holidays_Attitude)
                && string.IsNullOrEmpty(userProfileAttitude.Music_Attitude) && string.IsNullOrEmpty(userProfileAttitude.Religion_Attitude)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileAttitudeFormModel UserProfileAttitudeFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileAttitudeFormModel model = new ViewModels.UserProfileAttitudeFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            if (id == 0)
            {
                model = new UserProfileAttitudeFormModel
                {
                    UserProfileId = id,
                    Environment_Attitude = "B",
                    Fashion_Attitude = "B",
                    Fitness_Attitude = "B",
                    FinancialStabiity_Attitude = "B",
                    GoingOut_Attitude = "B",
                    Holidays_Attitude = "B",
                    Music_Attitude = "B",
                    Religion_Attitude = "B"
                };
                return model;
            }
            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileAttitude = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileAttitude(userProfileAttitude);
                    if (status == false)
                    {
                        model = new UserProfileAttitudeFormModel
                        {
                            Id = userProfileAttitude.Id,
                            UserProfileId = userProfile.UserProfileId,
                            Environment_Attitude = "B",
                            Fashion_Attitude = "B",
                            Fitness_Attitude = "B",
                            FinancialStabiity_Attitude = "B",
                            GoingOut_Attitude = "B",
                            Holidays_Attitude = "B",
                            Music_Attitude = "B",
                            Religion_Attitude = "B"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileAttitudeFormModel>(userProfileAttitude);
                    }
                    return model;
                }
                else
                {
                    model = new UserProfileAttitudeFormModel
                    {
                        UserProfileId = userProfile.UserProfileId,
                        Environment_Attitude = "B",
                        Fashion_Attitude = "B",
                        Fitness_Attitude = "B",
                        FinancialStabiity_Attitude = "B",
                        GoingOut_Attitude = "B",
                        Holidays_Attitude = "B",
                        Music_Attitude = "B",
                        Religion_Attitude = "B"
                    };
                    return model;
                }
            }
            else
            {
                model = new UserProfileAttitudeFormModel
                {
                    UserProfileId = userProfile.UserProfileId,
                    Environment_Attitude = "B",
                    Fashion_Attitude = "B",
                    Fitness_Attitude = "B",
                    FinancialStabiity_Attitude = "B",
                    GoingOut_Attitude = "B",
                    Holidays_Attitude = "B",
                    Music_Attitude = "B",
                    Religion_Attitude = "B"
                };
                return model;
            }
        }

        public SkizaProfileFormModel UserProfileSkizaModel(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            SkizaProfileFormModel model = new ViewModels.SkizaProfileFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            if (id == 0)
            {
                return new SkizaProfileFormModel
                {
                    UserProfileId = id,
                    Hustlers_AdType = "A",
                    Youth_AdType = "A",
                    DiscerningProfessionals_AdType = "A",
                    Mass_AdType = "A"
                };
            }
            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileSkiza = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileSkiza(userProfileSkiza);
                    if (status == false)
                    {
                        return new SkizaProfileFormModel
                        {
                            Id = userProfileSkiza.Id,
                            UserProfileId = userProfile.UserProfileId,
                            Hustlers_AdType = "A",
                            Youth_AdType = "A",
                            DiscerningProfessionals_AdType = "A",
                            Mass_AdType = "A"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, SkizaProfileFormModel>(userProfileSkiza);
                    }
                    return model;
                }
            }
            return new SkizaProfileFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                Hustlers_AdType = "A",
                Youth_AdType = "A",
                DiscerningProfessionals_AdType = "A",
                Mass_AdType = "A"
            };

        }

        public bool checkUserProfileSkiza(UserProfilePreference userProfileInternet)
        {
            if (string.IsNullOrEmpty(userProfileInternet.Hustlers_AdType) && string.IsNullOrEmpty(userProfileInternet.Youth_AdType) && string.IsNullOrEmpty(userProfileInternet.DiscerningProfessionals_AdType)
                && string.IsNullOrEmpty(userProfileInternet.Mass_AdType)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool checkUserProfileInternet(UserProfilePreference userProfileInternet)
        {
            if (string.IsNullOrEmpty(userProfileInternet.Auctions_Internet) && string.IsNullOrEmpty(userProfileInternet.Shopping_Internet) && string.IsNullOrEmpty(userProfileInternet.SocialNetworking_Internet)
                && string.IsNullOrEmpty(userProfileInternet.Research_Internet) && string.IsNullOrEmpty(userProfileInternet.Video_Internet)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileCinemaFormModel UserProfileCinemaFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileCinemaFormModel model = new ViewModels.UserProfileCinemaFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            if (id == 0)
            {
                return new UserProfileCinemaFormModel { UserProfileId = id, Cinema_Cinema = "A" };
            }
            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileCinema = userProfile.UserProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(userProfileCinema.Cinema_Cinema))
                    {
                        return new UserProfileCinemaFormModel { UserProfileId = id, Cinema_Cinema = "A", Id = userProfileCinema.Id };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileCinemaFormModel>(userProfileCinema);
                    }
                    return model;
                }
            }
            return new UserProfileCinemaFormModel { UserProfileId = userProfile.UserProfileId, Cinema_Cinema = "A" };

        }

        public UserProfileInternetFormModel UserProfileInternetFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileInternetFormModel model = new ViewModels.UserProfileInternetFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            if (id == 0)
            {
                return new UserProfileInternetFormModel
                {
                    UserProfileId = id,
                    Auctions_Internet = "A",
                    Shopping_Internet = "A",
                    SocialNetworking_Internet = "A",
                    Research_Internet = "A",
                    Video_Internet = "A"
                };
            }
            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileInternet = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileInternet(userProfileInternet);
                    if (status == false)
                    {
                        return new UserProfileInternetFormModel
                        {
                            Id = userProfileInternet.Id,
                            UserProfileId = userProfile.UserProfileId,
                            Auctions_Internet = "A",
                            Shopping_Internet = "A",
                            SocialNetworking_Internet = "A",
                            Research_Internet = "A",
                            Video_Internet = "A"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileInternetFormModel>(userProfileInternet);
                    }
                    return model;
                }
            }
            return new UserProfileInternetFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                Auctions_Internet = "A",
                Shopping_Internet = "A",
                SocialNetworking_Internet = "A",
                Research_Internet = "A",
                Video_Internet = "A"
            };

        }
        public UserProfileMobileFormModel UserProfileMobileFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileMobileFormModel model = new ViewModels.UserProfileMobileFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            User user = _userRepository.GetById(userProfile.UserId);
            int countryId = user.Operator.CountryId.Value;
            var profileMatch = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId).ToList();
            if (profileMatch == null || profileMatch.Count() == 0)
            {
                countryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }

            //ContractType
            IList<SelectListItem> contractTypeList = new List<SelectListItem>();
            var contractTypeProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Mobile plan".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var contractTypeProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == contractTypeProfileMatchId).ToList();
            int contractTypecounter = 0;
            foreach (var contractType in contractTypeProfileLabel)
            {
                contractTypeList.Add(new SelectListItem { Text = contractType.ProfileLabel, Value = _answerValues[contractTypecounter] });
                contractTypecounter++;
            }
            //IList<SelectListItem> contractTypeList = new List<SelectListItem>();
            //contractTypeList.Add(new SelectListItem { Value = "A", Text = "Don't Know" });
            //contractTypeList.Add(new SelectListItem { Value = "B", Text = "PAYG" });
            //contractTypeList.Add(new SelectListItem { Value = "C", Text = "Contract" });

            //Spend
            IList<SelectListItem> spendList = new List<SelectListItem>();
            var spendProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Average Monthly Spend".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var spendProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == spendProfileMatchId).ToList();
            int spendcounter = 0;
            foreach (var contractType in spendProfileLabel)
            {
                spendList.Add(new SelectListItem { Text = contractType.ProfileLabel, Value = _answerValues[spendcounter] });
                spendcounter++;
            }
            //IList<SelectListItem> spendList = new List<SelectListItem>();
            //spendList.Add(new SelectListItem { Value = "A", Text = "Don't Know" });
            //spendList.Add(new SelectListItem { Value = "B", Text = "0-9" });
            //spendList.Add(new SelectListItem { Value = "C", Text = "10-19" });
            //spendList.Add(new SelectListItem { Value = "D", Text = "20-29" });
            //spendList.Add(new SelectListItem { Value = "E", Text = "30-39" });
            //spendList.Add(new SelectListItem { Value = "F", Text = "40-49" });
            //spendList.Add(new SelectListItem { Value = "G", Text = "50+" });

            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileMobile = userProfile.UserProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(userProfileMobile.Spend_Mobile) && String.IsNullOrEmpty(userProfileMobile.ContractType_Mobile))
                    {
                        return new UserProfileMobileFormModel
                        {
                            Id = userProfileMobile.Id,
                            UserProfileId = id,
                            ContractTypeList = contractTypeList,
                            SpendList = spendList
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileMobileFormModel>(userProfileMobile);
                        model.ContractTypeList = contractTypeList;
                        model.SpendList = spendList;
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileMobileFormModel
                {
                    UserProfileId = id,
                    ContractTypeList = contractTypeList,
                    SpendList = spendList
                };
            }
            return new UserProfileMobileFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                ContractTypeList = contractTypeList,
                SpendList = spendList
            };

        }
        public bool checkUserProfilePress(UserProfilePreference userProfilePress)
        {
            if (string.IsNullOrEmpty(userProfilePress.FreeNewpapers_Press) && string.IsNullOrEmpty(userProfilePress.Local_Press) && string.IsNullOrEmpty(userProfilePress.Magazines_Press)
                && string.IsNullOrEmpty(userProfilePress.National_Press)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfilePressFormModel UserProfilePressFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfilePressFormModel model = new ViewModels.UserProfilePressFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);


            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfilePress = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfilePress(userProfilePress);
                    if (status == false)
                    {
                        return new UserProfilePressFormModel
                        {
                            Id = userProfilePress.Id,
                            UserProfileId = id,
                            FreeNewpapers_Press = "A",
                            Local_Press = "A",
                            Magazines_Press = "A",
                            National_Press = "A"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfilePressFormModel>(userProfilePress);
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfilePressFormModel
                {
                    UserProfileId = id,
                    FreeNewpapers_Press = "A",
                    Local_Press = "A",
                    Magazines_Press = "A",
                    National_Press = "A"
                };
            }
            return new UserProfilePressFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                FreeNewpapers_Press = "A",
                Local_Press = "A",
                Magazines_Press = "A",
                National_Press = "A"
            };
        }
        public bool checkUserProfileProductsService(UserProfilePreference userProfileProductsServices)
        {
            if (string.IsNullOrEmpty(userProfileProductsServices.AlcoholicDrinks_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.AppliancesOtherHouseholdDurables_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.CommunicationsInternet_ProductsService)
                && string.IsNullOrEmpty(userProfileProductsServices.DIYGardening_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.ElectronicsOtherPersonalItems_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.FinancialServices_ProductsService)
                && string.IsNullOrEmpty(userProfileProductsServices.Food_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.HolidaysTravel_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.Householdproducts_ProductsService)
                && string.IsNullOrEmpty(userProfileProductsServices.Motoring_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.NonAlcoholicDrinks_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.PetsPetFood_ProductsService)
                && string.IsNullOrEmpty(userProfileProductsServices.PharmaceuticalChemistsProducts_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.ShoppingRetailClothing_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.SportsLeisure_ProductsService)
                && string.IsNullOrEmpty(userProfileProductsServices.SweetSaltySnacks_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.TobaccoProducts_ProductsService) && string.IsNullOrEmpty(userProfileProductsServices.ToiletriesCosmetics_ProductsService)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileProductsServiceFormModel UserProfileProductsServiceFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileProductsServiceFormModel model = new ViewModels.UserProfileProductsServiceFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);


            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null &&
                    userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileProductsServices =
                        userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileProductsService(userProfileProductsServices);
                    if (status == false)
                    {
                        return new UserProfileProductsServiceFormModel
                        {
                            Id = userProfileProductsServices.Id,
                            UserProfileId = id,
                            AlcoholicDrinks_ProductsService = "A",
                            AppliancesOtherHouseholdDurables_ProductsService = "A",
                            CommunicationsInternet_ProductsService = "A",
                            DIYGardening_ProductsService = "A",
                            ElectronicsOtherPersonalItems_ProductsService = "A",
                            FinancialServices_ProductsService = "A",
                            Food_ProductsService = "A",
                            HolidaysTravel_ProductsService = "A",
                            Householdproducts_ProductsService = "A",
                            Motoring_ProductsService = "A",
                            NonAlcoholicDrinks_ProductsService = "A",
                            PetsPetFood_ProductsService = "A",
                            PharmaceuticalChemistsProducts_ProductsService = "A",
                            ShoppingRetailClothing_ProductsService = "A",
                            SportsLeisure_ProductsService = "A",
                            SweetSaltySnacks_ProductsService = "A",
                            TobaccoProducts_ProductsService = "A",
                            ToiletriesCosmetics_ProductsService = "A"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileProductsServiceFormModel>(
                               userProfileProductsServices);
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileProductsServiceFormModel
                {
                    UserProfileId = id,
                    AlcoholicDrinks_ProductsService = "A",
                    AppliancesOtherHouseholdDurables_ProductsService = "A",
                    CommunicationsInternet_ProductsService = "A",
                    DIYGardening_ProductsService = "A",
                    ElectronicsOtherPersonalItems_ProductsService = "A",
                    FinancialServices_ProductsService = "A",
                    Food_ProductsService = "A",
                    HolidaysTravel_ProductsService = "A",
                    Householdproducts_ProductsService = "A",
                    Motoring_ProductsService = "A",
                    NonAlcoholicDrinks_ProductsService = "A",
                    PetsPetFood_ProductsService = "A",
                    PharmaceuticalChemistsProducts_ProductsService = "A",
                    ShoppingRetailClothing_ProductsService = "A",
                    SportsLeisure_ProductsService = "A",
                    SweetSaltySnacks_ProductsService = "A",
                    TobaccoProducts_ProductsService = "A",
                    ToiletriesCosmetics_ProductsService = "A"
                };
            }
            return new UserProfileProductsServiceFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                AlcoholicDrinks_ProductsService = "A",
                AppliancesOtherHouseholdDurables_ProductsService = "A",
                CommunicationsInternet_ProductsService = "A",
                DIYGardening_ProductsService = "A",
                ElectronicsOtherPersonalItems_ProductsService = "A",
                FinancialServices_ProductsService = "A",
                Food_ProductsService = "A",
                HolidaysTravel_ProductsService = "A",
                Householdproducts_ProductsService = "A",
                Motoring_ProductsService = "A",
                NonAlcoholicDrinks_ProductsService = "A",
                PetsPetFood_ProductsService = "A",
                PharmaceuticalChemistsProducts_ProductsService = "A",
                ShoppingRetailClothing_ProductsService = "A",
                SportsLeisure_ProductsService = "A",
                SweetSaltySnacks_ProductsService = "A",
                TobaccoProducts_ProductsService = "A",
                ToiletriesCosmetics_ProductsService = "A"
            };
        }
        public bool checkUserProfileRadio(UserProfilePreference userProfileRadio)
        {
            if (string.IsNullOrEmpty(userProfileRadio.Local_Radio) && string.IsNullOrEmpty(userProfileRadio.Music_Radio) && string.IsNullOrEmpty(userProfileRadio.National_Radio)
                && string.IsNullOrEmpty(userProfileRadio.Sport_Radio) && string.IsNullOrEmpty(userProfileRadio.Talk_Radio)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileRadioFormModel UserProfileRadioFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileRadioFormModel model = new ViewModels.UserProfileRadioFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);


            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileRadio = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileRadio(userProfileRadio);
                    if (status == false)
                    {
                        return new UserProfileRadioFormModel
                        {
                            Id = userProfileRadio.Id,
                            UserProfileId = id,
                            Local_Radio = "A",
                            Music_Radio = "A",
                            National_Radio = "A",
                            Sport_Radio = "A",
                            Talk_Radio = "A"
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileRadioFormModel>(userProfileRadio);
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileRadioFormModel
                {
                    UserProfileId = id,
                    Local_Radio = "A",
                    Music_Radio = "A",
                    National_Radio = "A",
                    Sport_Radio = "A",
                    Talk_Radio = "A"
                };

            }
            return new UserProfileRadioFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                Local_Radio = "A",
                Music_Radio = "A",
                National_Radio = "A",
                Sport_Radio = "A",
                Talk_Radio = "A"
            };
        }
        public bool checkUserProfileTv(UserProfilePreference userProfileTv)
        {
            if (string.IsNullOrEmpty(userProfileTv.Cable_TV) && string.IsNullOrEmpty(userProfileTv.Internet_TV) && string.IsNullOrEmpty(userProfileTv.Satallite_TV)
                && string.IsNullOrEmpty(userProfileTv.Terrestrial_TV)
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public UserProfileTvFormModel UserProfileTvFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);
            UserProfileTvFormModel model = new ViewModels.UserProfileTvFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);


            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileTv = userProfile.UserProfilePreferences.FirstOrDefault();
                    bool status = checkUserProfileTv(userProfileTv);
                    if (status == false)
                    {
                        return new UserProfileTvFormModel
                        {
                            Id = userProfileTv.Id,
                            UserProfileId = userProfile.UserProfileId,
                            Cable_TV = "A",
                            Internet_TV = "A",
                            Satallite_TV = "A",
                            Terrestrial_TV = "A"
                        };
                    }
                    else
                    {
                        model = Mapper.Map<UserProfilePreference, UserProfileTvFormModel>(userProfileTv);
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileTvFormModel
                {
                    UserProfileId = 0,
                    Cable_TV = "A",
                    Internet_TV = "A",
                    Satallite_TV = "A",
                    Terrestrial_TV = "A"
                };
            }
            return new UserProfileTvFormModel
            {
                UserProfileId = userProfile.UserProfileId,
                Cable_TV = "A",
                Internet_TV = "A",
                Satallite_TV = "A",
                Terrestrial_TV = "A"
            };
        }
        public UserProfileTimeSettingFormModel UserProfileTimeSettingFormModel(int id)
        {
            var userid = Convert.ToInt32(Session["userId"]);

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

                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileTimeSettingFormModel
                { UserProfileId = id, AvailableTimes = GetTimes() };
            }
            return new UserProfileTimeSettingFormModel
            { UserProfileId = userProfile.UserProfileId, AvailableTimes = GetTimes() };
        }

        public IEnumerable<UserProfileAdvertsReceivedFromModel> UserProfileAdvertsReceivedFromModel(int id)
        {
            IEnumerable<UserProfileAdvertsReceivedFromModel> listPaged = GetPagedAdverts(id);
            return listPaged;
        }
        protected IEnumerable<UserProfileAdvertsReceivedFromModel> GetPagedAdverts(int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand
            IEnumerable<UserProfileAdvertsReceivedFromModel> listUnpaged = GetReceivedAdvertsFromDatabase();


            return listUnpaged;
        }
        protected IEnumerable<UserProfileAdvertsReceivedFromModel> GetReceivedAdvertsFromDatabase()
        {
            var userid = Convert.ToInt32(Session["userId"]);
            User user = _userRepository.GetById(userid);
            UserProfile userProfile = user.UserProfiles.FirstOrDefault();

            IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
                userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.DateTimePlayed);

            IEnumerable<UserProfileAdvertsReceivedFromModel> models =
                Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
                    userProfileAdvertsReceiveds);

            return models;
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
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        [Route("Save")]
        [HttpPost]
        public ActionResult Save(UserProfileFormModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    var userid = Convert.ToInt32(Session["userId"]);

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

                    command.UserId = userid;

                    if (ModelState.IsValid)
                    {
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

                return Json(ex.InnerException.Message);
            }


        }
        [Route("SaveAdverts")]
        [HttpPost]
        public ActionResult SaveAdverts(UserProfileAdvertFormModel model)
        {
            if (ModelState.IsValid)
            {
                var userid = Convert.ToInt32(Session["userId"]);

                CreateOrUpdateUserProfileAdvertCommand command =
                    Mapper.Map<UserProfileAdvertFormModel, CreateOrUpdateUserProfileAdvertCommand>(model);


                if (ModelState.IsValid)
                {
                    User user = _userRepository.GetById(userid);
                    command.CountryId = user.Operator.CountryId;
                    ICommandResult result = _commandBus.Submit(command);

                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();

                    //obj.UpdateAdTypes(model, user, SQLServerEntities);                   
                    var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                            var userData = SQLServerEntities.Users.Where(s => s.AdtoneServerUserId == user.UserId).FirstOrDefault();
                            var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(SQLServerEntities, model.UserProfileId);
                            if(userData != null && externalServerUserProfileId != 0)
                            {
                                model.UserProfileId = externalServerUserProfileId;
                                obj.UpdateAdTypes(model, userData, SQLServerEntities);
                                PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                            }
                            
                        }
                    }

                    if (result.Success)
                    {
                        return Json("success");
                    }
                }
            }

            return Json("fail");
        }

        [Route("SaveSkizaProfile")]
        [HttpPost]
        public ActionResult SaveSkizaProfile(SkizaProfileFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                User user = _userRepository.GetById(efmvcUser.UserId);
                if (ModelState.IsValid)
                {
                    //Comment 08-08-2019
                    //var countryId = user.Operator.CountryId;

                    #region UserProfilePreference
                    EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    var userProfileData = SQLServerEntities.UserProfilePreference.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
                    if (userProfileData != null)
                    {
                        userProfileData.Hustlers_AdType = model.Hustlers_AdType;
                        userProfileData.Youth_AdType = model.Youth_AdType;
                        userProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                        userProfileData.Mass_AdType = model.Mass_AdType;
                        SQLServerEntities.SaveChanges();
                    }
                    else
                    {
                        userProfileData.Hustlers_AdType = model.Hustlers_AdType;
                        userProfileData.Youth_AdType = model.Youth_AdType;
                        userProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                        userProfileData.Mass_AdType = model.Mass_AdType;
                        SQLServerEntities.UserProfilePreference.Add(userProfileData);
                        SQLServerEntities.SaveChanges();
                    }
                    #endregion

                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //obj.UpdateSkizaProfile(model, user, SQLServerEntities);
                   
                    var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();
                        foreach (var item in ConnString)
                        {
                            SQLServerEntities = new EFMVCDataContex(item);
                            var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(SQLServerEntities, model.UserProfileId);
                            #region External User Profile Preference
                            var externalUserProfileData = SQLServerEntities.UserProfilePreference.Where(s => s.UserProfileId == externalServerUserProfileId).FirstOrDefault();
                            if (externalUserProfileData != null)
                            {
                                externalUserProfileData.Hustlers_AdType = model.Hustlers_AdType;
                                externalUserProfileData.Youth_AdType = model.Youth_AdType;
                                externalUserProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                externalUserProfileData.Mass_AdType = model.Mass_AdType;
                                SQLServerEntities.SaveChanges();
                            }
                            else
                            {
                                externalUserProfileData.Hustlers_AdType = model.Hustlers_AdType;
                                externalUserProfileData.Youth_AdType = model.Youth_AdType;
                                externalUserProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                                externalUserProfileData.Mass_AdType = model.Mass_AdType;
                                SQLServerEntities.UserProfilePreference.Add(externalUserProfileData);
                                SQLServerEntities.SaveChanges();
                            }
                            #endregion
                            var userData = SQLServerEntities.Users.Where(s => s.AdtoneServerUserId == user.UserId).FirstOrDefault();
                            if(userData != null && externalServerUserProfileId != 0)
                            {
                                model.UserProfileId = externalServerUserProfileId;
                                obj.UpdateSkizaProfile(model, userData, SQLServerEntities);
                                PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                            }                           
                        }
                    }

                    return Json("success");
                }
            }

            return Json("fail");
        }

        [Route("SaveAttitude")]
        [HttpPost]
        public ActionResult SaveAttitude(UserProfileAttitudeFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileAttitudeCommand command =
                    Mapper.Map<UserProfileAttitudeFormModel, CreateOrUpdateUserProfileAttitudeCommand>(model);

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        return Json("success");
                    }
                }
            }

            return Json("fail");
        }
        [Route("SaveCinema")]
        [HttpPost]
        public ActionResult SaveCinema(UserProfileCinemaFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileCinemaCommand command =
                    Mapper.Map<UserProfileCinemaFormModel, CreateOrUpdateUserProfileCinemaCommand>(model);

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
        [Route("SaveInternet")]
        [HttpPost]
        public ActionResult SaveInternet(UserProfileInternetFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileInternetCommand command =
                    Mapper.Map<UserProfileInternetFormModel, CreateOrUpdateUserProfileInternetCommand>(model);

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
        [Route("SavePress")]
        [HttpPost]
        public ActionResult SavePress(UserProfilePressFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfilePressCommand command =
                    Mapper.Map<UserProfilePressFormModel, CreateOrUpdateUserProfilePressCommand>(model);

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
        [Route("SaveProductsServices")]
        [HttpPost]
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
        [Route("SaveRadio")]
        [HttpPost]
        public ActionResult SaveRadio(UserProfileRadioFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileRadioCommand command =
                    Mapper.Map<UserProfileRadioFormModel, CreateOrUpdateUserProfileRadioCommand>(model);

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
        [Route("SaveTv")]
        [HttpPost]
        public ActionResult SaveTv(UserProfileTvFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileTvCommand command =
                    Mapper.Map<UserProfileTvFormModel, CreateOrUpdateUserProfileTvCommand>(model);

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
        [Route("SaveMobile")]
        [HttpPost]
        public ActionResult SaveMobile(UserProfileMobileFormModel model)
        {
            if (ModelState.IsValid)
            {
                var userid = Convert.ToInt32(Session["userId"]);
                CreateOrUpdateUserProfileMobileCommand command =
                    Mapper.Map<UserProfileMobileFormModel, CreateOrUpdateUserProfileMobileCommand>(model);

                if (ModelState.IsValid)
                {
                    User user = _userRepository.GetById(userid);
                    command.CountryId = user.Operator.CountryId;

                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //obj.UpdateMobileData(model, user, SQLServerEntities);
                       
                        var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                using (SQLServerEntities)
                                {
                                    var userData = SQLServerEntities.Users.Where(s => s.AdtoneServerUserId == user.UserId).FirstOrDefault();
                                    var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(SQLServerEntities, model.UserProfileId);
                                    if(userData != null && externalServerUserProfileId != 0)
                                    {
                                        model.UserProfileId = externalServerUserProfileId;
                                        obj.UpdateMobileData(model, userData, SQLServerEntities);
                                        PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                                    }
                                    
                                }
                            }
                        }

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
        [Route("SaveTimeSettings")]
        [HttpPost]
        public ActionResult SaveTimeSettings(UserProfileTimeSettingFormModel model)
        {
            if (ModelState.IsValid)
            {
                model.OperatorId = _profileRepository.GetById(model.UserProfileId).User.OperatorId;
                CreateOrUpdateUserProfileTimeSettingCommand command =
                    Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

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
        private Dictionary<string, IList<SelectListItem>> GetSelectLists()
        {
            var lists = new Dictionary<string, IList<SelectListItem>>();

            IList<SelectListItem> ageBracketList = new List<SelectListItem>();
            ageBracketList.Add(new SelectListItem { Text = "Under 18", Value = "0" });
            ageBracketList.Add(new SelectListItem { Text = "18-24", Value = "1" });
            ageBracketList.Add(new SelectListItem { Text = "25-34", Value = "2" });
            ageBracketList.Add(new SelectListItem { Text = "35-44", Value = "3" });
            ageBracketList.Add(new SelectListItem { Text = "45-54", Value = "4" });
            ageBracketList.Add(new SelectListItem { Text = "55-64", Value = "5" });
            ageBracketList.Add(new SelectListItem { Text = "65+", Value = "6" });
            ageBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "7" });

            IList<SelectListItem> genderList = new List<SelectListItem>();
            genderList.Add(new SelectListItem { Text = "Male", Value = "0" });
            genderList.Add(new SelectListItem { Text = "Female", Value = "1" });
            genderList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "2" });

            IList<SelectListItem> incomeBracketList = new List<SelectListItem>();
            incomeBracketList.Add(new SelectListItem { Text = "£0 to £14,999", Value = "0" });
            incomeBracketList.Add(new SelectListItem { Text = "£15,000 to £24,999", Value = "1" });
            incomeBracketList.Add(new SelectListItem { Text = "£25,000 to £39,999", Value = "2" });
            incomeBracketList.Add(new SelectListItem { Text = "£40,000 to £74,999", Value = "3" });
            incomeBracketList.Add(new SelectListItem { Text = "£75,000 to £99,999", Value = "4" });
            incomeBracketList.Add(new SelectListItem { Text = "£100,000+", Value = "5" });
            incomeBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "6" });

            IList<SelectListItem> workingStatusList = new List<SelectListItem>();
            workingStatusList.Add(new SelectListItem { Text = "Employed", Value = "0" });
            workingStatusList.Add(new SelectListItem { Text = "Self-Employed", Value = "1" });
            workingStatusList.Add(new SelectListItem { Text = "Housewife/Househusband", Value = "2" });
            workingStatusList.Add(new SelectListItem { Text = "Retired", Value = "3" });
            workingStatusList.Add(new SelectListItem { Text = "Unpaid Carer", Value = "4" });
            workingStatusList.Add(new SelectListItem { Text = "Full or Part-time Education", Value = "5" });
            workingStatusList.Add(new SelectListItem { Text = "Not Working", Value = "6" });
            workingStatusList.Add(new SelectListItem { Text = "None of these", Value = "7" });
            workingStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "8" });

            IList<SelectListItem> relationshipStatusList = new List<SelectListItem>();
            relationshipStatusList.Add(new SelectListItem { Text = "Divorced", Value = "0" });
            relationshipStatusList.Add(new SelectListItem { Text = "Living with another", Value = "1" });
            relationshipStatusList.Add(new SelectListItem { Text = "Married", Value = "2" });
            relationshipStatusList.Add(new SelectListItem { Text = "Separated", Value = "3" });
            relationshipStatusList.Add(new SelectListItem { Text = "Single", Value = "4" });
            relationshipStatusList.Add(new SelectListItem { Text = "Widowed", Value = "5" });
            relationshipStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "6" });

            IList<SelectListItem> educationList = new List<SelectListItem>();
            educationList.Add(new SelectListItem { Text = "Secondary", Value = "0" });
            educationList.Add(new SelectListItem { Text = "College", Value = "1" });
            educationList.Add(new SelectListItem { Text = "University", Value = "2" });
            educationList.Add(new SelectListItem { Text = "Post Graduate", Value = "3" });
            educationList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "4" });

            IList<SelectListItem> householdStatusList = new List<SelectListItem>();
            householdStatusList.Add(new SelectListItem { Text = "Rent", Value = "0" });
            householdStatusList.Add(new SelectListItem { Text = "Owner", Value = "1" });
            householdStatusList.Add(new SelectListItem { Text = "Live with someone", Value = "2" });
            householdStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "3" });

            IList<SelectListItem> locationList = new List<SelectListItem>();
            locationList.Add(new SelectListItem { Text = "London", Value = "0" });
            locationList.Add(new SelectListItem { Text = "South East (excl. London)", Value = "1" });
            locationList.Add(new SelectListItem { Text = "South West", Value = "2" });
            locationList.Add(new SelectListItem { Text = "East Anglia", Value = "3" });
            locationList.Add(new SelectListItem { Text = "Midlands", Value = "4" });
            locationList.Add(new SelectListItem { Text = "Wales", Value = "5" });
            locationList.Add(new SelectListItem { Text = "North West", Value = "6" });
            locationList.Add(new SelectListItem { Text = "North East", Value = "7" });
            locationList.Add(new SelectListItem { Text = "Scotland", Value = "8" });
            locationList.Add(new SelectListItem { Text = "Northern Ireland", Value = "9" });

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
    }
}