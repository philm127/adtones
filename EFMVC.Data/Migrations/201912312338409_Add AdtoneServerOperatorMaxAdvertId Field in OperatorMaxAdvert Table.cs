namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerOperatorMaxAdvertIdFieldinOperatorMaxAdvertTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatorMaxAdverts", "AdtoneServerOperatorMaxAdvertId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OperatorMaxAdverts", "AdtoneServerOperatorMaxAdvertId");
        }
    }
}
