namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsMaxAdvertPerDayFieldinUserProfileAdvertReceived : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileAdvertsReceiveds", "IsMaxAdvertPerDay", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileAdvertsReceiveds", "IsMaxAdvertPerDay");
        }
    }
}
