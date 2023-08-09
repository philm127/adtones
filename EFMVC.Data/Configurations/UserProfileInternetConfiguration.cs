// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileInternetConfiguration.cs" company="Noat">
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
    /// Class UserProfileInternetConfiguration.
    /// </summary>
    public class UserProfileInternetConfiguration : EntityTypeConfiguration<UserProfileInternet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileInternetConfiguration"/> class.
        /// </summary>
        public UserProfileInternetConfiguration()
        {
            ToTable("UserProfileInternet");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.SocialNetworking);
            Property(u => u.Video);
            Property(u => u.Research);
            Property(u => u.Auctions);
            Property(u => u.Shopping);
        }
    }
}