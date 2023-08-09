namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMSISDNFieldinUserRewardHistoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRewardHistories", "MSISDN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRewardHistories", "MSISDN");
        }
    }
}
