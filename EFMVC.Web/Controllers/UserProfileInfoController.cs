using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.TempProfileFix;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class UserProfileInfoController : Controller
    {
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IProfileMatchLabelRepository _profileMatchLabelRepository;
        private readonly ICommandBus _commandBus;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileInfoController(ICommandBus commandBus, IUserRepository userRepository, IProfileRepository profileRepository,
                                        ICountryRepository countryRepository, ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository,
                                        IProfileMatchInformationRepository profileMatchInformationRepository, IProfileMatchLabelRepository profileMatchLabelRepository,
                                        IUnitOfWork unitOfWork)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _countryRepository = countryRepository;
            _campaignProfilePreferenceRepository = campaignProfilePreferenceRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _profileMatchLabelRepository = profileMatchLabelRepository;
            _unitOfWork = unitOfWork;
        }

        private readonly string[] _answerValues = new[]
                                                      {
                                                          "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                          "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                                      };

        // GET: UserProfileInfo
        public ActionResult Index(int id)
        {
            return View();
        }

        //Comment 19-03-2019
        //public ActionResult UpdateUserProfileInfo(int id)
        //{
        //    ViewBag.UserId = id;
        //    UserProfileResult _userProfileResult = new UserProfileResult();
        //    if (id != 0)
        //    {
        //        var userid = Convert.ToInt32(id);

        //        User user = _userRepository.GetById(userid);

        //        IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == userid);
        //        IEnumerable<UserProfileFormModel> userProfileFormModels =
        //            Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileFormModel>>(userProfiles);
        //        UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

        //        Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();
        //        var countryId = user.Operator.CountryId;
        //        if (userFormModel.UserProfile != null)
        //        {
        //            UserProfile userProfile = _profileRepository.GetById(userFormModel.UserProfile.UserProfileId);
        //            UserProfileAdvertFormModel userProfileAdvert = UserProfileAdvertFormModel(userFormModel.UserProfile.UserProfileId, userid);
        //            ProfileMatchOption(userProfileAdvert, countryId);
        //            _userProfileResult.UserProfileAdvertFormModel = userProfileAdvert;

        //            SkizaProfileFormModel skizaProfile = UserProfileSkizaModel(userFormModel.UserProfile.UserProfileId, userid);
        //            _userProfileResult.SkizaProfileFormModel = skizaProfile;

        //            UserProfileMobileFormModel userProfileMobile = UserProfileMobileFormModel(userFormModel.UserProfile.UserProfileId, userid);
        //            MobileMatchOption(userProfileMobile, countryId);
        //            _userProfileResult.UserProfileMobileFormModel = userProfileMobile;

        //            UserProfileTimeSettingFormModel userProfileTimeSetting = UserProfileTimeSettingFormModel(userFormModel.UserProfile.UserProfileId, userid);
        //            _userProfileResult.UserProfileTimeSettingFormModel = userProfileTimeSetting;

        //            UserProfileDemographicFormModel userProfileDemographic = UserProfileDemographicFormModel(userFormModel.UserProfile.UserProfileId, userid);
        //            DemographicMatchOption(userProfileDemographic, countryId);
        //            _userProfileResult.UserProfileDemographicFormModel = userProfileDemographic;

        //            UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);

        //            model.LocationList = selectLists["locationList"];
        //            model.GenderList = selectLists["genderList"];
        //            model.IncomeBracketList = selectLists["incomeBracketList"];
        //            model.WorkingStatusList = selectLists["workingStatusList"];
        //            model.RelationshipStatusList = selectLists["relationshipStatusList"];
        //            model.EducationList = selectLists["educationList"];
        //            model.HouseholdStatusList = selectLists["householdStatusList"];

        //            _userProfileResult.userProfileFormModels = userProfileFormModels;
        //            _userProfileResult.userProfileModel = model;
        //            return View(_userProfileResult);
        //        }
        //        else
        //        {
        //            UserProfile userProfile = new UserProfile();
        //            UserProfileAdvertFormModel userProfileAdvert = new UserProfileAdvertFormModel();
        //            ProfileMatchOption(userProfileAdvert, countryId);
        //            _userProfileResult.UserProfileAdvertFormModel = UserProfileAdvertFormModel(0, userid);
        //            UserProfileMobileFormModel userProfileMobile = UserProfileMobileFormModel(0, userid);
        //            MobileMatchOption(userProfileMobile, countryId);
        //            _userProfileResult.UserProfileMobileFormModel = userProfileMobile;
        //            UserProfileTimeSettingFormModel userProfileTimeSetting = UserProfileTimeSettingFormModel(0, userid);
        //            _userProfileResult.UserProfileTimeSettingFormModel = userProfileTimeSetting;
        //            UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);

        //             model.LocationList = selectLists["locationList"];
        //            model.GenderList = selectLists["genderList"];
        //            model.IncomeBracketList = selectLists["incomeBracketList"];
        //            model.WorkingStatusList = selectLists["workingStatusList"];
        //            model.RelationshipStatusList = selectLists["relationshipStatusList"];
        //            model.EducationList = selectLists["educationList"];
        //            model.HouseholdStatusList = selectLists["householdStatusList"];

        //            _userProfileResult.userProfileFormModels = userProfileFormModels;
        //            _userProfileResult.userProfileModel = model;
        //            return View(_userProfileResult);
        //        }
        //    }
        //    else
        //    {
        //        //return RedirectToAction("Campaign", "Dashboard", new { area = "UserAdmin" });
        //        return RedirectToAction("Index", "Dashboard");
        //    }
        //}

        //Add 19-03-2019
        public ActionResult UpdateUserProfileInfo(int id, int? campaignId)
        {
            ViewBag.UserId = id;
            UserProfileAdvertiserResult _userProfileAdvertiserResult = new UserProfileAdvertiserResult();
            if (id != 0)
            {
                var userid = Convert.ToInt32(id);

                User user = _userRepository.GetById(userid);

                IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == userid);
                IEnumerable<UserProfileFormModel> userProfileFormModels =
                    Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileFormModel>>(userProfiles);
                UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);

                Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();

                int? countryId = 0;
                if (user.Operator == null)
                {
                    countryId = 9;
                }
                else
                {
                    countryId = user.Operator.CountryId;
                }

                if (userFormModel.UserProfile != null)
                {
                    UserProfile userProfile = _profileRepository.GetById(userFormModel.UserProfile.UserProfileId);
                    UserProfileAdvertAdvertiserFormModel UserProfileAdvertAdvertiser = new UserProfileAdvertAdvertiserFormModel();
                    UserProfileSkizaProfileAdvertiserFormModel UserProfileSkizaProfileAdvertiser = new UserProfileSkizaProfileAdvertiserFormModel();
                    UserProfileMobileAdvertiserFormModel UserProfileMobileAdvertiser = new UserProfileMobileAdvertiserFormModel();
                    UserProfileTimesettingAdvertiserFormModel UserProfileTimesettingAdvertiser = new UserProfileTimesettingAdvertiserFormModel();
                    UserProfileDemographicAdvertiserFormModel UserProfileDemographicAdvertiser = new UserProfileDemographicAdvertiserFormModel();

                    ProfileMatchValue profileMatchValue = new ProfileMatchValue();

                    if (userProfile != null)
                    {
                        bool isQuestionnaire = false;
                        if (user.OperatorId == (int)OperatorTableId.Safaricom)
                        {
                            isQuestionnaire = true;
                        }
                        ViewBag.isQuestionnaire = isQuestionnaire;

                        if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                        {
                            UserProfilePreference userProfileAdvert = userProfile.UserProfilePreferences.FirstOrDefault();

                            //Ad Profile
                            UserProfileAdvertAdvertiser = Mapper.Map<UserProfilePreference, UserProfileAdvertAdvertiserFormModel>(userProfileAdvert);
                            _userProfileAdvertiserResult.UserProfileAdvertAdvertiserFormModel = UserProfileAdvertAdvertiser;

                            //Skiza Profile
                            UserProfileSkizaProfileAdvertiser = Mapper.Map<UserProfilePreference, UserProfileSkizaProfileAdvertiserFormModel>(userProfileAdvert);
                            //_userProfileAdvertiserResult.UserProfileSkizaProfileAdvertiserFormModel = UserProfileSkizaProfileAdvertiser;

                            _userProfileAdvertiserResult.UserProfileSkizaProfileAdvertiserFormModel = GetSkizaData(campaignId.Value, UserProfileSkizaProfileAdvertiser);

                           //AirTime Profile
                           UserProfileMobileAdvertiser = Mapper.Map<UserProfilePreference, UserProfileMobileAdvertiserFormModel>(userProfileAdvert);
                            int contractype = profileMatchValue.GetProfileMatchValue(UserProfileMobileAdvertiser.ContractType_Mobile);
                            var contractTypeProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Mobile plan".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                            if (contractTypeProfileMatchId == 0)
                            {
                                UserProfileMobileAdvertiser.ContractType_Mobile = "-";
                            }
                            else
                            {
                                var contractTypeProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == contractTypeProfileMatchId).Skip(contractype).Take(1).FirstOrDefault();
                                UserProfileMobileAdvertiser.ContractType_Mobile = contractTypeProfileLabel.ProfileLabel;
                            }

                            int mobileplan = profileMatchValue.GetProfileMatchValue(UserProfileMobileAdvertiser.Spend_Mobile);
                            var spendProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Average Monthly Spend".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                            if (spendProfileMatchId == 0)
                            {
                                UserProfileMobileAdvertiser.Spend_Mobile = "-";
                            }
                            else
                            {
                                var spendProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == spendProfileMatchId);
                                spendProfileLabel = spendProfileLabel.Skip(mobileplan);
                                var test = spendProfileLabel.Take(1).FirstOrDefault();
                                //UserProfileMobileAdvertiser.Spend_Mobile = spendProfileLabel.ProfileLabel;
                                UserProfileMobileAdvertiser.Spend_Mobile = test.ProfileLabel;
                            }

                            _userProfileAdvertiserResult.UserProfileMobileAdvertiserFormModel = UserProfileMobileAdvertiser;

                            //Demographic Profile
                            UserProfileDemographicAdvertiser = Mapper.Map<UserProfilePreference, UserProfileDemographicAdvertiserFormModel>(userProfileAdvert);
                            UserProfileDemographicAdvertiser.Location_Demographics = GetLocationData(campaignId.Value, UserProfileDemographicAdvertiser);
                            
                                UserProfileDemographicAdvertiser.Location_Demographics = UserProfileDemographicAdvertiser.Location_Demographics == null ? "-" : UserProfileDemographicAdvertiser.Location_Demographics;
                            if (UserProfileDemographicAdvertiser.Location_Demographics != "-")
                            {
                                int location = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.Location_Demographics);
                                var locationProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Location".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (locationProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.Location_Demographics = "-";
                                }
                                else
                                {
                                    var locationProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == locationProfileMatchId).Skip(location).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.Location_Demographics = locationProfileLabel.ProfileLabel;
                                }
                            }


                            UserProfileDemographicAdvertiser.Age_Demographics = UserProfileDemographicAdvertiser.Age_Demographics == null ? "-" : UserProfileDemographicAdvertiser.Age_Demographics;
                            int age = 0;
                            if (userProfile.DOB != null)
                            {
                                DateTime dob = (DateTime)userProfile.DOB;
                                DateTime now = DateTime.Today;
                                age = now.Year - dob.Year;
                            }
                            age = GetAgeData(campaignId.Value, age);
                            UserProfileDemographicAdvertiser.Age_Demographics = age.ToString();
                            UserProfileDemographicAdvertiser.Age = age.ToString();
                            // Commented lines out below as a quivk fix due to Age_Demographics not even stored anywhere for a User
                            //if (UserProfileDemographicAdvertiser.Age_Demographics == "-")
                            //{
                            //    int age = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.Age_Demographics);
                            //    var ageProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Age".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                            //    if (ageProfileMatchId == 0)
                            //    {
                            //        UserProfileDemographicAdvertiser.Age_Demographics = "-";
                            //    }
                            //    else
                            //    {
                            //        var ageProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == ageProfileMatchId).Skip(age).Take(1).FirstOrDefault();
                            //        UserProfileDemographicAdvertiser.Age_Demographics = ageProfileLabel?.ProfileLabel ?? "-";
                            //    }

                            //    //Comment 04-06-2019
                            //    //int age = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.Age_Demographics);
                            //    //var ageProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Age".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                            //    //if (ageProfileMatchId == 0)
                            //    //{
                            //    //    UserProfileDemographicAdvertiser.Age_Demographics = "-";
                            //    //}
                            //    //else
                            //    //{
                            //    //    var ageProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == ageProfileMatchId).Skip(age).Take(1).FirstOrDefault();
                            //    //    UserProfileDemographicAdvertiser.Age_Demographics = ageProfileLabel.ProfileLabel;
                            //    //}
                            //}
                            
                            UserProfileDemographicAdvertiser.Gender_Demographics = GetGenderData(campaignId.Value, UserProfileDemographicAdvertiser);

                            //UserProfileDemographicAdvertiser.Gender_Demographics = UserProfileDemographicAdvertiser.Gender_Demographics == null ? "-" : UserProfileDemographicAdvertiser.Gender_Demographics;
                            //if (UserProfileDemographicAdvertiser.Gender_Demographics != "-")
                            //{
                            int gender = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.Gender_Demographics);
                            var genderProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Gender".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                            if (genderProfileMatchId == 0)
                            {
                                UserProfileDemographicAdvertiser.Gender_Demographics = "-";
                            }
                            else
                            {
                                var genderProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == genderProfileMatchId).Skip(gender).Take(1).FirstOrDefault();
                                UserProfileDemographicAdvertiser.Gender_Demographics = genderProfileLabel.ProfileLabel;
                            }
                            //}

                            UserProfileDemographicAdvertiser.HouseholdStatus_Demographics = UserProfileDemographicAdvertiser.HouseholdStatus_Demographics == null ? "-" : UserProfileDemographicAdvertiser.HouseholdStatus_Demographics;
                            if (UserProfileDemographicAdvertiser.HouseholdStatus_Demographics != "-")
                            {
                                int household = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.HouseholdStatus_Demographics);
                                var householdProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Household Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (householdProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.HouseholdStatus_Demographics = "-";
                                }
                                else
                                {
                                    var householdProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == householdProfileMatchId).Skip(household).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.HouseholdStatus_Demographics = householdProfileLabel.ProfileLabel;
                                }
                            }

                            UserProfileDemographicAdvertiser.RelationshipStatus_Demographics = UserProfileDemographicAdvertiser.RelationshipStatus_Demographics == null ? "-" : UserProfileDemographicAdvertiser.RelationshipStatus_Demographics;
                            if (UserProfileDemographicAdvertiser.RelationshipStatus_Demographics != "-")
                            {
                                int relationship = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.RelationshipStatus_Demographics);
                                var relationshipProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Relationship Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (relationshipProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.RelationshipStatus_Demographics = "-";
                                }
                                else
                                {
                                    var relationshipProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == relationshipProfileMatchId).Skip(relationship).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.RelationshipStatus_Demographics = relationshipProfileLabel.ProfileLabel;
                                }
                            }

                            UserProfileDemographicAdvertiser.IncomeBracket_Demographics = UserProfileDemographicAdvertiser.IncomeBracket_Demographics == null ? "-" : UserProfileDemographicAdvertiser.IncomeBracket_Demographics;
                            if (UserProfileDemographicAdvertiser.IncomeBracket_Demographics != "-")
                            {
                                int incomebracket = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.IncomeBracket_Demographics);
                                var incomebracketProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("IncomeBracket".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (incomebracketProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.IncomeBracket_Demographics = "-";
                                }
                                else
                                {
                                    var incomebracketProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == incomebracketProfileMatchId).Skip(incomebracket).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.IncomeBracket_Demographics = incomebracketProfileLabel.ProfileLabel;
                                }
                            }

                            UserProfileDemographicAdvertiser.Education_Demographics = UserProfileDemographicAdvertiser.Education_Demographics == null ? "-" : UserProfileDemographicAdvertiser.Education_Demographics;
                            if (UserProfileDemographicAdvertiser.Education_Demographics != "-")
                            {
                                int education = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.Education_Demographics);
                                var educationProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Education".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (educationProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.Education_Demographics = "-";
                                }
                                else
                                {
                                    var educationProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == educationProfileMatchId).Skip(education).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.Education_Demographics = educationProfileLabel.ProfileLabel;
                                }
                            }

                            UserProfileDemographicAdvertiser.WorkingStatus_Demographics = UserProfileDemographicAdvertiser.WorkingStatus_Demographics == null ? "-" : UserProfileDemographicAdvertiser.WorkingStatus_Demographics;
                            if (UserProfileDemographicAdvertiser.WorkingStatus_Demographics != "-")
                            {
                                int working = profileMatchValue.GetProfileMatchValue(UserProfileDemographicAdvertiser.WorkingStatus_Demographics);
                                var workingProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Working Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
                                if (workingProfileMatchId == 0)
                                {
                                    UserProfileDemographicAdvertiser.WorkingStatus_Demographics = "-";
                                }
                                else
                                {
                                    var workingProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == workingProfileMatchId).Skip(working).Take(1).FirstOrDefault();
                                    UserProfileDemographicAdvertiser.WorkingStatus_Demographics = workingProfileLabel.ProfileLabel;
                                }
                            }

                            _userProfileAdvertiserResult.UserProfileDemographicAdvertiserFormModel = UserProfileDemographicAdvertiser;
                        }

                        //TimeBands Profile
                        if (userProfile.UserProfileTimeSettings != null && userProfile.UserProfileTimeSettings.Count != 0)
                        {
                            UserProfileTimeSetting userProfileTimeSettings = userProfile.UserProfileTimeSettings.FirstOrDefault();

                            if (userProfileTimeSettings.Monday != null)
                                UserProfileTimesettingAdvertiser.MondaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Monday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Tuesday != null)
                                UserProfileTimesettingAdvertiser.TuesdaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Tuesday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Wednesday != null)
                                UserProfileTimesettingAdvertiser.WednesdaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Wednesday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Thursday != null)
                                UserProfileTimesettingAdvertiser.ThursdaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Thursday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Friday != null)
                                UserProfileTimesettingAdvertiser.FridaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Friday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Saturday != null)
                                UserProfileTimesettingAdvertiser.SaturdaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Saturday.Split(",".ToCharArray()));

                            if (userProfileTimeSettings.Sunday != null)
                                UserProfileTimesettingAdvertiser.SundaySelectedTimes =
                                    ConvertTimesArrayToList(userProfileTimeSettings.Sunday.Split(",".ToCharArray()));

                            _userProfileAdvertiserResult.UserProfileTimesettingAdvertiserFormModel = UserProfileTimesettingAdvertiser;
                        }
                    }

                    return View(_userProfileAdvertiserResult);
                }
                else
                {
                    UserProfile userProfile = new UserProfile();
                    UserProfileAdvertFormModel userProfileAdvert = new UserProfileAdvertFormModel();
                    _userProfileAdvertiserResult.UserProfileAdvertAdvertiserFormModel = null;
                    _userProfileAdvertiserResult.UserProfileSkizaProfileAdvertiserFormModel = null;
                    _userProfileAdvertiserResult.UserProfileMobileAdvertiserFormModel = null;
                    _userProfileAdvertiserResult.UserProfileDemographicAdvertiserFormModel = null;
                    _userProfileAdvertiserResult.UserProfileTimesettingAdvertiserFormModel = null;
                    return View(_userProfileAdvertiserResult);
                }
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public UserProfileAdvertFormModel UserProfileAdvertFormModel(int id, int userid)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            UserProfileAdvertFormModel model = new ViewModels.UserProfileAdvertFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);

            User user = _userRepository.GetById(userid);
            var countryId = user.Operator.CountryId;

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
                            UserId = userid,
                            AlcoholicDrinks_Advert = "B",
                            Cinema_Advert = "B",
                            CommunicationsInternet_Advert = "B",
                            DIYGardening_Advert = "B",
                            ElectronicsOtherPersonalItems_Advert = "B",
                            Environment_Advert = "B",
                            FinancialServices_Advert = "B",
                            Fitness_Advert = "B",
                            Food_Advert = "B",
                            GoingOut_Advert = "B",
                            HolidaysTravel_Advert = "B",
                            Householdproducts_Advert = "B",
                            Motoring_Advert = "B",
                            Music_Advert = "B",
                            Newspapers_Advert = "B",
                            NonAlcoholicDrinks_Advert = "B",
                            PetsPetFood_Advert = "B",
                            PharmaceuticalChemistsProducts_Advert = "B",
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
                        model.Id = userProfileAdvert.Id;
                        model.UserId = userid;
                        model.UserProfileId = id;
                    }
                    ProfileMatchOption(model, countryId);
                    return model;

                }
                else
                {

                    model = new UserProfileAdvertFormModel
                    {
                        UserId = userid,
                        UserProfileId = id,
                        AlcoholicDrinks_Advert = "B",
                        Cinema_Advert = "B",
                        CommunicationsInternet_Advert = "B",
                        DIYGardening_Advert = "B",
                        ElectronicsOtherPersonalItems_Advert = "B",
                        Environment_Advert = "B",
                        FinancialServices_Advert = "B",
                        Fitness_Advert = "B",
                        Food_Advert = "B",
                        GoingOut_Advert = "B",
                        HolidaysTravel_Advert = "B",
                        Householdproducts_Advert = "B",
                        Motoring_Advert = "B",
                        Music_Advert = "B",
                        Newspapers_Advert = "B",
                        NonAlcoholicDrinks_Advert = "B",
                        PetsPetFood_Advert = "B",
                        PharmaceuticalChemistsProducts_Advert = "B",
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
                    ProfileMatchOption(model, countryId);
                    return model;
                }
            }
            else
            {
                model = new UserProfileAdvertFormModel
                {
                    UserId = userid,
                    UserProfileId = id,
                    AlcoholicDrinks_Advert = "B",
                    Cinema_Advert = "B",
                    CommunicationsInternet_Advert = "B",
                    DIYGardening_Advert = "B",
                    ElectronicsOtherPersonalItems_Advert = "B",
                    Environment_Advert = "B",
                    FinancialServices_Advert = "B",
                    Fitness_Advert = "B",
                    Food_Advert = "B",
                    GoingOut_Advert = "B",
                    HolidaysTravel_Advert = "B",
                    Householdproducts_Advert = "B",
                    Motoring_Advert = "B",
                    Music_Advert = "B",
                    Newspapers_Advert = "B",
                    NonAlcoholicDrinks_Advert = "B",
                    PetsPetFood_Advert = "B",
                    PharmaceuticalChemistsProducts_Advert = "B",
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
                ProfileMatchOption(model, countryId);
                return model;
            }
        }

        public SkizaProfileFormModel UserProfileSkizaModel(int id, int userid)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            SkizaProfileFormModel model = new ViewModels.SkizaProfileFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            if (id == 0)
            {
                return new SkizaProfileFormModel
                {
                    UserId = userid,
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
                            UserId = userid,
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
                        model.UserId = userid;
                    }
                    return model;
                }
            }
            return new SkizaProfileFormModel
            {
                UserId = userid,
                UserProfileId = userProfile.UserProfileId,
                Hustlers_AdType = "A",
                Youth_AdType = "A",
                DiscerningProfessionals_AdType = "A",
                Mass_AdType = "A"
            };
        }

        public UserProfileTimeSettingFormModel UserProfileTimeSettingFormModel(int id, int userid)
        {
            var Userid = Convert.ToInt32(userid);

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileTimeSettings != null && userProfile.UserProfileTimeSettings.Count != 0)
                {
                    UserProfileTimeSetting userProfileTimeSettings =
                        userProfile.UserProfileTimeSettings.FirstOrDefault();

                    var model = new UserProfileTimeSettingFormModel
                    {
                        UserId = Userid,
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
                { UserId = Userid, UserProfileId = id, AvailableTimes = GetTimes() };
            }
            return new UserProfileTimeSettingFormModel
            { UserId = Userid, UserProfileId = userProfile.UserProfileId, AvailableTimes = GetTimes() };
        }

        public UserProfileMobileFormModel UserProfileMobileFormModel(int id, int userid)
        {
            var Userid = Convert.ToInt32(userid);
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
                            UserId = Userid,
                            ContractTypeList = contractTypeList,
                            SpendList = spendList
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileMobileFormModel>(userProfileMobile);
                        model.UserId = Userid;
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
                    UserId = Userid,
                    UserProfileId = id,
                    ContractTypeList = contractTypeList,
                    SpendList = spendList
                };
            }
            return new UserProfileMobileFormModel
            {
                UserId = Userid,
                UserProfileId = userProfile.UserProfileId,
                ContractTypeList = contractTypeList,
                SpendList = spendList
            };
        }

        public UserProfileDemographicFormModel UserProfileDemographicFormModel(int id, int userid)
        {
            var Userid = Convert.ToInt32(userid);
            UserProfileDemographicFormModel model = new UserProfileDemographicFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);
            User user = _userRepository.GetById(userProfile.UserId);
            int countryId = user.Operator.CountryId.Value;
            var profileMatch = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId).ToList();
            if (profileMatch == null || profileMatch.Count() == 0)
            {
                countryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }

            EFMVCDataContex db = new EFMVCDataContex();
            var userProfilePreference = db.UserMatch.Where(top => top.UserId == Userid).ToList();
            if (userProfilePreference.Count() > 0)
            {
                model.DOB = userProfilePreference.FirstOrDefault().DOB;
                model.MSISDN = userProfilePreference.FirstOrDefault().MSISDN;
            }

            //Location
            IList<SelectListItem> locationList = new List<SelectListItem>();
            var locationProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Location".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var locationProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == locationProfileMatchId).ToList();
            int locationcounter = 0;
            foreach (var location in locationProfileLabel)
            {
                locationList.Add(new SelectListItem { Text = location.ProfileLabel, Value = _answerValues[locationcounter] });
                locationcounter++;
            }

            //Age
            IList<SelectListItem> ageList = new List<SelectListItem>();
            var ageProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Age".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var ageProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == ageProfileMatchId).ToList();
            int agecounter = 0;
            foreach (var age in ageProfileLabel)
            {
                ageList.Add(new SelectListItem { Text = age.ProfileLabel, Value = _answerValues[agecounter] });
                agecounter++;
            }

            //Gender
            IList<SelectListItem> genderList = new List<SelectListItem>();
            var genderProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Gender".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var genderProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == genderProfileMatchId).ToList();
            int gendercounter = 0;
            foreach (var gender in genderProfileLabel)
            {
                genderList.Add(new SelectListItem { Text = gender.ProfileLabel, Value = _answerValues[gendercounter] });
                gendercounter++;
            }

            //HouseholdStatus
            IList<SelectListItem> householdStatusList = new List<SelectListItem>();
            var householdStatusProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Household Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var householdStatusProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == householdStatusProfileMatchId).ToList();
            int householdStatuscounter = 0;
            foreach (var householdStatus in householdStatusProfileLabel)
            {
                householdStatusList.Add(new SelectListItem { Text = householdStatus.ProfileLabel, Value = _answerValues[householdStatuscounter] });
                householdStatuscounter++;
            }

            //RelationshipStatus
            IList<SelectListItem> relationshipStatusList = new List<SelectListItem>();
            var relationshipStatusProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Relationship Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var relationshipStatusProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == relationshipStatusProfileMatchId).ToList();
            int relationshipStatuscounter = 0;
            foreach (var relationshipStatus in relationshipStatusProfileLabel)
            {
                relationshipStatusList.Add(new SelectListItem { Text = relationshipStatus.ProfileLabel, Value = _answerValues[relationshipStatuscounter] });
                relationshipStatuscounter++;
            }

            //IncomeBracket
            IList<SelectListItem> incomeBracketList = new List<SelectListItem>();
            var incomeBracketProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("IncomeBracket".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var incomeBracketProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == incomeBracketProfileMatchId).ToList();
            int incomeBracketcounter = 0;
            foreach (var incomeBracket in incomeBracketProfileLabel)
            {
                incomeBracketList.Add(new SelectListItem { Text = incomeBracket.ProfileLabel, Value = _answerValues[incomeBracketcounter] });
                incomeBracketcounter++;
            }

            //Education
            IList<SelectListItem> educationList = new List<SelectListItem>();
            var educationProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Education".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var educationProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == educationProfileMatchId).ToList();
            int educationcounter = 0;
            foreach (var education in educationProfileLabel)
            {
                educationList.Add(new SelectListItem { Text = education.ProfileLabel, Value = _answerValues[educationcounter] });
                educationcounter++;
            }

            //WorkingStatus
            IList<SelectListItem> workingStatusList = new List<SelectListItem>();
            var workingStatusProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Working Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var workingStatusProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == workingStatusProfileMatchId).ToList();
            int workingStatuscounter = 0;
            foreach (var workingStatus in workingStatusProfileLabel)
            {
                workingStatusList.Add(new SelectListItem { Text = workingStatus.ProfileLabel, Value = _answerValues[workingStatuscounter] });
                workingStatuscounter++;
            }

            if (userProfile != null)
            {
                if (userProfile.UserProfilePreferences != null && userProfile.UserProfilePreferences.Count != 0)
                {
                    UserProfilePreference userProfileDemographic = userProfile.UserProfilePreferences.FirstOrDefault();
                    if (String.IsNullOrEmpty(userProfileDemographic.Location_Demographics) && String.IsNullOrEmpty(userProfileDemographic.Gender_Demographics)
                        && String.IsNullOrEmpty(userProfileDemographic.HouseholdStatus_Demographics) && String.IsNullOrEmpty(userProfileDemographic.RelationshipStatus_Demographics)
                        && String.IsNullOrEmpty(userProfileDemographic.IncomeBracket_Demographics) && String.IsNullOrEmpty(userProfileDemographic.Education_Demographics)
                        && String.IsNullOrEmpty(userProfileDemographic.WorkingStatus_Demographics))
                    {
                        return new UserProfileDemographicFormModel
                        {
                            Id = userProfileDemographic.Id,
                            UserProfileId = id,
                            UserId = Userid,
                            LocationList = locationList,
                            AgeList = ageList,
                            GenderList = genderList,
                            HouseholdStatusList = householdStatusList,
                            RelationshipStatusList = relationshipStatusList,
                            IncomeBracketList = incomeBracketList,
                            EducationList = educationList,
                            WorkingStatusList = workingStatusList
                        };
                    }
                    else
                    {
                        model =
                           Mapper.Map<UserProfilePreference, UserProfileDemographicFormModel>(userProfileDemographic);
                        model.UserId = Userid;
                        model.LocationList = locationList;
                        model.AgeList = ageList;
                        model.GenderList = genderList;
                        model.HouseholdStatusList = householdStatusList;
                        model.RelationshipStatusList = relationshipStatusList;
                        model.IncomeBracketList = incomeBracketList;
                        model.EducationList = educationList;
                        model.WorkingStatusList = workingStatusList;
                    }
                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileDemographicFormModel
                {
                    UserId = Userid,
                    UserProfileId = id,
                    LocationList = locationList,
                    AgeList = ageList,
                    GenderList = genderList,
                    HouseholdStatusList = householdStatusList,
                    RelationshipStatusList = relationshipStatusList,
                    IncomeBracketList = incomeBracketList,
                    EducationList = educationList,
                    WorkingStatusList = workingStatusList
                };
            }
            return new UserProfileDemographicFormModel
            {
                UserId = Userid,
                UserProfileId = userProfile.UserProfileId,
                LocationList = locationList,
                AgeList = ageList,
                GenderList = genderList,
                HouseholdStatusList = householdStatusList,
                RelationshipStatusList = relationshipStatusList,
                IncomeBracketList = incomeBracketList,
                EducationList = educationList,
                WorkingStatusList = workingStatusList
            };
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

            return model;

        }

        private UserProfileMobileFormModel MobileMatchOption(UserProfileMobileFormModel model, int? countryId)
        {
            model.DisplayContractType = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Mobile plan");
            model.DisplaySpend = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Average Monthly Spend");

            return model;
        }

        private UserProfileDemographicFormModel DemographicMatchOption(UserProfileDemographicFormModel model, int? countryId)
        {
            model.DisplayLocation = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Location");
            model.DisplayAge = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Age");
            model.DisplayGender = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Gender");
            model.DisplayHouseholdStatus = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Household Status");
            model.DisplayRelationshipStatus = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Relationship Status");
            model.DisplayIncomeBracket = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "IncomeBracket");
            model.DisplayEducation = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Education");
            model.DisplayWorkingStatus = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Working Status");

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


        //[Route("SaveAdverts")]
        //[HttpPost]
        //public ActionResult SaveAdverts(UserProfileResult _model)
        //{
        //    UserProfileAdvertFormModel model = new UserProfileAdvertFormModel();
        //    model = _model.UserProfileAdvertFormModel;
        //    if (ModelState.IsValid)
        //    {
        //        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

        //        CreateOrUpdateUserProfileAdvertCommand command =
        //            Mapper.Map<UserProfileAdvertFormModel, CreateOrUpdateUserProfileAdvertCommand>(model);

        //        User user = _userRepository.GetById(model.UserId);
        //        if (ModelState.IsValid)
        //        {
        //            var countryId = user.Operator.CountryId;
        //            command.CountryId = countryId;
        //            command.OperatorId = user.OperatorId;
        //            ICommandResult result = _commandBus.Submit(command);

        //            UserMatchTableProcess obj = new UserMatchTableProcess();
        //            EFMVCDataContex SQLServerEntities = new EFMVCDataContex();

        //            obj.UpdateAdTypes(model, user, SQLServerEntities);

        //            var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
        //            if (ConnString != null && ConnString.Count() > 0)
        //            {
        //                foreach (var item in ConnString)
        //                {
        //                    SQLServerEntities = new EFMVCDataContex(item);
        //                    obj.UpdateAdTypes(model, user, SQLServerEntities);
        //                    PreMatchProcess.PreCampaignUsermatchProcess(model.UserId, user.UserMatchTableName, item);
        //                }
        //            }
        //            if (result.Success)
        //            {
        //                return Json("success");
        //            }
        //        }
        //    }
        //    return Json("fail");
        //}

        //[Route("SaveTimeSettings")]
        //[HttpPost]
        //public ActionResult SaveTimeSettings(UserProfileTimeSettingFormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
        //        model.OperatorId = _userRepository.GetById(model.UserId).OperatorId;
        //        CreateOrUpdateUserProfileTimeSettingCommand command =
        //            Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

        //        if (ModelState.IsValid)
        //        {
        //            ICommandResult result = _commandBus.Submit(command);

        //            if (result.Success)
        //            {
        //                return Json("success");
        //            }
        //            else
        //            {
        //                return Json("fail");
        //            }
        //        }
        //    }

        //    return Json("fail");
        //}

        //[Route("SaveSkizaProfile")]
        //[HttpPost]
        //public ActionResult SaveSkizaProfile(UserProfileResult _model)
        //{
        //    SkizaProfileFormModel model = new SkizaProfileFormModel();
        //    model = _model.SkizaProfileFormModel;
        //    if (ModelState.IsValid)
        //    {
        //        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
        //        User user = _userRepository.GetById(model.UserId);


        //        if (ModelState.IsValid)
        //        {
        //            var countryId = user.Operator.CountryId;

        //            #region UserProfilePreference
        //            EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //            var userProfileData = SQLServerEntities.UserProfilePreference.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
        //            if (userProfileData != null)
        //            {
        //                userProfileData.Hustlers_AdType = model.Hustlers_AdType;
        //                userProfileData.Youth_AdType = model.Youth_AdType;
        //                userProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
        //                userProfileData.Mass_AdType = model.Mass_AdType;
        //                SQLServerEntities.SaveChanges();
        //            }
        //            else
        //            {
        //                userProfileData.Hustlers_AdType = model.Hustlers_AdType;
        //                userProfileData.Youth_AdType = model.Youth_AdType;
        //                userProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
        //                userProfileData.Mass_AdType = model.Mass_AdType;
        //                SQLServerEntities.UserProfilePreference.Add(userProfileData);
        //                SQLServerEntities.SaveChanges();
        //            }
        //            #endregion

        //            UserMatchTableProcess obj = new UserMatchTableProcess();
        //            obj.UpdateSkizaProfile(model, user, SQLServerEntities);

        //            var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
        //            if (ConnString != null && ConnString.Count() > 0)
        //            {
        //                foreach (var item in ConnString)
        //                {
        //                    SQLServerEntities = new EFMVCDataContex(item);
        //                    #region External User Profile Preference
        //                    var externalUserProfileData = SQLServerEntities.UserProfilePreference.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
        //                    if (externalUserProfileData != null)
        //                    {
        //                        externalUserProfileData.Hustlers_AdType = model.Hustlers_AdType;
        //                        externalUserProfileData.Youth_AdType = model.Youth_AdType;
        //                        externalUserProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
        //                        externalUserProfileData.Mass_AdType = model.Mass_AdType;
        //                        SQLServerEntities.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        externalUserProfileData.Hustlers_AdType = model.Hustlers_AdType;
        //                        externalUserProfileData.Youth_AdType = model.Youth_AdType;
        //                        externalUserProfileData.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
        //                        externalUserProfileData.Mass_AdType = model.Mass_AdType;
        //                        SQLServerEntities.UserProfilePreference.Add(externalUserProfileData);
        //                        SQLServerEntities.SaveChanges();
        //                    }
        //                    #endregion
        //                    obj.UpdateSkizaProfile(model, user, SQLServerEntities);
        //                    PreMatchProcess.PreCampaignUsermatchProcess(model.UserId, user.UserMatchTableName, item);
        //                }
        //            }
        //            return Json("success");
        //        }
        //    }
        //    return Json("fail");
        //}

        //[Route("SaveDemographic")]
        //[HttpPost]
        //public ActionResult SaveDemographic(UserProfileResult _model)
        //{
        //    UserProfileDemographicFormModel model = new UserProfileDemographicFormModel();
        //    model = _model.UserProfileDemographicFormModel;
        //    if (ModelState.IsValid)
        //    {
        //        var userid = Convert.ToInt32(model.UserId);
        //        CreateOrUpdateUserProfileDemographicsCommand command =
        //            Mapper.Map<UserProfileDemographicFormModel, CreateOrUpdateUserProfileDemographicsCommand>(model);

        //        if (ModelState.IsValid)
        //        {
        //            User user = _userRepository.GetById(userid);
        //            command.CountryId = user.Operator.CountryId;

        //            ICommandResult result = _commandBus.Submit(command);
        //            if (result.Success)
        //            {
        //                UserMatchTableProcess obj = new UserMatchTableProcess();
        //                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //                obj.UpdateDemographicData(model, user, SQLServerEntities);

        //                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
        //                if (ConnString != null && ConnString.Count() > 0)
        //                {
        //                    foreach (var item in ConnString)
        //                    {
        //                        SQLServerEntities = new EFMVCDataContex(item);
        //                        using (SQLServerEntities)
        //                        {
        //                            obj.UpdateDemographicData(model, user, SQLServerEntities);
        //                            PreMatchProcess.PreCampaignUsermatchProcess(user.UserId, user.UserMatchTableName, item);
        //                        }
        //                    }
        //                }
        //                return Json("success");
        //            }
        //            else
        //            {
        //                return Json("fail");
        //            }
        //        }
        //    }
        //    return Json("fail");
        //}

        //[Route("SaveMobile")]
        //[HttpPost]
        //public ActionResult SaveMobile(UserProfileResult _model)
        //{
        //    UserProfileMobileFormModel model = new UserProfileMobileFormModel();
        //    model = _model.UserProfileMobileFormModel;
        //    if (ModelState.IsValid)
        //    {
        //        var userid = Convert.ToInt32(model.UserId);
        //        CreateOrUpdateUserProfileMobileCommand command =
        //            Mapper.Map<UserProfileMobileFormModel, CreateOrUpdateUserProfileMobileCommand>(model);

        //        if (ModelState.IsValid)
        //        {
        //            User user = _userRepository.GetById(userid);
        //            command.CountryId = user.Operator.CountryId;

        //            ICommandResult result = _commandBus.Submit(command);
        //            if (result.Success)
        //            {
        //                UserMatchTableProcess obj = new UserMatchTableProcess();
        //                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
        //                obj.UpdateMobileData(model, user, SQLServerEntities);

        //                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
        //                if (ConnString != null && ConnString.Count() > 0)
        //                {
        //                    foreach (var item in ConnString)
        //                    {
        //                        SQLServerEntities = new EFMVCDataContex(item);
        //                        using (SQLServerEntities)
        //                        {
        //                            obj.UpdateMobileData(model, user, SQLServerEntities);
        //                            PreMatchProcess.PreCampaignUsermatchProcess(user.UserId, user.UserMatchTableName, item);
        //                        }
        //                    }
        //                }
        //                return Json("success");
        //            }
        //            else
        //            {
        //                return Json("fail");
        //            }
        //        }
        //    }
        //    return Json("fail");
        //}


        public string GetLocationData(int Id, UserProfileDemographicAdvertiserFormModel userProfileDemographic)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Location_Demographics).FirstOrDefault();
            var userLocation = userProfileDemographic.Location_Demographics;

            if (userLocation == null || advertData.IndexOf(userLocation) == -1)
                return advertData.Substring(0, 1);
            else
                return userLocation;
        }


        public int GetAgeData(int Id, int age)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Age_Demographics).FirstOrDefault();
            var ageArray = advertData.Select(x => new string(x, 1)).ToArray();
            if (ageArray.Count() > 7)
                return age;
            else if (advertData == null || ageArray.Count() == 0)
                return age;
            else
                return checkAge(age, ageArray);
        }

        public string GetGenderData(int Id, UserProfileDemographicAdvertiserFormModel userProfileDemographic)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Gender_Demographics).FirstOrDefault();
            var userGender = userProfileDemographic.Gender_Demographics;
            if ((advertData == null || advertData.Length == 0) && userGender == null)
                return "A";
            else if (advertData == null || advertData.Length == 0)
                return userGender;
            else if (userGender == null || advertData.IndexOf(userGender) == -1)
                return advertData.Substring(0, 1);
            else
                return userGender;
        }

        public int checkAge(int age, string[] ranges)
        {
            var rnd = new Random();
            var letter = "Z";
            if (age >= 1 && age < 18)
                letter = "A";
            else if (age >= 18 && age < 25)
                letter = "B";
            else if (age >= 25 && age < 35)
                letter = "C";
            else if (age >= 35 && age < 45)
                letter = "D";
            else if (age >= 45 && age < 55)
                letter = "E";
            else if (age >= 55 && age < 65)
                letter = "F";
            else if (age >= 65)
                letter = "G";
            else
                letter = "H";

            if(ranges == null || ranges.Count() == 0)
                return age;
            else if (ranges.Contains(letter))
                return age;
            else if (ranges.Contains("B") && ranges.Contains("C"))
                return rnd.Next(18, 36);
            else if (ranges.Contains("B") && ranges.Contains("C"))
                return rnd.Next(18, 36);
            else if (ranges.Contains("C"))
                return rnd.Next(25, 36);
            else if (ranges.Contains("H"))
                return 0;
            else return 28;
        }


        public UserProfileSkizaProfileAdvertiserFormModel GetSkizaData(int campaignId, UserProfileSkizaProfileAdvertiserFormModel model)
        {
            model.Hustlers_AdType = GetHustlersData(campaignId);
            model.Mass_AdType = GetMassData(campaignId);
            model.Youth_AdType = GetYouthData(campaignId);
            model.DiscerningProfessionals_AdType = GetDiscerningData(campaignId);
            return model;
        }

        public string GetHustlersData(int Id)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Hustlers_AdType).FirstOrDefault();
            if (advertData == null || advertData.Length == 0)
                return "B";
            else
                return advertData;
        }

        public string GetYouthData(int Id)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Youth_AdType).FirstOrDefault();
            if (advertData == null || advertData.Length == 0)
                return "C";
            else
                return advertData;
        }

        public string GetDiscerningData(int Id)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.DiscerningProfessionals_AdType).FirstOrDefault();
            if (advertData == null || advertData.Length == 0)
                return "A";
            else
                return advertData;
        }

        public string GetMassData(int Id)
        {
            var advertData = _campaignProfilePreferenceRepository.AsQueryable().Where(c => c.CampaignProfileId == Id).Select(cp => cp.Mass_AdType).FirstOrDefault();
            if (advertData == null || advertData.Length == 0)
                return "E";
            else
                return advertData;
        }
    }
}