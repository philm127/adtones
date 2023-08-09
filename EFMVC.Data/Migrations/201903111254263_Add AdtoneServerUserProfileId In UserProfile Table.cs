namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerUserProfileIdInUserProfileTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "AdtoneServerUserProfileId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "AdtoneServerUserProfileId");
        }
    }
}
