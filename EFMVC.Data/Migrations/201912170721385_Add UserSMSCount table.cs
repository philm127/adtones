namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserSMSCounttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSMSCounts",
                c => new
                    {
                        UserSMSCountId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MSISDN = c.String(),
                        SubscribeSMSCount = c.Int(nullable: false),
                        UnSubscribeSMSCount = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        RewardSMSCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserSMSCountId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSMSCounts", "UserId", "dbo.Users");
            DropIndex("dbo.UserSMSCounts", new[] { "UserId" });
            DropTable("dbo.UserSMSCounts");
        }
    }
}
