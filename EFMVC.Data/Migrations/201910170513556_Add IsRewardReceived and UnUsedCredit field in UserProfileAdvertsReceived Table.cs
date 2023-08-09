namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRewardReceivedandUnUsedCreditfieldinUserProfileAdvertsReceivedTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileAdvertsReceiveds", "IsRewardReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserProfileAdvertsReceiveds", "UnUsedCredit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileAdvertsReceiveds", "UnUsedCredit");
            DropColumn("dbo.UserProfileAdvertsReceiveds", "IsRewardReceived");
        }
    }
}
