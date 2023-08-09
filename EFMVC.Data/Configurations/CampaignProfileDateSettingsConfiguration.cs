// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileDateSettingsConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileDateSettingsConfiguration.
    /// </summary>
    public class CampaignProfileDateSettingsConfiguration : EntityTypeConfiguration<CampaignProfileDateSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileDateSettingsConfiguration"/> class.
        /// </summary>
        public CampaignProfileDateSettingsConfiguration()
        {
            ToTable("CampaignProfileDateSettings");
            Property(a => a.CampaignDateSettingsId).IsRequired();
            Property(a => a.CampaignProfileId).IsRequired();
            Property(a => a.CampaignDate);
            Property(a => a.Active);
        }
    }
}