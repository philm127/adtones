using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class UserMatch6
    {
        [Key]
        public int UserProfileId { get; set; }

        [StringLength(50)]
        public string Gender_Demographics { get; set; }

        [StringLength(50)]
        public string IncomeBracket_Demographics { get; set; }

        [StringLength(50)]
        public string WorkingStatus_Demographics { get; set; }

        [StringLength(50)]
        public string RelationshipStatus_Demographics { get; set; }

        [StringLength(50)]
        public string Education_Demographics { get; set; }

        [StringLength(50)]
        public string HouseholdStatus_Demographics { get; set; }

        [StringLength(50)]
        public string Location_Demographics { get; set; }

        [StringLength(50)]
        public string Food_Advert { get; set; }

        [StringLength(50)]
        public string SweetSaltySnacks_Advert { get; set; }

        [StringLength(50)]
        public string AlcoholicDrinks_Advert { get; set; }

        [StringLength(50)]
        public string NonAlcoholicDrinks_Advert { get; set; }

        [StringLength(50)]
        public string Householdproducts_Advert { get; set; }

        [StringLength(50)]
        public string ToiletriesCosmetics_Advert { get; set; }

        [StringLength(50)]
        public string PharmaceuticalChemistsProducts_Advert { get; set; }

        [StringLength(50)]
        public string TobaccoProducts_Advert { get; set; }

        [StringLength(50)]
        public string PetsPetFood_Advert { get; set; }

        [StringLength(50)]
        public string ShoppingRetailClothing_Advert { get; set; }

        [StringLength(50)]
        public string DIYGardening_Advert { get; set; }

        [StringLength(50)]
        public string AppliancesOtherHouseholdDurables_Advert { get; set; }

        [StringLength(50)]
        public string ElectronicsOtherPersonalItems_Advert { get; set; }

        [StringLength(50)]
        public string CommunicationsInternet_Advert { get; set; }

        [StringLength(50)]
        public string FinancialServices_Advert { get; set; }

        [StringLength(50)]
        public string HolidaysTravel_Advert { get; set; }

        [StringLength(50)]
        public string SportsLeisure_Advert { get; set; }

        [StringLength(50)]
        public string Motoring_Advert { get; set; }

        [StringLength(50)]
        public string Newspapers_Advert { get; set; }

        [StringLength(50)]
        public string Magazines_Advert { get; set; }

        [StringLength(50)]
        public string TV_Advert { get; set; }

        [StringLength(50)]
        public string Radio_Advert { get; set; }

        [StringLength(50)]
        public string Cinema_Advert { get; set; }

        [StringLength(50)]
        public string SocialNetworking_Advert { get; set; }

        [StringLength(50)]
        public string GeneralUse_Advert { get; set; }

        [StringLength(50)]
        public string Shopping_Advert { get; set; }

        [StringLength(50)]
        public string Fitness_Advert { get; set; }

        [StringLength(50)]
        public string Holidays_Advert { get; set; }

        [StringLength(50)]
        public string Environment_Advert { get; set; }

        [StringLength(50)]
        public string GoingOut_Advert { get; set; }

        [StringLength(50)]
        public string FinancialProducts_Advert { get; set; }

        [StringLength(50)]
        public string Religion_Advert { get; set; }

        [StringLength(50)]
        public string Fashion_Advert { get; set; }

        [StringLength(50)]
        public string Music_Advert { get; set; }

        [StringLength(50)]
        public string Fitness_Attitude { get; set; }

        [StringLength(50)]
        public string Holidays_Attitude { get; set; }

        [StringLength(50)]
        public string Environment_Attitude { get; set; }

        [StringLength(50)]
        public string GoingOut_Attitude { get; set; }

        [StringLength(50)]
        public string FinancialStabiity_Attitude { get; set; }

        [StringLength(50)]
        public string Religion_Attitude { get; set; }

        [StringLength(50)]
        public string Fashion_Attitude { get; set; }

        [StringLength(50)]
        public string Music_Attitude { get; set; }

        [StringLength(50)]
        public string Cinema_Cinema { get; set; }

        [StringLength(50)]
        public string SocialNetworking_Internet { get; set; }

        [StringLength(50)]
        public string Video_Internet { get; set; }

        [StringLength(50)]
        public string Research_Internet { get; set; }

        [StringLength(50)]
        public string Auctions_Internet { get; set; }

        [StringLength(50)]
        public string Shopping_Internet { get; set; }

        [StringLength(50)]
        public string ContractType_Mobile { get; set; }

        [StringLength(50)]
        public string Spend_Mobile { get; set; }

        [StringLength(50)]
        public string Local_Press { get; set; }

        [StringLength(50)]
        public string National_Press { get; set; }

        [StringLength(50)]
        public string FreeNewpapers_Press { get; set; }

        [StringLength(50)]
        public string Magazines_Press { get; set; }

        [StringLength(50)]
        public string Food_ProductsService { get; set; }

        [StringLength(50)]
        public string SweetSaltySnacks_ProductsService { get; set; }

        [StringLength(50)]
        public string AlcoholicDrinks_ProductsService { get; set; }

        [StringLength(50)]
        public string NonAlcoholicDrinks_ProductsService { get; set; }

        [StringLength(50)]
        public string Householdproducts_ProductsService { get; set; }

        [StringLength(50)]
        public string ToiletriesCosmetics_ProductsService { get; set; }

        [StringLength(50)]
        public string PharmaceuticalChemistsProducts_ProductsService { get; set; }

        [StringLength(50)]
        public string TobaccoProducts_ProductsService { get; set; }

        [StringLength(50)]
        public string PetsPetFood_ProductsService { get; set; }

        [StringLength(50)]
        public string ShoppingRetailClothing_ProductsService { get; set; }

        [StringLength(50)]
        public string DIYGardening_ProductsService { get; set; }

        [StringLength(50)]
        public string AppliancesOtherHouseholdDurables_ProductsService { get; set; }

        [StringLength(50)]
        public string ElectronicsOtherPersonalItems_ProductsService { get; set; }

        [StringLength(50)]
        public string CommunicationsInternet_ProductsService { get; set; }

        [StringLength(50)]
        public string FinancialServices_ProductsService { get; set; }

        [StringLength(50)]
        public string HolidaysTravel_ProductsService { get; set; }

        [StringLength(50)]
        public string SportsLeisure_ProductsService { get; set; }

        [StringLength(50)]
        public string Motoring_ProductsService { get; set; }

        [StringLength(50)]
        public string National_Radio { get; set; }

        [StringLength(50)]
        public string Local_Radio { get; set; }

        [StringLength(50)]
        public string Music_Radio { get; set; }

        [StringLength(50)]
        public string Sport_Radio { get; set; }

        [StringLength(50)]
        public string Talk_Radio { get; set; }

        [StringLength(50)]
        public string Satallite_TV { get; set; }

        [StringLength(50)]
        public string Cable_TV { get; set; }

        [StringLength(50)]
        public string Terrestrial_TV { get; set; }

        [StringLength(50)]
        public string Internet_TV { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [StringLength(250)]
        public string MSISDN { get; set; }

        public int? MSUserProfileId { get; set; }
        public int? UserId { get; set; }
        public DateTime? DOB { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string IncomeBracket { get; set; }

        [StringLength(50)]
        public string WorkingStatus { get; set; }

        [StringLength(50)]
        public string RelationshipStatus { get; set; }

        [StringLength(50)]
        public string Education { get; set; }

        [StringLength(50)]
        public string HouseholdStatus { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Age_Demographics { get; set; }


        [StringLength(50)]
        public string BusinessOrOpportunities_AdType { get; set; }

        [StringLength(50)]
        public string Gambling_AdType { get; set; }

        [StringLength(50)]
        public string Restaurants_AdType { get; set; }

        [StringLength(50)]
        public string Insurance_AdType { get; set; }

        [StringLength(50)]
        public string Furniture_AdType { get; set; }

        [StringLength(50)]
        public string InformationTechnology_AdType { get; set; }

        [StringLength(50)]
        public string Energy_AdType { get; set; }

        [StringLength(50)]
        public string Supermarkets_AdType { get; set; }

        [StringLength(50)]
        public string Healthcare_AdType { get; set; }

        [StringLength(50)]
        public string JobsAndEducation_AdType { get; set; }

        [StringLength(50)]
        public string Gifts_AdType { get; set; }

        [StringLength(50)]
        public string AdvocacyOrLegal_AdType { get; set; }

        [StringLength(50)]
        public string DatingAndPersonal_AdType { get; set; }

        [StringLength(50)]
        public string RealEstate_AdType { get; set; }

        [StringLength(50)]
        public string Games_AdType { get; set; }

        //[StringLength(50)]
        //public string SkizaProfile_AdType { get; set; }

        [StringLength(50)]
        public string Hustlers_AdType { get; set; }

        [StringLength(50)]
        public string Youth_AdType { get; set; }

        [StringLength(50)]
        public string DiscerningProfessionals_AdType { get; set; }

        [StringLength(50)]
        public string Mass_AdType { get; set; }

        public int? AdtoneServerUserMatch6Id { get; set; }
    }
}
