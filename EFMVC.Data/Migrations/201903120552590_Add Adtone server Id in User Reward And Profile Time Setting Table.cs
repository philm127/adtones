namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneserverIdinUserRewardAndProfileTimeSettingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileTimeSetting", "AdtoneServerUserProfileTimeSettingId", c => c.Int());
            AddColumn("dbo.UserRewards", "AdtoneServerUserRewardId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRewards", "AdtoneServerUserRewardId");
            DropColumn("dbo.UserProfileTimeSetting", "AdtoneServerUserProfileTimeSettingId");
        }
    }
}
