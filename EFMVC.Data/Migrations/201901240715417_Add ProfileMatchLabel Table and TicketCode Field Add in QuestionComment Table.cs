namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileMatchLabelTableandTicketCodeFieldAddinQuestionCommentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileMatchLabels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileMatchInformationId = c.Int(nullable: false),
                        ProfileLabel = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProfileMatchInformations", t => t.ProfileMatchInformationId, cascadeDelete: true)
                .Index(t => t.ProfileMatchInformationId);
            
            AddColumn("dbo.QuestionComment", "TicketCode", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileMatchLabels", "ProfileMatchInformationId", "dbo.ProfileMatchInformations");
            DropIndex("dbo.ProfileMatchLabels", new[] { "ProfileMatchInformationId" });
            DropColumn("dbo.QuestionComment", "TicketCode");
            DropTable("dbo.ProfileMatchLabels");
        }
    }
}
