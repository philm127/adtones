namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactPhoneinClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "ContactPhone", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "ContactPhone");
        }
    }
}
