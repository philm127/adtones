namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUdatedByFieldinquestiontable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "UpdatedBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "UpdatedBy");
        }
    }
}
