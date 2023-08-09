namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRotateAdvertTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RotateAdverts",
                c => new
                    {
                        RotateAdvertId = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(),
                        CampaignProfileId = c.Int(),
                        AdvertId = c.Int(),
                        IsAdvertPlayed = c.Boolean(nullable: false),
                        UserMatchTableName = c.String(),
                        AddedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RotateAdvertId)
                .ForeignKey("dbo.Advert", t => t.AdvertId)
                .ForeignKey("dbo.CampaignProfile", t => t.CampaignProfileId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.UserProfileId)
                .Index(t => t.CampaignProfileId)
                .Index(t => t.AdvertId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RotateAdverts", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.RotateAdverts", "CampaignProfileId", "dbo.CampaignProfile");
            DropForeignKey("dbo.RotateAdverts", "AdvertId", "dbo.Advert");
            DropIndex("dbo.RotateAdverts", new[] { "AdvertId" });
            DropIndex("dbo.RotateAdverts", new[] { "CampaignProfileId" });
            DropIndex("dbo.RotateAdverts", new[] { "UserProfileId" });
            DropTable("dbo.RotateAdverts");
        }
    }
}
