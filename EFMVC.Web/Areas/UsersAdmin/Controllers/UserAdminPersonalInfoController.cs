using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Security;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("UserAdminPersonalInfo")]
    public class UserAdminPersonalInfoController : Controller
    {
        // GET: Users/PersonalInfo
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
        private readonly ICountryRepository _countryRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IProfileMatchLabelRepository _profileMatchLabelRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;
        public UserAdminPersonalInfoController(ICommandBus commandBus, IProfileRepository profileRepository, IUserRepository userRepository, IUserProfilePreferenceRepository userProfilePreferenceRepository, ICountryRepository countryRepository, IProfileMatchInformationRepository profileMatchInformationRepository, IProfileMatchLabelRepository profileMatchLabelRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _countryRepository = countryRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _profileMatchLabelRepository = profileMatchLabelRepository;
        }

        private readonly string[] _answerValues = new[]
                                                      {
                                                          "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                                          "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                                      };

        public bool checkuserProfileDemography(UserProfilePreference userprofilePref)
        {
            if (String.IsNullOrEmpty(userprofilePref.HouseholdStatus_Demographics) && String.IsNullOrEmpty(userprofilePref.IncomeBracket_Demographics) && String.IsNullOrEmpty(userprofilePref.Location_Demographics) &&
                String.IsNullOrEmpty(userprofilePref.RelationshipStatus_Demographics) && String.IsNullOrEmpty(userprofilePref.Gender_Demographics) && String.IsNullOrEmpty(userprofilePref.WorkingStatus_Demographics)
                && String.IsNullOrEmpty(userprofilePref.Education_Demographics))
            {
                return false;
            }
            else
            {

                return true;
            }

        }

        [Route("Index")]
        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {

                var userId = Convert.ToInt32(Session["userId"]);

                User user = _userRepository.GetById(userId);
                int countryId = user.Operator.CountryId.Value;
                IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == userId);
                UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);
                //Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists();
                Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists(countryId);
                //ViewBag.Country = _countryRepository.Get(s => s.Name.ToLower() == "kenya").Name;//ConfigurationManager.AppSettings["Country"];
                ViewBag.Country = _countryRepository.GetById(countryId).Name;
                if (userFormModel.UserProfile != null)
                {
                    UserProfile userProfile = _profileRepository.GetById(userFormModel.UserProfile.UserProfileId);
                    ViewBag.emailAddress = user.Email;
                    ViewBag.UserId = user.UserId;
                    UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);
                    //check userprofilepreferance;
                    model.Email = user.Email;
                    var userprofilePref = _userProfilePreferenceRepository.Get(top => top.UserProfileId == userFormModel.UserProfile.UserProfileId);
                    if (userprofilePref != null)
                    {
                        bool status = checkuserProfileDemography(userprofilePref);
                        if (status == true)
                        {
                            model.HouseholdStatus = userprofilePref.HouseholdStatus_Demographics;
                            model.IncomeBracket = userprofilePref.IncomeBracket_Demographics;
                            // //code commented on 30-03-2017
                            // model.Location = userprofilePref.Location_Demographics;
                            model.RelationshipStatus = userprofilePref.RelationshipStatus_Demographics;
                            model.Gender = userprofilePref.Gender_Demographics;
                            model.WorkingStatus = userprofilePref.WorkingStatus_Demographics;
                            model.Education = userprofilePref.Education_Demographics;
                        }
                        else
                        {
                            model.HouseholdStatus = string.Empty;
                            model.IncomeBracket = string.Empty;
                            // //code commented on 30-03-2017
                            // model.Location = string.Empty;
                            model.RelationshipStatus = string.Empty;
                            model.Gender = string.Empty;
                            model.WorkingStatus = string.Empty;
                            model.Education = string.Empty;
                        }

                    }
                    //code commented on 30-03-2017
                    //model.LocationList = selectLists["locationList"];
                    model.GenderList = selectLists["genderList"];
                    model.IncomeBracketList = selectLists["incomeBracketList"];
                    model.WorkingStatusList = selectLists["workingStatusList"];
                    model.RelationshipStatusList = selectLists["relationshipStatusList"];
                    model.EducationList = selectLists["educationList"];
                    model.HouseholdStatusList = selectLists["householdStatusList"];
                    return View(model);
                }
                else
                {
                    UserProfile userProfile = new UserProfile();
                    UserProfileFormModel model = Mapper.Map<UserProfile, UserProfileFormModel>(userProfile);
                     //code commented on 30-03-2017
                    //model.LocationList = selectLists["locationList"];
                    model.GenderList = selectLists["genderList"];
                    model.IncomeBracketList = selectLists["incomeBracketList"];
                    model.WorkingStatusList = selectLists["workingStatusList"];
                    model.RelationshipStatusList = selectLists["relationshipStatusList"];
                    model.EducationList = selectLists["educationList"];
                    model.HouseholdStatusList = selectLists["householdStatusList"];
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "UserAdmin" });
            }
        }
        [Route("Save")]
        [HttpPost]
        public ActionResult Save(UserProfileFormModel model, string parameterValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["userId"] != null)
                    {
                        var userId = Convert.ToInt32(Session["userId"]);

                        User user = _userRepository.GetById(userId);
                        int countryId = user.Operator.CountryId.Value;
                        if (model.DOB == null && !string.IsNullOrEmpty(parameterValue))
                        {
                            model.DOB = Convert.ToDateTime(parameterValue);
                        }

                        EFMVCDataContex db = new EFMVCDataContex();
                        var userprofileData = db.Userprofiles.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
                        if (userprofileData != null)
                        {
                            model.MSISDN = userprofileData.MSISDN;
                        }

                        //var checkMobile = _profileRepository.Get(top => top.MSISDN == model.MSISDN && top.UserId != user.UserId);
                        //if (checkMobile != null)
                        //{
                        //    TempData["error"] = "The Mobile Phone Number you are attempting to register already exists on the system.";
                        //    return RedirectToAction("Index");
                        //}
                        var _existsemail = _userRepository.GetMany(top => top.Email.Trim() == model.Email.Trim() && top.UserId != userId).FirstOrDefault();
                        if (_existsemail != null)
                        {
                            if (_existsemail.Activated == 3)
                            {
                                //use deleted email address
                            }
                            else
                            {
                                TempData["error"] = "The Email Address is already exists in database so please choose another one.";
                                //return RedirectToAction("Index");
                                Dictionary<string, IList<SelectListItem>> selectLists = GetSelectLists(countryId);
                                model.GenderList = selectLists["genderList"];
                                model.IncomeBracketList = selectLists["incomeBracketList"];
                                model.WorkingStatusList = selectLists["workingStatusList"];
                                model.RelationshipStatusList = selectLists["relationshipStatusList"];
                                model.EducationList = selectLists["educationList"];
                                model.HouseholdStatusList = selectLists["householdStatusList"];
                                return View("Index", model);
                            }

                        }
                        //var countryId = user.Operator.CountryId;
                        var usercommand = new ChangeUserProfileInfoCommand
                        {
                            Email = model.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Organisation = user.Organisation,
                            UserId = user.UserId
                            //RewardId = user.RewardId
                        };

                        ICommandResult commandResult = _commandBus.Submit(usercommand);

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

                        command.UserId = userId;

                        if (ModelState.IsValid)
                        {
                            command.CountryId = countryId;
                            ICommandResult result = _commandBus.Submit(command);
                            if (result.Success)
                            {
                                var ProfileId = result.Id;
                                //update the Demographic Info
                                CreateOrUpdateUserProfileDemographicsCommand demographic_command = new CreateOrUpdateUserProfileDemographicsCommand();
                                //check userprofilepreferance;
                                var userprofilePref = _userProfilePreferenceRepository.Get(top => top.UserProfileId == ProfileId);
                                if (userprofilePref != null)
                                {
                                    demographic_command.Id = userprofilePref.Id;
                                }
                                else
                                {
                                    demographic_command.Id = 0;
                                }
                                demographic_command.UserProfileId = ProfileId;
                                //demographic_command.Education_Demographics = model.HouseholdStatus;
                                demographic_command.Education_Demographics = model.Education;
                                demographic_command.Gender_Demographics = model.Gender;
                                demographic_command.HouseholdStatus_Demographics = model.HouseholdStatus;
                                demographic_command.IncomeBracket_Demographics = model.IncomeBracket;
                                // //code commented on 30-03-2017
                                //demographic_command.Location_Demographics = model.Location;
                                demographic_command.RelationshipStatus_Demographics = model.RelationshipStatus;
                                demographic_command.WorkingStatus_Demographics = model.WorkingStatus;
                                demographic_command.CountryId = countryId;
                                ICommandResult result_demo = _commandBus.Submit(demographic_command);

                                //UserMatchTableProcess obj = new UserMatchTableProcess();
                                //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                                //obj.UpdatePersonalInfoData(model, user, ProfileId, SQLServerEntities);
                               
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
                                            if (userData != null && externalServerUserProfileId != 0)
                                            {
                                                obj.UpdatePersonalInfoData(model, userData, externalServerUserProfileId, SQLServerEntities);
                                                PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                                            }
                                           
                                        }
                                    }
                                }


                                if (result_demo.Success)
                                {
                                    TempData["sucess"] = "Record updated successfully";
                                    return RedirectToAction("Index");
                                }

                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "UserAdmin" });
                }

            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }


        }
        private Dictionary<string, IList<SelectListItem>> GetSelectLists(int countryId)
        {
            var lists = new Dictionary<string, IList<SelectListItem>>();
            
            var profileMatch = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId).ToList();
            if (profileMatch == null || profileMatch.Count() == 0)
            {
                countryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;
            }

            //Age
            IList<SelectListItem> ageBracketList = new List<SelectListItem>();
            var ageProfileMatchId = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && top.ProfileName.ToLower().Equals("Age".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
            var ageProfileLabel = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == ageProfileMatchId).ToList();
            int agecounter = 0;
            foreach (var age in ageProfileLabel)
            {
                ageBracketList.Add(new SelectListItem { Text = age.ProfileLabel, Value = _answerValues[agecounter] });
                agecounter++;
            }
            //IList<SelectListItem> ageBracketList = new List<SelectListItem>();
            //ageBracketList.Add(new SelectListItem { Text = "Under 18", Value = "A" });
            //ageBracketList.Add(new SelectListItem { Text = "18-24", Value = "B" });
            //ageBracketList.Add(new SelectListItem { Text = "25-34", Value = "C" });
            //ageBracketList.Add(new SelectListItem { Text = "35-44", Value = "D" });
            //ageBracketList.Add(new SelectListItem { Text = "45-54", Value = "E" });
            //ageBracketList.Add(new SelectListItem { Text = "55-64", Value = "F" });
            //ageBracketList.Add(new SelectListItem { Text = "65+", Value = "G" });
            //ageBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "H" });

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
            //IList<SelectListItem> genderList = new List<SelectListItem>();
            //genderList.Add(new SelectListItem { Text = "Male", Value = "A" });
            //genderList.Add(new SelectListItem { Text = "Female", Value = "B" });
            //genderList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "C" });

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
            //IList<SelectListItem> incomeBracketList = new List<SelectListItem>();
            //incomeBracketList.Add(new SelectListItem { Text = "£0 to £14,999", Value = "A" });
            //incomeBracketList.Add(new SelectListItem { Text = "£15,000 to £24,999", Value = "B" });
            //incomeBracketList.Add(new SelectListItem { Text = "£25,000 to £39,999", Value = "C" });
            //incomeBracketList.Add(new SelectListItem { Text = "£40,000 to £74,999", Value = "D" });
            //incomeBracketList.Add(new SelectListItem { Text = "£75,000 to £99,999", Value = "E" });
            //incomeBracketList.Add(new SelectListItem { Text = "£100,000+", Value = "F" });
            //incomeBracketList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "G" });

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
            //IList<SelectListItem> workingStatusList = new List<SelectListItem>();
            //workingStatusList.Add(new SelectListItem { Text = "Employed", Value = "A" });
            //workingStatusList.Add(new SelectListItem { Text = "Self-Employed", Value = "B" });
            //workingStatusList.Add(new SelectListItem { Text = "Housewife/Househusband", Value = "C" });
            //workingStatusList.Add(new SelectListItem { Text = "Retired", Value = "D" });
            //workingStatusList.Add(new SelectListItem { Text = "Unpaid Carer", Value = "E" });
            //workingStatusList.Add(new SelectListItem { Text = "Full or Part-time Education", Value = "F" });
            //workingStatusList.Add(new SelectListItem { Text = "Not Working", Value = "G" });
            //workingStatusList.Add(new SelectListItem { Text = "None of these", Value = "H" });
            //workingStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "I" });

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
            //IList<SelectListItem> relationshipStatusList = new List<SelectListItem>();
            //relationshipStatusList.Add(new SelectListItem { Text = "Divorced", Value = "A" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Living with another", Value = "B" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Married", Value = "C" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Separated", Value = "D" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Single", Value = "E" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Widowed", Value = "F" });
            //relationshipStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "G" });

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
            //IList<SelectListItem> educationList = new List<SelectListItem>();
            //educationList.Add(new SelectListItem { Text = "Secondary", Value = "A" });
            //educationList.Add(new SelectListItem { Text = "College", Value = "B" });
            //educationList.Add(new SelectListItem { Text = "University", Value = "C" });
            //educationList.Add(new SelectListItem { Text = "Post Graduate", Value = "D" });
            //educationList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "E" });

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
            //IList<SelectListItem> householdStatusList = new List<SelectListItem>();
            //householdStatusList.Add(new SelectListItem { Text = "Rent", Value = "A" });
            //householdStatusList.Add(new SelectListItem { Text = "Owner", Value = "B" });
            //householdStatusList.Add(new SelectListItem { Text = "Live with someone", Value = "C" });
            //householdStatusList.Add(new SelectListItem { Text = "Prefer not to answer", Value = "D" });

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
            //IList<SelectListItem> locationList = new List<SelectListItem>();
            //locationList.Add(new SelectListItem { Text = "London", Value = "A" });
            //locationList.Add(new SelectListItem { Text = "South East (excl. London)", Value = "B" });
            //locationList.Add(new SelectListItem { Text = "South West", Value = "C" });
            //locationList.Add(new SelectListItem { Text = "East Anglia", Value = "D" });
            //locationList.Add(new SelectListItem { Text = "Midlands", Value = "E" });
            //locationList.Add(new SelectListItem { Text = "Wales", Value = "F" });
            //locationList.Add(new SelectListItem { Text = "North West", Value = "G" });
            //locationList.Add(new SelectListItem { Text = "North East", Value = "H" });
            //locationList.Add(new SelectListItem { Text = "Scotland", Value = "I" });
            //locationList.Add(new SelectListItem { Text = "Northern Ireland", Value = "J" });

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
        [Route("ChangePassword")]
        public ActionResult ChangePassword(int id)
        {
            var details = _userRepository.Get(top => top.UserId == id);
            ViewBag.UserId = details.UserId;
            ViewBag.fullname = details.FirstName + " " + details.LastName;
            ChangePasswordFormModel _form = new ChangePasswordFormModel();
            return View(_form);
        }
        [Route("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(string userid, ChangePasswordFormModel form)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {

                    var command = new ChangePasswordCommand
                    {
                        UserId = Convert.ToInt32(userid),
                        OldPassword = form.OldPassword,
                        NewPassword = form.NewPassword
                    };
                    IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
                    ModelState.AddModelErrors(errors);
                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            var userinfo = _userRepository.GetById(Convert.ToInt32(userid));
                            if (userinfo != null)
                            {
                                changepassowrd(userinfo.Email);
                            }
                            TempData["success"] = "User Password changed successfully.";
                            return RedirectToAction("ChangePassword", new { @id = Convert.ToInt32(userid) });
                            //return View("ChangePassword");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Model", "The current password is incorrect or the new password is invalid.");
                        int Userid = Convert.ToInt32(userid);
                        var details = _userRepository.Get(top => top.UserId == Userid);
                        ViewBag.UserId = details.UserId;
                        ViewBag.fullname = details.FirstName + " " + details.LastName;
                        return View("ChangePassword");
                    }
                }

            }
            return RedirectToAction("ChangePassword", new { @id = Convert.ToInt32(userid) });
        }
        public void changepassowrd(string email)
        {

            var reader =
                new StreamReader(
                    Server.MapPath(ConfigurationManager.AppSettings["ChangePassowordTemplate"]));
            string emailContent = reader.ReadToEnd();
            emailContent = string.Format(emailContent, email);
            MailSending("support@adtones.xyz", "Supp0rtPa55w0rd!", "ChangePassword", email, emailContent, "smtp.gmail.com", 587, true);

        }
        public void MailSending(string Username, string Password, string mailSubject, string mailTo, string mailBody, string host, int port, bool EnableSSL)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            mail.From = new MailAddress(Username, Username);
            mail.Subject = mailSubject;
            mail.To.Add(mailTo);
            mail.Body = mailBody;
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;


            client.Port = port;
            client.EnableSsl = EnableSSL;
            if (String.IsNullOrEmpty(Username) && String.IsNullOrEmpty(Password))
            {
                client.UseDefaultCredentials = true;

            }
            else
            {
                client.UseDefaultCredentials = false;

            }
            client.Host = host;
            client.Credentials = new NetworkCredential(Username, Password);
            client.SendMailAsync(mail);
        }
        [Route("DeleteUser")]
        public ActionResult DeleteUser(int id)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = 3;
            ICommandResult result = _commandBus.Submit(command);
            TempData["status"] = "User is deleted successfully.";
            return RedirectToAction("Index", "UserAccount");
            //return RedirectToAction("Index");

        }
    }
}