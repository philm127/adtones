// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignProductsServicesRepository.cs" company="">
//     Copyright (c) . All rights reserved.
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
    /// Interface ICampaignProductsServicesRepository
    /// </summary>
    public interface ICampaignProductsServicesRepository : IRepository<CampaignProfileProductsService>
    {
    }

    /// <summary>
    /// Class CampaignProductsServicesRepository.
    /// </summary>
    public class CampaignProductsServicesRepository : RepositoryBase<CampaignProfileProductsService>,
                                                      ICampaignProductsServicesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProductsServicesRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignProductsServicesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}