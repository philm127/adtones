namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActivefieldinoperatortable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operators", "IsActive");
        }
    }
}
