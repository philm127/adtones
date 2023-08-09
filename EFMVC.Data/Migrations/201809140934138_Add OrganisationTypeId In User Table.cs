namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganisationTypeIdInUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OrganisationTypeId", c => c.Int());
            CreateIndex("dbo.Users", "OrganisationTypeId");
            AddForeignKey("dbo.Users", "OrganisationTypeId", "dbo.OrganisationTypes", "OrganisationTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "OrganisationTypeId", "dbo.OrganisationTypes");
            DropIndex("dbo.Users", new[] { "OrganisationTypeId" });
            DropColumn("dbo.Users", "OrganisationTypeId");
        }
    }
}
