namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostcodeonUserProfilePreference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfilePreference", "Postcode", c => c.String(maxLength: 100));
            DropColumn("dbo.UserProfile", "Postcode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "Postcode", c => c.String(maxLength: 100));
            DropColumn("dbo.UserProfilePreference", "Postcode");
        }
    }
}
