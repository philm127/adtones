namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorMaxAdvertTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperatorMaxAdverts",
                c => new
                    {
                        OperatorMaxAdvertId = c.Int(nullable: false, identity: true),
                        KeyName = c.String(maxLength: 100),
                        KeyValue = c.String(maxLength: 10),
                        Addeddate = c.DateTime(nullable: false),
                        Updateddate = c.DateTime(),
                        OperatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OperatorMaxAdvertId)
                .ForeignKey("dbo.Operators", t => t.OperatorId, cascadeDelete: true)
                .Index(t => t.OperatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatorMaxAdverts", "OperatorId", "dbo.Operators");
            DropIndex("dbo.OperatorMaxAdverts", new[] { "OperatorId" });
            DropTable("dbo.OperatorMaxAdverts");
        }
    }
}
