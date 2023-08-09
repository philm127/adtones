// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 01-03-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 01-03-2014
// ***********************************************************************
// <copyright file="201311211516022_CampaignAuditToCampaignProfile.cs" company="">
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
    /// Class CampaignAuditToCampaignProfile.
    /// </summary>
    public partial class CampaignAuditToCampaignProfile : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddForeignKey("dbo.CampaignAudit", "CampaignProfileId", "dbo.CampaignProfile", "CampaignProfileId",
                          cascadeDelete: true);
            CreateIndex("dbo.CampaignAudit", "CampaignProfileId");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.CampaignAudit", new[] {"CampaignProfileId"});
            DropForeignKey("dbo.CampaignAudit", "CampaignProfileId", "dbo.CampaignProfile");
        }
    }
}