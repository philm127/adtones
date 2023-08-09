// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignAttitudeRepository.cs" company="">
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
    /// Interface ICampaignAttitudeRepository
    /// </summary>
    public interface ICampaignAttitudeRepository : IRepository<CampaignProfileAttitude>
    {
    }

    /// <summary>
    /// Class CampaignAttitudeRepository.
    /// </summary>
    public class CampaignAttitudeRepository : RepositoryBase<CampaignProfileAttitude>, ICampaignAttitudeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignAttitudeRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignAttitudeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}