namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailandPhoneticAlphabetinClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "Email", c => c.String());
            AddColumn("dbo.Client", "PhoneticAlphabet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Client", "PhoneticAlphabet");
            DropColumn("dbo.Client", "Email");
        }
    }
}
