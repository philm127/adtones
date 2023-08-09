using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Security;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EFMVC.Web.Common
{
    public enum OperatorTableId
    {
        //Expresso = 1, // Senegal Country
        //Safaricom = 2 // Kenya Country

        Safaricom = 1,// Kenya Country
        Expresso = 2 // Senegal Country       
    }

    public class OperatorFunctionality
    {
        private readonly IUserRepository _userRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly ICommandBus _commandBus;
        private readonly IRewardRepository _rewardRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IFormsAuthentication formAuthentication;
        private readonly IProfileRepository _profileRepository;
        public OperatorFunctionality(IUserRepository userRepository, IOperatorRepository operatorRepository, IRewardRepository rewardRepository, IUserProfileRepository userProfileRepository, IFormsAuthentication formAuthentication, IProfileRepository profileRepository, ICommandBus commandBus)
        {
            _userRepository = userRepository;
            _operatorRepository = operatorRepository;
            _rewardRepository = rewardRepository;
            _userProfileRepository = userProfileRepository;
            this.formAuthentication = formAuthentication;
            _profileRepository = profileRepository;
            _commandBus = commandBus;
        }

        public int AddUser(UserFormModel form, StreamReader reader)
        {
            #region Add User

            var OperatorId = (int)form.OperatorId;
            UserMatchTableProcess objUserMatch = new UserMatchTableProcess();

            //form.FirstName = FirstName;
            //form.LastName = LastName;
            //form.Email = Email;
            //form.Organisation = Organisation;
            //form.Password = Password;
            //form.MSISDN = CountryCode + MSISDN;
            //form.OperatorId = OperatorId;

            form.MSISDN = form.CountryCode + form.MSISDN;
            form.IsMobileVerfication = true;
            form.VerificationStatus = true;
            form.IsMsisdnMatch = true;

            form.UserMatchTableName = objUserMatch.GetUserMatchTableNumber(OperatorId);

            UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
            command.Activated = 1;
            command.RoleId = (Int32)UserRoles.User;
            command.Outstandingdays = 0;
            command.VerificationStatus = true;
            command.IsSessionFlag = false;
            command.LockOutTime = null;
            command.LastPasswordChangedDate = DateTime.Now;
            //IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
            //ModelState.AddModelErrors(errors);

            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                //Add 22-02-2019
                var operatorExists = _rewardRepository.GetMany(top => top.OperatorId == OperatorId).ToList();
                if (operatorExists.Count() > 0)
                {
                    var rewardDetails = _rewardRepository.GetMany(top => top.OperatorId == OperatorId && top.RewardName.Trim().ToLower().Equals("data")).FirstOrDefault();
                    if (rewardDetails != null)
                    {
                        var userRewardCommand = new ChangeUserRewardInfoCommand
                        {
                            UserRewardId = 0,
                            UserId = result.Id,
                            RewardId = rewardDetails.RewardId,
                            OperatorId = OperatorId
                        };
                        ICommandResult userRewardResult = _commandBus.Submit(userRewardCommand);
                        if (userRewardResult.Success)
                        {
                            //return Json("success");
                        }
                    }
                }

                //Add 01-03-2019
                string a1 = "01:00";
                string a2 = "02:00";
                string a3 = "03:00";
                string a4 = "04:00";
                string a5 = "05:00";
                string a6 = "06:00";
                string a7 = "07:00";
                string a8 = "08:00";
                string a9 = "09:00";
                string a10 = "10:00";
                string a11 = "11:00";
                string a12 = "12:00";
                string a13 = "13:00";
                string a14 = "14:00";
                string a15 = "15:00";
                string a16 = "16:00";
                string a17 = "17:00";
                string a18 = "18:00";
                string a19 = "19:00";
                string a20 = "20:00";
                string a21 = "21:00";
                string a22 = "22:00";
                string a23 = "23:00";
                string a24 = "24:00";
                string[] data = { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24 };

                PostedTimesModel postedTimesModel = new PostedTimesModel();
                postedTimesModel.DayIds = data;

                UserProfileTimeSettingFormModel model = new UserProfileTimeSettingFormModel();
                model.MondayPostedTimes = postedTimesModel;
                model.TuesdayPostedTimes = postedTimesModel;
                model.WednesdayPostedTimes = postedTimesModel;
                model.ThursdayPostedTimes = postedTimesModel;
                model.FridayPostedTimes = postedTimesModel;
                model.SaturdayPostedTimes = postedTimesModel;
                model.SundayPostedTimes = postedTimesModel;

                model.OperatorId = _userRepository.GetById(result.Id).OperatorId;
                model.UserProfileId = _userProfileRepository.Get(top => top.UserId == result.Id).UserProfileId;
                CreateOrUpdateUserProfileTimeSettingCommand command1 =
                    Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

                //if (ModelState.IsValid)
                //{
                    ICommandResult result1 = _commandBus.Submit(command1);

                    if (result1.Success)
                    {
                        //return Json("success");
                    }
                //}

                User user = _userRepository.Get(u => u.Email == form.Email);

                //var reader =
                // new StreamReader(
                //     Server.MapPath(ConfigurationManager.AppSettings["UsersResendVerificationEmailTemplate"]));

                if (user.Email != null)
                {
                    string emailContent = reader.ReadToEnd();

                    Random generator = new Random();
                    String emailcode1 = generator.Next(0, 99).ToString("D2");
                    String emailcode2 = generator.Next(0, 99).ToString("D2");
                    String emailcode3 = generator.Next(0, 99).ToString("D2");

                    string verifyCode = emailcode1 + " " + emailcode2 + " " + emailcode3 + " ";


                    emailContent = string.Format(emailContent, verifyCode);


                    MailMessage mail = new MailMessage();
                    mail.To.Add(user.Email);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                    mail.Subject = "Email Verification";

                    mail.Body = emailContent;

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                    smtp.Send(mail);

                    // CheckUserExistSoapApi(user);

                    EFMVCDataContex db = new EFMVCDataContex();
                    var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == user.UserId).ToList();
                    foreach (var item in emailVerificationCode)
                    {
                        db.EmailVerificationCode.Remove(item);
                        db.SaveChanges();
                    }

                    EmailVerificationCode emailVerification = new EmailVerificationCode();

                    emailVerification.UserId = user.UserId;
                    emailVerification.VerificationCode = emailcode1 + "-" + emailcode2 + "-" + emailcode3;
                    emailVerification.DateCreated = DateTime.Now;
                    db.EmailVerificationCode.Add(emailVerification);
                    db.SaveChanges();
                }

                //if (CountryCode == "44")
                //{
                //    // CreateNumberBeginingTable(form.MSISDN); Remove this comment when it is live
                //}

                AddRecordsOnPrematch(user.UserId, user.UserMatchTableName);


                return user.UserId;
              
                //return Json(new { status = "Success", Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            #endregion
            return 0;
        }

        public int AddUserByUSSD(UserFormModel form)
        {
            #region Add User

            var OperatorId =  (int)OperatorTableId.Safaricom;
            UserMatchTableProcess objUserMatch = new UserMatchTableProcess();

            form.FirstName = "FirstName";
            form.LastName = "LastName";
            form.Email = null;
            form.Organisation = null;
            form.Password = "Pa55w0rd";
            form.OperatorId = OperatorId;
            //form.MSISDN = form.CountryCode + form.MSISDN;

            form.IsMobileVerfication = true;
            form.VerificationStatus = true;
            form.IsMsisdnMatch = true;
           
            form.UserMatchTableName = objUserMatch.GetUserMatchTableNumber(OperatorId);

            UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
            command.Activated = 1;
            command.RoleId = (Int32)UserRoles.User;
            command.Outstandingdays = 0;
            command.VerificationStatus = true;
            //IEnumerable<ValidationResult> errors = _commandBus.Validate(command);
            //ModelState.AddModelErrors(errors);

            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                //Add 22-02-2019
                var operatorExists = _rewardRepository.GetMany(top => top.OperatorId == OperatorId).ToList();
                if (operatorExists.Count() > 0)
                {
                    var rewardDetails = _rewardRepository.GetMany(top => top.OperatorId == OperatorId && top.RewardName.Trim().ToLower().Equals("data")).FirstOrDefault();
                    if (rewardDetails != null)
                    {
                        var userRewardCommand = new ChangeUserRewardInfoCommand
                        {
                            UserRewardId = 0,
                            UserId = result.Id,
                            RewardId = rewardDetails.RewardId,
                            OperatorId = OperatorId
                        };
                        ICommandResult userRewardResult = _commandBus.Submit(userRewardCommand);
                        if (userRewardResult.Success)
                        {
                            //return Json("success");
                        }
                    }
                }

                //Add 01-03-2019
                string a1 = "01:00";
                string a2 = "02:00";
                string a3 = "03:00";
                string a4 = "04:00";
                string a5 = "05:00";
                string a6 = "06:00";
                string a7 = "07:00";
                string a8 = "08:00";
                string a9 = "09:00";
                string a10 = "10:00";
                string a11 = "11:00";
                string a12 = "12:00";
                string a13 = "13:00";
                string a14 = "14:00";
                string a15 = "15:00";
                string a16 = "16:00";
                string a17 = "17:00";
                string a18 = "18:00";
                string a19 = "19:00";
                string a20 = "20:00";
                string a21 = "21:00";
                string a22 = "22:00";
                string a23 = "23:00";
                string a24 = "24:00";
                string[] data = { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24 };

                PostedTimesModel postedTimesModel = new PostedTimesModel();
                postedTimesModel.DayIds = data;

                UserProfileTimeSettingFormModel model = new UserProfileTimeSettingFormModel();
                model.MondayPostedTimes = postedTimesModel;
                model.TuesdayPostedTimes = postedTimesModel;
                model.WednesdayPostedTimes = postedTimesModel;
                model.ThursdayPostedTimes = postedTimesModel;
                model.FridayPostedTimes = postedTimesModel;
                model.SaturdayPostedTimes = postedTimesModel;
                model.SundayPostedTimes = postedTimesModel;

                model.OperatorId = _userRepository.GetById(result.Id).OperatorId;
                model.UserProfileId = _userProfileRepository.Get(top => top.UserId == result.Id).UserProfileId;
                CreateOrUpdateUserProfileTimeSettingCommand command1 =
                    Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

                //if (ModelState.IsValid)
                //{
                ICommandResult result1 = _commandBus.Submit(command1);

                if (result1.Success)
                {
                    //return Json("success");
                }
                //}

                User user = _userRepository.GetById(result.Id);
                

                // CheckUserExistSoapApi(user);

                EFMVCDataContex db = new EFMVCDataContex();
                var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == user.UserId).ToList();
                foreach (var item in emailVerificationCode)
                {
                    db.EmailVerificationCode.Remove(item);
                    db.SaveChanges();
                }                

                AddRecordsOnPrematch(user.UserId, user.UserMatchTableName);
                return user.UserId;

            }
            #endregion
            return 0;
        }

        public int AddUserBySenegalProvision(UserFormModel form)
        {
            #region Add User

            var OperatorId = (int)OperatorTableId.Expresso;
            UserMatchTableProcess objUserMatch = new UserMatchTableProcess();
            form.FirstName = "FirstName";
            form.LastName = "LastName";
            form.Email = null;
            form.Organisation = null;
            form.Password = "Pa55w0rd";
            form.OperatorId = OperatorId;
            form.IsMobileVerfication = true;
            form.VerificationStatus = true;
            form.IsMsisdnMatch = true;
            form.UserMatchTableName = objUserMatch.GetUserMatchTableNumber(OperatorId);
            UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
            command.Activated = 1;
            command.RoleId = (Int32)UserRoles.User;
            command.Outstandingdays = 0;
            command.VerificationStatus = true;
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                var operatorExists = _rewardRepository.GetMany(top => top.OperatorId == OperatorId).ToList();
                if (operatorExists.Count() > 0)
                {
                    var rewardDetails = _rewardRepository.GetMany(top => top.OperatorId == OperatorId && top.RewardName.Trim().ToLower().Equals("data")).FirstOrDefault();
                    if (rewardDetails != null)
                    {
                        var userRewardCommand = new ChangeUserRewardInfoCommand
                        {
                            UserRewardId = 0,
                            UserId = result.Id,
                            RewardId = rewardDetails.RewardId,
                            OperatorId = OperatorId
                        };
                        ICommandResult userRewardResult = _commandBus.Submit(userRewardCommand);
                        if (userRewardResult.Success)
                        {
                        }
                    }
                }
                string a1 = "01:00";
                string a2 = "02:00";
                string a3 = "03:00";
                string a4 = "04:00";
                string a5 = "05:00";
                string a6 = "06:00";
                string a7 = "07:00";
                string a8 = "08:00";
                string a9 = "09:00";
                string a10 = "10:00";
                string a11 = "11:00";
                string a12 = "12:00";
                string a13 = "13:00";
                string a14 = "14:00";
                string a15 = "15:00";
                string a16 = "16:00";
                string a17 = "17:00";
                string a18 = "18:00";
                string a19 = "19:00";
                string a20 = "20:00";
                string a21 = "21:00";
                string a22 = "22:00";
                string a23 = "23:00";
                string a24 = "24:00";
                string[] data = { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24 };
                PostedTimesModel postedTimesModel = new PostedTimesModel();
                postedTimesModel.DayIds = data;
                UserProfileTimeSettingFormModel model = new UserProfileTimeSettingFormModel();
                model.MondayPostedTimes = postedTimesModel;
                model.TuesdayPostedTimes = postedTimesModel;
                model.WednesdayPostedTimes = postedTimesModel;
                model.ThursdayPostedTimes = postedTimesModel;
                model.FridayPostedTimes = postedTimesModel;
                model.SaturdayPostedTimes = postedTimesModel;
                model.SundayPostedTimes = postedTimesModel;
                model.OperatorId = _userRepository.GetById(result.Id).OperatorId;
                model.UserProfileId = _userProfileRepository.Get(top => top.UserId == result.Id).UserProfileId;
                CreateOrUpdateUserProfileTimeSettingCommand command1 = Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);
                ICommandResult result1 = _commandBus.Submit(command1);
                if (result1.Success)
                {
                }
                User user = _userRepository.GetById(result.Id);
                EFMVCDataContex db = new EFMVCDataContex();
                var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == user.UserId).ToList();
                foreach (var item in emailVerificationCode)
                {
                    db.EmailVerificationCode.Remove(item);
                    db.SaveChanges();
                }
                AddRecordsOnPrematch(user.UserId, user.UserMatchTableName);
                return user.UserId;
            }
            #endregion
            return 0;
        }

        private void AddRecordsOnPrematch(int userId, string userMatchTableName)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            AddUserProfilePreference(db, userId, userMatchTableName);

        }

        public void UpdatePreMatchesForUser(User user)
        {
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
            if (ConnString != null && ConnString.Count > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    using (db)
                    {
                        var userData = db.Users.Include(nameof(User.UserProfiles)).FirstOrDefault(s => s.AdtoneServerUserId == user.UserId);
                        if (userData == null)
                        {
                            Trace.TraceWarning($"User is not found in provision database. Adtones UserID: [{user.UserId}]");
                            continue;
                        }
                        var profileId = user.GetUserProfile?.UserProfileId;
                        if (profileId == null)
                        {
                            Trace.TraceWarning($"User does not have a Profile in Main database. Adtones UserID: [{user.UserId}]");
                            continue;
                        }
                        var profilePref2 = db.Userprofiles.FirstOrDefault(s => s.AdtoneServerUserProfileId == profileId);
                        if (profilePref2 != null)
                        {
                            Trace.TraceInformation($"Running PreMatch rebuild for UserID: [{userData.UserId}]");
                            PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                        }
                    }
                }
            }
        }

        private void AddUserProfilePreference(EFMVCDataContex db, int userId, string userMatchTableName)
        {
            var profilePref = _profileRepository.GetMany(s => s.UserId == userId).FirstOrDefault();
            if (profilePref != null)
            {
                #region UserProfilePreference
                UserProfilePreference objPref = new UserProfilePreference();

                objPref.UserProfileId = profilePref.UserProfileId;
                objPref.Gender_Demographics = "C";
                objPref.WorkingStatus_Demographics = "I";
                objPref.RelationshipStatus_Demographics = "G";
                objPref.Education_Demographics = "E";
                objPref.HouseholdStatus_Demographics = "D";
                objPref.Location_Demographics = "A"; // null will not check on CampaignUserMatch Process
                objPref.Food_Advert = "B";
                objPref.SweetSaltySnacks_Advert = "B";
                objPref.AlcoholicDrinks_Advert = "B";
                objPref.NonAlcoholicDrinks_Advert = "B";
                objPref.Householdproducts_Advert = "B";
                objPref.ToiletriesCosmetics_Advert = "B";
                objPref.PharmaceuticalChemistsProducts_Advert = "B";
                objPref.TobaccoProducts_Advert = "B";
                objPref.PetsPetFood_Advert = "B";
                objPref.ShoppingRetailClothing_Advert = "B";
                objPref.DIYGardening_Advert = "B";
                objPref.ElectronicsOtherPersonalItems_Advert = "B";
                objPref.CommunicationsInternet_Advert = "B";
                objPref.FinancialServices_Advert = "B";
                objPref.HolidaysTravel_Advert = "B";
                objPref.SportsLeisure_Advert = "B";
                objPref.Motoring_Advert = "B";
                objPref.Newspapers_Advert = "B";
                objPref.TV_Advert = "B";
                objPref.Cinema_Advert = "B";
                objPref.SocialNetworking_Advert = "B";
                objPref.Shopping_Advert = "B";
                objPref.Fitness_Advert = "B";
                objPref.Environment_Advert = "B";
                objPref.GoingOut_Advert = "B";
                objPref.Religion_Advert = "B";
                objPref.Music_Advert = "B";
                objPref.BusinessOrOpportunities_AdType = "B";
                objPref.Gambling_AdType = "B";
                objPref.Restaurants_AdType = "B";
                objPref.Insurance_AdType = "B";
                objPref.Furniture_AdType = "B";
                objPref.InformationTechnology_AdType = "B";
                objPref.Energy_AdType = "B";
                objPref.Supermarkets_AdType = "B";
                objPref.Healthcare_AdType = "B";
                objPref.JobsAndEducation_AdType = "B";
                objPref.Gifts_AdType = "B";
                objPref.AdvocacyOrLegal_AdType = "B";
                objPref.DatingAndPersonal_AdType = "B";
                objPref.RealEstate_AdType = "B";
                objPref.Games_AdType = "B";
                objPref.Hustlers_AdType = "A";
                objPref.Youth_AdType = "A";
                objPref.DiscerningProfessionals_AdType = "A";
                objPref.Mass_AdType = "A";
                objPref.ContractType_Mobile = "A";
                objPref.Spend_Mobile = "A";

                db.UserProfilePreference.Add(objPref);
                db.SaveChanges();
                #endregion

                UserMatchTableProcess obj = new UserMatchTableProcess();
                var operatorId = _userRepository.GetById(userId).OperatorId;
                
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        db = new EFMVCDataContex(item);
                        using (db)
                        {
                            var userData = db.Users.Where(s => s.AdtoneServerUserId == userId).FirstOrDefault();

                            var profilePref2 = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == profilePref.UserProfileId).FirstOrDefault();
                            if (profilePref2 != null)
                            {
                                #region External UserProfilePreference
                                UserProfilePreference objUserPref = new UserProfilePreference();
                                var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, profilePref.UserProfileId);
                                objUserPref.UserProfileId = externalServerUserProfileId;
                                objUserPref.Gender_Demographics = "C";
                                objUserPref.WorkingStatus_Demographics = "I";
                                objUserPref.RelationshipStatus_Demographics = "G";
                                objUserPref.Education_Demographics = "E";
                                objUserPref.HouseholdStatus_Demographics = "D";
                                objUserPref.Location_Demographics = "A";
                                objUserPref.Food_Advert = "B";
                                objUserPref.SweetSaltySnacks_Advert = "B";
                                objUserPref.AlcoholicDrinks_Advert = "B";
                                objUserPref.NonAlcoholicDrinks_Advert = "B";
                                objUserPref.Householdproducts_Advert = "B";
                                objUserPref.ToiletriesCosmetics_Advert = "B";
                                objUserPref.PharmaceuticalChemistsProducts_Advert = "B";
                                objUserPref.TobaccoProducts_Advert = "B";
                                objUserPref.PetsPetFood_Advert = "B";
                                objUserPref.ShoppingRetailClothing_Advert = "B";
                                objUserPref.DIYGardening_Advert = "B";
                                objUserPref.ElectronicsOtherPersonalItems_Advert = "B";
                                objUserPref.CommunicationsInternet_Advert = "B";
                                objUserPref.FinancialServices_Advert = "B";
                                objUserPref.HolidaysTravel_Advert = "B";
                                objUserPref.SportsLeisure_Advert = "B";
                                objUserPref.Motoring_Advert = "B";
                                objUserPref.Newspapers_Advert = "B";
                                objUserPref.TV_Advert = "B";
                                objUserPref.Cinema_Advert = "B";
                                objUserPref.SocialNetworking_Advert = "B";
                                objUserPref.Shopping_Advert = "B";
                                objUserPref.Fitness_Advert = "B";
                                objUserPref.Environment_Advert = "B";
                                objUserPref.GoingOut_Advert = "B";
                                objUserPref.Religion_Advert = "B";
                                objUserPref.Music_Advert = "B";
                                objUserPref.BusinessOrOpportunities_AdType = "B";
                                objUserPref.Gambling_AdType = "B";
                                objUserPref.Restaurants_AdType = "B";
                                objUserPref.Insurance_AdType = "B";
                                objUserPref.Furniture_AdType = "B";
                                objUserPref.InformationTechnology_AdType = "B";
                                objUserPref.Energy_AdType = "B";
                                objUserPref.Supermarkets_AdType = "B";
                                objUserPref.Healthcare_AdType = "B";
                                objUserPref.JobsAndEducation_AdType = "B";
                                objUserPref.Gifts_AdType = "B";
                                objUserPref.AdvocacyOrLegal_AdType = "B";
                                objUserPref.DatingAndPersonal_AdType = "B";
                                objUserPref.RealEstate_AdType = "B";
                                objUserPref.Games_AdType = "B";
                                objUserPref.Hustlers_AdType = "A";
                                objUserPref.Youth_AdType = "A";
                                objUserPref.DiscerningProfessionals_AdType = "A";
                                objUserPref.Mass_AdType = "A";
                                objUserPref.ContractType_Mobile = "A";
                                objUserPref.Spend_Mobile = "A";

                                db.UserProfilePreference.Add(objUserPref);
                                db.SaveChanges();
                                #endregion
                                obj.AddUserMatchData(userData.UserMatchTableName, profilePref2, userData, db);
                                PreMatchProcess.PreCampaignUsermatchProcess(userData.UserId, userData.UserMatchTableName, item);
                            }

                        }
                    }
                }

            }

        }

    }
}