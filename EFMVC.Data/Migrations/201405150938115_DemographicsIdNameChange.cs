// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-22-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="201405150938115_DemographicsIdNameChange.cs" company="Noat">
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
    /// Class DemographicsIdNameChange.
    /// </summary>
    public partial class DemographicsIdNameChange : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
//            AddColumn("dbo.CampaignProfileDemographics", "CampaignProfileDemographicsId", c => c.Int(nullable: false, identity: true));
//            DropPrimaryKey("dbo.CampaignProfileDemographics", new[] { "Id" });
//            AddPrimaryKey("dbo.CampaignProfileDemographics", "CampaignProfileDemographicsId");
//            DropColumn("dbo.CampaignProfileDemographics", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
//            AddColumn("dbo.CampaignProfileDemographics", "Id", c => c.Int(nullable: false, identity: true));
//            DropPrimaryKey("dbo.CampaignProfileDemographics", new[] { "CampaignProfileDemographicsId" });
//            AddPrimaryKey("dbo.CampaignProfileDemographics", "Id");
//            DropColumn("dbo.CampaignProfileDemographics", "CampaignProfileDemographicsId");
        }
    }
}