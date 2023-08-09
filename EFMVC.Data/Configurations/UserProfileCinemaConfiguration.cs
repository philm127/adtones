// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileCinemaConfiguration.cs" company="Noat">
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
    /// Class UserProfileCinemaConfiguration.
    /// </summary>
    public class UserProfileCinemaConfiguration : EntityTypeConfiguration<UserProfileCinema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileCinemaConfiguration"/> class.
        /// </summary>
        public UserProfileCinemaConfiguration()
        {
            ToTable("UserProfileCinema");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.Cinema);
        }
    }
}