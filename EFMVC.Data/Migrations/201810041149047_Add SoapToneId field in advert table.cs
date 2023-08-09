namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoapToneIdfieldinadverttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "SoapToneId", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "SoapToneId");
        }
    }
}
