namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addforeignkeytousertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OperatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "OperatorId");
            //AddForeignKey("dbo.Users", "OperatorId", "dbo.Operators", "OperatorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Users", "OperatorId", "dbo.Operators");
            //DropIndex("dbo.Users", new[] { "OperatorId" });
            //DropColumn("dbo.Users", "OperatorId");
        }
    }
}
