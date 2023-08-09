namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionalUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionalUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MSISDN = c.String(),
                        BatchID = c.Int(nullable: false),
                        DailyPlay = c.Int(nullable: false),
                        WeeklyPlay = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PromotionalUsers");
        }
    }
}
