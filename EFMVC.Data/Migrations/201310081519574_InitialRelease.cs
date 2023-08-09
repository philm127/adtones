// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-08-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="201310081519574_InitialRelease.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The Migrations namespace.
/// </summary>

using System.Data.Entity.Migrations;

namespace EFMVC.Data.Migrations
{
    /// <summary>
    /// Class InitialRelease.
    /// </summary>
    public partial class InitialRelease : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                         {
                             UserId = c.Int(nullable: false, identity: true),
                             Email = c.String(nullable: false),
                             FirstName = c.String(nullable: false),
                             LastName = c.String(nullable: false),
                             PasswordHash = c.String(),
                             DateCreated = c.DateTime(nullable: false),
                             LastLoginTime = c.DateTime(),
                             Activated = c.Boolean(nullable: false),
                             RoleId = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.UserId);

            CreateTable(
                "dbo.Advert",
                c => new
                         {
                             AdvertId = c.Int(nullable: false, identity: true),
                             UserId = c.Int(nullable: false),
                             AdvertName = c.String(maxLength: 250),
                             AdvertDescription = c.String(maxLength: 2000),
                             Brand = c.String(maxLength: 250),
                             MediaFileLocation = c.String(maxLength: 500),
                             UploadedToMediaServer = c.Boolean(nullable: false),
                             CreatedDateTime = c.DateTime(nullable: false),
                             UpdatedDateTime = c.DateTime(nullable: false),
                             CampaignProfile_CampaignProfileId = c.Int(),
                         })
                .PrimaryKey(t => t.AdvertId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfile_CampaignProfileId)
                .Index(t => t.UserId)
                .Index(t => t.CampaignProfile_CampaignProfileId);

