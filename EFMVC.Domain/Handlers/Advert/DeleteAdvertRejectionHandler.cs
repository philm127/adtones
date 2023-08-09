// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteAdvertRejectionHandler.cs" company="Noat">
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
    /// Class DeleteAdvertRejectionHandler.
    /// </summary>
    public class DeleteAdvertRejectionHandler : ICommandHandler<DeleteAdvertRejectionCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRejectionRepository _advertRejectionRepository;
        private readonly IAdvertRepository _advertRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAdvertRejectionHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteAdvertRejectionHandler(IAdvertRejectionRepository advertRejectionRepository, IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
        {
            _advertRejectionRepository = advertRejectionRepository;
            _advertRepository = advertRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteAdvertRejectionCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteAdvertRejectionCommand command)
        {
            AdvertRejection advertRejection = _advertRejectionRepository.GetById(command.Id);

            var advert = _advertRepository.GetById((long)advertRejection.AdvertId);
            var ConnString = ConnectionString.GetConnectionStringByCountryId(advert.CountryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var adverttRejectionDetail = db.AdvertRejection.Where(s => s.AdtoneServerAdvertRejectionId == command.Id).FirstOrDefault();
                    if (adverttRejectionDetail != null)
                    {
                        db.AdvertRejection.Remove(adverttRejectionDetail);
                        db.SaveChanges();
                    }
                }
            }
            _advertRejectionRepository.Delete(advertRejection);
           
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}