namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneserverIdinOperatorTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "AdtoneServerOperatorId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operators", "AdtoneServerOperatorId");
        }
    }
}
