// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignMobileRepository.cs" company="">
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
    /// Interface ICampaignMobileRepository
    /// </summary>
    public interface ICampaignMobileRepository : IRepository<CampaignProfileMobile>
    {
    }

    /// <summary>
    /// Class CampaignMobileRepository.
    /// </summary>
    public class CampaignMobileRepository : RepositoryBase<CampaignProfileMobile>, ICampaignMobileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignMobileRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public CampaignMobileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}