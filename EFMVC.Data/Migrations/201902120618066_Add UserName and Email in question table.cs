namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameandEmailinquestiontable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "UserName", c => c.String(maxLength: 50));
            AddColumn("dbo.Question", "Email", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "Email");
            DropColumn("dbo.Question", "UserName");
        }
    }
}
