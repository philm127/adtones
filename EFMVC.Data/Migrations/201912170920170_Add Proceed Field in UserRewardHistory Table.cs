namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProceedFieldinUserRewardHistoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRewardHistories", "Proceed", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRewardHistories", "Proceed");
        }
    }
}
