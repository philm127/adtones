namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGravityFormsTrackTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GravityFormsTracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GravityFormsId = c.Int(),
                        Email = c.String(),
                        MSISDN = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GravityFormsTracks");
        }
    }
}
