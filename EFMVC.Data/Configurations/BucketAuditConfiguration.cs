// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="BucketAuditConfiguration.cs" company="Noat">
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
    /// Class BucketAuditConfiguration.
    /// </summary>
    public class BucketAuditConfiguration : EntityTypeConfiguration<BucketAudit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BucketAuditConfiguration"/> class.
        /// </summary>
        public BucketAuditConfiguration()
        {
            ToTable("BucketAudits");
            HasKey(x => x.Id);
            Property(x => x.Id).IsRequired();
            Property(x => x.BucketId).IsRequired();
            Property(x => x.MSISDN).HasMaxLength(50);
            Property(x => x.BucketPeriodStart).HasMaxLength(250);
            Property(x => x.TargetDeliveryServer).HasMaxLength(250);
            Property(x => x.Processed);
        }
    }
}