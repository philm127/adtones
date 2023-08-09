namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneticAlphabetinAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "PhoneticAlphabet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "PhoneticAlphabet");
        }
    }
}
