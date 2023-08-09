// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileAdvertHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileAdvertHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileAdvertHandler : ICommandHandler<CreateOrUpdateUserProfileAdvertCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferencRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileAdvertHandler"/> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileAdvertHandler(IUserProfilePreferenceRepository userProfilePreferencRepository,
                                                      IUnitOfWork unitOfWork)
        {
            _userProfilePreferencRepository = userProfilePreferencRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileAdvertCommand command)
        {
            var userProfilePreference = new UserProfilePreference
            {
                AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert,
                //AppliancesOtherHouseholdDurables_Advert = command.AppliancesOtherHouseholdDurables_Advert,
                Cinema_Advert = command.Cinema_Advert,
                CommunicationsInternet_Advert = command.CommunicationsInternet_Advert,
                DIYGardening_Advert = command.DIYGardening_Advert,
                ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert,
                Environment_Advert = command.Environment_Advert,
               // Fashion_Advert = command.Fashion_Advert,
                //FinancialProducts_Advert = command.FinancialProducts_Advert,
                FinancialServices_Advert = command.FinancialServices_Advert,
                Fitness_Advert = command.Fitness_Advert,
                Food_Advert = command.Food_Advert,
               // GeneralUse_Advert = command.GeneralUse_Advert,
                GoingOut_Advert = command.GoingOut_Advert,
                //Holidays_Advert = command.Holidays_Advert,
                HolidaysTravel_Advert = command.HolidaysTravel_Advert,
                Householdproducts_Advert = command.Householdproducts_Advert,
               // Magazines_Advert = command.Magazines_Advert,
                Motoring_Advert = command.Motoring_Advert,
                Music_Advert = command.Music_Advert,
                Newspapers_Advert = command.Newspapers_Advert,
                NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert,
                PetsPetFood_Advert = command.PetsPetFood_Advert,
                PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert,
                //Radio_Advert = command.Radio_Advert,
                Religion_Advert = command.Religion_Advert,
                Shopping_Advert = command.Shopping_Advert,
                ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert,
                SocialNetworking_Advert = command.SocialNetworking_Advert,
                SportsLeisure_Advert = command.SportsLeisure_Advert,
                SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert,
                TV_Advert = command.TV_Advert,
                TobaccoProducts_Advert = command.TobaccoProducts_Advert,
                ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert,
                UserProfileId = command.UserProfileId,

                BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType,
                Gambling_AdType = command.Gambling_AdType,
                Restaurants_AdType = command.Restaurants_AdType,
                Insurance_AdType = command.Insurance_AdType,
                Furniture_AdType = command.Furniture_AdType,
                InformationTechnology_AdType = command.InformationTechnology_AdType,
                Energy_AdType = command.Energy_AdType,
                Supermarkets_AdType = command.Supermarkets_AdType,
                Healthcare_AdType = command.Healthcare_AdType,
                JobsAndEducation_AdType = command.JobsAndEducation_AdType,
                Gifts_AdType = command.Gifts_AdType,
                AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType,
                DatingAndPersonal_AdType = command.DatingAndPersonal_AdType,
                RealEstate_AdType = command.RealEstate_AdType,
                Games_AdType = command.Games_AdType,                
                Id = command.Id
            };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (userProfilePreference.Id == 0)
            {
                _userProfilePreferencRepository.Add(userProfilePreference);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);

                        if(externalServerUserProfileId != 0)
                        {
                            var userProfilePreference2 = new UserProfilePreference
                            {
                                AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert,
                                Cinema_Advert = command.Cinema_Advert,
                                CommunicationsInternet_Advert = command.CommunicationsInternet_Advert,
                                DIYGardening_Advert = command.DIYGardening_Advert,
                                ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert,
                                Environment_Advert = command.Environment_Advert,
                                FinancialServices_Advert = command.FinancialServices_Advert,
                                Fitness_Advert = command.Fitness_Advert,
                                Food_Advert = command.Food_Advert,
                                GoingOut_Advert = command.GoingOut_Advert,
                                HolidaysTravel_Advert = command.HolidaysTravel_Advert,
                                Householdproducts_Advert = command.Householdproducts_Advert,
                                Motoring_Advert = command.Motoring_Advert,
                                Music_Advert = command.Music_Advert,
                                Newspapers_Advert = command.Newspapers_Advert,
                                NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert,
                                PetsPetFood_Advert = command.PetsPetFood_Advert,
                                PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert,
                                Religion_Advert = command.Religion_Advert,
                                Shopping_Advert = command.Shopping_Advert,
                                ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert,
                                SocialNetworking_Advert = command.SocialNetworking_Advert,
                                SportsLeisure_Advert = command.SportsLeisure_Advert,
                                SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert,
                                TV_Advert = command.TV_Advert,
                                TobaccoProducts_Advert = command.TobaccoProducts_Advert,
                                ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert,
                                UserProfileId = externalServerUserProfileId,
                                BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType,
                                Gambling_AdType = command.Gambling_AdType,
                                Restaurants_AdType = command.Restaurants_AdType,
                                Insurance_AdType = command.Insurance_AdType,
                                Furniture_AdType = command.Furniture_AdType,
                                InformationTechnology_AdType = command.InformationTechnology_AdType,
                                Energy_AdType = command.Energy_AdType,
                                Supermarkets_AdType = command.Supermarkets_AdType,
                                Healthcare_AdType = command.Healthcare_AdType,
                                JobsAndEducation_AdType = command.JobsAndEducation_AdType,
                                Gifts_AdType = command.Gifts_AdType,
                                AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType,
                                DatingAndPersonal_AdType = command.DatingAndPersonal_AdType,
                                RealEstate_AdType = command.RealEstate_AdType,
                                Games_AdType = command.Games_AdType,
                                Id = command.Id,
                                AdtoneServerUserProfilePrefId = userProfilePreference.Id
                            };
                            db.UserProfilePreference.Add(userProfilePreference2);
                            db.SaveChanges();
                        }
                       
                    }
                        
                }
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferencRepository.GetById(command.Id);
                userprofile.AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert;
                userprofile.Cinema_Advert = command.Cinema_Advert;
                userprofile.CommunicationsInternet_Advert = command.CommunicationsInternet_Advert;
                userprofile.DIYGardening_Advert = command.DIYGardening_Advert;
                userprofile.ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert;
                userprofile.Environment_Advert = command.Environment_Advert;            
                userprofile.FinancialServices_Advert = command.FinancialServices_Advert;
                userprofile.Fitness_Advert = command.Fitness_Advert;
                userprofile.Food_Advert = command.Food_Advert;
                userprofile.GoingOut_Advert = command.GoingOut_Advert;
                userprofile.HolidaysTravel_Advert = command.HolidaysTravel_Advert;
                userprofile.Householdproducts_Advert = command.Householdproducts_Advert;
                userprofile.Motoring_Advert = command.Motoring_Advert;
                userprofile.Music_Advert = command.Music_Advert;
                userprofile.Newspapers_Advert = command.Newspapers_Advert;
                userprofile.NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert;
                userprofile.PetsPetFood_Advert = command.PetsPetFood_Advert;
                userprofile.PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert;
                userprofile.Religion_Advert = command.Religion_Advert;
                userprofile.Shopping_Advert = command.Shopping_Advert;
                userprofile.ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert;
                userprofile.SocialNetworking_Advert = command.SocialNetworking_Advert;
                userprofile.SportsLeisure_Advert = command.SportsLeisure_Advert;
                userprofile.SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert;
                userprofile.TV_Advert = command.TV_Advert;
                userprofile.TobaccoProducts_Advert = command.TobaccoProducts_Advert;
                userprofile.ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert;
                userprofile.BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType;
                userprofile.Gambling_AdType = command.Gambling_AdType;
                userprofile.Restaurants_AdType = command.Restaurants_AdType;
                userprofile.Insurance_AdType = command.Insurance_AdType;
                userprofile.Furniture_AdType = command.Furniture_AdType;
                userprofile.InformationTechnology_AdType = command.InformationTechnology_AdType;
                userprofile.Energy_AdType = command.Energy_AdType;
                userprofile.Supermarkets_AdType = command.Supermarkets_AdType;
                userprofile.Healthcare_AdType = command.Healthcare_AdType;
                userprofile.JobsAndEducation_AdType = command.JobsAndEducation_AdType;
                userprofile.Gifts_AdType = command.Gifts_AdType;
                userprofile.AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType;
                userprofile.DatingAndPersonal_AdType = command.DatingAndPersonal_AdType;
                userprofile.RealEstate_AdType = command.RealEstate_AdType;
                userprofile.Games_AdType = command.Games_AdType;
                _userProfilePreferencRepository.Update(userprofile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userProfilePref = db.UserProfilePreference.Where(s => s.AdtoneServerUserProfilePrefId == command.Id).FirstOrDefault();
                        if (userProfilePref != null)
                        {
                            userProfilePref.AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert;
                            userProfilePref.Cinema_Advert = command.Cinema_Advert;
                            userProfilePref.CommunicationsInternet_Advert = command.CommunicationsInternet_Advert;
                            userProfilePref.DIYGardening_Advert = command.DIYGardening_Advert;
                            userProfilePref.ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert;
                            userProfilePref.Environment_Advert = command.Environment_Advert;
                            userProfilePref.FinancialServices_Advert = command.FinancialServices_Advert;
                            userProfilePref.Fitness_Advert = command.Fitness_Advert;
                            userProfilePref.Food_Advert = command.Food_Advert;
                            userProfilePref.GoingOut_Advert = command.GoingOut_Advert;
                            userProfilePref.HolidaysTravel_Advert = command.HolidaysTravel_Advert;
                            userProfilePref.Householdproducts_Advert = command.Householdproducts_Advert;
                            userProfilePref.Motoring_Advert = command.Motoring_Advert;
                            userProfilePref.Music_Advert = command.Music_Advert;
                            userProfilePref.Newspapers_Advert = command.Newspapers_Advert;
                            userProfilePref.NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert;
                            userProfilePref.PetsPetFood_Advert = command.PetsPetFood_Advert;
                            userProfilePref.PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert;
                            userProfilePref.Religion_Advert = command.Religion_Advert;
                            userProfilePref.Shopping_Advert = command.Shopping_Advert;
                            userProfilePref.ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert;
                            userProfilePref.SocialNetworking_Advert = command.SocialNetworking_Advert;
                            userProfilePref.SportsLeisure_Advert = command.SportsLeisure_Advert;
                            userProfilePref.SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert;
                            userProfilePref.TV_Advert = command.TV_Advert;
                            userProfilePref.TobaccoProducts_Advert = command.TobaccoProducts_Advert;
                            userProfilePref.ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert;
                            userProfilePref.BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType;
                            userProfilePref.Gambling_AdType = command.Gambling_AdType;
                            userProfilePref.Restaurants_AdType = command.Restaurants_AdType;
                            userProfilePref.Insurance_AdType = command.Insurance_AdType;
                            userProfilePref.Furniture_AdType = command.Furniture_AdType;
                            userProfilePref.InformationTechnology_AdType = command.InformationTechnology_AdType;
                            userProfilePref.Energy_AdType = command.Energy_AdType;
                            userProfilePref.Supermarkets_AdType = command.Supermarkets_AdType;
                            userProfilePref.Healthcare_AdType = command.Healthcare_AdType;
                            userProfilePref.JobsAndEducation_AdType = command.JobsAndEducation_AdType;
                            userProfilePref.Gifts_AdType = command.Gifts_AdType;
                            userProfilePref.AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType;
                            userProfilePref.DatingAndPersonal_AdType = command.DatingAndPersonal_AdType;
                            userProfilePref.RealEstate_AdType = command.RealEstate_AdType;
                            userProfilePref.Games_AdType = command.Games_AdType;                           
                            db.SaveChanges();
                        }
                    }                        
                }             

            }
           

            return new CommandResult(true);
        }

        #endregion
    }
}