//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.ProvisioningModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class campaignmatch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public campaignmatch()
        {
            this.campaignmatchusermatches = new HashSet<campaignmatchusermatch>();
        }
    
        public int CampaignProfileId { get; set; }
        public string Budget { get; set; }
        public Nullable<int> MaxBid { get; set; }
        public string SpentToDate { get; set; }
        public string AvailableCredit { get; set; }
        public string Food_Advert { get; set; }
        public string SweetSaltySnacks_Advert { get; set; }
        public string AlcoholicDrinks_Advert { get; set; }
        public string NonAlcoholicDrinks_Advert { get; set; }
        public string Householdproducts_Advert { get; set; }
        public string ToiletriesCosmetics_Advert { get; set; }
        public string PharmaceuticalChemistsProducts_Advert { get; set; }
        public string TobaccoProducts_Advert { get; set; }
        public string PetsPetFood_Advert { get; set; }
        public string ShoppingRetailClothing_Advert { get; set; }
        public string DIYGardening_Advert { get; set; }
        public string AppliancesOtherHouseholdDurables_Advert { get; set; }
        public string ElectronicsOtherPersonalItems_Advert { get; set; }
        public string CommunicationsInternet_Advert { get; set; }
        public string FinancialServices_Advert { get; set; }
        public string HolidaysTravel_Advert { get; set; }
        public string SportsLeisure_Advert { get; set; }
        public string Motoring_Advert { get; set; }
        public string Newspapers_Advert { get; set; }
        public string Magazines_Advert { get; set; }
        public string TV_Advert { get; set; }
        public string Radio_Advert { get; set; }
        public string Cinema_Advert { get; set; }
        public string SocialNetworking_Advert { get; set; }
        public string GeneralUse_Advert { get; set; }
        public string Shopping_Advert { get; set; }
        public string Fitness_Advert { get; set; }
        public string Holidays_Advert { get; set; }
        public string Environment_Advert { get; set; }
        public string GoingOut_Advert { get; set; }
        public string FinancialProducts_Advert { get; set; }
        public string Religion_Advert { get; set; }
        public string Fashion_Advert { get; set; }
        public string Music_Advert { get; set; }
        public string Fitness_Attitude { get; set; }
        public string Holidays_Attitude { get; set; }
        public string Environment_Attitude { get; set; }
        public string GoingOut_Attitude { get; set; }
        public string FinancialStabiity_Attitude { get; set; }
        public string Religion_Attitude { get; set; }
        public string Fashion_Attitude { get; set; }
        public string Music_Attitude { get; set; }
        public string Cinema_Cinema { get; set; }
        public Nullable<System.DateTime> DOBStart_Demographics { get; set; }
        public Nullable<System.DateTime> DOBEnd_Demographics { get; set; }
        public string Gender_Demographics { get; set; }
        public string IncomeBracket_Demographics { get; set; }
        public string WorkingStatus_Demographics { get; set; }
        public string RelationshipStatus_Demographics { get; set; }
        public string Education_Demographics { get; set; }
        public string HouseholdStatus_Demographics { get; set; }
        public string Location_Demographics { get; set; }
        public string Age_Demographics { get; set; }
        public string SocialNetworking_Internet { get; set; }
        public string Video_Internet { get; set; }
        public string Research_Internet { get; set; }
        public string Auctions_Internet { get; set; }
        public string Shopping_Internet { get; set; }
        public string ContractType_Mobile { get; set; }
        public string Spend_Mobile { get; set; }
        public string Local_Press { get; set; }
        public string National_Press { get; set; }
        public string FreeNewpapers_Press { get; set; }
        public string Magazines_Press { get; set; }
        public string Food_ProductsService { get; set; }
        public string SweetSaltySnacks_ProductsService { get; set; }
        public string AlcoholicDrinks_ProductsService { get; set; }
        public string NonAlcoholicDrinks_ProductsService { get; set; }
        public string Householdproducts_ProductsService { get; set; }
        public string ToiletriesCosmetics_ProductsService { get; set; }
        public string PharmaceuticalChemistsProducts_ProductsService { get; set; }
        public string TobaccoProducts_ProductsService { get; set; }
        public string PetsPetFood_ProductsService { get; set; }
        public string ShoppingRetailClothing_ProductsService { get; set; }
        public string DIYGardening_ProductsService { get; set; }
        public string AppliancesOtherHouseholdDurables_ProductsService { get; set; }
        public string ElectronicsOtherPersonalItems_ProductsService { get; set; }
        public string CommunicationsInternet_ProductsService { get; set; }
        public string FinancialServices_ProductsService { get; set; }
        public string HolidaysTravel_ProductsService { get; set; }
        public string SportsLeisure_ProductsService { get; set; }
        public string Motoring_ProductsService { get; set; }
        public string National_Radio { get; set; }
        public string Local_Radio { get; set; }
        public string Music_Radio { get; set; }
        public string Sport_Radio { get; set; }
        public string Talk_Radio { get; set; }
        public string Satallite_TV { get; set; }
        public string Cable_TV { get; set; }
        public string Terrestrial_TV { get; set; }
        public string Internet_TV { get; set; }
        public string EMAIL_MESSAGE { get; set; }
        public string MEDIA_URL { get; set; }
        public string SMS_MESSAGE { get; set; }
        public string ORIGINATOR { get; set; }
        public Nullable<int> MSCampaignProfileId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDescription { get; set; }
        public Nullable<decimal> TotalBudget { get; set; }
        public Nullable<decimal> MaxDailyBudget { get; set; }
        public Nullable<decimal> MaxMonthBudget { get; set; }
        public Nullable<decimal> MaxWeeklyBudget { get; set; }
        public Nullable<decimal> MaxHourlyBudget { get; set; }
        public Nullable<decimal> TotalCredit { get; set; }
        public Nullable<int> PlaysToDate { get; set; }
        public Nullable<int> PlaysLastMonth { get; set; }
        public Nullable<int> PlaysCurrentMonth { get; set; }
        public Nullable<int> CancelledToDate { get; set; }
        public Nullable<int> CancelledLastMonth { get; set; }
        public Nullable<int> CancelledCurrentMonth { get; set; }
        public Nullable<int> SmsToDate { get; set; }
        public Nullable<int> SmsLastMonth { get; set; }
        public Nullable<int> SmsCurrentMonth { get; set; }
        public Nullable<int> EmailToDate { get; set; }
        public Nullable<int> EmailsLastMonth { get; set; }
        public Nullable<int> EmailsCurrentMonth { get; set; }
        public string EmailFileLocation { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> NumberOfPlays { get; set; }
        public Nullable<int> AverageDailyPlays { get; set; }
        public Nullable<int> SmsRequests { get; set; }
        public Nullable<int> EmailsDelievered { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailSenderAddress { get; set; }
        public string SmsOriginator { get; set; }
        public string SmsBody { get; set; }
        public string SMSFileLocation { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> NumberInBatch { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<campaignmatchusermatch> campaignmatchusermatches { get; set; }
    }
}