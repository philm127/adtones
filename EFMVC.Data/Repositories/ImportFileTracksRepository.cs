// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ImportFileTracksRepository.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;

/// <summary>
/// The Repositories namespace.
/// </summary>

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Class ImportFileTracksRepository.
    /// </summary>
    public class ImportFileTracksRepository : RepositoryBase<ImportFileTrack>, IImportFileTracksRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFileTracksRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ImportFileTracksRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    /// <summary>
    /// Interface IImportFileTracksRepository
    /// </summary>
    public interface IImportFileTracksRepository : IRepository<ImportFileTrack>
    {
    }
}