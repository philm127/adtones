namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addoperatortable : DbMigration
    {
        public override void Up()
        {
           
            
            
            CreateTable(
                "dbo.Operators",
                c => new
                    {
                        OperatorId = c.Int(nullable: false, identity: true),
                        OperatorName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.OperatorId);
            
            
        }
        
        public override void Down()
        {
           
        }
    }
}
