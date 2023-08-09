// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignCinemaRepository.cs" company="">
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
    /// Interface ICampaignCinemaRepository
    /// </summary>
    public interface ICampaignCinemaRepository : IRepository<CampaignProfileCinema>
    {
    }

    /// <summary>
    /// Class CampaignCinemaRepository.
    /// </summary>
    public class CampaignCinemaRepository : RepositoryBase<CampaignProfileCinema>, ICampaignCinemaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignCinemaRepository" /> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignCinemaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}