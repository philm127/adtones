namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorIdFieldinOperatorFtpdetailtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatorFTPDetails", "OperatorId", c => c.Int(nullable: false));
            AlterColumn("dbo.OperatorFTPDetails", "Host", c => c.String(maxLength: 100));
            AlterColumn("dbo.OperatorFTPDetails", "Port", c => c.String(maxLength: 10));
            AlterColumn("dbo.OperatorFTPDetails", "UserName", c => c.String(maxLength: 100));
            AlterColumn("dbo.OperatorFTPDetails", "Password", c => c.String(maxLength: 100));
            AlterColumn("dbo.OperatorFTPDetails", "FtpRoot", c => c.String(maxLength: 100));
            CreateIndex("dbo.OperatorFTPDetails", "OperatorId");
            AddForeignKey("dbo.OperatorFTPDetails", "OperatorId", "dbo.Operators", "OperatorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatorFTPDetails", "OperatorId", "dbo.Operators");
            DropIndex("dbo.OperatorFTPDetails", new[] { "OperatorId" });
            AlterColumn("dbo.OperatorFTPDetails", "FtpRoot", c => c.String());
            AlterColumn("dbo.OperatorFTPDetails", "Password", c => c.String());
            AlterColumn("dbo.OperatorFTPDetails", "UserName", c => c.String());
            AlterColumn("dbo.OperatorFTPDetails", "Port", c => c.String());
            AlterColumn("dbo.OperatorFTPDetails", "Host", c => c.String());
            DropColumn("dbo.OperatorFTPDetails", "OperatorId");
        }
    }
}
