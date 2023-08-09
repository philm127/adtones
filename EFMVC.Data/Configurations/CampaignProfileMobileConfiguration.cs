// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileMobileConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileMobileConfiguration.
    /// </summary>
    public class CampaignProfileMobileConfiguration : EntityTypeConfiguration<CampaignProfileMobile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileMobileConfiguration"/> class.
        /// </summary>
        public CampaignProfileMobileConfiguration()
        {
            ToTable("CampaignProfileMobile");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.ContractType);
            Property(u => u.Spend);
        }
    }
}