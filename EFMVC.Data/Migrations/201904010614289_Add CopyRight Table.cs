namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCopyRightTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CopyRights",
                c => new
                    {
                        CopyRightId = c.Int(nullable: false, identity: true),
                        CopyRightText = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CopyRightId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CopyRights");
        }
    }
}
