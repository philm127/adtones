namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRewardHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRewardHistories",
                c => new
                    {
                        UserRewardHistoryId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RewardId = c.Int(nullable: false),
                        EarnedReward = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserRewardHistoryId)
                .ForeignKey("dbo.Rewards", t => t.RewardId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RewardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewardHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewardHistories", new[] { "RewardId" });
            DropIndex("dbo.UserRewardHistories", new[] { "UserId" });
            DropTable("dbo.UserRewardHistories");
        }
    }
}
