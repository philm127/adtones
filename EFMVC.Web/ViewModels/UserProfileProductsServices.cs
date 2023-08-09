// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileProductsServices.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileProductsServiceFormModel.
    /// </summary>
    public class UserProfileProductsServiceFormModel
    {
        /// <summary>
        /// The _alcoholic drinks
        /// </summary>
        private string _AlcoholicDrinks_ProductsService;

        /// <summary>
        /// The _appliances other household durables
        /// </summary>
        private string _AppliancesOtherHouseholdDurables_ProductsService;

        /// <summary>
        /// The _communications internet
        /// </summary>
        private string _CommunicationsInternet_ProductsService;

        /// <summary>
        /// The _diy gardening
        /// </summary>
        private string _DIYGardening_ProductsService;

        /// <summary>
        /// The _electronics other personal items
        /// </summary>
        private string _ElectronicsOtherPersonalItems_ProductsService;

        /// <summary>
        /// The _financial services
        /// </summary>
        private string _FinancialServices_ProductsService;

        /// <summary>
        /// The _food
        /// </summary>
        private string _Food_ProductsService;

        /// <summary>
        /// The _holidays travel
        /// </summary>
        private string _HolidaysTravel_ProductsService;

        /// <summary>
        /// The _householdproducts
        /// </summary>
        private string _Householdproducts_ProductsService;

        /// <summary>
        /// The _motoring
        /// </summary>
        private string _Motoring_ProductsService;

        /// <summary>
        /// The _non alcoholic drinks
        /// </summary>
        private string _NonAlcoholicDrinks_ProductsService;

        /// <summary>
        /// The _pets pet food
        /// </summary>
        private string _PetsPetFood_ProductsService;

        /// <summary>
        /// The _pharmaceutical chemists products
        /// </summary>
        private string _PharmaceuticalChemistsProducts_ProductsService;

        /// <summary>
        /// The _shopping retail clothing
        /// </summary>
        private string _ShoppingRetailClothing_ProductsService;

        /// <summary>
        /// The _sports leisure
        /// </summary>
        private string _SportsLeisure_ProductsService;

        /// <summary>
        /// The _sweet salty snacks
        /// </summary>
        private string _SweetSaltySnacks_ProductsService;

        /// <summary>
        /// The _tobacco products
        /// </summary>
        private string _TobaccoProducts_ProductsService;

        /// <summary>
        /// The _toiletries cosmetics
        /// </summary>
        private string _ToiletriesCosmetics_ProductsService;

        /// <summary>
        /// Gets or sets the user profile products services identifier.
        /// </summary>
        /// <value>The user profile products services identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public UserProfileFormModel UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the food.
        /// </summary>
        /// <value>The food.</value>
        [Display(Name = "Food")]
        public string Food_ProductsService
        {
            get
            {
                if (_Food_ProductsService == null) return "A";
                return _Food_ProductsService;
            }
            set { _Food_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the sweet salty snacks.
        /// </summary>
        /// <value>The sweet salty snacks.</value>
        [Display(Name = "Sweet/Salty Snacks")]
        public string SweetSaltySnacks_ProductsService
        {
            get
            {
                if (_SweetSaltySnacks_ProductsService == null) return "A";
                return _SweetSaltySnacks_ProductsService;
            }
            set { _SweetSaltySnacks_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the alcoholic drinks.
        /// </summary>
        /// <value>The alcoholic drinks.</value>
        [Display(Name = "Alcoholic Drinks")]
        public string AlcoholicDrinks_ProductsService
        {
            get
            {
                if (_AlcoholicDrinks_ProductsService == null) return "A";
                return _AlcoholicDrinks_ProductsService;
            }
            set { _AlcoholicDrinks_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the non alcoholic drinks.
        /// </summary>
        /// <value>The non alcoholic drinks.</value>
        [Display(Name = "Non-Alcoholic Drinks")]
        public string NonAlcoholicDrinks_ProductsService
        {
            get
            {
                if (_NonAlcoholicDrinks_ProductsService == null) return "A";
                return _NonAlcoholicDrinks_ProductsService;
            }
            set { _NonAlcoholicDrinks_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the householdproducts.
        /// </summary>
        /// <value>The householdproducts.</value>
        [Display(Name = "Household Products")]
        public string Householdproducts_ProductsService
        {
            get
            {
                if (_Householdproducts_ProductsService == null) return "A";
                return _Householdproducts_ProductsService;
            }
            set { _Householdproducts_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the toiletries cosmetics.
        /// </summary>
        /// <value>The toiletries cosmetics.</value>
        [Display(Name = "Toiletries/Cosmetics")]
        public string ToiletriesCosmetics_ProductsService
        {
            get
            {
                if (_ToiletriesCosmetics_ProductsService == null) return "A";
                return _ToiletriesCosmetics_ProductsService;
            }
            set { _ToiletriesCosmetics_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products.
        /// </summary>
        /// <value>The pharmaceutical chemists products.</value>
        [Display(Name = "Pharmaceutical/Chemists Products")]
        public string PharmaceuticalChemistsProducts_ProductsService
        {
            get
            {
                if (_PharmaceuticalChemistsProducts_ProductsService == null) return "A";
                return _PharmaceuticalChemistsProducts_ProductsService;
            }
            set { _PharmaceuticalChemistsProducts_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the tobacco products.
        /// </summary>
        /// <value>The tobacco products.</value>
        [Display(Name = "Tobacco Products")]
        public string TobaccoProducts_ProductsService
        {
            get
            {
                if (_TobaccoProducts_ProductsService == null) return "A";
                return _TobaccoProducts_ProductsService;
            }
            set { _TobaccoProducts_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the pets pet food.
        /// </summary>
        /// <value>The pets pet food.</value>
        [Display(Name = "Pet Food")]
        public string PetsPetFood_ProductsService
        {
            get
            {
                if (_PetsPetFood_ProductsService == null) return "A";
                return _PetsPetFood_ProductsService;
            }
            set { _PetsPetFood_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the shopping retail clothing.
        /// </summary>
        /// <value>The shopping retail clothing.</value>
        [Display(Name = "Shopping/Retail/Clothing")]
        public string ShoppingRetailClothing_ProductsService
        {
            get
            {
                if (_ShoppingRetailClothing_ProductsService == null) return "A";
                return _ShoppingRetailClothing_ProductsService;
            }
            set { _ShoppingRetailClothing_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the diy gardening.
        /// </summary>
        /// <value>The diy gardening.</value>
        [Display(Name = "DIY/Gardening")]
        public string DIYGardening_ProductsService
        {
            get
            {
                if (_DIYGardening_ProductsService == null) return "A";
                return _DIYGardening_ProductsService;
            }
            set { _DIYGardening_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the appliances other household durables.
        /// </summary>
        /// <value>The appliances other household durables.</value>
        [Display(Name = "Appliances/Other Household Durables")]
        public string AppliancesOtherHouseholdDurables_ProductsService
        {
            get
            {
                if (_AppliancesOtherHouseholdDurables_ProductsService == null) return "A";
                return _AppliancesOtherHouseholdDurables_ProductsService;
            }
            set { _AppliancesOtherHouseholdDurables_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the electronics other personal items.
        /// </summary>
        /// <value>The electronics other personal items.</value>
        [Display(Name = "Electronics/Other Personal Items")]
        public string ElectronicsOtherPersonalItems_ProductsService
        {
            get
            {
                if (_ElectronicsOtherPersonalItems_ProductsService == null) return "A";
                return _ElectronicsOtherPersonalItems_ProductsService;
            }
            set { _ElectronicsOtherPersonalItems_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the communications internet.
        /// </summary>
        /// <value>The communications internet.</value>
        [Display(Name = "Communications/Internet")]
        public string CommunicationsInternet_ProductsService
        {
            get
            {
                if (_CommunicationsInternet_ProductsService == null) return "A";
                return _CommunicationsInternet_ProductsService;
            }
            set { _CommunicationsInternet_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the financial services.
        /// </summary>
        /// <value>The financial services.</value>
        [Display(Name = "Financial Services")]
        public string FinancialServices_ProductsService
        {
            get
            {
                if (_FinancialServices_ProductsService == null) return "A";
                return _FinancialServices_ProductsService;
            }
            set { _FinancialServices_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the holidays travel.
        /// </summary>
        /// <value>The holidays travel.</value>
        [Display(Name = "Holidays/Travel")]
        public string HolidaysTravel_ProductsService
        {
            get
            {
                if (_HolidaysTravel_ProductsService == null) return "A";
                return _HolidaysTravel_ProductsService;
            }
            set { _HolidaysTravel_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the sports leisure.
        /// </summary>
        /// <value>The sports leisure.</value>
        [Display(Name = "Sports/Leisure")]
        public string SportsLeisure_ProductsService
        {
            get
            {
                if (_SportsLeisure_ProductsService == null) return "A";
                return _SportsLeisure_ProductsService;
            }
            set { _SportsLeisure_ProductsService = value; }
        }

        /// <summary>
        /// Gets or sets the motoring.
        /// </summary>
        /// <value>The motoring.</value>
        [Display(Name = "Motoring")]
        public string Motoring_ProductsService
        {
            get
            {
                if (_Motoring_ProductsService == null) return "A";
                return _Motoring_ProductsService;
            }
            set { _Motoring_ProductsService = value; }
        }
    }
}