namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganisationTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganisationTypes",
                c => new
                    {
                        OrganisationTypeId = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.OrganisationTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrganisationTypes");
        }
    }
}
