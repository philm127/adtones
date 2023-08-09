namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermAndConditionFileNamefieldoncountrytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "TermAndConditionFileName", c => c.String());
            DropColumn("dbo.Users", "AdvertiserPhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AdvertiserPhoneNumber", c => c.String());
            DropColumn("dbo.Country", "TermAndConditionFileName");
        }
    }
}
