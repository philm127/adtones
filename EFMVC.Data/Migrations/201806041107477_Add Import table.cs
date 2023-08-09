namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImporttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceCode = c.String(),
                        CallingNumber = c.String(),
                        CalledNumber = c.String(),
                        RBTCode = c.String(),
                        SPCode = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CallScheme = c.String(),
                        DTMF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Imports");
        }
    }
}
