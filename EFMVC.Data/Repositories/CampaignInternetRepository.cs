// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignInternetRepository.cs" company="">
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
    /// Interface ICampaignInternetRepository
    /// </summary>
    public interface ICampaignInternetRepository : IRepository<CampaignProfileInternet>
    {
    }

    /// <summary>
    /// Class CampaignInternetRepository.
    /// </summary>
    public class CampaignInternetRepository : RepositoryBase<CampaignProfileInternet>, ICampaignInternetRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignInternetRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignInternetRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}