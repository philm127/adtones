// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-09-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-09-2013
// ***********************************************************************
// <copyright file="201310091155463_BucketAudit.cs" company="">
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
    /// Class BucketAudit.
    /// </summary>
    public partial class BucketAudit : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.BucketAudits",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             BucketId = c.Int(nullable: false),
                             MSISDN = c.String(maxLength: 50),
                             BucketPeriodStart = c.String(maxLength: 250),
                             TargetDeliveryServer = c.String(maxLength: 250),
                         })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.BucketAuditRows",
                c => new
                         {
                             Id = c.Int(nullable: false, identity: true),
                             BucketAuditId = c.Int(nullable: false),
                             State = c.Int(nullable: false),
                             MediaUrl = c.String(maxLength: 500),
                             BidValue = c.String(maxLength: 250),
                             Dtmf = c.String(maxLength: 250),
                             Start = c.String(maxLength: 250),
                             End = c.String(maxLength: 250),
                             CampaignProfileId = c.Int(nullable: false),
                             Sms = c.Int(nullable: false),
                             Email = c.Int(nullable: false),
                         })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BucketAudits", t => t.BucketAuditId, cascadeDelete: true)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.BucketAuditId)
                .Index(t => t.CampaignProfileId);
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.BucketAuditRows", new[] {"CampaignProfileId"});
            DropIndex("dbo.BucketAuditRows", new[] {"BucketAuditId"});
            DropForeignKey("dbo.BucketAuditRows", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.BucketAuditRows", "BucketAuditId", "dbo.BucketAudits");
            DropTable("dbo.BucketAuditRows");
            DropTable("dbo.BucketAudits");
        }
    }
}