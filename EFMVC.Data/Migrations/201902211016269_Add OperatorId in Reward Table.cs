namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorIdinRewardTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rewards", "OperatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rewards", "OperatorId");
            AddForeignKey("dbo.Rewards", "OperatorId", "dbo.Operators", "OperatorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rewards", "OperatorId", "dbo.Operators");
            DropIndex("dbo.Rewards", new[] { "OperatorId" });
            DropColumn("dbo.Rewards", "OperatorId");
        }
    }
}
