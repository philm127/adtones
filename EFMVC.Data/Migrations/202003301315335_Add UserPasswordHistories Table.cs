namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPasswordHistoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPasswordHistories",
                c => new
                    {
                        UserPasswordHistoryId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PasswordHash = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        AdtoneServerUserPasswordHistoryId = c.Int(),
                    })
                .PrimaryKey(t => t.UserPasswordHistoryId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPasswordHistories", "UserId", "dbo.Users");
            DropIndex("dbo.UserPasswordHistories", new[] { "UserId" });
            DropTable("dbo.UserPasswordHistories");
        }
    }
}
