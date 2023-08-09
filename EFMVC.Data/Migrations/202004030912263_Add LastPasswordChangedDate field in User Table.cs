namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastPasswordChangedDatefieldinUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastPasswordChangedDate");
        }
    }
}
