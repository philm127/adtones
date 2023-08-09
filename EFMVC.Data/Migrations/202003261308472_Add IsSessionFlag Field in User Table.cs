namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSessionFlagFieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsSessionFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsSessionFlag");
        }
    }
}
