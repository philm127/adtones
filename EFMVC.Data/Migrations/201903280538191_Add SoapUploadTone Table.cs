namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoapUploadToneTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoapUploadTones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadId = c.Long(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Advert", "SoapToneCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "SoapToneCode");
            DropTable("dbo.SoapUploadTones");
        }
    }
}
