using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model
{
    public class CampaignMatch4
    {
        [Key]
        public int CampaignProfileId { get; set; }

        [StringLength(100)]
        public string Budget { get; set; }
        public int? MaxBid { get; set; }

        [StringLength(50)]
        public string SpentToDate { get; set; }

        [StringLength(50)]
        public string AvailableCredit { get; set; }

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
        public string DOBStart_Demographics { get; set; }

        [StringLength(50)]
        public string DOBEnd_Demographics { get; set; }

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
        public string Age_Demographics { get; set; }

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

        [StringLength(2500)]
        public string EMAIL_MESSAGE { get; set; }

        [StringLength(2500)]
        public string MEDIA_URL { get; set; }

        [StringLength(500)]
        public string SMS_MESSAGE { get; set; }

        [StringLength(500)]
        public string ORIGINATOR { get; set; }


        public int? MSCampaignProfileId { get; set; }
        public int? UserId { get; set; }
        public int? ClientId { get; set; }

        [StringLength(500)]
        public string CampaignName { get; set; }

        [StringLength(1000)]
        public string CampaignDescription { get; set; }


        public decimal? TotalBudget { get; set; }
        public decimal? MaxDailyBudget { get; set; }
        public decimal? MaxMonthBudget { get; set; }
        public decimal? MaxWeeklyBudget { get; set; }
        public decimal? MaxHourlyBudget { get; set; }
        public decimal? TotalCredit { get; set; }


        public int? PlaysToDate { get; set; }
        public int? PlaysLastMonth { get; set; }
        public int? PlaysCurrentMonth { get; set; }
        public int? CancelledToDate { get; set; }
        public int? CancelledLastMonth { get; set; }
        public int? CancelledCurrentMonth { get; set; }
        public int? SmsToDate { get; set; }
        public int? SmsLastMonth { get; set; }
        public int? SmsCurrentMonth { get; set; }
        public int? EmailToDate { get; set; }
        public int? EmailsLastMonth { get; set; }
        public int? EmailsCurrentMonth { get; set; }

        [StringLength(400)]
        public string EmailFileLocation { get; set; }
        public bool? Active { get; set; }

        public int? NumberOfPlays { get; set; }
        public int? AverageDailyPlays { get; set; }
        public int? SmsRequests { get; set; }
        public int? EmailsDelievered { get; set; }

        [StringLength(1000)]
        public string EmailSubject { get; set; }

        [StringLength(1000)]
        public string EmailBody { get; set; }

        [StringLength(1000)]
        public string EmailSenderAddress { get; set; }

        [StringLength(1000)]
        public string SmsOriginator { get; set; }

        [StringLength(1000)]
        public string SmsBody { get; set; }

        [StringLength(1000)]
        public string SMSFileLocation { get; set; }


        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumberInBatch { get; set; }

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

        public int? CountryId { get; set; }

        public float RemainingMaxMonthBudget { get; set; }
        public float RemainingMaxDailyBudget { get; set; }
        public float RemainingMaxWeeklyBudget { get; set; }
        public float RemainingMaxHourlyBudget { get; set; }
    }
}
