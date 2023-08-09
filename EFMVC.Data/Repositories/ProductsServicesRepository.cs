// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ProductsServicesRepository.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.Data.Infrastructure;
using EFMVC.Model;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IProductsServicesRepository
    /// </summary>
    public interface IProductsServicesRepository : IRepository<UserProfileProductsService>
    {
    }

    /// <summary>
    /// Class ProductsServicesRepository.
    /// </summary>
    public class ProductsServicesRepository : RepositoryBase<UserProfileProductsService>, IProductsServicesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsServicesRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ProductsServicesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}