// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileProductsServicesHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateCampaignProfileProductsServicesHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileProductsServicesHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileProductsServiceCommand>
    {
        /// <summary>
        /// The _products services repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _productsServicesRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileProductsServicesHandler"/> class.
        /// </summary>
        /// <param name="productsServicesRepository">The products services repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileProductsServicesHandler(
            ICampaignProfilePreferenceRepository productsServicesRepository, IUnitOfWork unitOfWork)
        {
            _productsServicesRepository = productsServicesRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileProductsServiceCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileProductsServiceCommand command)
        {
            var ProductsServices = new CampaignProfilePreference
                                       {
                                           AlcoholicDrinks_ProductsService = command.AlcoholicDrinks_ProductsService,
                                           AppliancesOtherHouseholdDurables_ProductsService = command.AppliancesOtherHouseholdDurables_ProductsService,
                                           CommunicationsInternet_ProductsService = command.CommunicationsInternet_ProductsService,
                                           DIYGardening_ProductsService = command.DIYGardening_ProductsService,
                                           ElectronicsOtherPersonalItems_ProductsService = command.ElectronicsOtherPersonalItems_ProductsService,
                                           FinancialServices_ProductsService = command.FinancialServices_ProductsService,
                                           Food_ProductsService = command.Food_ProductsService,
                                           HolidaysTravel_ProductsService = command.HolidaysTravel_ProductsService,
                                           Householdproducts_ProductsService = command.Householdproducts_ProductsService,
                                           Motoring_ProductsService = command.Motoring_ProductsService,
                                           NonAlcoholicDrinks_ProductsService = command.NonAlcoholicDrinks_ProductsService,
                                           PetsPetFood_ProductsService = command.PetsPetFood_ProductsService,
                                           PharmaceuticalChemistsProducts_ProductsService = command.PharmaceuticalChemistsProducts_ProductsService,
                                           ShoppingRetailClothing_ProductsService = command.ShoppingRetailClothing_ProductsService,
                                           SportsLeisure_ProductsService = command.SportsLeisure_ProductsService,
                                           SweetSaltySnacks_ProductsService = command.SweetSaltySnacks_ProductsService,
                                           TobaccoProducts_ProductsService = command.TobaccoProducts_ProductsService,
                                           ToiletriesCosmetics_ProductsService = command.ToiletriesCosmetics_ProductsService,
                                           CampaignProfileId = command.CampaignProfileId,
                                           Id = command.CampaignProfileProductsServicesId,
                                       };

            if (ProductsServices.Id == 0)
            {
                _productsServicesRepository.Add(ProductsServices);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _productsServicesRepository.GetById(command.CampaignProfileProductsServicesId);
                campaignProfile.AlcoholicDrinks_ProductsService = command.AlcoholicDrinks_ProductsService;
                campaignProfile.AppliancesOtherHouseholdDurables_ProductsService = command.AppliancesOtherHouseholdDurables_ProductsService;
                campaignProfile.CommunicationsInternet_ProductsService = command.CommunicationsInternet_ProductsService;
                campaignProfile.DIYGardening_ProductsService = command.DIYGardening_ProductsService;
                campaignProfile.ElectronicsOtherPersonalItems_ProductsService = command.ElectronicsOtherPersonalItems_ProductsService;
                campaignProfile.FinancialServices_ProductsService = command.FinancialServices_ProductsService;
                campaignProfile.Food_ProductsService = command.Food_ProductsService;
                campaignProfile.HolidaysTravel_ProductsService = command.HolidaysTravel_ProductsService;
                campaignProfile.Householdproducts_ProductsService = command.Householdproducts_ProductsService;
                campaignProfile.Motoring_ProductsService = command.Motoring_ProductsService;
                campaignProfile.NonAlcoholicDrinks_ProductsService = command.NonAlcoholicDrinks_ProductsService;
                campaignProfile.PetsPetFood_ProductsService = command.PetsPetFood_ProductsService;
                campaignProfile.PharmaceuticalChemistsProducts_ProductsService = command.PharmaceuticalChemistsProducts_ProductsService;
                campaignProfile.ShoppingRetailClothing_ProductsService = command.ShoppingRetailClothing_ProductsService;
                campaignProfile.SportsLeisure_ProductsService = command.SportsLeisure_ProductsService;
                campaignProfile.SweetSaltySnacks_ProductsService = command.SweetSaltySnacks_ProductsService;
                campaignProfile.TobaccoProducts_ProductsService = command.TobaccoProducts_ProductsService;
                campaignProfile.ToiletriesCosmetics_ProductsService = command.ToiletriesCosmetics_ProductsService;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _productsServicesRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}