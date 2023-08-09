// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="201405091749064_StringToIntForCampaignQuestions.cs" company="">
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
    /// Class StringToIntForCampaignQuestions.
    /// </summary>
    public partial class StringToIntForCampaignQuestions : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.UserProfileCinema", "Cinema", c => c.Int());
            AlterColumn("dbo.UserProfileInternet", "SocialNetworking", c => c.Int());
            AlterColumn("dbo.UserProfileInternet", "Video", c => c.Int());
            AlterColumn("dbo.UserProfileInternet", "Research", c => c.Int());
            AlterColumn("dbo.UserProfileInternet", "Auctions", c => c.Int());
            AlterColumn("dbo.UserProfileInternet", "Shopping", c => c.Int());
            AlterColumn("dbo.UserProfilePress", "Local", c => c.Int());
            AlterColumn("dbo.UserProfilePress", "National", c => c.Int());
            AlterColumn("dbo.UserProfilePress", "FreeNewpapers", c => c.Int());
            AlterColumn("dbo.UserProfilePress", "Magazines", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "Food", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "SweetSaltySnacks", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "AlcoholicDrinks", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "NonAlcoholicDrinks", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "Householdproducts", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "ToiletriesCosmetics", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "PharmaceuticalChemistsProducts", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "TobaccoProducts", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "PetsPetFood", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "ShoppingRetailClothing", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "DIYGardening", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "AppliancesOtherHouseholdDurables", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "ElectronicsOtherPersonalItems", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "CommunicationsInternet", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "FinancialServices", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "HolidaysTravel", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "SportsLeisure", c => c.Int());
            AlterColumn("dbo.UserProfileProductsService", "Motoring", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Fitness", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Holidays", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Environment", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "GoingOut", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "FinancialStabiity", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Religion", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Fashion", c => c.Int());
            AlterColumn("dbo.UserProfileAttitude", "Music", c => c.Int());
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.UserProfileAttitude", "Music", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "Fashion", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "Religion", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "FinancialStabiity", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "GoingOut", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "Environment", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "Holidays", c => c.String());
            AlterColumn("dbo.UserProfileAttitude", "Fitness", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "Motoring", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "SportsLeisure", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "HolidaysTravel", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "FinancialServices", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "CommunicationsInternet", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "ElectronicsOtherPersonalItems", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "AppliancesOtherHouseholdDurables", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "DIYGardening", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "ShoppingRetailClothing", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "PetsPetFood", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "TobaccoProducts", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "PharmaceuticalChemistsProducts", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "ToiletriesCosmetics", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "Householdproducts", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "NonAlcoholicDrinks", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "AlcoholicDrinks", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "SweetSaltySnacks", c => c.String());
            AlterColumn("dbo.UserProfileProductsService", "Food", c => c.String());
            AlterColumn("dbo.UserProfilePress", "Magazines", c => c.String());
            AlterColumn("dbo.UserProfilePress", "FreeNewpapers", c => c.String());
            AlterColumn("dbo.UserProfilePress", "National", c => c.String());
            AlterColumn("dbo.UserProfilePress", "Local", c => c.String());
            AlterColumn("dbo.UserProfileInternet", "Shopping", c => c.String());
            AlterColumn("dbo.UserProfileInternet", "Auctions", c => c.String());
            AlterColumn("dbo.UserProfileInternet", "Research", c => c.String());
            AlterColumn("dbo.UserProfileInternet", "Video", c => c.String());
            AlterColumn("dbo.UserProfileInternet", "SocialNetworking", c => c.String());
            AlterColumn("dbo.UserProfileCinema", "Cinema", c => c.String());
        }
    }
}