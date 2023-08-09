namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTibcoMessageIdinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "TibcoMessageId", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TibcoMessageId");
        }
    }
}
