namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddduplicateCampaignAuditTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignAudit10",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit2",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit3",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit4",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit5",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit6",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit7",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit8",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
            CreateTable(
                "dbo.CampaignAudit9",
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
                .PrimaryKey(t => t.CampaignAuditId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId, cascadeDelete: true)
                .Index(t => t.CampaignProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignAudit9", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit8", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit7", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit6", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit5", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit4", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit3", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit2", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.CampaignAudit10", "CampaignProfileId", "dbo.CampaignProfile");
            DropIndex("dbo.CampaignAudit9", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit8", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit7", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit6", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit5", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit4", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit3", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit2", new[] { "CampaignProfileId" });
            DropIndex("dbo.CampaignAudit10", new[] { "CampaignProfileId" });
            DropTable("dbo.CampaignAudit9");
            DropTable("dbo.CampaignAudit8");
            DropTable("dbo.CampaignAudit7");
            DropTable("dbo.CampaignAudit6");
            DropTable("dbo.CampaignAudit5");
            DropTable("dbo.CampaignAudit4");
            DropTable("dbo.CampaignAudit3");
            DropTable("dbo.CampaignAudit2");
            DropTable("dbo.CampaignAudit10");
        }
    }
}
