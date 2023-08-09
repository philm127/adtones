// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignAuditConfiguration.cs" company="Noat">
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
    /// Class CampaignAuditConfiguration.
    /// </summary>
    public class CampaignAuditConfiguration : EntityTypeConfiguration<CampaignAudit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignAuditConfiguration"/> class.
        /// </summary>
        public CampaignAuditConfiguration()
        {
            ToTable("CampaignAudit");
            Property(u => u.CampaignAuditId).IsRequired();
            Property(u => u.CampaignProfileId);
            Property(u => u.UserProfileId);
            Property(u => u.BidValue);
            Property(u => u.StartTime);
            Property(u => u.EndTime);
            Property(u => u.PlayLengthTicks);
            Property(u => u.SMS);
            Property(u => u.SMSCost);
            Property(u => u.Email);
            Property(u => u.EmailCost);
            Property(u => u.TotalCost);
            Property(u => u.Status);
         
            
        }
    }
}