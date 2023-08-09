// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="OrderConfiguration.cs" company="Noat">
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
    /// Class OrderConfiguration.
    /// </summary>
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderConfiguration"/> class.
        /// </summary>
        public OrderConfiguration()
        {
            ToTable("Orders");

            HasKey(x => x.OrderId);
            Property(x => x.OrderId);
        }
    }
}