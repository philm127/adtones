namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditReceivedSMSCountFieldinUserSMSCountTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSMSCounts", "CreditReceivedSMSCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSMSCounts", "CreditReceivedSMSCount");
        }
    }
}
