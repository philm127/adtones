// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="UserRegisterHandler.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.ObjectModel;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

/// <summary>
/// The Security namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.Security
{
    /// <summary>
    /// Class UserRegisterHandler.
    /// </summary>
    public class UserRegisterHandler : ICommandHandler<UserRegisterCommand>
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
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegisterHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserRegisterHandler(IUserRepository userRepository, IProfileRepository profileRepository,IOperatorRepository operatorRepository,
                                   IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _operatorRepository = operatorRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<UserRegisterCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(UserRegisterCommand command)
        {
            User user = null;

            if (command.Password == null) // Social Login 
            {
                user = new User
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    Organisation = command.Organisation,
                    RoleId = command.RoleId,
                    DateCreated = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    Activated = command.Activated,
                    Outstandingdays = command.Outstandingdays,
                    VerificationStatus = command.VerificationStatus,
                    OperatorId = command.OperatorId,
                    IsMobileVerfication = command.IsMobileVerfication,
                    OrganisationTypeId = command.OrganisationTypeId == 0 ? null : command.OrganisationTypeId,
                    UserMatchTableName = command.UserMatchTableName,
                    IsMsisdnMatch = command.IsMsisdnMatch,
                    TibcoMessageId = command.TibcoMessageId,
                    IsSessionFlag = command.IsSessionFlag,
                    LockOutTime = command.LockOutTime,
                    LastPasswordChangedDate = command.LastPasswordChangedDate
                };
            }
            else
            {
                user = new User
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    Organisation = command.Organisation,
                    Password = command.Password,
                    RoleId = command.RoleId,
                    DateCreated = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    Activated = command.Activated,
                    Outstandingdays = command.Outstandingdays,
                    VerificationStatus = command.VerificationStatus,
                    OperatorId = command.OperatorId,
                    IsMobileVerfication = command.IsMobileVerfication,
                    OrganisationTypeId = command.OrganisationTypeId == 0 ? null : command.OrganisationTypeId,
                    UserMatchTableName = command.UserMatchTableName,
                    IsMsisdnMatch = command.IsMsisdnMatch,
                    TibcoMessageId = command.TibcoMessageId,
                    IsSessionFlag = command.IsSessionFlag,
                    LockOutTime = command.LockOutTime,
                    LastPasswordChangedDate = command.LastPasswordChangedDate
                };
            }

            

            _userRepository.Add(user);
            _unitOfWork.Commit();
            if (command.RoleId == 2)
            {                
                var userProfile = new UserProfile
                {
                    UserId = user.UserId,
                    MSISDN = command.MSISDN,
                    UserProfileAdverts = new Collection<UserProfileAdvert> { new UserProfileAdvert { AlcoholicDrinks = 1, AppliancesOtherHouseholdDurables = 1, Cinema = 1, CommunicationsInternet = 1, DIYGardening = 1, ElectronicsOtherPersonalItems = 1, Environment = 1, Fashion = 1, FinancialProducts = 1, FinancialServices = 1, Fitness = 1, Food = 1, GeneralUse = 1, GoingOut = 1, Holidays = 1, HolidaysTravel = 1, Householdproducts = 1, Magazines = 1, Motoring = 1, Music = 1, Newspapers = 1, NonAlcoholicDrinks = 1, PetsPetFood = 1, PharmaceuticalChemistsProducts = 1, Radio = 1, Religion = 1, Shopping = 1, ShoppingRetailClothing = 1, SocialNetworking = 1, SportsLeisure = 1, SweetSaltySnacks = 1, TV = 1, TobaccoProducts = 1, ToiletriesCosmetics = 1 } },
                    UserProfileAttitudes = new Collection<UserProfileAttitude> { new UserProfileAttitude { Environment = 1, Fashion = 1, Fitness = 1, FinancialStabiity = 1, GoingOut = 1, Holidays = 1, Music = 1, Religion = 1 } },
                    UserProfileCinemas = new Collection<UserProfileCinema> { new UserProfileCinema { Cinema = 0 } },
                    UserProfileInternets = new Collection<UserProfileInternet> { new UserProfileInternet { Auctions = 0, Research = 0, Shopping = 0, SocialNetworking = 0, Video = 0 } },
                    UserProfileMobiles = new Collection<UserProfileMobile> { new UserProfileMobile { ContractType = "0", Spend = "0" } },
                    UserProfilePresses = new Collection<UserProfilePress> { new UserProfilePress { FreeNewpapers = 0, Local = 0, Magazines = 0, National = 0 } },
                    UserProfileProductsServices = new Collection<UserProfileProductsService> { new UserProfileProductsService { AlcoholicDrinks = 0, AppliancesOtherHouseholdDurables = 0, CommunicationsInternet = 0, DIYGardening = 0, ElectronicsOtherPersonalItems = 0, FinancialServices = 0, Food = 0, HolidaysTravel = 0, Householdproducts = 0, Motoring = 0, NonAlcoholicDrinks = 0, PetsPetFood = 0, PharmaceuticalChemistsProducts = 0, ShoppingRetailClothing = 0, SportsLeisure = 0, SweetSaltySnacks = 0, TobaccoProducts = 0, ToiletriesCosmetics = 0 } },
                    UserProfileRadios = new Collection<UserProfileRadio> { new UserProfileRadio { Local = 0, Music = 0, National = 0, Sport = 0, Talk = 0 } },
                    //UserProfileTimeSettings = new Collection<UserProfileTimeSetting> { new UserProfileTimeSetting() },
                    UserProfileTvs = new Collection<UserProfileTv> { new UserProfileTv { Cable = 0, Internet = 0, Satallite = 0, Terrestrial = 0 } }
                };
                _profileRepository.Add(userProfile);
                _unitOfWork.Commit();

               
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                        if (externalServerOperatorId != 0)
                        {
                            User user2 = null;
                            if(command.Password == null) // Social Login
                            {
                                user2 = new User
                                {
                                    FirstName = command.FirstName,
                                    LastName = command.LastName,
                                    Email = command.Email,
                                    Organisation = command.Organisation,
                                    RoleId = command.RoleId,
                                    DateCreated = DateTime.Now,
                                    LastLoginTime = DateTime.Now,
                                    Activated = command.Activated,
                                    Outstandingdays = command.Outstandingdays,
                                    VerificationStatus = command.VerificationStatus,
                                    OperatorId = externalServerOperatorId,
                                    IsMobileVerfication = command.IsMobileVerfication,
                                    OrganisationTypeId = command.OrganisationTypeId == 0 ? null : command.OrganisationTypeId,
                                    UserMatchTableName = command.UserMatchTableName,
                                    IsMsisdnMatch = command.IsMsisdnMatch,
                                    AdtoneServerUserId = user.UserId,
                                    TibcoMessageId = command.TibcoMessageId,
                                    IsSessionFlag = command.IsSessionFlag,
                                    LockOutTime = command.LockOutTime,
                                    LastPasswordChangedDate = command.LastPasswordChangedDate
                                };
                                db.Users.Add(user2);
                                db.SaveChanges();
                            }
                            else
                            {
                                user2 = new User
                                {
                                    FirstName = command.FirstName,
                                    LastName = command.LastName,
                                    Email = command.Email,
                                    Organisation = command.Organisation,
                                    Password = command.Password,
                                    RoleId = command.RoleId,
                                    DateCreated = DateTime.Now,
                                    LastLoginTime = DateTime.Now,
                                    Activated = command.Activated,
                                    Outstandingdays = command.Outstandingdays,
                                    VerificationStatus = command.VerificationStatus,
                                    OperatorId = externalServerOperatorId,
                                    IsMobileVerfication = command.IsMobileVerfication,
                                    OrganisationTypeId = command.OrganisationTypeId == 0 ? null : command.OrganisationTypeId,
                                    UserMatchTableName = command.UserMatchTableName,
                                    IsMsisdnMatch = command.IsMsisdnMatch,
                                    AdtoneServerUserId = user.UserId,
                                    TibcoMessageId = command.TibcoMessageId,
                                    IsSessionFlag = command.IsSessionFlag,
                                    LockOutTime = command.LockOutTime,
                                    LastPasswordChangedDate = command.LastPasswordChangedDate
                                };
                                db.Users.Add(user2);
                                db.SaveChanges();
                            }
                            

                            

                            var userProfile2 = new UserProfile
                            {
                                UserId = user2.UserId,
                                MSISDN = command.MSISDN,
                                AdtoneServerUserProfileId = userProfile.UserProfileId,
                                UserProfileAdverts = new Collection<UserProfileAdvert> { new UserProfileAdvert { AlcoholicDrinks = 1, AppliancesOtherHouseholdDurables = 1, Cinema = 1, CommunicationsInternet = 1, DIYGardening = 1, ElectronicsOtherPersonalItems = 1, Environment = 1, Fashion = 1, FinancialProducts = 1, FinancialServices = 1, Fitness = 1, Food = 1, GeneralUse = 1, GoingOut = 1, Holidays = 1, HolidaysTravel = 1, Householdproducts = 1, Magazines = 1, Motoring = 1, Music = 1, Newspapers = 1, NonAlcoholicDrinks = 1, PetsPetFood = 1, PharmaceuticalChemistsProducts = 1, Radio = 1, Religion = 1, Shopping = 1, ShoppingRetailClothing = 1, SocialNetworking = 1, SportsLeisure = 1, SweetSaltySnacks = 1, TV = 1, TobaccoProducts = 1, ToiletriesCosmetics = 1 } },
                                UserProfileAttitudes = new Collection<UserProfileAttitude> { new UserProfileAttitude { Environment = 1, Fashion = 1, Fitness = 1, FinancialStabiity = 1, GoingOut = 1, Holidays = 1, Music = 1, Religion = 1 } },
                                UserProfileCinemas = new Collection<UserProfileCinema> { new UserProfileCinema { Cinema = 0 } },
                                UserProfileInternets = new Collection<UserProfileInternet> { new UserProfileInternet { Auctions = 0, Research = 0, Shopping = 0, SocialNetworking = 0, Video = 0 } },
                                UserProfileMobiles = new Collection<UserProfileMobile> { new UserProfileMobile { ContractType = "0", Spend = "0" } },
                                UserProfilePresses = new Collection<UserProfilePress> { new UserProfilePress { FreeNewpapers = 0, Local = 0, Magazines = 0, National = 0 } },
                                UserProfileProductsServices = new Collection<UserProfileProductsService> { new UserProfileProductsService { AlcoholicDrinks = 0, AppliancesOtherHouseholdDurables = 0, CommunicationsInternet = 0, DIYGardening = 0, ElectronicsOtherPersonalItems = 0, FinancialServices = 0, Food = 0, HolidaysTravel = 0, Householdproducts = 0, Motoring = 0, NonAlcoholicDrinks = 0, PetsPetFood = 0, PharmaceuticalChemistsProducts = 0, ShoppingRetailClothing = 0, SportsLeisure = 0, SweetSaltySnacks = 0, TobaccoProducts = 0, ToiletriesCosmetics = 0 } },
                                UserProfileRadios = new Collection<UserProfileRadio> { new UserProfileRadio { Local = 0, Music = 0, National = 0, Sport = 0, Talk = 0 } },
                                //UserProfileTimeSettings = new Collection<UserProfileTimeSetting> { new UserProfileTimeSetting() },
                                UserProfileTvs = new Collection<UserProfileTv> { new UserProfileTv { Cable = 0, Internet = 0, Satallite = 0, Terrestrial = 0 } }
                            };

                            db.Userprofiles.Add(userProfile2);
                            db.SaveChanges();
                        }
                        
                    }
                 
                }
            }
            else
            {
                _unitOfWork.Commit();

            }

            return new CommandResult(true, user.UserId);
            //return new CommandResult(true);
        }

        #endregion
    }
}