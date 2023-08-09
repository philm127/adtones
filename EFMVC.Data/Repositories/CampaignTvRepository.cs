// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignTvRepository.cs" company="Noat">
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
    /// Interface ICampaignTvRepository
    /// </summary>
    public interface ICampaignTvRepository : IRepository<CampaignProfileTv>
    {
    }

    /// <summary>
    /// Class CampaignTvRepository.
    /// </summary>
    public class CampaignTvRepository : RepositoryBase<CampaignProfileTv>, ICampaignTvRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignTvRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignTvRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}