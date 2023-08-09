namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullRewardIdinUserTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "RewardId", "dbo.Rewards");
            DropIndex("dbo.Users", new[] { "RewardId" });
            AlterColumn("dbo.Users", "RewardId", c => c.Int());
            CreateIndex("dbo.Users", "RewardId");
            AddForeignKey("dbo.Users", "RewardId", "dbo.Rewards", "RewardId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RewardId", "dbo.Rewards");
            DropIndex("dbo.Users", new[] { "RewardId" });
            AlterColumn("dbo.Users", "RewardId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "RewardId");
            AddForeignKey("dbo.Users", "RewardId", "dbo.Rewards", "RewardId", cascadeDelete: true);
        }
    }
}
