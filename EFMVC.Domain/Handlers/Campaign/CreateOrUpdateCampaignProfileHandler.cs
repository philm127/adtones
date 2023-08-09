// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.ObjectModel;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using System.Linq;
using EFMVC.Domain.OperatorServerData;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateCampaignProfileHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileHandler : ICommandHandler<CreateOrUpdateCampaignProfileCommand>
    {
        /// <summary>
        /// The _profile campaign repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileCampaignRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileHandler"/> class.
        /// </summary>
        /// <param name="profileCampaignRepository">The profile campaign repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileHandler(ICampaignProfileRepository profileCampaignRepository,
                                                    IUnitOfWork unitOfWork)
        {
            _profileCampaignRepository = profileCampaignRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        ///
        public ICommandResult Execute(CreateOrUpdateCampaignProfileCommand command)
        {
            CampaignProfile profile = null;
            //EFMVCDataContex db = new EFMVCDataContex();
            if (command.CampaignProfileId != 0)
            {
                
                profile = _profileCampaignRepository.GetById(command.CampaignProfileId);                
                if (profile != null)
                {

                    profile.ClientId = command.ClientId;
                    profile.Active = command.Active;
                    profile.AvailableCredit = command.AvailableCredit;
                    profile.CampaignDescription = command.CampaignDescription;
                    profile.CampaignName = command.CampaignName;
                    profile.CancelledCurrentMonth = command.CancelledCurrentMonth;
                    profile.CancelledLastMonth = command.CancelledLastMonth;
                    profile.CancelledToDate = command.CancelledToDate;
                    profile.CreatedDateTime = command.CreatedDateTime;
                    profile.StartDate = command.StartDate;
                    profile.EndDate = command.EndDate;
                    profile.EmailToDate = command.EmailToDate;
                    profile.EmailsCurrentMonth = command.EmailsCurrentMonth;
                    profile.EmailsLastMonth = command.EmailsLastMonth;
                    profile.MaxBid = command.MaxBid;
                    profile.MaxMonthBudget = command.MaxMonthBudget;
                    profile.MaxWeeklyBudget = command.MaxWeeklyBudget;
                    profile.MaxHourlyBudget = command.MaxHourlyBudget;
                    profile.TotalCredit = command.TotalCredit;
                    profile.SpendToDate = command.SpendToDate;
                    profile.MaxDailyBudget = command.MaxDailyBudget;
                    profile.PlaysCurrentMonth = command.PlaysCurrentMonth;
                    profile.PlaysLastMonth = command.PlaysLastMonth;
                    profile.PlaysToDate = command.PlaysToDate;
                    profile.SmsCurrentMonth = command.SmsCurrentMonth;
                    profile.SmsLastMonth = command.SmsLastMonth;
                    profile.SmsToDate = command.SmsToDate;
                    profile.TotalBudget = command.TotalBudget;
                    profile.UpdatedDateTime = command.UpdatedDateTime;
                    profile.UserId = command.UserId;
                    profile.EmailBody = command.EmailBody;
                    profile.EmailSenderAddress = command.EmailSenderAddress;
                    profile.EmailSubject = command.EmailSubject;
                    profile.SmsBody = command.SmsBody;
                    profile.SmsOriginator = command.SmsOriginator;
                    profile.EmailFileLocation = command.EmailFileLocation;
                    profile.SMSFileLocation = command.SMSFileLocation;
                    profile.Status = command.Status;
                    profile.NumberInBatch = command.NumberInBatch;
                    profile.IsAdminApproval = command.IsAdminApproval;
                    profile.NextStatus = command.NextStatus;
                    if(command.CountryId != 0)
                        profile.CountryId = command.CountryId;
                    profile.CurrencyCode = command.CurrencyCode;
                }
            }
            else
            {
                profile = new CampaignProfile
                {
                    CampaignProfileId = command.CampaignProfileId,
                    Active = command.Active,
                    ClientId = command.ClientId,
                    Status = command.Status,
                    AvailableCredit = command.AvailableCredit,
                    CampaignDescription = command.CampaignDescription,
                    CampaignName = command.CampaignName,
                    CancelledCurrentMonth = command.CancelledCurrentMonth,
                    CancelledLastMonth = command.CancelledLastMonth,
                    CancelledToDate = command.CancelledToDate,
                    CreatedDateTime = command.CreatedDateTime,
                    StartDate = command.StartDate,
                    EndDate = command.EndDate,
                    EmailToDate = command.EmailToDate,
                    EmailsCurrentMonth = command.EmailsCurrentMonth,
                    EmailsLastMonth = command.EmailsLastMonth,
                    MaxBid = command.MaxBid,
                    MaxMonthBudget = command.MaxMonthBudget,
                    MaxWeeklyBudget = command.MaxWeeklyBudget,
                    MaxHourlyBudget = command.MaxHourlyBudget,
                    TotalCredit = command.TotalCredit,
                    SpendToDate = command.SpendToDate,
                    MaxDailyBudget = command.MaxDailyBudget,
                    PlaysCurrentMonth = command.PlaysCurrentMonth,
                    PlaysLastMonth = command.PlaysLastMonth,
                    PlaysToDate = command.PlaysToDate,
                    SmsCurrentMonth = command.SmsCurrentMonth,
                    SmsLastMonth = command.SmsLastMonth,
                    EmailFileLocation = command.EmailFileLocation,
                    SMSFileLocation = command.SMSFileLocation,
                    SmsToDate = command.SmsToDate,
                    TotalBudget = command.TotalBudget,
                    UpdatedDateTime = command.UpdatedDateTime,
                    UserId = command.UserId,
                    NumberInBatch = command.NumberInBatch,
                    CountryId = command.CountryId,
                    IsAdminApproval = command.IsAdminApproval,
                    NextStatus = command.NextStatus,
                    CurrencyCode = command.CurrencyCode,
                    CampaignProfileAdverts = new Collection<CampaignProfileAdvert> { new CampaignProfileAdvert() },
                    CampaignProfileAttitudes = new Collection<CampaignProfileAttitude> { new CampaignProfileAttitude() },
                    CampaignProfileCinemas = new Collection<CampaignProfileCinema> { new CampaignProfileCinema() },
                    CampaignProfileDemographics = new Collection<CampaignProfileDemographics> { new CampaignProfileDemographics() },
                    CampaignProfileInternets = new Collection<CampaignProfileInternet> { new CampaignProfileInternet() },
                    CampaignProfileMobiles = new Collection<CampaignProfileMobile> { new CampaignProfileMobile() },
                    CampaignProfilePresses = new Collection<CampaignProfilePress> { new CampaignProfilePress() },
                    CampaignProfileProductsServices = new Collection<CampaignProfileProductsService> { new CampaignProfileProductsService() },
                    CampaignProfileRadios = new Collection<CampaignProfileRadio> { new CampaignProfileRadio() },
                    CampaignProfileTimeSettings = new Collection<CampaignProfileTimeSetting> { new CampaignProfileTimeSetting() },
                    CampaignProfileTvs = new Collection<CampaignProfileTv> { new CampaignProfileTv() }
                };
            }
            
            var ConnString = ConnectionString.GetConnectionStringByCountryId(profile.CountryId);
            if (profile.CampaignProfileId == 0)
            {
                _profileCampaignRepository.Add(profile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach(var item in ConnString)
                    {
                        int clientId = 0;
                        if (command.ClientId == null)
                        {
                            clientId = 0;
                        }
                        else
                        {
                            clientId = command.ClientId.Value;
                        }

                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, profile.UserId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, (int)profile.CountryId);

                        if (externalServerUserId != 0 && externalServerCountryId != 0)//externalServerClientId != 0 && 
                        {
                            int? operatorClientId;
                            if (externalServerClientId == 0)
                            {
                                operatorClientId = null;
                            }
                            else
                            {
                                operatorClientId = externalServerClientId;
                            }

                            var profile2 = new CampaignProfile
                            {
                                CampaignProfileId = command.CampaignProfileId,
                                Active = command.Active,
                                ClientId = operatorClientId,
                                Status = command.Status,
                                AvailableCredit = command.AvailableCredit,
                                CampaignDescription = command.CampaignDescription,
                                CampaignName = command.CampaignName,
                                CancelledCurrentMonth = command.CancelledCurrentMonth,
                                CancelledLastMonth = command.CancelledLastMonth,
                                CancelledToDate = command.CancelledToDate,
                                CreatedDateTime = command.CreatedDateTime,
                                StartDate = command.StartDate,
                                EndDate = command.EndDate,
                                EmailToDate = command.EmailToDate,
                                EmailsCurrentMonth = command.EmailsCurrentMonth,
                                EmailsLastMonth = command.EmailsLastMonth,
                                MaxBid = command.MaxBid,
                                MaxMonthBudget = command.MaxMonthBudget,
                                MaxWeeklyBudget = command.MaxWeeklyBudget,
                                MaxHourlyBudget = command.MaxHourlyBudget,
                                TotalCredit = command.TotalCredit,
                                SpendToDate = command.SpendToDate,
                                MaxDailyBudget = command.MaxDailyBudget,
                                PlaysCurrentMonth = command.PlaysCurrentMonth,
                                PlaysLastMonth = command.PlaysLastMonth,
                                PlaysToDate = command.PlaysToDate,
                                SmsCurrentMonth = command.SmsCurrentMonth,
                                SmsLastMonth = command.SmsLastMonth,
                                EmailFileLocation = command.EmailFileLocation,
                                SMSFileLocation = command.SMSFileLocation,
                                SmsToDate = command.SmsToDate,
                                TotalBudget = command.TotalBudget,
                                UpdatedDateTime = command.UpdatedDateTime,
                                UserId = externalServerUserId,
                                NumberInBatch = command.NumberInBatch,
                                CountryId = externalServerCountryId,
                                IsAdminApproval = command.IsAdminApproval,
                                NextStatus = command.NextStatus,
                                CurrencyCode = command.CurrencyCode,
                                AdtoneServerCampaignProfileId = profile.CampaignProfileId,
                                CampaignProfileAdverts = new Collection<CampaignProfileAdvert> { new CampaignProfileAdvert() },
                                CampaignProfileAttitudes = new Collection<CampaignProfileAttitude> { new CampaignProfileAttitude() },
                                CampaignProfileCinemas = new Collection<CampaignProfileCinema> { new CampaignProfileCinema() },
                                CampaignProfileDemographics = new Collection<CampaignProfileDemographics> { new CampaignProfileDemographics() },
                                CampaignProfileInternets = new Collection<CampaignProfileInternet> { new CampaignProfileInternet() },
                                CampaignProfileMobiles = new Collection<CampaignProfileMobile> { new CampaignProfileMobile() },
                                CampaignProfilePresses = new Collection<CampaignProfilePress> { new CampaignProfilePress() },
                                CampaignProfileProductsServices = new Collection<CampaignProfileProductsService> { new CampaignProfileProductsService() },
                                CampaignProfileRadios = new Collection<CampaignProfileRadio> { new CampaignProfileRadio() },
                                //CampaignProfileTimeSettings = new Collection<CampaignProfileTimeSetting> { new CampaignProfileTimeSetting() },
                                CampaignProfileTvs = new Collection<CampaignProfileTv> { new CampaignProfileTv() }
                            };
                            db.CampaignProfiles.Add(profile2);
                            db.SaveChanges();

                            //Add 25-03-2019
                            var timeSetting2 = new CampaignProfileTimeSetting();
                            timeSetting2.Monday = null;
                            timeSetting2.Tuesday = null;
                            timeSetting2.Wednesday = null;
                            timeSetting2.Thursday = null;
                            timeSetting2.Friday = null;
                            timeSetting2.Saturday = null;
                            timeSetting2.Sunday = null;
                            timeSetting2.CampaignProfileId = profile2.CampaignProfileId;
                            timeSetting2.AdtoneServerCampaignProfileTimeId = profile.CampaignProfileTimeSettings.FirstOrDefault().CampaignProfileTimeSettingsId;

                            db.CampaignProfileTimeSettings.Add(timeSetting2);
                            db.SaveChanges();
                        }
                      
                    }                   
                }
            }
            else
            {
                _profileCampaignRepository.Update(profile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campProfile = db.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == command.CampaignProfileId).FirstOrDefault();
                        if (campProfile != null)
                        {
                            int clientId = 0;
                            if (command.ClientId == null)
                            {
                                clientId = 0;
                            }
                            else
                            {
                                clientId = command.ClientId.Value;
                            }
                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, profile.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, (int)profile.CountryId);

                            if(externalServerUserId != 0 && externalServerCountryId != 0)//externalServerClientId != 0 && 
                            {
                                int? operatorClientId;
                                if (externalServerClientId == 0)
                                {
                                    operatorClientId = null;
                                }
                                else
                                {
                                    operatorClientId = externalServerClientId;
                                }

                                campProfile.ClientId = operatorClientId;
                                campProfile.Active = command.Active;
                                campProfile.AvailableCredit = command.AvailableCredit;
                                campProfile.CampaignDescription = command.CampaignDescription;
                                campProfile.CampaignName = command.CampaignName;
                                campProfile.CancelledCurrentMonth = command.CancelledCurrentMonth;
                                campProfile.CancelledLastMonth = command.CancelledLastMonth;
                                campProfile.CancelledToDate = command.CancelledToDate;
                                campProfile.CreatedDateTime = command.CreatedDateTime;
                                campProfile.StartDate = command.StartDate;
                                campProfile.EndDate = command.EndDate;
                                campProfile.EmailToDate = command.EmailToDate;
                                campProfile.EmailsCurrentMonth = command.EmailsCurrentMonth;
                                campProfile.EmailsLastMonth = command.EmailsLastMonth;
                                campProfile.MaxBid = command.MaxBid;
                                campProfile.MaxMonthBudget = command.MaxMonthBudget;
                                campProfile.MaxWeeklyBudget = command.MaxWeeklyBudget;
                                campProfile.MaxHourlyBudget = command.MaxHourlyBudget;
                                campProfile.TotalCredit = command.TotalCredit;
                                campProfile.SpendToDate = command.SpendToDate;
                                campProfile.MaxDailyBudget = command.MaxDailyBudget;
                                campProfile.PlaysCurrentMonth = command.PlaysCurrentMonth;
                                campProfile.PlaysLastMonth = command.PlaysLastMonth;
                                campProfile.PlaysToDate = command.PlaysToDate;
                                campProfile.SmsCurrentMonth = command.SmsCurrentMonth;
                                campProfile.SmsLastMonth = command.SmsLastMonth;
                                campProfile.SmsToDate = command.SmsToDate;
                                campProfile.TotalBudget = command.TotalBudget;
                                campProfile.UpdatedDateTime = command.UpdatedDateTime;
                                campProfile.UserId = externalServerUserId;
                                campProfile.EmailBody = command.EmailBody;
                                campProfile.EmailSenderAddress = command.EmailSenderAddress;
                                campProfile.EmailSubject = command.EmailSubject;
                                campProfile.SmsBody = command.SmsBody;
                                campProfile.SmsOriginator = command.SmsOriginator;
                                campProfile.EmailFileLocation = command.EmailFileLocation;
                                campProfile.SMSFileLocation = command.SMSFileLocation;
                                campProfile.Status = command.Status;
                                campProfile.NumberInBatch = command.NumberInBatch;
                                campProfile.CountryId = externalServerCountryId;
                                campProfile.NextStatus = command.NextStatus;
                                campProfile.CurrencyCode = command.CurrencyCode;
                                db.SaveChanges();
                            }
                          
                        }
                    }
                }             
            }

          

            return new CommandResult(true, profile.CampaignProfileId);
        }



        #endregion

        /// <summary>
        /// Translates the campaign date settings.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        private void TranslateCampaignDateSettings(CreateOrUpdateCampaignProfileCommand command, CampaignProfile profile)
        {
            foreach (CreateOrUpdateCampaignDateSettingsCommand date in command.CampaignProfileDateSettings)
            {
                profile.CampaignProfileDateSettings.Add(new CampaignProfileDateSettings
                {
                    Active = date.Active,
                    CampaignDate = date.CampaignDate,
                    CampaignDateSettingsId = date.CampaignDateSettingsId,
                    CampaignProfileId = date.CampaignProfileId
                });
            }
        }

        /// <summary>
        /// Translates the campaign adverts.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private void TranslateCampaignAdverts(CreateOrUpdateCampaignProfileCommand command, CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignAdvertCommand advert in command.CampaignAdverts)
        //    {
        //        profile.CampaignAdverts.Add(new CampaignAdvert
        //        {
        //            CampaignAdvertId = advert.CampaignAdvertId,
        //            CampaignProfileId = advert.CampaignProfileId
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile attitudes.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileAttitudes(CreateOrUpdateCampaignProfileCommand command,
        //                                                      CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileAttitudeCommand attitude in command.CampaignProfileAttitudes)
        //    {
        //        profile.CampaignProfileAttitudes.Add(new CampaignProfileAttitude
        //        {
        //            Environment = attitude.Environment,
        //            Fashion = attitude.Fashion,
        //            FinancialStabiity = attitude.FinancialStabiity,
        //            Fitness = attitude.Fitness,
        //            GoingOut = attitude.GoingOut,
        //            Holidays = attitude.Holidays,
        //            Music = attitude.Music,
        //            Religion = attitude.Religion,
        //            CampaignProfileAttitudeId =
        //                                                         attitude.CampaignProfileAttitudeId,
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile adverts.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileAdverts(CreateOrUpdateCampaignProfileCommand command,
        //                                                    CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileAdvertCommand advert in command.CampaignProfileAdverts)
        //    {
        //        profile.CampaignProfileAdverts.Add(new CampaignProfileAdvert
        //        {
        //            AppliancesOtherHouseholdDurables =
        //                                                       advert.AppliancesOtherHouseholdDurables,
        //            AlcoholicDrinks = advert.AlcoholicDrinks,
        //            Cinema = advert.Cinema,
        //            CommunicationsInternet = advert.CommunicationsInternet,
        //            DIYGardening = advert.DIYGardening,
        //            ElectronicsOtherPersonalItems =
        //                                                       advert.ElectronicsOtherPersonalItems,
        //            Environment = advert.Environment,
        //            Fashion = advert.Fashion,
        //            FinancialProducts = advert.FinancialProducts,
        //            FinancialServices = advert.FinancialServices,
        //            Fitness = advert.Fitness,
        //            Food = advert.Food,
        //            GeneralUse = advert.GeneralUse,
        //            GoingOut = advert.GoingOut,
        //            Holidays = advert.Holidays,
        //            HolidaysTravel = advert.HolidaysTravel,
        //            Householdproducts = advert.Householdproducts,
        //            Magazines = advert.Magazines,
        //            Motoring = advert.Motoring,
        //            Music = advert.Music,
        //            Newspapers = advert.Newspapers,
        //            NonAlcoholicDrinks = advert.NonAlcoholicDrinks,
        //            PetsPetFood = advert.PetsPetFood,
        //            PharmaceuticalChemistsProducts =
        //                                                       advert.PharmaceuticalChemistsProducts,
        //            Radio = advert.Radio,
        //            Religion = advert.Religion,
        //            Shopping = advert.Shopping,
        //            ShoppingRetailClothing = advert.ShoppingRetailClothing,
        //            SocialNetworking = advert.SocialNetworking,
        //            SportsLeisure = advert.SportsLeisure,
        //            SweetSaltySnacks = advert.SweetSaltySnacks,
        //            TV = advert.TV,
        //            TobaccoProducts = advert.TobaccoProducts,
        //            ToiletriesCosmetics = advert.ToiletriesCosmetics
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile cinema.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileCinema(CreateOrUpdateCampaignProfileCommand command,
        //                                                   CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileCinemaCommand cinema in command.CampaignProfileCinemas)
        //    {
        //        profile.CampaignProfileCinemas.Add(new CampaignProfileCinema
        //        {
        //            Cinema = cinema.Cinema
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile internet.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileInternet(CreateOrUpdateCampaignProfileCommand command,
        //                                                     CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileInternetCommand internet in command.CampaignProfileInternets)
        //    {
        //        profile.CampaignProfileInternets.Add(new CampaignProfileInternet
        //        {
        //            Auctions = internet.Auctions,
        //            Research = internet.Research,
        //            Shopping = internet.Shopping,
        //            SocialNetworking = internet.SocialNetworking,
        //            Video = internet.Video
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile mobile.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileMobile(CreateOrUpdateCampaignProfileCommand command,
        //                                                   CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileMobileCommand mobile in command.CampaignProfileMobiles)
        //    {
        //        profile.CampaignProfileMobiles.Add(new CampaignProfileMobile
        //        {
        //            ContractType = mobile.ContractType,
        //            Spend = mobile.Spend
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile press.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfilePress(CreateOrUpdateCampaignProfileCommand command,
        //                                                  CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfilePressCommand press in command.CampaignProfilePresses)
        //    {
        //        profile.CampaignProfilePresses.Add(new CampaignProfilePress
        //        {
        //            FreeNewpapers = press.FreeNewpapers,
        //            Local = press.Local,
        //            Magazines = press.Magazines,
        //            National = press.National
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile product services.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileProductServices(CreateOrUpdateCampaignProfileCommand command,
        //                                                            CampaignProfile profile)
        //{
        //    foreach (
        //        CreateOrUpdateCampaignProfileProductsServiceCommand productService in
        //            command.CampaignProfileProductsServices)
        //    {
        //        profile.CampaignProfileProductsServices.Add(new CampaignProfileProductsService
        //        {
        //            AlcoholicDrinks = productService.AlcoholicDrinks,
        //            AppliancesOtherHouseholdDurables =
        //                                                                productService.AppliancesOtherHouseholdDurables,
        //            CommunicationsInternet =
        //                                                                productService.CommunicationsInternet,
        //            DIYGardening = productService.DIYGardening,
        //            ElectronicsOtherPersonalItems =
        //                                                                productService.ElectronicsOtherPersonalItems,
        //            FinancialServices = productService.FinancialServices,
        //            Food = productService.Food,
        //            HolidaysTravel = productService.HolidaysTravel,
        //            Householdproducts = productService.Householdproducts,
        //            Motoring = productService.Motoring,
        //            NonAlcoholicDrinks =
        //                                                                productService.NonAlcoholicDrinks,
        //            PetsPetFood = productService.PetsPetFood,
        //            PharmaceuticalChemistsProducts =
        //                                                                productService.PharmaceuticalChemistsProducts,
        //            ShoppingRetailClothing =
        //                                                                productService.ShoppingRetailClothing,
        //            SportsLeisure = productService.SportsLeisure,
        //            SweetSaltySnacks = productService.SweetSaltySnacks,
        //            TobaccoProducts = productService.TobaccoProducts,
        //            ToiletriesCosmetics =
        //                                                                productService.ToiletriesCosmetics
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile radio.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileRadio(CreateOrUpdateCampaignProfileCommand command,
        //                                                  CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileRadioCommand radio in command.CampaignProfileRadios)
        //    {
        //        profile.CampaignProfileRadios.Add(new CampaignProfileRadio
        //        {
        //            Local = radio.Local,
        //            Music = radio.Music,
        //            National = radio.National,
        //            Sport = radio.Sport,
        //            Talk = radio.Talk
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign profile time settings.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        private static void TranslateCampaignProfileTimeSettings(CreateOrUpdateCampaignProfileCommand command,
                                                                 CampaignProfile profile)
        {
            foreach (CreateOrUpdateCampaignProfileTimeSettingCommand time in command.CampaignProfileTimeSettings)
            {
                var timeSettings = new CampaignProfileTimeSetting();

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

                profile.CampaignProfileTimeSettings.Add(timeSettings);
            }
        }

        /// <summary>
        /// Translates the campaign profile tv.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private static void TranslateCampaignProfileTv(CreateOrUpdateCampaignProfileCommand command,
        //                                               CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileTvCommand tv in command.CampaignProfileTvs)
        //    {
        //        profile.CampaignProfileTvs.Add(new CampaignProfileTv
        //        {
        //            Cable = tv.Cable,
        //            Internet = tv.Internet,
        //            Satallite = tv.Satallite,
        //            Terrestrial = tv.Terrestrial
        //        });
        //    }
        //}

        /// <summary>
        /// Translates the campaign demographics.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="profile">The profile.</param>
        //private void TranslateCampaignDemographics(CreateOrUpdateCampaignProfileCommand command, CampaignProfile profile)
        //{
        //    foreach (CreateOrUpdateCampaignProfileDemographicsCommand demographic in command.CampaignProfileDemographics
        //        )
        //    {
        //        profile.CampaignProfileDemographics.Add(new CampaignProfileDemographics
        //        {
        //            CampaignProfileId = demographic.CampaignProfileId,
        //            DOBEnd = demographic.DOBEnd,
        //            DOBStart = demographic.DOBStart,
        //            Education = demographic.Education,
        //            Gender = demographic.Gender,
        //            HouseholdStatus = demographic.HouseholdStatus,
        //            IncomeBracket = demographic.IncomeBracket,
        //            Location = demographic.Location,
        //            RelationshipStatus = demographic.RelationshipStatus,
        //            WorkingStatus = demographic.WorkingStatus
        //        });
        //    }
        //}
    }
}