// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignProfileRepository.cs" company="">
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
    /// Interface ICampaignProfileRepository
    /// </summary>
    public interface ICampaignProfileRepository : IRepository<CampaignProfile>
    {
    }

    /// <summary>
    /// Class CampaignProfileRepository.
    /// </summary>
    public class CampaignProfileRepository : RepositoryBase<CampaignProfile>, ICampaignProfileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignProfileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}