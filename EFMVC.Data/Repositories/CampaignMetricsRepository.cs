// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AdvertRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface ICampaignMetricsRepository
    /// </summary>
    public interface ICampaignMetricsRepository : IRepository<CampaignMetrics>
    {
    }

    /// <summary>
    /// Class CampaignMetricsRepository.
    /// </summary>
    public class CampaignMetricsRepository : RepositoryBase<CampaignMetrics>, ICampaignMetricsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignMetricsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignMetricsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}