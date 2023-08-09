namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneserverIdinRewardTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rewards", "AdtoneServerRewardId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rewards", "AdtoneServerRewardId");
        }
    }
}
