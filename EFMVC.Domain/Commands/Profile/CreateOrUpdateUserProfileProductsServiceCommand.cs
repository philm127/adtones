// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileProductsServiceCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileProductsServiceCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileProductsServiceCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user profile products services identifier.
        /// </summary>
        /// <value>The user profile products services identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the food.
        /// </summary>
        /// <value>The food.</value>
        public string Food_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the sweet salty snacks.
        /// </summary>
        /// <value>The sweet salty snacks.</value>
        public string SweetSaltySnacks_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the alcoholic drinks.
        /// </summary>
        /// <value>The alcoholic drinks.</value>
        public string AlcoholicDrinks_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the non alcoholic drinks.
        /// </summary>
        /// <value>The non alcoholic drinks.</value>
        public string NonAlcoholicDrinks_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the householdproducts.
        /// </summary>
        /// <value>The householdproducts.</value>
        public string Householdproducts_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the toiletries cosmetics.
        /// </summary>
        /// <value>The toiletries cosmetics.</value>
        public string ToiletriesCosmetics_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products.
        /// </summary>
        /// <value>The pharmaceutical chemists products.</value>
        public string PharmaceuticalChemistsProducts_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the tobacco products.
        /// </summary>toiletriesCosmetics
        /// <value>The tobacco products.</value>
        public string TobaccoProducts_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the pets pet food.
        /// </summary>
        /// <value>The pets pet food.</value>
        public string PetsPetFood_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the shopping retail clothing.
        /// </summary>
        /// <value>The shopping retail clothing.</value>
        public string ShoppingRetailClothing_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the diy gardening.
        /// </summary>
        /// <value>The diy gardening.</value>
        public string DIYGardening_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the appliances other household durables.
        /// </summary>
        /// <value>The appliances other household durables.</value>
        public string AppliancesOtherHouseholdDurables_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the electronics other personal items.
        /// </summary>
        /// <value>The electronics other personal items.</value>
        public string ElectronicsOtherPersonalItems_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the communications internet.
        /// </summary>
        /// <value>The communications internet.</value>
        public string CommunicationsInternet_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the financial services.
        /// </summary>
        /// <value>The financial services.</value>
        public string FinancialServices_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the holidays travel.
        /// </summary>
        /// <value>The holidays travel.</value>
        public string HolidaysTravel_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the sports leisure.
        /// </summary>
        /// <value>The sports leisure.</value>
        public string SportsLeisure_ProductsService { get; set; }

        /// <summary>
        /// Gets or sets the motoring.
        /// </summary>
        /// <value>The motoring.</value>
        public string Motoring_ProductsService { get; set; }
    }
}