// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-12-2014
// ***********************************************************************
// <copyright file="201405091651516_IntToStringForCampaignQuestions.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
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
    /// Class IntToStringForCampaignQuestions.
    /// </summary>
    public partial class IntToStringForCampaignQuestions : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.CampaignProfileCinema", "Cinema", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileInternet", "SocialNetworking", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileInternet", "Video", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileInternet", "Research", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileInternet", "Auctions", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileInternet", "Shopping", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfilePress", "Local", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfilePress", "National", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfilePress", "FreeNewpapers", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfilePress", "Magazines", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "Food", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "SweetSaltySnacks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "AlcoholicDrinks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "NonAlcoholicDrinks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "Householdproducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "ToiletriesCosmetics", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "PharmaceuticalChemistsProducts",
                        c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "TobaccoProducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "PetsPetFood", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "ShoppingRetailClothing", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "DIYGardening", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "AppliancesOtherHouseholdDurables",
                        c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "ElectronicsOtherPersonalItems",
                        c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "CommunicationsInternet", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "FinancialServices", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "HolidaysTravel", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "SportsLeisure", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileProductsService", "Motoring", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileRadio", "National", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileRadio", "Local", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileRadio", "Music", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileRadio", "Sport", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileRadio", "Talk", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileTv", "Satallite", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileTv", "Cable", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileTv", "Terrestrial", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileTv", "Internet", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Fitness", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Holidays", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Environment", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "GoingOut", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "FinancialStabiity", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Religion", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Fashion", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAttitude", "Music", c => c.String(maxLength: 50));
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.CampaignProfileAttitude", "Music", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "Fashion", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "Religion", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "FinancialStabiity", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "GoingOut", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "Environment", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "Holidays", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAttitude", "Fitness", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileTv", "Internet", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileTv", "Terrestrial", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileTv", "Cable", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileTv", "Satallite", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileRadio", "Talk", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileRadio", "Sport", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileRadio", "Music", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileRadio", "Local", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileRadio", "National", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "Motoring", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "SportsLeisure", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "HolidaysTravel", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "FinancialServices", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "CommunicationsInternet", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "ElectronicsOtherPersonalItems",
                        c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "AppliancesOtherHouseholdDurables",
                        c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "DIYGardening", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "ShoppingRetailClothing", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "PetsPetFood", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "TobaccoProducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "PharmaceuticalChemistsProducts",
                        c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "ToiletriesCosmetics", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "Householdproducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "NonAlcoholicDrinks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "AlcoholicDrinks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "SweetSaltySnacks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileProductsService", "Food", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfilePress", "Magazines", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfilePress", "FreeNewpapers", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfilePress", "National", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfilePress", "Local", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileInternet", "Shopping", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileInternet", "Auctions", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileInternet", "Research", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileInternet", "Video", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileInternet", "SocialNetworking", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileCinema", "Cinema", c => c.Int(nullable: false));
        }
    }
}