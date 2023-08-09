// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignRepository.cs" company="">
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
    /// Interface ICampaignRepository
    /// </summary>
    public interface ICampaignRepository : IRepository<CampaignProfile>
    {
    }

    /// <summary>
    /// Class CampaignRepository.
    /// </summary>
    public class CampaignRepository : RepositoryBase<CampaignProfile>, ICampaignRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}