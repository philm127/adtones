// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="EFMVCDataContex.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using EFMVC.Data.Configurations;
using EFMVC.Model;
using EFMVC.Model.Entities;

/// <summary>
/// The Data namespace.
/// </summary>

namespace EFMVC.Data
{
    /// <summary>
    /// Class EFMVCDataContex.
    /// </summary>
    /// 


    public class EFMVCDataContex : DbContext
    {

        public EFMVCDataContex()
            : base("name=EFMVCDataContex")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 3600;
            // Database.Log = s => WriteFile.WriteSQL(s);
        }

        public EFMVCDataContex(string conn)
           : base(conn)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 3600;
            // Database.Log = s => WriteFile.WriteSQL(s);
        }


        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the userprofiles.
        /// </summary>
        /// <value>The userprofiles.</value>
        public DbSet<UserProfile> Userprofiles { get; set; }

        /// <summary>
        /// Gets or sets the user profile advert.
        /// </summary>
        /// <value>The user profile advert.</value>
        public DbSet<UserProfileAdvert> UserProfileAdvert { get; set; }

        /// <summary>
        /// Gets or sets the user profile attitude.
        /// </summary>
        /// <value>The user profile attitude.</value>
        public DbSet<UserProfileAttitude> UserProfileAttitude { get; set; }

        /// <summary>
        /// Gets or sets the user profile cinema.
        /// </summary>
        /// <value>The user profile cinema.</value>
        public DbSet<UserProfileCinema> UserProfileCinema { get; set; }

        /// <summary>
        /// Gets or sets the user profile internet.
        /// </summary>
        /// <value>The user profile internet.</value>
        public DbSet<UserProfileInternet> UserProfileInternet { get; set; }

        /// <summary>
        /// Gets or sets the user profile mobile.
        /// </summary>
        /// <value>The user profile mobile.</value>
        public DbSet<UserProfileMobile> UserProfileMobile { get; set; }

        /// <summary>
        /// Gets or sets the user profile press.
        /// </summary>
        /// <value>The user profile press.</value>
        public DbSet<UserProfilePress> UserProfilePress { get; set; }

        /// <summary>
        /// Gets or sets the user profile products services.
        /// </summary>
        /// <value>The user profile products services.</value>
        public DbSet<UserProfileProductsService> UserProfileProductsServices { get; set; }

        /// <summary>
        /// Gets or sets the user profile radio.
        /// </summary>
        /// <value>The user profile radio.</value>
        public DbSet<UserProfileRadio> UserProfileRadio { get; set; }

        /// <summary>
        /// Gets or sets the user profile time setting.
        /// </summary>
        /// <value>The user profile time setting.</value>
        public DbSet<UserProfileTimeSetting> UserProfileTimeSetting { get; set; }

        /// <summary>
        /// Gets or sets the user profile tv.
        /// </summary>
        /// <value>The user profile tv.</value>
        public DbSet<UserProfileTv> UserProfileTv { get; set; }

        /// <summary>
        /// Gets or sets the blocked numbers.
        /// </summary>
        /// <value>The blocked numbers.</value>
        public DbSet<BlockedNumber> BlockedNumbers { get; set; }

        /// <summary>
        /// Gets or sets the campaign audits.
        /// </summary>
        /// <value>The campaign audits.</value>
        public DbSet<CampaignAudit> CampaignAudits { get; set; }

        /// <summary>
        /// Gets or sets the campaign adverts.
        /// </summary>
        /// <value>The campaign adverts.</value>
        public DbSet<CampaignAdvert> CampaignAdverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profiles.
        /// </summary>
        /// <value>The campaign profiles.</value>
        public DbSet<CampaignProfile> CampaignProfiles { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile adverts.
        /// </summary>
        /// <value>The campaign profile adverts.</value>
        public DbSet<CampaignProfileAdvert> CampaignProfileAdverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile attitudes.
        /// </summary>
        /// <value>The campaign profile attitudes.</value>
        public DbSet<CampaignProfileAttitude> CampaignProfileAttitudes { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile cinemas.
        /// </summary>
        /// <value>The campaign profile cinemas.</value>
        public DbSet<CampaignProfileCinema> CampaignProfileCinemas { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile date settings.
        /// </summary>
        /// <value>The campaign profile date settings.</value>
        public DbSet<CampaignProfileDateSettings> CampaignProfileDateSettings { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile internets.
        /// </summary>
        /// <value>The campaign profile internets.</value>
        public DbSet<CampaignProfileInternet> CampaignProfileInternets { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile mobiles.
        /// </summary>
        /// <value>The campaign profile mobiles.</value>
        public DbSet<CampaignProfileMobile> CampaignProfileMobiles { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile press.
        /// </summary>
        /// <value>The campaign profile press.</value>
        public DbSet<CampaignProfilePress> CampaignProfilePress { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile products services.
        /// </summary>
        /// <value>The campaign profile products services.</value>
        public DbSet<CampaignProfileProductsService> CampaignProfileProductsServices { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile radios.
        /// </summary>
        /// <value>The campaign profile radios.</value>
        public DbSet<CampaignProfileRadio> CampaignProfileRadios { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile time settings.
        /// </summary>
        /// <value>The campaign profile time settings.</value>
        public DbSet<CampaignProfileTimeSetting> CampaignProfileTimeSettings { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile TVS.
        /// </summary>
        /// <value>The campaign profile TVS.</value>
        public DbSet<CampaignProfileTv> CampaignProfileTvs { get; set; }

        /// <summary>
        /// Gets or sets the adverts.
        /// </summary>
        /// <value>The adverts.</value>
        public DbSet<Advert> Adverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile user profiles.
        /// </summary>
        /// <value>The campaign profile user profiles.</value>
        public DbSet<CampaignProfileUserProfile> CampaignProfileUserProfiles { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile demographics.
        /// </summary>
        /// <value>The campaign profile demographics.</value>
        public DbSet<CampaignProfileDemographics> CampaignProfileDemographics { get; set; }

        /// <summary>
        /// Gets or sets the bucket audits.
        /// </summary>
        /// <value>The bucket audits.</value>
        public DbSet<BucketAudit> BucketAudits { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit rows.
        /// </summary>
        /// <value>The bucket audit rows.</value>
        public DbSet<BucketAuditRow> BucketAuditRows { get; set; }

        /// <summary>
        /// Gets or sets the Contacts.
        /// </summary>
        /// <value>The Contacts.</value>
        public DbSet<Contacts> Contacts { get; set; }

        public DbSet<CompanyDetails> CompanyDetails { get; set; }

        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public DbSet<Billing> Billings { get; set; }

        public DbSet<BillingDetails> BillingDetails { get; set; }


        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<UsersCredit> UsersCredits { get; set; }

        public DbSet<UsersCreditPayment> UsersCreditPayments { get; set; }


        public DbSet<Question> Questions { get; set; }


        public DbSet<QuestionComment> QuestionComments { get; set; }

        public DbSet<QuestionCommentImages> QuestionCommentImagess { get; set; }

        public DbSet<QuestionImages> QuestionImagess { get; set; }

        public DbSet<QuestionSubject> QuestionSubjects { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<CountryTax> CountryTax { get; set; }

        public DbSet<UserProfilePreference> UserProfilePreference { get; set; }

        public DbSet<CampaignProfilePreference> CampaignProfilePreference { get; set; }
        public DbSet<Operator> Operator { get; set; }


        public DbSet<BucketBatchStatus> BucketBatchStatus { get; set; }

        public DbSet<CampaignMatch> CampaignMatch { get; set; }
        public DbSet<CampaignMatch2> CampaignMatch2 { get; set; }
        public DbSet<CampaignMatch3> CampaignMatch3 { get; set; }
        public DbSet<CampaignMatch4> CampaignMatch4 { get; set; }
        public DbSet<CampaignMatch5> CampaignMatch5 { get; set; }
        public DbSet<CampaignMatch6> CampaignMatch6 { get; set; }
        public DbSet<CampaignMatch7> CampaignMatch7 { get; set; }
        public DbSet<CampaignMatch8> CampaignMatch8 { get; set; }
        public DbSet<CampaignMatch9> CampaignMatch9 { get; set; }
        public DbSet<CampaignMatch10> CampaignMatch10 { get; set; }


        public DbSet<UserMatch> UserMatch { get; set; }
        public DbSet<UserMatch2> UserMatch2 { get; set; }
        public DbSet<UserMatch3> UserMatch3 { get; set; }
        public DbSet<UserMatch4> UserMatch4 { get; set; }
        public DbSet<UserMatch5> UserMatch5 { get; set; }
        public DbSet<UserMatch6> UserMatch6 { get; set; }
        public DbSet<UserMatch7> UserMatch7 { get; set; }
        public DbSet<UserMatch8> UserMatch8 { get; set; }
        public DbSet<UserMatch9> UserMatch9 { get; set; }
        public DbSet<UserMatch10> UserMatch10 { get; set; }

        public DbSet<CampaignmatchUsermatch> CampaignmatchUsermatch { get; set; }
        public DbSet<CampaignmatchUsermatch2> CampaignmatchUsermatch2 { get; set; }
        public DbSet<CampaignmatchUsermatch3> CampaignmatchUsermatch3 { get; set; }
        public DbSet<CampaignmatchUsermatch4> CampaignmatchUsermatch4 { get; set; }
        public DbSet<CampaignmatchUsermatch5> CampaignmatchUsermatch5 { get; set; }
        public DbSet<CampaignmatchUsermatch6> CampaignmatchUsermatch6 { get; set; }
        public DbSet<CampaignmatchUsermatch7> CampaignmatchUsermatch7 { get; set; }
        public DbSet<CampaignmatchUsermatch8> CampaignmatchUsermatch8 { get; set; }
        public DbSet<CampaignmatchUsermatch9> CampaignmatchUsermatch9 { get; set; }
        public DbSet<CampaignmatchUsermatch10> CampaignmatchUsermatch10 { get; set; }

        public DbSet<BucketBatch> BucketBatch { get; set; }
        public DbSet<BucketBatch2> BucketBatch2 { get; set; }
        public DbSet<BucketBatch3> BucketBatch3 { get; set; }
        public DbSet<BucketBatch4> BucketBatch4 { get; set; }
        public DbSet<BucketBatch5> BucketBatch5 { get; set; }
        public DbSet<BucketBatch6> BucketBatch6 { get; set; }
        public DbSet<BucketBatch7> BucketBatch7 { get; set; }
        public DbSet<BucketBatch8> BucketBatch8 { get; set; }
        public DbSet<BucketBatch9> BucketBatch9 { get; set; }
        public DbSet<BucketBatch10> BucketBatch10 { get; set; }


        public DbSet<Bucket> Bucket { get; set; }
        public DbSet<Bucket2> Bucket2 { get; set; }
        public DbSet<Bucket3> Bucket3 { get; set; }
        public DbSet<Bucket4> Bucket4 { get; set; }
        public DbSet<Bucket5> Bucket5 { get; set; }
        public DbSet<Bucket6> Bucket6 { get; set; }
        public DbSet<Bucket7> Bucket7 { get; set; }
        public DbSet<Bucket8> Bucket8 { get; set; }
        public DbSet<Bucket9> Bucket9 { get; set; }
        public DbSet<Bucket10> Bucket10 { get; set; }

        public DbSet<BucketItem> BucketItem { get; set; }
        public DbSet<BucketItem2> BucketItem2 { get; set; }
        public DbSet<BucketItem3> BucketItem3 { get; set; }
        public DbSet<BucketItem4> BucketItem4 { get; set; }
        public DbSet<BucketItem5> BucketItem5 { get; set; }
        public DbSet<BucketItem6> BucketItem6 { get; set; }
        public DbSet<BucketItem7> BucketItem7 { get; set; }
        public DbSet<BucketItem8> BucketItem8 { get; set; }
        public DbSet<BucketItem9> BucketItem9 { get; set; }
        public DbSet<BucketItem10> BucketItem10 { get; set; }


        public DbSet<CampaignAudit2> CampaignAudit2 { get; set; }
        public DbSet<CampaignAudit3> CampaignAudit3 { get; set; }
        public DbSet<CampaignAudit4> CampaignAudit4 { get; set; }
        public DbSet<CampaignAudit5> CampaignAudit5 { get; set; }
        public DbSet<CampaignAudit6> CampaignAudit6 { get; set; }
        public DbSet<CampaignAudit7> CampaignAudit7 { get; set; }
        public DbSet<CampaignAudit8> CampaignAudit8 { get; set; }
        public DbSet<CampaignAudit9> CampaignAudit9 { get; set; }
        public DbSet<CampaignAudit10> CampaignAudit10 { get; set; }

        public DbSet<UserProfileAdvertsReceived> UserProfileAdvertsReceived { get; set; }
        public DbSet<UserProfileAdvertsReceived2> UserProfileAdvertsReceived2 { get; set; }
        public DbSet<UserProfileAdvertsReceived3> UserProfileAdvertsReceived3 { get; set; }
        public DbSet<UserProfileAdvertsReceived4> UserProfileAdvertsReceived4 { get; set; }
        public DbSet<UserProfileAdvertsReceived5> UserProfileAdvertsReceived5 { get; set; }
        public DbSet<UserProfileAdvertsReceived6> UserProfileAdvertsReceived6 { get; set; }
        public DbSet<UserProfileAdvertsReceived7> UserProfileAdvertsReceived7 { get; set; }
        public DbSet<UserProfileAdvertsReceived8> UserProfileAdvertsReceived8 { get; set; }
        public DbSet<UserProfileAdvertsReceived9> UserProfileAdvertsReceived9 { get; set; }
        public DbSet<UserProfileAdvertsReceived10> UserProfileAdvertsReceived10 { get; set; }

        public DbSet<GravityFormsTrack> GravityFormsTrack { get; set; }
        public DbSet<CampaignConfig> CampaignConfig { get; set; }

        public DbSet<ProfileMatchInformation> ProfileMatchInformation { get; set; }
        public DbSet<CountryConnectionString> CountryConnectionString { get; set; }

        public DbSet<Area> Area { get; set; }
        public DbSet<Import> Import { get; set; }
        public DbSet<Import2> Import2 { get; set; }
        public DbSet<Import3> Import3 { get; set; }
        public DbSet<Import4> Import4 { get; set; }
        public DbSet<Import5> Import5 { get; set; }
        public DbSet<Import6> Import6 { get; set; }
        public DbSet<Import7> Import7 { get; set; }
        public DbSet<Import8> Import8 { get; set; }
        public DbSet<Import9> Import9 { get; set; }
        public DbSet<Import10> Import10 { get; set; }
        public DbSet<ImportFileTrack> ImportFileTrack { get; set; }
        public DbSet<ImportUserCSV> ImportUserCSV { get; set; }
        public DbSet<UserTokenLink> UserTokenLink { get; set; }
        public DbSet<EmailVerificationCode> EmailVerificationCode { get; set; }
        public DbSet<OrganisationType> OrganisationType { get; set; }
        public DbSet<AdvertCategory> AdvertCategory { get; set; }
        public DbSet<AdvertRejection> AdvertRejection { get; set; }

        public DbSet<PreMatch> PreMatch { get; set; }
        public DbSet<PreMatch2> PreMatch2 { get; set; }
        public DbSet<PreMatch3> PreMatch3 { get; set; }
        public DbSet<PreMatch4> PreMatch4 { get; set; }
        public DbSet<PreMatch5> PreMatch5 { get; set; }
        public DbSet<PreMatch6> PreMatch6 { get; set; }
        public DbSet<PreMatch7> PreMatch7 { get; set; }
        public DbSet<PreMatch8> PreMatch8 { get; set; }
        public DbSet<PreMatch9> PreMatch9 { get; set; }
        public DbSet<PreMatch10> PreMatch10 { get; set; }
        public DbSet<CampaignBudget> CampaignBudget { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<CurrencyRate> CurrencyRate { get; set; }
        public DbSet<CampaignMetrics> CampaignMetrics { get; set; }
        public DbSet<FlotChartMetrics> FlotChartMetrics { get; set; }
        public DbSet<SparkLineMetrics> SparkLineMetrics { get; set; }
        public DbSet<SpentBucket> SpentBuckets{ get; set; }
        public DbSet<SpentBucket2> SpentBucket2 { get; set; }
        public DbSet<SpentBucket3> SpentBucket3 { get; set; }
        public DbSet<SpentBucket4> SpentBucket4 { get; set; }
        public DbSet<SpentBucket5> SpentBucket5 { get; set; }
        public DbSet<SpentBucket6> SpentBucket6 { get; set; }
        public DbSet<SpentBucket7> SpentBucket7 { get; set; }
        public DbSet<SpentBucket8> SpentBucket8 { get; set; }
        public DbSet<SpentBucket9> SpentBucket9 { get; set; }
        public DbSet<SpentBucket10> SpentBucket10 { get; set; }

        public DbSet<SpentBucketItem> SpentBucketItems { get; set; }
        public DbSet<SpentBucketItem2> SpentBucketItem2 { get; set; }
        public DbSet<SpentBucketItem3> SpentBucketItem3 { get; set; }
        public DbSet<SpentBucketItem4> SpentBucketItem4 { get; set; }
        public DbSet<SpentBucketItem5> SpentBucketItem5 { get; set; }
        public DbSet<SpentBucketItem6> SpentBucketItem6 { get; set; }
        public DbSet<SpentBucketItem7> SpentBucketItem7 { get; set; }
        public DbSet<SpentBucketItem8> SpentBucketItem8 { get; set; }
        public DbSet<SpentBucketItem9> SpentBucketItem9 { get; set; }
        public DbSet<SpentBucketItem10> SpentBucketItem10 { get; set; }

        public DbSet<SoapApiResponseCode> SoapApiResponseCode { get; set; }

        public DbSet<ProfileMatchLabel> ProfileMatchLabel { get; set; }

        public DbSet<Reward> Rewards { get; set; }

        public DbSet<UserReward> UserRewards { get; set; }
        public DbSet<SoapUploadTone> SoapUploadTone { get; set; }
        public DbSet<CopyRight> CopyRights { get; set; }
        public DbSet<OperatorFTPDetail> OperatorFTPDetails { get; set; }
        public DbSet<TIBCOResponseCode> TIBCOResponseCodes { get; set; }
        public DbSet<CampaignCreditPeriod> CampaignCreditPeriod { get; set; }
        public DbSet<LinuxServerConnectionString> LinuxServerConnectionString { get; set; }
        public DbSet<MaximumAdvertPerDay> MaximumAdvertPerDay { get; set; }
        public DbSet<UserRewardHistory> UserRewardHistory { get; set; }
        public DbSet<UserRewardCount> UserRewardCounts { get; set; }
        public DbSet<UserSMSCount> UserSMSCounts { get; set; }
        public DbSet<MpesaHistory> MpesaHistory { get; set; }
        public DbSet<OperatorMaxAdvert> OperatorMaxAdverts { get; set; }
        public DbSet<OperatorConfiguration> OperatorConfigurations { get; set; }
        public DbSet<RotateAdvert> RotateAdverts { get; set; }
        public DbSet<PromotionalUser> PromotionalUsers { get; set; }
        public DbSet<PromotionalCampaign> PromotionalCampaigns { get; set; }
        public DbSet<PromotionalAdvert> PromotionalAdverts { get; set; }
        public DbSet<ProvitionUser> ProvitionUsers { get; set; }
        public DbSet<PromotionalCampaignAudit> PromotionalCampaignAudits { get; set; }

        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }
        public DbSet<LoggedIn> LoggedIns { get; set; }



        //        public DbSet<Order2> Orders2 { get; set; }
        //        public DbSet<Product2> Products2 { get; set; }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">Entity Validation Failed - errors follow:\n +
        ///                     sb</exception>
        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                Console.WriteLine(dbUpdateException);
                throw;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (DbEntityValidationResult failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (DbValidationError error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb, ex
                    ); // Add the original exception as the innerException
            }
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        ///                 before the model has been locked down and used to initialize the context.  The default
        ///                 implementation of this method does nothing, but it can be overridden in a derived class
        ///                 such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>Typically, this method is called only once when the first instance of a derived context
        ///                 is created.  The model for that context is then cached and is for all further instances of
        ///                 the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///                 property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///                 More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///                 classes directly.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new UserProfileAdvertConfiguration());
            modelBuilder.Configurations.Add(new UserProfileAttitudeConfiguration());
            modelBuilder.Configurations.Add(new UserProfileCinemaConfiguration());
            modelBuilder.Configurations.Add(new UserProfileInternetConfiguration());
            modelBuilder.Configurations.Add(new UserProfileMobileConfiguration());
            modelBuilder.Configurations.Add(new UserProfilePressConfiguration());
            modelBuilder.Configurations.Add(new UserProfileProductsServiceConfiguration());
            modelBuilder.Configurations.Add(new UserProfileRadioConfiguration());
            modelBuilder.Configurations.Add(new UserProfileTimeSettingConfiguration());
            modelBuilder.Configurations.Add(new UserProfileTvConfiguration());
            modelBuilder.Configurations.Add(new BlockedNumberConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileConfiguration());
            modelBuilder.Configurations.Add(new CampaignAuditConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileAdvertConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileAttitudeConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileCinemaConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileDateSettingsConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileInternetConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileMobileConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfilePressConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileProductsServiceConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileRadioConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileTimeSettingConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileTvConfiguration());
            modelBuilder.Configurations.Add(new AdvertConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileUserProfileConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfileDemographicsConfiguration());
            modelBuilder.Configurations.Add(new BucketAuditConfiguration());
            modelBuilder.Configurations.Add(new BucketAuditRowConfiguration());
            modelBuilder.Configurations.Add(new SystemConfigConfiguration());
            modelBuilder.Configurations.Add(new ContactsConfiguration());
            modelBuilder.Configurations.Add(new CompanyDetailsConfiguration());
            modelBuilder.Configurations.Add(new BillingConfiguration());
            modelBuilder.Configurations.Add(new BillingDetailsConfiguration());
            modelBuilder.Configurations.Add(new PaymentMethodConfiguration());
            modelBuilder.Configurations.Add(new UsersCreditConfiguration());
            modelBuilder.Configurations.Add(new UsersCreditPaymentConfiguration());
            modelBuilder.Configurations.Add(new QuestionConfiguration());
            modelBuilder.Configurations.Add(new QuestionCommentConfiguration());
            modelBuilder.Configurations.Add(new QuestionCommentImagesConfiguration());
            modelBuilder.Configurations.Add(new QuestionImagesConfiguration());
            modelBuilder.Configurations.Add(new QuestionSubjectConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new CountryTaxConfiguration());
            modelBuilder.Configurations.Add(new UserProfilePreferenceConfiguration());
            modelBuilder.Configurations.Add(new CampaignProfilePreferenceConfiguration());
            //            modelBuilder.Configurations.Add(new OrderConfiguration());
            //            modelBuilder.Configurations.Add(new Order2Configuration());
            //            modelBuilder.Configurations.Add(new ProductConfiguration());
            //            modelBuilder.Configurations.Add(new Product2Configuration());

     

        }

        
    }
}