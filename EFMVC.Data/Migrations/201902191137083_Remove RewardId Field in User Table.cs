namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRewardIdFieldinUserTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "RewardId", "dbo.Rewards");
            DropIndex("dbo.Users", new[] { "RewardId" });
            DropColumn("dbo.Users", "RewardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RewardId", c => c.Int());
            CreateIndex("dbo.Users", "RewardId");
            AddForeignKey("dbo.Users", "RewardId", "dbo.Rewards", "RewardId");
        }
    }
}
