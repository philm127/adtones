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
    /// Interface IFlotChartMetricsRepository
    /// </summary>
    public interface IFlotChartMetricsRepository : IRepository<FlotChartMetrics>
    {
    }

    /// <summary>
    /// Class FlotChartMetricsRepository.
    /// </summary>
    public class FlotChartMetricsRepository : RepositoryBase<FlotChartMetrics>, IFlotChartMetricsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlotChartMetricsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public FlotChartMetricsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}