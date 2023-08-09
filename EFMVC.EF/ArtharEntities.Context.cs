﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFMVC.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    //using System.Data.Objects;
    //using System.Data.Objects.DataClasses;
    using System.Linq;

    public partial class ArtharEntities : DbContext
    {
        public ArtharEntities()
            : base("name=ArtharEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<BillingDetail> BillingDetails { get; set; }
        public DbSet<BlockedNumber> BlockedNumbers { get; set; }
        public DbSet<BucketAuditRow> BucketAuditRows { get; set; }
        public DbSet<BucketAudit> BucketAudits { get; set; }
        public DbSet<CampaignAdvert> CampaignAdverts { get; set; }
        public DbSet<CampaignAudit> CampaignAudits { get; set; }
        public DbSet<CampaignProfile> CampaignProfiles { get; set; }
        public DbSet<CampaignProfileAdvert> CampaignProfileAdverts { get; set; }
        public DbSet<CampaignProfileAttitude> CampaignProfileAttitudes { get; set; }
        public DbSet<CampaignProfileCinema> CampaignProfileCinemas { get; set; }
        public DbSet<CampaignProfileDateSetting> CampaignProfileDateSettings { get; set; }
        public DbSet<CampaignProfileDemographic> CampaignProfileDemographics { get; set; }
        public DbSet<CampaignProfileInternet> CampaignProfileInternets { get; set; }
        public DbSet<CampaignProfileMobile> CampaignProfileMobiles { get; set; }
        public DbSet<CampaignProfileMSISDN> CampaignProfileMSISDNs { get; set; }
        public DbSet<CampaignProfilePress> CampaignProfilePresses { get; set; }
        public DbSet<CampaignProfileProductsService> CampaignProfileProductsServices { get; set; }
        public DbSet<CampaignProfileRadio> CampaignProfileRadios { get; set; }
        public DbSet<CampaignProfileReport> CampaignProfileReports { get; set; }
        public DbSet<CampaignProfileTimeSetting> CampaignProfileTimeSettings { get; set; }
        public DbSet<CampaignProfileTv> CampaignProfileTvs { get; set; }
        public DbSet<CampaignProfileUserProfile> CampaignProfileUserProfiles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CompanyDetail> CompanyDetails { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryTax> CountryTaxes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionComment> QuestionComments { get; set; }
        public DbSet<QuestionCommentImage> QuestionCommentImages { get; set; }
        public DbSet<QuestionImage> QuestionImages { get; set; }
        public DbSet<QuestionSubject> QuestionSubjects { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileAdvert> UserProfileAdverts { get; set; }
        public DbSet<UserProfileAdvertsReceived> UserProfileAdvertsReceiveds { get; set; }
        public DbSet<UserProfileAttitude> UserProfileAttitudes { get; set; }
        public DbSet<UserProfileCinema> UserProfileCinemas { get; set; }
        public DbSet<UserProfileCreditsReceived> UserProfileCreditsReceiveds { get; set; }
        public DbSet<UserProfileInternet> UserProfileInternets { get; set; }
        public DbSet<UserProfileMobile> UserProfileMobiles { get; set; }
        public DbSet<UserProfilePress> UserProfilePresses { get; set; }
        public DbSet<UserProfileProductsService> UserProfileProductsServices { get; set; }
        public DbSet<UserProfileRadio> UserProfileRadios { get; set; }
        public DbSet<UserProfileTimeSetting> UserProfileTimeSettings { get; set; }
        public DbSet<UserProfileTv> UserProfileTvs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersCredit> UsersCredits { get; set; }
        public DbSet<UsersCreditPayment> UsersCreditPayments { get; set; }
    
        public virtual int uspa_campaign_user_profilematch()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspa_campaign_user_profilematch");
        }
    
        public virtual int uspa_delete_campaign_user_match(Nullable<long> campaignProfileId)
        {
            var campaignProfileIdParameter = campaignProfileId.HasValue ?
                new ObjectParameter("CampaignProfileId", campaignProfileId) :
                new ObjectParameter("CampaignProfileId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspa_delete_campaign_user_match", campaignProfileIdParameter);
        }
    
        public virtual int BucketAuditRow()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketAuditRow");
        }
    
        public virtual int BucketProvision()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision");
        }
    
        public virtual int BucketProvision10()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision10");
        }
    
        public virtual int BucketProvision2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision2");
        }
    
        public virtual int BucketProvision3()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision3");
        }
    
        public virtual int BucketProvision4()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision4");
        }
    
        public virtual int BucketProvision5()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision5");
        }
    
        public virtual int BucketProvision6()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision6");
        }
    
        public virtual int BucketProvision7()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision7");
        }
    
        public virtual int BucketProvision8()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision8");
        }
    
        public virtual int BucketProvision9()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BucketProvision9");
        }
    
        public virtual int CampaignUserMatchSp()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp");
        }
    
        public virtual int CampaignUserMatchSp10()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp10");
        }
    
        public virtual int CampaignUserMatchSp2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp2");
        }
    
        public virtual int CampaignUserMatchSp3()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp3");
        }
    
        public virtual int CampaignUserMatchSp4()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp4");
        }
    
        public virtual int CampaignUserMatchSp5()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp5");
        }
    
        public virtual int CampaignUserMatchSp6()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp6");
        }
    
        public virtual int CampaignUserMatchSp7()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp7");
        }
    
        public virtual int CampaignUserMatchSp8()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp8");
        }
    
        public virtual int CampaignUserMatchSp9()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CampaignUserMatchSp9");
        }
    
        public virtual int LatestBucketProvision()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("LatestBucketProvision");
        }
    
        public virtual int ProcessBucketAudit()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit");
        }
    
        public virtual int ProcessBucketAudit10()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit10");
        }
    
        public virtual int ProcessBucketAudit2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit2");
        }
    
        public virtual int ProcessBucketAudit3()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit3");
        }
    
        public virtual int ProcessBucketAudit4()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit4");
        }
    
        public virtual int ProcessBucketAudit5()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit5");
        }
    
        public virtual int ProcessBucketAudit6()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit6");
        }
    
        public virtual int ProcessBucketAudit7()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit7");
        }
    
        public virtual int ProcessBucketAudit8()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit8");
        }
    
        public virtual int ProcessBucketAudit9()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAudit9");
        }
    
        public virtual int ProcessBucketAuditBackup()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ProcessBucketAuditBackup");
        }
    
        public virtual int Test2LatestBucketProvision()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Test2LatestBucketProvision");
        }
    
        public virtual int Test3LatestBucketProvision()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Test3LatestBucketProvision");
        }
    
        public virtual int TestLatestBucketProvision()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TestLatestBucketProvision");
        }
    
        public virtual int TransferData()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData");
        }
    
        public virtual int TransferData10()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData10");
        }
    
        public virtual int TransferData2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData2");
        }
    
        public virtual int TransferData3()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData3");
        }
    
        public virtual int TransferData4()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData4");
        }
    
        public virtual int TransferData5()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData5");
        }
    
        public virtual int TransferData6()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData6");
        }
    
        public virtual int TransferData7()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData7");
        }
    
        public virtual int TransferData8()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData8");
        }
    
        public virtual int TransferData9()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TransferData9");
        }
    
        public virtual int UsersAdvertProcess()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UsersAdvertProcess");
        }
    }
}
