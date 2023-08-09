// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileAdvertHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileAdvertHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileAdvertHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileAdvertCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _advertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileAdvertHandler"/> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileAdvertHandler(ICampaignProfilePreferenceRepository advertRepository, ICampaignProfileRepository profileRepository,
                                                          IUnitOfWork unitOfWork)
        {
            _advertRepository = advertRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileAdvertCommand command)
        {
            var advert = new CampaignProfilePreference
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
                CampaignProfileId = command.CampaignProfileId,
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
                Id = command.CampaignProfileAdvertsId,
                NextStatus = command.NextStatus
            };

            if (advert.Id == 0)
            {
                _advertRepository.Add(advert);
                _unitOfWork.Commit();
                int countryId = 0;
                var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
                if (campaignProfile != null)
                {
                    countryId = (int)campaignProfile.CountryId;
                }
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                        if(externalServerCampaignProfileId != 0)
                        {
                            var advert2 = new CampaignProfilePreference
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
                                CampaignProfileId = externalServerCampaignProfileId,
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
                                Id = command.CampaignProfileAdvertsId,
                                AdtoneServerCampaignProfilePrefId = advert.Id
                            };
                            db.CampaignProfilePreference.Add(advert2);
                            db.SaveChanges();
                        }
                      
                    }
                        
                }
            }
            else
            {
                CampaignProfilePreference campaignProfile = _advertRepository.GetById(command.CampaignProfileAdvertsId);
                campaignProfile.AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert;
                campaignProfile.Cinema_Advert = command.Cinema_Advert;
                campaignProfile.CommunicationsInternet_Advert = command.CommunicationsInternet_Advert;
                campaignProfile.DIYGardening_Advert = command.DIYGardening_Advert;
                campaignProfile.ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert;
                campaignProfile.Environment_Advert = command.Environment_Advert;
                campaignProfile.FinancialServices_Advert = command.FinancialServices_Advert;
                campaignProfile.Fitness_Advert = command.Fitness_Advert;
                campaignProfile.Food_Advert = command.Food_Advert;
                campaignProfile.GoingOut_Advert = command.GoingOut_Advert;
                campaignProfile.HolidaysTravel_Advert = command.HolidaysTravel_Advert;
                campaignProfile.Householdproducts_Advert = command.Householdproducts_Advert;
                campaignProfile.Motoring_Advert = command.Motoring_Advert;
                campaignProfile.Music_Advert = command.Music_Advert;
                campaignProfile.Newspapers_Advert = command.Newspapers_Advert;
                campaignProfile.NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert;
                campaignProfile.PetsPetFood_Advert = command.PetsPetFood_Advert;
                campaignProfile.PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert;
                campaignProfile.Religion_Advert = command.Religion_Advert;
                campaignProfile.Shopping_Advert = command.Shopping_Advert;
                campaignProfile.ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert;
                campaignProfile.SocialNetworking_Advert = command.SocialNetworking_Advert;
                campaignProfile.SportsLeisure_Advert = command.SportsLeisure_Advert;
                campaignProfile.SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert;
                campaignProfile.TV_Advert = command.TV_Advert;
                campaignProfile.TobaccoProducts_Advert = command.TobaccoProducts_Advert;
                campaignProfile.ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType;
                campaignProfile.Gambling_AdType = command.Gambling_AdType;
                campaignProfile.Restaurants_AdType = command.Restaurants_AdType;
                campaignProfile.Insurance_AdType = command.Insurance_AdType;
                campaignProfile.Furniture_AdType = command.Furniture_AdType;
                campaignProfile.InformationTechnology_AdType = command.InformationTechnology_AdType;
                campaignProfile.Energy_AdType = command.Energy_AdType;
                campaignProfile.Supermarkets_AdType = command.Supermarkets_AdType;
                campaignProfile.Healthcare_AdType = command.Healthcare_AdType;
                campaignProfile.JobsAndEducation_AdType = command.JobsAndEducation_AdType;
                campaignProfile.Gifts_AdType = command.Gifts_AdType;
                campaignProfile.AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType;
                campaignProfile.DatingAndPersonal_AdType = command.DatingAndPersonal_AdType;
                campaignProfile.RealEstate_AdType = command.RealEstate_AdType;
                campaignProfile.Games_AdType = command.Games_AdType;
                campaignProfile.Id = campaignProfile.Id;
                _advertRepository.Update(campaignProfile);
                _unitOfWork.Commit();
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach(var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campProfilePreference = db.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == command.CampaignProfileAdvertsId).FirstOrDefault();
                        if (campProfilePreference != null)
                        {
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                            if(externalServerCampaignProfileId != 0)
                            {
                                campProfilePreference.AlcoholicDrinks_Advert = command.AlcoholicDrinks_Advert;
                                campProfilePreference.Cinema_Advert = command.Cinema_Advert;
                                campProfilePreference.CommunicationsInternet_Advert = command.CommunicationsInternet_Advert;
                                campProfilePreference.DIYGardening_Advert = command.DIYGardening_Advert;
                                campProfilePreference.ElectronicsOtherPersonalItems_Advert = command.ElectronicsOtherPersonalItems_Advert;
                                campProfilePreference.Environment_Advert = command.Environment_Advert;
                                campProfilePreference.FinancialServices_Advert = command.FinancialServices_Advert;
                                campProfilePreference.Fitness_Advert = command.Fitness_Advert;
                                campProfilePreference.Food_Advert = command.Food_Advert;
                                campProfilePreference.GoingOut_Advert = command.GoingOut_Advert;
                                campProfilePreference.HolidaysTravel_Advert = command.HolidaysTravel_Advert;
                                campProfilePreference.Householdproducts_Advert = command.Householdproducts_Advert;
                                campProfilePreference.Motoring_Advert = command.Motoring_Advert;
                                campProfilePreference.Music_Advert = command.Music_Advert;
                                campProfilePreference.Newspapers_Advert = command.Newspapers_Advert;
                                campProfilePreference.NonAlcoholicDrinks_Advert = command.NonAlcoholicDrinks_Advert;
                                campProfilePreference.PetsPetFood_Advert = command.PetsPetFood_Advert;
                                campProfilePreference.PharmaceuticalChemistsProducts_Advert = command.PharmaceuticalChemistsProducts_Advert;
                                campProfilePreference.Religion_Advert = command.Religion_Advert;
                                campProfilePreference.Shopping_Advert = command.Shopping_Advert;
                                campProfilePreference.ShoppingRetailClothing_Advert = command.ShoppingRetailClothing_Advert;
                                campProfilePreference.SocialNetworking_Advert = command.SocialNetworking_Advert;
                                campProfilePreference.SportsLeisure_Advert = command.SportsLeisure_Advert;
                                campProfilePreference.SweetSaltySnacks_Advert = command.SweetSaltySnacks_Advert;
                                campProfilePreference.TV_Advert = command.TV_Advert;
                                campProfilePreference.TobaccoProducts_Advert = command.TobaccoProducts_Advert;
                                campProfilePreference.ToiletriesCosmetics_Advert = command.ToiletriesCosmetics_Advert;
                                campProfilePreference.CampaignProfileId = externalServerCampaignProfileId;
                                campProfilePreference.BusinessOrOpportunities_AdType = command.BusinessOrOpportunities_AdType;
                                campProfilePreference.Gambling_AdType = command.Gambling_AdType;
                                campProfilePreference.Restaurants_AdType = command.Restaurants_AdType;
                                campProfilePreference.Insurance_AdType = command.Insurance_AdType;
                                campProfilePreference.Furniture_AdType = command.Furniture_AdType;
                                campProfilePreference.InformationTechnology_AdType = command.InformationTechnology_AdType;
                                campProfilePreference.Energy_AdType = command.Energy_AdType;
                                campProfilePreference.Supermarkets_AdType = command.Supermarkets_AdType;
                                campProfilePreference.Healthcare_AdType = command.Healthcare_AdType;
                                campProfilePreference.JobsAndEducation_AdType = command.JobsAndEducation_AdType;
                                campProfilePreference.Gifts_AdType = command.Gifts_AdType;
                                campProfilePreference.AdvocacyOrLegal_AdType = command.AdvocacyOrLegal_AdType;
                                campProfilePreference.DatingAndPersonal_AdType = command.DatingAndPersonal_AdType;
                                campProfilePreference.RealEstate_AdType = command.RealEstate_AdType;
                                campProfilePreference.Games_AdType = command.Games_AdType;
                                db.SaveChanges();
                            }
               
                        }
                    }
                   
                }
            }
           

            return new CommandResult(true);
        }

        #endregion
    }
}