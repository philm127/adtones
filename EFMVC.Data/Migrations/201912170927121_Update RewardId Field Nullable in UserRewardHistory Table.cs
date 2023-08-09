namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRewardIdFieldNullableinUserRewardHistoryTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewardHistories", new[] { "RewardId" });
            AlterColumn("dbo.UserRewardHistories", "RewardId", c => c.Int());
            CreateIndex("dbo.UserRewardHistories", "RewardId");
            AddForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards", "RewardId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewardHistories", new[] { "RewardId" });
            AlterColumn("dbo.UserRewardHistories", "RewardId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserRewardHistories", "RewardId");
            AddForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards", "RewardId", cascadeDelete: true);
        }
    }
}
