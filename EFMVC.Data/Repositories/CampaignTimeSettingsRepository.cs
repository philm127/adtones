// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignTimeSettingsRepository.cs" company="">
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
    /// Interface ICampaignTimeSettingsRepository
    /// </summary>
    public interface ICampaignTimeSettingsRepository : IRepository<CampaignProfileTimeSetting>
    {
    }

    /// <summary>
    /// Class CampaignTimeSettingsRepository.
    /// </summary>
    public class CampaignTimeSettingsRepository : RepositoryBase<CampaignProfileTimeSetting>,
                                                  ICampaignTimeSettingsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignTimeSettingsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignTimeSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}