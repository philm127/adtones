namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertiserPhoneNumberInUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AdvertiserPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AdvertiserPhoneNumber");
        }
    }
}
