// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileTimeSettingConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileTimeSettingConfiguration.
    /// </summary>
    public class CampaignProfileTimeSettingConfiguration : EntityTypeConfiguration<CampaignProfileTimeSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileTimeSettingConfiguration" /> class.
        /// </summary>
        public CampaignProfileTimeSettingConfiguration()
        {
            ToTable("CampaignProfileTimeSetting");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Monday).HasMaxLength(200);
            Property(u => u.Tuesday).HasMaxLength(200);
            Property(u => u.Wednesday).HasMaxLength(200);
            Property(u => u.Thursday).HasMaxLength(200);
            Property(u => u.Friday).HasMaxLength(200);
            Property(u => u.Saturday).HasMaxLength(200);
            Property(u => u.Sunday).HasMaxLength(200);
        }
    }
}