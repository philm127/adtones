using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("Mobile")]
    public class MobileController : Controller
    {
        // GET: Users/Mobile

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

        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;
        public MobileController(ICommandBus commandBus, IProfileRepository profileRepository,
                               IUserRepository userRepository, IUserProfilePreferenceRepository userProfilePreferenceRepository, ICountryRepository countryRepository,
        IProfileMatchInformationRepository profileMatchInformationRepository, IProfileMatchLabelRepository profileMatchLabelRepository)
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

        [Route("Index")]
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);
            var countryId = user.Operator.CountryId;
            IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);
            UserProfileMobileFormModel _userprofileMobileModel = new ViewModels.UserProfileMobileFormModel();
            
            if (userFormModel.UserProfile != null)
            {
                ViewBag.UserProfileId = userFormModel.UserProfile.UserProfileId;
                _userprofileMobileModel = UserProfileMobileFormModel(userFormModel.UserProfile.UserProfileId, (int)countryId);
            }
            else
            {
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

                _userprofileMobileModel.ContractTypeList = contractTypeList;
                _userprofileMobileModel.SpendList = spendList;
                _userprofileMobileModel.UserProfileId = 0;
                ViewBag.UserProfileId = 0;
            }
            MobileMatchOption(_userprofileMobileModel, countryId);
            return View(_userprofileMobileModel);
        }

        private UserProfileMobileFormModel MobileMatchOption(UserProfileMobileFormModel model,int? countryId)
        {
            //model.DisplayContractType = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.ContractType);
            //model.DisplaySpend = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, (int)ProfileMatchInfo.AverageMonthlySpend);
            model.DisplayContractType = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Mobile plan");
            model.DisplaySpend = GetProfileMatchOption.IsActiveProfileInfo((int)countryId, "Average Monthly Spend");

            return model;
        }

        [Route("SaveMobile")]
        [HttpPost]
        public ActionResult SaveMobile(UserProfileMobileFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                User user = _userRepository.GetById(efmvcUser.UserId);

                CreateOrUpdateUserProfileMobileCommand command =
                    Mapper.Map<UserProfileMobileFormModel, CreateOrUpdateUserProfileMobileCommand>(model);
                var userprofilePref = _userProfilePreferenceRepository.Get(top => top.UserProfileId == model.UserProfileId);
                if (userprofilePref != null)
                {
                    command.Id = userprofilePref.Id;
                }
                else
                {
                    command.Id = 0;
                }
                if (ModelState.IsValid)
                {
                    var countryId = user.Operator.CountryId;
                    command.CountryId = countryId;
                    command.OperatorId = user.OperatorId;
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        
                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //obj.UpdateMobileData(model, user, SQLServerEntities);
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //PreMatchProcess.PreCampaignUsermatchProcess(efmvcUser.UserId, user.UserMatchTableName,conn);
                        
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
                                        model.UserProfileId = externalServerUserProfileId;
                                        obj.UpdateMobileData(model, userData, SQLServerEntities);
                                        PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                                    }
                                   
                                }
                            }                         
                        }

                       
                        TempData["sucess"] = "Record updated successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Internal server error. Please try again";
                        return RedirectToAction("Index");
                        
                    }
                }
            }

            TempData["error"] = "Internal server error. Please try again";
            return RedirectToAction("Index");
        }
        public UserProfileMobileFormModel UserProfileMobileFormModel(int id, int countryId)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            UserProfileMobileFormModel model = new ViewModels.UserProfileMobileFormModel();

            UserProfile userProfile = _profileRepository.GetById(id);

            var profileMatch = _profileMatchInformationRepository.GetMany(top => top.CountryId == countryId && (top.ProfileName == "Mobile plan" || top.ProfileName == "Average Monthly Spend")).ToList();
            if (profileMatch == null || profileMatch.Count() == 0)
            {
                //Comment 08-03-2019
                //countryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya".ToLower()).Id;

                //Add 08-03-2019
                countryId = countryId;
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
    }
}