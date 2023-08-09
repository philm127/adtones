namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorIdInAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "OperatorId", c => c.Int());
            CreateIndex("dbo.Advert", "OperatorId");
            AddForeignKey("dbo.Advert", "OperatorId", "dbo.Operators", "OperatorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advert", "OperatorId", "dbo.Operators");
            DropIndex("dbo.Advert", new[] { "OperatorId" });
            DropColumn("dbo.Advert", "OperatorId");
        }
    }
}
