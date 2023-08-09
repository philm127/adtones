namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRewardTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRewards",
                c => new
                    {
                        UserRewardId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RewardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRewardId)
                .ForeignKey("dbo.Rewards", t => t.RewardId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RewardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewards", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRewards", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewards", new[] { "RewardId" });
            DropIndex("dbo.UserRewards", new[] { "UserId" });
            DropTable("dbo.UserRewards");
        }
    }
}
