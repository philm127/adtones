namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorConfigurationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperatorConfigurations",
                c => new
                    {
                        OperatorConfigurationId = c.Int(nullable: false, identity: true),
                        OperatorId = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        AdtoneServerOperatorConfigurationId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OperatorConfigurationId)
                .ForeignKey("dbo.Operators", t => t.OperatorId, cascadeDelete: true)
                .Index(t => t.OperatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatorConfigurations", "OperatorId", "dbo.Operators");
            DropIndex("dbo.OperatorConfigurations", new[] { "OperatorId" });
            DropTable("dbo.OperatorConfigurations");
        }
    }
}
