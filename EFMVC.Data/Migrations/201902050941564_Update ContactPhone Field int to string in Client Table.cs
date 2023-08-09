namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactPhoneFieldinttostringinClientTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Client", "ContactPhone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Client", "ContactPhone", c => c.Int(nullable: false));
        }
    }
}
