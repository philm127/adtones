// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ProductConfiguration.cs" company="Noat">
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
    /// Class ProductConfiguration.
    /// </summary>
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductConfiguration"/> class.
        /// </summary>
        public ProductConfiguration()
        {
            ToTable("Products");

            HasKey(x => x.ProductId);
            Property(x => x.ProductId).IsRequired();

            Property(x => x.ProductName);
        }
    }
}