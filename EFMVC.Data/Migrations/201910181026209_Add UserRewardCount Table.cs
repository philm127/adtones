namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRewardCountTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRewardCounts",
                c => new
                    {
                        UserRewardCountId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MSISDN = c.String(),
                        Count = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserRewardCountId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewardCounts", "UserId", "dbo.Users");
            DropIndex("dbo.UserRewardCounts", new[] { "UserId" });
            DropTable("dbo.UserRewardCounts");
        }
    }
}
