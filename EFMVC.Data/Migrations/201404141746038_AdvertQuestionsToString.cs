// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-12-2014
// ***********************************************************************
// <copyright file="201404141746038_AdvertQuestionsToString.cs" company="Noat">
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
    /// Class AdvertQuestionsToString.
    /// </summary>
    public partial class AdvertQuestionsToString : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.CampaignProfileAdvert", "Food", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "SweetSaltySnacks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "AlcoholicDrinks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "NonAlcoholicDrinks", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Householdproducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "ToiletriesCosmetics", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "PharmaceuticalChemistsProducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "TobaccoProducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "PetsPetFood", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "ShoppingRetailClothing", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "DIYGardening", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "AppliancesOtherHouseholdDurables", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "ElectronicsOtherPersonalItems", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "CommunicationsInternet", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "FinancialServices", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "HolidaysTravel", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "SportsLeisure", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Motoring", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Newspapers", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Magazines", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "TV", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Radio", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Cinema", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "SocialNetworking", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "GeneralUse", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Shopping", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Fitness", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Holidays", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Environment", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "GoingOut", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "FinancialProducts", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Religion", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Fashion", c => c.String(maxLength: 50));
            AlterColumn("dbo.CampaignProfileAdvert", "Music", c => c.String(maxLength: 50));
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.CampaignProfileAdvert", "Music", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Fashion", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Religion", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "FinancialProducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "GoingOut", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Environment", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Holidays", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Fitness", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Shopping", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "GeneralUse", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "SocialNetworking", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Cinema", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Radio", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "TV", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Magazines", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Newspapers", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Motoring", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "SportsLeisure", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "HolidaysTravel", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "FinancialServices", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "CommunicationsInternet", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "ElectronicsOtherPersonalItems", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "AppliancesOtherHouseholdDurables", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "DIYGardening", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "ShoppingRetailClothing", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "PetsPetFood", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "TobaccoProducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "PharmaceuticalChemistsProducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "ToiletriesCosmetics", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Householdproducts", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "NonAlcoholicDrinks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "AlcoholicDrinks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "SweetSaltySnacks", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignProfileAdvert", "Food", c => c.Int(nullable: false));
        }
    }
}