// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileRadioConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileRadioConfiguration.
    /// </summary>
    public class CampaignProfileRadioConfiguration : EntityTypeConfiguration<CampaignProfileRadio>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileRadioConfiguration"/> class.
        /// </summary>
        public CampaignProfileRadioConfiguration()
        {
            ToTable("CampaignProfileRadio");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.National);
            Property(u => u.Local);
            Property(u => u.Music);
            Property(u => u.Sport);
            Property(u => u.Talk);
        }
    }
}