// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileAdvertFormModel.
    /// </summary>
    public class UserProfileAdvertFormModel
    {
        /// <summary>
        /// The _alcoholic drinks
        /// </summary>
        private string _AlcoholicDrinks_Advert;

        /// <summary>
        /// The _appliances other household durables
        /// </summary>
       // private string _AppliancesOtherHouseholdDurables_Advert;

        /// <summary>
        /// The _cinema
        /// </summary>
        private string _Cinema_Advert;

        /// <summary>
        /// The _communications internet
        /// </summary>
        private string _CommunicationsInternet_Advert;

        /// <summary>
        /// The _diy gardening
        /// </summary>
        private string _DIYGardening_Advert;

        /// <summary>
        /// The _electronics other personal items
        /// </summary>
        private string _ElectronicsOtherPersonalItems_Advert;

        /// <summary>
        /// The _environment
        /// </summary>
        private string _Environment_Advert;

        /// <summary>
        /// The _fashion
        /// </summary>
        //private string _Fashion_Advert;

        /// <summary>
        /// The _financial products
        /// </summary>
        //private string _FinancialProducts_Advert;

      

        /// <summary>
        /// The _fitness
        /// </summary>
        private string _Fitness_Advert;

        /// <summary>
        /// The _food
        /// </summary>
        private string _Food_Advert;

        /// <summary>
        /// The financial services_ advert
        /// </summary>
        public string _FinancialServices_Advert;

        /// <summary>
        /// The _general use
        /// </summary>
        //private string _GeneralUse_Advert;

        /// <summary>
        /// The _going out
        /// </summary>
        private string _GoingOut_Advert;

        /// <summary>
        /// The _holidays
        /// </summary>
       // private string _Holidays_Advert;

        /// <summary>
        /// The _holidays travel
        /// </summary>
        private string _HolidaysTravel_Advert;

        /// <summary>
        /// The _householdproducts
        /// </summary>
        private string _Householdproducts_Advert;

        /// <summary>
        /// The _magazines
        /// </summary>
       // private string _Magazines_Advert;

        /// <summary>
        /// The _motoring
        /// </summary>
        private string _Motoring_Advert;

        /// <summary>
        /// The _music
        /// </summary>
        private string _Music_Advert;

        /// <summary>
        /// The _newspapers
        /// </summary>
        private string _Newspapers_Advert;

        /// <summary>
        /// The _non alcoholic drinks
        /// </summary>
        private string _NonAlcoholicDrinks_Advert;

        /// <summary>
        /// The _pets pet food
        /// </summary>
        private string _PetsPetFood_Advert;

        /// <summary>
        /// The _pharmaceutical chemists products
        /// </summary>
        private string _PharmaceuticalChemistsProducts_Advert;

        /// <summary>
        /// The _radio
        /// </summary>
        //private string _Radio_Advert;

        /// <summary>
        /// The _religion
        /// </summary>
        private string _Religion_Advert;

        /// <summary>
        /// The _shopping
        /// </summary>
        private string _Shopping_Advert;

        /// <summary>
        /// The _shopping retail clothing
        /// </summary>
        private string _ShoppingRetailClothing_Advert;

        /// <summary>
        /// The _social networking
        /// </summary>
        private string _SocialNetworking_Advert;

        /// <summary>
        /// The _sports leisure
        /// </summary>
        private string _SportsLeisure_Advert;

        /// <summary>
        /// The _sweet salty snacks
        /// </summary>
        private string _SweetSaltySnacks_Advert;

        /// <summary>
        /// The _tobacco products
        /// </summary>
        private string _TobaccoProducts_Advert;

        /// <summary>
        /// The _toiletries cosmetics
        /// </summary>
        private string _ToiletriesCosmetics_Advert;

        /// <summary>
        /// The _TV
        /// </summary>
        private string _TV_Advert;


        private string _BusinessOrOpportunities_AdType;
        private string _Gambling_AdType;
        private string _Restaurants_AdType;
        private string _Insurance_AdType;
        private string _Furniture_AdType;
        private string _InformationTechnology_AdType;
        private string _Energy_AdType;
        private string _Supermarkets_AdType;
        private string _Healthcare_AdType;
        private string _JobsAndEducation_AdType;
        private string _Gifts_AdType;
        private string _AdvocacyOrLegal_AdType;
        private string _DatingAndPersonal_AdType;
        private string _RealEstate_AdType;
        private string _Games_AdType;

        private string _Hustlers_AdType;
        private string _Youth_AdType;
        private string _DiscerningProfessionals_AdType;
        private string _Mass_AdType;

        /// <summary>
        /// Gets or sets the user profile adverts identifier.
        /// </summary>
        /// <value>The user profile adverts identifier.</value>
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

        //Add 27-02-2019
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the food.
        /// </summary>
        /// <value>The food.</value>
        [Display(Name = "Food")]
        public string Food_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Food_Advert)) return "B";
                return _Food_Advert;
            }
            set { _Food_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the sweet salty snacks.
        /// </summary>
        /// <value>The sweet salty snacks.</value>
        [Display(Name = "Sweets/Snacks")]
        public string SweetSaltySnacks_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_SweetSaltySnacks_Advert)) return "B";
                return _SweetSaltySnacks_Advert;
            }
            set { _SweetSaltySnacks_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the alcoholic drinks.
        /// </summary>
        /// <value>The alcoholic drinks.</value>
        [Display(Name = "Alcoholic Drinks")]
        public string AlcoholicDrinks_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_AlcoholicDrinks_Advert)) return "B";
                return _AlcoholicDrinks_Advert;
            }
            set { _AlcoholicDrinks_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the non alcoholic drinks.
        /// </summary>
        /// <value>The non alcoholic drinks.</value>
        [Display(Name = "Non-Alcoholic Drinks")]
        public string NonAlcoholicDrinks_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_NonAlcoholicDrinks_Advert)) return "B";
                return _NonAlcoholicDrinks_Advert;
            }
            set { _NonAlcoholicDrinks_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the householdproducts.
        /// </summary>
        /// <value>The householdproducts.</value>
        [Display(Name = "Household Appliances/Products")]
        public string Householdproducts_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Householdproducts_Advert)) return "B";
                return _Householdproducts_Advert;
            }
            set { _Householdproducts_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the toiletries cosmetics.
        /// </summary>
        /// <value>The toiletries cosmetics.</value>
        [Display(Name = "Toiletries/Cosmetics")]
        public string ToiletriesCosmetics_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_ToiletriesCosmetics_Advert)) return "B";
                return _ToiletriesCosmetics_Advert;
            }
            set { _ToiletriesCosmetics_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products.
        /// </summary>
        /// <value>The pharmaceutical chemists products.</value>
        [Display(Name = "Pharmaceutical/Chemists Products")]
        public string PharmaceuticalChemistsProducts_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_PharmaceuticalChemistsProducts_Advert)) return "B";
                return _PharmaceuticalChemistsProducts_Advert;
            }
            set { _PharmaceuticalChemistsProducts_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the tobacco products.
        /// </summary>
        /// <value>The tobacco products.</value>
        [Display(Name = "Tobacco Products")]
        public string TobaccoProducts_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_TobaccoProducts_Advert)) return "B";
                return _TobaccoProducts_Advert;
            }
            set { _TobaccoProducts_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the pets pet food.
        /// </summary>
        /// <value>The pets pet food.</value>
        [Display(Name = "Pets")]
        public string PetsPetFood_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_PetsPetFood_Advert)) return "B";
                return _PetsPetFood_Advert;
            }
            set { _PetsPetFood_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the shopping retail clothing.
        /// </summary>
        /// <value>The shopping retail clothing.</value>
        [Display(Name = "Clothing/Fashion")]
        public string ShoppingRetailClothing_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_ShoppingRetailClothing_Advert)) return "B";
                return _ShoppingRetailClothing_Advert;
            }
            set { _ShoppingRetailClothing_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the diy gardening.
        /// </summary>
        /// <value>The diy gardening.</value>
        [Display(Name = "DIY/Gardening")]
        public string DIYGardening_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_DIYGardening_Advert)) return "B";
                return _DIYGardening_Advert;
            }
            set { _DIYGardening_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the appliances other household durables.
        /// </summary>
        /// <value>The appliances other household durables.</value>
        //[Display(Name = "Appliances/Other Household Durables")]
        //public string AppliancesOtherHouseholdDurables_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_AppliancesOtherHouseholdDurables_Advert)) return "B";
        //        return _AppliancesOtherHouseholdDurables_Advert;
        //    }
        //    set { _AppliancesOtherHouseholdDurables_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the electronics other personal items.
        /// </summary>
        /// <value>The electronics other personal items.</value>
        [Display(Name = "Electronics/Other Personal Items")]
        public string ElectronicsOtherPersonalItems_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_ElectronicsOtherPersonalItems_Advert)) return "B";
                return _ElectronicsOtherPersonalItems_Advert;
            }
            set { _ElectronicsOtherPersonalItems_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the communications internet.
        /// </summary>
        /// <value>The communications internet.</value>
        [Display(Name = "Communications/Internet Telecom")]
        public string CommunicationsInternet_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_CommunicationsInternet_Advert)) return "B";
                return _CommunicationsInternet_Advert;
            }
            set { _CommunicationsInternet_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the financial services.
        /// </summary>
        /// <value>The financial services.</value>
        [Display(Name = "Financial Services")]
        public string FinancialServices_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_FinancialServices_Advert)) return "B";
                return _FinancialServices_Advert;
            }
            set { _FinancialServices_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the holidays travel.
        /// </summary>
        /// <value>The holidays travel.</value>
        [Display(Name = "Holidays/Travel Tourism")]
        public string HolidaysTravel_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_HolidaysTravel_Advert)) return "B";
                return _HolidaysTravel_Advert;
            }
            set { _HolidaysTravel_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the sports leisure.
        /// </summary>
        /// <value>The sports leisure.</value>
        [Display(Name = "Sports/Leisure")]
        public string SportsLeisure_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_SportsLeisure_Advert)) return "B";
                return _SportsLeisure_Advert;
            }
            set { _SportsLeisure_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the motoring.
        /// </summary>
        /// <value>The motoring.</value>
        [Display(Name = "Motoring/Automotive")]
        public string Motoring_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Motoring_Advert)) return "B";
                return _Motoring_Advert;
            }
            set { _Motoring_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the newspapers.
        /// </summary>
        /// <value>The newspapers.</value>
        [Display(Name = "Newspapers/Magazines")]
        public string Newspapers_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Newspapers_Advert)) return "B";
                return _Newspapers_Advert;
            }
            set { _Newspapers_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the magazines.
        /// </summary>
        /// <value>The magazines.</value>
        //[Display(Name = "Magazines")]
        //public string Magazines_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_Magazines_Advert)) return "B";
        //        return _Magazines_Advert;
        //    }
        //    set { _Magazines_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the tv.
        /// </summary>
        /// <value>The tv.</value>
        [Display(Name = "TV/Video/Radio")]
        public string TV_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_TV_Advert)) return "B";
                return _TV_Advert;
            }
            set { _TV_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the radio.
        /// </summary>
        /// <value>The radio.</value>
        //[Display(Name = "Radio")]
        //public string Radio_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_Radio_Advert)) return "B";
        //        return _Radio_Advert;
        //    }
        //    set { _Radio_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the cinema.
        /// </summary>
        /// <value>The cinema.</value>
        [Display(Name = "Cinema")]
        public string Cinema_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Cinema_Advert)) return "B";
                return _Cinema_Advert;
            }
            set { _Cinema_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the social networking.
        /// </summary>
        /// <value>The social networking.</value>
        [Display(Name = "Social Networking")]
        public string SocialNetworking_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_SocialNetworking_Advert)) return "B";
                return _SocialNetworking_Advert;
            }
            set { _SocialNetworking_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the general use.
        /// </summary>
        /// <value>The general use.</value>
        //[Display(Name = "General Use")]
        //public string GeneralUse_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_GeneralUse_Advert)) return "B";
        //        return _GeneralUse_Advert;
        //    }
        //    set { _GeneralUse_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the shopping.
        /// </summary>
        /// <value>The shopping.</value>
        [Display(Name = "Shopping(retail gen merc)")]
        public string Shopping_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Shopping_Advert)) return "B";
                return _Shopping_Advert;
            }
            set { _Shopping_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>The fitness.</value>
        [Display(Name = "Fitness")]
        public string Fitness_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Fitness_Advert)) return "B";
                return _Fitness_Advert;
            }
            set { _Fitness_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        //[Display(Name = "Holidays")]
        //public string Holidays_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_Holidays_Advert)) return "B";
        //        return _Holidays_Advert;
        //    }
        //    set { _Holidays_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Display(Name = "Environment")]
        public string Environment_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Environment_Advert)) return "B";
                return _Environment_Advert;
            }
            set { _Environment_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the going out.
        /// </summary>
        /// <value>The going out.</value>
        [Display(Name = "Going Out/Entertainment")]
        public string GoingOut_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_GoingOut_Advert)) return "B";
                return _GoingOut_Advert;
            }
            set { _GoingOut_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the financial products.
        /// </summary>
        /// <value>The financial products.</value>
        //[Display(Name = "Financial Products")]
        //public string FinancialProducts_Advert
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_FinancialProducts_Advert)) return "B";
        //        return _FinancialProducts_Advert;
        //    }
        //    set { _FinancialProducts_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>The religion.</value>
        [Display(Name = "Religion")]
        public string Religion_Advert
        {
            get
            {
                if (String.IsNullOrEmpty(_Religion_Advert)) return "B";
                return _Religion_Advert;
            }
            set { _Religion_Advert = value; }
        }

        /// <summary>
        /// Gets or sets the fashion.
        /// </summary>
        /// <value>The fashion.</value>
        //[Display(Name = "Fashion")]
        //public string Fashion_Advert
        //{
        //    get
        //    {
        //        if (_Fashion_Advert == null) return "B";
        //        return _Fashion_Advert;
        //    }
        //    set { _Fashion_Advert = value; }
        //}

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        [Display(Name = "Music")]
        public string Music_Advert
        {
            get
            {
                if (_Music_Advert == null) return "B";
                return _Music_Advert;
            }
            set { _Music_Advert = value; }
        }

        [Display(Name = "Business/opportunities")]
        public string BusinessOrOpportunities_AdType
        {
            get
            {
                if (_BusinessOrOpportunities_AdType == null) return "B";
                return _BusinessOrOpportunities_AdType;
            }
            set { _BusinessOrOpportunities_AdType = value; }
        }

        [Display(Name = "Over 18/Gambling")]
        public string Gambling_AdType
        {
            get
            {
                if (_Gambling_AdType == null) return "B";
                return _Gambling_AdType;
            }
            set { _Gambling_AdType = value; }
        }

        [Display(Name = "Restaurants")]
        public string Restaurants_AdType
        {
            get
            {
                if (_Restaurants_AdType == null) return "B";
                return _Restaurants_AdType;
            }
            set { _Restaurants_AdType = value; }
        }


        [Display(Name = "Insurance")]
        public string Insurance_AdType
        {
            get
            {
                if (_Insurance_AdType == null) return "B";
                return _Insurance_AdType;
            }
            set { _Insurance_AdType = value; }
        }

        [Display(Name = "Furniture")]
        public string Furniture_AdType
        {
            get
            {
                if (_Furniture_AdType == null) return "B";
                return _Furniture_AdType;
            }
            set { _Furniture_AdType = value; }
        }

        [Display(Name = "Information technology")]
        public string InformationTechnology_AdType
        {
            get
            {
                if (_InformationTechnology_AdType == null) return "B";
                return _InformationTechnology_AdType;
            }
            set { _InformationTechnology_AdType = value; }
        }

        [Display(Name = "Energy")]
        public string Energy_AdType
        {
            get
            {
                if (_Energy_AdType == null) return "B";
                return _Energy_AdType;
            }
            set { _Energy_AdType = value; }
        }

        [Display(Name = "Supermarkets")]
        public string Supermarkets_AdType
        {
            get
            {
                if (_Supermarkets_AdType == null) return "B";
                return _Supermarkets_AdType;
            }
            set { _Supermarkets_AdType = value; }
        }

        [Display(Name = "Healthcare")]
        public string Healthcare_AdType
        {
            get
            {
                if (_Healthcare_AdType == null) return "B";
                return _Healthcare_AdType;
            }
            set { _Healthcare_AdType = value; }
        }

        [Display(Name = "Jobs and Education")]
        public string JobsAndEducation_AdType
        {
            get
            {
                if (_JobsAndEducation_AdType == null) return "B";
                return _JobsAndEducation_AdType;
            }
            set { _JobsAndEducation_AdType = value; }
        }

        [Display(Name = "Gifts")]
        public string Gifts_AdType
        {
            get
            {
                if (_Gifts_AdType == null) return "B";
                return _Gifts_AdType;
            }
            set { _Gifts_AdType = value; }
        }

        [Display(Name = "Advocacy/Legal")]
        public string AdvocacyOrLegal_AdType
        {
            get
            {
                if (_AdvocacyOrLegal_AdType == null) return "B";
                return _AdvocacyOrLegal_AdType;
            }
            set { _AdvocacyOrLegal_AdType = value; }
        }

        [Display(Name = "Dating & Personal")]
        public string DatingAndPersonal_AdType
        {
            get
            {
                if (_DatingAndPersonal_AdType == null) return "B";
                return _DatingAndPersonal_AdType;
            }
            set { _DatingAndPersonal_AdType = value; }
        }

        [Display(Name = "Real Estate/Property")]
        public string RealEstate_AdType
        {
            get
            {
                if (_RealEstate_AdType == null) return "B";
                return _RealEstate_AdType;
            }
            set { _RealEstate_AdType = value; }
        }

        [Display(Name = "Games")]
        public string Games_AdType
        {
            get
            {
                if (_Games_AdType == null) return "B";
                return _Games_AdType;
            }
            set { _Games_AdType = value; }
        }

        [Display(Name = "Hustlers")]
        public string Hustlers_AdType
        {
            get
            {
                if (_Hustlers_AdType == null) return "B";
                return _Hustlers_AdType;
            }
            set { _Hustlers_AdType = value; }
        }

        [Display(Name = "Youth")]
        public string Youth_AdType
        {
            get
            {
                if (_Youth_AdType == null) return "B";
                return _Youth_AdType;
            }
            set { _Youth_AdType = value; }
        }

        [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType
        {
            get
            {
                if (_DiscerningProfessionals_AdType == null) return "B";
                return _DiscerningProfessionals_AdType;
            }
            set { _DiscerningProfessionals_AdType = value; }
        }

        [Display(Name = "Mass")]
        public string Mass_AdType
        {
            get
            {
                if (_Mass_AdType == null) return "B";
                return _Mass_AdType;
            }
            set { _Mass_AdType = value; }
        }

        #region Country Wise Hide Show Option
        public bool Food { get; set; }
        public bool SweetsSnacks { get; set;}
        public bool AlcoholicDrinks { get; set; }
        public bool NonAlcoholicDrinks { get; set; }
        public bool HouseholdAppliancesProducts { get; set; }
        public bool ToiletriesCosmetics { get; set; }
        public bool PharmaceuticalChemistsProducts { get; set; }
        public bool TobaccoProducts { get; set; }
        public bool Pets { get; set; }
        public bool ClothingFashion { get; set; }
        public bool DIYGardening { get; set; }
        public bool ElectronicsOtherPersonalItems { get; set; }
        public bool CommunicationsInternetTelecom { get; set; }
        public bool FinancialServices { get; set; }
        public bool HolidaysTravelTourism { get; set; }
        public bool SportsLeisure { get; set; }
        public bool MotoringAutomotive { get; set; }
        public bool NewspapersMagazines { get; set; }
        public bool TvVideoRadio { get; set; }
        public bool Cinema { get; set; }
        public bool SocialNetworking { get; set; }
        public bool Shopping { get; set; }
        public bool Fitness { get; set; }
        public bool Environment { get; set; }
        public bool GoingOutEntertainment { get; set; }
        public bool Religion { get; set; }
        public bool Music { get; set; }
        public bool BusinessOpportunities { get; set; }
        public bool Over18Gambling { get; set; }
        public bool Restaurants { get; set; }
        public bool Insurance { get; set; }
        public bool Furniture { get; set; }
        public bool Informationtechnology { get; set; }
        public bool Energy { get; set; }
        public bool Supermarkets { get; set; }
        public bool Healthcare { get; set; }
        public bool JobsandEducation { get; set; }
        public bool Gifts { get; set; }
        public bool AdvocacyLegal { get; set; }
        public bool DatingPersonal { get; set; }
        public bool RealEstateProperty { get; set; }
        public bool Games { get; set; }
        public bool SkizaProfile { get; set; }
        #endregion
    }
}