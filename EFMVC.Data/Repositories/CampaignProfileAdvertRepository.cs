// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignProfileAdvertRepository.cs" company="">
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
    /// Interface ICampaignProfileAdvertRepository
    /// </summary>
    public interface ICampaignProfileAdvertRepository : IRepository<CampaignProfileAdvert>
    {
    }

    /// <summary>
    /// Class CampaignProfileAdvertRepository.
    /// </summary>
    public class CampaignProfileAdvertRepository : RepositoryBase<CampaignProfileAdvert>,
                                                   ICampaignProfileAdvertRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileAdvertRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignProfileAdvertRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}