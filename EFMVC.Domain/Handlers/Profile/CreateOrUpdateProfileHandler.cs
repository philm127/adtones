// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateProfileHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System.Linq;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateProfileHandler.
    /// </summary>
    public class CreateOrUpdateProfileHandler : ICommandHandler<CreateOrUpdateProfileCommand>
    {
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateProfileHandler"/> class.
        /// </summary>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateProfileHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateProfileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateProfileCommand command)
        {
            var profile = new UserProfile
                              {
                                  UserProfileId = command.UserProfileId,
                                  DOB = command.DOB,
                                  Education = command.Education,
                                  Gender = command.Gender,
                                  HouseholdStatus = command.HouseholdStatus,
                                  IncomeBracket = command.IncomeBracket,
                                  Location = command.Location,
                                  MSISDN = command.MSISDN,
                                  RelationshipStatus = command.RelationshipStatus,
                                  UserId = command.UserId,
                                  WorkingStatus = command.WorkingStatus,
                                  
            };

            //TranslateUserProfileAdverts(command, profile);
            //TranslateUserProfileAttitudes(command, profile);
            //TranslateUserProfileCinema(command, profile);
            //TranslateUserProfileInternet(command, profile);
            //TranslateUserProfileMobile(command, profile);
            //TranslateUserProfilePress(command, profile);
            //TranslateUserProfileProductServices(command, profile);
            //TranslateUserProfileRadio(command, profile);
            TranslateUserProfileTimeSettings(command, profile);
            //TranslateUserProfileTv(command, profile);

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (profile.UserProfileId == 0)
            {
                _profileRepository.Add(profile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        if(externalServerUserId != 0)
                        {
                            var profile2 = new UserProfile
                            {
                                UserProfileId = command.UserProfileId,
                                DOB = command.DOB,
                                Education = command.Education,
                                Gender = command.Gender,
                                HouseholdStatus = command.HouseholdStatus,
                                IncomeBracket = command.IncomeBracket,
                                Location = command.Location,
                                MSISDN = command.MSISDN,
                                RelationshipStatus = command.RelationshipStatus,
                                UserId = command.UserId,
                                WorkingStatus = command.WorkingStatus,
                                AdtoneServerUserProfileId = profile.UserProfileId
                            };
                            db.Userprofiles.Add(profile2);
                            db.SaveChanges();
                        }
                        
                    }                      
                    
                }
              
                return new CommandResult(true,profile.UserProfileId);
            }
            else
            {
                _profileRepository.Update(profile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userProfileResult = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == profile.UserProfileId).FirstOrDefault();
                        if (userProfileResult != null)
                        {
                            // userProfileResult.UserProfileId = command.UserProfileId;
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            if (externalServerUserId != 0)
                            {
                                userProfileResult.DOB = command.DOB;
                                userProfileResult.Education = command.Education;
                                userProfileResult.Gender = command.Gender;
                                userProfileResult.HouseholdStatus = command.HouseholdStatus;
                                userProfileResult.IncomeBracket = command.IncomeBracket;
                                userProfileResult.Location = command.Location;
                                userProfileResult.MSISDN = command.MSISDN;
                                userProfileResult.RelationshipStatus = command.RelationshipStatus;
                                userProfileResult.UserId = externalServerUserId;
                                userProfileResult.WorkingStatus = command.WorkingStatus;
                                db.SaveChanges();
                            }
                                
                        }
                    }
                   
                }
                
                return new CommandResult(true, profile.UserProfileId);
            }
          

          
        }

        #endregion

        /// <summary>
        /// Translates the user profile attitudes.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileAttitudes(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileAttitudeCommand attitude in command.UserProfileAttitudes)
        //    {
        //        profile.UserProfileAttitudes.Add(new UserProfileAttitude
        //                                             {
        //                                                 Environment = attitude.Environment,
        //                                                 Fashion = attitude.Fashion,
        //                                                 FinancialStabiity = attitude.FinancialStabiity,
        //                                                 Fitness = attitude.Fitness,
        //                                                 GoingOut = attitude.GoingOut,
        //                                                 Holidays = attitude.Holidays,
        //                                                 Music = attitude.Music,
        //                                                 Religion = attitude.Religion,
        //                                                 UserProfileAttitudeId = attitude.UserProfileAttitudeId,
        //                                             });
        //    }
        //}

        /// <summary>
        /// Translates the user profile adverts.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileAdverts(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileAdvertCommand advert in command.UserProfileAdverts)
        //    {
        //        profile.UserProfileAdverts.Add(new UserProfileAdvert
        //                                           {
        //                                               AppliancesOtherHouseholdDurables =
        //                                                   advert.AppliancesOtherHouseholdDurables,
        //                                               AlcoholicDrinks = advert.AlcoholicDrinks,
        //                                               Cinema = advert.Cinema,
        //                                               CommunicationsInternet = advert.CommunicationsInternet,
        //                                               DIYGardening = advert.DIYGardening,
        //                                               ElectronicsOtherPersonalItems =
        //                                                   advert.ElectronicsOtherPersonalItems,
        //                                               Environment = advert.Environment,
        //                                               Fashion = advert.Fashion,
        //                                               FinancialProducts = advert.FinancialProducts,
        //                                               FinancialServices = advert.FinancialServices,
        //                                               Fitness = advert.Fitness,
        //                                               Food = advert.Food,
        //                                               GeneralUse = advert.GeneralUse,
        //                                               GoingOut = advert.GoingOut,
        //                                               Holidays = advert.Holidays,
        //                                               HolidaysTravel = advert.HolidaysTravel,
        //                                               Householdproducts = advert.Householdproducts,
        //                                               Magazines = advert.Magazines,
        //                                               Motoring = advert.Motoring,
        //                                               Music = advert.Music,
        //                                               Newspapers = advert.Newspapers,
        //                                               NonAlcoholicDrinks = advert.NonAlcoholicDrinks,
        //                                               PetsPetFood = advert.PetsPetFood,
        //                                               PharmaceuticalChemistsProducts =
        //                                                   advert.PharmaceuticalChemistsProducts,
        //                                               Radio = advert.Radio,
        //                                               Religion = advert.Religion,
        //                                               Shopping = advert.Shopping,
        //                                               ShoppingRetailClothing = advert.ShoppingRetailClothing,
        //                                               SocialNetworking = advert.SocialNetworking,
        //                                               SportsLeisure = advert.SportsLeisure,
        //                                               SweetSaltySnacks = advert.SweetSaltySnacks,
        //                                               TV = advert.TV,
        //                                               TobaccoProducts = advert.TobaccoProducts,
        //                                               ToiletriesCosmetics = advert.ToiletriesCosmetics
        //                                           });
        //    }
        //}

        /// <summary>
        /// Translates the user profile cinema.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileCinema(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileCinemaCommand cinema in command.UserProfileCinemas)
        //    {
        //        profile.UserProfileCinemas.Add(new UserProfileCinema
        //                                           {
        //                                               Cinema = cinema.Cinema
        //                                           });
        //    }
        //}

        /// <summary>
        /// Translates the user profile internet.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileInternet(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileInternetCommand internet in command.UserProfileInternets)
        //    {
        //        profile.UserProfileInternets.Add(new UserProfileInternet
        //                                             {
        //                                                 Auctions = internet.Auctions,
        //                                                 Research = internet.Research,
        //                                                 Shopping = internet.Shopping,
        //                                                 SocialNetworking = internet.SocialNetworking,
        //                                                 Video = internet.Video
        //                                             });
        //    }
        //}

        /// <summary>
        /// Translates the user profile mobile.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileMobile(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileMobileCommand mobile in command.UserProfileMobiles)
        //    {
        //        profile.UserProfileMobiles.Add(new UserProfileMobile
        //                                           {
        //                                               ContractType = mobile.ContractType,
        //                                               Spend = mobile.Spend
        //                                           });
        //    }
        //}

        /// <summary>
        /// Translates the user profile press.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfilePress(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfilePressCommand press in command.UserProfilePresses)
        //    {
        //        profile.UserProfilePresses.Add(new UserProfilePress
        //                                           {
        //                                               FreeNewpapers = press.FreeNewpapers,
        //                                               Local = press.Local,
        //                                               Magazines = press.Magazines,
        //                                               National = press.National
        //                                           });
        //    }
        //}

        /// <summary>
        /// Translates the user profile product services.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileProductServices(CreateOrUpdateProfileCommand command,
        //                                                        UserProfile profile)
        //{
        //    foreach (
        //        CreateOrUpdateUserProfileProductsServiceCommand productService in command.UserProfileProductsServices)
        //    {
        //        profile.UserProfileProductsServices.Add(new UserProfileProductsService
        //                                                    {
        //                                                        AlcoholicDrinks = productService.AlcoholicDrinks,
        //                                                        AppliancesOtherHouseholdDurables =
        //                                                            productService.AppliancesOtherHouseholdDurables,
        //                                                        CommunicationsInternet =
        //                                                            productService.CommunicationsInternet,
        //                                                        DIYGardening = productService.DIYGardening,
        //                                                        ElectronicsOtherPersonalItems =
        //                                                            productService.ElectronicsOtherPersonalItems,
        //                                                        FinancialServices = productService.FinancialServices,
        //                                                        Food = productService.Food,
        //                                                        HolidaysTravel = productService.HolidaysTravel,
        //                                                        Householdproducts = productService.Householdproducts,
        //                                                        Motoring = productService.Motoring,
        //                                                        NonAlcoholicDrinks = productService.NonAlcoholicDrinks,
        //                                                        PetsPetFood = productService.PetsPetFood,
        //                                                        PharmaceuticalChemistsProducts =
        //                                                            productService.PharmaceuticalChemistsProducts,
        //                                                        ShoppingRetailClothing =
        //                                                            productService.ShoppingRetailClothing,
        //                                                        SportsLeisure = productService.SportsLeisure,
        //                                                        SweetSaltySnacks = productService.SweetSaltySnacks,
        //                                                        TobaccoProducts = productService.TobaccoProducts,
        //                                                        ToiletriesCosmetics = productService.ToiletriesCosmetics
        //                                                    });
        //    }
        //}

        /// <summary>
        /// Translates the user profile radio.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileRadio(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileRadioCommand radio in command.UserProfileRadios)
        //    {
        //        profile.UserProfileRadios.Add(new UserProfileRadio
        //                                          {
        //                                              Local = radio.Local,
        //                                              Music = radio.Music,
        //                                              National = radio.National,
        //                                              Sport = radio.Sport,
        //                                              Talk = radio.Talk
        //                                          });
        //    }
        //}

        /// <summary>
        /// Translates the user profile time settings.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        private static void TranslateUserProfileTimeSettings(CreateOrUpdateProfileCommand command, UserProfile profile)
        {
            foreach (CreateOrUpdateUserProfileTimeSettingCommand time in command.UserProfileTimeSettings)
            {
                var timeSettings = new UserProfileTimeSetting();

                timeSettings.Monday = string.Empty;
                foreach (string s in time.MondayPostedTimes.DayIds)
                    timeSettings.Monday += s + ",";
                timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);

                timeSettings.Tuesday = string.Empty;
                foreach (string s in time.TuesdayPostedTimes.DayIds)
                    timeSettings.Tuesday += s + ",";
                timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);

                timeSettings.Wednesday = string.Empty;
                foreach (string s in time.WednesdayPostedTimes.DayIds)
                    timeSettings.Wednesday += s + ",";
                timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);

                timeSettings.Thursday = string.Empty;
                foreach (string s in time.ThursdayPostedTimes.DayIds)
                    timeSettings.Thursday += s + ",";
                timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);

                timeSettings.Friday = string.Empty;
                foreach (string s in time.FridayPostedTimes.DayIds)
                    timeSettings.Friday += s + ",";
                timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);

                timeSettings.Saturday = string.Empty;
                foreach (string s in time.SaturdayPostedTimes.DayIds)
                    timeSettings.Saturday += s + ",";
                timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);

                timeSettings.Sunday = string.Empty;
                foreach (string s in time.SundayPostedTimes.DayIds)
                    timeSettings.Sunday += s + ",";
                timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);

                profile.UserProfileTimeSettings.Add(timeSettings);
            }
        }

        /// <summary>
        /// Translates the user profile tv.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateUserProfileTv(CreateOrUpdateProfileCommand command, UserProfile profile)
        //{
        //    foreach (CreateOrUpdateUserProfileTvCommand tv in command.UserProfileTvs)
        //    {
        //        profile.UserProfileTvs.Add(new UserProfileTv
        //                                       {
        //                                           Cable = tv.Cable,
        //                                           Internet = tv.Internet,
        //                                           Satallite = tv.Satallite,
        //                                           Terrestrial = tv.Terrestrial
        //                                       });
        //    }
        //}
    }
}