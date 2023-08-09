namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneticAlphabetcolumninusertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PhoneticAlphabet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PhoneticAlphabet");
        }
    }
}
