namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorFTPDetailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperatorFTPDetails",
                c => new
                    {
                        OperatorFTPDetailId = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        Port = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        FtpRoot = c.String(),
                    })
                .PrimaryKey(t => t.OperatorFTPDetailId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OperatorFTPDetails");
        }
    }
}
