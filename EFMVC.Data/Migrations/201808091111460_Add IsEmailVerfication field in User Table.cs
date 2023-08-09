namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsEmailVerficationfieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsEmailVerfication", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsEmailVerfication");
        }
    }
}
