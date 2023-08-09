// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-09-2013
// ***********************************************************************
// <copyright file="BlockedNumberConfiguration.cs" company="">
//     Copyright (c) . All rights reserved.
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
    /// Class BlockedNumberConfiguration.
    /// </summary>
    public class BlockedNumberConfiguration : EntityTypeConfiguration<BlockedNumber>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedNumberConfiguration"/> class.
        /// </summary>
        public BlockedNumberConfiguration()
        {
            ToTable("BlockedNumbers");
            Property(b => b.Id).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.TelephoneNumber).HasMaxLength(50);
            Property(b => b.Active).IsRequired();
        }
    }
}