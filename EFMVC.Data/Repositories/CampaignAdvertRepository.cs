// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignAdvertRepository.cs" company="">
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
    /// Interface ICampaignAdvertRepository
    /// </summary>
    public interface ICampaignAdvertRepository : IRepository<CampaignAdvert>
    {
    }

    /// <summary>
    /// Class CampaignAdvertRepository.
    /// </summary>
    public class CampaignAdvertRepository : RepositoryBase<CampaignAdvert>, ICampaignAdvertRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignAdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignAdvertRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}