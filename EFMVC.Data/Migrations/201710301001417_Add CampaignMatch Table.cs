namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignMatchTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignMatches",
                c => new
                    {
                        CampaignProfileId = c.Int(nullable: false, identity: true),
                        Budget = c.String(maxLength: 100),
                        MaxBid = c.Int(),
                        SpentToDate = c.String(maxLength: 50),
                        AvailableCredit = c.String(maxLength: 50),
                        Food_Advert = c.String(maxLength: 50),
                        SweetSaltySnacks_Advert = c.String(maxLength: 50),
                        AlcoholicDrinks_Advert = c.String(maxLength: 50),
                        NonAlcoholicDrinks_Advert = c.String(maxLength: 50),
                        Householdproducts_Advert = c.String(maxLength: 50),
                        ToiletriesCosmetics_Advert = c.String(maxLength: 50),
                        PharmaceuticalChemistsProducts_Advert = c.String(maxLength: 50),
                        TobaccoProducts_Advert = c.String(maxLength: 50),
                        PetsPetFood_Advert = c.String(maxLength: 50),
                        ShoppingRetailClothing_Advert = c.String(maxLength: 50),
                        DIYGardening_Advert = c.String(maxLength: 50),
                        AppliancesOtherHouseholdDurables_Advert = c.String(maxLength: 50),
                        ElectronicsOtherPersonalItems_Advert = c.String(maxLength: 50),
                        CommunicationsInternet_Advert = c.String(maxLength: 50),
                        FinancialServices_Advert = c.String(maxLength: 50),
                        HolidaysTravel_Advert = c.String(maxLength: 50),
                        SportsLeisure_Advert = c.String(maxLength: 50),
                        Motoring_Advert = c.String(maxLength: 50),
                        Newspapers_Advert = c.String(maxLength: 50),
                        Magazines_Advert = c.String(maxLength: 50),
                        TV_Advert = c.String(maxLength: 50),
                        Radio_Advert = c.String(maxLength: 50),
                        Cinema_Advert = c.String(maxLength: 50),
                        SocialNetworking_Advert = c.String(maxLength: 50),
                        GeneralUse_Advert = c.String(maxLength: 50),
                        Shopping_Advert = c.String(maxLength: 50),
                        Fitness_Advert = c.String(maxLength: 50),
                        Holidays_Advert = c.String(maxLength: 50),
                        Environment_Advert = c.String(maxLength: 50),
                        GoingOut_Advert = c.String(maxLength: 50),
                        FinancialProducts_Advert = c.String(maxLength: 50),
                        Religion_Advert = c.String(maxLength: 50),
                        Fashion_Advert = c.String(maxLength: 50),
                        Music_Advert = c.String(maxLength: 50),
                        Fitness_Attitude = c.String(maxLength: 50),
                        Holidays_Attitude = c.String(maxLength: 50),
                        Environment_Attitude = c.String(maxLength: 50),
                        GoingOut_Attitude = c.String(maxLength: 50),
                        FinancialStabiity_Attitude = c.String(maxLength: 50),
                        Religion_Attitude = c.String(maxLength: 50),
                        Fashion_Attitude = c.String(maxLength: 50),
                        Music_Attitude = c.String(maxLength: 50),
                        Cinema_Cinema = c.String(maxLength: 50),
                        DOBStart_Demographics = c.String(maxLength: 50),
                        DOBEnd_Demographics = c.String(maxLength: 50),
                        Gender_Demographics = c.String(maxLength: 50),
                        IncomeBracket_Demographics = c.String(maxLength: 50),
                        WorkingStatus_Demographics = c.String(maxLength: 50),
                        RelationshipStatus_Demographics = c.String(maxLength: 50),
                        Education_Demographics = c.String(maxLength: 50),
                        HouseholdStatus_Demographics = c.String(maxLength: 50),
                        Location_Demographics = c.String(maxLength: 50),
                        Age_Demographics = c.String(maxLength: 50),
                        SocialNetworking_Internet = c.String(maxLength: 50),
                        Video_Internet = c.String(maxLength: 50),
                        Research_Internet = c.String(maxLength: 50),
                        Auctions_Internet = c.String(maxLength: 50),
                        Shopping_Internet = c.String(maxLength: 50),
                        ContractType_Mobile = c.String(maxLength: 50),
                        Spend_Mobile = c.String(maxLength: 50),
                        Local_Press = c.String(maxLength: 50),
                        National_Press = c.String(maxLength: 50),
                        FreeNewpapers_Press = c.String(maxLength: 50),
                        Magazines_Press = c.String(maxLength: 50),
                        Food_ProductsService = c.String(maxLength: 50),
                        SweetSaltySnacks_ProductsService = c.String(maxLength: 50),
                        AlcoholicDrinks_ProductsService = c.String(maxLength: 50),
                        NonAlcoholicDrinks_ProductsService = c.String(maxLength: 50),
                        Householdproducts_ProductsService = c.String(maxLength: 50),
                        ToiletriesCosmetics_ProductsService = c.String(maxLength: 50),
                        PharmaceuticalChemistsProducts_ProductsService = c.String(maxLength: 50),
                        TobaccoProducts_ProductsService = c.String(maxLength: 50),
                        PetsPetFood_ProductsService = c.String(maxLength: 50),
                        ShoppingRetailClothing_ProductsService = c.String(maxLength: 50),
                        DIYGardening_ProductsService = c.String(maxLength: 50),
                        AppliancesOtherHouseholdDurables_ProductsService = c.String(maxLength: 50),
                        ElectronicsOtherPersonalItems_ProductsService = c.String(maxLength: 50),
                        CommunicationsInternet_ProductsService = c.String(maxLength: 50),
                        FinancialServices_ProductsService = c.String(maxLength: 50),
                        HolidaysTravel_ProductsService = c.String(maxLength: 50),
                        SportsLeisure_ProductsService = c.String(maxLength: 50),
                        Motoring_ProductsService = c.String(maxLength: 50),
                        National_Radio = c.String(maxLength: 50),
                        Local_Radio = c.String(maxLength: 50),
                        Music_Radio = c.String(maxLength: 50),
                        Sport_Radio = c.String(maxLength: 50),
                        Talk_Radio = c.String(maxLength: 50),
                        Satallite_TV = c.String(maxLength: 50),
                        Cable_TV = c.String(maxLength: 50),
                        Terrestrial_TV = c.String(maxLength: 50),
                        Internet_TV = c.String(maxLength: 50),
                        EMAIL_MESSAGE = c.String(maxLength: 2500),
                        MEDIA_URL = c.String(maxLength: 2500),
                        SMS_MESSAGE = c.String(maxLength: 500),
                        ORIGINATOR = c.String(maxLength: 500),
                        MSCampaignProfileId = c.Int(),
                        UserId = c.Int(),
                        ClientId = c.Int(),
                        CampaignName = c.String(maxLength: 500),
                        CampaignDescription = c.String(maxLength: 1000),
                        TotalBudget = c.Decimal(precision: 18, scale: 2),
                        MaxDailyBudget = c.Decimal(precision: 18, scale: 2),
                        MaxMonthBudget = c.Decimal(precision: 18, scale: 2),
                        MaxWeeklyBudget = c.Decimal(precision: 18, scale: 2),
                        MaxHourlyBudget = c.Decimal(precision: 18, scale: 2),
                        TotalCredit = c.Decimal(precision: 18, scale: 2),
                        PlaysToDate = c.Int(),
                        PlaysLastMonth = c.Int(),
                        PlaysCurrentMonth = c.Int(),
                        CancelledToDate = c.Int(),
                        CancelledLastMonth = c.Int(),
                        CancelledCurrentMonth = c.Int(),
                        SmsToDate = c.Int(),
                        SmsLastMonth = c.Int(),
                        SmsCurrentMonth = c.Int(),
                        EmailToDate = c.Int(),
                        EmailsLastMonth = c.Int(),
                        EmailsCurrentMonth = c.Int(),
                        EmailFileLocation = c.String(maxLength: 400),
                        Active = c.Boolean(),
                        NumberOfPlays = c.Int(),
                        AverageDailyPlays = c.Int(),
                        SmsRequests = c.Int(),
                        EmailsDelievered = c.Int(),
                        EmailSubject = c.String(maxLength: 1000),
                        EmailBody = c.String(maxLength: 1000),
                        EmailSenderAddress = c.String(maxLength: 1000),
                        SmsOriginator = c.String(maxLength: 1000),
                        SmsBody = c.String(maxLength: 1000),
                        SMSFileLocation = c.String(maxLength: 1000),
                        CreatedDateTime = c.DateTime(),
                        UpdatedDateTime = c.DateTime(),
                        Status = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        NumberInBatch = c.Int(),
                    })
                .PrimaryKey(t => t.CampaignProfileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CampaignMatches");
        }
    }
}
