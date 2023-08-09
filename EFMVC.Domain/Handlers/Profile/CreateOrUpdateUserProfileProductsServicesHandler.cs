// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileProductsServicesHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileProductsServicesHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileProductsServicesHandler :
        ICommandHandler<CreateOrUpdateUserProfileProductsServiceCommand>
    {
        /// <summary>
        /// The _products services repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileProductsServicesHandler"/> class.
        /// </summary>
        /// <param name="productsServicesRepository">The products services repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileProductsServicesHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository,
                                                                IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileProductsServiceCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileProductsServiceCommand command)
        {
            var ProductsServices = new UserProfilePreference
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
                UserProfileId = command.UserProfileId,
                Id = command.Id,
            };

            if (ProductsServices.Id == 0)
            {
                _userProfilePreferenceRepository.Add(ProductsServices);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.AlcoholicDrinks_ProductsService = command.AlcoholicDrinks_ProductsService;
                userprofile.AppliancesOtherHouseholdDurables_ProductsService = command.AppliancesOtherHouseholdDurables_ProductsService;
                userprofile.CommunicationsInternet_ProductsService = command.CommunicationsInternet_ProductsService;
                userprofile.DIYGardening_ProductsService = command.DIYGardening_ProductsService;
                userprofile.ElectronicsOtherPersonalItems_ProductsService = command.ElectronicsOtherPersonalItems_ProductsService;
                userprofile.FinancialServices_ProductsService = command.FinancialServices_ProductsService;
                userprofile.Food_ProductsService = command.Food_ProductsService;
                userprofile.HolidaysTravel_ProductsService = command.HolidaysTravel_ProductsService;
                userprofile.Householdproducts_ProductsService = command.Householdproducts_ProductsService;
                userprofile.Motoring_ProductsService = command.Motoring_ProductsService;
                userprofile.NonAlcoholicDrinks_ProductsService = command.NonAlcoholicDrinks_ProductsService;
                userprofile.PetsPetFood_ProductsService = command.PetsPetFood_ProductsService;
                userprofile.PharmaceuticalChemistsProducts_ProductsService = command.PharmaceuticalChemistsProducts_ProductsService;
                userprofile.ShoppingRetailClothing_ProductsService = command.ShoppingRetailClothing_ProductsService;
                userprofile.SportsLeisure_ProductsService = command.SportsLeisure_ProductsService;
                userprofile.SweetSaltySnacks_ProductsService = command.SweetSaltySnacks_ProductsService;
                userprofile.TobaccoProducts_ProductsService = command.TobaccoProducts_ProductsService;
                userprofile.ToiletriesCosmetics_ProductsService = command.ToiletriesCosmetics_ProductsService;
                userprofile.UserProfileId = command.UserProfileId;
                userprofile.Id = userprofile.Id;
                _userProfilePreferenceRepository.Update(userprofile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}