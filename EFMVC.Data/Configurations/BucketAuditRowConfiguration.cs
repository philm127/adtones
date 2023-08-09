// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="BucketAuditRowConfiguration.cs" company="Noat">
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
    /// Class BucketAuditRowConfiguration.
    /// </summary>
    public class BucketAuditRowConfiguration : EntityTypeConfiguration<BucketAuditRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BucketAuditRowConfiguration"/> class.
        /// </summary>
        public BucketAuditRowConfiguration()
        {
            ToTable("BucketAuditRows");
            HasKey(x => x.Id);
            Property(x => x.Id).IsRequired();
            Property(x => x.BucketAuditId);
            Property(x => x.State);
            Property(x => x.MediaUrl).HasMaxLength(500);
            Property(x => x.BidValue).HasMaxLength(250);
            Property(x => x.Dtmf).HasMaxLength(250);
            Property(x => x.Start).HasMaxLength(250);
            Property(x => x.End).HasMaxLength(250);
            Property(x => x.CampaignProfileId);
            Property(x => x.Sms);
            Property(x => x.Email);
            Property(x => x.Processed);
        }
    }
}