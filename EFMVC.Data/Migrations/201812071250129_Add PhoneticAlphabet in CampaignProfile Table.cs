namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneticAlphabetinCampaignProfileTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfile", "PhoneticAlphabet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignProfile", "PhoneticAlphabet");
        }
    }
}
