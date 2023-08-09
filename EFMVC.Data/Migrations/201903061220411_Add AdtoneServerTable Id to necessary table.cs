namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdtoneServerTableIdtonecessarytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertRejections", "AdtoneServerAdvertRejectionId", c => c.Int());
            AddColumn("dbo.Advert", "AdtoneServerAdvertId", c => c.Int());
            AddColumn("dbo.CampaignAdverts", "AdtoneServerCampaignAdvertId", c => c.Int());
            AddColumn("dbo.CampaignProfile", "AdtoneServerCampaignProfileId", c => c.Int());
            AddColumn("dbo.CampaignProfilePreference", "AdtoneServerCampaignProfilePrefId", c => c.Int());
            AddColumn("dbo.Users", "AdtoneServerUserId", c => c.Int());
            AddColumn("dbo.Client", "AdtoneServerClientId", c => c.Int());
            AddColumn("dbo.UserProfilePreference", "AdtoneServerUserProfilePrefId", c => c.Int());
            AddColumn("dbo.Billing", "AdtoneServerBillingId", c => c.Int());
            AddColumn("dbo.CampaignProfileTimeSetting", "AdtoneServerCampaignProfileTimeId", c => c.Int());
            AddColumn("dbo.BillingDetails", "AdtoneServerBillingDetailId", c => c.Int());
            AddColumn("dbo.CampaignMatches", "AdtoneServerCampaignMatchId", c => c.Int());
            AddColumn("dbo.UserMatches", "AdtoneServerUserMatchId", c => c.Int());
            AddColumn("dbo.UserMatch10", "AdtoneServerUserMatch10Id", c => c.Int());
            AddColumn("dbo.UserMatch2", "AdtoneServerUserMatch2Id", c => c.Int());
            AddColumn("dbo.UserMatch3", "AdtoneServerUserMatch3Id", c => c.Int());
            AddColumn("dbo.UserMatch4", "AdtoneServerUserMatch4Id", c => c.Int());
            AddColumn("dbo.UserMatch5", "AdtoneServerUserMatch5Id", c => c.Int());
            AddColumn("dbo.UserMatch6", "AdtoneServerUserMatch6Id", c => c.Int());
            AddColumn("dbo.UserMatch7", "AdtoneServerUserMatch7Id", c => c.Int());
            AddColumn("dbo.UserMatch8", "AdtoneServerUserMatch8Id", c => c.Int());
            AddColumn("dbo.UserMatch9", "AdtoneServerUserMatch9Id", c => c.Int());
            AddColumn("dbo.CompanyDetails", "AdtoneServerCompanyDetailId", c => c.Int());
            AddColumn("dbo.Contacts", "AdtoneServerContactId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "AdtoneServerContactId");
            DropColumn("dbo.CompanyDetails", "AdtoneServerCompanyDetailId");
            DropColumn("dbo.UserMatch9", "AdtoneServerUserMatch9Id");
            DropColumn("dbo.UserMatch8", "AdtoneServerUserMatch8Id");
            DropColumn("dbo.UserMatch7", "AdtoneServerUserMatch7Id");
            DropColumn("dbo.UserMatch6", "AdtoneServerUserMatch6Id");
            DropColumn("dbo.UserMatch5", "AdtoneServerUserMatch5Id");
            DropColumn("dbo.UserMatch4", "AdtoneServerUserMatch4Id");
            DropColumn("dbo.UserMatch3", "AdtoneServerUserMatch3Id");
            DropColumn("dbo.UserMatch2", "AdtoneServerUserMatch2Id");
            DropColumn("dbo.UserMatch10", "AdtoneServerUserMatch10Id");
            DropColumn("dbo.UserMatches", "AdtoneServerUserMatchId");
            DropColumn("dbo.CampaignMatches", "AdtoneServerCampaignMatchId");
            DropColumn("dbo.BillingDetails", "AdtoneServerBillingDetailId");
            DropColumn("dbo.CampaignProfileTimeSetting", "AdtoneServerCampaignProfileTimeId");
            DropColumn("dbo.Billing", "AdtoneServerBillingId");
            DropColumn("dbo.UserProfilePreference", "AdtoneServerUserProfilePrefId");
            DropColumn("dbo.Client", "AdtoneServerClientId");
            DropColumn("dbo.Users", "AdtoneServerUserId");
            DropColumn("dbo.CampaignProfilePreference", "AdtoneServerCampaignProfilePrefId");
            DropColumn("dbo.CampaignProfile", "AdtoneServerCampaignProfileId");
            DropColumn("dbo.CampaignAdverts", "AdtoneServerCampaignAdvertId");
            DropColumn("dbo.Advert", "AdtoneServerAdvertId");
            DropColumn("dbo.AdvertRejections", "AdtoneServerAdvertRejectionId");
        }
    }
}
