// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="RadioRepository.cs" company="Noat">
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
    /// Interface IRadioRepository
    /// </summary>
    public interface IRadioRepository : IRepository<UserProfileRadio>
    {
    }

    /// <summary>
    /// Class RadioRepository.
    /// </summary>
    public class RadioRepository : RepositoryBase<UserProfileRadio>, IRadioRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public RadioRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}