// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertsReceivedConfiguration.cs" company="Noat">
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
    /// Class UserProfileAdvertsReceivedConfiguration.
    /// </summary>
    public class UserProfileAdvertsReceivedConfiguration : EntityTypeConfiguration<UserProfileAdvertsReceived>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileAdvertsReceivedConfiguration"/> class.
        /// </summary>
        public UserProfileAdvertsReceivedConfiguration()
        {
            ToTable("UserProfileAdvertsReceived");
            HasKey(x => x.Id);
            Property(x => x.Id).IsRequired();
            Property(x => x.UserProfileId);
            Property(x => x.AdvertName).HasMaxLength(250);
            Property(x => x.AdvertRef).HasMaxLength(50);
            Property(x => x.Brand).HasMaxLength(250);
            Property(x => x.FileName).HasMaxLength(250);
            Property(x => x.DateTimePlayed);
            Property(x => x.CreditsReceived);
        }
    }
}