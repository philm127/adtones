namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveforeignKeyfromrewardidfromUserRewardHistoryTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewardHistories", new[] { "RewardId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.UserRewardHistories", "RewardId");
            AddForeignKey("dbo.UserRewardHistories", "RewardId", "dbo.Rewards", "RewardId");
        }
    }
}
