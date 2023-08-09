// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileCreditsReceivedConfiguration.cs" company="Noat">
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
    /// Class UserProfileCreditsReceivedConfiguration.
    /// </summary>
    public class UserProfileCreditsReceivedConfiguration : EntityTypeConfiguration<UserProfileCreditsReceived>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileCreditsReceivedConfiguration"/> class.
        /// </summary>
        public UserProfileCreditsReceivedConfiguration()
        {
            ToTable("UserProfileCreditsReceived");
            HasKey(x => x.Id);
            Property(x => x.Id).IsRequired();
            Property(x => x.UserProfileId);
            Property(x => x.TotalCredits).HasMaxLength(10);
            Property(x => x.LastMonthCredits).HasMaxLength(10);
            Property(x => x.CurrentMonthCredits).HasMaxLength(10);
        }
    }
}