// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileConfiguration.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

/// <summary>
/// The Configurations namespace.
/// </summary>

namespace EFMVC.Data.Configurations
{
    /// <summary>
    /// Class CampaignProfileConfiguration.
    /// </summary>
    public class CampaignProfileConfiguration : EntityTypeConfiguration<CampaignProfile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileConfiguration"/> class.
        /// </summary>
        public CampaignProfileConfiguration()
        {
            ToTable("CampaignProfile");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.UserId);
            Property(u => u.ClientId);
            Property(u => u.CampaignDescription);
            Property(u => u.CampaignName);
            Property(u => u.AvailableCredit);
            Property(u => u.TotalBudget);
            Property(u => u.MaxDailyBudget);
            Property(u => u.MaxBid);
            Property(u => u.MaxMonthBudget);
            Property(u => u.MaxWeeklyBudget);
            Property(u => u.MaxHourlyBudget);
            Property(u => u.TotalCredit);
            Property(u => u.SpendToDate);
            Property(u => u.PlaysCurrentMonth);
            Property(u => u.PlaysLastMonth);
            Property(u => u.PlaysToDate);
            Property(u => u.CancelledCurrentMonth);
            Property(u => u.CancelledLastMonth);
            Property(u => u.CancelledToDate);
            Property(u => u.SmsCurrentMonth);
            Property(u => u.SmsLastMonth);
            Property(u => u.SmsToDate);
            Property(u => u.EmailToDate);
            Property(u => u.EmailsCurrentMonth);
            Property(u => u.EmailsLastMonth);
            Property(u => u.Active);
            Property(u => u.NumberOfPlays);
            Property(u => u.AverageDailyPlays);
            Property(u => u.SmsRequests);
            Property(u => u.EmailsDelievered);

            Property(u => u.EmailSenderAddress);
            Property(u => u.EmailSubject);
            Property(u => u.EmailBody).HasColumnType("text");
            Property(u => u.SMSFileLocation);
            Property(u => u.EmailFileLocation);
            Property(u => u.SmsOriginator);
            Property(u => u.SmsBody).HasMaxLength(1000);

            Property(u => u.CreatedDateTime);
            Property(u => u.UpdatedDateTime);

            Property(u => u.StartDate);
            Property(u => u.EndDate);

            HasKey(u => u.CampaignProfileId);

            HasRequired(p => p.User)
                        .WithMany(c => c.CampaignProfiles)
                        .HasForeignKey(p => p.UserId);

            HasRequired(p => p.Client)
            .WithMany(c => c.CampaignProfiles)
            .HasForeignKey(p => p.ClientId);
        }
    }
}