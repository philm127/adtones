namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedatatypeofDateCreatedfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ImportUserCSVs", "DateCreated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ImportUserCSVs", "DateCreated", c => c.String(maxLength: 30));
        }
    }
}
