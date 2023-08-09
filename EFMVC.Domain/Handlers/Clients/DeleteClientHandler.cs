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
using EFMVC.Domain.Commands.Clients;
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
    public class DeleteClientHandler : ICommandHandler<DeleteClientCommand>
    {
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAdvertHandler" /> class.
        /// </summary>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteClientHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteClientCommand command)
        {
            Client client = _clientRepository.GetById(command.Id);
            _clientRepository.Delete(client);
            var ConnString = ConnectionString.GetConnectionStringByCountryId(client.CountryId);

            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var clientData = db.Clients.Where(s => s.AdtoneServerClientId == command.Id).FirstOrDefault();
                    if(clientData != null)
                    {
                        db.Clients.Remove(clientData);
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