namespace EFMVC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsubfieldforskizaprofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignProfilePreference", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignProfilePreference", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignProfilePreference", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignProfilePreference", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserProfilePreference", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserProfilePreference", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserProfilePreference", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserProfilePreference", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatches", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatches", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatches", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatches", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch10", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch10", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch10", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch10", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch2", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch2", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch2", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch2", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch3", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch3", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch3", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch3", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch4", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch4", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch4", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch4", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch5", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch5", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch5", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch5", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch6", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch6", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch6", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch6", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch7", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch7", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch7", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch7", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch8", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch8", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch8", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch8", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch9", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch9", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch9", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch9", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatches", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatches", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatches", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatches", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch10", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch10", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch10", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch10", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch2", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch2", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch2", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch2", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch3", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch3", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch3", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch3", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch4", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch4", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch4", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch4", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch5", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch5", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch5", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch5", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch6", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch6", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch6", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch6", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch7", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch7", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch7", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch7", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch8", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch8", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch8", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch8", "Mass_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch9", "Hustlers_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch9", "Youth_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch9", "DiscerningProfessionals_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch9", "Mass_AdType", c => c.String(maxLength: 50));
            DropColumn("dbo.CampaignProfilePreference", "SkizaProfile_AdType");
            DropColumn("dbo.UserProfilePreference", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatches", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch10", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch2", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch3", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch4", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch5", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch6", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch7", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch8", "SkizaProfile_AdType");
            DropColumn("dbo.CampaignMatch9", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatches", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch10", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch2", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch3", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch4", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch5", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch6", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch7", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch8", "SkizaProfile_AdType");
            DropColumn("dbo.UserMatch9", "SkizaProfile_AdType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserMatch9", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch8", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch7", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch6", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch5", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch4", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch3", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch2", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatch10", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserMatches", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch9", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch8", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch7", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch6", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch5", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch4", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch3", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch2", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatch10", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignMatches", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.UserProfilePreference", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            AddColumn("dbo.CampaignProfilePreference", "SkizaProfile_AdType", c => c.String(maxLength: 50));
            DropColumn("dbo.UserMatch9", "Mass_AdType");
            DropColumn("dbo.UserMatch9", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch9", "Youth_AdType");
            DropColumn("dbo.UserMatch9", "Hustlers_AdType");
            DropColumn("dbo.UserMatch8", "Mass_AdType");
            DropColumn("dbo.UserMatch8", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch8", "Youth_AdType");
            DropColumn("dbo.UserMatch8", "Hustlers_AdType");
            DropColumn("dbo.UserMatch7", "Mass_AdType");
            DropColumn("dbo.UserMatch7", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch7", "Youth_AdType");
            DropColumn("dbo.UserMatch7", "Hustlers_AdType");
            DropColumn("dbo.UserMatch6", "Mass_AdType");
            DropColumn("dbo.UserMatch6", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch6", "Youth_AdType");
            DropColumn("dbo.UserMatch6", "Hustlers_AdType");
            DropColumn("dbo.UserMatch5", "Mass_AdType");
            DropColumn("dbo.UserMatch5", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch5", "Youth_AdType");
            DropColumn("dbo.UserMatch5", "Hustlers_AdType");
            DropColumn("dbo.UserMatch4", "Mass_AdType");
            DropColumn("dbo.UserMatch4", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch4", "Youth_AdType");
            DropColumn("dbo.UserMatch4", "Hustlers_AdType");
            DropColumn("dbo.UserMatch3", "Mass_AdType");
            DropColumn("dbo.UserMatch3", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch3", "Youth_AdType");
            DropColumn("dbo.UserMatch3", "Hustlers_AdType");
            DropColumn("dbo.UserMatch2", "Mass_AdType");
            DropColumn("dbo.UserMatch2", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch2", "Youth_AdType");
            DropColumn("dbo.UserMatch2", "Hustlers_AdType");
            DropColumn("dbo.UserMatch10", "Mass_AdType");
            DropColumn("dbo.UserMatch10", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatch10", "Youth_AdType");
            DropColumn("dbo.UserMatch10", "Hustlers_AdType");
            DropColumn("dbo.UserMatches", "Mass_AdType");
            DropColumn("dbo.UserMatches", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserMatches", "Youth_AdType");
            DropColumn("dbo.UserMatches", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch9", "Mass_AdType");
            DropColumn("dbo.CampaignMatch9", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch9", "Youth_AdType");
            DropColumn("dbo.CampaignMatch9", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch8", "Mass_AdType");
            DropColumn("dbo.CampaignMatch8", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch8", "Youth_AdType");
            DropColumn("dbo.CampaignMatch8", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch7", "Mass_AdType");
            DropColumn("dbo.CampaignMatch7", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch7", "Youth_AdType");
            DropColumn("dbo.CampaignMatch7", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch6", "Mass_AdType");
            DropColumn("dbo.CampaignMatch6", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch6", "Youth_AdType");
            DropColumn("dbo.CampaignMatch6", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch5", "Mass_AdType");
            DropColumn("dbo.CampaignMatch5", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch5", "Youth_AdType");
            DropColumn("dbo.CampaignMatch5", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch4", "Mass_AdType");
            DropColumn("dbo.CampaignMatch4", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch4", "Youth_AdType");
            DropColumn("dbo.CampaignMatch4", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch3", "Mass_AdType");
            DropColumn("dbo.CampaignMatch3", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch3", "Youth_AdType");
            DropColumn("dbo.CampaignMatch3", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch2", "Mass_AdType");
            DropColumn("dbo.CampaignMatch2", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch2", "Youth_AdType");
            DropColumn("dbo.CampaignMatch2", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatch10", "Mass_AdType");
            DropColumn("dbo.CampaignMatch10", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatch10", "Youth_AdType");
            DropColumn("dbo.CampaignMatch10", "Hustlers_AdType");
            DropColumn("dbo.CampaignMatches", "Mass_AdType");
            DropColumn("dbo.CampaignMatches", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignMatches", "Youth_AdType");
            DropColumn("dbo.CampaignMatches", "Hustlers_AdType");
            DropColumn("dbo.UserProfilePreference", "Mass_AdType");
            DropColumn("dbo.UserProfilePreference", "DiscerningProfessionals_AdType");
            DropColumn("dbo.UserProfilePreference", "Youth_AdType");
            DropColumn("dbo.UserProfilePreference", "Hustlers_AdType");
            DropColumn("dbo.CampaignProfilePreference", "Mass_AdType");
            DropColumn("dbo.CampaignProfilePreference", "DiscerningProfessionals_AdType");
            DropColumn("dbo.CampaignProfilePreference", "Youth_AdType");
            DropColumn("dbo.CampaignProfilePreference", "Hustlers_AdType");
        }
    }
}
