// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileAttitudeConfiguration.cs" company="Noat">
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
    /// Class UserProfileAttitudeConfiguration.
    /// </summary>
    public class UserProfileAttitudeConfiguration : EntityTypeConfiguration<UserProfileAttitude>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileAttitudeConfiguration"/> class.
        /// </summary>
        public UserProfileAttitudeConfiguration()
        {
            ToTable("UserProfileAttitude");
            Property(u => u.UserProfileId).IsRequired();
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