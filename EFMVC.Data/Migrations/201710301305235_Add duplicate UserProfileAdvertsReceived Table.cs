namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddduplicateUserProfileAdvertsReceivedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfileAdvertsReceived10",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived4",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived5",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived6",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived7",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived8",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
            CreateTable(
                "dbo.UserProfileAdvertsReceived9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        AdvertRef = c.String(),
                        AdvertName = c.String(),
                        Brand = c.String(),
                        FileName = c.String(),
                        DateTimePlayed = c.DateTime(nullable: false),
                        CreditsReceived = c.String(),
                        CampaignAuditId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignAudit", t => t.CampaignAuditId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignAuditId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfileAdvertsReceived9", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived9", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived8", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived8", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived7", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived7", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived6", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived6", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived5", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived5", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived4", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived4", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived3", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived3", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived2", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived2", "CampaignAuditId", "dbo.CampaignAudit");
            DropForeignKey("dbo.UserProfileAdvertsReceived10", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfileAdvertsReceived10", "CampaignAuditId", "dbo.CampaignAudit");
            DropIndex("dbo.UserProfileAdvertsReceived9", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived9", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived8", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived8", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived7", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived7", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived6", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived6", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived5", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived5", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived4", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived4", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived3", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived3", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived2", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived2", new[] { "UserProfileId" });
            DropIndex("dbo.UserProfileAdvertsReceived10", new[] { "CampaignAuditId" });
            DropIndex("dbo.UserProfileAdvertsReceived10", new[] { "UserProfileId" });
            DropTable("dbo.UserProfileAdvertsReceived9");
            DropTable("dbo.UserProfileAdvertsReceived8");
            DropTable("dbo.UserProfileAdvertsReceived7");
            DropTable("dbo.UserProfileAdvertsReceived6");
            DropTable("dbo.UserProfileAdvertsReceived5");
            DropTable("dbo.UserProfileAdvertsReceived4");
            DropTable("dbo.UserProfileAdvertsReceived3");
            DropTable("dbo.UserProfileAdvertsReceived2");
            DropTable("dbo.UserProfileAdvertsReceived10");
        }
    }
}
