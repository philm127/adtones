// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="TimeSettingsRepository.cs" company="Noat">
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
    /// Interface ITimeSettingsRepository
    /// </summary>
    public interface ITimeSettingsRepository : IRepository<UserProfileTimeSetting>
    {
    }

    /// <summary>
    /// Class TimeSettingsRepository.
    /// </summary>
    public class TimeSettingsRepository : RepositoryBase<UserProfileTimeSetting>, ITimeSettingsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSettingsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public TimeSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}