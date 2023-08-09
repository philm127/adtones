// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileAttitudeConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileAttitudeConfiguration.
    /// </summary>
    public class CampaignProfileAttitudeConfiguration : EntityTypeConfiguration<CampaignProfileAttitude>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileAttitudeConfiguration"/> class.
        /// </summary>
        public CampaignProfileAttitudeConfiguration()
        {
            ToTable("CampaignProfileAttitude");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Fitness);
            Property(u => u.Holidays);
            Property(u => u.Environment);
            Property(u => u.GoingOut);
            Property(u => u.FinancialStabiity);
            Property(u => u.Religion);
            Property(u => u.Fashion);
            Property(u => u.Music);
        }
    }
}