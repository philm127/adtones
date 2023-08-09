namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRewardIdinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RewardId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "RewardId");
            AddForeignKey("dbo.Users", "RewardId", "dbo.Rewards", "RewardId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RewardId", "dbo.Rewards");
            DropIndex("dbo.Users", new[] { "RewardId" });
            DropColumn("dbo.Users", "RewardId");
        }
    }
}
