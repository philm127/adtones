// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteAdvertHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model.Entities;
using System.Linq;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class DeleteAdvertHandler.
    /// </summary>
    public class DeleteAreaHandler : ICommandHandler<DeleteAreaCommand>
    {
        /// <summary>
        /// The _Area repository
        /// </summary>
        private readonly IAreaRepository _areaRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAdvertHandler" /> class.
        /// </summary>
        /// <param name="AreaRepository">The Area repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteAreaHandler(IAreaRepository areaRepository, IUnitOfWork unitOfWork)
        {
            _areaRepository = areaRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteAreaCommand command)
        {
            Area area = _areaRepository.GetById(command.AreaId);            
            var ConnString = ConnectionString.GetConnectionString(area.CountryId);
            if(!string.IsNullOrEmpty(ConnString))
            {
                EFMVCDataContex db = new EFMVCDataContex(ConnString);
                var areaResult = db.Area.Where(s=>s.AreaId == command.AreaId).FirstOrDefault();
                if(areaResult != null)
                {
                    db.Area.Remove(areaResult);
                    db.SaveChanges();
                }
            }
            _areaRepository.Delete(area);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}