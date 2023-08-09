// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 01-03-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 01-03-2014
// ***********************************************************************
// <copyright file="201311211451501_CampaignAudit.cs" company="">
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
    /// Class CampaignAudit.
    /// </summary>
    public partial class CampaignAudit : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignAudit",
                c => new
                         {
                             CampaignAuditId = c.Int(nullable: false, identity: true),
                             CampaignProfileId = c.Int(nullable: false),
                             UserProfileId = c.Int(nullable: false),
                             BidValue = c.Double(nullable: false),
                             StartTime = c.DateTime(nullable: false),
                             EndTime = c.DateTime(nullable: false),
                             PlayLengthTicks = c.Long(nullable: false),
                             SMS = c.String(),
                             SMSCost = c.Double(nullable: false),
                             Email = c.String(),
                             EmailCost = c.Double(nullable: false),
                             TotalCost = c.Double(nullable: false),
                             Status = c.String(),
                         })
                .PrimaryKey(t => t.CampaignAuditId);
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.CampaignAudit");
        }
    }
}