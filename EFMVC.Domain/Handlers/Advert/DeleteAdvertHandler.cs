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
using EFMVC.Model;
using System.Linq;
/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class DeleteAdvertHandler.
    /// </summary>
    public class DeleteAdvertHandler : ICommandHandler<DeleteAdvertCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAdvertHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteAdvertHandler(IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
        {
            _advertRepository = advertRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteAdvertCommand command)
        {
            Model.Advert advert = _advertRepository.GetById(command.Id);
            _advertRepository.Delete(advert);
            var ConnString = ConnectionString.GetConnectionStringByCountryId(advert.CountryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var advertDetail = db.Adverts.Where(s => s.AdtoneServerAdvertId == command.Id).FirstOrDefault();
                    if(advertDetail != null)
                    {
                        db.Adverts.Remove(advertDetail);
                        db.SaveChanges();
                    }                 
                }

            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}