            CreateTable(
                "dbo.CampaignAdverts",
                c => new
                         {
                             CampaignAdvertId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             AdvertId = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignAdvertId)
                .ForeignKey("dbo.Advert", t => t.AdvertId, cascadeDelete: true)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.AdvertId)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfile",
                c => new
                         {
                             CampaignProfileId = c.Int(nullable: false, identity: true),
                             UserId = c.Int(nullable: false),
                             CampaignName = c.String(),
                             CampaignDescription = c.String(),
                             TotalBudget = c.Single(nullable: false),
                             MaxDailyBudget = c.Single(nullable: false),
                             MaxBid = c.Single(nullable: false),
                             AvailableCredit = c.Single(nullable: false),
                             PlaysToDate = c.Int(nullable: false),
                             PlaysLastMonth = c.Int(nullable: false),
                             PlaysCurrentMonth = c.Int(nullable: false),
                             CancelledToDate = c.Int(nullable: false),
                             CancelledLastMonth = c.Int(nullable: false),
                             CancelledCurrentMonth = c.Int(nullable: false),
                             SmsToDate = c.Int(nullable: false),
                             SmsLastMonth = c.Int(nullable: false),
                             SmsCurrentMonth = c.Int(nullable: false),
                             EmailToDate = c.Int(nullable: false),
                             EmailsLastMonth = c.Int(nullable: false),
                             EmailsCurrentMonth = c.Int(nullable: false),
                             Active = c.Boolean(nullable: false),
                             NumberOfPlays = c.Int(nullable: false),
                             AverageDailyPlays = c.Int(nullable: false),
                             SmsRequests = c.Int(nullable: false),
                             EmailsDelievered = c.Int(nullable: false),
                             EmailSubject = c.String(),
                             EmailBody = c.String(unicode: false, storeType: "text"),
                             EmailSenderAddress = c.String(),
                             SmsOriginator = c.String(),
                             SmsBody = c.String(maxLength: 1000),
                             CreatedDateTime = c.DateTime(nullable: false),
                             UpdatedDateTime = c.DateTime(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.CampaignProfileAdvert",
                c => new
                         {
                             CampaignProfileAdvertsId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Food = c.Int(nullable: false),
                             SweetSaltySnacks = c.Int(nullable: false),
                             AlcoholicDrinks = c.Int(nullable: false),
                             NonAlcoholicDrinks = c.Int(nullable: false),
                             Householdproducts = c.Int(nullable: false),
                             ToiletriesCosmetics = c.Int(nullable: false),
                             PharmaceuticalChemistsProducts = c.Int(nullable: false),
                             TobaccoProducts = c.Int(nullable: false),
                             PetsPetFood = c.Int(nullable: false),
                             ShoppingRetailClothing = c.Int(nullable: false),
                             DIYGardening = c.Int(nullable: false),
                             AppliancesOtherHouseholdDurables = c.Int(nullable: false),
                             ElectronicsOtherPersonalItems = c.Int(nullable: false),
                             CommunicationsInternet = c.Int(nullable: false),
                             FinancialServices = c.Int(nullable: false),
                             HolidaysTravel = c.Int(nullable: false),
                             SportsLeisure = c.Int(nullable: false),
                             Motoring = c.Int(nullable: false),
                             Newspapers = c.Int(nullable: false),
                             Magazines = c.Int(nullable: false),
                             TV = c.Int(nullable: false),
                             Radio = c.Int(nullable: false),
                             Cinema = c.Int(nullable: false),
                             SocialNetworking = c.Int(nullable: false),
                             GeneralUse = c.Int(nullable: false),
                             Shopping = c.Int(nullable: false),
                             Fitness = c.Int(nullable: false),
                             Holidays = c.Int(nullable: false),
                             Environment = c.Int(nullable: false),
                             GoingOut = c.Int(nullable: false),
                             FinancialProducts = c.Int(nullable: false),
                             Religion = c.Int(nullable: false),
                             Fashion = c.Int(nullable: false),
                             Music = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileAdvertsId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileCinema",
                c => new
                         {
                             CampaignProfileCinemaId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Cinema = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileCinemaId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileInternet",
                c => new
                         {
                             CampaignProfileInternetId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             SocialNetworking = c.Int(nullable: false),
                             Video = c.Int(nullable: false),
                             Research = c.Int(nullable: false),
                             Auctions = c.Int(nullable: false),
                             Shopping = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileInternetId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileMobile",
                c => new
                         {
                             CampaignProfileMobileId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             ContractType = c.String(),
                             Spend = c.String(),
                         })
                .PrimaryKey(t => t.CampaignProfileMobileId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfilePress",
                c => new
                         {
                             CampaignProfilePressId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Local = c.Int(nullable: false),
                             National = c.Int(nullable: false),
                             FreeNewpapers = c.Int(nullable: false),
                             Magazines = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfilePressId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileProductsService",
                c => new
                         {
                             CampaignProfileProductsServicesId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Food = c.Int(nullable: false),
                             SweetSaltySnacks = c.Int(nullable: false),
                             AlcoholicDrinks = c.Int(nullable: false),
                             NonAlcoholicDrinks = c.Int(nullable: false),
                             Householdproducts = c.Int(nullable: false),
                             ToiletriesCosmetics = c.Int(nullable: false),
                             PharmaceuticalChemistsProducts = c.Int(nullable: false),
                             TobaccoProducts = c.Int(nullable: false),
                             PetsPetFood = c.Int(nullable: false),
                             ShoppingRetailClothing = c.Int(nullable: false),
                             DIYGardening = c.Int(nullable: false),
                             AppliancesOtherHouseholdDurables = c.Int(nullable: false),
                             ElectronicsOtherPersonalItems = c.Int(nullable: false),
                             CommunicationsInternet = c.Int(nullable: false),
                             FinancialServices = c.Int(nullable: false),
                             HolidaysTravel = c.Int(nullable: false),
                             SportsLeisure = c.Int(nullable: false),
                             Motoring = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileProductsServicesId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileRadio",
                c => new
                         {
                             CampaignProfileRadioId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             National = c.Int(nullable: false),
                             Local = c.Int(nullable: false),
                             Music = c.Int(nullable: false),
                             Sport = c.Int(nullable: false),
                             Talk = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileRadioId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileTimeSetting",
                c => new
                         {
                             CampaignProfileTimeSettingsId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Monday = c.String(maxLength: 200),
                             Tuesday = c.String(maxLength: 200),
                             Wednesday = c.String(maxLength: 200),
                             Thursday = c.String(maxLength: 200),
                             Friday = c.String(maxLength: 200),
                             Saturday = c.String(maxLength: 200),
                             Sunday = c.String(maxLength: 200),
                         })
                .PrimaryKey(t => t.CampaignProfileTimeSettingsId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileTv",
                c => new
                         {
                             CampaignProfileTvId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Satallite = c.Int(nullable: false),
                             Cable = c.Int(nullable: false),
                             Terrestrial = c.Int(nullable: false),
                             Internet = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileTvId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileAttitude",
                c => new
                         {
                             CampaignProfileAttitudeId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             Fitness = c.Int(nullable: false),
                             Holidays = c.Int(nullable: false),
                             Environment = c.Int(nullable: false),
                             GoingOut = c.Int(nullable: false),
                             FinancialStabiity = c.Int(nullable: false),
                             Religion = c.Int(nullable: false),
                             Fashion = c.Int(nullable: false),
                             Music = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignProfileAttitudeId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileDateSettings",
                c => new
                         {
                             CampaignDateSettingsId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             CampaignDate = c.DateTime(nullable: false),
                             Active = c.Boolean(nullable: false),
                         })
                .PrimaryKey(t => t.CampaignDateSettingsId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.CampaignProfileDemographics",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             DOBStart = c.DateTime(),
                             DOBEnd = c.DateTime(),
                             Gender = c.String(maxLength: 50, defaultValue: "0"),
                             IncomeBracket = c.String(maxLength: 50, defaultValue: "0"),
                             WorkingStatus = c.String(maxLength: 50, defaultValue: "0"),
                             RelationshipStatus = c.String(maxLength: 50, defaultValue: "0"),
                             Education = c.String(maxLength: 50, defaultValue: "0"),
                             HouseholdStatus = c.String(maxLength: 50, defaultValue: "0"),
                             Location = c.String(maxLength: 50, defaultValue: "0"),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);

            CreateTable(
                "dbo.BlockedNumbers",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             UserId = c.Int(nullable: false),
                             TelephoneNumber = c.String(maxLength: 50),
                             Active = c.Boolean(nullable: false),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserProfile",
                c => new
                         {
                             UserProfileId = c.Int(nullable: false, identity: true),
                             UserId = c.Int(nullable: false),
                             DOB = c.DateTime(),
                             Gender = c.String(maxLength: 50),
                             IncomeBracket = c.String(maxLength: 50),
                             WorkingStatus = c.String(maxLength: 50),
                             RelationshipStatus = c.String(maxLength: 50),
                             Education = c.String(maxLength: 50),
                             HouseholdStatus = c.String(maxLength: 50),
                             Location = c.String(maxLength: 50),
                             MSISDN = c.String(maxLength: 50),
                         })
                .PrimaryKey(t => t.UserProfileId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserProfileAdvert",
                c => new
                         {
                             UserProfileAdvertsId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Food = c.Int(),
                             SweetSaltySnacks = c.Int(),
                             AlcoholicDrinks = c.Int(),
                             NonAlcoholicDrinks = c.Int(),
                             Householdproducts = c.Int(),
                             ToiletriesCosmetics = c.Int(),
                             PharmaceuticalChemistsProducts = c.Int(),
                             TobaccoProducts = c.Int(),
                             PetsPetFood = c.Int(),
                             ShoppingRetailClothing = c.Int(),
                             DIYGardening = c.Int(),
                             AppliancesOtherHouseholdDurables = c.Int(),
                             ElectronicsOtherPersonalItems = c.Int(),
                             CommunicationsInternet = c.Int(),
                             FinancialServices = c.Int(),
                             HolidaysTravel = c.Int(),
                             SportsLeisure = c.Int(),
                             Motoring = c.Int(),
                             Newspapers = c.Int(),
                             Magazines = c.Int(),
                             TV = c.Int(),
                             Radio = c.Int(),
                             Cinema = c.Int(),
                             SocialNetworking = c.Int(),
                             GeneralUse = c.Int(),
                             Shopping = c.Int(),
                             Fitness = c.Int(),
                             Holidays = c.Int(),
                             Environment = c.Int(),
                             GoingOut = c.Int(),
                             FinancialProducts = c.Int(),
                             Religion = c.Int(),
                             Fashion = c.Int(),
                             Music = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileAdvertsId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileCinema",
                c => new
                         {
                             UserProfileCinemaId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Cinema = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileCinemaId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileInternet",
                c => new
                         {
                             UserProfileInternetId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             SocialNetworking = c.Int(),
                             Video = c.Int(),
                             Research = c.Int(),
                             Auctions = c.Int(),
                             Shopping = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileInternetId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileMobile",
                c => new
                         {
                             UserProfileMobileId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             ContractType = c.String(),
                             Spend = c.String(),
                         })
                .PrimaryKey(t => t.UserProfileMobileId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfilePress",
                c => new
                         {
                             UserProfilePressId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Local = c.Int(),
                             National = c.Int(),
                             FreeNewpapers = c.Int(),
                             Magazines = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfilePressId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileProductsService",
                c => new
                         {
                             UserProfileProductsServicesId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Food = c.Int(),
                             SweetSaltySnacks = c.Int(),
                             AlcoholicDrinks = c.Int(),
                             NonAlcoholicDrinks = c.Int(),
                             Householdproducts = c.Int(),
                             ToiletriesCosmetics = c.Int(),
                             PharmaceuticalChemistsProducts = c.Int(),
                             TobaccoProducts = c.Int(),
                             PetsPetFood = c.Int(),
                             ShoppingRetailClothing = c.Int(),
                             DIYGardening = c.Int(),
                             AppliancesOtherHouseholdDurables = c.Int(),
                             ElectronicsOtherPersonalItems = c.Int(),
                             CommunicationsInternet = c.Int(),
                             FinancialServices = c.Int(),
                             HolidaysTravel = c.Int(),
                             SportsLeisure = c.Int(),
                             Motoring = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileProductsServicesId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileRadio",
                c => new
                         {
                             UserProfileRadioId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             National = c.Int(),
                             Local = c.Int(),
                             Music = c.Int(),
                             Sport = c.Int(),
                             Talk = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileRadioId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileTimeSetting",
                c => new
                         {
                             UserProfileTimeSettingsId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Monday = c.String(maxLength: 200),
                             Tuesday = c.String(maxLength: 200),
                             Wednesday = c.String(maxLength: 200),
                             Thursday = c.String(maxLength: 200),
                             Friday = c.String(maxLength: 200),
                             Saturday = c.String(maxLength: 200),
                             Sunday = c.String(maxLength: 200),
                         })
                .PrimaryKey(t => t.UserProfileTimeSettingsId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileTv",
                c => new
                         {
                             UserProfileTvId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Satallite = c.Int(),
                             Cable = c.Int(),
                             Terrestrial = c.Int(),
                             Internet = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileTvId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileAttitude",
                c => new
                         {
                             UserProfileAttitudeId = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             Fitness = c.Int(),
                             Holidays = c.Int(),
                             Environment = c.Int(),
                             GoingOut = c.Int(),
                             FinancialStabiity = c.Int(),
                             Religion = c.Int(),
                             Fashion = c.Int(),
                             Music = c.Int(),
                         })
                .PrimaryKey(t => t.UserProfileAttitudeId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileAdvertsReceiveds",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             AdvertRef = c.String(),
                             AdvertName = c.String(),
                             Brand = c.String(),
                             DateTimePlayed = c.DateTime(nullable: false),
                             CreditsReceived = c.String(),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.UserProfileCreditsReceiveds",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             UserProfileId = c.Int(nullable: false),
                             TotalCredits = c.String(),
                             LastMonthCredits = c.String(),
                             CurrentMonthCredits = c.String(),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);

            CreateTable(
                "dbo.CampaignProfileUserProfile",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             UserProfileId = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.UserProfileId);
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.CampaignProfileUserProfile", new[] {"UserProfileId"});
            DropIndex("dbo.CampaignProfileUserProfile", new[] {"CampaignProfileId"});
            DropIndex("dbo.UserProfileCreditsReceiveds", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileAdvertsReceiveds", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileAttitude", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileTv", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileTimeSetting", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileRadio", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileProductsService", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfilePress", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileMobile", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileInternet", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileCinema", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfileAdvert", new[] {"UserProfileId"});
            DropIndex("dbo.UserProfile", new[] {"UserId"});
            DropIndex("dbo.BlockedNumbers", new[] {"UserId"});
            DropIndex("dbo.CampaignProfileDemographics", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileDateSettings", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileAttitude", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileTv", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileTimeSetting", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileRadio", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileProductsService", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfilePress", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileMobile", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileInternet", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileCinema", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfileAdvert", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignProfile", new[] {"UserId"});
            DropIndex("dbo.CampaignAdverts", new[] {"CampaignProfileId"});
            DropIndex("dbo.CampaignAdverts", new[] {"AdvertId"});
            DropIndex("dbo.Advert", new[] {"CampaignProfile_CampaignProfileId"});
            DropIndex("dbo.Advert", new[] {"UserId"});
            DropForeignKey("dbo.CampaignProfileUserProfile", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.CampaignProfileUserProfile", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.UserProfileCreditsReceiveds", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceiveds", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAttitude", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileTv", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileTimeSetting", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileRadio", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileProductsService", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfilePress", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileMobile", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileInternet", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileCinema", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvert", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfile", "UserId", "dbo.Users");
            DropForeignKey("dbo.BlockedNumbers", "UserId", "dbo.Users");
            DropForeignKey("dbo.CampaignProfileDemographics", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileDateSettings", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileAttitude", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileTv", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileTimeSetting", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileRadio", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileProductsService", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfilePress", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileMobile", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileInternet", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileCinema", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfileAdvert", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignProfile", "UserId", "dbo.Users");
            DropForeignKey("dbo.CampaignAdverts", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAdverts", "AdvertId", "dbo.Advert");
            DropForeignKey("dbo.Advert", "CampaignProfile_CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.Advert", "UserId", "dbo.Users");
            DropTable("dbo.CampaignProfileUserProfile");
            DropTable("dbo.UserProfileCreditsReceiveds");
            DropTable("dbo.UserProfileAdvertsReceiveds");
            DropTable("dbo.UserProfileAttitude");
            DropTable("dbo.UserProfileTv");
            DropTable("dbo.UserProfileTimeSetting");
            DropTable("dbo.UserProfileRadio");
            DropTable("dbo.UserProfileProductsService");
            DropTable("dbo.UserProfilePress");
            DropTable("dbo.UserProfileMobile");
            DropTable("dbo.UserProfileInternet");
            DropTable("dbo.UserProfileCinema");
            DropTable("dbo.UserProfileAdvert");
            DropTable("dbo.UserProfile");
            DropTable("dbo.BlockedNumbers");
            DropTable("dbo.CampaignProfileDemographics");
            DropTable("dbo.CampaignProfileDateSettings");
            DropTable("dbo.CampaignProfileAttitude");
            DropTable("dbo.CampaignProfileTv");
            DropTable("dbo.CampaignProfileTimeSetting");
            DropTable("dbo.CampaignProfileRadio");
            DropTable("dbo.CampaignProfileProductsService");
            DropTable("dbo.CampaignProfilePress");
            DropTable("dbo.CampaignProfileMobile");
            DropTable("dbo.CampaignProfileInternet");
            DropTable("dbo.CampaignProfileCinema");
            DropTable("dbo.CampaignProfileAdvert");
            DropTable("dbo.CampaignProfile");
            DropTable("dbo.CampaignAdverts");
            DropTable("dbo.Advert");
            DropTable("dbo.Users");
        }
    }
}