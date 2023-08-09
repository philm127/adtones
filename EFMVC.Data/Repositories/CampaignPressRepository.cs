// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CampaignPressRepository.cs" company="">
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
    /// Interface ICampaignPressRepository
    /// </summary>
    public interface ICampaignPressRepository : IRepository<CampaignProfilePress>
    {
    }

    /// <summary>
    /// Class PressCampaignRepository.
    /// </summary>
    public class PressCampaignRepository : RepositoryBase<CampaignProfilePress>, ICampaignPressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PressCampaignRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public PressCampaignRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}