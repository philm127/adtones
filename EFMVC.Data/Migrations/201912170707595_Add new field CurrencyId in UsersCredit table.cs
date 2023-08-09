namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewfieldCurrencyIdinUsersCredittable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersCredit", "CurrencyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersCredit", "CurrencyId");
        }
    }
}
