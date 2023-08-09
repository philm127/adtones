namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRewardIdNullableinUserRewardTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRewards", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewards", new[] { "RewardId" });
            AlterColumn("dbo.UserRewards", "RewardId", c => c.Int());
            CreateIndex("dbo.UserRewards", "RewardId");
            AddForeignKey("dbo.UserRewards", "RewardId", "dbo.Rewards", "RewardId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewards", "RewardId", "dbo.Rewards");
            DropIndex("dbo.UserRewards", new[] { "RewardId" });
            AlterColumn("dbo.UserRewards", "RewardId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserRewards", "RewardId");
            AddForeignKey("dbo.UserRewards", "RewardId", "dbo.Rewards", "RewardId", cascadeDelete: true);
        }
    }
}
