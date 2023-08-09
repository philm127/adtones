namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailVerificationCodetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailVerificationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        VerificationCode = c.String(maxLength: 10),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailVerificationCodes", "UserId", "dbo.Users");
            DropIndex("dbo.EmailVerificationCodes", new[] { "UserId" });
            DropTable("dbo.EmailVerificationCodes");
        }
    }
}
