namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMpesaHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MpesaHistories",
                c => new
                    {
                        MpesaHistoryId = c.Int(nullable: false, identity: true),
                        BillingId = c.Int(nullable: false),
                        ReceiptNo = c.String(),
                        TransactionType = c.String(),
                        Description = c.String(),
                        AccountReference = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.MpesaHistoryId)
                .ForeignKey("dbo.Billing", t => t.BillingId, cascadeDelete: true)
                .Index(t => t.BillingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MpesaHistories", "BillingId", "dbo.Billing");
            DropIndex("dbo.MpesaHistories", new[] { "BillingId" });
            DropTable("dbo.MpesaHistories");
        }
    }
}
